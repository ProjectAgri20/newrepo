using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Authenticates on a JediOmni control panel using Windows credentials (Username, password).
    /// </summary>
    internal class JediOmniWindowsAuthenticator : JediOmniAppAuthenticatorBase
    {
        protected const string AuthDropdownId = "#hpid-dropdown-agent";
        protected readonly List<string> UserNameIds = new List<string> { ".hp-credential-control:first:text", ".hp-credential-control:text", ".hp-credential-control:password", ".hp - textbox:last" };

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniWindowsAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniWindowsAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering username and password.
        /// </summary>
        public override void EnterCredentials()
        {
            EnterUserNamePassword();
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {          
            if (ControlPanel.CheckState(KeyboardId, OmniElementState.Useable) && ControlPanel.CheckState("#hpid-keyboard-key-done", OmniElementState.Useable))
            {
                ControlPanel.Press("#hpid-keyboard-key-done");
                ControlPanel.WaitForState(KeyboardId, OmniElementState.Hidden);
            }

            if (ControlPanel.CheckState("#hpid-button-signin-ok", OmniElementState.Exists) && ControlPanel.CheckState("#hpid-button-signin-ok", OmniElementState.Useable))
            {
                ControlPanel.Press("#hpid-button-signin-ok");
            }
        }

        /// <summary>
        /// Enters the user name, password (and domain if applicable) on the device control panel.
        /// </summary>
        protected virtual void EnterUserNamePassword()
        {
            EnterDomain();
            EnterUserName();
            EnterPassword();
        }

        /// <summary>
        /// Enters the user name into the appropriate text box.
        /// </summary>
        protected virtual void EnterUserName()
        {
            if (!VerifyKeyboard(UserNameIds))
            {
                throw new DeviceWorkflowException("Unable to find 'User Name' text box.");
            }
            TypeOnVirtualKeyboard(Credential.UserName);
        }

        /// <summary>
        /// Enters the password into the appropriate text box.
        /// </summary>
        protected virtual void EnterPassword()
        {
            bool available = true;

            if (!ControlPanel.CheckState(".hp-credential-control:password", OmniElementState.Exists))
            {
                available = ControlPanel.WaitForAvailable(".hp-credential-control:password", TimeSpan.FromSeconds(3));
            }
            if (!available || !PressElementForKeyboard(".hp-credential-control:password"))
            {
                throw new DeviceWorkflowException("Unable to find 'Password' text box.");
            }
            TypeOnVirtualKeyboard(Credential.Password);
        }

        /// <summary>
        /// Checks domain field and enters the domain if needed.
        /// </summary>
        protected virtual void EnterDomain()
        {
            List<string> selectors = new List<string> { "#hpid-signin-body .hp-button-text", ".hp-credential-control:text:nth-of-type(2n)" };
            string domain = null;

            int i = 0;
            while (domain == null && i < selectors.Count)
            {
                try
                {
                    domain = ControlPanel.GetValue(selectors[i], "innerText", OmniPropertyType.Property).Trim();
                    if (string.IsNullOrEmpty(domain) && !string.IsNullOrEmpty(Credential.Domain))
                    {
                        if (!PressElementForKeyboard(selectors[i]))
                        {
                            throw new DeviceWorkflowException("Unable to find 'Domain' field.");
                        }
                        TypeOnVirtualKeyboard(Credential.Domain);
                        break;
                    }
                }
                catch (ElementNotFoundException) { }
                i++;
            }
        }

    }
}
