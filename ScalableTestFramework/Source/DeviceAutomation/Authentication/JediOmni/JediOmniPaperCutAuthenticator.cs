using System;
using System.Collections.Generic;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Authenticates on a JediOmni control panel using Windows credentials (Username, password).
    /// Intended for use with PaperCut Agent configuration.
    /// </summary>
    internal class JediOmniPaperCutAuthenticator : JediOmniWindowsAuthenticator
    {

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniPaperCutAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniPaperCutAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Checks the device control panel for successful authentication.
        /// </summary>
        /// <returns>
        ///   <c>true</c> if the authentication operation is valid, <c>false</c> otherwise.
        /// </returns>
        public override bool ValidateAuthentication()
        {
            bool signedIn = true; //Assume the login operation was successful

            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: JediOmniPaperCutAuthenticator::ValidateAuthentication::Checking for indication of login status.");
            _popupManager.WaitForPopup(TimeSpan.FromSeconds(5));

            // HomeScreenAuthenticationNotification validates when the SignIn button was pressed.
            // OnOxpdApplicationScreen validates when the solution button was pressed.
            // SignedInNotification validates when using an older version of FW that displays the "success" popup.
            List<Func<bool>> handlers = new List<Func<bool>>
            {
                () => UserNotification(out signedIn),
                () => HomeScreenAuthenticationNotification(out signedIn),
                () => OnOxpdApplicationScreen(out signedIn),
                () => SignedInNotification(out signedIn)
            };

            InvokeValidationHandlers(handlers);

            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: JediOmniPaperCutAuthenticator::ValidateAuthentication::SignIn status: {signedIn}.");
            return signedIn;
        }

        /// <summary>
        /// This method checks for a popup that displays upon successful login in older versions of FW.
        /// </summary>
        /// <param name="signedIn"></param>
        /// <returns></returns>
        private bool SignedInNotification(out bool signedIn)
        {
            bool found = false;
            signedIn = false;

            if (found = _popupManager.HandleButtonOk("#hpid-button-Ok", "auth.success") == true)
            {
                signedIn = true;
            }
            System.Diagnostics.Debug.WriteLine($"{DateTime.Now}: JediOmniPaperCutAuthenticator::SignedInNotification::Found valid signin notification: {found}.");

            return found;
        }

    }
}
