using System;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni ISecStar Authenticator class.  Provides authentication for ISecStar on Jedi Omni devices.
    /// </summary>
    internal class JediOmniISecStarAuthenticator : JediOmniWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniISecStarAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniISecStarAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
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
        /// Enters the user name, password.
        /// </summary>
        protected override void EnterUserNamePassword()
        {
            OxpdBrowserEngine engine = new OxpdBrowserEngine(ControlPanel);
            // The following code is not needed here.  SetProvider will have already been called from the base class.  See AuthenticatorBase lines 62-66
            // In the ISecStar plugin, to force the code to use ISecStar every time, remove the "Auto-Detect" options from the plugin dropdown.
            //if (!(ControlPanel.CheckState($"{AuthDropdownId} .hp-listitem-text:contains({ProviderMap[provider].SetText})", OmniElementState.Exists)))
            //{
            //    SetProvider(AuthenticationProvider.ISecStar);
            //}
            engine.PressElementById("username");
            ControlPanel.WaitForAvailable("#hpid-keyboard", TimeSpan.FromSeconds(4));
            ControlPanel.TypeOnVirtualKeyboard(Credential.UserName);

            if (ControlPanel.WaitForAvailable(("#hpid-keyboard-key-done")))
            {
                ControlPanel.Press("#hpid-keyboard-key-done");
            }

            engine.PressElementById("password");
            ControlPanel.WaitForAvailable("#hpid-keyboard", TimeSpan.FromSeconds(4));
            ControlPanel.TypeOnVirtualKeyboard(Credential.Password);

            if (ControlPanel.WaitForAvailable(("#hpid-keyboard-key-done")))
            {
                ControlPanel.Press("#hpid-keyboard-key-done");
            }

            engine.PressElementById("okayButton");
        }
    }
}
