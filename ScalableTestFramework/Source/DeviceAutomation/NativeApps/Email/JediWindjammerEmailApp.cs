using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Email
{
    /// <summary>
    /// Implementation of <see cref="IEmailApp" /> for a <see cref="JediWindjammerDevice" />.
    /// </summary>
    public sealed class JediWindjammerEmailApp : DeviceWorkflowLogSource, IEmailApp
    {
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
        private readonly JediWindjammerEmailJobOptions _optionsManager;
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
        /// Initializes a new instance of the <see cref="JediWindjammerEmailApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerEmailApp(JediWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediWindjammerEmailJobOptions(device);
            _executionManager = new JediWindjammerJobExecutionManager(device);
            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Email application on the device.     
		/// </summary>        
        public void Launch()
        {
            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Email");
                _controlPanel.ScrollPressNavigate("mAccessPointDisplay", "EmailApp", "EmailForm", ignorePopups: true);
                RecordEvent(DeviceWorkflowMarker.AppShown);
                Pacekeeper.Pause();
            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("Email application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Email application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "EmailForm":
                        // The application launched successfully. This happens sometimes.
                        RecordEvent(DeviceWorkflowMarker.AppShown);
                        Pacekeeper.Reset();
                        return;

                    case "SignInForm":
                        throw new DeviceWorkflowException("Sign-in required to launch the Email application.", ex);

                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Could not launch Email application: {message}", ex);

                    default:
                        throw new DeviceWorkflowException($"Could not launch Email application: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Presses the scan to email button.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DeviceWorkflowException">
        /// Could not launch Email application.
        /// or
        /// Email application button was not found on device home screen.
        /// </exception>
        private bool PressScanToEmailButton(string expectedForm, TimeSpan timeSpan)
        {
            bool signInScreenLoaded = false;

            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Email");
                signInScreenLoaded = _controlPanel.PressWait("EmailApp", expectedForm, timeSpan);
                Pacekeeper.Pause();

                if (!signInScreenLoaded)
                {
                    if (_controlPanel.WaitForForm("EmailForm", TimeSpan.FromSeconds(3)))
                    {
                        // The application launched without prompting for credentials
                        Pacekeeper.Reset();
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Could not launch Email application.");
                    }
                }

            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("Email application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Email application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "EmailForm":
                        // The application launched without prompting for credentials.
                        Pacekeeper.Reset();
                        signInScreenLoaded = false;
                        break;
                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        if (message.StartsWith("Invalid", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new DeviceWorkflowException($"Could not launch Email application: {message}", ex);
                        }
                        else
                        {
                            throw new DeviceWorkflowException($"Could not launch Email application: {message}", ex);
                        }
                    default:
                        throw new DeviceWorkflowException($"Could not launch Email application: {ex.Message}", ex);
                }
            }
            return signInScreenLoaded;
        }

        /// <summary>
        /// Presses the scan to email quick set.
        /// </summary>
        /// <param name="quickSetName">Name of the quick set.</param>
        /// <returns></returns>
        /// <exception cref="DeviceWorkflowException">
        /// Could not launch Email application.
        /// or
        /// Email application button was not found on device home screen.
        /// </exception>
        private bool PressScanToEmailQuickSet(string quickSetName)
        {
            bool signInScreenLoaded = false;

            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Email");
                if (!string.IsNullOrEmpty(quickSetName))
                {
                    _controlPanel.PressToNavigate("NLevelApp", "NLevelHomeScreenForm", ignorePopups: true);
                    signInScreenLoaded = _controlPanel.PressWait(quickSetName, "SignInForm", TimeSpan.FromSeconds(1));
                }
                else
                {
                    signInScreenLoaded = _controlPanel.PressWait("EmailApp", "SignInForm", TimeSpan.FromSeconds(1));
                }
                if (!signInScreenLoaded)
                {
                    if (_controlPanel.WaitForForm("EmailForm", TimeSpan.FromSeconds(1)))
                    {
                        // The application launched without prompting for credentials
                        Pacekeeper.Reset();
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Could not launch Email application.");
                    }
                }
                Pacekeeper.Pause();
            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("Email application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Email application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "EmailForm":
                        // The application launched without prompting for credentials.
                        RecordEvent(DeviceWorkflowMarker.AppShown);
                        Pacekeeper.Reset();
                        signInScreenLoaded = false;
                        break;
                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        if (message.StartsWith("Invalid", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new DeviceWorkflowException($"Could not launch Email application: {message}", ex);
                        }
                        else
                        {
                            throw new DeviceWorkflowException($"Could not launch Email application: {message}", ex);
                        }

                    default:
                        throw new DeviceWorkflowException($"Could not launch Email application: {ex.Message}", ex);
                }
            }
            return signInScreenLoaded;
        }

        /// <summary>
        /// Launches the Copy application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                if (PressScanToEmailButton("SignInForm", TimeSpan.FromSeconds(6)))
                {
                    Authenticate(authenticator, "EmailForm");
                }
            }
            else  // AuthenticationMode.Eager
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);
                PressScanToEmailButton("EmailForm", TimeSpan.FromSeconds(3));
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Launches the Copy application using the specified authenticator, authentication mode, and quickset.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quick set.</param>
        /// <exception cref="DeviceWorkflowException">Not supported in Jedi Windjammer device</exception>
        public void LaunchFromQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                if (PressScanToEmailQuickSet(quickSetName))
                {
                    Authenticate(authenticator, "EmailForm");
                }
            }
            else  // AuthenticationMode.Eager
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);
                PressScanToEmailQuickSet(quickSetName);
            }
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="quickSetName">Name of the Quickset if exists or else empty/null</param>
        /// </summary>  
        public void SelectQuickset(string quickSetName)
        {
            throw new NotImplementedException("Not supported in Jedi Windjammer device");
        }

        /// <summary>
        /// Authenticates the user for DSS.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">desired form after action</param>
        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            try
            {
                authenticator.Authenticate();

                _controlPanel.WaitForForm(waitForm, StringMatch.Contains, TimeSpan.FromSeconds(30));
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
        /// Enters the "To" email address.
        /// </summary>
        /// <param name="emailAddresses">The "To" email addresses.</param>
        public void EnterToAddress(IEnumerable<string> emailAddresses)
        {
            foreach (string address in emailAddresses)
            {
                EnterParameter("mToTextBox", address, "To address");
            }
        }

        /// <summary>
        /// Enters the email subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        public void EnterSubject(string subject)
        {
            EnterParameter("mSubjectTextBox", subject, "Subject");
        }

        /// <summary>
        /// Enters the name to use for the scanned file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void EnterFileName(string fileName)
        {
            EnterParameter("mFileNameTextBox", fileName, "File name");
        }

        private void EnterParameter(string control, string value, string parameterName)
        {
            try
            {
                _controlPanel.ScrollPressNavigate("mTextBoxPanel", control, "EmailKeyboardForm", ignorePopups: false);
                Pacekeeper.Pause();
                _controlPanel.TypeOnVirtualKeyboard("mKeyboard", value);
                Pacekeeper.Sync();

                // Pause for a moment to make sure that any auto-complete popups are gone
                System.Threading.Thread.Sleep(1000);

                _controlPanel.PressToNavigate("ok", "EmailForm", ignorePopups: false);
                Pacekeeper.Pause();
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "EmailForm":
                        if (_controlPanel.GetProperty<bool>(control, "Enabled") == false)
                        {
                            throw new DeviceWorkflowException($"{parameterName} is not user-editable.", ex);
                        }
                        else
                        {
                            throw;
                        }

                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Error entering {parameterName}: {message}", ex);

                    default:
                        throw;
                }
            }
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            bool complete = _executionManager.ExecuteScanJob(executionOptions, "EmailForm");
            if (complete && _controlPanel.CurrentForm() == "BaseJobStartPopup")
            {
                _controlPanel.PressToNavigate("m_OKButton", "EmailForm", ignorePopups: true);
            }
            return complete;
        }

        /// <summary>
        /// Gets the <see cref="IEmailJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IEmailJobOptions Options => _optionsManager;

        private class JediWindjammerEmailJobOptions : JediWindjammerJobOptionsManager, IEmailJobOptions
        {
            public JediWindjammerEmailJobOptions(JediWindjammerDevice device)
                : base(device, "EmailForm")
            {
            }
        }
    }
}
