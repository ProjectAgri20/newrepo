using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Oz;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Authentication.OzWindjammer
{
    /// <summary>
    /// Oz Authenticator.  Handles all possible authentication scenarios for Oz devices.
    /// </summary>
    public class OzWindjammerAuthenticator : AuthenticatorBase
    {
        private OzWindjammerControlPanel ControlPanel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OzWindjammerAuthenticator"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="provider">The provider.</param>
        public OzWindjammerAuthenticator(OzWindjammerDevice device, AuthenticationCredential credential, AuthenticationProvider provider) : base(credential, provider)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            ControlPanel = device.ControlPanel;
        }

        /// <summary>
        /// Gets the app authenticator for the specified <see cref="AuthenticationProvider" />.
        /// </summary>
        /// <returns><see cref="IAppAuthenticator" /></returns>
        protected override IAppAuthenticator GetAppAuthenticator(AuthenticationProvider provider)
        {
            // No Factory needed here since Oz Windjammer only supports Windows Auth.

            switch (provider)
            {
                case AuthenticationProvider.Windows:
                    return new OzWindjammerWindowsAuthenticator(ControlPanel, Credential, Pacekeeper);
                default:
                    throw new DeviceInvalidOperationException($"{provider.ToString()} Authentication is not supported in Oz Windjammer.");
            }
        }

        /// <summary>
        /// Gets the default authentication setting from the device.
        /// Oz only supports Windows authentication
        /// </summary>
        /// <returns><see cref="AuthenticationProvider" /></returns>
        protected override AuthenticationProvider GetDefaultProvider() => AuthenticationProvider.Windows;

    }
}
