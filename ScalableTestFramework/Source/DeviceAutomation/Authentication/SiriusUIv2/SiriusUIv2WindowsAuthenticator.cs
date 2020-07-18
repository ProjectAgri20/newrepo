using System;
using System.Collections.Generic;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv2
{
    /// <summary>
    /// Sirius UIv2 Windows Authenticator class.  Provides authentication for Windows (Username, password) on Sirius UIv2 devices.
    /// </summary>
    public class SiriusUIv2WindowsAuthenticator : IAppAuthenticator
    {
        /// <summary>
        /// The error message
        /// </summary>
        protected string _errorMessage = string.Empty;

        /// <summary>
        /// Gets the <see cref="SiriusUIv2ControlPanel" /> for this solution authenticator.
        /// </summary>
        public SiriusUIv2ControlPanel ControlPanel { get; }

        /// <summary>
        /// Gets the <see cref="AuthenticationCredential" /> for this solution authenticator.
        /// </summary>
        public AuthenticationCredential Credential { get; }

        /// <summary>
        /// Gets the <see cref="Pacekeeper" /> for this solution authenticator.
        /// </summary>
        public Pacekeeper Pacekeeper { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        /// <value>
        /// The error message.
        /// </value>
        public string ErrorMessage
        {
            get
            {
                return _errorMessage;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv2WindowsAuthenticator"/> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="SiriusUIv2ControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public SiriusUIv2WindowsAuthenticator(SiriusUIv2ControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
        {
            ControlPanel = controlPanel;
            Credential = credential;
            Pacekeeper = pacekeeper;
        }

        /// <summary>
        /// Enters credentials on the device by entering Username and Password.
        /// </summary>
        public virtual void EnterCredentials()
        {
            EnterUserNamePassword();
        }

        /// <summary>
        /// Applies additional authentication parameters before submission of authentication request.
        /// </summary>
        /// <param name="parameters"></param>
        public void ApplyParameters(Dictionary<string, object> parameters)
        {
        }

        /// <summary>
        /// Enters the user name and password on the device control panel.
        /// </summary>
        private void EnterUserNamePassword()
        {
            string label;

            try
            {
                label = ControlPanel.ActiveScreenLabel();

                switch (label)
                {
                    case "view_authentication_ldap_login":
                        //Enter Username
                        EnterText("ldap_login_username_edit", "view_ldap_login_username_touch", "ldap_ldap_login_username_keyboard.4", Credential.UserName);
                        Pacekeeper.Sync();
                        //Enter Password
                        EnterText("ldap_password_entry_password", "view_user_password_entry_password", "user_password_entry_password_keyboard.4", Credential.Password);
                        break;
                    case "view_authentication_window_login":
                        //Enter Username
                        EnterText("windows_login_username_entry", "view_windows_login_username_entry_username", "ldap_windows_login_username_keyboard.4", Credential.UserName);
                        Pacekeeper.Sync();
                        //Enter Password
                        EnterText("windows_login_password_entry_password", "view_windows_password_entry_password", "windows_password_entry_password_keyboard.4", Credential.Password);
                        break;
                }
                Pacekeeper.Sync();

            }
            catch (ElementNotFoundException ex)
            {
                label = ControlPanel.ActiveScreenLabel();
                switch (label)
                {
                    case "view_authentication_window_login":
                        throw new DeviceWorkflowException("Could not sign in using Windows authentication.", ex);

                    case "view_oxpd_auth_result":
                        ControlPanel.Press("oxpd_auth_result_msg_footer.0");
                        throw new DeviceWorkflowException("Could not sign in using LDAP authentication.", ex);
                }
            }
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public virtual void SubmitAuthentication()
        {
            // Press OK and wait for authentication to complete
            try
            {
                string label = ControlPanel.ActiveScreenLabel();
                if (label.Equals("view_authentication_ldap_login") || label.Equals("view_authentication_window_login"))
                {
                    ControlPanel.Press("native_login_footer.0");
                }
            }
            catch (SiriusInvalidOperationException ex)
            {
                throw new DeviceWorkflowException($"Could not sign in: {ex.Message}", ex);
            }

        }

        /// <summary>
        /// Checks the device control panel for successful authentication.
        /// </summary>
        /// <returns></returns>
        public bool ValidateAuthentication()
        {
            return true;
        }

        private void EnterText(string button, string screen, string control, string controlValue)
        {
            ControlPanel.Press(button);
            Pacekeeper.Sync();
            ControlPanel.WaitForActiveScreenLabel(screen, TimeSpan.FromSeconds(3));
            ControlPanel.SetValue(control, controlValue);
            Pacekeeper.Sync();
        }
    }
}
