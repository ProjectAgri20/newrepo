using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.NativeApps.NetworkFolder;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using System.Collections.Generic;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ScanToFolder
{
    /// <summary>
    /// Class NetworkFolderScanManager.
    /// </summary>
    public class NetworkFolderScanManager : ScanActivityManager
    {
        private readonly ScanToFolderData _data;
        private INetworkFolderApp _folderApp;
        protected override string ScanType => "Folder";

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkFolderScanManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="scanOptions">The scan options.</param>
        public NetworkFolderScanManager(PluginExecutionData executionData)
                    : base(executionData)
        {
            _data = executionData.GetMetadata<ScanToFolderData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = _data.ScanOptions;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="NetworkFolderScanManager" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="scanOptions">The scan options.</param>
        /// <param name="serverName">Name of the server.</param>
        public NetworkFolderScanManager(PluginExecutionData executionData, string serverName)
                    : base(executionData, serverName)
        {
            _data = executionData.GetMetadata<ScanToFolderData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = _data.ScanOptions;
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

            // Update the destination type
            ScanLog.ScanType = _data.DestinationType;

            UpdateStatus($"Setting up device at address {device.Address} for user {ExecutionData.Credential.UserName}");

            InitializeAuthenticator(_data.AuthProvider, device, ExecutionData);
            // Load the network folder application
            _folderApp = NetworkFolderAppFactory.Create(device);

            _folderApp.WorkflowLogger = Authenticator.WorkflowLogger = WorkflowLogger;
            _folderApp.Pacekeeper = Authenticator.Pacekeeper = new Pacekeeper(_data.AutomationPause);
            _folderApp.Launch(Authenticator, !_data.ApplicationAuthentication ? AuthenticationMode.Eager : AuthenticationMode.Lazy);

            // Apply settings from the configuration
            if (_data.UseQuickset)
            {
                _folderApp.SelectQuickset(_data.QuickSetName);
            }
            else
            {
                //Network credential being passed as parameter to access the folder and it is used by jediomninetworkfolderapp now
                _folderApp.AddFolderPath(_data.FolderPath, ExecutionData.Credential, _data.ApplyCredentialsOnVerification);
            }

            // Update the destination count
            ScanLog.DestinationCount = (_data.DestinationCount > 0 ?
                                        (short)_data.DestinationCount :
                                        (short)_folderApp.GetDestinationCount());

            // Enter the file name
            _folderApp.EnterFileName(FilePrefix.ToString().ToLowerInvariant());

            //Sets the scan job options
            SetOptions(_data.ScanOptions, _folderApp.Options.GetType(), _folderApp.GetType(), device);
            ScanLog.Ocr = _data.UseOcr;

            // Set job build
            _folderApp.Options.SetJobBuildState(UseJobBuild);
        }
        
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            var result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");

            try
            {
                ScanExecutionOptions options = new ScanExecutionOptions();
                if (UseJobBuild)
                {
                    options.JobBuildSegments = _data.ScanOptions.PageCount;
                }
                options.ImagePreview = (ImagePreviewOption)_data.ImagePreviewOptions;

                // Load the network folder application
                _folderApp = NetworkFolderAppFactory.Create(device);

                _folderApp.WorkflowLogger = WorkflowLogger;
                _folderApp.Pacekeeper = new Pacekeeper(_data.AutomationPause);
                if (_folderApp.ExecuteJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }
                else
                {
                    throw new DeviceWorkflowException(result.Message);
                }
            }
            finally
            {
                SetJobEndStatus(result);
                // We got far enough to start the scan job, so submit the log
                ExecutionServices.DataLogger.Submit(ScanLog);
            }
            return result;
        }
    }
}