using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using System.Collections.Generic;
using System.Linq;


namespace HP.ScalableTest.DeviceAutomation.NativeApps.Fax
{
    /// <summary>
    /// Implementation of <see cref="IFaxApp" /> for a <see cref="JediWindjammerDevice" />.
    /// </summary>
    public sealed class JediWindjammerFaxApp : DeviceWorkflowLogSource, IFaxApp
    {
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
        private readonly JediWindjammerFaxJobOptions _optionsManager;
        private readonly JediWindjammerJobExecutionManager _executionManager;
        private Pacekeeper _pacekeeper;

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper
        {
            get
            {
                return _pacekeeper;
            }
            set
            {
                _pacekeeper = value;
                _optionsManager.Pacekeeper = _pacekeeper;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerFaxApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerFaxApp(JediWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediWindjammerFaxJobOptions(device);
            _executionManager = new JediWindjammerJobExecutionManager(device);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Fax application on the device.
        /// </summary>
        public void Launch()
        {
            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Fax");
                _controlPanel.ScrollPressNavigate("mAccessPointDisplay", "SendFaxApp", "SendFaxAppMainForm", ignorePopups: true);
                RecordEvent(DeviceWorkflowMarker.AppShown);
                Pacekeeper.Pause();
            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("Fax application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Fax application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "SendFaxAppMainForm":
                        // The application launched successfully. This happens sometimes.
                        RecordEvent(DeviceWorkflowMarker.AppShown);
                        Pacekeeper.Reset();
                        return;

                    case "SignInForm":
                        throw new DeviceWorkflowException("Sign-in required to launch the Fax application.", ex);

                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Could not launch Fax application: {message}", ex);

                    default:
                        throw new DeviceWorkflowException($"Could not launch Fax application: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Launches the FAX solution with the specified authenticator using the given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="DeviceWorkflowException">Fax application button was not found on device home screen.</exception>
        /// <exception cref="System.NotImplementedException">Eager authentication has not been implemented for this solution.</exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                if (PressFaxSolutionButton("SignInForm"))
                {
                    Authenticate(authenticator);
                }
            }
            else
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                Authenticate(authenticator);
                PressFaxSolutionButton("SendFaxAppMainForm");
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Presses the Fax button.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DeviceWorkflowException">
        /// Could not launch Email application.
        /// or
        /// Email application button was not found on device home screen.
        /// </exception>
        private bool PressFaxSolutionButton(string expectedForm)
        {
            bool signInScreenLoaded = false;

            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Fax");
                signInScreenLoaded = _controlPanel.ScrollPressWait("mAccessPointDisplay", "SendFaxApp", expectedForm, TimeSpan.FromSeconds(20));
                Pacekeeper.Pause();

                if (!signInScreenLoaded)
                {
                    if (_controlPanel.WaitForForm("SendFaxAppMainForm", StringMatch.Contains, TimeSpan.FromSeconds(30)) == true)
                    {
                        RecordEvent(DeviceWorkflowMarker.AppShown);
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Could not launch Fax application.");
                    }
                }
            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("Fax application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Fax application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "SendFaxAppMainForm":
                        // The application launched without prompting for credentials.
                        Pacekeeper.Reset();
                        break;
                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Could not launch Fax application: {message}", ex);

                    default:
                        throw new DeviceWorkflowException($"Could not launch Fax application: {ex.Message}", ex);
                }
            }

            return signInScreenLoaded;
        }
        /// <summary>
        /// Authenticates the user for DSS.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        private void Authenticate(IAuthenticator authenticator)
        {
            try
            {
                authenticator.Authenticate();
            }
            catch (WindjammerInvalidOperationException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                switch (currentForm)
                {
                    case "OxpUIAppMainForm":
                        // The application launched successfully. This happens sometimes.
                        RecordEvent(DeviceWorkflowMarker.AppShown);
                        break;
                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        if (message.StartsWith("Invalid", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new DeviceWorkflowException(string.Format("Could not launch application: {0}", message), ex);
                        }
                        else
                        {
                            throw new DeviceInvalidOperationException(string.Format("Could not launch application: {0}", message), ex);
                        }

                    default:
                        throw new DeviceInvalidOperationException(string.Format("Could not launch application: {0}", ex.Message), ex);
                }
            }
        }

        /// <summary>
        /// Adds the specified recipient for the fax.
        /// </summary>
        /// <param name="recipients">The recipients. Contains PINs, if used.</param>
        /// <param name="useSpeedDial">Uses the #s as speed dials</param>
        ///     useSpeedDial param would be used for later implementation
        public void AddRecipients(Dictionary<string, string> recipients, bool useSpeedDial)
        {
            _controlPanel.PressToNavigate("mFaxNumberTextBox", "HPNumericKeypad", ignorePopups: false);
            Pacekeeper.Pause();
            _controlPanel.Type(recipients.First().Key);
            Pacekeeper.Sync();
            _controlPanel.PressToNavigate("mButtonOK", "SendFaxAppMainForm", ignorePopups: false);
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            bool complete = _executionManager.ExecuteScanJob(executionOptions, "SendFaxAppMainForm");
            string currentForm = _controlPanel.CurrentForm();

            if (complete)
            {
                if (currentForm.Equals("BaseJobStartPopup"))
                {
                    _controlPanel.PressToNavigate("m_OKButton", "SendFaxAppMainForm", ignorePopups: true);
                }
            }
            else
            {
                if (currentForm.Equals(JediWindjammerLaunchHelper.HOMESCREEN_FORM))
                {
                    throw new DeviceWorkflowException("At home screen; expected Fax Application Main Form or the Base Job popup in order to press the Okay button.");
                }
            }

            return complete;
        }

        /// <summary>
        /// Retrieves the html String from Fax Report(not implemented for Windjammer)
        /// </summary>
        public string RetrieveFaxReport()
        {
            throw new NotImplementedException("Fax Report is yet to be supported for Jedi Windjammer");
        }

        /// <summary>
        /// Adds the recipient.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        public void AddRecipient(string recipient)
        {
            _controlPanel.PressToNavigate("mFaxNumberTextBox", "HPNumericKeypad", ignorePopups: false);
            Pacekeeper.Pause();
            _controlPanel.Type(recipient);
            Pacekeeper.Sync();
            _controlPanel.PressToNavigate("mButtonOK", "SendFaxAppMainForm", ignorePopups: false);
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Gets the <see cref="IFaxJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IFaxJobOptions Options => _optionsManager;

        private class JediWindjammerFaxJobOptions : JediWindjammerJobOptionsManager, IFaxJobOptions
        {
            public JediWindjammerFaxJobOptions(JediWindjammerDevice device)
                : base(device, "SendFaxAppMainForm")
            {
            }
        }

    }
}
