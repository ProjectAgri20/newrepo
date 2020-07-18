using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation.Helpers.SiriusUIv3;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.DevicePreparation
{
    /// <summary>
    /// Implementation of <see cref="IDevicePreparationManager" /> for a <see cref="SiriusUIv3Device" />.
    /// </summary>
    public sealed class SiriusUIv3PreparationManager : DeviceWorkflowLogSource, IDevicePreparationManager
    {
        private readonly SiriusUIv3Device _device;
        private readonly SiriusUIv3ControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3PreparationManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public SiriusUIv3PreparationManager(SiriusUIv3Device device)
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
            DateTime timeout = DateTime.UtcNow + TimeSpan.FromSeconds(60);
            string message = string.Empty;

            while (DateTime.UtcNow < timeout)
            {
                ScreenInfo screen = _controlPanel.GetScreenInfo();

                // Check to see if the home screen is displayed
                // If it's SafeCom, the home screen has a different label
                if (screen.ScreenLabels.Contains("Home") || screen.ScreenLabels.Contains("vw_auth_blocking"))
                {
                    return;
                }

                // Check to see if a soft key can be pressed
                if (screen.Leds[SiriusLed.Home] == LedState.On)
                {
                    _controlPanel.PressKey(SiriusSoftKey.Home);
                }
                else if (screen.Leds[SiriusLed.Back] == LedState.On)
                {
                    _controlPanel.PressKey(SiriusSoftKey.Back);
                }
                else if (screen.ScreenLabels.Any(n => n.Contains("warning", StringComparison.OrdinalIgnoreCase) ||
                                                      n.Contains("error", StringComparison.OrdinalIgnoreCase)))
                {
                    SiriusUIv3PopupManager popupManager = new SiriusUIv3PopupManager(_controlPanel);
                    message = popupManager.HandleAny(screen);
                    if (!string.IsNullOrEmpty(message))
                    {
                        //Popup was handled.
                        break;
                    }
                }
                else
                {
                    // Nothing to do at this point - wait a second and see what changes
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }

            // If we're not there after 60 seconds, we're not going to make it.
            // If no error message has been set, use a default.
            if (string.IsNullOrEmpty(message))
            {
                message = "Unable to navigate to home screen.";
            }

            throw new DeviceWorkflowException(message);

        }

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
            // Same as returning to the home screen and signing out
            NavigateHome();
            SignOut();
        }

        /// <summary>
        /// Determines if authentication took place. If so, signing out is required.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public bool SignOutRequired()
        {
            Widget signOutWidget = null;
            try
            {
                signOutWidget = _device.ControlPanel.GetScreenInfo().Widgets.FindByValue("Sign Out");
                return (signOutWidget != null);
            }
            catch (ElementNotFoundException)
            {
                //Assume Signed Out
            }
            return false;
        }

        /// <summary>
        /// Signs out of the device using the provided sign out method
        /// </summary>
        /// <param name="signOutMethod">The sign out method.</param>
        /// <exception cref="HP.ScalableTest.DeviceAutomation.DeviceWorkflowException">SiriusUIv3 devices do not support  + EnumUtil.GetDescription(signOutMethod)</exception>
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
                default:
                    throw new DeviceWorkflowException("SiriusUIv3 devices do not support " + EnumUtil.GetDescription(signOutMethod));
            }
            RecordEvent(DeviceWorkflowMarker.DeviceSignOutEnd);
        }

        private void SignOutByTimeout()
        {
            TimeSpan waitTimeSpan = TimeSpan.FromMinutes(10);
            DateTime maxTime = DateTime.UtcNow.Add(waitTimeSpan);

            if (_controlPanel.WaitForScreenLabel("Home", waitTimeSpan))
            {
                // Verify
                if (!Wait.ForTrue(() => IsUserSignedOut(), maxTime.Subtract(DateTime.UtcNow)))
                {
                    throw new DeviceInvalidOperationException("Did not sign out within inactivity timeout.");
                }
            }
            else
            {
                throw new DeviceInvalidOperationException("Did not return to home screen within inactivity timeout.");
            }
        }

        private bool IsUserSignedOut() => _controlPanel.GetScreenInfo().Widgets.Any(w => w.HasValue("Sign In"));

        private void PressSignOutButton()
        {
            try
            {
                _device.ControlPanel.WaitForScreenLabel("Home", TimeSpan.FromSeconds(5));
                //slight delay induced to allow for screen transition
                Thread.Sleep(200);
                var signOutWidget = _device.ControlPanel.WaitForWidgetByValue("Sign Out", TimeSpan.FromSeconds(5));
                //signOutWidget = _device.ControlPanel.GetScreenInfo().Widgets.FindByValue("Sign Out");
                if (signOutWidget != null)
                {
                    _device.ControlPanel.Press(signOutWidget);
                    //Confirm on sign out
                    _device.ControlPanel.WaitForScreenId("flow_auth::st_auth_signout", TimeSpan.FromSeconds(5));
                    _device.ControlPanel.Press("mdlg_action_button");
                }
            }
            catch (ElementNotFoundException)
            {
                //Assume Signed Out
            }
        }
    }
}
