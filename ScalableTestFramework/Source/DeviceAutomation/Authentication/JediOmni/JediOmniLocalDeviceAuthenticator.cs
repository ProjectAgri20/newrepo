using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni Local Device Authenticator class.  Provides authentication for device admin on Jedi Omni devices.
    /// </summary>
    internal class JediOmniLocalDeviceAuthenticator : JediOmniWindowsAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniLocalDeviceAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniLocalDeviceAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by entering the device admin password.
        /// </summary>
        public override void EnterCredentials()
        {
            string adminCodeElementId = ".hp-drop-down.hp-control.hp-credential-control";
            string value = ControlPanel.GetValue(adminCodeElementId, "innerText", OmniPropertyType.Property);

            if (!value.Equals(adminCodeElementId))
            {
                SetAccessType("Administrator Access Code");
            }
        }

        /// <summary>
        /// Applies additional authentication parameters before submission of authentication request.
        /// </summary>
        /// <param name="parameters">Enters the device admin password.</param>
        public override void ApplyParameters(Dictionary<string, object> parameters)
        {
            string key = parameters.Keys.First();
            EnterAccessCode((string)parameters[key]);
            Pacekeeper.Sync();
        }

        private void SetAccessType(string accessType)
        {
            //Check to see if the text exists in the AccessType dropdown
            bool itemExists = ControlPanel.CheckState($".hp-drop-down .hp-listitem-text:contains({accessType})", OmniElementState.Exists);
            if (itemExists)
            {
                //Set the item value
                ControlPanel.Press(".hp-drop-down.hp-control.hp-credential-control");
                ControlPanel.PressWait($".hp-drop-down .hp-listitem-text:contains({accessType})", JediOmniLaunchHelper.SignInForm);
            }
            else
            {
                throw new DeviceInvalidOperationException($"Unable to set Access Type to {accessType}.");
            }
        }

        /// <summary>
        /// Enters the local device access code using the device virtual keyboard.
        /// </summary>
        /// <param name="accessCode">The access code.</param>
        private void EnterAccessCode(string accessCode)
        {
            if (ControlPanel.CheckState(".hp-textbox:last", OmniElementState.Useable))
            {
                ControlPanel.Press(".hp-textbox:last");
            }

            //VerifyKeyboard();
            ControlPanel.TypeOnVirtualKeyboard(accessCode);
        }
    }
}
