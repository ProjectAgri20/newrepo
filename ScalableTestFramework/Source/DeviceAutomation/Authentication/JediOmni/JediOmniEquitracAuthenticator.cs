using System;
using HP.ScalableTest.Utility;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni Equitrac Authenticator class.  Provides authentication for Equitrac on Jedi Omni devices.
    /// </summary>
    internal class JediOmniEquitracAuthenticator : JediOmniPinAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniEquitracAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniEquitracAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device.
        /// If the device prompts for username and password, enters them.
        /// If the device prompts for PIN, enters PIN.
        /// </summary>
        public override void EnterCredentials()
        {
             EnterPin();
        }

        /// <summary>
        /// Enters Pin on the device.
        /// </summary>
        protected override void EnterPin()
        {
            ControlPanel.TypeOnVirtualKeyboard(Credential.Pin);
            Pacekeeper.Sync();
        }

        /// <summary>
        /// Detects if the Authentication screen is configured for PIN authentication.
        /// </summary>
        /// <returns>true if configured for PIN auth, false otherwise.</returns>
        protected override bool ConfiguredForPin()
        {
            TimeSpan waitTime = TimeSpan.FromSeconds(30);

            if (!ControlPanel.WaitForAvailable(AuthDropdownId, waitTime))
            {
                throw new DeviceWorkflowException($"Authentication screen did not display within {waitTime.TotalSeconds} seconds.");
            }

            string value = GetSignInText();
            //Pin Authentication also uses 'user id' so we are switching to use the password string
            //STF supports 'Equitrac Pins' and 'External user ID and password' server authentication options. 'Equitrac PIN with external password not supported
            return (!value.Contains("Password", StringComparison.OrdinalIgnoreCase));
        }
    }
}
