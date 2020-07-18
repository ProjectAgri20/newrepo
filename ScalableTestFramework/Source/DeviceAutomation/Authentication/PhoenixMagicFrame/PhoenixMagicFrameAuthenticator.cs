using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Phoenix;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Authentication.PhoenixMagicFrame
{
    /// <summary>
    /// Phoenix MagicFrame Authenticator.  Handles all possible authentication scenarios for Phoenix MagicFrame devices.
    /// </summary>
    public class PhoenixMagicFrameAuthenticator : AuthenticatorBase
    {
        private PhoenixMagicFrameControlPanel ControlPanel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoenixMagicFrameAuthenticator"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="provider">The provider.</param>
        public PhoenixMagicFrameAuthenticator(PhoenixMagicFrameDevice device, AuthenticationCredential credential, AuthenticationProvider provider) : base(credential, provider)
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
                    return new PhoenixMagicFrameWindowsAuthenticator(ControlPanel, Credential, Pacekeeper);
                default:
                    throw new DeviceInvalidOperationException($"{provider} Authentication is not supported in Phoenix MagicFrame.");
            }
        }

        /// <summary>
        /// Gets the default authentication setting from the device.
        /// Magicframe only supports Windows authentication
        /// </summary>
        /// <returns><see cref="AuthenticationProvider" /></returns>
        protected override AuthenticationProvider GetDefaultProvider() => AuthenticationProvider.Windows;

    }
}
