using System;
using System.Linq;
using System.Threading;
using HP.ScalableTest.Utility;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    /// <summary>
    /// Jedi Windjammer SafeCom Authenticator class.  Provides authentication for SafeCom on Jedi Windjammer devices.
    /// </summary>
    internal class JediWindjammerSafeComAuthenticator : JediWindjammerPinAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerSafeComAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediWindjammerControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediWindjammerSafeComAuthenticator(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device.
        /// If the device prompts for username and password, enters them.
        /// If the device prompts for PIN, enters PIN.
        /// </summary>
        public override void EnterCredentials()
        {
            WaitForSafeComTitle();
            CheckForSignInForm();

            if (ConfiguredForPin())
            {
                EnterPin();
                return;
            }

            EnterUserNamePassword(EnterDomainText());
            return;

            //If we end up here, we encountered an unexpected screen
            throw new ControlNotFoundException($"Expecting form {_signInFormId} but found {ControlPanel.CurrentForm()} instead.");
        }

        /// <summary>
        /// SafeCom DS does not require the domain
        /// Currently the easiest way to determine if SafeCom DS vs. SafeCom is by the authentication title:
        /// Titles:
        ///   SafeCom DS: Please enter Windows credentials
        ///   SafeCom:    Fill in fields and tap OK
        ///   
        /// Returns true if SafeCom 'NON' DS
        /// </summary>
        /// <returns>bool</returns>
        private bool EnterDomainText()
        {
            bool domainPresent = false;
            string title = GetDeviceTitle();

            if (!string.IsNullOrEmpty(title))
            {
                domainPresent = title.Contains("Fill in fields and tap OK");
            }

            return domainPresent;
        }

        /// <summary>
        /// Windows Title: "Sign In > Windows"
        /// SafeCom UserName/Pwd Title: "Fill in fields and tap OK"
        /// SafeCom PIN Title: "Please enter code" or "Enter ID code and tap OK"
        /// </summary>
        private void WaitForSafeComTitle()
        {
            string title = GetDeviceTitle();

            for (int i = 0; i < 10; i++)
            {
                if (title.Contains("code", StringComparison.OrdinalIgnoreCase) || title.Contains("fill in fields", StringComparison.OrdinalIgnoreCase))
                {
                    break;
                }

                Thread.Sleep(TimeSpan.FromSeconds(1));
                try
                {
                    title = GetDeviceTitle();
                }
                catch { } //Don't crash.  If the "contacting server" popup is present, GetDeviceTitle() will throw.
            }
        }

    }
}
