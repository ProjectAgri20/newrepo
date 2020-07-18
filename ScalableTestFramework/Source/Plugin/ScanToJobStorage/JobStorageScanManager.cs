using System;
using HP.DeviceAutomation;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.DeviceAutomation.NativeApps.JobStorage;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using System.Collections.Generic;

namespace HP.ScalableTest.Plugin.ScanToJobStorage
{
    /// <summary>
    /// Scan Manager for the ScanToJobStorage plugin.
    /// </summary>
    public class JobStorageScanManager : ScanActivityManager
    {
        private readonly ScanToJobStorageData _data;
        IJobStorageScanApp _jobStorageScanApp;

        /// <summary>
        /// Scan Type
        /// </summary>
        protected override string ScanType => "Job Storage";

        private const int maxLength = 16;
        /// <summary>
        /// Scan Manager for the ScanToJobStorage plugin.
        /// </summary>
        public JobStorageScanManager(PluginExecutionData executionData)
            : base(executionData)
        {
            _data = executionData.GetMetadata<ScanToJobStorageData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = _data.ScanOptions;
        }

        /// <summary>
        /// Sets up the copy job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            FilePrefix.MaxLength = maxLength;
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            InitializeAuthenticator(_data.AuthProvider, device, ExecutionData);

            // Load the Job storage application
            _jobStorageScanApp = ScanJobStorageAppFactory.Create(device);

            _jobStorageScanApp.WorkflowLogger = Authenticator.WorkflowLogger = WorkflowLogger;
            _jobStorageScanApp.Pacekeeper = Authenticator.Pacekeeper = new Pacekeeper(_data.AutomationPause);
            _jobStorageScanApp.Launch(Authenticator, !_data.ApplicationAuthentication ? AuthenticationMode.Eager : AuthenticationMode.Lazy);
            // Enter the job name
            if (_data.IsPinRequired)
            {
                _jobStorageScanApp.AddJobName(FilePrefix.ToString().ToLowerInvariant(), _data.Pin);
            }
            else
            {
                _jobStorageScanApp.AddJobName(FilePrefix.ToString().ToLowerInvariant());
            }
            //Sets the scan job options
            SetOptions(_data.ScanOptions, _jobStorageScanApp.Options.GetType(), _jobStorageScanApp.GetType(), device);

            // Set job build
            _jobStorageScanApp.Options.SetJobBuildState(UseJobBuild);
        }

        
        /// <summary>
        /// Finish up the copy job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");

            try
            {
                ScanExecutionOptions options = new ScanExecutionOptions();
                if (UseJobBuild)
                {
                    options.JobBuildSegments = _data.ScanOptions.PageCount;
                }

                _jobStorageScanApp.Pacekeeper = new Pacekeeper(_data.AutomationPause);
                if (_jobStorageScanApp.ExecuteScanJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }
            }
            finally
            {
                // We got far enough to start the scan job, so submit the log
                ExecutionServices.DataLogger.Submit(ScanLog);
            }
            return result;
        }
    }
}