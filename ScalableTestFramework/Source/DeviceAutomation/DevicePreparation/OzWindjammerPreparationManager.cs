using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation.Oz;

namespace HP.ScalableTest.DeviceAutomation.DevicePreparation
{
    /// <summary>
    /// Implementation of <see cref="IDevicePreparationManager" /> for an <see cref="OzWindjammerDevice" />.
    /// </summary>
    public sealed class OzWindjammerPreparationManager : DeviceWorkflowLogSource, IDevicePreparationManager
    {
        private const int _homeScreenId = 567;
        private readonly OzWindjammerDevice _device;
        private readonly OzWindjammerControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="OzWindjammerPreparationManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public OzWindjammerPreparationManager(OzWindjammerDevice device)
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
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Resets the device, ensuring it is at the home screen and no user is signed in.
        /// </summary>
        public void Reset()
        {
            // Press the hard key and wait for the home screen
            _controlPanel.PressKey(OzHardKey.Reset);
            bool success = _controlPanel.WaitForScreen(_homeScreenId, TimeSpan.FromSeconds(10));

            // If we didn't make it to the home screen, see if there is a "hide" button someplace
            if (!success)
            {
                var hideButton = _controlPanel.GetWidgets().FirstOrDefault(n => n.Text == "Hide");
                if (hideButton != null)
                {
                    _controlPanel.Press(hideButton);
                    _controlPanel.WaitForScreen(_homeScreenId, TimeSpan.FromSeconds(10));
                }
            }
        }

        void IDevicePreparationManager.NavigateHome() => Reset();
        void IDevicePreparationManager.SignOut() => Reset();

        /// <summary>
        /// Determines if authentication took place. If so, signing out is required.
        /// </summary>
        /// <returns>System.Boolean.</returns>
        public bool SignOutRequired()
        {
            throw new NotImplementedException();
        }

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
