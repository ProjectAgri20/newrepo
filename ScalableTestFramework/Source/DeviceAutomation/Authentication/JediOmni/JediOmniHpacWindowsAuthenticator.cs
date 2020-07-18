using System;
using System.Collections.Generic;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    internal class JediOmniHpacWindowsAuthenticator : JediOmniWindowsAuthenticator
    {
        private JediOmniControlPanel _controlPanel;

        public JediOmniHpacWindowsAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
            _controlPanel = controlPanel;
        }

        /// <summary>
        /// Enters credentials on the device by entering the PIN.
        /// </summary>
        public override void EnterCredentials()
        {            
            _controlPanel.PressWait("#hpid-button-signin-ok", "#hpid-signin-app-screen", TimeSpan.FromSeconds(30));
            base.EnterCredentials();
        }
    }
}
