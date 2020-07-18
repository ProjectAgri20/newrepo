using System;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeCom
{
    /// <summary>
    /// Omni SafeCom implementation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.SafeCom.SafeComAppBase" />
    public class JediOmniSafeComApp : SafeComAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniMasthead _masthead;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly TimeSpan _idleTimeoutOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniSafeComApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniSafeComApp(JediOmniDevice device)
            : base(device.Snmp, device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _masthead = new JediOmniMasthead(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));

        }

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        public override void SignInReleaseAll(IAuthenticator authenticator)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;
            _launchHelper.PressSignInButton();
            Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
        }

        /// <summary>
        /// Presses the safe COM solution button.
        /// </summary>
        /// <param name="waitForm">The wait form.</param>
        /// <exception cref="ControlNotFoundException">Unable to press control, " + SafeComJavaScript.SolutionButtonTitle + ", at this time.</exception>
        private void PressSafeComSolutionButton(string waitForm)
        {
            _launchHelper.PressSolutionButton(SolutionButtonTitle, "SafeCom " + SolutionButtonTitle, waitForm);
        }

        /// <summary>
        /// Launches SafeCom with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
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
                PressSafeComSolutionButton(JediOmniLaunchHelper.LazySuccessScreen);
            }
            else // AuthenticationMode.Lazy
            {
                PressSafeComSolutionButton(JediOmniLaunchHelper.SignInForm);
                Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Authenticates the specified authenticator.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">The wait form.</param>
        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            _controlPanel.WaitForAvailable(waitForm);
        }

        /// <summary>
        /// Checks the device job status by control panel.
        /// </summary>
        /// <returns>
        /// bool
        /// </returns>
        protected override bool CheckDeviceJobStatusByControlPanel()
        {
            _masthead.WaitForActiveJobsButtonState(true, TimeSpan.FromSeconds(6));
            bool returnVal = _masthead.WaitForActiveJobsButtonState(false, TimeSpan.FromMinutes(5));

            return returnVal;
        }
        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        public override bool FinishedProcessingWork() => _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool StartedProcessingWork(TimeSpan ts) => _masthead.WaitForActiveJobsButtonState(true, ts);
        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void SetCopyCount(int numCopies)
        {           
            if (IsNewVersion())
            {
                ProcessCopyCount();
                UseKeyPad(numCopies);
            }
            else
            {
                ProcessCopyCount("buttonText");
                UseKeyPad(numCopies);
            }
        }

        private void UseKeyPad(int numCopies)
        {
            Thread.Sleep(1000);

            if (_controlPanel.CheckState("#hpid-keypad-key-close", OmniElementState.Exists))
            {
                _controlPanel.Type(SpecialCharacter.Backspace);
                _controlPanel.TypeOnVirtualKeyboard(numCopies.ToString());
                _controlPanel.Press("#hpid-keypad-key-close");
            }
            else
            {
                _controlPanel.Type(SpecialCharacter.RightArrow);
                _controlPanel.Type(SpecialCharacter.Backspace);
                _controlPanel.TypeOnVirtualKeyboard(numCopies.ToString());
                _controlPanel.Press("#hpid-keyboard-key-done");
            }
        }
    }
}
