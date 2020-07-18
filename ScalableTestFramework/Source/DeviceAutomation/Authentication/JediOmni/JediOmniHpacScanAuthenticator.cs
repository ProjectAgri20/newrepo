using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    internal class JediOmniHpacScanAuthenticator : JediOmniWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniWindowsAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniHpacScanAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering username and password.
        /// </summary>
        public override void EnterCredentials()
        {
            EnterHpacScanUserName();
            EnterHpacScanPassword();
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            WaitForElementIdOxpd("text_field_action_ok", TimeSpan.FromSeconds(10));
            PressElementOxpd("text_field_action_ok");
        }

        private void EnterHpacScanUserName()
        {
            PressElementOxpd("LOGIN_USER");
            TypeOnVirtualKeyboard(Credential.UserName, true);
            PressElementOxpd("text_field_action_ok");

            WaitForElementIdOxpd("modal_button_2_title", TimeSpan.FromSeconds(10));
            PressElementOxpd("modal_button_2_title");
        }

        private void EnterHpacScanPassword()
        {
            WaitForElementIdOxpd("LOGIN_PASSWORD", TimeSpan.FromSeconds(10));
            PressElementOxpd("LOGIN_PASSWORD");

            TypeOnVirtualKeyboard(Credential.Password, true);
        }
    }
}
