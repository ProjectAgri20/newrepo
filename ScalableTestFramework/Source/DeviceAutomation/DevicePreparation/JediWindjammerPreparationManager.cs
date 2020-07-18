using System;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.DevicePreparation
{
    /// <summary>
    /// Implementation of <see cref="IDevicePreparationManager" /> for a <see cref="JediWindjammerDevice" />.
    /// </summary>
    public sealed class JediWindjammerPreparationManager : DeviceWorkflowLogSource, IDevicePreparationManager
    {
        private const string _homeScreenForm = "HomeScreenForm";
        private const string _signInButton = "mSignInButton";

        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerPreparationManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerPreparationManager(JediWindjammerDevice device)
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
        public void InitializeDevice(bool performSignOut)
        {
            WakeDevice();
            Reset();
        }

        /// <summary>
        /// Ensures the device is awake and ready for interaction.
        /// </summary>
        public void WakeDevice()
        {
            _device.PowerManagement.Wake();
        }

        /// <summary>
        /// Navigates to the home screen.  (Does not sign out.)
        /// </summary>
        public void NavigateHome()
        {
            if (_controlPanel.CurrentForm() == _homeScreenForm)
            {
                return;
            }

            // Press the menu (home) button, then wait until the home screen appears
            // If there is a job running, we might have to press it a couple times.
            for (int i = 0; i < 10; i++)
            {
                if (_controlPanel.PressKeyWait(JediHardKey.Menu, _homeScreenForm, TimeSpan.FromSeconds(3)))
                {
                    return;
                }
            }

            // If we're not there after 10 tries, we're not going to make it.
            throw new DeviceWorkflowException("Unable to navigate to home screen.");
        }
        /// <summary>
        ///  Determines if authentication took place. If so, signing out is required.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        public bool SignOutRequired() => (_controlPanel.GetProperty(_signInButton, "Text") == "Sign Out");

        /// <summary>
        /// Signs out of the device.  The device must already be on the correct screen for sign out.
        /// </summary>
        public void SignOut()
        {
            SignOut(DeviceSignOutMethod.PressSignOut);
        }

        /// <summary>
        /// Resets the device, ensuring it is at the home screen and no user is signed in.
        /// </summary>
        public void Reset()
        {
            _controlPanel.PressKeyWait(JediHardKey.Reset, _homeScreenForm);
            Thread.Sleep(1000);
            if (_controlPanel.CurrentForm().Equals("TimedMessageBox"))
            {
                _controlPanel.WaitForForm(_homeScreenForm, true);
            }
        }

        /// <summary>
        /// Signs the out.
        /// </summary>
        /// <param name="signOutMethod">The sign out method.</param>
        public void SignOut(DeviceSignOutMethod signOutMethod)
        {
            RecordInfo(DeviceWorkflowMarker.SignOutType, signOutMethod.ToString());
            RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);

            switch (signOutMethod)
            {
                case DeviceSignOutMethod.PressSignOut:
                    NavigateHome();
                    PressSignOutButton();
                    break;
                case DeviceSignOutMethod.Timeout:
                    SignOutByTimeout();
                    break;
                case DeviceSignOutMethod.PressResetHardKey:
                    Reset();
                    break;
                case DeviceSignOutMethod.PressResetSoftKey:
                    SignOutByResetSoftKey();
                    break;
                default:
                    throw new WindjammerInvalidOperationException("Jedi Windjammer devices do not support " + EnumUtil.GetDescription(signOutMethod));
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
                    throw new WindjammerInvalidOperationException("Did not sign out within inactivity timeout");
                }
            }
            else
            {
                throw new WindjammerInvalidOperationException("Did not return to home screen within inactivity timeout");
            }
        }

        private bool AtHomeScreen() => _controlPanel.CurrentForm().Equals(JediWindjammerLaunchHelper.HOMESCREEN_FORM);
        /// <summary>
        /// Verifies the signed out.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        private bool IsUserSignedOut()
        {
            return
            AtHomeScreen()
            && _controlPanel.WaitForControl(JediWindjammerLaunchHelper.SIGNIN_BUTTON, TimeSpan.FromSeconds(1))
            && _controlPanel.GetProperty(JediWindjammerLaunchHelper.SIGNIN_BUTTON, "Text").EqualsIgnoreCase("Sign In");
        }
        private void PressSignOutButton()
        {
            try
            {
                if (_controlPanel.GetProperty(_signInButton, "Text") == "Sign Out")
                {
                    _controlPanel.Press(_signInButton);
                }
            }
            catch (WindjammerInvalidOperationException wje) when (wje.Message.Contains("Did not receive the Click event from the control within 5 seconds"))
            {
                // ignoring the error since this only occasionally fails on the end of SafeCom workflows
            }
            catch (ControlNotFoundException)
            {
                throw new DeviceWorkflowException("Sign out button could not be found.");
            }
        }

        private void SignOutByResetSoftKey()
        {
            NavigateHome();
            _controlPanel.Press("mResetButton");

            // Verify
            if (!Wait.ForTrue(() => IsUserSignedOut(), TimeSpan.FromSeconds(15)))
            {
                throw new DeviceInvalidOperationException("User not signed out");
            }
        }
    }
}
