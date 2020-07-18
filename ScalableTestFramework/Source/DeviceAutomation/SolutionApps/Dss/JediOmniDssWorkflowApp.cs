using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Dss
{
    /// <summary>
    /// Implementation of <see cref="IDssWorkflowApp" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public sealed class JediOmniDssWorkflowApp : DeviceWorkflowLogSource, IDssWorkflowApp
    {
        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniMasthead _masthead;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniNotificationPanel _notificationPanel;
        private readonly DssEnhancedWorkflowApp _enhancedWorkflowApp;
        private readonly TimeSpan _idleTimeoutOffset;
        private readonly JediOmniPopupManager _popupManager;
        private readonly OxpdBrowserEngine _engine;

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniDssWorkflowApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniDssWorkflowApp(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _masthead = new JediOmniMasthead(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            _notificationPanel = new JediOmniNotificationPanel(device);
            _enhancedWorkflowApp = new DssEnhancedWorkflowApp(_controlPanel);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
            _popupManager = new JediOmniPopupManager(device);
            _engine = new OxpdBrowserEngine(_device.ControlPanel, DssWorkflowResource.DssWorkflowJavaScript);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Workflow application on the device.
        /// </summary>
        public void Launch()
        {
            bool appLoaded = false;

            // The button ID uses a generated identifier which always starts with the same GUID
            string appButtonId = _controlPanel.GetIds("[id^=hpid-1c043000]", OmniIdCollectionType.Self).FirstOrDefault();
            if (appButtonId != null)
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "DSS Workflow");
                appLoaded = _controlPanel.ScrollPressWait($"#{appButtonId}", "#hpid-oxpd-scroll-pane", TimeSpan.FromSeconds(10));
                RecordEvent(DeviceWorkflowMarker.AppShown);
                Pacekeeper.Pause();
            }
            else
            {
                throw new DeviceWorkflowException("DSS Workflow application button was not found on device home screen.");
            }

            if (!appLoaded)
            {
                if (_controlPanel.CheckState("#hpid-signin-app-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException("Sign-in required to launch the DSS Workflow application.");
                }
                else
                {
                    throw new DeviceWorkflowException("Could not launch DSS Workflow application.");
                }
            }
        }

        /// <summary>
        /// Launches DSS with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                if (authenticator.Provider != AuthenticationProvider.Card)
                {
                    _launchHelper.WorkflowLogger = WorkflowLogger;
                    _launchHelper.PressSignInButton();
                }
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                PressDssSolutionButton("#hpid-oxpd-scroll-pane");
            }
            else // AuthenticationMode.Lazy
            {
                if (PressDssSolutionButton("#hpid-signin-body"))
                {
                    Authenticate(authenticator, "#hpid-oxpd-scroll-pane");
                }
            }
            _controlPanel.WaitForAvailable("#hpid-oxpd-scroll-pane");
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Authenticates the specified authenticator.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">The wait form.</param>
        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            if (!_controlPanel.WaitForAvailable(waitForm, _idleTimeoutOffset))
            {
                throw new DeviceWorkflowException("DSS Menu screen did not show within " + _idleTimeoutOffset.Seconds.ToString() + " seconds.");
            }
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Presses the DSS solution button.
        /// </summary>
        /// <returns>bool - true means the sign in screen is present and authentication must be called.</returns>
        /// <exception cref="DeviceWorkflowException">
        /// DSS Workflow application button was not found on device home screen.
        /// or
        /// Could not launch DSS Workflow application.
        /// </exception>
        private bool PressDssSolutionButton(string expectedSelector)
        {
            bool signInScreenLoaded = false;

            // The button ID uses a generated identifier which always starts with the same GUID
            string appButtonId = _controlPanel.GetIds("[id^=hpid-1c043000]", OmniIdCollectionType.Self).FirstOrDefault();
            if (appButtonId != null)
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "DSS Workflow");
                signInScreenLoaded = _controlPanel.ScrollPressWait($"#{appButtonId}", expectedSelector, TimeSpan.FromSeconds(10));
                Pacekeeper.Pause();
            }
            else
            {
                throw new DeviceWorkflowException("DSS Workflow application button was not found on device home screen.");
            }
            if (expectedSelector == "#hpid-signin-body" && !signInScreenLoaded)
            {
                if (_controlPanel.CheckState("#hpid-oxpd-scroll-pane", OmniElementState.Exists))
                {
                    // The application launched without prompting for credentials
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                    Pacekeeper.Reset();
                }
                else
                {
                    throw new DeviceWorkflowException("Could not launch DSS Workflow application.");
                }
            }
            return signInScreenLoaded;
        }

        /// <summary>
        /// Selects the workflow with the specified name from the default workflow menu.
        /// </summary>
        /// <param name="workflowName">The workflow name.</param>
        public void SelectWorkflow(string workflowName)
        {
            if (_masthead.WaitForBusyState(true, TimeSpan.FromSeconds(3)))
            {
                _masthead.WaitForBusyState(false, TimeSpan.FromSeconds(10));
            }
            _enhancedWorkflowApp.SelectWorkflow(workflowName);
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Enters the specified value for the specified workflow prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <param name="value">The value.</param>
        public void EnterPromptValue(string prompt, string value)
        {
            _enhancedWorkflowApp.SelectPrompt(prompt);

            _controlPanel.WaitForAvailable("#hpid-keyboard", TimeSpan.FromSeconds(4));

            _controlPanel.TypeOnVirtualKeyboard(value);
            _controlPanel.WaitForAvailable("#hpid-keyboard-key-done", TimeSpan.FromSeconds(5));
            _controlPanel.Press("#hpid-keyboard-key-done");
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            bool completedJob = false;
            string statusMsg = string.Empty;
            _enhancedWorkflowApp.WorkflowLogger = this.WorkflowLogger;

            if (executionOptions.JobBuildSegments > 1)
            {
                completedJob = ScanDocuments(executionOptions);
            }
            else
            {
                _enhancedWorkflowApp.ExecuteJob();
                _masthead.WaitForActiveJobsButtonState(true, TimeSpan.FromSeconds(3));
                statusMsg = _engine.ExecuteFunction("getStatusMessage").Trim('"');

                RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);

                if (string.IsNullOrEmpty(statusMsg)) // old way of finding the status
                {
                    completedJob = ProcessNoStatusBanner(executionOptions);
                }
                else
                {
                    completedJob = ProcessStatusBanner(statusMsg);

                }                
            }
            return completedJob;
        }

        private bool ProcessNoStatusBanner(ScanExecutionOptions executionOptions)
        {
            if (_popupManager.WaitForPopup("Contacting Quota server", TimeSpan.FromSeconds(2)))
            {
                _popupManager.HandleTemporaryPopup("Contacting Quota server", TimeSpan.FromSeconds(30));
            }

            _notificationPanel.WaitForNotDisplaying("Initiating job");
            _notificationPanel.WaitForNotDisplaying("Pending");
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            if (executionOptions.JobBuildSegments == 0)
            {
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                _notificationPanel.WaitForNotDisplaying(_idleTimeoutOffset, "Scanning");
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            }

            return MarkSendingJob(statusMsg: string.Empty);
        }

        private bool ProcessStatusBanner(string statusMsg)
        {
            while (statusMsg != "Scanning..." && statusMsg.Length < 17)
            {
                statusMsg = _engine.ExecuteFunction("getStatusMessage").Trim('"');
                Thread.Sleep(TimeSpan.FromMilliseconds(250));
            }
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            statusMsg = WhileScanningOnBanner(statusMsg);
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);

            return MarkSendingJob(statusMsg);
        }

        private string WhileScanningOnBanner(string statusMsg)
        {
            while (statusMsg == "Scanning...")
            {
                statusMsg = _engine.ExecuteFunction("getStatusMessage").Trim('"');
                Thread.Sleep(TimeSpan.FromMilliseconds(250));
            }

            return statusMsg;
        }

        private bool MarkSendingJob(string statusMsg)
        {
            bool completed = false;

            if (string.IsNullOrEmpty(statusMsg) || !statusMsg.Equals("Sending..."))
            {             
                _notificationPanel.WaitForDisplaying(TimeSpan.FromSeconds(5), "Sending...");
                statusMsg = "Sending...";
            }

            if (!string.IsNullOrEmpty(statusMsg))
            {
                if (statusMsg == "Sending...")
                {
                    RecordEvent(DeviceWorkflowMarker.SendingJobBegin);
                    while (statusMsg == "Sending...")
                    {
                        statusMsg = _engine.ExecuteFunction("getStatusMessage").Trim('"');
                        Thread.Sleep(TimeSpan.FromMilliseconds(250));
                    }
                    RecordEvent(DeviceWorkflowMarker.SendingJobEnd);
                }
            }
            else
            {
                RecordEvent(DeviceWorkflowMarker.SendingJobBegin);
                completed = _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);
                completed = _notificationPanel.WaitForNotDisplaying(_idleTimeoutOffset, "Sending...");
                RecordEvent(DeviceWorkflowMarker.SendingJobEnd);
            }
            statusMsg = _engine.ExecuteFunction("getStatusMessage").Trim('"');

            if (statusMsg.Contains("Job is successfully"))
            {
                completed = true;
            }
            else
            {
                throw new DeviceWorkflowException("Unknown DSS Workflow Status: '" + statusMsg + "'.");
            }
            return completed;
        }

        private bool ScanDocuments(ScanExecutionOptions executionOptions)
        {
            bool jobCompleted = false;
            RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
            string statusMsg = _engine.ExecuteFunction("getStatusMessage").Trim('"');
            if (string.IsNullOrEmpty(statusMsg))
            {
                JobBuildPages(executionOptions);               
            }
            else
            {
                JobBuildPages(executionOptions);
                statusMsg = _engine.ExecuteFunction("getStatusMessage").Trim('"');
                WhileScanningOnBanner(statusMsg);
            }
            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);

            jobCompleted = MarkSendingJob(_engine.ExecuteFunction("getStatusMessage").Trim('"'));

            return jobCompleted;
        }

        private void JobBuildPages(ScanExecutionOptions executionOptions)
        {
            _enhancedWorkflowApp.ExecuteJob();

            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            _controlPanel.WaitForAvailable("#hpid-button-scan");
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);


            for (int iTimes = 1; iTimes < executionOptions.JobBuildSegments; iTimes++)
            {
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                _controlPanel.WaitForAvailable("#hpid-button-scan");

                _controlPanel.ScrollPressWait("#hpid-button-scan", "#hpid-button-done", _idleTimeoutOffset);
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                Thread.Sleep(250);
            }
            // presses the send button
            _controlPanel.Press("#hpid-button-done");
        }

        /// <summary>
        /// Gets the <see cref="IDssWorkflowJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IDssWorkflowJobOptions Options => _enhancedWorkflowApp;
    }
}
