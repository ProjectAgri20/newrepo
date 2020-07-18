using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.GeniusBytes;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.InfoCollection.TriageData;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.PluginSupport.GeniusBytes;
using System;
using System.Linq;
using System.Threading;
using System.Collections.Generic;
using static HP.ScalableTest.Framework.Logger;
using DeviceSimulatorInfo = HP.ScalableTest.Framework.Assets.DeviceSimulatorInfo;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Email;

namespace HP.ScalableTest.Plugin.GeniusBytesScan
{
    /// <summary>
    /// GeniusBytesScanManager class.
    /// </summary>
    public class GeniusBytesScanManager : ScanActivityManager
    {
        /// <summary>
        /// The OXPD engine
        /// </summary>
        private IGeniusBytesApp _geniusBytesApp = null;
        private GeniusBytesScanActivityData _activityData = null;
        private PluginExecutionData _pluginExecutionData;

        protected override string ScanType => "Scan";

        /// <summary>
        /// Initializes a new instance of the <see cref="GeniusBytesScanManager"/> class.
        /// </summary>
        /// <param name="pluginExecutionData">The plugin execution data.</param>
        /// <param name="scanOptions">The activity data.</param>
        public GeniusBytesScanManager(PluginExecutionData pluginExecutionData, ScanOptions scanOptions) : base(pluginExecutionData)
        {
            _pluginExecutionData = pluginExecutionData;
            _activityData = _pluginExecutionData.GetMetadata<GeniusBytesScanActivityData>();
            if (ScanLog != null)
            {
                ScanLog.Ocr = false;
            }
            ScanOptions = scanOptions;
        }

        /// <summary>
        /// Sets up the GeniusBytes scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }
            _geniusBytesApp.WorkflowLogger = WorkflowLogger;
            _geniusBytesApp.StatusMessageUpdate += (s, e) => UpdateStatus(e.StatusMessage);

            InitializeAuthenticator(_activityData.AuthProvider, device, ExecutionData);
            Launch();
            SetOptions();
        }

        /// <summary>
        /// Finish up the GeniusBytes scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            if (ScanJob(device))
            {
                result = new PluginExecutionResult(PluginResult.Passed);
            }

            return result;
        }

        /// <summary>
        /// Launch the GeniusBytes solution.
        /// </summary>
        protected void Launch()
        {
            DateTime startTime = DateTime.Now;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            _geniusBytesApp.Launch(Authenticator);
            _geniusBytesApp.ScrollToObject(_activityData.AppName.GetDescription(), false);
            _geniusBytesApp.PressAppName(_activityData.AppName.GetDescription());
        }

        /// <summary>
        /// Set options for the scan job.
        /// </summary>
        /// <returns>The result of the scan.</returns>
        private void SetOptions()
        {
            TimeSpan timeout = TimeSpan.FromSeconds(5);
            string optionText = string.Empty;

            if (_activityData.AppName == GeniusBytesScanType.Scan2Mail)
            {
                _geniusBytesApp.AddDocument(timeout);
            }

            _geniusBytesApp.SetFileName(FilePrefix.ToString(), _activityData.AppName);
            _geniusBytesApp.SetImagePreview(_activityData.ImagePreview);

            optionText = _activityData.ColourOption.GetDescription();
            if (!_geniusBytesApp.ExistElementText(optionText))
            {
                UpdateStatus($"Setting color option to {optionText}.");
                _geniusBytesApp.SelectColorOption(_activityData.ColourOption, timeout);
            }

            optionText = _activityData.SidesOption.GetDescription();
            if (!_geniusBytesApp.ExistElementText(optionText))
            {
                UpdateStatus($"Setting sides option to {optionText}.");
                _geniusBytesApp.SelectSidesOption(_activityData.SidesOption, timeout);
            }

            optionText = _activityData.ResolutionOption.GetDescription();
            if (!_geniusBytesApp.ExistElementText(optionText))
            {
                UpdateStatus($"Setting resolution option to {optionText}.");
                _geniusBytesApp.SelectResolutionOption(_activityData.ResolutionOption, timeout);
            }

            if (_activityData.FileType != GeniusByteScanFileTypeOption.Multipage_PDF)
            {
                _geniusBytesApp.SetFileType(_activityData.FileType, timeout);
            }
        }

        /// <summary>
        /// Sets the options for ocr.
        /// DWA -- I don't believe this is needed...
        /// </summary>
         private void SetOptionsforOCR()
        {
            _geniusBytesApp.WaitForElementReady("Click here to modify Output file type", TimeSpan.FromSeconds(5));
            _geniusBytesApp.SetFileType(_activityData.FileType, TimeSpan.FromSeconds(3));
        }

        /// <summary>
        /// Executes the scan job using the specified device.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="deviceInfo">The device information.</param>
        /// <returns>
        /// The result of execution.
        /// </returns>
        protected override PluginExecutionResult ExecuteScan(IDevice device, IDeviceInfo deviceInfo)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Automation Failure", "Device workflow error.");
            try
            {
                _geniusBytesApp = GeniusBytesAppFactory.Create(device);
                var devicePrepManager = new GeniusBytesPreparationManager((JediOmniDevice)device, _geniusBytesApp);
                devicePrepManager.WorkflowLogger = WorkflowLogger;
                devicePrepManager.InitializeDevice(false);

                // Configure the device (apply settings, load simulator ADF, etc.)
                UpdateStatus("Configuring device...");
                ConfigureDevice(deviceInfo);
                ScanLog.PageCount = (short)ScanOptions.PageCount;
                ScanLog.Sender = ExecutionData.Credential.UserName;

                // Set up the job (enter parameters, etc.)
                RecordEvent(DeviceWorkflowMarker.ActivityBegin);
                UpdateStatus("Setting up job...");
                SetupJob(device);
                UpdateStatus("Job setup complete.");

                // Finish the job (apply job build options, press start, wait for finish)
                UpdateStatus("Finishing job...");
                result = FinishJob(device);
                UpdateStatus("Job finished.");

                try
                {
                    SignOut();
                    RecordEvent(DeviceWorkflowMarker.ActivityEnd);
                }
                catch (Exception ex) when (ex is DeviceCommunicationException || ex is DeviceInvalidOperationException)
                {
                    // Don't fail the activity if there is an exception here.
                    UpdateStatus("Device could not return to home screen.");
                    //GatherTriageData(device, $"Device could not return to home screen: {ex.ToString()}");
                }

                // SignOut will be developed
                UpdateStatus("Activity finished.");
            }
            catch (DeviceCommunicationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device communication error.");
                GatherTriageData(device, ex.ToString());
            }
            catch (DeviceInvalidOperationException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device automation error.");
                GatherTriageData(device, ex.ToString());
            }
            catch (DeviceWorkflowException ex)
            {
                result = new PluginExecutionResult(PluginResult.Failed, ex, "Device workflow error.");
                GatherTriageData(device, ex.ToString());
            }
            catch (Exception ex)
            {
                GatherTriageData(device, $"Unexpected exception, gathering triage data: {ex.ToString()}");
                throw;
            }
            return result;
        }

        /// <summary>
        /// Run Scan Job in the GeniusBytesScan solution.
        /// </summary>
        protected bool ScanJob(IDevice device)
        {
            //Start Scan
            _geniusBytesApp.StartScan(_activityData.SidesOption.GetDescription(), _activityData.ScanCount);

            if (_activityData.AppName == GeniusBytesScanType.Scan2Mail)
            {
                EmailBuilder emailBuilder = new EmailBuilder(_pluginExecutionData.Credential.UserName, _pluginExecutionData);

                _geniusBytesApp.AddEmail();  
                _geniusBytesApp.SetEmailAddress(emailBuilder.ToString());
            }

            return true;
        }

        private void GatherTriageData(IDevice device, string reason)
        {
            if (device != null)
            {
                ITriage triage = TriageFactory.Create(device, ExecutionData);
                triage.CollectTriageData(reason);
                triage.Submit();
            }
            else
            {
                UpdateStatus("Device is null - cannot gather triage data.");
            }
        }

        private void ConfigureDevice(IDeviceInfo deviceInfo)
        {
            // If this is a Jedi simulator, load documents into the ADF
            DeviceSimulatorInfo simulatorInfo = deviceInfo as DeviceSimulatorInfo;
            if (simulatorInfo?.SimulatorType == "Jedi" && ScanOptions.UseAdf)
            {
                LoadAdf(deviceInfo.Address, ScanOptions.PageCount);
            }
        }

        /// <summary>
        /// Signs the user out of the device.
        /// </summary>
        protected void SignOut()
        {
            _geniusBytesApp.SignOut();

            if (!_geniusBytesApp.VerifySignOut())
            {
                if (!_geniusBytesApp.ExistElementText("Manual Login"))
                {
                    _geniusBytesApp.SignOut();
                    if (!_geniusBytesApp.VerifySignOut())
                    {
                        throw new DeviceWorkflowException("Verify SingOut Failure.");
                    }
                }
            }
        }

        private void LoadAdf(string address, int pageCount)
        {
            if (!ExecutionData.Documents.Any())
            {
                throw new ArgumentException("Document selection data resulted in 0 documents.");
            }

            // Clear the ADF before we do anything else
            AdfSimulator.Clear(address);

            // Iterate over the documents until we reach the desired page count
            DocumentCollectionIterator iterator = new DocumentCollectionIterator(CollectionSelectorMode.RoundRobin);
            for (int i = 0; i < pageCount; i++)
            {
                // Select and log document(s)
                Document document = iterator.GetNext(ExecutionData.Documents);
                ActivityExecutionDocumentUsageLog documentLog = new ActivityExecutionDocumentUsageLog(ExecutionData, document);
                ExecutionServices.DataLogger.Submit(documentLog);

                // Convert relative paths to absolute network paths
                string sharePath = ExecutionServices.FileRepository.AsInternal().GetDocumentSharePath(document);

                // Load it into the ADF
                AdfSimulator.LoadPage(address, sharePath);
            }
        }
    }
}
