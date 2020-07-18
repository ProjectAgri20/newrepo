using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using RobotClient;
using RobotClient.Endpoints;

namespace HP.ScalableTest.DeviceAutomation.Authentication.JediWindjammer
{
    /// <summary>
    /// Jedi Windjammer Proximity Card Authenticator class.  Provides authentication for proximity cards on Jedi Windjammer devices.
    /// </summary>
    internal class JediWindjammerCardAuthenticator : JediWindjammerAppAuthenticatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerCardAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="JediOmniControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public JediWindjammerCardAuthenticator(JediWindjammerControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
            : base(controlPanel, credential, pacekeeper)
        {
        }

        /// <summary>
        /// Enters credentials on the device by scanning the proximity card.
        /// </summary>
        public override void EnterCredentials()
        {
            ScanCard();
        }

        /// <summary>
        /// Scans the proximity card.
        /// </summary>
        private void ScanCard()
        {
            RobotControllerClient badgeBox = new RobotControllerClient(Credential.BadgeBoxInfo.Address);
            RfidSwitch badgeBoxEndPoint = badgeBox.GetEndpoint<RfidSwitch>();

            Framework.Logger.LogDebug("Entering Badge Scan.");
            try
            {
                //Check card with user
                Framework.Assets.BadgeInfo badge = Credential.BadgeBoxInfo.Badges.FirstOrDefault(x => x.UserName.Equals(Credential.UserName, StringComparison.OrdinalIgnoreCase));
                if (badge == null)
                {
                    throw new DeviceInvalidOperationException($"Unable fo find badge info for User {Credential.UserName}");
                }

                Framework.Logger.LogDebug($"Swiping badge for user {badge.UserName}. (index {badge.Index})");
                var scanResult = badgeBoxEndPoint.Activate(badge.Index);
                if (ControlPanel.CurrentForm() != JediWindjammerLaunchHelper.HOMESCREEN_FORM)
                {
                    ControlPanel.WaitForForm(JediWindjammerLaunchHelper.HOMESCREEN_FORM, true);
                }
                ControlPanel.WaitForPropertyValue(JediWindjammerLaunchHelper.SIGNIN_BUTTON, "Text", "Sign Out", TimeSpan.FromSeconds(3));

                var cpResult = ValidateAuthentication();

                Framework.Logger.LogDebug(scanResult.DetectionOffset != null ? "Badge Scan Successful." : "Badge Scan Failed...");

                if (cpResult)
                {
                    // If the card does not scan, we may get a false pass after ValidateAuthentication.
                    // Do an extra check to be sure that we are not still on the home screen & not signed in.
                    if (ControlPanel.CurrentForm() == JediWindjammerLaunchHelper.HOMESCREEN_FORM
                     && ControlPanel.GetProperty(JediWindjammerLaunchHelper.SIGNIN_BUTTON, "Text") != "Sign Out")
                    {
                        throw new DeviceInvalidOperationException("No 'Sign Out' button detected.");
                    }
                    else
                    {
                        Framework.Logger.LogDebug("Login Successful");
                    }
                }
                else
                {
                    throw new DeviceInvalidOperationException("Sign in was not successful.");
                }

            }
            catch (Exception ex)
            {
                throw new DeviceInvalidOperationException($"Unable to authenticate badge scan.", ex);
            }
        }
    }
}
