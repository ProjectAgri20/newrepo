using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Oz;
using HP.ScalableTest.DeviceAutomation.Authentication;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr
{
    /// <summary>
    /// Oz WindJammer HPCR App
    /// </summary>
    public class OzWindJammerHpcrApp : HpcrAppBase
    {
        private readonly string _buttonTitle = "Scan To X";

        private const int _homeScreen = 567;
        private const int _signInScreen = 18;
        private const int _signInKeyboard = 32;
        private const int _scanScreen = 1052;

        private readonly OzWindjammerControlPanel _controlPanel;
        private int _lastScreenId;

        /// <summary>
        /// Constructor with given button to press
        /// </summary>
        /// <param name="device"></param>
        /// <param name="buttonTitle"></param>
        public OzWindJammerHpcrApp(OzWindjammerDevice device, string buttonTitle) : base()
        {
            _buttonTitle = buttonTitle;
            _controlPanel = device.ControlPanel;
        }

        /// <summary>
        /// Signs into WindJammer using the authenticator method and ignores authentication Mode
        /// </summary>
        /// <param name="authenticator"></param>
        /// <param name="authenticationMode"></param>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticator == null)
            {
                throw new ArgumentNullException(nameof(authenticator));
            }

            Widget appButton = _controlPanel.ScrollToItem("Title", _buttonTitle);
            if (appButton != null)
            {
                UpdateStatus("Pressing {0}", _buttonTitle);
                RecordEvent(DeviceWorkflowMarker.AppButtonPress);
                _controlPanel.Press(appButton);
                bool promptedForCredentials = _controlPanel.WaitForScreen(_signInScreen, TimeSpan.FromSeconds(6));
                if (promptedForCredentials)
                {
                    UpdateStatus("Entering credentials");
                    authenticator.Authenticate();
                    _controlPanel.WaitForScreen(_scanScreen, TimeSpan.FromSeconds(15));
                    RecordEvent(DeviceWorkflowMarker.AppShown);
                }

                GetCurrentForm();
            }
            else
            {
                if (GetCurrentForm() == _homeScreen)
                {
                    throw new DeviceWorkflowException(string.Format("Application {0} button was not found on device home screen.", _buttonTitle));
                }
                else
                {
                    throw new DeviceInvalidOperationException(string.Format("Cannot launch the {0} application: Not at device home screen.", _buttonTitle));
                }
            }
        }

        /// <summary>
        /// Executes Scan Job
        /// </summary>
        /// <param name="executionOptions"></param>
        /// <returns></returns>
        public override bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            VerifyReadyForScanning(true);
            bool done = false;
            int pagesScanned = 0;
            int pagesToScan = (executionOptions == null ? 1 : executionOptions.JobBuildSegments);
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            while (pagesScanned < pagesToScan)
            {
                UpdateStatus("Scanning page {0} of {1}...", pagesScanned + 1, pagesToScan);
                if (pagesScanned > 0)
                {
                    // wait for scan completed 
                    WaitForWidget("Scan Completed", StringMatch.Contains, TimeSpan.FromSeconds(6));

                    // press button Scan More
                    _controlPanel.PressScreen(new Coordinate(560, 65));
                    VerifyReadyForScanning(true);
                }

                _controlPanel.PressKey(OzHardKey.Start);
                WaitForWidget("Scan Completed", StringMatch.Contains, TimeSpan.FromSeconds(30));
                pagesScanned++;
            }
            done = true;
            HpcrFinalStatus = "Success";
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            UpdateStatus("Scanned {0} pages", pagesScanned);

            // Press button Finished
            _controlPanel.PressScreen(new Coordinate(560, 175));


            if (!_controlPanel.WaitForScreen(_homeScreen, TimeSpan.FromSeconds(30)))
            {
                HpcrFinalStatus = "Failed";
                throw new DeviceInvalidOperationException("Unable to return to home screen");
            }
            // sign out
            RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);
            _controlPanel.PressScreen(new Coordinate(565, 220));
            RecordEvent(DeviceWorkflowMarker.DeviceSignOutEnd);
            return done;
        }

        /// <summary>
        /// Throws NotImplemented Exception
        /// </summary>
        public override void JobFinished()
        {
            throw new NotImplementedException("JobFinished() is not implemented in the OzWindJammerHpcrApp");
        }

        private bool VerifyReadyForScanning(bool throwIfNotReady)
        {
            bool result = false;
            var widget = WaitForWidget("Ready for scanning", StringMatch.Contains, TimeSpan.FromSeconds(6));
            if (widget != null)
            {
                result = true;
            }

            if (!result && throwIfNotReady)
            {
                throw new DeviceInvalidOperationException("Device not ready for scanning.");
            }

            return result;
        }

        private Widget WaitForWidget(string search, StringMatch matchMethod, TimeSpan waitTime)
        {
            var widget = Wait.ForNotNull(() =>
            {
                var widgets = _controlPanel.GetWidgets();
                return widgets.Find(search, matchMethod);
            }
                , waitTime
                );
            return widget;
        }

        private int GetCurrentForm()
        {
            _lastScreenId = _controlPanel.ActiveScreenId();
            return _lastScreenId;
        }
    }
}
