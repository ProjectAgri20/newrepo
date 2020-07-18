using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.Utility;
using RobotClient;
using RobotClient.Endpoints;
using RobotClient.Endpoints.Data;
using HP.ScalableTest.Framework.Assets;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.Authentication.SiriusUIv3
{
    /// <summary>
    /// Sirius UIv3 Proximity Card Authenticator class.  Provides authentication for proximity cards Sirius UIv3 devices.
    /// </summary>
    internal class SiriusUIv3CardAuthenticator : SiriusUIv3AppAuthenticatorBase
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusUIv3CardAuthenticator" /> class.
        /// </summary>
        /// <param name="controlPanel">The <see cref="SiriusUIv3ControlPanel"/> object.</param>
        /// <param name="credential">The <see cref="AuthenticationCredential"/> object.</param>
        /// <param name="pacekeeper">The <see cref="Pacekeeper"/> object.</param>
        public SiriusUIv3CardAuthenticator(SiriusUIv3ControlPanel controlPanel, AuthenticationCredential credential, Pacekeeper pacekeeper)
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
        /// Submits the authentication request.
        /// Does nothing in the card authenticator.  By the time this method is called, the authentication has been completed.
        /// </summary>
        public override void SubmitAuthentication()
        {
        }

        /// <summary>
        /// Scans the proximity card.
        /// </summary>
        private void ScanCard()
        {
            RobotControllerClient badgeBox = new RobotControllerClient(Credential.BadgeBoxInfo.Address);
            RfidSwitch badgeBoxEndPoint = badgeBox.GetEndpoint<RfidSwitch>();
            RfidActivateResult scanResult = null;

            LogDebug("Entering Badge Scan.");
            try
            {
                //Check card with user
                BadgeInfo badge = Credential.BadgeBoxInfo.Badges.FirstOrDefault(x => x.UserName.EqualsIgnoreCase(Credential.UserName));
                if (badge == null)
                {
                    throw new DeviceWorkflowException($"Unable fo find badge info for User {Credential.UserName}");
                }

                LogDebug($"Swiping badge for user {badge.UserName}. (index {badge.Index})");
                scanResult = badgeBoxEndPoint.Activate(badge.Index);

                if (scanResult.DetectionOffset != null)
                {
                    LogDebug("Badge Scan Successful.");
                }
                else
                {
                    LogDebug("Badge Scan Failed.");
                }

                //Handle Notification Screen, if present.  It may not be present for all cases.
                Widget okButton =ControlPanel.WaitForWidgetByValue("OK", TimeSpan.FromSeconds(1));
                if (okButton != null)
                {
                    ControlPanel.Press(okButton);
                    Pacekeeper.Sync();
                }

                if (ValidateAuthentication())
                {
                    LogDebug("Login Successful.");
                }
                else
                {
                    throw new DeviceWorkflowException("No 'Sign Out' button detected.");
                }

                Thread.Sleep(1000);
            }
            catch (Exception ex)
            {
                throw new DeviceWorkflowException($"Unable to authenticate badge scan.", ex);
            }
        }
    }
}
