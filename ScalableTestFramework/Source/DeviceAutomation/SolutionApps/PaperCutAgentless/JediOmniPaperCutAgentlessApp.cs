using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCutAgentless
{
    /// <summary>
    /// PaperCutOmniApp runs PaperCut automation of the Control Panel
    /// </summary>
    public class JediOmniPaperCutAgentlessApp : PaperCutAgentlessAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniMasthead _masthead;
        private readonly TimeSpan _idleTimeoutOffset;

        /// <summary>
        /// Keyboard Id
        /// </summary>
        protected const string KeyboardId = "#hpid-keyboard";

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniPaperCutAgentlessApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniPaperCutAgentlessApp(JediOmniDevice device)
            : base(device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _launchHelper = new JediOmniLaunchHelper(device);

            _masthead = new JediOmniMasthead(device);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
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
        /// Press Print Release from app screen for launching PaperCutAgentless
        /// </summary>
        /// <param name="waitForm">Form after launching pull printing app</param>
        private void PressPaperCutAgentlessSolutionButton(string waitForm)
        {
            _launchHelper.PressSolutionButton(SolutionButtonTitle, SolutionButtonTitle, waitForm);
        }

        /// <summary>
        /// Launches PaperCutAgentless with the specified authenticator using the given authentication mode
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
                PressPaperCutAgentlessSolutionButton(JediOmniLaunchHelper.LazySuccessScreen);
            }
            else // AuthenticationMode.Lazy
            {
                PressPaperCutAgentlessSolutionButton(JediOmniLaunchHelper.SignInForm);
                Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Set the copies value to copies text box
        /// </summary>
        /// <param name="copies">Number for copy pages.</param>
        public override void SetCopies(int copies)
        {
            _controlPanel.WaitForState(KeyboardId, OmniElementState.Useable, TimeSpan.FromSeconds(3));

            _controlPanel.Type(SpecialCharacter.RightArrow);
            _controlPanel.Type(SpecialCharacter.Backspace);
            _controlPanel.Type(copies.ToString());

            PressDoneOnVirtualKeyboard();
        }

        /// <summary>
        /// Press Done button when UI display the keyboard
        /// </summary>
        private void PressDoneOnVirtualKeyboard()
        {
            List<string> controls = _controlPanel.GetIds("Div", OmniIdCollectionType.Children).ToList();

            if (controls.Contains("hpid-keyboard-key-done"))
            {
                if (_controlPanel.WaitForAvailable("#hpid-keyboard-key-done"))
                {
                    _controlPanel.Press("#hpid-keyboard-key-done");
                }
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
        /// Authenticates the user for PaperCut.
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
                    throw new DeviceWorkflowException("The PaperCut application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceInvalidOperationException(string.Format("Could not launch the PaperCut application from {0}.", currentForm), ex);
                }
            }
            catch (OmniInvalidOperationException ex)
            {
                throw new DeviceInvalidOperationException(string.Format("Could not launch the PaperCut application: {0}", ex.Message), ex);
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
        public override bool StartedProcessingWork(TimeSpan ts)
        {
            bool printing = true;
            if (_controlPanel.GetScreenSize().Width > 480)
            {
                printing = _masthead.WaitForActiveJobsButtonState(true, ts);
            }
            return printing;
        }

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        public override bool FinishedProcessingWork() => _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);

    }
}
