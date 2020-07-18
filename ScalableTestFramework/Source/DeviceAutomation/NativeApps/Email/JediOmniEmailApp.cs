using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.DeviceAutomation;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Email
{
    /// <summary>
    /// Implementation of <see cref="IEmailApp" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public sealed class JediOmniEmailApp : DeviceWorkflowLogSource, IEmailApp
    {
        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniEmailJobOptions _optionsManager;
        private readonly JediOmniJobExecutionManager _executionManager;
        private readonly JediOmniLaunchHelper _launchHelper;
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
                _launchHelper.Pacekeeper = _pacekeeper;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniEmailApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniEmailApp(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediOmniEmailJobOptions(device);
            _executionManager = new JediOmniJobExecutionManager(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Email application on the device.      
        /// </summary>
        public void Launch()
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = PressScanToEmailButton("#hpid-email-app-screen");
            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                if (_controlPanel.CheckState("#hpid-signin-app-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException("Sign-in required to launch the Email application.");
                }
                else
                {
                    //the error seen here is mostly due to incorrect configuration and would be displaying a modal dialog to configure, let's cancel and exit
                    if (_controlPanel.CheckState("#hpid-setup-wizard-configuration-popup-cancel-button", OmniElementState.Exists))
                    {
                        _controlPanel.Press("#hpid-setup-wizard-configuration-popup-cancel-button");
                    }
                    throw new DeviceWorkflowException("Could not launch Email application.");
                }
            }
        }

        /// <summary>
        /// Launches the Email application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = false;
            if (authenticationMode == AuthenticationMode.Lazy)
            {
                bool signInScreenLoaded = PressScanToEmailButton("#hpid-signin-body");
                if (signInScreenLoaded)
                {
                    appLoaded = Authenticate(authenticator, "#hpid-email-landing-page");
                }
                else if (_controlPanel.CheckState("#hpid-email-app-screen", OmniElementState.Exists))
                {
                    // The application launched without prompting for credentials
                    appLoaded = true;
                    Pacekeeper.Reset();
                }
            }
            else
            {
                if (authenticator.Provider != AuthenticationProvider.Card)
                {
                    _launchHelper.PressSignInButton();
                }
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                appLoaded = PressScanToEmailButton("#hpid-email-app-screen");
            }

            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                throw new DeviceWorkflowException("Could not launch Email application.");
            }
        }

        private bool PressScanToEmailButton(string expectedDestination)
        {
            return _launchHelper.PressAppButton("Email", "#hpid-email-homescreen-button", "#hpid-scan-homescreen-button", expectedDestination);
        }

        /// <summary>
        /// Launches the specified authenticator.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quick set.</param>
        public void LaunchFromQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            if (authenticationMode == AuthenticationMode.Lazy)
            {
                if (PressQuickSetButton(quickSetName))
                {
                    Authenticate(authenticator, "#hpid-email-landing-page");
                }
            }
            else // AuthenticationMode.Eager
            {
                if (authenticator.Provider != AuthenticationProvider.Card)
                {
                    _launchHelper.PressSignInButton();
                }
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                PressQuickSetButton(quickSetName);
            }
        }

        private bool PressQuickSetButton(string quickSetName)
        {
            bool signInScreenLoaded = false;

            // Determine whether the Quickset Button is on the home screen 
            if (_controlPanel.CheckState("#hpid-quicksets-homescreen-button", OmniElementState.Exists))
            {
                _controlPanel.ScrollPressWait("#hpid-quicksets-homescreen-button", "#hpid-quicksets-app-screen", TimeSpan.FromSeconds(2));

                //Since ".hp-homescreen-button:contains(" + quickSetName + ")" " was not useable,we went with retrieving the contol Id at runtime.
                string elementId = ".hp-homescreen-button:contains(" + quickSetName + ")";
                List<string> controls = _controlPanel.GetIds(elementId, OmniIdCollectionType.Self).ToList();

                if (controls.Count > 0)
                {
                    elementId = "#" + controls.Last();
                    signInScreenLoaded = _controlPanel.ScrollPressWait(elementId, "#hpid-signin-body", TimeSpan.FromSeconds(30));
                    Pacekeeper.Pause();
                }
                else
                {
                    throw new DeviceWorkflowException($"Could not find quickset with text '{quickSetName}'.");
                }
            }
            else
            {
                throw new DeviceWorkflowException("Could not launch Quickset.");
            }

            if (!signInScreenLoaded)
            {
                if (_controlPanel.CheckState("#hpid-email-app-screen", OmniElementState.Exists))
                {
                    // The application launched without prompting for credentials
                    Pacekeeper.Reset();
                }
                else
                {
                    throw new DeviceWorkflowException("Could not launch Email application.");
                }
            }

            return signInScreenLoaded;
        }

        private bool Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            bool formLoaded = _controlPanel.WaitForAvailable(waitForm);
            Pacekeeper.Pause();
            return formLoaded;
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="quickSetName">Name of the Quickset if exists or else empty/null</param>
        /// </summary>  
        public void SelectQuickset(string quickSetName)
        {
            System.Threading.Thread.Sleep(250);
            _controlPanel.ScrollPressWait(".hp-load-quickset-button", "#hpid-quicksets-list");
            Pacekeeper.Sync();
            if (_controlPanel.CheckState(".hp-listitem:contains(" + quickSetName + ")", OmniElementState.Exists))
            {
                string fileTypeItemSelector = ".hp-listitem-text:contains(" + quickSetName + ")";
                int matches = _controlPanel.GetCount(fileTypeItemSelector);
                PressQuickSetItem(quickSetName, fileTypeItemSelector, matches);
                Pacekeeper.Sync();
                _controlPanel.Press("#hpid-quicksets-load-button");
                Pacekeeper.Sync();
            }
            else
            {
                throw new DeviceWorkflowException($"Could not find quickset with text '{quickSetName}'.");
            }
        }

        /// <summary>
        /// Matches the quicksets and presses the list item
        /// </summary>
        /// <param name="quicksetName">The QuickSet Name</param>
        /// <param name="listItemSelector">List of Items related to the passed QuickSet Name</param>
        /// <param name="matches">Count of matched similar QuickSet</param>
        private void PressQuickSetItem(string quicksetName, string listItemSelector, int matches)
        {
            for (int i = 0; i < matches; i++)
            {
                string specificListItem = $"{listItemSelector}:eq({i})";
                if (quicksetName.Equals(_controlPanel.GetValue(specificListItem, "innerText", OmniPropertyType.Property)))
                {
                    _controlPanel.PressWait(specificListItem, "#hpid-quicksets-load-button");
                }

            }
        }

        /// <summary>
        /// Enters the "To" email address.
        /// </summary>
        /// <param name="emailAddresses">The "To" email addresses.</param>
        public void EnterToAddress(IEnumerable<string> emailAddresses)
        {
            // currently the CC and BCC address are not entered.
            // Omni values are hpid-cc-recipient and hpid-bcc-recipient
            _controlPanel.WaitForState("#hpid-to-recipient-textbox", OmniElementState.Exists, TimeSpan.FromSeconds(6));
            //To check if ScanToMe feature is enabled in Email app. (ScanToMe feature is configured from DSS Server)
            if (!_controlPanel.CheckState("#hpid-to-recipient-textbox", OmniElementState.Constrained))
            {
                _controlPanel.ScrollPressWait("#hpid-to-recipient-textbox", "#hpid-keyboard");
                Pacekeeper.Sync();
                foreach (string address in emailAddresses)
                {
                    _controlPanel.TypeOnVirtualKeyboard(address);
                    _device.ControlPanel.Type(SpecialCharacter.Enter);
                    Pacekeeper.Sync();
                }
            }
        }

        /// <summary>
        /// Enters the email subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        public void EnterSubject(string subject)
        {
            _controlPanel.ScrollPressWait("#hpid-subject", "#hpid-keyboard");
            Pacekeeper.Sync();
            _controlPanel.TypeOnVirtualKeyboard(subject);
            Pacekeeper.Sync();
        }

        /// <summary>
        /// Enters the name to use for the scanned file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void EnterFileName(string fileName)
        {
            _controlPanel.ScrollPressWait("#hpid-file-name", "#hpid-keyboard");
            Pacekeeper.Sync();
            _controlPanel.TypeOnVirtualKeyboard(fileName);
            Pacekeeper.Sync();
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            return _executionManager.ExecuteScanJob(executionOptions, "#hpid-button-email-start");
        }

        /// <summary>
        /// Gets the <see cref="IEmailJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IEmailJobOptions Options => _optionsManager;

        private class JediOmniEmailJobOptions : JediOmniJobOptionsManager, IEmailJobOptions
        {
            public JediOmniEmailJobOptions(JediOmniDevice device)
                : base(device)
            {
            }
        }
    }
}