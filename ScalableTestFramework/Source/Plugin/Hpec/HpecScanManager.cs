using System;
using System.ComponentModel;
using HP.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.SolutionApps.Hpec;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Scan;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.Hpec
{
    public class HpecScanManager : ScanActivityManager
    {
        private readonly HpecActivityData _data;

        private IHpecApp _hpecApp;

        protected override string ScanType => "HPEC";

        /// <summary>
        /// Initializes a new instance of the <see cref="HpcrScanManager"/> class.
        /// </summary>
        public HpecScanManager(PluginExecutionData executionData, ScanOptions scanOptions)
            : base(executionData)
        {
            _data = executionData.GetMetadata<HpecActivityData>();
            ScanOptions = scanOptions;
        }

        /// <summary>
        /// Sets up the scan job.
        /// </summary>
        /// <param name="device">The device.</param>
        protected override void SetupJob(IDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            UpdateStatus(string.Format("Setting up device at address {0} for user {1}", device.Address, ExecutionData.Credential.UserName));

            string entryButtonTitle = "My workflow (FutureSmart)";

            IAuthenticator auth = AuthenticatorFactory.Create(device, ExecutionData.Credential, AuthenticationProvider.Auto);
            auth.WorkflowLogger = WorkflowLogger;

            AuthenticationMode am = (_data.ApplicationAuthentication == false) ? AuthenticationMode.Eager : AuthenticationMode.Lazy;
            if (am.Equals(AuthenticationMode.Eager))
            {
                UpdateStatus("Authenticating by pressing the Sign In button first.");
            }
            else
            {
                UpdateStatus(string.Format("Authenticating by pressing the {0} button first.", entryButtonTitle));
            }

            _hpecApp = HpecFactory.Create(device);
            _hpecApp.WorkflowLogger = WorkflowLogger;

            _hpecApp.Launch(auth, am);
        }

        /// <summary>
        /// Gets the work flow enum based on the workflow string in HpecActivityData.
        /// This will need to be expanded as workflows are added.
        /// </summary>
        /// <returns></returns>
        private HpecWorkflows GetWorkFlowEnum()
        {
            HpecWorkflows wf = HpecWorkflows.SendToEmail;

            if (_data.WorkflowName.Equals("Send to Network Folder"))
            {
                wf = HpecWorkflows.SendToNetworkFolder;
            }

            return wf;
        }
        /// <summary>
        /// Finishes the job.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="configuration">The configuration.</param>
        protected override PluginExecutionResult FinishJob(IDevice device)
        {
            string wf = GetWorkFlowEnum().GetDescription();
            string documentName = FilePrefix.ToString();

            UpdateStatus(string.Format("Pressing HPEC workflow button {0}.", wf));

            PluginExecutionResult result = new PluginExecutionResult(PluginResult.Failed, "Error occurred After Login and Job Configuration.", "Device automation error.");
            try
            {
                _hpecApp.ExecuteJob(wf, documentName);

                UpdateStatus("Start scanning the document.");

                _hpecApp.HpecStartScan(_data.PageCount);

                UpdateStatus("Processing the HPEC Workflow.");
                if (_hpecApp.ProcessJobAfterScan())
                {
                    result = new PluginExecutionResult(PluginResult.Passed);
                }

                UpdateStatus("Signing out...");
                _hpecApp.JobFinished();

                ScanLog.ScanType = wf;
                ScanLog.DestinationCount = (short)_hpecApp.ReportedHpecPageCount;
                ScanLog.JobEndStatus = "Success";
            }
            catch (DeviceWorkflowException ex)
            {
                UpdateStatus(ex.Message);
                ScanLog.JobEndStatus = "Failed";
                throw;
            }
            catch
            {
                UpdateStatus("Error executing activity");
                ScanLog.JobEndStatus = "Failed";
                throw;
            }
            finally
            {
                ExecutionServices.DataLogger.Submit(ScanLog);
            }
            return result;
        }

        /// <summary>
        /// Used for maintaining the HPEC workflows
        /// </summary>
        private enum HpecWorkflows
        {
            [Description("Send to Email")]
            SendToEmail,

            [Description("Send to Network Folder")]
            SendToNetworkFolder
        }
    }
}
