using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Equitrac
{
    /// <summary>
    /// Executes the Jedi Windjammer version of Equitrac
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.Equitrac.EquitracAppBase" />
    public class JediWindjammerEquitracApp : EquitracAppBase
    {
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerEquitracApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindjammerEquitracApp(JediWindjammerDevice device)
            : base(device.ControlPanel)
        {
            _device = device;
            _controlPanel = _device.ControlPanel;
        }

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns>
        /// bool
        /// </returns>
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
        /// Launches Equitrac with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            bool communicateSuccess = false;
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                if (authenticator.Provider != AuthenticationProvider.Card)
                {
                    RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                    _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                }
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);

                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Equitrac " + SolutionButtonTitle);
                communicateSuccess = _controlPanel.ScrollPressWait("mAccessPointDisplay", "Title", SolutionButtonTitle, JediWindjammerLaunchHelper.OxpdFormIdentifier, StringMatch.Contains, TimeSpan.FromSeconds(50));

            }
            else // AuthenticationMode.Lazy
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "Equitrac " + SolutionButtonTitle);
                if (_controlPanel.ScrollPressWait("mAccessPointDisplay", "Title", SolutionButtonTitle, JediWindjammerLaunchHelper.SIGNIN_FORM, TimeSpan.FromSeconds(50)) == true)
                {
                    communicateSuccess = Authenticate(authenticator, JediWindjammerLaunchHelper.OxpdFormIdentifier);
                }
            }

            if (!communicateSuccess)
            {
                throw new DeviceCommunicationException("Unable to communicate with the Equitrac server within 50 seconds.");
            }
        }

        /// <summary>
        /// Authenticates the user for HPCR.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="waitForm">desired form after action</param>
        private bool Authenticate(IAuthenticator authenticator, string waitForm)
        {
            bool success = false;
            try
            {
                authenticator.Authenticate();

                if (success = _controlPanel.WaitForForm(waitForm, StringMatch.Contains, TimeSpan.FromSeconds(50)) == true)
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
                            throw new DeviceWorkflowException(string.Format("Could not launch the Equitrac application: {0}", message), ex);
                        }
                        else
                        {
                            throw new DeviceInvalidOperationException(string.Format("Could not launch the Equitrac application: {0}", message), ex);
                        }

                    default:
                        throw new DeviceInvalidOperationException(string.Format("Could not launch the Equitrac application: {0}", ex.Message), ex);
                }
            }
            return success;
        }

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
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
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopies">The number copies.</param>
        public override void SetCopyCount(int numCopies)
        {
            ProcessCopyCount();
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
