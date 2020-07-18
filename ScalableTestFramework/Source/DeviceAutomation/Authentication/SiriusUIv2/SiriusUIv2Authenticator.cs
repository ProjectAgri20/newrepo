using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv2
{
    /// <summary>
    /// Sirius UIv2 Authenticator.  Handles all possible authentication scenarios for Sirius UIv2 devices.
    /// </summary>
    public class SiriusUIv2Authenticator : AuthenticatorBase
    {
        private SiriusUIv2ControlPanel ControlPanel { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv2Authenticator"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="credential">The credential.</param>
        /// <param name="provider">The provider.</param>
        public SiriusUIv2Authenticator(SiriusUIv2Device device, AuthenticationCredential credential, AuthenticationProvider provider) : base(credential, provider)
        {
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
                case AuthenticationProvider.DSS:
                case AuthenticationProvider.LDAP:
                    return new SiriusUIv2WindowsAuthenticator(ControlPanel, Credential, Pacekeeper);
                case AuthenticationProvider.HpacAgentLess:
                case AuthenticationProvider.HpacDra:
                case AuthenticationProvider.HpacIrm:
                case AuthenticationProvider.SafeCom:
                case AuthenticationProvider.Equitrac:
                    return new SiriusUIv2OxpdAuthenticator(ControlPanel, Credential, Pacekeeper);
                default:
                    throw new DeviceInvalidOperationException($"{provider} Authentication is not supported in Sirius UIv2.");
            }
        }

        /// <summary>
        /// Gets the default authentication setting from the device.
        /// </summary>
        /// <returns><see cref="AuthenticationProvider" /></returns>
        protected override AuthenticationProvider GetDefaultProvider() => Provider;

    }
}
