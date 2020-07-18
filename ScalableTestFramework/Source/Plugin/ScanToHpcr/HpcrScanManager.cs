using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.Scan;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.ScanToHpcr
{
    public class HpcrScanManager : ScanActivityManager
    {
        private readonly ScanToHpcrActivityData _activityData;
        private PluginExecutionData _pluginExecutionData;
        private IHpcrApp _hpcrScanApplication;

        private CollectMemoryManager _collectMemoryManager;
        private readonly string _documentName = string.Empty;

        protected override string ScanType => "HpcrScan";

        /// <summary>
        /// Initializes a new instance of the <see cref="HpcrScanManager"/> class.
        /// </summary>
        public HpcrScanManager(PluginExecutionData executionData, ScanOptions scanOptions)
            : base(executionData)
        {
            _pluginExecutionData = executionData;
            _activityData = _pluginExecutionData.GetMetadata<ScanToHpcrActivityData>();

            _documentName = FilePrefix.ToString();

            if (ScanLog != null)
            {
                ScanLog.Ocr = false;
            }
            ScanOptions = scanOptions;
        }

        /// <summary>
        /// Gets the plugin application for the type of device being processed.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns></returns>
        private IHpcrApp GetApp(IDevice device)
        {
            // the session ID is used as part of the document name in order to track plug in metrics
            IHpcrApp result = HpcrAppFactory.Create(device, _activityData.HpcrScanButton, _activityData.ScanDestination, _activityData.ScanDistribution, _documentName, _activityData.ImagePreview);

            result.WorkflowLogger = WorkflowLogger;
            result.StatusMessageUpdate += (s, e) => UpdateStatus(e.StatusMessage);

            return result;
        }

        /// <summary>
        /// Sets up the scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            ScanLog.JobEndStatus = "Failed";
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            ScanLog.ScanType = _activityData.HpcrScanButton;

            UpdateStatus($"Setting up device at address {device.Address} for user {_pluginExecutionData.Credential.UserName}");

            InitializeAuthenticator(_activityData.AuthProvider, device, ExecutionData);

            _hpcrScanApplication = GetApp(device);
            AuthenticationMode am = (_activityData.ApplicationAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;
            _hpcrScanApplication.Launch(Authenticator, am);
        }

        /// <summary>
        /// Finishes the setup portion of the job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            UpdateStatus(string.Format("Pressing HPCR workflow button {0}.", _activityData.HpcrScanButton));
            try
            {
                ScanExecutionOptions options = new ScanExecutionOptions()
                {
                    JobBuildSegments = _activityData.PageCount
                };

                if (_hpcrScanApplication.ExecuteJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }

                ScanLog.JobEndStatus = _hpcrScanApplication.HpcrFinalStatus;

                _hpcrScanApplication.JobFinished();
            }
            catch (System.ServiceModel.FaultException e)
            {
                ExecutionServices.SystemTrace.LogError(e);
                result = new PluginExecutionResult(PluginResult.Error, e);
            }
            finally
            {
                // We got far enough to start the scan job, so submit the log
                SetJobEndStatus(result);
                ExecutionServices.DataLogger.Submit(ScanLog);
            }
            return result;
        }

        protected override PluginExecutionResult ExecuteScan(IDevice device, IDeviceInfo deviceInfo)
        {
            //var result = new PluginExecutionResult(PluginResult.Passed);
            var result = new PluginExecutionResult(PluginResult.Failed, "Automation Failure", "Device workflow error.");
            try
            {
                _collectMemoryManager = new CollectMemoryManager(device, deviceInfo);
                result = base.ExecuteScan(device, deviceInfo);
            }
            finally
            {
                _collectMemoryManager.CollectDeviceMemoryProfile(_activityData.DeviceMemoryProfilerConfig, "ScanToHpcr");
            }
            return result;
        }

    }
}
