using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni SafeCom Authenticator class.  Provides authentication for SafeCom on Jedi Omni devices.
    /// </summary>
    internal class JediOmniSafeComAuthenticator : JediOmniPinAuthenticator
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniSafeComAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniSafeComAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
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
            if (UseSafeComLdap())
            {
                EnterUserNamePassword();
            }
            else
            {
                EnterPin();
            }
        }
        
        private bool UseSafeComLdap()
        {            
            bool useSafeComLdap = true;
            JediOmniMasthead masthead = new JediOmniMasthead(ControlPanel);

            if (ControlPanel.CheckState(masthead.MastheadSpinner, OmniElementState.Exists))
            {
                if (Wait.ForTrue(() => masthead.BusyStateActive(), TimeSpan.FromSeconds(30)))
                {
                    throw new DeviceWorkflowException("SafeCom Authentication screen did not show within 30 seconds.");
                }
            }
            if (ControlPanel.WaitForAvailable(AuthDropdownId, TimeSpan.FromSeconds(30)))
            {
                string value = GetSignInText();
                if (value.Contains("code", StringComparison.OrdinalIgnoreCase))
                {
                    useSafeComLdap = false;
                }               
            }
            else
            {
                throw new DeviceWorkflowException("Unable to determine type of authentication (LDAP/PIN) for SafeCom.");
            }
            return useSafeComLdap;
        }

    }
}
