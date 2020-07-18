using System.Collections.Generic;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    internal class JediOmniMyQAuthenticator : JediOmniWindowsAuthenticator
    {
        protected readonly List<string> UserNameTextboxId = new List<string> { "username" };
        protected readonly List<string> PasswordTextboxId = new List<string> { "password" };

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniWindowsAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniMyQAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering username and password.
        /// </summary>
        public override void EnterCredentials()
        {
            EnterMyQUserName();
            EnterMyQPassword();
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            PressElementOxpd("mqC8");
        }

        private void EnterMyQUserName()
        {
            PressElementOxpd(UserNameTextboxId);
            TypeOnVirtualKeyboard(Credential.UserName, true);
        }

        private void EnterMyQPassword()
        {
            PressElementOxpd(PasswordTextboxId);
            TypeOnVirtualKeyboard(Credential.Password, true);
        }
    }
}
