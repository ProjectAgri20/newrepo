using System;
using System.Linq;
using HP.ScalableTest.Utility;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    /// <summary>
    /// Jedi Windjammer PIN Authenticator class.  Provides authentication for user PIN on Jedi Windjammer devices.
    /// </summary>
    internal class JediWindjammerPinAuthenticator : JediWindjammerWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerPinAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediWindjammerControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediWindjammerPinAuthenticator(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering the PIN.
        /// </summary>
        public override void EnterCredentials()
        {
            EnterPin();
        }

        /// <summary>
        /// Enters the PIN on the device control panel.
        /// </summary>
        protected void EnterPin()
        {
            ControlPanel.Type(Credential.Pin);
            Pacekeeper.Sync();
        }

        /// <summary>
        /// Detects if the Authentication screen is configured for PIN authentication.
        /// </summary>
        /// <returns>true if configured for PIN auth, false otherwise.</returns>
        protected virtual bool ConfiguredForPin()
        {
            string value = GetDeviceTitle();
            return (value.Contains("code", StringComparison.OrdinalIgnoreCase));
        }
    }
}
