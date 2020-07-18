using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Utility;
using RobotClient;
using RobotClient.Endpoints;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediOmni
{
    /// <summary>
    /// Jedi Omni Proximity Card Authenticator class.  Provides authentication for proximity cards on Jedi Omni devices.
    /// </summary>
    internal class JediOmniCardAuthenticator : JediOmniAppAuthenticatorBase
    {
        private BadgeInfo _badge = null;
        private RfidSwitch _badgeBoxEndPoint = null;
        private TimeSpan _validationTimeout = TimeSpan.FromSeconds(15);
      
        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniCardAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediOmniCardAuthenticator(JediOmniControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
            //Check card with user
            _badge = Credential.BadgeBoxInfo.Badges.FirstOrDefault(x => x.UserName.Equals(Credential.UserName, StringComparison.OrdinalIgnoreCase));
            if (_badge == null)
            {
                throw new DeviceInvalidOperationException($"Unable to find badge info for User {Credential.UserName}");
            }

            RobotControllerClient badgeBox = new RobotControllerClient(Credential.BadgeBoxInfo.Address);
            _badgeBoxEndPoint = badgeBox.GetEndpoint<RfidSwitch>();
        }
        /// <summary>
        /// Activating the badge box for card authentication
        /// </summary>
        /// <param name="parameters"></param>
        /// <exception cref="DeviceInvalidOperationException"></exception>
        public override void ApplyParameters(Dictionary<string, object> parameters)
        {
            Framework.Logger.LogDebug($"Swiping badge for user {_badge.UserName}. (index {_badge.Index})");
            var scanResult = _badgeBoxEndPoint.Activate(_badge.Index);

            Framework.Logger.LogDebug(scanResult.DetectionOffset != null ? "Badge Scan Successful." : "Badge Scan Failed...");
        }
        /// <summary>
        /// Proximity card has been scanned. Waiting for the device to respond.
        /// </summary>
        public override void EnterCredentials()
        {
            bool authSucceeded = false;
            try
            {
                Framework.Logger.LogDebug("Starting Validation of sign in.");
                authSucceeded = ValidateAuthentication();
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException("Unable to authenticate badge scan.", ex);
            }

            if (!authSucceeded)
            {
                throw new DeviceWorkflowException($"Sign in was not successful within {_validationTimeout.TotalSeconds} seconds.");

            }

            Framework.Logger.LogDebug("Validation of sign in was successful.");
        }

        /// <summary>
        /// Submits the authentication request.
        /// Does nothing in the card authenticator.  By the time this method is called, the authentication has been completed.
        /// </summary>
        public override void SubmitAuthentication()
        {
        }

        /// <summary>
        /// Checks the device control panel for successful authentication.
        /// </summary>
        /// <returns><c>true</c> if the authentication operation is valid, <c>false</c> otherwise.</returns>
        public override bool ValidateAuthentication()
        {
            return Wait.ForTrue(() => SignedIn(), _validationTimeout, TimeSpan.FromMilliseconds(250));
        }

        private bool SignedIn()
        {             
            string signInButtonText = ControlPanel.GetValue(JediOmniLaunchHelper.SignInOrSignoutButton, "innerText", OmniPropertyType.Property).Trim();
            return signInButtonText.Equals("Sign Out");
        }

    }
}
