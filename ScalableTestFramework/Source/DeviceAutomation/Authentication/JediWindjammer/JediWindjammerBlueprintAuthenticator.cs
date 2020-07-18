using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation;
using System;
using System.Linq;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    /// <summary>
    /// Jedi Windjammer Blueprint Authenticator class.  Provides authentication for Blueprint on Jedi Windjammer devices.
    /// </summary>
    internal class JediWindjammerBlueprintAuthenticator : JediWindjammerWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerBlueprintAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediWindjammerControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediWindjammerBlueprintAuthenticator(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }
        /// <summary>
        /// Enters the user name, password (and domain if applicable) on the device control panel.
        /// </summary>
        protected override void EnterUserNamePassword(bool enterDomain = false)
        {
            //Enter user name
            ControlPanel.Type(Credential.UserName);
            Pacekeeper.Sync();

            string okCtlrs = "ok|mOkButton.*";

            if (ControlPanel.WaitForControl(okCtlrs, StringMatch.Regex, TimeSpan.FromSeconds(5)))
            {
                string okButton = ControlPanel.GetControls().Contains("ok") ? "ok" : "mOkButton";

                ControlPanel.Press(okButton);
                Pacekeeper.Sync();

                // Check if domain field is expected
                if (enterDomain)
                {
                    // Enter domain if populated
                    if (!string.IsNullOrEmpty(Credential.Domain))
                    {
                        ControlPanel.Type(Credential.Domain);
                        Pacekeeper.Sync();
                    }

                    //Tab to next control
                    if (ControlPanel.WaitForControl(okButton, TimeSpan.FromSeconds(5)))
                    {
                        ControlPanel.Press(okButton);
                        Pacekeeper.Sync();
                    }
                    else
                    {
                        throw new DeviceWorkflowException($"Unable to press '{okButton}'");
                    }
                }

                //Enter password
                ControlPanel.Type(Credential.Password);
                Pacekeeper.Sync();
            }
            else
            {
                throw new DeviceWorkflowException("Unable to press the Blueprint 'OK' button");
            }
        }
    }
}
