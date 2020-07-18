using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Fax
{
    /// <summary>
    /// Implementation of <see cref="IFaxApp" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public sealed class JediOmniFaxApp : DeviceWorkflowLogSource, IFaxApp
    {
        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniFaxJobOptions _optionsManager;
        private readonly JediOmniJobExecutionManager _executionManager;
        private readonly JediOmniPopupManager _popupManager;
        private readonly JediOmniLaunchHelper _launchHelper;
        private Pacekeeper _pacekeeper;

        private const string _faxActivityLogCheckBoxId = "#hpid-report-page-checkbox-c9da251f-64e0-4512-b676-2797a441a03a";

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
        /// Initializes a new instance of the <see cref="JediWindjammerFaxApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniFaxApp(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
            _optionsManager = new JediOmniFaxJobOptions(device);
            _executionManager = new JediOmniJobExecutionManager(device);
            _popupManager = new JediOmniPopupManager(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Launches the Fax application on the device.
        /// </summary>
        public void Launch()
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = PressFaxButton("#hpid-sendfax-landing-page");
            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                if (_controlPanel.CheckState("#hpid-signin-app-screen", OmniElementState.Exists))
                {
                    throw new DeviceWorkflowException("Sign-in required to launch the Fax application.");
                }
                else
                {
                    throw new DeviceWorkflowException("Could not launch Fax application.");
                }
            }
        }

        /// <summary>
        /// Launches the FAX solution with the specified authenticator using the given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="DeviceWorkflowException">
        /// Fax application button was not found on device home screen or in scan folder.
        /// or
        /// Fax application button was not found on device home screen.
        /// or
        /// Could not launch Fax application.
        /// </exception>
        /// <exception cref="System.NotImplementedException">Eager authentication has not been implemented for this solution.</exception>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            bool appLoaded = false;
            if (authenticationMode == AuthenticationMode.Lazy)
            {
                bool signInScreenLoaded = PressFaxButton("#hpid-signin-body");
                if (signInScreenLoaded)
                {
                    appLoaded = Authenticate(authenticator, "#hpid-sendfax-landing-page");
                }
                else if (_controlPanel.CheckState("#hpid-sendfax-app-screen", OmniElementState.Exists))
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
                appLoaded = PressFaxButton("#hpid-sendfax-landing-page");
            }

            if (appLoaded)
            {
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            else
            {
                throw new DeviceWorkflowException("Could not launch Fax application.");
            }
        }

        private bool PressFaxButton(string expectedDestination)
        {
            return _launchHelper.PressAppButton("Fax", "#hpid-sendFax-homescreen-button", "#hpid-scan-homescreen-button", expectedDestination);
        }

        private bool Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            bool formLoaded = _controlPanel.WaitForAvailable(waitForm);
            Pacekeeper.Pause();
            return formLoaded;
        }

        /// <summary>
        /// Adds the specified recipient for the fax.
        /// </summary>
        /// <param name="recipient">The recipient.</param>
        public void AddRecipient(string recipient)
        {
            _controlPanel.WaitForAvailable("#hpid-sendfax-recipient-textbox", TimeSpan.FromSeconds(10));

            if (!_controlPanel.CheckState("#hpid-sendfax-recipient-textbox", OmniElementState.Selected) || !_controlPanel.CheckState("#hpid-keypad", OmniElementState.Useable))
            {
                _controlPanel.PressWait("#hpid-sendfax-recipient-textbox", ".hp-keypad", TimeSpan.FromSeconds(6));
            }
            Pacekeeper.Sync();

            // TypeOnNumericKeypad cannot handle the "pause" key, represented as a comma. 
            // Split the string around any pauses, and then type in the pauses manually.
            string[] pieces = recipient.Split(',');
            for (int i = 0; i < pieces.Length - 1; i++)
            {
                _controlPanel.TypeOnNumericKeypad(pieces[i]);
                _controlPanel.Press("#hpid-keypad-key-pause");
            }
            _controlPanel.TypeOnNumericKeypad(pieces.Last());

            if (_controlPanel.WaitForAvailable("#hpid-keypad-key-close", TimeSpan.FromSeconds(1)))
            {
                _controlPanel.Press("#hpid-keypad-key-close");
            }

            Pacekeeper.Sync();
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Adds the specified recipient/s for the fax.
        /// </summary>
        /// <param name="recipients">The recipients. Contains PINs, if used.</param>
        /// <param name="useSpeedDial">Uses the #s as speed dials</param>
        public void AddRecipients(Dictionary<string, string> recipients, bool useSpeedDial)
        {
            var keyboard = _controlPanel.CheckState(".hp-keypad", OmniElementState.Useable);

            if (!_controlPanel.CheckState("#hpid-sendfax-recipient-textbox", OmniElementState.Selected) || !_controlPanel.CheckState("#hpid-keypad", OmniElementState.Useable))
            {
                _controlPanel.PressWait("#hpid-sendfax-recipient-textbox", ".hp-keypad", TimeSpan.FromSeconds(6));
            }
            Pacekeeper.Sync();

            if (!useSpeedDial)
            {
                foreach (var recipient in recipients)
                {
                    if (!string.IsNullOrEmpty(recipient.Value))
                    {
                        AddPINRecipient(recipient.Key, recipient.Value);
                    }
                    else
                    {
                        _controlPanel.TypeOnNumericKeypad(recipient.Key);
                    }

                    bool state = _controlPanel.CheckState("#hpid-keypad-key-close", OmniElementState.Exists);
                    bool state2 = _controlPanel.CheckState("#hpid-keypad-key-close", OmniElementState.Useable);
                    bool state3 = _controlPanel.CheckState("#hpid-keypad-key-close", OmniElementState.Hidden);
                    bool state4 = _controlPanel.CheckState("#hpid-keypad-key-close", OmniElementState.Enabled);
                    bool state5 = _controlPanel.CheckState("#hpid-keypad-key-close", OmniElementState.VisibleCompletely);
                    bool state6 = _controlPanel.CheckState("#hpid-keypad-key-close", OmniElementState.VisiblePartially);



                    if (state && state2)
                    {
                        _controlPanel.Press("#hpid-keypad-key-close");
                    }
                    else
                    {
                        //_controlPanel.WaitForState("#hpid-keypad-key-pause", OmniElementState.Useable,TimeSpan.FromSeconds(35));
                        _controlPanel.Press(".hp-keypad-key:contains(Pause):last");
                    }
                    Pacekeeper.Pause();
                }

                if (!string.IsNullOrEmpty(recipients.Last().Value))
                {
                    AddPINRecipient(recipients.Last().Key, recipients.Last().Value);
                }
                else
                {
                    _controlPanel.TypeOnNumericKeypad(recipients.Last().Key);
                }
            }
            else
            {
                AddRecipientsSpeedDial(recipients);
            }

            Pacekeeper.Sync();
            Pacekeeper.Pause();
        }

        /// <summary>
        /// Adds the recipients specified as speed dials for the fax 
        /// </summary>
        /// <param name="recipients"></param>
        private void AddRecipientsSpeedDial(Dictionary<string, string> recipients)
        {
            string recipient;
            foreach (var item in recipients)
            {
                recipient = item.Key;
                _controlPanel.TypeOnNumericKeypad(recipient);
                Pacekeeper.Pause();
                if (_controlPanel.CheckState("#hpid-sendfax-recipient-autocomplete-list", OmniElementState.Useable))
                {
                    _controlPanel.Press(".hp-listitem:first");
                }
                else
                {
                    RemoveTextInput("#hpid-sendfax-recipient-textbox", recipient);
                }

            }
            recipient = recipients.Last().Key;
            _controlPanel.TypeOnNumericKeypad(recipient);
            Pacekeeper.Pause();
            if (_controlPanel.CheckState("#hpid-sendfax-recipient-autocomplete-list", OmniElementState.Useable))
            {
                _controlPanel.Press(".hp-listitem:first");
            }
            else
            {
                RemoveTextInput("#hpid-sendfax-recipient-textbox", recipient);
            }

            // Close the numeric keypad if it is visible
            if (_controlPanel.CheckState("#hpid-keypad-key-close", OmniElementState.Useable))
            {
                _controlPanel.PressWait("#hpid-keypad-key-close", "#hpid-button-sendfax-start");
            }

        }

        /// <summary>
        /// Removes the last entered text(the input element value, before accepting it as a bubble unit text)
        /// </summary>
        /// <param name="textboxID"></param>
        /// <param name="text">The text to delete</param>
        private void RemoveTextInput(string textboxID, string text)
        {
            if (text.Equals(_controlPanel.GetValue(textboxID + " .hp-control:text:nth-of-type(1)", "value", OmniPropertyType.Property)))
            {
                for (int len = 0; len < text.Length; len++)
                {
                    _controlPanel.Type(HP.DeviceAutomation.SpecialCharacter.Backspace);
                }
            }
        }

        private void AddPINRecipient(string recipient, string PIN)
        {
            _controlPanel.Press(".hp-keypad-key:contains(\"PIN\"):last");
            _controlPanel.TypeOnNumericKeypad(PIN);
            _controlPanel.Press(".hp-keypad-key:contains(\"PIN\"):last");
            _controlPanel.TypeOnNumericKeypad(recipient);
        }


        /// <summary>
        /// Retrieves the HTML string for the Fax report from Device Control Panel
        /// </summary>
        public string RetrieveFaxReport()
        {
            _controlPanel.ScrollPressWait("#hpid-reports-homescreen-button", "#hpid-reports-app-screen");
            Pacekeeper.Sync();
            _controlPanel.Press("#hpid-tree-node-listitem-faxreportspages");
            Pacekeeper.Sync();
            _controlPanel.WaitForState(_faxActivityLogCheckBoxId, OmniElementState.Useable);
            _controlPanel.Press(_faxActivityLogCheckBoxId);
            Pacekeeper.Sync();
            _controlPanel.Press("#hpid-view-button");
            Pacekeeper.Sync();
            Pacekeeper.Pause();
            string htmlReport = _controlPanel.GetValue("#hpid-report-page-data", "srcdoc", OmniPropertyType.Attribute);
            _controlPanel.Press(".hp-button-done");
            return htmlReport;
        }

        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            _executionManager.WorkflowLogger = WorkflowLogger;
            bool complete = _executionManager.ExecuteScanJob(executionOptions, "#hpid-button-sendfax-start");
            if (complete && _popupManager.HandleRetainSettingsPopup(false))
            {
                _controlPanel.WaitForState("#hpid-sendfax-landing-page", OmniElementState.Useable);
            }
            return complete;
        }

        /// <summary>
        /// Gets the <see cref="IFaxJobOptions" /> for this application.
        /// </summary>
        /// <value>The job options manager.</value>
        public IFaxJobOptions Options => _optionsManager;

        private class JediOmniFaxJobOptions : JediOmniJobOptionsManager, IFaxJobOptions
        {
            public JediOmniFaxJobOptions(JediOmniDevice device)
                : base(device)
            {
            }
        }
    }
}
