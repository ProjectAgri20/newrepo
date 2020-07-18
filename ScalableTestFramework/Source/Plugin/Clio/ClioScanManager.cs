using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Enums;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.Clio;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.Utility;
using System;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;


namespace HP.ScalableTest.Plugin.Clio
{
    class ClioScanManager : LinkScanActivityManager
    {
        private readonly ClioActivityData _data;
        private IDevice _device;
        ClioApp _clio = null;

        protected override string LinkJobType => ClioJobType.Scan.GetDescription();

        /// <summary>
        /// Initializes a new instance of the <see cref="ClioScanManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="scanOptions">The scan options.</param>
        /// <param name="lockTimeoutData">The lock timeout options.</param>
        public ClioScanManager(PluginExecutionData executionData, LinkScanOptions scanOptions, LockTimeoutData lockTimeoutData)
            : base(executionData, scanOptions, lockTimeoutData)
        {
            _data = executionData.GetMetadata<ClioActivityData>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ClioScanManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="scanOptions">The scan options.</param>
        /// <param name="serverName">Name of the server.</param>
        public ClioScanManager(PluginExecutionData executionData, LinkScanOptions scanOptions, LockTimeoutData lockTimeoutData, string serverName)
            : base(executionData, scanOptions, lockTimeoutData, serverName)
        {
            _data = executionData.GetMetadata<ClioActivityData>();
        }

        /// <summary>
        /// Sets up the Clio scan job.
        /// </summary>
        /// <param name="device">The de vice.</param>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }
            _device = device;
            _clio = new ClioApp(device);
            _clio.WorkflowLogger = WorkflowLogger;
            ConnectorLog.PageCount = _data.PageCount;
            ConnectorLog.FilePrefix = FilePrefix.ToString();

            UpdateStatus($"Starting App Launch");
            _clio.Launch(_data.SIO);

            // Login
            UpdateStatus($"Starting Log in");
            if (!_data.SIO.Equals(SIOMethod.SIOWithoutIDPWD))
            {
                _clio.Login(_data.ID, _data.PW);
            }
            else
            {
                UpdateStatus($"Skip Login");
            }

            //Select storage type
            UpdateStatus($"Select location :: {_data.Location}");
            _clio.SelectStorageLocation(_data.Location);

            //Matter
            UpdateStatus($"Select matter :: {_data.Matter}");
            _clio.SelectMatter(_data.Matter);

            // Do Navigate to destination
            if(!string.IsNullOrEmpty(_data.FolderPath))
            {
                UpdateStatus($"Navigating destination :: {_data.FolderPath}");
                _clio.NavigateToDestination(_data.FolderPath, _data.JobType);
            }
            _clio.PressScanButton();
            _clio.ScanDocumentOptions(FilePrefix.ToString().ToLowerInvariant());
            _clio.SelectJobForSetOptions(EnumUtil.GetByDescription<ClioJobType>(LinkJobType));
            // Select JobType(Scan or Print Button in the folder manager)
            try
            {
                SetOptions(_clio.ScanOptionManager, _data.ScanOptions);
            }
            catch (DeviceWorkflowException ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fali to set options: {ex.Message} :: Clio", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SelectOptions.GetDescription());
                throw e;
            }
            
        }

        /// <summary>
        /// Finish up the Clio scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            _clio.ExecutionScanJob(_data.PageCount, _data.ScanOptions.OriginalSides);
            result = new PluginExecutionResult(PluginResult.Passed);
            return result;
        }

        protected override PluginExecutionResult ExecuteLinkScan(IDevice device, IDeviceInfo deviceInfo)
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
                    //NavigateHome();
                    RecordEvent(DeviceWorkflowMarker.ActivityEnd);

                    try
                    {
                        CollectJetAdvantagelinkMemoryMonitoring(deviceInfo);
                    }
                    catch (Exception ex)
                    {
                        SubmitConnectorLog(result.Result.ToString());
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
                    result = new PluginExecutionResult(PluginResult.Error, ex, $"Webview communication error - You need check \"Debuggable option enable\" to hpk file: {ex.Message}");
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
        /// Set options for the scan job.
        /// </summary>
        /// <param name="scanOptionsManager">The JetAdvantageLinkPrintOptionManager.</param>
        /// <param name="scanOptions">The Clio Options.</param>
        /// <returns>The result of the scan.</returns>
        protected override void SetOptions(JetAdvantageLinkScanOptionManager scanOptionsManager, LinkScanOptions scanOptions)
        {
            UpdateStatus($"Set the options screen");
            scanOptionsManager.SetOptionsScreen();

            UpdateStatus($"Select option activity with File Type {scanOptions.FileType.GetDescription()}, Resolution {scanOptions.Resolution.GetDescription()} is being started");
            scanOptionsManager.SetFileTypeAndResolution(scanOptions.FileType, scanOptions.Resolution);
            UpdateStatus($"Select option activity with File Type {scanOptions.FileType.GetDescription()}, Resolution {scanOptions.Resolution.GetDescription()} is being completed");

            UpdateStatus($"Select option activity with Original Sides {scanOptions.OriginalSides.GetDescription()} is being started");
            scanOptionsManager.SetOriginalSides(scanOptions.OriginalSides);
            UpdateStatus($"Select option activity with Original Sides {scanOptions.OriginalSides.GetDescription()} is being completed");

            UpdateStatus($"Select option activity with Color/Black {scanOptions.ColorBlack.GetDescription()} is being started");
            scanOptionsManager.SetColorBlack(scanOptions.ColorBlack);
            UpdateStatus($"Select option activity with Color/Black {scanOptions.ColorBlack.GetDescription()} is being completed");

            UpdateStatus($"Select option activity with Original Size {scanOptions.OriginalSize.GetDescription()} is being started");
            scanOptionsManager.SetOriginalSize(scanOptions.OriginalSize);
            UpdateStatus($"Select option activity with Original Size {scanOptions.OriginalSize.GetDescription()} is being completed");

            UpdateStatus($"Select option activity with ContentOrientation {scanOptions.ContentOrientation.GetDescription()} is being started");
            scanOptionsManager.SetOrientation(scanOptions.ContentOrientation);
            UpdateStatus($"Select option activity with ContentOrientation {scanOptions.ContentOrientation.GetDescription()} is being completed");
        }

        /// <summary>
        /// Navigates to the home screen.
        /// </summary>
        protected override void SignOut()
        {
            if (_clio != null)
            {
                _clio.SignOut(_data.LogOut);
            }
        }

        /// <summary>
        /// Dispose Clio
        /// </summary>
        public override void Dispose()
        {
            if (_clio != null)
            {
                _clio.Dispose();
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
                if(_clio != null)
                {
                    JetAdvantageLinkMemoryMonitoring jalMem = new JetAdvantageLinkMemoryMonitoring(_clio.LinkUI, _clio.ClioAppsPackageName, ExecutionData, deviceInfo);
                    jalMem.CollectMemoryMonitoringData();
                    jalMem.Submit();
                }
                else
                {
                    UpdateStatus("Device is null - can not submit the JetAdvantageLink memory.");
                }
            }
            catch (Exception)
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
                if (_clio != null)
                {
                    JetAdvantageLinkTriage triage = new JetAdvantageLinkTriage(_clio.Device, _clio.LinkUI, ExecutionData);
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
                if (_clio != null)
                {
                    UpdateStatus($"Dispose Clio App on GatherTriageData");
                    _clio.Dispose();
                }
            }
        }
    }
}
