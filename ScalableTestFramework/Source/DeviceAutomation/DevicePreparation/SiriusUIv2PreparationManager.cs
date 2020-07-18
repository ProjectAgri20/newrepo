using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.DevicePreparation
{
    /// <summary>
    /// Implementation of <see cref="IDevicePreparationManager" /> for a <see cref="SiriusUIv2Device" />.
    /// </summary>
    public sealed class SiriusUIv2PreparationManager : DeviceWorkflowLogSource, IDevicePreparationManager
    {
        private readonly SiriusUIv2Device _device;
        private readonly SiriusUIv2ControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv2PreparationManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public SiriusUIv2PreparationManager(SiriusUIv2Device device)
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
            while (DateTime.UtcNow < timeout)
            {
                ScreenInfo screen = _controlPanel.GetScreenInfo();

                // Check to see if the home screen is displayed
                if (screen.ScreenLabels.Contains("view_home"))
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
                else if (screen.Leds[SiriusLed.Cancel] == LedState.On)
                {
                    _controlPanel.PressKey(SiriusSoftKey.Cancel);
                }
                else if (screen.ScreenLabels.Any(n => n.Contains("warning", StringComparison.OrdinalIgnoreCase)
                                                   || n.Contains("error", StringComparison.OrdinalIgnoreCase)))
                {
                    // If a warning or error screen is displayed and only an OK button is available, press it
                    var buttons = screen.Widgets.Where(n => n.HasAction(WidgetAction.Select)).ToList();
                    if (buttons.Count == 1 && buttons.First().HasValue("OK"))
                    {
                        _controlPanel.Press(buttons.First());
                    }
                }
                else
                {
                    // Nothing to do at this point - wait a second and see what changes
                    Thread.Sleep(TimeSpan.FromSeconds(1));
                }
            }

            // If we're not there after 60 seconds, we're not going to make it.
            throw new DeviceWorkflowException("Unable to navigate to home screen.");
        }

        /// <summary>
        /// Signs out of the device.  The device must already be on the correct screen for sign out.
        /// </summary>
        public void SignOut()
        {
            try
            {
                if (_device.ControlPanel.GetScreenInfo().Widgets.Find("home_oxpd_sign_in_out").HasValue("Sign Out"))
                {
                    _device.ControlPanel.PressByValue("Sign Out", StringMatch.StartsWith);
                    Thread.Sleep(TimeSpan.FromSeconds(3));

                    _device.ControlPanel.WaitForActiveScreenLabel("view_oxpd_signout", TimeSpan.FromSeconds(5));
                    _device.ControlPanel.Press("gr_footer_yes_no.1");
                    Thread.Sleep(TimeSpan.FromSeconds(3));
                }
            }
            catch (ElementNotFoundException)
            {
                // No sign out button.  Ignore.
            }
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
        public bool SignOutRequired() => (_device.ControlPanel.GetScreenInfo().Widgets.Find("home_oxpd_sign_in_out").HasValue("Sign Out"));

        /// <summary>
        /// Represents an event that is raised when the sign-out operation is complete.
        /// </summary>
        /// <param name="signOutMethod">The sign out method.</param>
        /// <exception cref="NotImplementedException"></exception>
        public void SignOut(DeviceSignOutMethod signOutMethod)
        {
            throw new NotImplementedException();
        }
    }
}
