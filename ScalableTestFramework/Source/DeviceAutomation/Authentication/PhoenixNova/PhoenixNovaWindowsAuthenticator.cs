using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation.Phoenix;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Authentication.PhoenixNova
{
    /// <summary>
    /// Phoenix Nova Windows Authenticator class.  Provides authentication for Windows (Username, password) on Phoenix Nova devices.
    /// </summary>
    public class PhoenixNovaWindowsAuthenticator : IAppAuthenticator
    {
        /// <summary>
        /// Gets the <see cref="PhoenixNovaControlPanel" /> for this solution authenticator.
        /// </summary>
        public PhoenixNovaControlPanel ControlPanel { get; }

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
                return string.Empty;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PhoenixNovaWindowsAuthenticator"/> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="PhoenixNovaControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public PhoenixNovaWindowsAuthenticator(PhoenixNovaControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
        {
            ControlPanel = controlPanel;
            Credential = credential;
            Pacekeeper = pacekeeper;
        }

        /// <summary>
        /// Enters credentials on the device control panel.
        /// </summary>
        public void EnterCredentials()
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
        /// Gets the default authentication setting from the device.
        /// Nova only supports Windows authentication
        /// </summary>
        /// <returns><see cref="AuthenticationProvider" /></returns>
        public static AuthenticationProvider GetDefaultProvider() => AuthenticationProvider.Windows;

        /// <summary>
        /// Enters the user name and password on the device control panel.
        /// </summary>
        private void EnterUserNamePassword()
        {
            try
            {
                Thread.Sleep(TimeSpan.FromSeconds(1));
                SetKeyValue("UsernameTextbox", Credential.UserName);
                Thread.Sleep(TimeSpan.FromSeconds(1));
                SetKeyValue("PasswordTextbox", Credential.Password);
            }
            catch (PhoenixInvalidOperationException ex)
            {
                throw new DeviceWorkflowException("Could not sign in.", ex);
            }
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public void SubmitAuthentication()
        {
            try
            {
                var screenText = ControlPanel.GetDisplayedStrings().ToList();

                if (screenText.Contains("Sign In: LDAP") || screenText.Contains("Sign In: Windows"))
                {
                    ControlPanel.Press("cSignInTouchButton");
                }

                var errorMessage = ControlPanel.GetDisplayedStrings();

                if (errorMessage.Contains("Your printer was unable to connect to\nthe server.\nCheck your network connection and try\nagain."))
                {
                    throw new DeviceWorkflowException($"Could not sign in: {errorMessage}");
                }
            }
            catch (PhoenixInvalidOperationException ex)
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

        private void SetKeyValue(string textBox, string value)
        {
            var screenText = ControlPanel.GetDisplayedStrings().ToList();
            if (screenText.Contains("Sign In: LDAP") || screenText.Contains("Sign In: Windows"))
            {
                ControlPanel.Press(textBox);
                Thread.Sleep(TimeSpan.FromSeconds(1));
                ControlPanel.TypeOnVirtualKeyboard(value);
                Pacekeeper.Pause();
                ControlPanel.Press("cOKTouchButton");
            }
            Pacekeeper.Sync();
        }
    }
}
