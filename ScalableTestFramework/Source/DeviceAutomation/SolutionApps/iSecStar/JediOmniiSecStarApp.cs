using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.iSecStar
{
    /// <summary>
    /// ISecStarOmniApp runs ISecStar automation of the Control Panel
    /// </summary>
    public class JediOmniiSecStarApp : iSecStarAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniMasthead _masthead;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniiSecStarApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniiSecStarApp(JediOmniDevice device)
            : base(device.Snmp, device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _controlPanel.DefaultWaitTime = TimeSpan.FromSeconds(2);
            _launchHelper = new JediOmniLaunchHelper(device);

            _masthead = new JediOmniMasthead(device);
        }

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        public override bool SignInReleaseAll(IAuthenticator authenticator)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;
            _launchHelper.PressSignInButton();
            Authenticate(authenticator);
            bool released = _controlPanel.WaitForState(".hp-label:contains(Releasing jobs)", OmniElementState.Exists);
            _controlPanel.WaitForAvailable(JediOmniLaunchHelper.SignInOrSignoutButton);
            return released;
        }

        /// <summary>
        /// Searches for ISecStar Print Option and Presses the Print Option.
        /// </summary>
        /// <param name="waitForm"></param>
        private void PressISecStarSolutionButton(string waitForm)
        {
            string elementId = string.Empty;
            List<string> controls = _controlPanel.GetIds("Div", OmniIdCollectionType.Children).ToList();
            foreach (string controlId in controls)
            {
                if (_controlPanel.GetValue('#' + controlId, "innerText", OmniPropertyType.Property).Equals("Print"))
                {
                    elementId = '#' + controlId;
                    break;
                }
            }
            if (!_controlPanel.WaitForState(".hp-oxpd-app-screen", OmniElementState.Useable, TimeSpan.FromSeconds(5)))
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, iSecStarResource.SolutionButtonTitle);
                _controlPanel.PressWait(elementId, waitForm);
            }
        }

        /// <summary>
        /// Launches ISecStar with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            // dwa : 07/16/2019 - not updated for Story RDLINT-327 
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                if (authenticator.Provider != AuthenticationProvider.Card)
                {
                    _launchHelper.WorkflowLogger = WorkflowLogger;
                    _launchHelper.PressSignInButton();
                }
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);

                PressISecStarSolutionButton(JediOmniLaunchHelper.LazySuccessScreen);
            }
            else // AuthenticationMode.Lazy
            {
                PressISecStarSolutionButton(JediOmniLaunchHelper.SignInForm);
                Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
            }
        }

        /// <summary>
        /// Authenticates the specified authenticator.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">The relavent Id for Sign In/Sign out button on DUT.</param>
        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            _controlPanel.WaitForAvailable(waitForm);
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Authenticates the user for HEEC.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns>true on success</returns>
        private bool Authenticate(IAuthenticator authenticator)
        {
            bool bSuccess = false;
            try
            {
                authenticator.Authenticate();

                bSuccess = true;
            }
            catch (ElementNotFoundException ex)
            {
                List<string> currentForm = _controlPanel.GetIds("hp-smart-grid", OmniIdCollectionType.Children).ToList();
                if (currentForm.Contains("HomeScreenForm"))
                {
                    throw new DeviceWorkflowException("Application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceInvalidOperationException(string.Format("Cannot launch the application from {0}.", currentForm), ex);
                }
            }
            catch (OmniInvalidOperationException ex)
            {
                throw new DeviceInvalidOperationException(string.Format("Could not launch the application: {0}", ex.Message), ex);
            }

            return bSuccess;
        }

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns>bool - true if error statement exists</returns>
        public override bool BannerErrorState()
        {
            string bannerText = _controlPanel.GetValue(".hp-masthead-title:last", "innerText", OmniPropertyType.Property);
            return bannerText.Contains("Runtime Error");
        }

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        public override bool FinishedProcessingWork() => _masthead.WaitForActiveJobsButtonState(false, TimeSpan.FromMinutes(5));

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public override bool StartedProcessingWork(TimeSpan ts) => _masthead.WaitForActiveJobsButtonState(true, ts);
    }
}
