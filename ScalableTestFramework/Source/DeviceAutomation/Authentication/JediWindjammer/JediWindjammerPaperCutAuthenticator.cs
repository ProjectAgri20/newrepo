using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    internal class JediWindjammerPaperCutAuthenticator : JediWindjammerWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerPaperCutAuthenticator"/> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediWindjammerControlPanel" /> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential" /> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper" /> object.</param>
        public JediWindjammerPaperCutAuthenticator(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {

        }

        /// <summary>
        /// Enters credentials on the device control panel.
        /// </summary>
        public override void EnterCredentials()
        {
            // Enter domain\user name
            ControlPanel.Type(Credential.Domain + "\\" + Credential.UserName);
            Pacekeeper.Sync();

            // Tab to Next control
            ControlPanel.Type(SpecialCharacter.Tab);
            Pacekeeper.Sync();

            // Enter password
            ControlPanel.Type(Credential.Password);
            Pacekeeper.Sync();
        }
    }
}
