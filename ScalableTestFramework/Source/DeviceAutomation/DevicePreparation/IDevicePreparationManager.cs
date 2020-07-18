namespace HP.ScalableTest.DeviceAutomation.DevicePreparation
{
    /// <summary>
    /// Interface for preparing devices for test by waking them, returning to the home screen, etc.
    /// </summary>
    public interface IDevicePreparationManager: IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Initializes the device for interaction by ensuring connectivity, waking the device,
        /// clearing all settings and navigating to the home screen.
        /// </summary>
        /// <param name="performSignOut">Whether or not to sign out any current users during initialization.</param>
        void InitializeDevice(bool performSignOut);

        /// <summary>
        /// Ensures the device is awake and ready for interaction.
        /// </summary>
        void WakeDevice();

        /// <summary>
        /// Navigates to the home screen.  (Does not sign out.)
        /// </summary>
        void NavigateHome();

        /// <summary>
        /// Determines if authentication took place. If so, signing out is required.
        /// </summary>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        bool SignOutRequired();

        /// <summary>
        /// Signs out of the device.  The device must already be on the correct screen for sign out.
        /// </summary>
        void SignOut();

        /// <summary>
        /// Signs out of the device using the provided sign out method
        /// </summary>
        /// <param name="signOutMethod">The sign out method.</param>
        void SignOut(DeviceSignOutMethod signOutMethod);

        /// <summary>
        /// Resets the device, ensuring it is at the home screen and no user is signed in.
        /// </summary>
        void Reset();
    }
}
