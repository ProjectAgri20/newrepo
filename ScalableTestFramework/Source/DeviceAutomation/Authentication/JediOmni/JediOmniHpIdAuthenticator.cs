using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Authenticates on a JediOmni control panel using HPID credentials (Username, password).
    /// </summary>
    internal class JediOmniHpIdAuthenticator : JediOmniAppAuthenticatorBase
    {
        protected readonly List<string> UserNameTextboxId = new List<string> { "username" };
        protected readonly List<string> PasswordTextboxId = new List<string> { "password" };
        protected readonly TimeSpan ElementAvailableWait = TimeSpan.FromSeconds(5);

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniHpIdAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel"></param>
        /// <param name="credential"></param>
        /// <param name="pacekeeper"></param>
        public JediOmniHpIdAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters the credential information.
        /// </summary>
        public override void EnterCredentials()
        {
            EnterUserName();
            PressSubmitButton();
            EnterPassword();
            //Don't Press the "Sign In" button here.  SubmitAuthentication will do that.
        }

        /// <summary>
        /// Submits the credentials for authentication.
        /// </summary>
        public override void SubmitAuthentication()
        {
            PressSubmitButton();            
        }

        /// <summary>
        /// Enters the user name into the appropriate text box.
        /// </summary>
        private void EnterUserName()
        {
            WaitForForm("loginUserName", "HpId Username");
            PressElementOxpd(UserNameTextboxId);
            TypeOnVirtualKeyboard(Credential.UserName, true);
        }

        /// <summary>
        /// Enters the password into the appropriate text box.
        /// </summary>
        private void EnterPassword()
        {
            WaitForForm("loginPassword", "HpId Password");
            PressElementOxpd(PasswordTextboxId);
            TypeOnVirtualKeyboard(Credential.Password, true);
        }

        /// <summary>
        /// vn-button is used for both Next and Submit operations.
        /// </summary>
        private void PressSubmitButton()
        {
            try
            {
                PressElementOxpdByClassIndex("vn-button", 0);
            }
            catch (ArgumentException)
            {
                //Could get an error here if the button is below the screen boundary (4.3" screen for example).
                ScrollUp(10);
                PressElementOxpdByClassIndex("vn-button", 0);
            }
        }

        private void WaitForForm(string formId, string formDescription)
        {
            if (! WaitForElementIdOxpd(formId, ElementAvailableWait))
            {
                throw new DeviceWorkflowException($"{formDescription} form did not display within {ElementAvailableWait.TotalSeconds} seconds.");
            }
        }
    }
}
