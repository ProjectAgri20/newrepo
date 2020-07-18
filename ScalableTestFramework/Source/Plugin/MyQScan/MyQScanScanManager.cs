using System;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.Framework.Plugin;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.MyQ;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation;

namespace HP.ScalableTest.Plugin.MyQScan
{
    public class MyQScanScanManager : ScanActivityManager
    {
        private IMyQApp _myQApp = null;
        private MyQScanActivityData _activityData = null;
        private PluginExecutionData _pluginExecutionData;
        protected override string ScanType => "Scan";

        public MyQScanScanManager(PluginExecutionData pluginExecutionData, ScanOptions scanOptions) : base(pluginExecutionData)
        {
            _pluginExecutionData = pluginExecutionData;
            _activityData = _pluginExecutionData.GetMetadata<MyQScanActivityData>();
            if(ScanLog != null)
            {
                ScanLog.Ocr = false;
            }
            ScanOptions = scanOptions;
        }

        /// <summary>
        /// Sets up the MyQ scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }
            _myQApp = MyQAppFactory.Create(device);
            _myQApp.WorkflowLogger = WorkflowLogger;
            InitializeAuthenticator(_activityData.AuthProvider, device, ExecutionData);
            Launch();
        }

        /// <summary>
        /// Finish up the MyQ scan job.
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
        /// Launch the MyQ scan solution.
        /// </summary>
        protected void ScanJob()
        {
            UpdateStatus("Press scan button");

            if (_activityData.ScanType.Equals(MyQScanType.Email))
            {
                _myQApp.PressEasyScanEmail();
            }
            else if (_activityData.ScanType.Equals(MyQScanType.Folder))
            {
                _myQApp.PressEasyScanFolder();
            }

            bool result = _myQApp.FinishedProcessingWork();
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);

            if (!result)
            {
                throw new DeviceWorkflowException("Fail to finish scan");
            }
            UpdateStatus("Scan job completed");
        }

        /// <summary>
        /// Launch the MyQ Scan solution.
        /// </summary>
        private void Launch()
        {
            DateTime startTime = DateTime.Now;
            AuthenticationMode am = AuthenticationMode.Eager;

            UpdateStatus("Pressing the MyQ Button");
            _myQApp.Launch(Authenticator, am);
        }
    }
}
