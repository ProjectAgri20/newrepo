using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Enums;
using HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.Utility;
using System;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.CloudConnector
{
    public class CloudConnectorPrintManager : LinkPrintActivityManager
    {
        private readonly CloudConnectorActivityData _data;
        CloudConnectorApp _cloudApp = null;
        protected override string LinkJobType => ConnectorJobType.Print.GetDescription();

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudConnectorPrintManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="printOptions">The scan options.</param>
        public CloudConnectorPrintManager(PluginExecutionData executionData, LinkPrintOptions printOptions, LockTimeoutData lockTimeoutData)
                    : base(executionData, printOptions, lockTimeoutData)
        {
            ExecutionData = executionData;
            _data = executionData.GetMetadata<CloudConnectorActivityData>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CloudConnectorPrintManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="printOptions">The print options.</param>
        /// <param name="serverName">Name of the server.</param>
        public CloudConnectorPrintManager(PluginExecutionData executionData, LinkPrintOptions printOptions, LockTimeoutData lockTimeoutData, string serverName)
                    : base(executionData, printOptions, lockTimeoutData, serverName)
        {
            ExecutionData = executionData;
            _data = executionData.GetMetadata<CloudConnectorActivityData>();
        }

        /// <summary>
        /// Sets up the cloud print job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }
            
            _cloudApp = new CloudConnectorApp(_data.CloudAppType, device);

            _cloudApp.WorkflowLogger = WorkflowLogger;
            _cloudApp.ActivityStatusChanged += LinkAppsActivityStatusChanged;

            string[] filename = _data.FilePath.Split('/');
            ConnectorLog.FilePrefix = filename[filename.Length - 1];
            ConnectorLog.FilePath = _data.FilePath.ToLowerInvariant();
            ConnectorLog.LoginID = _data.ID;

            UpdateStatus($"Starting App Launch :: {_data.CloudAppType.GetDescription()}");
            _cloudApp.Launch(_data.SIO);

            UpdateStatus($"Starting Log in :: {_data.CloudAppType.GetDescription()}");
            if (!_data.SIO.Equals(SIOMethod.SIOWithoutIDPWD))
            {
                _cloudApp.Login(_data.CloudAppType, _data.ID, _data.PWD);
            }
            else
            {
                UpdateStatus($"Skip Login");
            }
            
            UpdateStatus($"Starting navigate to destination :: {_data.CloudAppType.GetDescription()}");
            if (_data.CloudAppType.Equals(ConnectorName.SharePoint))
            {
                _cloudApp.SelectSharePointSite(_data.SharePointSite);
                _cloudApp.NavigateToDestination(_data.FilePath);

                UpdateStatus($"JobType is :: {_data.CloudJobType}");

                if (_data.CloudJobType.Equals(ConnectorJobType.MultiplePrint.GetDescription()))
                {
                    _cloudApp.MultiplefileSelectionForSharePoint(_data.FileList);
                }
            }
            else
            {
                _cloudApp.NavigateToDestination(_data.FilePath);

                UpdateStatus($"JobType is :: {_data.CloudJobType}");

                if (_data.CloudJobType.Equals(ConnectorJobType.MultiplePrint.GetDescription()))
                {
                    _cloudApp.MultiplefileSelection(_data.FileList);
                }
            }

            UpdateStatus($"Starting selection options :: {_data.CloudAppType.GetDescription()}");
            _cloudApp.SelectJobForSetOptions(EnumUtil.GetByDescription<ConnectorJobType>(LinkJobType));

            try
            {
                SetOptions(_cloudApp.PrintOptionManager, _data.CloudPrintOptions);
            }
            catch (DeviceWorkflowException ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to set options: {ex.Message} :: {_data.CloudAppType.GetDescription()}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SelectOptions.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Finish up the Cloud Connector print job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");

            CloudConnectorApp.JobExecutionOptions options = new CloudConnectorApp.JobExecutionOptions();
            options.JobType = EnumUtil.GetByDescription<ConnectorJobType>(LinkJobType);
            options.PageCount = _data.CloudPrintOptions.PageCount;

            if ((_data.FileList != null) && (_data.FileList.Count > 0))
            {            
                options.DocumentCount = _data.FileList.Count;
            }
            else
            {            
                options.DocumentCount = 1;
            }
            
            UpdateStatus($"Starting Execution Print Job :: {_data.CloudAppType.GetDescription()}");
            _cloudApp.ExecutionJob(options);
            result = new PluginExecutionResult(PluginResult.Passed);                
            return result;            
        }

        protected override PluginExecutionResult ExecuteLinkPrint(IDevice device, IDeviceInfo deviceInfo)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Automation Failure", "Device workflow error.");
            try
            {
                // Make sure the device is in a good state
                var devicePrepManager = DevicePreparationManagerFactory.Create(device);
                devicePrepManager.WorkflowLogger = WorkflowLogger;
                devicePrepManager.InitializeDevice(false);

                if (_data.SIO.Equals(SIOMethod.NoneSIO))
                {
                    devicePrepManager.Reset();
                }

                // Set up the job (enter parameters, etc.)
                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                UpdateStatus("Setting up job...");
                SetupJob(device);
                UpdateStatus("Job setup complete.");

                // Finish the job (apply job build options, press start, wait for finish)
                UpdateStatus("Finishing job...");
                result = FinishJob(device);
                UpdateStatus("Job finished.");

                // Clean up
                try
                {
                    SignOut();
                    RecordEvent(DeviceWorkflowMarker.ActivityEnd);

                    try
                    {
                        CollectJetAdvantagelinkMemoryMonitoring(deviceInfo);
                    }
                    catch (Exception ex)
                    {
                        SubmitConnectorLog(result.Result.ToString()+ex);
                        return result;
                    }
                }
                catch (Exception ex) when (ex is DeviceCommunicationException || ex is DeviceInvalidOperationException)
                {
                    // Don't fail the activity if there is an exception here.
                    GatherTriageData(device, $"Device could not return to home screen: {ex.ToString()}");
                }
                UpdateStatus("Activity finished.");
            }
            catch (DeviceCommunicationException ex)
            {
                if (ex.Data.Contains(_exceptionCategoryData) && ex.Data[_exceptionCategoryData].Equals(ConnectorExceptionCategory.EnvironmentError.GetDescription()))
                {
                    result = new PluginExecutionResult(PluginResult.Error, ex, ex.Data[_exceptionCategoryData].ToString());
                }
                else
                {
                    result = new PluginExecutionResult(PluginResult.Failed, ex, $"Device communication error: {ex.Message}");
                }
            }
            catch (DeviceInvalidOperationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, $"Device automation error: {ex.Message}");
            }
            catch (DeviceWorkflowException ex)
            {
                if (ex.Data.Contains(_exceptionCategoryData))
                {
                    result = new PluginExecutionResult(PluginResult.Failed, ex, ex.Data[_exceptionCategoryData].ToString());
                }
                else
                {
                    result = new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error.");
                }

                GatherTriageData(device, ex.ToString());
            }
            catch (Exception ex)
            {
                GatherTriageData(device, $"Unexpected exception, gathering triage data: {ex.ToString()}");
                throw;
            }
            SubmitConnectorLog(result.Result.ToString());
            return result;
        }


        /// <summary>
        /// Press SignOut.
        /// </summary>
        protected override void SignOut()
        {
            if (_cloudApp != null)
            {
                _cloudApp.SignOut(_data.LogOut);
            }
        }

        /// <summary>
        /// Dispose Cloud Connector
        /// </summary>
        public override void Dispose()
        {
            if(_cloudApp != null)
            {
                _cloudApp.Dispose();
            }            
        }

        /// <summary>
        /// Collecct and Submit JetAdvantageLink MemoryMonitoring.
        /// </summary>        
        protected override void CollectJetAdvantagelinkMemoryMonitoring(IDeviceInfo deviceInfo)
        {
            if (deviceInfo == null)
            {
                throw new ArgumentNullException(nameof(deviceInfo));
            }

            try
            {
                if (_cloudApp != null)
                {
                    JetAdvantageLinkMemoryMonitoring jalMem = new JetAdvantageLinkMemoryMonitoring(_cloudApp.LinkUI, _cloudApp.CloudConnectorPackageName, ExecutionData, deviceInfo);
                    jalMem.CollectMemoryMonitoringData();
                    jalMem.Submit();
                }
                else
                {
                    UpdateStatus("Device is null - can not submit the JetAdvantageLink memory.");
                }
            }
            catch(Exception)
            {
                UpdateStatus($"JetAdvantageMemoryMonitoring failed - cannot submit the JetAdvantageLink memory.");
            } 
        }

        /// <summary>
        /// Submit TrageData to DB
        /// </summary>
        /// <param name="device"></param>
        /// <param name="reason"></param>    
        protected override void GatherTriageData(IDevice device, string reason)
        {            
            try
            {
                if (_cloudApp != null)
                {                    
                    JetAdvantageLinkTriage triage = new JetAdvantageLinkTriage(_cloudApp.Device, _cloudApp.LinkUI, ExecutionData);
                    triage.CollectTriageData(reason);                    
                    triage.Submit();                    
                }
                else
                {
                    UpdateStatus("Device is null - cannot gather triage data.");
                }
            }
            catch (Exception ex)
            {
                UpdateStatus($"GatherTriageData failed - cannot gather triage data: {ex.Message}");                
            }
            finally
            {
                if(_cloudApp != null)
                {
                    UpdateStatus($"Dispose CloudApp on GatherTriageData");
                    _cloudApp.Dispose();
                }                
            }
        }

        private void LinkAppsActivityStatusChanged(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }
    }
}
