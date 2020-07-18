using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv2
{
    /// <summary>
    /// Sirius UIv2 OXPd Authenticator class.  Provides authentication for OXPd (Username, password) on Sirius UIv2 devices.
    /// </summary>
    internal class SiriusUIv2OxpdAuthenticator : SiriusUIv2WindowsAuthenticator
    {
        private const int _defaultPause = 2000;
        private readonly TimeSpan _defaultWait = TimeSpan.FromMilliseconds(_defaultPause);

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv2OxpdAuthenticator"/> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="SiriusUIv2ControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public SiriusUIv2OxpdAuthenticator(SiriusUIv2ControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper) 
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering Username and Password.
        /// </summary>
        public override void EnterCredentials()
        {
            EnterUserNamePassword();
        }

        /// <summary>
        /// Enters UserName and Password for Oxpd Login.
        /// </summary>
        private void EnterUserNamePassword()
        {
            if (ControlPanel.WaitForActiveScreenLabel("view_sips_form", _defaultWait))
            {
                if (!string.IsNullOrEmpty(Credential.Domain))
                {
                    //press the button beside domain
                    ControlPanel.Press("sips_form_region1_value");
                    ControlPanel.WaitForActiveScreenLabel("view_sips_form_entry_region2", _defaultWait);
                    Pacekeeper.Pause();
                    ControlPanel.SetValue("sips_form_region1_kbd.4", Credential.Domain);
                    ControlPanel.WaitForActiveScreenLabel("view_sips_form", _defaultWait);
                    Pacekeeper.Pause();
                }

                // press the button beside User Login button
                ControlPanel.Press("sips_form_region2_value");
                ControlPanel.WaitForActiveScreenLabel("view_sips_form_entry_region2", _defaultWait);
                Pacekeeper.Pause();
                ControlPanel.SetValue("sips_form_region2_kbd.4", Credential.UserName);
                ControlPanel.WaitForActiveScreenLabel("view_sips_form", _defaultWait);
                Pacekeeper.Pause();

                // press the button beside the Password button
                ControlPanel.Press("sips_form_region3_value");
                ControlPanel.WaitForActiveScreenLabel("view_sips_form_region3_kbd", _defaultWait);
                Pacekeeper.Pause();
                ControlPanel.SetValue("sips_form_region3_kbd.4", Credential.Password);
                ControlPanel.WaitForActiveScreenLabel("view_sips_form", _defaultWait);
                Pacekeeper.Pause();
            }
            else
            {
                throw new DeviceInvalidOperationException("Unable to enter credentials. Not at device sign in screen.");
            }
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            try
            {
                ControlPanel.WaitForActiveScreenLabel("view_sips_form");
                ControlPanel.PressByValue("Login");

                Pacekeeper.Sync();
            }
            catch (SiriusInvalidOperationException ex)
            {
                throw new DeviceWorkflowException($"Could not sign in: {ex.Message}", ex);
            }
        }

    }
}
