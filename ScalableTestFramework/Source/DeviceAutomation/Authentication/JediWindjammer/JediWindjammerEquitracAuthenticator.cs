
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    /// <summary>
    /// Jedi Windjammer Equitrac Authenticator class.  Provides authentication for Equitrac on Jedi Windjammer devices.
    /// </summary>
    internal class JediWindjammerEquitracAuthenticator : JediWindjammerPinAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerEquitracAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediWindjammerControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediWindjammerEquitracAuthenticator(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
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
            if (ConfiguredForPin())
            {
                EnterPin();
            }
            else
            {
                EnterUserNamePassword();
            }
        }
    }
}
