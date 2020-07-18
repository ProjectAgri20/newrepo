using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation.Phoenix;

namespace HP.ScalableTest.DeviceAutomation.Authentication.PhoenixMagicFrame
{
    /// <summary>
    /// Phoenix MagicFrame Windows Authenticator class.  Provides authentication for Windows (Username, password) on Phoenix MagicFrame devices.
    /// </summary>
    public class PhoenixMagicFrameWindowsAuthenticator : IAppAuthenticator
    {
        /// <summary>
        /// Gets the <see cref="PhoenixMagicFrameControlPanel" /> for this solution authenticator.
        /// </summary>
        public PhoenixMagicFrameControlPanel ControlPanel { get; }

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
        /// Initializes a new instance of the <see cref="PhoenixMagicFrameWindowsAuthenticator"/> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="PhoenixMagicFrameControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public PhoenixMagicFrameWindowsAuthenticator(PhoenixMagicFrameControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
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
        /// Submits the authentication request.
        /// </summary>
        public void SubmitAuthentication()
        {
            try
            {
                List<string> screenText = ControlPanel.GetDisplayedStrings().ToList();

                if (screenText.Contains("Sign In: LDAP") || screenText.Contains("Sign In: Windows"))
                {
                    ControlPanel.Press("cSignInTouchButton");
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
