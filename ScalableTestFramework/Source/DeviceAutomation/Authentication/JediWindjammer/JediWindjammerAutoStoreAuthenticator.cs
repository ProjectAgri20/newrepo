using System;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    internal class JediWindjammerAutoStoreAuthenticator : JediWindjammerWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerAutoStoreAuthenticator"/> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediWindjammerControlPanel" /> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential" /> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper" /> object.</param>
        public JediWindjammerAutoStoreAuthenticator(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {

        }

        /// <summary>
        /// Enters credentials on the device control panel.
        /// </summary>
        public override void EnterCredentials()
        {
            PressElementOxpd("username");

            ControlPanel.TypeOnVirtualKeyboard("mKeyboard", Credential.UserName);
            ControlPanel.Press("ok");

            PressElementOxpd("password");
            ControlPanel.TypeOnVirtualKeyboard("mKeyboard", Credential.Password);
            ControlPanel.Press("ok");
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            PressElementOxpd("submitButton");
        }

    }
}
