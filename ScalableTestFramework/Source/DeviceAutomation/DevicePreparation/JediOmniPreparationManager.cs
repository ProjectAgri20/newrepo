using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Jedi.OmniUserInteraction;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.DevicePreparation
{
    /// <summary>
    /// Implementation of <see cref="IDevicePreparationManager" /> for a <see cref="JediOmniDevice" />.
    /// </summary>
    public class JediOmniPreparationManager : DeviceWorkflowLogSource, IDevicePreparationManager
    {
        private const string _sleepOverlay = "#hpid-sleep-modal-overlay";
        private const string _homeScreenLogo = "#hpid-homescreen-logo-icon";
        private const string _resetButton = "#hpid-button-reset";
        private const string _notificationPanel = "#hpid-notification-panel-notification";
        private const string _messageCenterExitButton = "#hpid-message-center-exit-button";
        private const string _topViewAttribute = "[hp-global-top-view=true]";

        protected readonly JediOmniDevice _device;
        protected readonly JediOmniControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniPreparationManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniPreparationManager(JediOmniDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _device = device;
            _controlPanel = _device.ControlPanel;
        }

        /// <summary>
        /// Initializes the device for interaction by ensuring connectivity, waking the device,
        /// clearing all settings and navigating to the home screen.
        /// </summary>
        /// <param name="performSignOut">Whether or not to sign out any current users during initialization.</param>
        public virtual void InitializeDevice(bool performSignOut)
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

            if (performSignOut && SignOutRequired())
            {
                SignOut();
                PressExitButton();
            }
        }

        /// <summary>
        /// Ensures the device is awake and ready for interaction.
        /// </summary>
        public void WakeDevice()
        {
            try
            {
                // Introduced in Jedi version 24.5
                _device.ControlPanel.SignalUserActivity();
            }
            catch (Exception ex) when (ex is DeviceInvalidOperationException || ex is DeviceCommunicationException)
            {
                _device.PowerManagement.Wake();
            }

            if (!_device.ControlPanel.WaitForState(_sleepOverlay, OmniElementState.Exists, false))
            {
                throw new DeviceWorkflowException($"Unable to wake device {_device.Address}.");
            }
            // Wake the device and check if the notification dropdown appears, if so, wait for it to go away
            // Sometimes two messages show back to back, leaving sleep, initializing scanner...
            _controlPanel.WaitForState(_notificationPanel, OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(3));
            _controlPanel.WaitForState(JediOmniLaunchHelper.SignInOrSignoutButton, OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Navigates to the home screen.  (Does not necessarily sign out.)
        /// </summary>
        public virtual void NavigateHome()
        {
            bool homeScreen = false;
            string screen = GetTopmostScreen();

            if ((homeScreen = AtHomeScreen()) == false)
            {
                RecordEvent(DeviceWorkflowMarker.NavigateHomeBegin);
                // Pressing the home button should get us back to the home screen,
                // but if a dialog pops up, we might need to press home again to dismiss it.
                for (int i = 0; i < 10; i++)
                {
                    // Press the home button, then give it a few seconds for the topmost screen to change
                    screen = GetTopmostScreen();
                    _controlPanel.PressHome();
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
        }

        /// <summary>
        /// Determines if a user is currently logged into the device. If so, signing out is required.
        /// </summary>
        /// <returns><c>true</c> if a user is logged in, <c>false</c> otherwise.</returns>
        public bool SignOutRequired()
        {
            bool signOutRequired = true;
            string text = _controlPanel.GetValue(JediOmniLaunchHelper.SignInOrSignoutButton, "innerText", OmniPropertyType.Property);
            if (!text.Contains("Sign Out"))
            {
                signOutRequired = false;
            }
            return signOutRequired;
        }

        /// <summary>
        /// Signs out of the device.  The device must already be on the correct screen for sign out.
        /// </summary>
        public void SignOut()
        {
            SignOut(DeviceSignOutMethod.PressSignOut);
        }

        /// <summary>
        /// Signs out of the device using the provided sign out method
        /// </summary>
        /// <param name="signOutMethod">The sign out method.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void SignOut(DeviceSignOutMethod signOutMethod)
        {
            RecordInfo(DeviceWorkflowMarker.SignOutType, signOutMethod.ToString());
            switch (signOutMethod)
            {
                case DeviceSignOutMethod.PressSignOut:
                    NavigateHome();
                    RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);
                    PressSignOutButton();
                    break;
                case DeviceSignOutMethod.Timeout:
                    RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);
                    SignOutByTimeout();
                    break;
                case DeviceSignOutMethod.DoNotSignOut:
                    break;
                default:
                    throw new DeviceInvalidOperationException("Jedi Omni devices do not support " + EnumUtil.GetDescription(signOutMethod));
            }
            RecordEvent(DeviceWorkflowMarker.DeviceSignOutEnd);
        }

        private void SignOutByTimeout()
        {
            DateTime maxTime = DateTime.UtcNow.AddMinutes(10);
            if (Wait.ForTrue(() => AtHomeScreen(), maxTime.Subtract(DateTime.UtcNow)))
            {
                // Verify
                if (DateTime.UtcNow >= maxTime
                     || !Wait.ForTrue(() => IsUserSignedOut(), maxTime.Subtract(DateTime.UtcNow))
                   )
                {
                    throw new DeviceInvalidOperationException("Did not sign out within inactivity timeout");
                }
            }
            else
            {
                throw new DeviceInvalidOperationException("Did not return to home screen within inactivity timeout");
            }
        }

        private bool IsUserSignedOut()
        {
            string buttonText = _controlPanel.GetValue(JediOmniLaunchHelper.SignInOrSignoutButton, "innerText", OmniPropertyType.Property);
            return buttonText.StartsWith("Sign In");
        }

        private void PressSignOutButton()
        {
            if (_controlPanel.CheckState(JediOmniLaunchHelper.SignInOrSignoutButton, OmniElementState.Useable))
            {
                string text = _controlPanel.GetValue(JediOmniLaunchHelper.SignInOrSignoutButton, "innerText", OmniPropertyType.Property);
                if (text.Contains("Sign Out"))
                {
                    // Press the sign out button, then wait for the notification dropdown to appear and then disappear
                    _controlPanel.Press(JediOmniLaunchHelper.SignInOrSignoutButton);
                    _controlPanel.WaitForState(_notificationPanel, OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(5));
                    _controlPanel.WaitForState(JediOmniLaunchHelper.SignInOrSignoutButton, OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(20));
                }
            }
            else
            {
                throw new DeviceWorkflowException("Sign out button is not usable.");
            }
        }

        /// <summary>
        /// Resets the device, ensuring it is at the home screen and no user is signed in.
        /// </summary>
        public void Reset()
        {
            // Return to the home screen and sign out so that the Reset button is available
            NavigateHome();

            if (SignOutRequired())
            {
                SignOut();
            }

            // Press the reset button, then wait for the notification dropdown to appear and then disappear
            _controlPanel.WaitForState(_resetButton, OmniElementState.Useable, TimeSpan.FromSeconds(5));
            _controlPanel.Press(_resetButton);
            _controlPanel.WaitForState(_notificationPanel, OmniElementState.VisiblePartially, TimeSpan.FromSeconds(5));
            _controlPanel.WaitForState(_resetButton, OmniElementState.VisibleCompletely, TimeSpan.FromSeconds(5));

            PressExitButton();
        }

        /// <summary>
        /// Checks ControlPanel is at homescreen.
        /// </summary>
        /// <returns></returns>
        protected virtual bool AtHomeScreen()
        {
            return _controlPanel.CheckState($".hp-homescreen-folder-view{_topViewAttribute}", OmniElementState.Exists)
                && _controlPanel.CheckState(_homeScreenLogo, OmniElementState.VisibleCompletely) ||
                _controlPanel.CheckState(_homeScreenLogo, OmniElementState.VisibleCompletely); //임의수정
        }

        /// <summary>
        /// Gets attribute of TopmostScreen.
        /// </summary>
        /// <returns></returns>
        protected string GetTopmostScreen()
        {
            if (_controlPanel.GetCount(_topViewAttribute) > 1)
            {
                return "Multiple screens";
            }
            else if (_controlPanel.CheckState($"{_topViewAttribute}[id]", OmniElementState.Exists))
            {
                return _controlPanel.GetValue($"{_topViewAttribute}[id]", "id", OmniPropertyType.Property);
            }
            else if (_controlPanel.CheckState($"{_topViewAttribute}[class]", OmniElementState.Exists))
            {
                return _controlPanel.GetValue($"{_topViewAttribute}[class]", "class", OmniPropertyType.Property);
            }
            else
            {
                return null;
            }
        }

        /// <summary>
        /// If there is a warning message (e.g. low toner) it will popup after the sign out button is pressed.
        /// We have to navigate home again.
        /// </summary>
        protected void PressExitButton()
        {            
            if (_controlPanel.CheckState(_messageCenterExitButton, OmniElementState.Useable))
            {
                _controlPanel.PressWait(_messageCenterExitButton, _resetButton);
            }
        }

    }
}
