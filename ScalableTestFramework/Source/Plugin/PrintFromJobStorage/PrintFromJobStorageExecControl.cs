using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.DeviceAutomation.NativeApps.JobStorage;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using HP.ScalableTest.Framework.Data;
using static HP.ScalableTest.Framework.Logger;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.PrintFromJobStorage
{
    /// <summary>
    /// Execution control for the Print From JobStorage plugin.
    /// </summary>
    [ToolboxItem(false)]
    public partial class PrintFromJobStorageExecControl : UserControl, IPluginExecutionEngine
    {
        private List<IDeviceInfo> _deviceAssets;
        private PluginExecutionData _executionData;
        private DeviceWorkflowLogger _workflowLogger;
        private PrintFromJobStorageActivityData _activityData;
        private IDeviceInfo _deviceInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintFromJobStorageExecControl"/> class.
        /// </summary>
        public PrintFromJobStorageExecControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Executes this plugin's workflow using the specified <see cref="T:HP.ScalableTest.Framework.Plugin.PluginExecutionData" />.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <returns>A <see cref="T:HP.ScalableTest.Framework.Plugin.PluginExecutionResult" /> indicating the outcome of the execution.</returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Passed);
            var acquireTimeout = TimeSpan.FromMinutes(5);
            var holdTimeout = TimeSpan.FromMinutes(5);
            _executionData = executionData;
            _deviceAssets = executionData.Assets.OfType<IDeviceInfo>().ToList();

            //If there is no device to run
            if (_deviceAssets.Count() == 0)
            {
                //Skip When there are no device to execute on.
                return new PluginExecutionResult(PluginResult.Skipped, "No Asset available to run.");
            }

            _workflowLogger = new DeviceWorkflowLogger(executionData);
            UpdateStatus("Starting task engine");
            List<AssetLockToken> tokens = _deviceAssets.Select(n => new AssetLockToken(n, acquireTimeout, holdTimeout)).ToList();
            _workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockBegin);
            ExecutionServices.CriticalSection.Run(tokens, selectedToken =>
            {
                _deviceInfo = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;
                if (!_deviceInfo.Attributes.HasFlag(AssetAttributes.Printer))
                {
                    result = new PluginExecutionResult(PluginResult.Skipped, "Device does not have print capability.");
                    return;
                }
                UpdateStatus($"Using device {_deviceInfo.AssetId} ({_deviceInfo.Address})");
                using (IDevice device = IDeviceCreate(_deviceInfo)) 
                {
                    //Activity Asset Usage is required for triage data
                    ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(_executionData, _deviceInfo));

                    var retryManager = new PluginRetryManager(executionData, UpdateStatus);
                    result = retryManager.Run(() => RunApp(device));
                }
            });

            _workflowLogger.RecordEvent(DeviceWorkflowMarker.DeviceLockEnd);

            return result;
        }

        private IDevice IDeviceCreate(IDeviceInfo d)
        {
            try
            {
                return DeviceConstructor.Create(d);
            }
            catch(Exception e)
            {
                LogError($"Error creating device: {e.ToString()}");
                ExecutionServices.SessionRuntime.ReportAssetError(d);

                throw;
            }
            
        }

        /// <summary>
        /// Updates the status text in the execution control display.
        /// </summary>
        /// <param name="statusMsg"></param>
        protected virtual void UpdateStatus(string statusMsg)
        {
            status_RichTextBox.InvokeIfRequired(c =>
            {
                ExecutionServices.SystemTrace.LogNotice("Status=" + statusMsg);
                c.AppendText($"{DateTime.Now.ToString("yyyy/MM/dd HH:mm:ss.fff")}  {statusMsg}\n");
                c.Select(c.Text.Length, 0);
                c.ScrollToCaret();
            });
        }

        private PluginExecutionResult RunApp(IDevice device)
        {
            try
            {
                PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed);

                _activityData = _executionData.GetMetadata<PrintFromJobStorageActivityData>();

                IJobStoragePrintApp jobStorageApp = JobStoragePrintAppFactory.Create(device);
                IAuthenticator authenticator = GetAuthenticator(_activityData.AuthProvider, device);

                AuthenticationMode am = (_activityData.ApplicationAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

                var preparationManager = DevicePreparationManagerFactory.Create(device);
                preparationManager.InitializeDevice(true);

                jobStorageApp.Pacekeeper = authenticator.Pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(2));
                jobStorageApp.Launch(authenticator, am);
                UpdateStatus("The Print From Job Storage app is launched");

                jobStorageApp.SelectFolder(_activityData.FolderName);
                UpdateStatus($"The Selected Folder: '{_activityData.FolderName}'");

                if (_activityData.PrintAll)
                {
                    bool allJobsSelected;
                    allJobsSelected = jobStorageApp.SelectAllJobs(_activityData.Pin, _activityData.FolderName);
                    if (allJobsSelected)
                    {
                        jobStorageApp.ExecutePrintJob();
                        UpdateStatus("All jobs are selected and printed");
                        if (_activityData.DeleteJobAfterPrint)
                        {
                            try
                            {
                                jobStorageApp.DeletePrintedJob();
                                UpdateStatus("All Selected Jobs were deleted");
                            }
                            catch (JobStorageDeleteJobExeception ex)
                            {
                                string message = $"The selected Job was not deleted. {ex.ToString()}";
                                LogDebug(message);
                                UpdateStatus(message);
                            }
                        }
                        result = new PluginExecutionResult(PluginResult.Passed);
                    }
                }
                else
                {
                    jobStorageApp.SelectFirstJob(_activityData.Pin, _activityData.NumberOfCopies, _activityData.FolderName);
                    UpdateStatus("The first job is selected");
                    jobStorageApp.ExecutePrintJob();
                    UpdateStatus("The first job is printed");
                    if (_activityData.DeleteJobAfterPrint)
                    {
                        try
                        {
                            jobStorageApp.DeletePrintedJob();
                            UpdateStatus("The Selected job was deleted");
                        }
                        catch (JobStorageDeleteJobExeception ex)
                        {
                            string message = $"The selected Job was not deleted. {ex.ToString()}";
                            LogDebug(message);
                            UpdateStatus(message);
                        }
                    }
                    result = new PluginExecutionResult(PluginResult.Passed);
                }
                preparationManager.Reset();
                return result;
            }
            catch (DeviceCommunicationException ex)
            {
                GatherTriageData(device, ex.ToString());
                return new PluginExecutionResult(PluginResult.Failed, ex.Message, "Device communication error.");
            }
            catch (DeviceInvalidOperationException ex)
            {
                GatherTriageData(device, ex.ToString());
                return new PluginExecutionResult(PluginResult.Failed, ex.Message, "Device automation error.");
            }
            catch (DeviceWorkflowException ex)
            {
                GatherTriageData(device, ex.ToString());
                return new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error.");
            }
            catch (Exception ex)
            {
                GatherTriageData(device, ex.ToString());
                throw;
            }
        }

        /// <summary>
        /// Initializes the authenticator that will be used in the run, including setting up the badgebox if needed.
        /// </summary>
        /// <param name="provider">The authentication provider to create</param>
        /// <param name="device">The device that will be used for the run.</param>        
        private IAuthenticator GetAuthenticator(AuthenticationProvider provider, IDevice device)
        {
            IAuthenticator authenticator = AuthenticatorFactory.Create(_deviceInfo.AssetId, device, provider, _executionData);
            authenticator.WorkflowLogger = _workflowLogger;
            return authenticator;
        }

        private void GatherTriageData(IDevice device, string reason)
        {
            if (device != null)
            {
                ITriage triage = TriageFactory.Create(device, _executionData);
                triage.CollectTriageData(reason);
                triage.Submit();
            }
            else
            {
                LogDebug("Device is null - cannot gather triage data.");
            }
        }
    }
}
