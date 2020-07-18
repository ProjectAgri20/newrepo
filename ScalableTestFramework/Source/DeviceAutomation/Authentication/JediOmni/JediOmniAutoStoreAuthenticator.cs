using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni AutoStore Authentication
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.Authentication.JediOmni.JediOmniWindowsAuthenticator" />
    internal class JediOmniAutoStoreAuthenticator : JediOmniWindowsAuthenticator
    {
        private TimeSpan _idleTimeoutOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniAutoStoreAuthenticator"/> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel" /> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential" /> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper" /> object.</param>
        public JediOmniAutoStoreAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
            _idleTimeoutOffset = TimeSpan.FromSeconds(50);
        }

        /// <summary>
        /// Enters credentials on the device by entering username and password.
        /// </summary>
        /// <exception cref="DeviceWorkflowException">Keyboard did not show within 15 seconds.</exception>
        public override void EnterCredentials()
        {
            PressElementOxpd("username");

            TypeOnVirtualKeyboard(Credential.UserName);            

            PressElementOxpd("password");
            TypeOnVirtualKeyboard(Credential.Password);
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
            if (ControlPanel.GetScreenSize().Width.Equals(480))
            {
                Coordinate swipeStart = new Coordinate(240, 218);
                Coordinate swipeEnd = new Coordinate(240, 75);

                ControlPanel.SwipeScreen(swipeStart, swipeEnd, TimeSpan.FromMilliseconds(250));
            }

            PressElementOxpd("submitButton");
        }

        /// <summary>
        /// Checks the device control panel for successful authentication.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the authentication operation is valid, <c>false</c> otherwise.
        /// </returns>
        public override bool ValidateAuthentication()
        {
            bool validate = true;
            string emailAddress = (Credential.UserName + "@" + Credential.Domain).ToLower();
            // the email credentials are shown when authentication has been verified

            
            if (!WaitForHtmlContains(emailAddress, _idleTimeoutOffset))
            {
                _errorMessage = string.Format(" AutoStore did not authenticate for user {0} within {1} seconds.", emailAddress, _idleTimeoutOffset);
                validate = false;
            }
            return validate;
        }

    }
}
