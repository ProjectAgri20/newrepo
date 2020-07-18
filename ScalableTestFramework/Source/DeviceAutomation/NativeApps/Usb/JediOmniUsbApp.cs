using System;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.USB
{
    /// <summary>
    /// Implementation of <see cref="IUsbApp" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public sealed class JediOmniUsbApp : DeviceWorkflowLogSource, IUsbApp
    {
        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniUsbJobOptions _optionsManager;
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
        /// Constructor for JediOmin Device.
        /// </summary>
        /// <param name="device">JediOmni Device </param>
        public JediOmniUsbApp(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediOmniUsbJobOptions(device);
            _executionManager = new JediOmniJobExecutionManager(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Scan To Usb application on the device.
        /// </summary>
        public void LaunchScanToUsb()
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = PressScanToUsbButton("#hpid-send-usb-landing-page");
            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                if (_controlPanel.CheckState("#hpid-signin-app-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException("Sign-in required to launch the Save To Usb application.");
                }
                else
                {
                    throw new DeviceWorkflowException("Could not launch Save To Usb application.");
                }
            }
        }

        /// <summary>
        /// Launches the Scan to USB application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void LaunchScanToUsb(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = false;
            if (authenticationMode == AuthenticationMode.Lazy)
            {
                bool signInScreenLoaded = PressScanToUsbButton("#hpid-signin-body");
                if (signInScreenLoaded)
                {
                    appLoaded = Authenticate(authenticator, "#hpid-send-usb-landing-page");
                }
                else if (_controlPanel.CheckState("#hpid-send-usb-landing-page", OmniElementState.Exists))
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
                appLoaded = PressScanToUsbButton("#hpid-send-usb-landing-page");
            }

            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                throw new DeviceWorkflowException("Could not launch Scan to USB application.");
            }
        }

        private bool PressScanToUsbButton(string expectedDestination)
        {
            return _launchHelper.PressAppButton("USB", "#hpid-sendUsb-homescreen-button", "#hpid-scan-homescreen-button", expectedDestination);
        }

        /// <summary>
        /// Launches the Print From Usb application on the device.
        /// </summary>
        public void LaunchPrintFromUsb()
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = PressPrintFromUsbButton("#hpid-print-usb-app-screen");
            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                if (_controlPanel.CheckState("#hpid-signin-app-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException("Sign-in required to launch the Print from Usb application.");
                }
                else
                {
                    throw new DeviceWorkflowException("Could not launch Print from Usb application.");
                }
            }
        }

        /// <summary>
        /// Launches the Print to USB application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void LaunchPrintFromUsb(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = false;
            if (authenticationMode == AuthenticationMode.Lazy)
            {
                bool signInScreenLoaded = PressPrintFromUsbButton("#hpid-signin-body");
                if (signInScreenLoaded)
                {
                    appLoaded = Authenticate(authenticator, "#hpid-print-usb-app-screen");
                }
                else if (_controlPanel.CheckState("#hpid-print-usb-app-screen", OmniElementState.Exists))
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
                appLoaded = PressPrintFromUsbButton("#hpid-print-usb-app-screen");
            }

            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                throw new DeviceWorkflowException("Could not launch Print From Usb application.");
            }
        }

        private bool PressPrintFromUsbButton(string expectedDestination)
        {
            return _launchHelper.PressAppButton("Print From USB", "#hpid-printUsb-homescreen-button", "#hpid-print-homescreen-button", expectedDestination);
        }

        /// <summary>
        /// Authenticates the specified authenticator.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">The wait form.</param>
        private bool Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            bool formLoaded = _controlPanel.WaitForAvailable(waitForm);
            Pacekeeper.Pause();
            return formLoaded;
        }

        /// <summary>
        /// Enters the name to use for the scanned file.
        /// </summary>
        /// <param name="jobname">The file name.</param>
        public void AddJobName(string jobname)
        {
            _controlPanel.ScrollPressWait("#hpid-file-details-container-name-textbox", "#hpid-keyboard");
            Pacekeeper.Sync();
            _controlPanel.TypeOnVirtualKeyboard(jobname);
            Pacekeeper.Sync();
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Select the Usb device.
        /// </summary>
        /// <param name="usbName">The file name.</param>
        public void SelectUsbDevice(string usbName)
        {
            if (_device.ControlPanel.GetValue("#hpid-destination-button", "innerText", OmniPropertyType.Property).Contains("Choose"))
            {
                _device.ControlPanel.ScrollPressWait("#hpid-destination-button", "#hpid-send-usb-setting-view", TimeSpan.FromSeconds(5));
                if (_device.ControlPanel.CheckState(".hp-list-item-folder:contains(" + usbName + ")", OmniElementState.Exists))
                {
                    _device.ControlPanel.Press(".hp-list-item-folder:contains(" + usbName + ")");
                }
                else
                {
                    throw new DeviceWorkflowException("The User entered USB name is not found on device.");
                }
            }
        }

        /// <summary>
        /// Select the Usb device.
        /// </summary>
        /// <param name="usbName">The file name.</param>
        public void SelectUsbPrint(string usbName)
        {
            _device.ControlPanel.ScrollPressWait("#hpid-print-usb-choose-file-button", "#hpid-send-usb-setting-view", TimeSpan.FromSeconds(5));
            if (!_device.ControlPanel.CheckState(".hp-list-item-device", OmniElementState.Hidden))
            {
                if (_device.ControlPanel.CheckState(".hp-list-item-device:contains(" + usbName + ")", OmniElementState.Useable))
                {
                    _device.ControlPanel.Press(".hp-list-item-device:contains(" + usbName + ")");
                }
                else
                {
                    throw new DeviceWorkflowException("The User entered USB name .");
                }
            }
        }

        /// <summary>
        /// Select the Usb device.
        /// </summary>
        public void SelectFile()
        {
            if (_device.ControlPanel.CheckState(".hp-list-item-file", OmniElementState.Exists))
            {
                _device.ControlPanel.ScrollPress(".hp-list-item-file:first");
            }
            else
            {
                _device.ControlPanel.ScrollPress(".hp-list-item-device:first");
                {
                    if (_device.ControlPanel.CheckState(".hp-list-item-file", OmniElementState.Exists))
                    {
                        _device.ControlPanel.ScrollPress(".hp-list-item-file:first");

                    }
                    else
                    {
                        throw new DeviceWorkflowException("There is no file found to print");
                    }
                }
            }
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// </summary>
        /// <param name="quicksetName">The quickset name.</param>
        public void SelectQuickset(string quicksetName)
        {
            _controlPanel.ScrollPressWait(".hp-load-quickset-button", "#hpid-quicksets-list");
            Pacekeeper.Sync();
            _controlPanel.Press(".hp-listitem:contains(" + quicksetName + ")");
            Pacekeeper.Sync();
            _controlPanel.Press("#hpid-quicksets-load-button");
            Pacekeeper.Sync();
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteScanJob(ScanExecutionOptions executionOptions)
        {
            bool done = false;
            _executionManager.WorkflowLogger = WorkflowLogger;
            done = _executionManager.ExecuteScanJob(executionOptions, "#hpid-button-send-usb-start");
            if (_device.ControlPanel.CheckState("#hpid-retain-settings-clear-button", OmniElementState.Exists))
            {
                _device.ControlPanel.Press("#hpid-retain-settings-clear-button");
            }
            return done;
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified />.
        /// </summary>
        public bool ExecutePrintJob()
        {
            bool success = false;
            JediOmniMasthead masthead = new JediOmniMasthead(_device);

            // for SFP's which have a slightly different workflow
            if (_controlPanel.CheckState("#hpid-select-usb-job-to-print-button", OmniElementState.Useable))
            {
                _controlPanel.Press("#hpid-select-usb-job-to-print-button");
            }
            if (_controlPanel.WaitForAvailable("#hpid-button-print-usb-start", TimeSpan.FromSeconds(5)))
            {
                RecordEvent(DeviceWorkflowMarker.PrintJobBegin);
                _controlPanel.Press("#hpid-button-print-usb-start");
                if (masthead.WaitForActiveJobsButtonState(true, TimeSpan.FromSeconds(20)))
                {
                    masthead.WaitForActiveJobsButtonState(false, TimeSpan.FromSeconds(120));
                    RecordEvent(DeviceWorkflowMarker.PrintJobEnd);
                    success = true;
                }
                else
                {
                    throw new DeviceWorkflowException("Unable to determine if print job started.");
                }
            }
            return success;
        }

        /// <summary>
        /// Launches the scan to usb by a given quick set.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quickset</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void LaunchScanToUsbByQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the <see cref="IUsbJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IUsbJobOptions Options => _optionsManager;

        private class JediOmniUsbJobOptions : JediOmniJobOptionsManager, IUsbJobOptions
        {
            public JediOmniUsbJobOptions(JediOmniDevice device)
                : base(device)
            {
            }
        }
    }
}
