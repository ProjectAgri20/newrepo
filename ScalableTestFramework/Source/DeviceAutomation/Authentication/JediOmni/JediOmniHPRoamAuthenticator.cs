using System;
using System.Collections.Generic;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    internal class JediOmniHPRoamAuthenticator : JediOmniWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of <see cref="JediOmniHPRoamAuthenticator"/> class.
        /// </summary>
        /// <param name="controlPanel"></param>
        /// <param name="credential"></param>
        /// <param name="pacekeeper"></param>
        public JediOmniHPRoamAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters Credentials on the device by entering the PIN
        /// </summary>
        public override void EnterCredentials()
        {
            EnterPin();
        }

        /// <summary>
        /// Enters Pin for HP Roam
        /// </summary>
        private void EnterPin()
        {
            if (!PressElementForKeyboard("#hpid-signin-textbox-UserNameTextBox"))
            {
                throw new DeviceWorkflowException("Keyboard is not displayed.");
            }
            ControlPanel.TypeOnVirtualKeyboard(Credential.UserName.Substring(2));
            Pacekeeper.Sync();
        }


    }
}
