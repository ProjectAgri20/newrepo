using System;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Equitrac
{
    /// <summary>
    /// Equitrac Omni Application
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.Equitrac.EquitracAppBase" />
    public class JediOmniEquitracApp : EquitracAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniMasthead _masthead;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly TimeSpan _idleTimeoutOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniEquitracApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniEquitracApp(JediOmniDevice device)
            : base(device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _masthead = new JediOmniMasthead(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
        }

        private void PressEquitracSolutionButton(string waitForm)
        {
            _launchHelper.PressSolutionButton(SolutionButtonTitle, "Equitrac " + SolutionButtonTitle, waitForm);
        }

        /// <summary>
        /// Launches Equitrac with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        /// <exception cref="System.NotImplementedException"></exception>
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
                PressEquitracSolutionButton(JediOmniLaunchHelper.LazySuccessScreen);
            }
            else // AuthenticationMode.Lazy
            {
                PressEquitracSolutionButton(JediOmniLaunchHelper.SignInForm);
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
            if (!_controlPanel.WaitForAvailable(waitForm))
            {
                throw new DeviceWorkflowException(string.Format("The Equitrac solution waited to long for the form {0} to appear.", waitForm));
            }
        }

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        public override bool FinishedProcessingWork()
        {
            return _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);
        }

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public override bool StartedProcessingWork(TimeSpan ts)
        {
            return _masthead.WaitForActiveJobsButtonState(true, ts);
        }

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void SetCopyCount(int numCopies)
        {
            ProcessCopyCount();
            Thread.Sleep(1000);

            _controlPanel.Type(SpecialCharacter.Backspace);
            _controlPanel.TypeOnVirtualKeyboard(numCopies.ToString());

            // The new server is using the keyboard instead of the keypad...
            if (_controlPanel.CheckState("#hpid-keyboard-key-done", OmniElementState.Exists))
            {
                _controlPanel.Press("#hpid-keyboard-key-done");
            }
            else
            {
                _controlPanel.Press("#hpid-keypad-key-close");
            }           
        }
    }
}
