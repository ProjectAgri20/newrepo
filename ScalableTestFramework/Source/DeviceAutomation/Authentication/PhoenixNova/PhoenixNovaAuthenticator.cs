using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Phoenix;

namespace HP.ScalableTest.DeviceAutomation.Authentication.PhoenixNova
{
    /// <summary>
    /// Phoenix Nova Authenticator.  Handles all possible authentication scenarios for Phoenix Nova devices.
    /// </summary>
    public class PhoenixNovaAuthenticator : AuthenticatorBase
    {
        private PhoenixNovaControlPanel ControlPanel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoenixNovaAuthenticator"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="provider">The provider.</param>
        public PhoenixNovaAuthenticator(PhoenixNovaDevice device, AuthenticationCredential credential, AuthenticationProvider provider) : base(credential, provider)
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
                    return new PhoenixNovaWindowsAuthenticator(ControlPanel, Credential, Pacekeeper);
                default:
                    throw new DeviceInvalidOperationException($"{provider} Authentication is not supported in Phoenix Nova.");
            }
        }

        /// <summary>
        /// Gets the default authentication setting from the device.
        /// Nova only supports Windows authentication
        /// </summary>
        /// <returns><see cref="AuthenticationProvider" /></returns>
        protected override AuthenticationProvider GetDefaultProvider() => AuthenticationProvider.Windows;
 
    }
}
