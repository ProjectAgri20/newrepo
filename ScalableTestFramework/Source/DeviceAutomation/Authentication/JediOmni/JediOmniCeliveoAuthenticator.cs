using System;
using System.Collections.Generic;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni Celiveo Authenticator class.  Provides authentication for Celiveo on Jedi Omni devices.
    /// </summary>
    internal class JediOmniCeliveoAuthenticator : JediOmniWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniCeliveoAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniCeliveoAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering the PIN
        /// </summary>
        public override void EnterCredentials()
        {
            if (ExistElementIdOxpd("buttonBUTTONAUTHHOMELP"))
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
            List<string> windowElements = new List<string>
            {
                "buttonBUTTONAUTHHOMELP"
                ,"textboxLOGINPASSINPUT1"
                ,"textboxLOGINPASSINPUT2"
                ,"buttonBUTTONOK"
            };

            PressElementOxpd(windowElements[0]);

            if (ExistElementIdOxpd(windowElements[1]))
            {
                PressElementOxpd(windowElements[1]);
                ControlPanel.WaitForAvailable(KeyboardId, TimeSpan.FromSeconds(3));
                TypeOnVirtualKeyboard(Credential.UserName);

                PressElementOxpd(windowElements[2]);
                TypeOnVirtualKeyboard(Credential.Password);

                if (ControlPanel.GetScreenSize().Width > 480)
                {
                    base.SubmitAuthentication();
                }     
            }
        }

        /// <summary>
        /// Submits the authentication request
        /// </summary>
        public override void SubmitAuthentication()
        {
            base.SubmitAuthentication();
            PressElementOxpd("buttonBUTTONOK");
        }

        /// <summary>
        /// Enters the PIN for Celiveo
        /// </summary>
        private void EnterPin()
        {
            PressElementOxpd("textboxMANUALIDINPUT1");

            if (!ControlPanel.WaitForAvailable(KeyboardId, TimeSpan.FromSeconds(3)))
            {
                throw new DeviceWorkflowException("Keyboard is not displayed");
            }
            ControlPanel.TypeOnVirtualKeyboard(Credential.Pin);

            Pacekeeper.Sync();
        }
    }
}
