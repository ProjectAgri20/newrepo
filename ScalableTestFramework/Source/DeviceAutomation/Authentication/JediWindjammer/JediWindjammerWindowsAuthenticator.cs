using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    /// <summary>
    /// Jedi Windjammer Windows Authenticator class.  Provides authentication for Windows (Username, password) on Jedi Windjammer devices.
    /// </summary>
    internal class JediWindjammerWindowsAuthenticator : JediWindjammerAppAuthenticatorBase
    {
        protected const string _signInFormId = "SignInForm";

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerWindowsAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediWindjammerControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediWindjammerWindowsAuthenticator(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device control panel.
        /// </summary>
        public override void EnterCredentials()
        {
            // small screen SFP check
            var controls = ControlPanel.GetControls().ToList();
            if (controls.Contains("DomainDropDown_mInContentionButton") && controls.Contains("m_RadioButton"))
            {
                ControlPanel.PressWait("mOkButton", _signInFormId);
            }

            EnterUserNamePassword();
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            try
            {
                // The name of the OK button differs depending on the screen resolution
                string okButton = ControlPanel.GetControls().Contains("ok") ? "ok" : "mOkButton";

                // moved to press wait to handle the no click or no communications with server event within 10 seconds
                ControlPanel.PressWait(okButton, JediWindjammerLaunchHelper.HOMESCREEN_FORM, TimeSpan.FromSeconds(10));

                Pacekeeper.Pause();
            }
            catch (WindjammerInvalidOperationException ex)
            {
                switch (ControlPanel.CurrentForm())
                {
                    case "OneButtonMessageBox":
                        try
                        {
                            string message = ControlPanel.GetProperty("mMessageLabel", "Text");
                            if (message.StartsWith("Invalid"))
                            {
                                throw new DeviceWorkflowException($"Could not sign in: {message}", ex);
                            }
                            else
                            {
                                throw new DeviceInvalidOperationException($"Could not sign in: {message}", ex);
                            }
                        }
                        catch (ControlNotFoundException)
                        {
                            throw new DeviceInvalidOperationException($"Could not sign in: {ex.Message}", ex);
                        }

                    default:
                        throw new DeviceInvalidOperationException($"Could not sign in: {ex.Message}", ex);
                }
            }
        }

        /// <summary>
        /// Enters the user name, password (and domain if applicable) on the device control panel.
        /// </summary>
        protected virtual void EnterUserNamePassword(bool enterDomain = false)
        {
            //Enter user name
            ControlPanel.Type(Credential.UserName);
            Pacekeeper.Sync();

            // tab to Next control
            ControlPanel.Type(SpecialCharacter.Tab);
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
                ControlPanel.Type(SpecialCharacter.Tab);
                Pacekeeper.Sync();
            }

            //Enter password
            ControlPanel.Type(Credential.Password);
            Pacekeeper.Sync();
        }



    }
}
