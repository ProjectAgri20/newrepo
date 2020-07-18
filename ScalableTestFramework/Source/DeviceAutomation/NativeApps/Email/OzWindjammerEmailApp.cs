using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Oz;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.OzWindjammer;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Email
{
    /// <summary>
    /// Implementation of <see cref="IEmailApp" /> for an <see cref="OzWindjammerDevice" />.
    /// </summary>
    public sealed class OzWindjammerEmailApp : DeviceWorkflowLogSource, IEmailApp
    {
        private const int _emailMain = 700;
        private const int _emailKeyboard = 31;
        private const int _addressBookAddPrompt = -791;
        private const int _homeScreen = 567;
        private const int _signInScreen = 18;

        private readonly OzWindjammerDevice _device;
        private readonly OzWindjammerControlPanel _controlPanel;
        private readonly OzWindjammerEmailJobOptions _optionsManager;
        private readonly OzWindjammerJobExecutionManager _executionManager;
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
        /// Initializes a new instance of the <see cref="OzWindjammerEmailApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public OzWindjammerEmailApp(OzWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new OzWindjammerEmailJobOptions(device);
            _executionManager = new OzWindjammerJobExecutionManager(device);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Email application on the device.
        /// </summary>        
        public void Launch()
        {
            Widget appButton = _controlPanel.ScrollToItem("Title", "E-mail");
            if (appButton != null)
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress);
                _controlPanel.Press(appButton);
                bool success = _controlPanel.WaitForScreen(_emailMain, TimeSpan.FromSeconds(30));
                if (!success)
                {
                    int activeScreen = _controlPanel.ActiveScreenId();
                    if (activeScreen == _signInScreen)
                    {
                        throw new DeviceWorkflowException("Sign-in required to launch the Email application.");
                    }
                    else
                    {
                        throw new DeviceWorkflowException($"Could not launch Email application. Active screen: {activeScreen}");
                    }
                }
                RecordEvent(DeviceWorkflowMarker.AppShown);
                Pacekeeper.Sync();
            }
            else
            {
                if (_controlPanel.ActiveScreenId() == _homeScreen)
                {
                    throw new DeviceWorkflowException("Email application button was not found on device home screen.");
                }
                else
                {
                    throw new DeviceWorkflowException("Cannot launch the Email application: Not at device home screen.");
                }
            }
        }

        /// <summary>
        /// Launches the Email application using the specified authenticator and authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                Widget appButton = _controlPanel.ScrollToItem("Title", "E-mail");
                if (appButton != null)
                {
                    RecordEvent(DeviceWorkflowMarker.AppButtonPress);
                    _controlPanel.Press(appButton);
                    bool promptedForCredentials = _controlPanel.WaitForScreen(_signInScreen, TimeSpan.FromSeconds(10));
                    if (promptedForCredentials)
                    {
                        Pacekeeper.Sync();
                        authenticator.Authenticate();
                    }

                    bool success = _controlPanel.WaitForScreen(_emailMain, TimeSpan.FromSeconds(30));
                    if (!success)
                    {
                        throw new DeviceWorkflowException($"Could not launch Email application. Active screen: {_controlPanel.ActiveScreenId()}");
                    }
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                    Pacekeeper.Sync();
                }
                else
                {
                    if (_controlPanel.ActiveScreenId() == _homeScreen)
                    {
                        throw new DeviceWorkflowException("Email application button was not found on device home screen.");
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Cannot launch the Email application: Not at device home screen.");
                    }
                }
            }
            else  // AuthenticationMode.Lazy
            {
                throw new NotImplementedException("Eager Authentication has not been implemented in OzWindjammerEmailApp.");
            }
        }

        /// <summary>
        /// Launches the Email application using the specified authenticator, authentication mode, and quickset.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <param name="quickSetName">Name of the quick set.</param>
        /// <exception cref="System.NotImplementedException">LaunchFromQuickset has not been implemented in OzWindjammerEmailApp.</exception>
        public void LaunchFromQuickSet(IAuthenticator authenticator, AuthenticationMode authenticationMode, string quickSetName)
        {
            throw new NotImplementedException("LaunchFromQuickset has not been implemented in OzWindjammerEmailApp.");
        }

        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="quickSetName">Name of the Quickset if exists or else empty/null</param>
        /// </summary>  
        public void SelectQuickset(string quickSetName)
        {
            throw new NotImplementedException("Qucickset selection has not been implemented in OzWindjammerEmailApp.");
        }

        /// <summary>
        /// Enters the "To" email address.
        /// </summary>
        /// <param name="emailAddresses">The "To" email addresses.</param>
        public void EnterToAddress(IEnumerable<string> emailAddresses)
        {
            foreach (string address in emailAddresses)
            {
                // Sometimes the locations for the widgets are incorrect.
                // Look to see if all the edit boxes are reporting the same location.
                if (AreLocationsIncorrect())
                {
                    // The bounding boxes are incorrect, so we'll have to use a raw X,Y coordinate.
                    _controlPanel.PressScreen(new Coordinate(267, 134));
                }
                else
                {
                    // The labels do not report the correct text, so the ID must be used.
                    // Depending on the device, it will be one of two IDs.
                    _controlPanel.Press(454, 3146663);
                }
                bool success = _controlPanel.WaitForScreen(_emailKeyboard, TimeSpan.FromSeconds(5));

                if (success)
                {
                    Pacekeeper.Sync();

                    // Type the email address
                    _controlPanel.TypeOnVirtualKeyboard(address);
                    _controlPanel.Press("OK");

                    // We might get a popup asking if we want to add this email to the address book
                    if (_controlPanel.WaitForScreen(_addressBookAddPrompt, TimeSpan.FromSeconds(3)))
                    {
                        Pacekeeper.Sync();
                        _controlPanel.Press("No");
                    }

                    // Wait for the main screen
                    _controlPanel.WaitForScreen(_emailMain, TimeSpan.FromSeconds(5));
                    Pacekeeper.Sync();
                }
                else
                {
                    throw new DeviceWorkflowException("Could not enter To address.");
                }
            }
        }

        /// <summary>
        /// Enters the email subject.
        /// </summary>
        /// <param name="subject">The subject.</param>
        public void EnterSubject(string subject)
        {
            // Sometimes the locations for the widgets are incorrect.
            // Look to see if all the edit boxes are reporting the same location.
            if (AreLocationsIncorrect())
            {
                // The bounding boxes are incorrect, so we'll have to use a raw X,Y coordinate.
                _controlPanel.PressScreen(new Coordinate(267, 184));
            }
            else
            {
                // The labels do not report the correct text, so the ID must be used.
                // Depending on the device, it will be one of two IDs.
                _controlPanel.Press(463, 3146672);
            }
            bool success = _controlPanel.WaitForScreen(_emailKeyboard, TimeSpan.FromSeconds(5));

            if (success)
            {
                Pacekeeper.Sync();

                // Type the subject and navigate to the main screen
                _controlPanel.TypeOnVirtualKeyboard(subject);
                Pacekeeper.Sync();
                _controlPanel.Press("OK");
                _controlPanel.WaitForScreen(_emailMain, TimeSpan.FromSeconds(5));
                Pacekeeper.Sync();
            }
            else
            {
                throw new DeviceWorkflowException("Could not enter Subject.");
            }
        }

        /// <summary>
        /// Enters the name to use for the scanned file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void EnterFileName(string fileName)
        {
            // This feature is not available on Oz devices.
        }

        private bool AreLocationsIncorrect()
        {
            WidgetCollection editBoxes = _controlPanel.GetWidgets().OfType(WidgetType.EditBox);
            int distinctLocations = editBoxes.Select(n => n.Location).Distinct().Count();
            return distinctLocations < editBoxes.Count;
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            bool completedJob = false;
            _executionManager.WorkflowLogger = WorkflowLogger;
            completedJob = _executionManager.ExecuteScanJob(executionOptions);
            return completedJob;
        }

        /// <summary>
        /// Gets the <see cref="IEmailJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IEmailJobOptions Options => _optionsManager;

        private class OzWindjammerEmailJobOptions : OzWindjammerJobOptionsManager, IEmailJobOptions
        {
            public OzWindjammerEmailJobOptions(OzWindjammerDevice device)
                : base(device)
            {
            }
        }
    }
}
