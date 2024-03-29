﻿using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceMemory.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.iManage;
using HP.ScalableTest.Utility;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.Plugin.iManage
{
    class iManageScanManager : LinkScanActivityManager
    {
        private readonly iManageActivityData _data;
        private IDevice _device;
        iManageApp _imanage = null;

        protected override string LinkJobType => iManageJobType.Scan.GetDescription();

        /// <summary>
        /// Initializes a new instance of the <see cref="iManageScanManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="scanOptions">The scan options.</param>
        /// <param name="lockTimeoutData">The lock timeout options.</param>
        public iManageScanManager(PluginExecutionData executionData, LinkScanOptions scanOptions, LockTimeoutData lockTimeoutData)
            : base(executionData, scanOptions, lockTimeoutData)
        {
            _data = executionData.GetMetadata<iManageActivityData>();
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="iManageScanManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="scanOptions">The scan options.</param>
        /// <param name="serverName">Name of the server.</param>
        public iManageScanManager(PluginExecutionData executionData, LinkScanOptions scanOptions, LockTimeoutData lockTimeoutData, string serverName)
            : base(executionData, scanOptions, lockTimeoutData, serverName)
        {
            _data = executionData.GetMetadata<iManageActivityData>();
        }

        /// <summary>
        /// Sets up the iManageApp scan job.
        /// </summary>
        /// <param name="device">The de vice.</param>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }
            _device = device;
            _imanage = new iManageApp(device);
            _imanage.WorkflowLogger = WorkflowLogger;
            _imanage.ActivityStatusChanged += LinkAppsActivityStatusChanged;

            ConnectorLog.PageCount = _data.PageCount;
            UpdateStatus($"Starting App Launch");
            _imanage.Launch(_data.SIO);

            // Login
            UpdateStatus($"Starting Log in");
            _imanage.Login(_data.ID, _data.Password);

            // Select storage type
            UpdateStatus($"Selecting storage :: {_data.Location.GetDescription()}");
            _imanage.SelectStorageLocation(_data.Location);

            // Do Navigate to destination
            UpdateStatus($"Navigating destination :: {_data.FolderPath}");

            // Select JobType(Scan or Print Button in the folder manager)
            _imanage.NavigateToDestination(_data.FolderPath, _data.JobType);
            
            // Set scan options
            SetOptions(_imanage.ScanOptionManager, _data.ScanOptions);
        }

        /// <summary>
        /// Finish up the Vertical Connector scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            _imanage.ExecutionScanJob(_data.PageCount, _data.ScanOptions.OriginalSides, _data.ScanOptions.UseOriginalSides);
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
        /// <param name="scanOptionsManager">The JetAdvantageLinkScanOptionManager.</param>
        /// <param name="scanOptions">The LinkScanOptions.</param>
        /// <returns>The result of the scan.</returns>
        protected override void SetOptions(JetAdvantageLinkScanOptionManager scanOptionsManager, LinkScanOptions scanOptions)
        {            
            UpdateStatus($"Select option activity with File Name {FilePrefix.ToString().ToLowerInvariant()} is being started");
            scanOptionsManager.SetFileName(FilePrefix.ToString().ToLowerInvariant(), "et_filename");
            UpdateStatus($"Select option activity with File Name {FilePrefix.ToString().ToLowerInvariant()} is being completed");

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
        /// Navigates to the home screen.
        /// </summary>
        protected override void SignOut()
        {
            if (_imanage != null)
            {
                _imanage.SignOut(_data.LogOut);
            }
        }

        /// <summary>
        /// Dispose Vertical Connector
        /// </summary>
        public override void Dispose()
        {
            if (_imanage != null)
            {
                _imanage.Dispose();
            }
        }

        /// <summary>
        /// Collecct and Submit JetAdvantageLink MemoryMonitoring.
        /// </summary>        
        protected override void CollectJetAdvantagelinkMemoryMonitoring(IDeviceInfo deviceInfo)
        {
            try
            {
                if (deviceInfo == null)
                {
                    throw new ArgumentNullException(nameof(deviceInfo));
                }

                JetAdvantageLinkMemoryMonitoring jalMem = new JetAdvantageLinkMemoryMonitoring(_imanage.LinkUI, _imanage.iManageAppsPackageName, ExecutionData, deviceInfo);
                jalMem.CollectMemoryMonitoringData();
                jalMem.Submit();
            }
            catch
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
                if (_imanage != null)
                {
                    JetAdvantageLinkTriage triage = new JetAdvantageLinkTriage(_imanage.Device, _imanage.LinkUI, ExecutionData);
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
                if (_imanage != null)
                {
                    UpdateStatus($"Dispose CloudApp on GatherTriageData");
                    _imanage.Dispose();
                }
            }
        }

        private void LinkAppsActivityStatusChanged(object sender, StatusChangedEventArgs e)
        {
            UpdateStatus(e.StatusMessage);
        }
    }
}
