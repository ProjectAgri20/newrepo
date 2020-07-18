using System;
using System.Collections.Generic;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni HPAC Agentless Authenticator class.  Provides authentication for agentless HPAC on Jedi Omni devices.
    /// </summary>
    internal class JediOmniHpacAgentlessAuthenticator : JediOmniWindowsAuthenticator
    {
        private const string NetworkButtonId = "authentication_screen-request-card_action";
        
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniHpacAgentlessAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniHpacAgentlessAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering the PIN.
        /// </summary>
        public override void EnterCredentials()
        {
            if (ExistElementIdOxpd(NetworkButtonId))
            {
                WindowsAuthentication();
            }
            else
            {
                EnterPin();
            }
        }

        private void WindowsAuthentication()
        {
            const string userNameBoxId = "authentication_screen-require-user_userUsername";
            const string passwordBoxId = "authentication_screen-require-user_userPassword";

            System.Diagnostics.Debug.WriteLine($"Pressing {NetworkButtonId}");
            PressElementOxpd(NetworkButtonId);

            TimeSpan timeout = TimeSpan.FromSeconds(15);
            if (WaitForElementIdOxpd(userNameBoxId, timeout))
            {
                PressElementOxpd(userNameBoxId);
                TypeOnVirtualKeyboard(Credential.UserName);

                PressElementOxpd(passwordBoxId);
                TypeOnVirtualKeyboard(Credential.Password);
            }
            else
            {
                throw new DeviceWorkflowException($"The username and password authentication screen did not show within {timeout.TotalSeconds} seconds.");
            }

        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            //List of possible Ids in descending order of HPAC revision (newest first).
            List<string> elementIds = new List<string>()
            {
                 "authentication_screen-require-code_accept"
                ,"authentication_screen-request-code_accept"
                ,"authentication_screen-require-user_accept"
            };

            base.SubmitAuthentication();
            PressElementOxpd(elementIds);
        }

        private void EnterPin()
        {
            //List of possible Ids in descending order of HPAC revision (newest first).
            List<string> elementIds = new List<string>()
            {
                 "authentication_screen-require-code_userCode"
                ,"authentication_screen-request-code_userCode"
            };

            PressElementOxpd(elementIds);
            ControlPanel.WaitForAvailable(KeyboardId, TimeSpan.FromSeconds(3));
            TypeOnVirtualKeyboard(Credential.Pin);
        }
    }
}
