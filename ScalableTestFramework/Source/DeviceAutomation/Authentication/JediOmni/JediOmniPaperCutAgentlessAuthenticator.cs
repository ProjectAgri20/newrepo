using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Authenticates on a JediOmni control panel using Windows credentials (Username, password).
    /// </summary>
    internal class JediOmniPaperCutAgentlessAuthenticator : JediOmniWindowsAuthenticator
    {        
        protected readonly List<string> UserNameTextboxId = new List<string> { "username-input" };
        protected readonly List<string> PasswordTextboxId = new List<string> { "password-input" };

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniPaperCutAgentlessAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniPaperCutAgentlessAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering username and password.
        /// </summary>
        public override void EnterCredentials()
        {
            EnterPapercutAgentlessUserName();
            EnterPapercutAgentlessPassword();
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            PressElementOxpd("login-submit");         
        }
        
        /// <summary>
        /// Enters the user name into the appropriate text box.
        /// </summary>
        protected void EnterPapercutAgentlessUserName()
        {
            PressElementOxpd(UserNameTextboxId);
            TypeOnVirtualKeyboard(Credential.UserName, true);
        }

        /// <summary>
        /// Enters the password into the appropriate text box.
        /// </summary>
        protected void EnterPapercutAgentlessPassword()
        {
            PressElementOxpd(PasswordTextboxId);
            TypeOnVirtualKeyboard(Credential.Password, true);
        }
    }
}
