using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.USB
{
    /// <summary>
    /// Implementation of <see cref="IUsbApp" /> for a <see cref="JediWindjammerDevice" />.
    /// </summary>
    public sealed class JediWindjammerUsbApp : DeviceWorkflowLogSource, IUsbApp
    {
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
        private readonly JediWindjammerUsbJobOptions _optionsManager;
        private readonly JediWindjammerJobExecutionManager _executionManager;
        private bool _success = false;

        private const string FILENAME_START_TYPE = "FileSystemNode";
        private const string SAVE_TO_USB_FORM = "SaveToUsbMainForm";
        private const string QUICKSET_BUTTON = "NLevelApp";
        private const string QUICKSET_FORM = "NLevelHomeScreenForm";

        private Pacekeeper _pacekeeper;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerUsbApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerUsbApp(JediWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediWindjammerUsbJobOptions(device);
            _executionManager = new JediWindjammerJobExecutionManager(device);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Print application on the device.
        /// </summary>
        /// <exception cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowException">
        /// Sign-in required to launch the USB application.
        /// or
        /// $Could not launch USB application: {message}
        /// or
        /// $Could not launch USB application: {ex.Message}
        /// </exception>
        public void LaunchPrintFromUsb()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Launches the Print to USB application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void LaunchPrintFromUsb(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);
                PressRetrieveFromUsbButton("OpenFromUsbMainForm");
            }
            else // AuthenticationMode.Lazy
            {
                if (PressRetrieveFromUsbButton("SignInForm"))
                {
                    // authentication required
                    Authenticate(authenticator, "OpenFromUsbMainForm");
                }
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        private bool PressRetrieveFromUsbButton(string expectedForm)
        {
            bool signInScreenLoaded = false;

            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Print From USB");
                signInScreenLoaded = _controlPanel.PressWait("OpenFromUsb", expectedForm, TimeSpan.FromSeconds(3));

                if (!signInScreenLoaded)
                {
                    if (!_controlPanel.WaitForForm("OpenFromUsbMainForm", TimeSpan.FromSeconds(3)))
                    {
                        throw new DeviceWorkflowException("Could not launch Retrieve from USB application.");
                    }
                }
            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("Retrieve from USB button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceWorkflowException($"Cannot launch the Retrieve from USB application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case "OpenFromUsbMainForm":
                        // The application launched without prompting for credentials.
                        Pacekeeper.Reset();
                        return false;

                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Could not launch Retrieve from USB application: {message}", ex);

                    default:
                        throw new DeviceWorkflowException($"Could not launch Retrieve from USB application:: {ex.Message}", ex);
                }
            }
            return signInScreenLoaded;
        }

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
        /// Launches the Scan application on the device.
        /// </summary>
        /// <exception cref="System.NotImplementedException"></exception>
        public void LaunchScanToUsb()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Launches the Scan to USB application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void LaunchScanToUsb(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);
                PressSaveToUsbButton();
            }
            else // AuthenticationMode.Lazy
            {
                if (PressSaveToUsbButton())
                {
                    // authentication required
                    Authenticate(authenticator, SAVE_TO_USB_FORM);
                }
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Launches the scan to USB by a given quick set.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quickset</param>
        public void LaunchScanToUsbByQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);
                PressQuickSetButton(quickSetName);
            }
            else
            {
                if (PressQuickSetButton(quickSetName))
                {
                    // authentication required
                    Authenticate(authenticator, SAVE_TO_USB_FORM);
                }
            }

            RecordEvent(DeviceWorkflowMarker.AppShown);

        }

        private bool PressQuickSetButton(string quickSetName)
        {
            bool signInScreenLoaded = false;

            var homeCtlrs = _controlPanel.GetControls();

            if (homeCtlrs.Where(f => f.Equals(quickSetName)).FirstOrDefault() == null)
            {
                _controlPanel.PressWait(QUICKSET_BUTTON, QUICKSET_FORM);
            }

            signInScreenLoaded = _controlPanel.PressWait(quickSetName, JediWindjammerLaunchHelper.SIGNIN_FORM, TimeSpan.FromSeconds(3));
            if (!signInScreenLoaded)
            {
                if (!_controlPanel.WaitForForm(SAVE_TO_USB_FORM, TimeSpan.FromSeconds(3)))
                {
                    throw new DeviceWorkflowException("Could not launch Save to USB application.");
                }
            }
            return signInScreenLoaded;
        }

        private bool PressSaveToUsbButton()
        {
            bool signInScreenLoaded = false;

            try
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Save To USB");
                signInScreenLoaded = _controlPanel.PressWait("SaveToUsb", "SignInForm", TimeSpan.FromSeconds(3));

                if (!signInScreenLoaded)
                {
                    if (!_controlPanel.WaitForForm(SAVE_TO_USB_FORM, TimeSpan.FromSeconds(3)))
                    {
                        throw new DeviceWorkflowException("Could not launch Save to USB application.");
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
                    throw new DeviceWorkflowException($"Cannot launch the Save to USB application from {currentForm}.", ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case SAVE_TO_USB_FORM:
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

        /// <summary>
        /// Adds the specified name for the job
        /// </summary>
        /// <param name="jobname">The job name.</param>
        /// <exception cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowException">
        /// File name is not user-editable.
        /// or
        /// $Error entering file name: {message}
        /// </exception>
        public void AddJobName(string jobname)
        {
            try
            {
                _controlPanel.PressToNavigate("mFileNameTextBox", "SaveToUsbKeyboardForm", ignorePopups: false);
                Pacekeeper.Pause();
                _controlPanel.TypeOnVirtualKeyboard("mKeyboard", jobname);
                Pacekeeper.Sync();
                _controlPanel.PressToNavigate("ok", SAVE_TO_USB_FORM, ignorePopups: false);
                Pacekeeper.Pause();
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (_controlPanel.CurrentForm())
                {
                    case SAVE_TO_USB_FORM:
                        if (_controlPanel.GetProperty<bool>("mFileNameTextBox", "Enabled") == false)
                        {
                            throw new DeviceWorkflowException("File name is not user-editable.", ex);
                        }
                        else
                        {
                            throw;
                        }

                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceWorkflowException($"Error entering file name: {message}", ex);

                    default:
                        throw;
                }
            }
        }

        /// <summary>
        /// Starts the current job and runs it to completion" /&gt;.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ExecutePrintJob()
        {
            _controlPanel.Press("mStartButton");
            if (_controlPanel.CurrentForm() == "BaseJobStartPopup")
            {
                _controlPanel.PressToNavigate("m_OKButton", "OpenFromUsbMainForm", ignorePopups: true);
            }
            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
            Retry.UntilTrue(() => CheckingStatusMessage(), 200, TimeSpan.FromSeconds(3));
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            return _success;
        }

        private bool CheckingStatusMessage()
        {
            string status = _controlPanel.GetProperty("mStatusLabel", "Text");
            if (status.Contains("Ready") || status.Contains("Paperless Mode"))
            {
                _success = true;
                return _success;
            }

            return false;
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool ExecuteScanJob(ScanExecutionOptions executionOptions)
        {
            bool success = false;

            _executionManager.WorkflowLogger = WorkflowLogger;
            success = _executionManager.ExecuteScanJob(executionOptions, "SaveToUsbMainForm");

            if (success && _controlPanel.CurrentForm() == "BaseJobStartPopup")
            {
                _controlPanel.PressToNavigate("m_OKButton", "SaveToUsbMainForm", true);
            }

            return success;
        }


        /// <summary>
        /// Starts the current job and runs it to completion" /&gt;.
        /// </summary>
        /// <exception cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowException">No PDF or Text files found on USB device.</exception>
        public void SelectFile()
        {
            bool found = false;

            while (true)
            {
                var controls = _controlPanel.GetControls();
                foreach (string control in controls.Where(n => n.StartsWith(FILENAME_START_TYPE)))
                {
                    string fileName = _controlPanel.GetProperty(control, "Text");
                    fileName = fileName.ToUpper();
                    if (fileName.EndsWith(".PDF") || fileName.EndsWith(".TXT"))
                    {
                        found = true;
                        _controlPanel.Press(control);
                        break;
                    }
                }

                if (!found)
                {
                    if (_controlPanel.GetProperty<bool>("IncrementButton", "Enabled") &&
                        _controlPanel.GetProperty<bool>("IncrementButton", "Visible"))
                    {
                        _controlPanel.Press("IncrementButton");
                        var updatedcontrols = _controlPanel.GetControls();
                        if (updatedcontrols.SequenceEqual(controls))
                        {
                            break;
                        }
                    }
                    else
                    {
                        break;
                    }
                }
                else
                {
                    break;
                }
            }
            if (!found)
            {
                throw new DeviceWorkflowException("No PDF or Text files found on USB device.");
            }
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// </summary>
        /// <param name="quicksetName">The quickset name.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SelectQuickset(string quicksetName)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Select the Usb device.
        /// </summary>
        /// <param name="usbName">The file name.</param>
        public void SelectUsbDevice(string usbName)
        {
            //throw new NotImplementedException();
        }

        /// <summary>
        /// Starts the current job and runs it to completion" /&gt;.
        /// </summary>
        /// <param name="usbName">Name of the usb.</param>
        public void SelectUsbPrint(string usbName)
        {
            //var ctrls = _controlPanel.GetControls();
            //string currentForm = _controlPanel.CurrentForm();
        }


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
        /// Gets the <see cref="IUsbJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IUsbJobOptions Options => _optionsManager;

        private class JediWindjammerUsbJobOptions : JediWindjammerJobOptionsManager, IUsbJobOptions
        {
            public JediWindjammerUsbJobOptions(JediWindjammerDevice device)
                : base(device, "SaveToUsbMainForm")
            {
            }
        }
    }
}
