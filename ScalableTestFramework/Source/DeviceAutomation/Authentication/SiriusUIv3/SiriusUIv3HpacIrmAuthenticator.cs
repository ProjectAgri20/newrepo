using System;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv3
{
    /// <summary>
    /// Sirius UIv3 HPAC IRM Authenticator class.  Provides authentication for HPAC IRM on Sirius UIv3 devices.
    /// </summary>
    internal class SiriusUIv3HpacIrmAuthenticator : SiriusUIv3PinAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3PinAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="SiriusUIv3ControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public SiriusUIv3HpacIrmAuthenticator(SiriusUIv3ControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters the PIN on the device control panel.
        /// Checks the provider connection before entering the PIN.
        /// </summary>
        public override void EnterCredentials()
        {
            CheckProviderConnection();
            EnterPin();
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            SubmitAuthentication("OK");
        }
    }
}
