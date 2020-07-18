using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Celiveo
{
    /// <summary>
    /// JediOmniCeliveoApp runs Celiveo automation of the Control Panel
    /// </summary>
    public class JediOmniCeliveoApp : CeliveoAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniMasthead _masthead;
        private readonly TimeSpan _idleTimeoutOffset;
        private readonly int _screenWidth = 1;

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="device"></param>
        public JediOmniCeliveoApp(JediOmniDevice device)
            : base(device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _launchHelper = new JediOmniLaunchHelper(device);
            _masthead = new JediOmniMasthead(device);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
            _screenWidth = _controlPanel.GetScreenSize().Width;
        }

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        public override bool SignInReleaseAll(IAuthenticator authenticator)
        {
            _launchHelper.PressSignInButton();
            Authenticate(authenticator);
            bool released = _controlPanel.WaitForState(".hp-label:contains(Releasing jobs)", OmniElementState.Exists);
            _controlPanel.WaitForAvailable(JediOmniLaunchHelper.SignInOrSignoutButton);
            return released;
        }

        /// <summary>
        /// Press Celiveo Solution button on home screen
        /// </summary>
        /// <param name="waitForm">Form for launched screen</param>
        private void PressCeliveoSolutionButton(string waitForm)
        {
            _launchHelper.PressSolutionButton(SolutionButtonTitle, SolutionButtonTitle, waitForm);
        }

        /// <summary>
        /// Launches Celiveo with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy</param>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                switch (authenticator.Provider)
                {
                    case AuthenticationProvider.Card:
                    case AuthenticationProvider.Skip:
                        break;
                    default:
                        _launchHelper.PressSignInButton();
                        break;
                }

                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                PressCeliveoSolutionButton(JediOmniLaunchHelper.LazySuccessScreen);
            }
            else // AuthenticationMode.Lazy
            {
                PressCeliveoSolutionButton(JediOmniLaunchHelper.SignInForm);
                Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Authenticates the specified authenticator
        /// </summary>
        /// <param name="authenticator"></param>
        /// <param name="waitForm"></param>
        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            _controlPanel.WaitForAvailable(waitForm);
        }

        /// <summary>
        /// Authenticates the user for Celiveo
        /// </summary>
        /// <param name="authenticator">The authenticator</param>
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
                    throw new DeviceWorkflowException("The Celiveo application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceInvalidOperationException(string.Format("Could not launch the Celiveo application from {0}.", currentForm), ex);
                }
            }
            catch (OmniInvalidOperationException ex)
            {
                throw new DeviceInvalidOperationException(string.Format("Could not launch the Celiveo application: {0}", ex.Message), ex);
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
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public override bool StartedProcessingWork(TimeSpan ts) => _masthead.WaitForActiveJobsButtonState(true, ts);


        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns></returns>
        public override bool FinishedProcessingWork() => _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);


        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        public override void SetCopyCount(int numCopies)
        {
            PressButton("printcopy");

            _controlPanel.WaitForAvailable("#hpid-keypad-key-close");

            _controlPanel.Type(SpecialCharacter.Backspace);
            _controlPanel.TypeOnNumericKeypad(numCopies.ToString());
            _controlPanel.Press("#hpid-keypad-key-close");
        }

    }
}
