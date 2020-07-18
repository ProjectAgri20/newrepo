using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.AutoStore;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.MemoryCollection;
using HP.ScalableTest.PluginSupport.Scan;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.AutoStore
{
    /// <summary>
    /// Setting the authentication provider to AutoStore. If AutoStore ever creates an eager authentication, this will need to be revisited. 
    /// DWA - 12/03/2018
    /// </summary>
    /// <seealso cref="HP.ScalableTest.PluginSupport.Scan.ScanActivityManager" />
    public class AutoStoreScanManager : ScanActivityManager
    {
        private readonly AutoStoreActivityData _activityData;
        private readonly string _documentName = string.Empty;
        private IAutoStoreApp _autoStoreApplication;

        private CollectMemoryManager _collectMemoryManager;

        /// <summary>
        /// Gets the type of the scan performed by this <see cref="T:HP.ScalableTest.PluginSupport.Scan.ScanActivityManager" />.
        /// </summary>
        protected override string ScanType => "AutoStore";

        /// <summary>
        /// Initializes a new instance of the <see cref="AutoStoreScanManager"/> class.
        /// </summary>
        /// <param name="executionData"></param>
        /// <!-- Badly formed XML comment ignored for member "M:HP.ScalableTest.PluginSupport.Scan.ScanActivityManager.#ctor(HP.ScalableTest.Framework.Plugin.PluginExecutionData)" -->
        public AutoStoreScanManager(PluginExecutionData executionData) : base(executionData)
        {
            _activityData = ExecutionData.GetMetadata<AutoStoreActivityData>();

            _activityData.AuthProvider = (_activityData.AutoStoreAuthentication == true) ? AuthenticationProvider.AutoStore : AuthenticationProvider.Auto;

            ScanOptions = _activityData.ScanOptions;
            _documentName = FilePrefix.ToString();
            
            if (ScanLog != null)
            {
                ScanLog.Ocr = _activityData.UseOcr;
            }
        }

        private IAutoStoreApp GetApp(IDevice device)
        {
            IAutoStoreApp result = AutoStoreAppFactory.Create(device, _activityData.AutoStoreScanButton, _documentName);

            result.WorkflowLogger = WorkflowLogger;
            result.StatusMessageUpdate += (s, e) => UpdateStatus(e.StatusMessage);

            return result;
        }
        /// <summary>
        /// Finishes the scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>
        /// The result of the scan.
        /// </returns>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            UpdateStatus(string.Format("Pressing AutoStore workflow button {0}.", _activityData.AutoStoreScanButton));
            try
            {
                AutoStoreExecutionOptions options = new AutoStoreExecutionOptions()
                {
                    JobBuildSegments = _activityData.ScanOptions.PageCount,
                    ImagePreview = _activityData.ImagePreview                   
                };
                options.SetAutoStoreWorkflow(_activityData.AutoStoreScanButton, _activityData.UseOcr);

                if (_autoStoreApplication.ExecuteJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                    ScanLog.JobEndStatus = "Success";
                }

                // Method used to clean up AutoStore settings after a run. If  options have been turned on they
                // must be turned off. Options are NOT auto reset after logout. So any option that is set by the
                // plugin must be reset back to default at end of run.

                _autoStoreApplication.JobFinished(options);
            }
            finally
            {
                _collectMemoryManager.CollectDeviceMemoryProfile(_activityData.DeviceMemoryProfilerConfig, "AutoStore");
                // We got far enough to start the scan job, so submit the log
                ExecutionServices.DataLogger.Submit(ScanLog);
            }
            return result;
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
            ScanLog.JobEndStatus = "Failed";
            try
            {
                _collectMemoryManager = new CollectMemoryManager(device, deviceInfo);
                result = base.ExecuteScan(device, deviceInfo);                
            }
            finally
            {
                SetJobEndStatus(result);
                //collectMemoryManager.CollectDeviceMemoryProfile(_activityData.DeviceMemoryProfilerConfig, "AutoStore");
            }
            return result;
        }
        /// <summary>
        /// Initializes the authenticator that will be used in the run.  
        /// </summary>
        /// <param name="provider">The authentication provider to create.</param>
        /// <param name="device">The device that will be used for the run.</param>
        protected void InitializeAuthenticator(AuthenticationProvider provider, IDevice device)
        {
            // Only AutoStore authentication (Lazy) is allowed by the application at this time.
            // Also, card auth is not supported at this time.

            Authenticator = AuthenticatorFactory.Create(device, ExecutionData.Credential, provider);
            Authenticator.WorkflowLogger = WorkflowLogger;
        }
        /// <summary>
        /// Sets up the scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <exception cref="ArgumentNullException">device</exception>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }
            
            AuthenticationMode am = (_activityData.AutoStoreAuthentication == true) ? AuthenticationMode.Lazy : AuthenticationMode.Eager;

            ScanLog.ScanType = _activityData.AutoStoreScanButton;
            InitializeAuthenticator(_activityData.AuthProvider, device);
            UpdateStatus($"Setting up device at address {device.Address} for user {ExecutionData.Credential.UserName}");

            _autoStoreApplication = GetApp(device);
            _autoStoreApplication.Launch(Authenticator, am);

        }
    }
}
