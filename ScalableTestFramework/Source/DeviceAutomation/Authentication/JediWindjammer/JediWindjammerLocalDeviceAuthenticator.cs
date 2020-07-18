using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation.Jedi;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    /// <summary>
    /// Jedi Windjammer Local Device Authenticator class.  Provides authentication for device admin on Jedi Windjammer devices.
    /// </summary>
    internal class JediWindjammerLocalDeviceAuthenticator : JediWindjammerWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerLocalDeviceAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediWindjammerControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediWindjammerLocalDeviceAuthenticator(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering the device admin password.
        /// </summary>
        public override void EnterCredentials()
        {
            //Click on the Dropdown
            ControlPanel.Press("mButton");
            Pacekeeper.Sync();

            //Press select Administrator Access Code
            ControlPanel.ScrollPress("mListBox", "mName", "AdminItem");
            Pacekeeper.Sync();
        }

        /// <summary>
        /// Applies additional authentication parameters before submission of authentication request.
        /// </summary>
        /// <param name="parameters">Enters the device admin password.</param>
        public override void ApplyParameters(Dictionary<string, object> parameters)
        {
            string key = parameters.Keys.First();
            ControlPanel.Type((string)parameters[key]);
            Pacekeeper.Sync();
        }
    }
}
