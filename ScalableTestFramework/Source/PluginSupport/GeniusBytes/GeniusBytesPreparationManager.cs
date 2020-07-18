using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Jedi.OmniUserInteraction;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DevicePreparation;
using HP.ScalableTest.DeviceAutomation.SolutionApps.GeniusBytes;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.PluginSupport.GeniusBytes
{
    /// <summary>
    /// Implementation of <see cref="JediOmniPreparationManager" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public class GeniusBytesPreparationManager : JediOmniPreparationManager
    {
        IGeniusBytesApp _geniusBytesApp;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniPreparationManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public GeniusBytesPreparationManager(JediOmniDevice device)
            : base(device) { }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniPreparationManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="geniusBytesApp">GeniusBytes solution app</param>
        public GeniusBytesPreparationManager(JediOmniDevice device, IGeniusBytesApp geniusBytesApp)
            : base(device)
        {
            _geniusBytesApp = geniusBytesApp;
        }

        /// <summary>
        /// Initializes the device for interaction by ensuring connectivity, waking the device,
        /// clearing all settings and navigating to the home screen.
        /// </summary>
        /// <param name="performSignOut">Whether or not to sign out any current users during initialization.</param>
        public override void InitializeDevice(bool performSignOut)
        {
            // Enable SystemTest mode if necessary.  Normally DAT takes care of this for us,
            // but in this case we are using a lower-layer call that circumvents the control panel
            // objects that will check for that case.
            bool success = _device.SystemTest.Enable();
            if (!success && _device.SystemTest.IsSupported())
            {
                LogWarn("Unable to enable SystemTest mode.");
            }

            // If all inspectable pages are in use, force disconnect one.
            using (WebInspector inspector = new WebInspector(_device.Address, 9222, TimeSpan.FromSeconds(30)))
            {
                var inspectablePages = inspector.DiscoverInspectablePages();
                if (inspectablePages.Any() && inspectablePages.All(n => n.ClientConnected))
                {
                    LogWarn($"Disconnecting inspectable page connection from {_device.Address}.");
                    inspector.ForceDisconnect(inspectablePages.First());
                }
            }

            WakeDevice();
            NavigateHome();
            PressExitButton();
        }

        /// <summary>
        /// Navigate to home screen
        /// </summary>
        public override void NavigateHome()
        {
            bool homeScreen = false;
            string screen = GetTopmostScreen();

            TimeSpan waitingTime = TimeSpan.FromMilliseconds(1000);

            if ((homeScreen = AtHomeScreen()) == false)
            {
                RecordEvent(DeviceWorkflowMarker.NavigateHomeBegin);                
                
                for (int i = 0; i < 10; i++)
                {
                    // Pressing the home button should get us back to the home screen,
                    if (_geniusBytesApp.WaitObjectForAvailable("Warning", waitingTime))
                    {
                        _geniusBytesApp.Confirm();
                    }
                    else if (_geniusBytesApp.WaitObjectForAvailable("OK", waitingTime))
                    {
                        _geniusBytesApp.PressOKKey();
                    }
                    else if (_geniusBytesApp.WaitObjectForAvailable("Close", waitingTime))
                    {
                        _geniusBytesApp.PressCloseKey();
                    }
                    else if (_geniusBytesApp.WaitObjectForAvailable("Cancel", waitingTime))
                    {
                        _geniusBytesApp.PressCancelKey();
                    }
                    else if (_geniusBytesApp.WaitObjectForAvailable("contextView-back", waitingTime))
                    {
                        _geniusBytesApp.PressBackKey();
                    }
                    else if (_geniusBytesApp.WaitObjectForAvailable("mainmenu-logout-normal", waitingTime))
                    {
                        _geniusBytesApp.SignOut();
                    }                     

                    Wait.ForChange(GetTopmostScreen, screen, TimeSpan.FromSeconds(3));
                    if (AtHomeScreen())
                    {
                        homeScreen = true;
                        RecordEvent(DeviceWorkflowMarker.NavigateHomeEnd);
                        break;
                    }
                }
            }

            if (!homeScreen)
            {
                // If we're not there after 10 tries, we're not going to make it.
                throw new DeviceWorkflowException($"Unable to navigate to home screen. Top most screen is '{screen}'.");
            }
            GetTopmostScreen();
        }                

        /// <summary>
        /// Check control pannel is at home screen
        /// </summary>
        /// <returns></returns>
        protected override bool AtHomeScreen()
        {
            return !_controlPanel.CheckState("#hpid-button-reset", OmniElementState.Hidden);                
        }
    }
}
