using System;
using System.Linq;
using HP.ScalableTest.Utility;
using HP.DeviceAutomation.Jedi;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni PIN Authenticator class.  Provides authentication for user PIN on Jedi Omni devices.
    /// </summary>
    internal class JediOmniPinAuthenticator : JediOmniWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniPinAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniPinAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters the PIN on the device control panel.
        /// </summary>
        public override void EnterCredentials()
        {
            TimeSpan waitTime = TimeSpan.FromSeconds(10);
            if (!ControlPanel.WaitForAvailable(AuthDropdownId, waitTime))
            {
                throw new DeviceWorkflowException($"Authentication screen did not display within {waitTime.TotalSeconds} seconds.");
            }

            EnterPin();
        }

        /// <summary>
        /// Enters the PIN on the device control panel.
        /// </summary>
        protected virtual void EnterPin()
        {
            if (!VerifyKeyboard(UserNameIds))
            {
                throw new DeviceWorkflowException("Unable to find 'Code' text box.");
            }
            ControlPanel.TypeOnVirtualKeyboard(Credential.Pin);
            Pacekeeper.Sync();
        }

        /// <summary>
        /// Detects if the Authentication screen is configured for PIN authentication.
        /// </summary>
        /// <returns>true if configured for PIN auth, false otherwise.</returns>
        protected virtual bool ConfiguredForPin()
        {
            TimeSpan waitTime = TimeSpan.FromSeconds(30);

            if (! ControlPanel.WaitForAvailable(AuthDropdownId, waitTime))
            {
                throw new DeviceWorkflowException($"Authentication screen did not display within {waitTime.TotalSeconds} seconds.");
            }

            string value = GetSignInText();
            return (value.Contains("code", StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Returns the inner text from the device control panel Sign In screen HTML body.
        /// </summary>
        /// <returns>The SignIn form text.</returns>
        protected virtual string GetSignInText()
        {
            string innerText = ControlPanel.GetValue("#hpid-signin-body", "innerText", OmniPropertyType.Property).Trim();
            DateTime expiration = DateTime.Now.AddSeconds(30);

            while (string.IsNullOrEmpty(innerText) && (DateTime.Now < expiration))
            {
                Thread.Sleep(250);
                innerText = ControlPanel.GetValue("#hpid-signin-body", "innerText", OmniPropertyType.Property).Trim();
            }

            if (string.IsNullOrEmpty(innerText))
            {
                innerText = string.Empty;
            }
            return innerText;
        }

    }
}
