using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.DeviceAutomation.NativeApps.USB;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows.Forms;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.PrintFromUsb
{
    [ToolboxItem(false)]
    public partial class PrintFromUsbExecControl : UserControl, IPluginExecutionEngine
    {
        private List<IDeviceInfo> _deviceAssets;
        private PluginExecutionData _executionData;
        private DeviceWorkflowLogger _workflowLogger;
        private PrintFromUsbActivityData _activityData;

        public PrintFromUsbExecControl()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Execute the task of the PrintFromUsb activity.
        /// </summary>
        /// <param name="executionData"></param>
        /// <returns></returns>
        public PluginExecutionResult Execute(PluginExecutionData executionData)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed);

            var acquireTimeout = TimeSpan.FromMinutes(10);
            var holdTimeout = TimeSpan.FromMinutes(10);
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
                IDeviceInfo deviceInfo = (selectedToken as AssetLockToken).AssetInfo as IDeviceInfo;
                if (!deviceInfo.Attributes.HasFlag(AssetAttributes.Printer))
                {
                    result = new PluginExecutionResult(PluginResult.Skipped, "Device does not have print capability.");
                    return;
                }

                UpdateStatus($"Using device {deviceInfo.AssetId} ({deviceInfo.Address})");
                ExecutionServices.DataLogger.Submit(new ActivityExecutionAssetUsageLog(executionData, deviceInfo));
                using (IDevice device = IDeviceCreate(deviceInfo))
                {
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
            catch (Exception e)
            {
                Framework.Logger.LogError($"Error creating device: {e.ToString()}");
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

                _activityData = _executionData.GetMetadata<PrintFromUsbActivityData>();

                IUsbApp usbApp = UsbAppFactory.Create(device);
                IAuthenticator auth = AuthenticatorFactory.Create(device, _executionData.Credential, AuthenticationProvider.Auto);

                usbApp.WorkflowLogger = auth.WorkflowLogger = _workflowLogger;

                var preparationManager = DevicePreparationManagerFactory.Create(device);
                preparationManager.InitializeDevice(true);
                preparationManager.WorkflowLogger = _workflowLogger;

                // need to add the ability for user to set eager or lazy authentication
                //AuthenticationMode am = (_data.ApplicationAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;

                usbApp.Pacekeeper = auth.Pacekeeper = new Pacekeeper(TimeSpan.FromSeconds(2));
                _workflowLogger.RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                usbApp.LaunchPrintFromUsb(auth, AuthenticationMode.Lazy);
                UpdateStatus("The Print From USB app is launched");

                usbApp.SelectUsbPrint(_activityData.UsbName);
                UpdateStatus("The USB is selected");

                usbApp.SelectFile();
                UpdateStatus("File to Print is selected");

                if (usbApp.ExecutePrintJob())
                {
                    UpdateStatus("The selected file is printed");
                    result = new PluginExecutionResult(PluginResult.Passed);
                }

                preparationManager.NavigateHome();
                if (preparationManager.SignOutRequired())
                {
                    preparationManager.SignOut();
                }
                _workflowLogger.RecordEvent(DeviceWorkflowMarker.ActivityEnd);
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
        private void GatherTriageData(IDevice device, string reason)
        {
            if (device != null)
            {
                ITriage triage = TriageFactory.Create(device, _executionData);
                triage.CollectTriageData(reason);
                triage.Submit();
            }
        }
    }
}
