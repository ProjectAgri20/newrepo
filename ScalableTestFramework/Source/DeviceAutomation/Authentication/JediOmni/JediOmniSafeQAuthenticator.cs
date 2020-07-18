using System;
using System.Collections.Generic;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni YSoft SafeQ Authenticator class.  Provides authentication for YSoft SafeQ on Jedi Omni devices.
    /// </summary>
    internal class JediOmniSafeQAuthenticator : JediOmniWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniSafeQAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniSafeQAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Applies additional authentication parameters before submission of authentication request.
        /// </summary>
        /// <param name="parameters"></param>
        public override void ApplyParameters(Dictionary<string, object> parameters)
        {
            if ((bool)parameters["PrintAll"])
            {
                PressElementOxpd("printAllButton");
            }
        }

        /// <summary>
        /// Enters credentials on the device.
        /// If the device prompts for username and password, enters them.
        /// If the device prompts for PIN, enters PIN.
        /// </summary>
        public override void EnterCredentials()
        {
            if (UseSafeQPin())
            {
                EnterPin();
            }
            else
            {
                EnterUserNameForSafeQ();
                EnterPasswordForSafeQ();
            }
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            base.SubmitAuthentication();

            if (!ControlPanel.CheckState("#hp-button-signin-or-signout", OmniElementState.Useable) &&  ExistElementIdOxpd("js-login-button"))
            {
                PressElementOxpd("js-login-button");
            }
        }

        /// <summary>
        /// Enters the user name into the appropriate text box.
        /// </summary>
        private void EnterUserNameForSafeQ()
        {
            PressElementOxpd("js-credentials-username");

            if (!ControlPanel.WaitForAvailable(KeyboardId, TimeSpan.FromSeconds(3)))
            {
                throw new DeviceWorkflowException("Keyboard is not displayed");
            }
            TypeOnVirtualKeyboard(Credential.UserName);

            Pacekeeper.Sync();
        }

        /// <summary>
        /// Enters the password into the appropriate text box.
        /// </summary>
        private void EnterPasswordForSafeQ()
        {
            PressElementOxpd("js-credentials-password");

            if (!ControlPanel.WaitForAvailable(KeyboardId, TimeSpan.FromSeconds(3)))
            {
                throw new DeviceWorkflowException("Keyboard is not displayed");
            }
            TypeOnVirtualKeyboard(Credential.Password);

            Pacekeeper.Sync();
        }

        private void EnterPin()
        {
            PressElementOxpd("js-credentials-pin");

            if (!ControlPanel.WaitForAvailable(KeyboardId, TimeSpan.FromSeconds(3)))
            {
                throw new DeviceWorkflowException("Keyboard is not displayed");
            }
            ControlPanel.TypeOnVirtualKeyboard(Credential.Pin);

            Pacekeeper.Sync();
        }

        private bool UseSafeQPin()
        {
            OxpdBrowserEngine engine = new OxpdBrowserEngine(ControlPanel);
            return engine.HtmlContains("js-credentials-pin");
        }
    }
}
