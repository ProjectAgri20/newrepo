using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.DeviceAutomation.LinkApps.LinkScanApps;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.Utility;
using System;
using static HP.ScalableTest.DeviceAutomation.LinkApps.LinkScanApps.LinkScanApp;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.LinkScanApps
{
    class LinkScanAppsScanManager : LinkScanActivityManager
    {
        private readonly LinkScanAppsActivityData _data;
        private IDevice _device;
        LinkScanApp _linkScanApp = null;

        protected override string LinkJobType => "Scan";

        /// <summary>
        /// Initializes a new instance of the <see cref="LinkScanAppsScanManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="scanOptions">The scan options.</param>
        public LinkScanAppsScanManager(PluginExecutionData executionData, LinkScanOptions scanOptions, LockTimeoutData lockTimeoutData)
            : base(executionData, scanOptions, lockTimeoutData)
        {
            _data = executionData.GetMetadata<LinkScanAppsActivityData>();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="LinkScanAppsScanManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="scanOptions">The scan options.</param>
        /// <param name="serverName">Name of the server.</param>
        public LinkScanAppsScanManager(PluginExecutionData executionData, LinkScanOptions scanOptions, LockTimeoutData lockTimeoutData, string serverName)
            : base(executionData, scanOptions, lockTimeoutData, serverName)
        {
            _data = executionData.GetMetadata<LinkScanAppsActivityData>();
        }

        /// <summary>
        /// Sets up the LinkScanApps scan job.
        /// </summary>
        /// <param name="device">The de vice.</param>
        protected override void SetupJob(IDevice device)
        {
            if(device == null)
            {
                throw new ArgumentNullException("device");
            }

            _device = device;

            _linkScanApp = new LinkScanApp(_data.ScanDestination, device);
            _linkScanApp.WorkflowLogger = WorkflowLogger;
            _linkScanApp.ActivityStatusChanged += LinkAppsActivityStatusChanged;

            UpdateStatus($"Starting App Launch :: {EnumUtil.GetDescription(_data.ScanDestination)}");
            _linkScanApp.Launch();

            UpdateStatus($"Starting navigate to destination :: {EnumUtil.GetDescription(_data.ScanDestination)}");

            if (!_data.FileNameIsChecked)
            {
                _data.FileName = FilePrefix.ToString();
            }
            if (!_data.SubjectIsChecked)
            {
                _data.Subject = FilePrefix.ToString();
            }
            if (!_data.MessageIsChecked)
            {
                _data.Message = "";
            }
            try
            {
                switch (_data.ScanDestination)
                {
                    case LinkScanDestination.Email:
                        UpdateStatus($"Starting Email Job Setting");
                        ScanToEmailInfo emailInfo = new ScanToEmailInfo()
                        {
                            From = _data.From,
                            To = _data.To,
                            Cc = _data.Cc,
                            Bcc = _data.Bcc,
                            Subject = _data.Subject,
                            Message = _data.Message
                        };
                        _linkScanApp.EmailJobSetting(_data.FileName,emailInfo);
                        break;
                    case LinkScanDestination.FTP:
                        UpdateStatus($"Starting FTP Job Setting");
                        ScanToSMBFTPInfo ftpInfo = new ScanToSMBFTPInfo()
                        {
                            Server = _data.Server,
                            UserName = _data.UserName,
                            Password = _data.Password,
                            DomainPort = _data.DomainPort,
                            FolderPath = _data.FolderPath
                        };
                        _linkScanApp.FileNameFolderPathSetting(_data.FileName, ftpInfo);
                        break;
                    case LinkScanDestination.SMB:
                        UpdateStatus($"Starting SMB Job Setting");
                        ScanToSMBFTPInfo smbInfo = new ScanToSMBFTPInfo()
                        {
                            Server = _data.Server,
                            UserName = _data.UserName,
                            Password = _data.Password,
                            DomainPort = _data.DomainPort
                        };
                        _linkScanApp.FileNameFolderPathSetting(_data.FileName,smbInfo);
                        break;
                    default:
                        throw new DeviceWorkflowException($"Scan destination is not valid: {EnumUtil.GetDescription(_data.ScanDestination)}");
                }

                UpdateStatus($"Starting selection options :: {EnumUtil.GetDescription(_data.ScanDestination)}");

                SetOptions(_linkScanApp.ScanOptionManager, _data.ScanOptions);
            }
            catch(DeviceWorkflowException ex)
            {
                if (ex.Data.Contains(_exceptionCategoryData))
                {
                    throw;
                }

                DeviceWorkflowException e = new DeviceWorkflowException(ex.Message + $" :: {EnumUtil.GetDescription(_data.ScanDestination)}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SelectOptions.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Set options for the scan job.
        /// </summary>
        /// <param name="scanOptionsManager">The JetAdvantageLinkScanOptionManager.</param>
        /// <param name="scanOptions">The LinkScanOptions.</param>
        /// <returns>The result of the scan.</returns>
        protected override void SetOptions(JetAdvantageLinkScanOptionManager scanOptionsManager, LinkScanOptions scanOptions)
        {
            UpdateStatus($"Set the options screen");
            scanOptionsManager.SetOptionsScreen();

            if (scanOptions.UseFileTypeandResolution)
            {
                UpdateStatus($"Select option activity with File Type {scanOptions.FileType.GetDescription()}, Resolution {scanOptions.Resolution.GetDescription()} is being started");
                scanOptionsManager.SetFileTypeAndResolution(scanOptions.FileType, scanOptions.Resolution);
                UpdateStatus($"Select option activity with File Type {scanOptions.FileType.GetDescription()}, Resolution {scanOptions.Resolution.GetDescription()} is being completed");
            }

            if (scanOptions.UseOriginalSides)
            {
                UpdateStatus($"Select option activity with Original Sides {scanOptions.OriginalSides.GetDescription()} is being started");
                scanOptionsManager.SetOriginalSides(scanOptions.OriginalSides);
                UpdateStatus($"Select option activity with Original Sides {scanOptions.OriginalSides.GetDescription()} is being completed");
            }

            if (scanOptions.UseColorBlack)
            {
                UpdateStatus($"Select option activity with Color/Black {scanOptions.ColorBlack.GetDescription()} is being started");
                scanOptionsManager.SetColorBlack(scanOptions.ColorBlack);
                UpdateStatus($"Select option activity with Color/Black {scanOptions.ColorBlack.GetDescription()} is being completed");
            }

            if (scanOptions.UseOriginalSize)
            {
                UpdateStatus($"Select option activity with Original Size {scanOptions.OriginalSize.GetDescription()} is being started");
                scanOptionsManager.SetOriginalSize(scanOptions.OriginalSize);
                UpdateStatus($"Select option activity with Original Size {scanOptions.OriginalSize.GetDescription()} is being completed");
            }

            if (scanOptions.UseContentOrientation)
            {
                UpdateStatus($"Select option activity with ContentOrientation {scanOptions.ContentOrientation.GetDescription()} is being started");
                scanOptionsManager.SetOrientation(scanOptions.ContentOrientation);
                UpdateStatus($"Select option activity with ContentOrientation {scanOptions.ContentOrientation.GetDescription()} is being completed");
            }            
        }

        /// <summary>
        /// Finish up the Cloud Connector scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");

            UpdateStatus($"Starting Execution Scan Job :: {EnumUtil.GetDescription(_data.ScanDestination)}");
            _linkScanApp.ExecutionScanJob(_data.PageCount, _data.ScanOptions.OriginalSides, _data.ScanOptions.UseOriginalSides);
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
                devicePrepManager.NavigateHome();
                //devicePrepManager.Reset();

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
                        SubmitConnectorLog(ex.ToString());

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
        /// Navigates to the home screen.
        /// </summary>
        protected override void SignOut()
        {
            if (_linkScanApp != null)
            {
                RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);
                _linkScanApp.BacktoHomeScreen();
                RecordEvent(DeviceWorkflowMarker.DeviceSignOutEnd);
            }

            if(_device != null)
            {
                var devicePrepManager = DevicePreparationManagerFactory.Create(_device);
                devicePrepManager.NavigateHome();
            }
        }

        /// <summary>
        /// Dispose Cloud Connector
        /// </summary>
        public override void Dispose()
        {
            if (_linkScanApp != null)
            {
                _linkScanApp.Dispose();
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
                if (_linkScanApp != null)
                {
                    JetAdvantageLinkMemoryMonitoring jalMem = new JetAdvantageLinkMemoryMonitoring(_linkScanApp.LinkUI, _linkScanApp.LinkScanAppsPackageName, ExecutionData, deviceInfo);
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
                if (_linkScanApp != null)
                {
                    JetAdvantageLinkTriage triage = new JetAdvantageLinkTriage(_linkScanApp.Device, _linkScanApp.LinkUI, ExecutionData);
                    triage.CollectTriageData(reason);
                    triage.Submit();
                }
                else
                {
                    UpdateStatus("Device is null - cannot gather triage data.");
                }
            }
            catch (Exception e)
            {
                UpdateStatus($"GatherTriageData failed - cannot gather triage data.: {e.Message}");
            }
            finally
            {
                if (_linkScanApp != null)
                {
                    UpdateStatus($"Dispose CloudApp on GatherTriageData");
                    _linkScanApp.Dispose();
                }
            }
        }

        private void LinkAppsActivityStatusChanged(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }
    }
}
