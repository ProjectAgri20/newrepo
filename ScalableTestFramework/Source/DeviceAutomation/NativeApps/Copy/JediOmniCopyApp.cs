using System;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Copy
{
    /// <summary>
    /// Implementation of <see cref="ICopyApp" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public sealed class JediOmniCopyApp : DeviceWorkflowLogSource, ICopyApp
    {
        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniCopyJobOptionsManager _optionsManager;
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
                _launchHelper.Pacekeeper = _pacekeeper;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniCopyApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniCopyApp(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediOmniCopyJobOptionsManager(device);
            _executionManager = new JediOmniJobExecutionManager(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Copy application on the device.
        /// </summary>   
        public void Launch()
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = PressCopyButton("#hpid-copy-landing-page");
            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                if (_controlPanel.CheckState("#hpid-signin-app-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException("Sign-in required to launch the Copy application.");
                }
                else
                {
                    throw new DeviceWorkflowException("Could not launch Copy application.");
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
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = false;
            if (authenticationMode == AuthenticationMode.Lazy)
            {
                bool signInScreenLoaded = PressCopyButton("#hpid-signin-body");
                if (signInScreenLoaded)
                {
                    appLoaded = Authenticate(authenticator, "#hpid-copy-landing-page");
                }
                else if (_controlPanel.CheckState("#hpid-copy-app-screen", OmniElementState.Exists))
                {
                    // The application launched without prompting for credentials
                    appLoaded = true;
                    Pacekeeper.Reset();
                }
            }
            else
            {
                switch (authenticator.Provider)
                {
                    case AuthenticationProvider.Card:
                    case AuthenticationProvider.Skip:
                        break;
                    default:
                        _launchHelper.PressSignInButton();
                        break;
                }

                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                appLoaded = PressCopyButton("#hpid-copy-app-screen");
            }

            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                throw new DeviceWorkflowException("Could not launch Copy application.");
            }
        }

        private bool PressCopyButton(string expectedDestination)
        {
            return _launchHelper.PressAppButton("Copy", "#hpid-copy-homescreen-button", expectedDestination);
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
                if (authenticator.Provider != AuthenticationProvider.Card)
                {
                    _launchHelper.WorkflowLogger = WorkflowLogger;
                    _launchHelper.PressSignInButton();
                }
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                PressCopyWithQuickset(quickSetName);
            }
            else // AuthenticationMode.Lazy
            {
                if (PressCopyWithQuickset(quickSetName))
                {
                    // authentication required
                    Authenticate(authenticator, "#hpid-copy-landing-page");
                }
            }
        }

        /// <summary>
        /// Presses the copy with quickset.
        /// </summary>
        /// <param name="quickSetName">Name of the quick set.</param>
        /// <returns></returns>
        /// <exception cref="DeviceWorkflowException">
        /// Quickset with name" + quickSetName + "not found
        /// or
        /// Could not launch Quickset.
        /// </exception>
        private bool PressCopyWithQuickset(string quickSetName)
        {
            bool signInScreenLoaded;

            string elementId = $".hp-homescreen-button:contains({quickSetName})";

            // check if the quickset is on a front page or in the quickset folder
            if (_controlPanel.CheckState(elementId, OmniElementState.Exists))
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, quickSetName);
                signInScreenLoaded = _controlPanel.ScrollPressWait(elementId, JediOmniLaunchHelper.SignInForm, TimeSpan.FromSeconds(10));
                Pacekeeper.Pause();
            }
            else if (_controlPanel.CheckState("#hpid-quicksets-homescreen-button", OmniElementState.Exists))
            {
                _controlPanel.ScrollPressWait("#hpid-quicksets-homescreen-button", "#hpid-quicksets-app-screen", TimeSpan.FromSeconds(3));
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, quickSetName);
                signInScreenLoaded = _controlPanel.ScrollPressWait(elementId, JediOmniLaunchHelper.SignInForm, TimeSpan.FromSeconds(10));
                Pacekeeper.Pause();
            }
            else
            {
                throw new DeviceWorkflowException("Could not launch Quickset.");
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
            RecordEvent(DeviceWorkflowMarker.QuickSetListReady);
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
        /// <param name="quickSetName">Name of the Quickset</param>
        /// </summary>  
        public void SelectQuickSet(string quickSetName)
        {
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
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param> 
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            var jobCompleted = _executionManager.ExecuteScanJob(executionOptions, "#hpid-button-copy-start");
            return jobCompleted;
        }

        /// <summary>
        /// Gets the <see cref="ICopyJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public ICopyJobOptions Options => _optionsManager;

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
    }
}