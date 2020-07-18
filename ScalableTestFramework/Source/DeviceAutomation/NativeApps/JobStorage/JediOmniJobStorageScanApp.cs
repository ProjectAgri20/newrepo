using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using System;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.JobStorage
{
    /// <summary>
    /// Implementation of <see cref="IJobStorageScanApp" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public sealed class JediOmniJobStorageScanApp : DeviceWorkflowLogSource, IJobStorageScanApp
    {
        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniJobStorageJobOptions _optionsManager;
        private readonly JediOmniJobExecutionManager _executionManager;
        private readonly JediOmniLaunchHelper _launchHelper;
        private static readonly TimeSpan ScreenWaitTimeout = new TimeSpan(0, 0, 0, 5);

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
        /// Constructor for JediOmin Device.
        /// </summary>
        /// <param name="device">JediOmni Device </param>
        public JediOmniJobStorageScanApp(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediOmniJobStorageJobOptions(device);
            _executionManager = new JediOmniJobExecutionManager(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Scan to Job Storage application on the device.
        /// </summary>
        public void Launch()
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = PressScanToJobStorageButton("#hpid-send-job-storage-landing-page");
            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                if (_controlPanel.CheckState("#hpid-signin-app-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException("Sign-in required to launch the Scan To Job Storage application.");
                }
                else
                {
                    throw new DeviceWorkflowException("Could not launch Scan To Job Storage application.");
                }
            }
        }

        /// <summary>
        /// Launches the Scan to Job Storage using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = false;
            if (authenticationMode == AuthenticationMode.Lazy)
            {
                bool signInScreenLoaded = PressScanToJobStorageButton("#hpid-signin-body");
                if (signInScreenLoaded)
                {
                    appLoaded = Authenticate(authenticator, "#hpid-send-job-storage-app-screen");
                }
                else if (_controlPanel.CheckState("#hpid-send-job-storage-app-screen", OmniElementState.Exists))
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
                appLoaded = PressScanToJobStorageButton("#hpid-send-job-storage-landing-page");
            }

            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                throw new DeviceWorkflowException("Could not launch Scan To Job Storage application.");
            }
        }

        private bool PressScanToJobStorageButton(string expectedDestination)
        {
            return _launchHelper.PressAppButton("Scan To Job Storage", "#hpid-sendJobStorage-homescreen-button", "#hpid-scan-homescreen-button", expectedDestination);
        }

        private bool Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            bool formLoaded = _controlPanel.WaitForAvailable(waitForm);
            Pacekeeper.Pause();
            return formLoaded;
        }

        /// <summary>
        /// Adds the specified name for the job along with the pin 
        /// </summary>
        /// <param name="jobname">The job name.</param>
        /// <param name="pin">Pin for the file.</param> 
        public void AddJobName(string jobname, string pin)
        {
            _controlPanel.ScrollPressWait("#hpid-file-details-container-name-textbox", "#hpid-keyboard");
            Pacekeeper.Sync();
            _controlPanel.TypeOnVirtualKeyboard(jobname);
            Pacekeeper.Sync();
            _controlPanel.Press("#hpid-file-details-container-pin-button");
            _controlPanel.ScrollPressWait("#hpid-file-details-container-pin-textbox", "#hpid-keyboard");
            Pacekeeper.Sync();
            _controlPanel.TypeOnVirtualKeyboard(pin);
            Pacekeeper.Sync();
            _controlPanel.Press("#hpid-keypad-key-close");
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Adds the specified name for the job along with the pin 
        /// </summary>
        /// <param name="jobname">The job name.</param>     
        public void AddJobName(string jobname)
        {
            _controlPanel.ScrollPressWait("#hpid-file-details-container-name-textbox", "#hpid-keyboard");
            Pacekeeper.Sync();
            _controlPanel.TypeOnVirtualKeyboard(jobname);
            Pacekeeper.Sync();
            _controlPanel.Press("#hpid-keyboard-key-done");
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Starts the Scan job and runs it to completion" />.
        /// </summary>
        /// <param name="executionOptions">The execution options</param>
        public bool ExecuteScanJob(ScanExecutionOptions executionOptions)
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            bool done = _executionManager.ExecuteScanJob(executionOptions, "#hpid-button-send-job-storage-start");
            if (_device.ControlPanel.CheckState("#hpid-retain-settings-clear-button", OmniElementState.Exists))
            {
                _device.ControlPanel.Press("#hpid-retain-settings-clear-button");
            }

            return done;
        }

        /// <summary>
        /// Gets the <see cref="IJobStorageJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IJobStorageJobOptions Options => _optionsManager;

        private class JediOmniJobStorageJobOptions : JediOmniJobOptionsManager, IJobStorageJobOptions
        {
            public JediOmniJobStorageJobOptions(JediOmniDevice device)
                : base(device)
            {
            }
        }
    }
}
