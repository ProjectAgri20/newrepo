using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    internal class JediWindjammerHpacWindowsAuthenticator : JediWindjammerWindowsAuthenticator
    {
        public JediWindjammerHpacWindowsAuthenticator(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering the PIN.
        /// </summary>
        public override void EnterCredentials()
        {
            OxpdBrowserEngine engine = new OxpdBrowserEngine(ControlPanel);

            ControlPanel.WaitForControl("mOkButton", TimeSpan.FromSeconds(5));
            ControlPanel.Press("mOkButton");

            base.EnterCredentials();
        }

 
    }
}
