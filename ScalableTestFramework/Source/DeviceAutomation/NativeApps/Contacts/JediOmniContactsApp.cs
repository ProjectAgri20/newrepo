using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Contacts
{

    /// <summary>
    /// Implementation of <see cref="IContactsApp" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public sealed class JediOmniContactsApp : DeviceWorkflowLogSource, IContactsApp
    {
        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private Pacekeeper _pacekeeper;
        private JediOmniPreparationManager _preparationManager;

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
               // _optionsManager.Pacekeeper = _pacekeeper;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniContactsApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniContactsApp(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
           // _optionsManager = new JediOmniFaxJobOptions(device);
            _preparationManager = new JediOmniPreparationManager(device);
            Pacekeeper = new Pacekeeper(TimeSpan.Zero);
        }

        /// <summary>
        /// Opens Contacts screen with the specified authenticator using the given authentication mode.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {                
                if (PressContactsHomescreenButton())
                {
                    // sign in form is present
                    Authenticate(authenticator, "#hpid-contacts-app-screen");
                }
            }                         
            else
            {
                throw new NotImplementedException("Eager authentication has not been implemented for this solution.");
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        private bool PressContactsHomescreenButton()
        {
            bool signInScreenLoaded = false;

            // Determine whether the Contacts Button is on the home screen 
            if (_controlPanel.CheckState("#hpid-contacts-homescreen-button", OmniElementState.Exists))
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Contacts");
                try
                {
                    signInScreenLoaded = _controlPanel.ScrollPressWait("#hpid-contacts-homescreen-button", "#hpid-signin-body", TimeSpan.FromSeconds(6));
                }
                catch (Exception)
                {
                    signInScreenLoaded = false;
                }

                if (!signInScreenLoaded)
                {
                    if (!_controlPanel.CheckState("#hpid-contacts-app-screen", OmniElementState.Exists))
                    {
                        _controlPanel.ScrollPress("#hpid-contacts-homescreen-button");
                            if (!_controlPanel.ScrollPressWait("#hpid-contacts-homescreen-button", "#hpid-app-record-list", TimeSpan.FromSeconds(30)))
                            {
                                // The application launched without prompting for credentials or moving to the contacts application string
                                throw new DeviceWorkflowException("Could not launch contacts application.");
                            }
                        
                    }
                }
                Pacekeeper.Pause();
            }
            else
            {
                throw new DeviceWorkflowException("Contacts application button was not found on device home screen.");
            }

            return signInScreenLoaded;

        }

        /// <summary>
        /// Creates a speed dial contact
        /// </summary>
        public int CreateSpeedDial(string DisplayName, string SpeedDial, string FaxNumber)
        {
            int speedDialNumber;
            _controlPanel.PressWait(".hp-address-book-selection-button", ".hp-contact-record-list");
            _controlPanel.PressWait(".hp-listitem:contains(" + "Fax Speed Dials)", ".hp-contact-record-list");
            _controlPanel.PressWait("#hpid-contacts-add-record-button", "#hpid-fax-speed-dial-edit-view");

            Pacekeeper.Sync();
            Pacekeeper.Pause();
            if(_controlPanel.WaitForState("#hpid-fax-speed-dial-edit-display-name-textbox", OmniElementState.Useable, TimeSpan.FromSeconds(5)))
            {
                _controlPanel.Press("#hpid-fax-speed-dial-edit-display-name-textbox");
            }            
            _controlPanel.TypeOnVirtualKeyboard(DisplayName);
            if (_controlPanel.WaitForState("#hpid-fax-speed-dial-edit-speed-dial-code-textbox", OmniElementState.Useable, TimeSpan.FromSeconds(5)))
            {
                _controlPanel.PressWait("#hpid-fax-speed-dial-edit-speed-dial-code-textbox", "#hpid-keypad");
            }
            _controlPanel.TypeOnNumericKeypad(SpeedDial);

            if (_controlPanel.WaitForState("#hpid-fax-speed-dial-edit-fax-numbers-textbox", OmniElementState.Useable, TimeSpan.FromSeconds(5)))
            {
                _controlPanel.PressWait("#hpid-fax-speed-dial-edit-fax-numbers-textbox", "#hpid-keypad");
            }
            _controlPanel.TypeOnNumericKeypad(FaxNumber);
            if (_controlPanel.WaitForState("#hpid-keypad-key-close", OmniElementState.Useable, TimeSpan.FromSeconds(5)))
            {
                _controlPanel.Press("#hpid-keypad-key-close");
            }

            string autoSpeedDial;
            autoSpeedDial = _controlPanel.GetValue("#hpid-fax-speed-dial-edit-speed-dial-code-textbox", "value", OmniPropertyType.Property);
            speedDialNumber = int.Parse(autoSpeedDial);
            _controlPanel.Press("#hpid-fax-speed-dial-edit-save-button");
            Pacekeeper.Sync();
            Pacekeeper.Pause();
            if (_controlPanel.WaitForState(".hp-button-done", OmniElementState.Useable, TimeSpan.FromSeconds(10)))
            {
                _controlPanel.Press(".hp-button-done");
                _pacekeeper.Pause();
            }                              
            _preparationManager.NavigateHome();

            return speedDialNumber;
        }

        /// <summary>
        /// Authenticates the specified authenticator.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">The wait form.</param>
        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            _controlPanel.WaitForAvailable(waitForm);
            Pacekeeper.Pause();
        }
    }
}
