using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeCom
{
    /// <summary>
    /// Jedi Windjammer implementation of SafeCom
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.SafeCom.SafeComAppBase" />
    public class JediWindjammerSafeComApp : SafeComAppBase
    {
        private readonly JediWindjammerControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerSafeComApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerSafeComApp(JediWindjammerDevice device)
            : base(device.Snmp, device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _isOmni = false;
        }

        /// <summary>
        /// Launches SafeCom with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressToNavigate(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM, true);
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);

                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "SafeCom " + SolutionButtonTitle);
                _controlPanel.ScrollPressWait("mAccessPointDisplay", "Title", SolutionButtonTitle, JediWindjammerLaunchHelper.OxpdFormIdentifier, StringMatch.Contains, TimeSpan.FromSeconds(30));
            }
            else // AuthenticationMode.Lazy
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "SafeCom " + SolutionButtonTitle);
                //_controlPanel.ScrollPressWait("mAccessPointDisplay", "Title", SolutionButtonTitle, JediWindjammerLaunchHelper.SIGNIN_FORM, TimeSpan.FromSeconds(30));
                _controlPanel.ScrollPressNavigate("mAccessPointDisplay", "Title", SolutionButtonTitle, JediWindjammerLaunchHelper.SIGNIN_FORM, true);
                Authenticate(authenticator, JediWindjammerLaunchHelper.OxpdFormIdentifier);
            }
        }

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        public override void SignInReleaseAll(IAuthenticator authenticator)
        {
            LogDebug("SafeCom release on sign-in is not available on Windjammer.");
        }

        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            try
            {
                authenticator.Authenticate();

                if (_controlPanel.WaitForForm(waitForm, StringMatch.Contains, TimeSpan.FromSeconds(30)))
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
                        break;
                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        if (message.StartsWith("Invalid", StringComparison.OrdinalIgnoreCase))
                        {
                            throw new DeviceWorkflowException(string.Format("Could not launch the SafeCom application: {0}", message), ex);
                        }
                        else
                        {
                            throw new DeviceInvalidOperationException(string.Format("Could not launch the SafeCom application: {0}", message), ex);
                        }

                    default:
                        throw new DeviceInvalidOperationException(string.Format("Could not launch the SafeCom application: {0}", ex.Message), ex);
                }
            }
        }

        /// <summary>
        /// Checks the device job status by control panel.
        /// </summary>
        /// <returns>
        /// bool
        /// </returns>
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
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool FinishedProcessingWork()
        {
            bool done = IsPrinting(JobStatusCheckBy.Oid);
            DateTime dt = DateTime.Now.AddSeconds(90);

            while (!done && dt > DateTime.Now)
            {
                Thread.Sleep(250);
                done = IsPrinting(JobStatusCheckBy.Oid);
            }

            return done;
        }

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        public override bool StartedProcessingWork(TimeSpan ts)
        {
            return Wait.ForTrue(() => IsPrinting(JobStatusCheckBy.Oid), TimeSpan.FromSeconds(10));
        }

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        public override void SetCopyCount(int numCopies)
        {
            LogDebug("Setting Copy count for Windjammer.");
            if (IsNewVersion())
            {
                LogInfo("SafeComGo new version");
                ProcessCopyCount();
            }
            else
            {
                LogInfo("SafeCom server 2016 version");
                ProcessCopyCount("buttonText");
            }
            Thread.Sleep(1000);

            _controlPanel.Type(SpecialCharacter.Backspace);
            _controlPanel.Type(numCopies.ToString());

            // The name of the OK button differs depending on the screen resolution
            string okButton = _controlPanel.GetControls().Contains("ok") ? "ok" : "mOkButton";

            // moved to press wait to handle the no click event within five (5) seconds
            _controlPanel.PressWait(okButton, JediWindjammerLaunchHelper.OxpdFormIdentifier, StringMatch.Contains, TimeSpan.FromSeconds(6));
        }
    }
}
