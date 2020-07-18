using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.UdocxScan;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using System;
using HP.ScalableTest.Utility;
using HP.ScalableTest.PluginSupport.Scan;
using System.Collections.Generic;

namespace HP.ScalableTest.Plugin.UdocxScan
{
    class UdocxScanManager : ScanActivityManager
    {
        private IUdocxScanApp _UdocxScanApp = null;
        private UdocxScanActivityData _activityData = null;
        private PluginExecutionData _pluginExecutionData;
        protected override string ScanType => "Scan";

        public UdocxScanManager(PluginExecutionData pluginExecutionData, ScanOptions scanOptions) : base(pluginExecutionData)
        {
            _pluginExecutionData = pluginExecutionData;
            _activityData = _pluginExecutionData.GetMetadata<UdocxScanActivityData>();
            if (ScanLog != null)
            {
                ScanLog.Ocr = false;
            }
            ScanOptions = scanOptions;
        }

        /// <summary>
        /// Sets up the CloudConnector scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            var devicePrepManager = DevicePreparationManagerFactory.Create(device);
            if (device == null)
            {
                throw new ArgumentNullException("device");
            }
            _UdocxScanApp = UdocxScanAppFactory.Create(device);
            _UdocxScanApp.WorkflowLogger = WorkflowLogger;
            devicePrepManager.Reset();
            InitializeAuthenticator(_activityData.AuthProvider, device, ExecutionData);
            AuthenticationMode am = (_activityData.UdocxScanAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;
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
            UpdateStatus("Start the Scan Job");
            ScanJob();
            UpdateStatus("Completed the Scan Job");
            //LogOut();
            UpdateStatus("Completed SignOut");
            return result;
        }
        /// <summary>
        /// Launch the SafeQ solution.
        /// </summary>
        protected void Launch()
        {
            string detination = EnumUtil.GetDescription(_activityData.JobType);
            DateTime startTime = DateTime.Now;
            Dictionary<string, object> parameters = new Dictionary<string, object>();
            AuthenticationMode am = AuthenticationMode.Lazy;
            _activityData.AuthProvider = (_activityData.AuthProvider == AuthenticationProvider.Card) ? AuthenticationProvider.Card : AuthenticationProvider.UdocxScan;
            if (_activityData.AuthProvider == AuthenticationProvider.Card)
            {
                    UpdateStatus("Authenticating by swiping badge on card reader first.");
            }
            else
            {
                UpdateStatus("Authenticating by pressing the UdocxScan App Button first.");
            }
            _UdocxScanApp.Launch(Authenticator, am);



            //_UdocxScanApp.Launch();
            UpdateStatus("Completed SignIn");
            _UdocxScanApp.SelectApp(detination);
        }

        /// <summary>
        /// Scan Job for Udocx
        /// </summary>
        protected void ScanJob()
        {
            switch (_activityData.JobType)
            {
                case UdocxScanJobType.ScantoMail:
                    _UdocxScanApp.Scan(_activityData.EmailAddress);
                    break;
                case UdocxScanJobType.ScantoOneDrive:
                case UdocxScanJobType.ScantoSharePoint365:
                    _UdocxScanApp.Scan();
                    break;
            }
        }

        /// <summary>
        /// SignOut for Udocx
        /// </summary>
        protected void LogOut()
        {
            //_UdocxScanApp.LogOut();
        }
    }
}
