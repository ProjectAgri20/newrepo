using System;
using HP.DeviceAutomation.Oz;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.OzWindjammer;
using System.Linq;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Fax
{
    /// <summary>
    /// Implementation of <see cref="IFaxApp" /> for an <see cref="OzWindjammerDevice" />.
    /// </summary>
    public sealed class OzWindjammerFaxApp : DeviceWorkflowLogSource, IFaxApp
    {
        private const int _faxMain = 717;
        private const int _faxKeypad = -730;
        private const int _homeScreen = 567;
        private const int _signInScreen = 18;

        private readonly OzWindjammerDevice _device;
        private readonly OzWindjammerControlPanel _controlPanel;
        private readonly OzWindjammerFaxJobOptions _optionsManager;
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
        /// Initializes a new instance of the <see cref="OzWindjammerFaxApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public OzWindjammerFaxApp(OzWindjammerDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new OzWindjammerFaxJobOptions(device);
            _executionManager = new OzWindjammerJobExecutionManager(device);

            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Fax application on the device.
        /// </summary>
        public void Launch()
        {
            Widget appButton = _controlPanel.ScrollToItem("Title", "Fax");
            if (appButton != null)
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress);
                _controlPanel.Press(appButton);
                bool success = _controlPanel.WaitForScreen(_faxMain, TimeSpan.FromSeconds(30));
                if (!success)
                {
                    int activeScreen = _controlPanel.ActiveScreenId();
                    if (activeScreen == _signInScreen)
                    {
                        throw new DeviceWorkflowException("Sign-in required to launch the Fax application.");
                    }
                    else
                    {
                        throw new DeviceWorkflowException($"Could not launch Fax application. Active screen: {activeScreen}");
                    }
                }
                RecordEvent(DeviceWorkflowMarker.AppShown);
                Pacekeeper.Sync();
            }
            else
            {
                if (_controlPanel.ActiveScreenId() == _homeScreen)
                {
                    throw new DeviceWorkflowException("Fax application button was not found on device home screen.");
                }
                else
                {
                    throw new DeviceWorkflowException("Cannot launch the Fax application: Not at device home screen.");
                }
            }
        }

        /// <summary>
        /// Launches the FAX solution with the specified authenticator using the given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="DeviceWorkflowException">
        /// Fax application button was not found on device home screen.
        /// or
        /// Cannot launch the Fax application: Not at device home screen.
        /// </exception>
        /// <exception cref="System.NotImplementedException">Eager authentication has not been implemented for this solution.</exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                Widget appButton = _controlPanel.ScrollToItem("Title", "Fax");
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

                    bool success = _controlPanel.WaitForScreen(_faxMain, TimeSpan.FromSeconds(30));
                    if (!success)
                    {
                        throw new DeviceWorkflowException($"Could not launch Fax application. Active screen: {_controlPanel.ActiveScreenId()}");
                    }
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                    Pacekeeper.Sync();
                }
                else
                {
                    if (_controlPanel.ActiveScreenId() == _homeScreen)
                    {
                        throw new DeviceWorkflowException("Fax application button was not found on device home screen.");
                    }
                    else
                    {
                        throw new DeviceWorkflowException("Cannot launch the Fax application: Not at device home screen.");
                    }
                }
            }
            else
            {
                throw new NotImplementedException("Eager authentication has not been implemented for this solution.");
            }
        }

        /// <summary>
        /// Adds the specified recipient/s for the fax.
        /// </summary>
        /// <param name="recipients">The recipients. Contains PINs, if used.</param>
        /// <param name="useSpeedDial">Uses the #s as speed dials</param>
        public void AddRecipients(Dictionary<string, string> recipients, bool useSpeedDial)
        {
            // The labels do not report the correct text, so the ID must be used.
            // Depending on the device, it will be one of a few options.
            Widget recipientBox = _controlPanel.GetWidgets().OfType(WidgetType.StringBox, WidgetType.EditBox).Find(1600, 1601, 4195058);
            _controlPanel.Press(recipientBox);
            _controlPanel.WaitForScreen(_faxKeypad, TimeSpan.FromSeconds(5));
            Pacekeeper.Sync();

            // Type the recipient and navigate to the main screen
            _controlPanel.TypeOnNumericKeypad(recipients.First().Key);
            Pacekeeper.Sync();
            _controlPanel.Press("OK");
            _controlPanel.WaitForScreen(_faxMain, TimeSpan.FromSeconds(5));
            Pacekeeper.Sync();

            // Add the recipient to the list
            _controlPanel.Press(1607, 4195060);
            Pacekeeper.Sync();
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            bool jobCompleted = false;
            _executionManager.WorkflowLogger = WorkflowLogger;
            jobCompleted = _executionManager.ExecuteScanJob(executionOptions);
            return jobCompleted;
        }

        /// <summary>
        /// Retrieves the html String from Fax Report(not implemented for Oz)
        /// </summary>
        public string RetrieveFaxReport()
        {
            throw new NotImplementedException("Fax Report is yet to be supported for oz Firmware");
        }

        /// <summary>
        /// Adds the recipient.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        public void AddRecipient(string recipient)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Gets the <see cref="IFaxJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IFaxJobOptions Options => _optionsManager;

        private class OzWindjammerFaxJobOptions : OzWindjammerJobOptionsManager, IFaxJobOptions
        {
            public OzWindjammerFaxJobOptions(OzWindjammerDevice device)
                : base(device)
            {
            }
        }

    }
}
