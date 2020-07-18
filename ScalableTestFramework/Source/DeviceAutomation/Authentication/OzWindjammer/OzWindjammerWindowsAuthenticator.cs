using System;
using System.Collections.Generic;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Oz;

namespace HP.ScalableTest.DeviceAutomation.Authentication.OzWindjammer
{
    /// <summary>
    /// Oz Windjammer Windows Authenticator class.  Provides authentication for Windows (Username, password) on Oz Windjammer devices.
    /// </summary>
    public class OzWindjammerWindowsAuthenticator : IAppAuthenticator
    {
        private const int _signInScreen = 18;
        private const int _signInKeyboard = 32;

        /// <summary>
        /// Gets the <see cref="OzWindjammerControlPanel" /> for this solution authenticator.
        /// </summary>
        public OzWindjammerControlPanel ControlPanel { get; }

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
        /// Initializes a new instance of the <see cref="OzWindjammerWindowsAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="OzWindjammerControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public OzWindjammerWindowsAuthenticator(OzWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
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
            ControlPanel.Press("OK");
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
            bool atSignInScreen = ControlPanel.WaitForScreen(_signInScreen, TimeSpan.FromSeconds(10));
            if (!atSignInScreen)
            {
                throw new DeviceWorkflowException("Cannot enter credentials: Not at device sign in screen.");
            }

            EnterParameter("User Name", Credential.UserName);
            EnterParameter("Password", Credential.Password);

            // The domain may already be filled in.  Check before typing it again.
            if (!string.IsNullOrEmpty(Credential.Domain) && ControlPanel.GetWidgets().Find(Credential.Domain) == null)
            {
                EnterParameter("Domain", Credential.Domain);
            }
        }

        private void EnterParameter(string labelText, string value)
        {
            Widget label = ControlPanel.GetWidgets().Find(labelText, StringMatch.Contains);
            if (label != null)
            {
                Widget stringBox = ControlPanel.GetWidgets().FindByLabel(label, WidgetType.StringBox, WidgetType.EditBox);
                ControlPanel.Press(stringBox);
                ControlPanel.WaitForScreen(_signInKeyboard, TimeSpan.FromSeconds(5));
                Pacekeeper.Sync();

                ControlPanel.TypeOnVirtualKeyboard(value);
                Pacekeeper.Sync();
                ControlPanel.Press("OK");
                ControlPanel.WaitForScreen(_signInScreen, TimeSpan.FromSeconds(5));
                Pacekeeper.Sync();
            }
            else
            {
                throw new DeviceWorkflowException($"Could not sign in using Windows authentication: No label with text {labelText}.");
            }
        }
    }
}
