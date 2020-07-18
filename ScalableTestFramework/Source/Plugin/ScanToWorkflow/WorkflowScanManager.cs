using System;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.Dss;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.ScanToWorkflow
{
    public class WorkflowScanManager : ScanActivityManager
    {
        public const string FileNamePrompt = "[OCR]FILE NAME";
        private readonly ScanToWorkflowData _data;
        private IDssWorkflowApp _workflowApp = null;

        protected override string ScanType => "Workflow";

        public WorkflowScanManager(PluginExecutionData executionData, ScanOptions scanOptions)
            : base(executionData)
        {
            _data = executionData.GetMetadata<ScanToWorkflowData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = scanOptions;
        }

        public WorkflowScanManager(PluginExecutionData executionData, ScanOptions scanOptions, string serverName)
            : base(executionData, serverName)
        {
            _data = executionData.GetMetadata<ScanToWorkflowData>(ConverterProvider.GetMetadataConverters());
            ScanOptions = scanOptions;
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
                throw new ArgumentNullException("device");
            }

            // Update the destination type
            ScanLog.ScanType = _data.DestinationType;
            ScanLog.Ocr = _data.UseOcr;

            UpdateStatus(string.Format("Setting up device at address {0} for user {1}", device.Address, ExecutionData.Credential.UserName));

            InitializeAuthenticator(_data.AuthProvider, device, ExecutionData);
            // Load the workflow application
            _workflowApp = DssWorkflowAppFactory.Create(device);

            _workflowApp.WorkflowLogger = Authenticator.WorkflowLogger = WorkflowLogger;
            _workflowApp.Pacekeeper = Authenticator.Pacekeeper = new Pacekeeper(_data.AutomationPause);

            _workflowApp.Launch(Authenticator, !_data.ApplicationAuthentication ? AuthenticationMode.Eager : AuthenticationMode.Lazy);

            // Select the workflow and enter the parameters
            _workflowApp.SelectWorkflow(_data.WorkflowName);
            foreach (WorkflowPromptValue item in _data.PromptValues)
            {
                EnterPromptValue(_workflowApp, item);
            }

            if (!_data.ExcludeFileNamePrompt)
            {
                // Set the file name (required for validation)
                WorkflowPromptValue fileNamePrompt = new WorkflowPromptValue(FileNamePrompt, FilePrefix.ToString().ToLowerInvariant());
                EnterPromptValue(_workflowApp, fileNamePrompt);
            }

            // Set job build
            _workflowApp.Options.SetJobBuildState(this.UseJobBuild);
        }

        private static void EnterPromptValue(IDssWorkflowApp workflowApp, WorkflowPromptValue item)
        {
            Retry.WhileThrowing<DeviceWorkflowException>(
                () => workflowApp.EnterPromptValue(item.PromptText, item.PromptValue),
                3,
                TimeSpan.FromSeconds(5)); // increased the waiting from two seconds to five
        }

        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            // Start the job
            try
            {
                ScanExecutionOptions options = new ScanExecutionOptions();
                if (this.UseJobBuild)
                {
                    options.JobBuildSegments = _data.PageCount;
                }

                if (_workflowApp.ExecuteJob(options))
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }
            }
            finally
            {
                // We got far enough to start the scan job, so submit the log
                SetJobEndStatus(result);
                ExecutionServices.DataLogger.Submit(ScanLog);
            }
            return result;
        }
    }
}
