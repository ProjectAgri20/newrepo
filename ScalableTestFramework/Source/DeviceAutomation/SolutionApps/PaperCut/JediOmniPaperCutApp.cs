﻿using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCut
{
    /// <summary>
    /// PaperCutOmniApp runs PaperCut automation of the Control Panel
    /// </summary>
    public class JediOmniPaperCutApp : PaperCutAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniMasthead _masthead;
        private readonly TimeSpan _idleTimeoutOffset;
        private const string PaperCutSolutionScreen = ".hp-oxpd-app-screen";

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniPaperCutApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniPaperCutApp(JediOmniDevice device)
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

        private void PressPaperCutSolutionButton(string waitForm)
        {
            _launchHelper.PressSolutionButton(SolutionButtonTitle, SolutionButtonTitle, waitForm);
        }

        /// <summary>
        /// Launches PaperCut with the specified authenticator using the given authentication mode
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
                PressPaperCutSolutionButton(PaperCutSolutionScreen);
            }
            else // AuthenticationMode.Lazy
            {
                PressPaperCutSolutionButton(JediOmniLaunchHelper.SignInForm);
                Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
                if (authenticator.Provider == AuthenticationProvider.Card)
                {
                    PressPaperCutSolutionButton(PaperCutSolutionScreen);
                }
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
        public override bool StartedProcessingWork(TimeSpan ts) => _masthead.WaitForActiveJobsButtonState(true, ts);

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        public override bool FinishedProcessingWork() => _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);

    }
}
