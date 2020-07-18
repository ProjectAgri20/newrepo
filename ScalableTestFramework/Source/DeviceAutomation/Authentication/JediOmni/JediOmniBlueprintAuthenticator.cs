using System;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni Blueprint Authenticator class.  Provides authentication for Blueprint on Jedi Omni devices.
    /// </summary>
    internal class JediOmniBlueprintAuthenticator : JediOmniWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniBlueprintAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniBlueprintAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper) 
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering Username and Password.
        /// </summary>
        public override void EnterCredentials()
        {
            // Blueprint 6.5
            if (ControlPanel.CheckState(".hp-credential-control:password", OmniElementState.VisibleCompletely))
            {
                EnterUserNamePassword();
            }
            else // 5.5
            {
                EnterUserNamePasswordSplitPage();
            }
        }

        /// <summary>
        /// Enters the username on one screen and the password on the next
        /// </summary>
        private void EnterUserNamePasswordSplitPage()
        {
            EnterUserName();

            SubmitAuthentication();
            Pacekeeper.Sync();
            bool result = ControlPanel.WaitForAvailable(".hp-credential-control:password", TimeSpan.FromSeconds(5));
            if (!result)
            {
                throw new DeviceWorkflowException("Unable to find the 'Password' text box");
            }
            EnterPassword();
        }
    }
}
