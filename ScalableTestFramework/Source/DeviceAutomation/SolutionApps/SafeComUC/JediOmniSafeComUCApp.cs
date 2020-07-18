using System;
using System.Collections.Generic;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeComUC
{
    /// <summary>
    /// Omni SafeCom implementation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.SafeComUC.SafeComUCAppBase" />
    public class JediOmniSafeComUCApp : SafeComUCAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniMasthead _masthead;
        private readonly JediOmniLaunchHelper _launchHelper;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniSafeComUCApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniSafeComUCApp(JediOmniDevice device)
            : base(device.Snmp, device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _masthead = new JediOmniMasthead(device);
            _launchHelper = new JediOmniLaunchHelper(device);
            DeviceInactivityTimeout = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));

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
        /// <param name="solutionButtonTitle">The solution app title</param>
        /// <exception cref="ControlNotFoundException">Unable to press control, " + SafeComJavaScript.SolutionButtonTitle + ", at this time.</exception>
        private void PressSafeComUCSolutionButton(string waitForm, string solutionButtonTitle)
        {
            _launchHelper.PressSolutionButton(solutionButtonTitle, "SafeComUC " + solutionButtonTitle, waitForm);
        }

        /// <summary>
        /// Launches SafeComUC Pull Print app with the specified authenticator using the given authentication mode
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
                PressSafeComUCSolutionButton(JediOmniLaunchHelper.LazySuccessScreen, SolutionButtonTitle);
            }
            else // AuthenticationMode.Lazy
            {
                PressSafeComUCSolutionButton(JediOmniLaunchHelper.SignInForm, SolutionButtonTitle);
                Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
            }

            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Launches SafeComUC Print All app with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public override void LaunchPrintAll(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;

            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                if (authenticator.Provider != AuthenticationProvider.Card)
                {
                    _launchHelper.PressSignInButton();
                }
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
                PressSafeComUCSolutionButton(JediOmniLaunchHelper.SignInOrSignoutButton, SolutionPrintAllButtonTitle);
            }
            else // AuthenticationMode.Lazy
            {
                PressSafeComUCSolutionButton(JediOmniLaunchHelper.SignInForm, SolutionPrintAllButtonTitle);
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton);
            }
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
        /// Checks to see if the processing of work has started, or timeout was reached.
        /// </summary>
        /// <param name="action">The pull print action</param>
        /// <param name="waitTime">Time to continue checking</param>
        /// <returns>bool</returns>
        public override bool StartedProcessingWork(SafeComUCPullPrintAction action, TimeSpan waitTime)
        {
            return _masthead.WaitForActiveJobsButtonState(true, waitTime);         
        }

        /// <summary>
        /// Returns true when finished processing the current work, or timeout was reached.
        /// </summary>
        /// <param name="action">The pull print action</param>
        /// <returns>bool</returns>
        public override bool FinishedProcessingWork(SafeComUCPullPrintAction action)
        {
            return _masthead.WaitForActiveJobsButtonState(false, DeviceInactivityTimeout);
        }

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void SetCopyCount(int numCopies)
        {
            PressCopyCount();

            _controlPanel.WaitForState("#hpid-keypad-key-close", OmniElementState.Exists, true, TimeSpan.FromSeconds(3));            
            _controlPanel.TypeOnNumericKeypad(numCopies.ToString());
            _controlPanel.PressWait("#hpid-keypad-key-close", ".hp-oxpd-app-screen");            
        }

    }
}
