using System;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Phoenix;

namespace HP.ScalableTest.DeviceAutomation.DevicePreparation
{
    /// <summary>
    /// Implementation of <see cref="IDevicePreparationManager" /> for a <see cref="PhoenixMagicFrameDevice" />.
    /// </summary>
    public sealed class PhoenixMagicFramePreparationManager : DeviceWorkflowLogSource, IDevicePreparationManager
    {
        private readonly PhoenixMagicFrameControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoenixMagicFramePreparationManager" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public PhoenixMagicFramePreparationManager(PhoenixMagicFrameDevice device)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            _controlPanel = device.ControlPanel;
        }

        /// <summary>
        /// Initializes the device for interaction by ensuring connectivity, waking the device,
        /// clearing all settings and navigating to the home screen.
        /// </summary>
        /// <param name="performSignOut">Whether or not to sign out any current users during initialization.</param>
        public void InitializeDevice(bool performSignOut)
        {
            WakeDevice();
            NavigateHome();
        }

        /// <summary>
        /// Ensures the device is awake and ready for interaction.
        /// </summary>
        public void WakeDevice()
        {
            if (!_controlPanel.IsDisplayReady())
            {
                _controlPanel.PressScreen(1, 1);
                Wait.ForTrue(() => _controlPanel.IsDisplayReady(), TimeSpan.FromSeconds(10));
            }
        }

        /// <summary>
        /// Navigates to the home screen.  (Does not sign out.)
        /// </summary>
        public void NavigateHome()
        {
            _controlPanel.PressKey(PhoenixSoftKey.Home);
            Thread.Sleep(TimeSpan.FromSeconds(1));
        }

        /// <summary>
        /// Resets the device, ensuring it is at the home screen and no user is signed in.
        /// </summary>
        public void Reset() => NavigateHome();

        // Phoenix devices don't allow sign-in
        void IDevicePreparationManager.SignOut() { }

        /// <summary>
        /// Determines if authentication took place. If so, signing out is required.
        /// </summary>
        /// <returns>System.Boolean.</returns>
        public bool SignOutRequired()
        {
            return false;
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
