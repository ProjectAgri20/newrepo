using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.SafeQ;
using HP.ScalableTest.Framework.Plugin;
using System;
using System.Collections.Generic;
using HP.ScalableTest.PluginSupport.Scan;

namespace HP.ScalableTest.Plugin.ScanToSafeQ
{
    class ScanToSafeQScanManager : ScanActivityManager
    {
        private ISafeQApp _SafeQApp = null;
        private ScanToSafeQActivityData _activityData = null;
        private PluginExecutionData _pluginExecutionData;
        private static List<ScanToSafeQFileType> _validationTargets = new List<ScanToSafeQFileType>() { ScanToSafeQFileType.PDF };
        protected override string ScanType => "Scan";
        public ScanToSafeQScanManager(PluginExecutionData pluginExecutionData, ScanOptions scanOptions) : base(pluginExecutionData)
        {
            _pluginExecutionData = pluginExecutionData;
            _activityData = _pluginExecutionData.GetMetadata<ScanToSafeQActivityData>();
            if (ScanLog != null)
            {
                ScanLog.Ocr = false;
            }
            ScanOptions = scanOptions;
        }
        /// <summary>
        /// Sets up the ScantoSafeQ scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }
            _SafeQApp = SafeQAppFactory.Create(device);
            _SafeQApp.WorkflowLogger = WorkflowLogger;
            InitializeAuthenticator(_activityData.AuthProvider, device, ExecutionData);
            AuthenticationMode am = (_activityData.SafeQAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;
            
            Launch();            
        }

        /// <summary>
        /// Finish up the Cloud Connector scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            result = new PluginExecutionResult(PluginResult.Passed);
                        
            ScanJob(device);
                        
            return result;
        }
        /// <summary>
        /// Launch the SafeQ solution.
        /// </summary>
        protected void Launch()
        {
            string _JobTitle = "SafeQ Scan";
            DateTime startTime = DateTime.Now;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            AuthenticationMode am = (_activityData.SafeQAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;
            if (am.Equals(AuthenticationMode.Eager))
            {
                if (_activityData.AuthProvider == AuthenticationProvider.Card)
                {
                    UpdateStatus("Authenticating by swiping badge on card reader first.");
                }
                else
                {
                    UpdateStatus("Authenticating by pressing the Sign In button first.");
                }
            }
            else // AuthenticationMode.Lazy
            {
                UpdateStatus("Authenticating by pressing the SafeQScan Button first.");
            }
            _SafeQApp.ChangeAppName(_JobTitle);
            _SafeQApp.Launch(Authenticator, am, parameters);
        }

        /// <summary>
        /// Launch the SafeQ solution.
        /// </summary>
        protected void ScanJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred while ScanJob.");
            
            UpdateStatus("Start FileType Selection");
            _SafeQApp.SelectJob(_activityData.WorkFlowDescription, _activityData.ScanCount);
            _SafeQApp.Scan(_activityData.ScanCount);
            UpdateStatus("Completed to select FileType");            
        }
    }
}
