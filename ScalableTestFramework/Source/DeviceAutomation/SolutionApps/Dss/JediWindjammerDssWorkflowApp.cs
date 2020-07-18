using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Dss
{
    /// <summary>
    /// Implementation of <see cref="IDssWorkflowApp" /> for a <see cref="JediWindjammerDevice" />.
    /// </summary>
    public sealed class JediWindjammerDssWorkflowApp : DeviceWorkflowLogSource, IDssWorkflowApp
    {
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
        private readonly DssEnhancedWorkflowApp _enhancedWorkflowApp;
        private readonly JediWindjammerDssWorkflowJobOptions _optionsManager;
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
        /// Initializes a new instance of the <see cref="JediWindjammerDssWorkflowApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerDssWorkflowApp(JediWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _enhancedWorkflowApp = new DssEnhancedWorkflowApp(_controlPanel);
            _optionsManager = new JediWindjammerDssWorkflowJobOptions(device);
            _executionManager = new JediWindjammerJobExecutionManager(device);
            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Workflow application on the device.
        /// </summary>
        public void Launch()
        {
            string appButton = FindWorkflowAppButton();
            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "DssWorkflow");
                bool launched = _controlPanel.ScrollPressWait("mAccessPointDisplay", appButton, "OxpBaseForm|OxpUIAppMainForm.*", StringMatch.Regex);
                if (launched)
                {
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                    Pacekeeper.Pause();
                }
                else
                {
                    string currentForm = _controlPanel.CurrentForm();
                    switch (currentForm)
                    {
                        case "SignInForm":
                            throw new DeviceWorkflowException("Sign-in required to launch the Workflow application.");

                        case "OneButtonMessageBox":
                        case "OxpDialogForm":
                            string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                            throw new DeviceWorkflowException($"Could not launch Workflow application: {message}");

                        default:
                            throw new DeviceWorkflowException($"Could not launch Workflow application. Current form: {currentForm}");
                    }
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                throw new DeviceWorkflowException($"Could not launch Workflow application: {ex.Message}", ex);
            }

            if (_controlPanel.CurrentForm() == "OxpBaseForm")
            {
                // Legacy workflow - wait for the workflows to finish loading
                Wait.ForNotNull(FindWorkflowMenuTree, TimeSpan.FromSeconds(10));
            }
        }

        /// <summary>
        /// Signs into the WindJammer Device using the IAuthenticator using the AuthenticationMode method
        /// </summary>
        /// <param name="authenticator"></param>
        /// <param name="authenticationMode"></param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            string desiredFrom = JediWindjammerLaunchHelper.OxpdBaseForm + "|" + JediWindjammerLaunchHelper.OxpdFormIdentifier + ".*";

            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                Authenticate(authenticator);
                PressDssSolutionButton();
            }
            else // AuthenticationMode.Lazy
            {
                if (PressDssSolutionButton())
                {
                    Authenticate(authenticator);
                }
            }
            
            if (_controlPanel.WaitForForm(desiredFrom, StringMatch.Regex, TimeSpan.FromSeconds(30)) == true)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
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
                    case JediWindjammerLaunchHelper.OxpdFormIdentifier:
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

        private bool PressDssSolutionButton()
        {
            bool promptedForCredentials = true;
            string appButton = FindWorkflowAppButton();
            string desiredFrom = JediWindjammerLaunchHelper.OxpdBaseForm + "|" + JediWindjammerLaunchHelper.SIGNIN_FORM + "|" + JediWindjammerLaunchHelper.OxpdFormIdentifier + ".*";
            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "DSS Workflow");
                _controlPanel.ScrollPressWait(JediWindjammerLaunchHelper.ScrollAccessPointDisplay, appButton, desiredFrom, StringMatch.Regex);

                string formName = _controlPanel.CurrentForm();

                if (formName.Equals(JediWindjammerLaunchHelper.OxpdBaseForm) || formName.Contains(JediWindjammerLaunchHelper.OxpdFormIdentifier))
                {
                    promptedForCredentials = false;
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                }

                Pacekeeper.Pause();
            }
            catch (WindjammerInvalidOperationException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                desiredFrom = JediWindjammerLaunchHelper.OxpdBaseForm + "|" + JediWindjammerLaunchHelper.OxpdFormIdentifier + ".*";

                //"OxpBaseForm|OxpUIAppMainForm.*"

                if (StringMatcher.IsMatch(desiredFrom, currentForm, StringMatch.Regex, false))
                {
                    // The application launched without prompting for credentials.
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                    Pacekeeper.Reset();
                    promptedForCredentials = false;
                }
                else
                {
                    switch (currentForm)
                    {
                        case "OneButtonMessageBox":
                        case "OxpDialogForm":
                            string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                            throw new DeviceWorkflowException($"Could not launch Workflow application: {message}", ex);

                        default:
                            throw new DeviceWorkflowException($"Could not launch Workflow application: {ex.Message}", ex);
                    }
                }
            }

            return promptedForCredentials;
        }

        private string FindWorkflowAppButton()
        {
            // The button name will be a GUID, but always starts with the same sequence.
            string appButton = _controlPanel.GetControls().FirstOrDefault(n => n.StartsWith("1c043000"));
            if (appButton != null)
            {
                return appButton;
            }
            else
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("DSS Workflow application button was not found on device home screen.");
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the DSS Workflow application from {currentForm}.");
                }
            }
        }

        /// <summary>
        /// Selects the workflow with the specified name from the default workflow menu.
        /// </summary>
        /// <param name="workflowName">The workflow name.</param>
        public void SelectWorkflow(string workflowName)
        {
            if (_controlPanel.CurrentForm() == "OxpBaseForm")
            {
                SelectWorkflowLegacy(workflowName);
            }
            else
            {
                _enhancedWorkflowApp.SelectWorkflow(workflowName);
            }
            Pacekeeper.Pause();
        }

        private void SelectWorkflowLegacy(string workflowName)
        {
            string menuTree = FindWorkflowMenuTree();
            if (menuTree == null)
            {
                throw new DeviceWorkflowException($"Could not find workflow list. Current form: {_controlPanel.CurrentForm()}");
            }

            try
            {
                _controlPanel.ScrollPress(menuTree, "Text", workflowName);
            }
            catch (ControlNotFoundException ex)
            {
                if (_controlPanel.CurrentForm() == "OxpBaseForm")
                {
                    throw new DeviceWorkflowException($"Could not find workflow named '{workflowName}'.", ex);
                }
                else
                {
                    throw;
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                if (ex.Message.StartsWith("Did not receive the Click event", StringComparison.OrdinalIgnoreCase))
                {
                    // Ignore this - it means the new set of controls loaded too fast for the Click event
                    // to be caught by the UI, but does not generally indicate that something went wrong.
                }
                else
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Enters the specified value for the specified workflow prompt.
        /// </summary>
        /// <param name="prompt">The prompt.</param>
        /// <param name="value">The value.</param>
        public void EnterPromptValue(string prompt, string value)
        {
            string currentForm = _controlPanel.CurrentForm();
            if (currentForm == "OxpBaseForm")
            {
                EnterPromptValueLegacy(prompt, value);
            }
            else
            {
                _enhancedWorkflowApp.SelectPrompt(prompt);
                Pacekeeper.Pause();
                _controlPanel.TypeOnVirtualKeyboard("mKeyboard", value);
                Pacekeeper.Sync();
                _controlPanel.PressToNavigate("ok", currentForm, ignorePopups: false);
            }
            Pacekeeper.Pause();
        }

        private void EnterPromptValueLegacy(string prompt, string value)
        {
            // Get a list of the displayed fields and their corresponding text box controls
            Dictionary<string, string> fields = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);
            foreach (string label in _controlPanel.GetControls().Where(n => n.StartsWith("OxpDataEntryControl", StringComparison.OrdinalIgnoreCase)
                                                                         && n.EndsWith("Label", StringComparison.OrdinalIgnoreCase)))
            {
                string labelText = RemoveWhiteSpace(_controlPanel.GetProperty(label, "Text").TrimStart('*'));
                string textBox = label.Replace("_Label", "_TextBox");
                fields.Add(labelText, textBox);
            }

            string promptMatch = StringMatcher.FindBestMatch(prompt, fields.Keys, StringMatch.Contains, ignoreCase: true, ignoreWhiteSpace: true);
            if (promptMatch != null)
            {
                _controlPanel.PressToNavigate(fields[promptMatch], "OxpDataEntryKeyboardForm", ignorePopups: false);
                Pacekeeper.Pause();
                _controlPanel.TypeOnVirtualKeyboard("m_Keyboard", value);
                Pacekeeper.Sync();
                _controlPanel.PressToNavigate("ok", "OxpBaseForm", ignorePopups: false);
            }
            else
            {
                throw new DeviceWorkflowException($"Could not find workflow prompt '{prompt}'.");
            }
        }

        private string FindWorkflowMenuTree()
        {
            return _controlPanel.GetControls().FirstOrDefault(n => n.EndsWith("mMenuTree", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {

            bool completedJob = false;
            if (_controlPanel.CurrentForm() == JediWindjammerLaunchHelper.OxpdBaseForm)
            {
                _executionManager.WorkflowLogger = WorkflowLogger;

                completedJob = _executionManager.ExecuteScanJob(executionOptions, JediWindjammerLaunchHelper.OxpdBaseForm);

                if (_controlPanel.CurrentForm() == "OxpDialogForm")
                {
                    // Clear the OXP confirmation dialog
                    _controlPanel.PressToNavigate("m_OKButton", "OxpBaseForm", ignorePopups: true);
                    completedJob = true;
                }
            }
            else
            {
                _enhancedWorkflowApp.WorkflowLogger = WorkflowLogger;

                // job build process
                if (executionOptions.JobBuildSegments > 1)
                {
                    completedJob = ScanDocuments(executionOptions);
                }
                else  // scanning one document 
                {
                    completedJob = _enhancedWorkflowApp.ExecuteJob(_controlPanel);

                }
            }
            return completedJob;
        }

        /// <summary>
        /// Scans the documents.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        private bool ScanDocuments(ScanExecutionOptions executionOptions)
        {
            bool completedJob = false;

            RecordEvent(DeviceWorkflowMarker.JobBuildBegin);

            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            _enhancedWorkflowApp.ExecuteJob();

            for (int pc = 2; pc <= executionOptions.JobBuildSegments; pc++)
            {
                //Wait for the job build form

                _controlPanel.WaitForForm("JobBuildPrompt", false);
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                PressOkButton(1);
            }

            //Wait for the job build form
            _controlPanel.WaitForForm("JobBuildPrompt", false);
            _controlPanel.Press("mFinishButton");

            string script = "document.getElementById('statusPopDown').getElementsByTagName('label')[0].innerHTML";
            string status = _controlPanel.ExecuteJavaScript(script);

            if (status.Contains("Scanning"))
            {
                while (status.Contains("Scanning"))
                {
                    Thread.Sleep(100);
                    status = _controlPanel.ExecuteJavaScript(script);
                }
            }

            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);

            // Check for sending...


            if (status.Contains("Sending"))
            {
                RecordEvent(DeviceWorkflowMarker.SendingJobBegin);
                while (status.Contains("Sending"))
                {
                    Thread.Sleep(100);
                    status = _controlPanel.ExecuteJavaScript(script);
                }
                RecordEvent(DeviceWorkflowMarker.SendingJobEnd);
            }

            if (status.ToLower().Contains("success"))
            {
                completedJob = true;
            }

            return completedJob;
        }

        /// <summary>
        /// Presses the ok button on the scan form.
        /// </summary>
        /// <param name="numTimes">The number times.</param>
        private void PressOkButton(int numTimes)
        {
            try
            {
                _controlPanel.Press("m_OKButton");
            }
            catch (Exception e)
            {
                if (e.Message.Contains("m_OkButton"))
                {
                    if (numTimes < 4)
                    {
                        PressOkButton(++numTimes);
                    }
                    else
                    {
                        throw;
                    }
                }

            }
        }

        /// <summary>
        /// Gets the <see cref="IDssWorkflowJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IDssWorkflowJobOptions Options
        {
            get
            {
                if (_controlPanel.CurrentForm() == "OxpBaseForm")
                {
                    return _optionsManager;
                }
                else
                {
                    return _enhancedWorkflowApp;
                }

            }
        }

        private static string RemoveWhiteSpace(string value)
        {
            if (value != null)
            {
                return Regex.Replace(value, @"\s+", string.Empty);
            }
            else
            {
                return null;
            }
        }

        private class JediWindjammerDssWorkflowJobOptions : JediWindjammerJobOptionsManager, IDssWorkflowJobOptions
        {
            public JediWindjammerDssWorkflowJobOptions(JediWindjammerDevice device)
                : base(device, "OxpBaseForm")
            {
            }
        }
    }
}
