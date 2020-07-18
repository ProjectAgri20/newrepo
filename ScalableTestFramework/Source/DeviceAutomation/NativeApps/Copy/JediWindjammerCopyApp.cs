using System;
using System.Collections.Generic;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Copy
{
    /// <summary>
    /// Implementation of <see cref="ICopyApp" /> for a <see cref="JediWindjammerDevice" />.
    /// </summary>
    public sealed class JediWindjammerCopyApp : DeviceWorkflowLogSource, ICopyApp
    {
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
        private readonly JediWindjammerCopyJobOptionsManager _optionsManager;
        private readonly JediWindjammerJobExecutionManager _executionManager;

        /// <summary>
        /// Gets or sets the pacekeeper for this application.
        /// </summary>
        /// <value>The pacekeeper.</value>
        public Pacekeeper Pacekeeper { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerCopyApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerCopyApp(JediWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediWindjammerCopyJobOptionsManager(device);
            _executionManager = new JediWindjammerJobExecutionManager(device);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Copy application on the device.
        /// </summary>       
        public void Launch()
        {
            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Copy");
                _controlPanel.ScrollPressNavigate("mAccessPointDisplay", "Copy", "CopyAppMainForm", true);
                RecordEvent(DeviceWorkflowMarker.AppShown);
                Pacekeeper.Pause();
            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("Copy application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Copy application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "CopyAppMainForm":
                        // The application launched without prompting for credentials.
                        RecordEvent(DeviceWorkflowMarker.AppShown);
                        Pacekeeper.Reset();
                        return;

                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Could not launch Copy application: {message}", ex);

                    default:
                        throw new DeviceWorkflowException($"Could not launch Copy application: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Launches the Copy application using the specified authenticator, authentication mode, and quickset.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quick set.</param>
        public void LaunchFromQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);
                PressCopyWithQuickset(quickSetName);
            }
            else // AuthenticationMode.Lazy
            {
                if (PressCopyWithQuickset(quickSetName))
                {
                    // authentication required
                    Authenticate(authenticator, "CopyAppMainForm");
                }
            }
        }
        /// <summary>
        /// Launches the Copy application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);
                PressCopySolutionButton("CopyAppMainForm");
            }
            else // AuthenticationMode.Lazy
            {
                if (PressCopySolutionButton("SignInForm"))
                {
                    // authentication required
                    Authenticate(authenticator, "CopyAppMainForm");
                }
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }
        /// <summary>
        /// Presses the copy with quickset.
        /// </summary>
        /// <param name="quickSetName">Name of the quick set.</param>
        /// <returns></returns>
        /// <exception cref="DeviceWorkflowException">
        /// Could not launch copy application.
        /// or
        /// Copy application button was not found on device home screen.
        /// or
        /// or
        /// or
        /// </exception>
        private bool PressCopyWithQuickset(string quickSetName)
        {
            bool signInScreenLoaded;

            try
            {
                if (!string.IsNullOrEmpty(quickSetName))
                {
                    if (ExistQuickSetButton(quickSetName))
                    {
                        // on a front page
                        signInScreenLoaded = _controlPanel.ScrollPressWait("mAccessPointDisplay", "Title", quickSetName, JediWindjammerLaunchHelper.SIGNIN_FORM, TimeSpan.FromSeconds(7));
                    }
                    else // in the quickset folder
                    {
                        _controlPanel.PressToNavigate("NLevelApp", "NLevelHomeScreenForm", true);
                        signInScreenLoaded = _controlPanel.PressWait(quickSetName, "SignInForm", TimeSpan.FromSeconds(7));
                    }
                }
                else
                {
                    signInScreenLoaded = _controlPanel.PressWait("CopyApp", "SignInForm", TimeSpan.FromSeconds(1));
                }
                if (!signInScreenLoaded)
                {
                    if (!_controlPanel.WaitForForm("CopyAppMainForm", TimeSpan.FromSeconds(3)))
                    {
                        throw new DeviceWorkflowException("Could not launch copy application.");
                    }
                }
                RecordEvent(DeviceWorkflowMarker.QuickSetListReady);
            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("Copy application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Copy application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "CopyAppMainForm":
                        // The application launched without prompting for credentials.
                        RecordEvent(DeviceWorkflowMarker.AppShown);
                        Pacekeeper.Reset();
                        return false;

                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Could not launch Copy application: {message}", ex);

                    default:
                        throw new DeviceWorkflowException($"Could not launch Copy application: {ex.Message}", ex);
                }
            }

            return signInScreenLoaded;
        }
        private bool ExistQuickSetButton(string quicksetName)
        {
            bool exist = false;

            IEnumerable<string> controls = _controlPanel.GetControls();
            foreach (string ctlr in controls)
            {
                if (ctlr.Equals(quicksetName))
                {
                    exist = true;
                    break;
                }
            }

            return exist;
        }
        /// <summary>
        /// Presses the copy solution button.
        /// </summary>
        /// <returns></returns>
        /// <exception cref="DeviceWorkflowException">
        /// Could not launch copy application.
        /// or
        /// Copy application button was not found on device home screen.
        /// or
        /// or
        /// or
        /// </exception>
        private bool PressCopySolutionButton(string expectedForm)
        {
            bool signInScreenLoaded;

            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Copy");
                signInScreenLoaded = _controlPanel.PressWait("CopyApp", expectedForm, TimeSpan.FromSeconds(3));

                if (!signInScreenLoaded)
                {
                    if (!_controlPanel.WaitForForm("CopyAppMainForm", TimeSpan.FromSeconds(3)))
                    {
                        throw new DeviceWorkflowException("Could not launch copy application.");
                    }
                }

            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("Copy application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Copy application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "CopyAppMainForm":
                        // The application launched without prompting for credentials.
                        Pacekeeper.Reset();
                        return false;

                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Could not launch Copy application: {message}", ex);

                    default:
                        throw new DeviceWorkflowException($"Could not launch Copy application: {ex.Message}", ex);
                }
            }
            return signInScreenLoaded;
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
        /// Selects the folder quickset with the specified name.
        /// <param name="quickSetName">Name of the Quickset</param>
        /// </summary>      
        public void SelectQuickSet(string quickSetName)
        {
            throw new NotSupportedException("Quick Sets from the copy screen is Not supported on Jedi Windjammer devices.");
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            var complete = _executionManager.ExecuteScanJob(executionOptions, "CopyAppMainForm");
            if (complete && _controlPanel.CurrentForm() == "BaseJobStartPopup")
            {
                _controlPanel.PressToNavigate("m_OKButton", "CopyAppMainForm", true);
            }
            return complete;
        }

        /// <summary>
        /// Gets the <see cref="ICopyJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public ICopyJobOptions Options => _optionsManager;


    }

}
