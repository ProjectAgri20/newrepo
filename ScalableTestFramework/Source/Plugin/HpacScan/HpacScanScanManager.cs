using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.HpacScan;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.DeviceAutomation.Enums;
using HP.ScalableTest.DeviceAutomation.HpacScan;

namespace HP.ScalableTest.Plugin.HpacScan
{
    public class HpacScanScanManager : ScanActivityManager
    {
        private IHpacScanApp _HpacApp = null;
        private HpacScanActivityData _activityData = null;
        private PluginExecutionData _pluginExecutionData;
        protected override string ScanType => "Scan";

        public HpacScanScanManager(PluginExecutionData pluginExecutionData, ScanOptions scanOptions) : base(pluginExecutionData)
        {
            _pluginExecutionData = pluginExecutionData;
            _activityData = _pluginExecutionData.GetMetadata<HpacScanActivityData>();
            if (ScanLog != null)
            {
                ScanLog.Ocr = false;
            }
            ScanOptions = scanOptions;
        }

        /// <summary>
        /// Sets up the Hpac scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            if(device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _HpacApp = HpacScanAppFactory.Create(device);
            _HpacApp.WorkflowLogger = WorkflowLogger;
            InitializeAuthenticator(_activityData.AuthProvider, device, ExecutionData);
            Launch();
            SetOptions();
        }

        /// <summary>
        /// Finish up the Hpac scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            result = new PluginExecutionResult(PluginResult.Passed);

            ScanJob();

            return result;
        }

        /// <summary>
        /// Launch the Hpac solution.
        /// </summary>
        private void Launch()
        {
            DateTime startTime = DateTime.Now;
            AuthenticationMode am = AuthenticationMode.Lazy;

            UpdateStatus("Pressing the Secure Scan Button");
            _HpacApp.Launch(Authenticator, am);
        }

        /// <summary>
        /// Set SetOptions for Hpac scan.
        /// </summary>
        private void SetOptions()
        {
            UpdateStatus("Set scan option start");
            _HpacApp.SelectOption();
            if (!_activityData.PaperSupplyType.Equals(PaperSupplyType.Simplex))
            {
                _HpacApp.SetPaperSupply(_activityData.PaperSupplyType);
            }
            if (!_activityData.ColorModeType.Equals(ColorModeType.Monochrome))
            {
                _HpacApp.SetColorMode(_activityData.ColorModeType);
            }
            if (!_activityData.QualityType.Equals(QualityType.Normal))
            {
                _HpacApp.SetQuality(_activityData.QualityType);
            }
            if (_activityData.JobBuild)
            {
                _HpacApp.SetJobBuild();
            }
        }

        /// <summary>
        /// Launch the Hpac scan solution.
        /// </summary>
        protected void ScanJob()
        {
            UpdateStatus("Press scan button");
            if (_activityData.PaperSupplyType.Equals(PaperSupplyType.Simplex))
            {
                _HpacApp.ScanSimplex(_activityData.ScanCount, _activityData.JobBuild);
            }
            else
            {
                _HpacApp.ScanDuplex(_activityData.ScanCount, _activityData.JobBuild);
            }
            _HpacApp.FinishedProcessingWork();
            UpdateStatus("Scan job completed");
        }

    }
}
