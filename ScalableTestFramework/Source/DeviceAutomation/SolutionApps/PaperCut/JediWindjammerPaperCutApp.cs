using System;
using System.Collections.Generic;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.PaperCut
{
    /// <summary>
    /// Provides PaperCut automation for WindJammer control panel.
    /// </summary>
    public class JediWindjammerPaperCutApp : PaperCutAppBase
    {
        private const string _oxpMainForm = "OxpUIAppMainForm";

        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
       

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerPaperCutApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerPaperCutApp(JediWindjammerDevice device)
            : base(device.ControlPanel)
        {
            _device = device;
            _controlPanel = device.ControlPanel;
        }

        /// <summary>
        /// Launches PaperCut with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                if (authenticator.Provider != AuthenticationProvider.Card)
                {
                    _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                }
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);


                RecordEvent(DeviceWorkflowMarker.AppButtonPress, SolutionButtonTitle);
                _controlPanel.ScrollPressWait("mAccessPointDisplay", "Title", SolutionButtonTitle, JediWindjammerLaunchHelper.OxpdFormIdentifier, StringMatch.Contains, TimeSpan.FromSeconds(30));
            }
            else // AuthenticationMode.Lazy
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, SolutionButtonTitle);
                _controlPanel.ScrollPressWait("mAccessPointDisplay", "Title", SolutionButtonTitle, JediWindjammerLaunchHelper.SIGNIN_FORM, TimeSpan.FromSeconds(30));
                Authenticate(authenticator, JediWindjammerLaunchHelper.OxpdFormIdentifier);
            }
        }

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        public override bool SignInReleaseAll(IAuthenticator authenticator)
        {
            LogDebug("PaperCut release on sign-in is not available for Windjammer.");
            return false;
        }

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns></returns>
        public override bool BannerErrorState()
        {
            bool errorState = false;
            IEnumerable<string> controls = _controlPanel.GetControls();

            foreach (string ctlr in controls)
            {
                if (ctlr.Equals("mTitleBar"))
                {
                    string value = _controlPanel.GetProperty("mTitleBar", "Text");
                    if (value.Contains("Runtime Error"))
                    {
                        errorState = true;
                        break;
                    }
                }
            }

            return errorState;
        }

        /// <summary>
        /// Checks the device job status by querying Job Status button on control panel
        /// </summary>
        /// <returns></returns>
        protected override bool CheckDeviceJobStatusByControlPanel()
        {
            bool ready = false;

            _controlPanel.PressKey(JediHardKey.Menu);
            Thread.Sleep(TimeSpan.FromSeconds(1));
            _controlPanel.ScrollPressNavigate("mAccessPointDisplay", "Title", "Job Status", "JobStatusMainForm", true);
            var cp = _controlPanel;
            var activeJobButtonText = cp.GetProperty("mActiveJobListBox_Item0", "Column1Text");
            if (string.IsNullOrEmpty(activeJobButtonText))
            {
                ready = true;
            }
            else
            {
                // inconclusive, could not find expected button which means unexpected screen may be up
                ready = false;
            }

            return ready;
        }

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>bool</returns>
        public override bool FinishedProcessingWork()
        {
            bool done = false;

            string status = _device.Snmp.Get("1.3.6.1.4.1.11.2.3.9.1.1.3.0").ToLower();
            while (status.Contains("processing"))
            {
                Thread.Sleep(250);
                status = _device.Snmp.Get("1.3.6.1.4.1.11.2.3.9.1.1.3.0").ToLower();
            }
            done = true;
            return done;
        }

        /// <summary>
        /// Checks to see if the processing of work has started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public override bool StartedProcessingWork(TimeSpan ts)
        {
            bool startProcessing = false;
            bool timeDone = false;

            if (ts.Equals(default(TimeSpan)))
            {
                ts = TimeSpan.FromSeconds(6);
            }
            DateTime dt = DateTime.Now.Add(ts);


            string status = _device.Snmp.Get("1.3.6.1.4.1.11.2.3.9.1.1.3.0").ToLower();
            while (!status.Contains("processing") && !timeDone)
            {
                Thread.Sleep(150);
                status = _device.Snmp.Get("1.3.6.1.4.1.11.2.3.9.1.1.3.0").ToLower();
                if (DateTime.Now > dt)
                {
                    timeDone = true;
                }
                if (status.Contains("processing"))
                {
                    startProcessing = true;
                }
            }

            return startProcessing;
        }
        /// <summary>
        /// Authenticates the user for PaperCut.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">desired form after action</param>
        /// <returns>bool</returns>
        private bool Authenticate(IAuthenticator authenticator, string waitForm)
        {
            bool bSuccess = false;
            try
            {
                authenticator.Authenticate();

                if (bSuccess = _controlPanel.WaitForForm(waitForm, StringMatch.Contains, TimeSpan.FromSeconds(30)) == true)
                {
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                }

            }
            catch (WindjammerInvalidOperationException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                switch (currentForm)
                {
                    case "OxpUIAppMainForm":
                        // The application launched successfully. This happens sometimes.
                        RecordEvent(DeviceWorkflowMarker.AppShown);
                        bSuccess = true;
                        break;
                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        if (message.StartsWith("Invalid", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new DeviceWorkflowException(string.Format("Could not launch application: {0}", message), ex);
                        }
                        else
                        {
                            throw new DeviceInvalidOperationException(string.Format("Could not launch application: {0}", message), ex);
                        }

                    default:
                        throw new DeviceInvalidOperationException(string.Format("Could not launch application: {0}", ex.Message), ex);
                }
            }
            return bSuccess;
        }
    }
}
