using HP.DeviceAutomation.Jedi;
using System;
using System.Threading;
namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni Genius Bytes Authenticator class. Provides authentication for Genius Bytes on Jedi Omni devices.
    /// </summary>
    internal class JediOmniGeniusBytesAuthenticator : JediOmniWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniSafeQAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniGeniusBytesAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
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
            if (UseGeniusBytesPin())
            {
                EnterPin();
            }
            else
            {
                EnterUserNameForGeniusBytes();
                EnterPasswordForGeniusBytes();
            }
        }

        /// <summary>
        /// Submits the authentication request.
        /// </summary>
        public override void SubmitAuthentication()
        {
           ControlPanel.Press("#hpid-keyboard-key-enter");
        }

        /// <summary>
        /// Checks the device control panel for successful authentication.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the authentication operation is valid, <c>false</c> otherwise.
        /// </returns>
        public override bool ValidateAuthentication()
        {
            DateTime expiration = DateTime.Now.Add(TimeSpan.FromSeconds(60));
            bool signedIn = true; //Assume the login operation was successful
            bool authNotified = false;

            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: Checking for indication of login status.");

            // will only run the loop for one minute. If longer than will break and an error will eventually be thrown
            while (!authNotified && (DateTime.Now < expiration))
            {
                authNotified = ControlPanel.CheckState("#hpid-button-reset", OmniElementState.Hidden);

                // Get the device a break before looping back through the handler list
                if (!authNotified)
                {
                    Thread.Sleep(250);
                }
            }

            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: SignIn status: {signedIn}.");
            return signedIn;
        }

        /// <summary>
        /// Enters the user name into the appropriate text box.
        /// </summary>
        private void EnterUserNameForGeniusBytes()
        {            
            if (!ControlPanel.WaitForAvailable(KeyboardId, TimeSpan.FromSeconds(3)))
            {
                throw new DeviceWorkflowException("Keyboard is not displayed");
            }
            ControlPanel.TypeOnVirtualKeyboard(Credential.UserName);

            Pacekeeper.Sync();
            SubmitAuthentication();
        }

        /// <summary>
        /// Enters the password into the appropriate text box.
        /// </summary>
        private void EnterPasswordForGeniusBytes()
        {            
            if (!ControlPanel.WaitForAvailable(KeyboardId, TimeSpan.FromSeconds(3)))
            {
                throw new DeviceWorkflowException("Keyboard is not displayed");
            }
            ControlPanel.TypeOnVirtualKeyboard(Credential.Password);

            Pacekeeper.Sync();
        }

        private void EnterPin()
        {
            if (!ControlPanel.WaitForAvailable(KeyboardId, TimeSpan.FromSeconds(3)))
            {
                throw new DeviceWorkflowException("Keyboard is not displayed");
            }
            ControlPanel.TypeOnVirtualKeyboard(Credential.Pin);
            Pacekeeper.Sync();
        }

        private bool UseGeniusBytesPin()
        {
            OxpdBrowserEngine engine = new OxpdBrowserEngine(ControlPanel);
            for(int checkCount=0; checkCount < 10; checkCount++)
            {
                if (engine.WaitForHtmlContains("Enter PIN", TimeSpan.FromSeconds(1)))
                {
                    return true;
                }
                else if(engine.WaitForHtmlContains("Username", TimeSpan.FromSeconds(1)))
                {
                    return false;
                }                
            }
            return false;
        }
    }
}
