using System;
using System.Collections.Generic;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.HpRoam
{
    /// <summary>
    /// JediOmniHpRoamApp runs HpRoam automation of the Control Panel
    /// </summary>
    public class JediOmniHpRoamApp : HpRoamAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniMasthead _masthead;
        private readonly JediOmniPopupManager _jediOmniPopupManager;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniHpRoamApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniHpRoamApp(JediOmniDevice device)
            : base(device.Snmp, device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _launchHelper = new JediOmniLaunchHelper(device);
            _jediOmniPopupManager = new JediOmniPopupManager(_controlPanel);
            _masthead = new JediOmniMasthead(device);

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
        /// Launches HpRoam with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            _launchHelper.WorkflowLogger = WorkflowLogger;
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                if (authenticator.Provider != AuthenticationProvider.Card)
                {
                    _launchHelper.PressSignInButton();
                }
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton, null);
                PressHpRoamSolutionButton(JediOmniLaunchHelper.LazySuccessScreen);
            }
            else // AuthenticationMode.Lazy
            {
                switch (authenticator.Provider)
                {
                    case AuthenticationProvider.HpId:
                        PressHpRoamSolutionButton(JediOmniLaunchHelper.LazySuccessScreen);
                        TimeSpan waitTime = TimeSpan.FromSeconds(10);
                        if (!OxpdEngine.WaitToExistElementId("manual-sign-in", waitTime))
                        {
                            throw new DeviceWorkflowException($"Roam Authentication screen did not display within {waitTime.TotalSeconds} seconds.");
                        }
                        PressButton("manual-sign-in");
                        break;
                    default:
                        PressHpRoamSolutionButton(JediOmniLaunchHelper.SignInForm);
                        break;
                }                        
                Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen, null);
            }

            CheckAppReady();
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }


        private bool HandlePopups()
        {
            bool success = Wait.ForTrue(() =>
            {
                if (_jediOmniPopupManager.HandleButtonOk("#hpid-button-ok", "Insufficient") == true || _controlPanel.CheckState(JediOmniLaunchHelper.LazySuccessScreen, OmniElementState.Useable))
                {
                    return true;
                }
                return false;
            }
            , TimeSpan.FromSeconds(5));

            return success;
        }

        private void CheckAppReady()
        {
            TimeSpan interval = TimeSpan.FromSeconds(3);
            TimeSpan timeOut = TimeSpan.FromSeconds(20);

            Func<bool> action = new Func<bool>(() =>
            {
                List<string> jobListFormDescriptors = new List<string>() { "Job history" };

                foreach (string descriptor in jobListFormDescriptors)
                {
                    if (OxpdEngine.HtmlContains(descriptor))
                    {
                        return true;
                    }
                }
                return false;
            });

            if (!Wait.ForTrue(action, timeOut, interval))
            {
                throw new DeviceWorkflowException($"HP Roam did not load within {timeOut.TotalSeconds} seconds.");
            }
        }

        /// <summary>
        /// Authenticates the specified authenticator.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">The wait form.</param>
        /// <param name="parameters">The authentication parameters</param>
        private void Authenticate(IAuthenticator authenticator, string waitForm, Dictionary<string, object> parameters)
        {
            authenticator.Authenticate(parameters);
            _controlPanel.WaitForAvailable(waitForm);
        }

        private void PressHpRoamSolutionButton(string waitForm)
        {
            try
            {
                _launchHelper.PressSolutionButton(HpRoamResource.SolutionButtonTitle, HpRoamResource.SolutionButtonTitle, waitForm);
            }
            catch (DeviceWorkflowException)
            {
                if (!HandlePopups())
                {
                    throw;
                }
            }
        }

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public override bool StartedProcessingWork(TimeSpan ts) => _masthead.WaitForActiveJobsButtonState(true, ts);

        /// <summary>
        /// Signals the app to stay awake for additional functions
        /// </summary>
        public override void KeepAwake()
        {
            _controlPanel.SignalUserActivity();
        }

        /// <summary>
        /// Checks that a document has finished Printing.
        /// </summary>
        /// <param name="initialJobCount"></param>
        /// <returns></returns>
        public override bool FinishProcessDelete(int initialJobCount)
        {
            bool popUpFound = _jediOmniPopupManager.WaitForPopup(TimeSpan.FromSeconds(2));
            bool success = false;

            if (popUpFound)
            {
                _controlPanel.Press("#hpid-button-yes");
            }

            
            Wait.ForTrue(() =>
            {
                success = base.FinishProcessDelete(initialJobCount);
                return success;
            }
            , TimeSpan.FromSeconds(20));
            return success;
        }

    }
}
