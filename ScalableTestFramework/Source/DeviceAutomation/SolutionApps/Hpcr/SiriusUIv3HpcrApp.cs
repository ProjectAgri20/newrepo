using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr
{
    /// <summary>
    /// Sirius v3 HPCR App
    /// </summary>
    public class SiriusUIv3HpcrApp : HpcrAppBase
    {
        private readonly string _buttonTitle;
        private readonly string _scanDestination;
        private readonly string _scanDistribution;

        private readonly SiriusUIv3ControlPanel _controlPanel;

        /// <summary>
        /// Constructor for HPCR Sirius App
        /// </summary>
        /// <param name="device"></param>
        /// <param name="buttonTitle"></param>
        /// <param name="scanDestination"></param>
        /// <param name="scanDistribution"></param>
        /// <param name="documentName"></param>
        /// <param name="imagePreview"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Constructor is invoked via reflection and requires this signature.")]
        public SiriusUIv3HpcrApp(SiriusUIv3Device device, string buttonTitle, string scanDestination, string scanDistribution, string documentName, bool imagePreview)
        {
            _buttonTitle = buttonTitle;
            _controlPanel = device.ControlPanel;
            _scanDestination = scanDestination;
            _scanDistribution = scanDistribution;
        }

        /// <summary>
        /// Signs into HPCR using given authenticator using desired authentication mode
        /// </summary>
        /// <param name="authenticator"></param>
        /// <param name="authenticationMode"></param>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                // Press Sign In button
                _controlPanel.PressByValue("Sign In");

                if (_controlPanel.WaitForScreenId("login", StringMatch.Contains))
                {
                    authenticator.Authenticate();
                    PressHpcrWorkflowButton(_buttonTitle);
                }
                else
                {
                    throw new DeviceWorkflowException("Timed out waiting for Sign In screen.");
                }
            }
            else // AuthenticationMode.Lazy
            {
                PressHpcrWorkflowButton(_buttonTitle);

                authenticator.Authenticate();
                _controlPanel.WaitForScreenLabel("vw_sips_apps_state", TimeSpan.FromSeconds(30));
                WaitForControl("HP Capture", TimeSpan.FromSeconds(6));
            }

            RecordEvent(DeviceWorkflowMarker.AppShown);

        }

        /// <summary>
        /// Executes HPCR Scan Job
        /// </summary>
        /// <param name="executionOptions"></param>
        /// <returns></returns>
        public override bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            bool done = false;
            int pagesScanned = 0;

            try
            {
                NavigateWorkflowPath();

                // wait for start scan screen
                WaitForControl("Ready for Scanning", TimeSpan.FromSeconds(6));

                int pagesToScan = executionOptions?.JobBuildSegments ?? 1;

                if (pagesToScan > 1)
                {
                    RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
                }

                while (pagesScanned < pagesToScan)
                {
                    UpdateStatus("Scanning page {0} of {1}...", pagesScanned + 1, pagesToScan);
                    if (pagesScanned > 0)
                    {
                        _controlPanel.WaitForWidgetByValue("Scan More");
                        _controlPanel.PressByValue("Scan More");
                        WaitForControl("Ready for Scanning", DefaultScreenWait);
                    }
                    RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                    UpdateStatus("Pressing start button");
                    _controlPanel.WaitForWidgetByValue("Start", TimeSpan.FromSeconds(10));
                    _controlPanel.PressByValue("Start");

                    UpdateStatus("Scan activity started");
                    WaitForControl("Scan Completed", DefaultScreenWait);
                    RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                    pagesScanned++;
                }
                done = true;
            }
            catch (Exception)
            {
                HpcrFinalStatus = "Failed";
                throw;
            }
            HpcrFinalStatus = "Success";
            UpdateStatus("Scanned {0} pages", pagesScanned);
            return done;
        }

        private void PressHpcrWorkflowButton(string buttonTitle)
        {
            if (_controlPanel.WaitForScreenLabel("Home", DefaultScreenWait))
            {
                //Scroll to Apps and Press
                _controlPanel.ScrollPressByValue("sfolderview_p", "Apps");

                string displayedTitle = GetButtonDisplayedTitle(buttonTitle);

                _controlPanel.WaitForWidget("sfolderview_p");

                UpdateStatus("Pressing {0} ({1})", displayedTitle, buttonTitle);
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, buttonTitle);

                _controlPanel.ScrollPressByValue("sfolderview_p", displayedTitle, StringMatch.StartsWith);
            }
            else
            {
                throw new DeviceWorkflowException("Unable to navigate to Home Screen.");
            }

        }

        /// <summary>
        /// The various workflows have different paths. Setup for processing different HPCR workflows.
        /// </summary>
        private void NavigateWorkflowPath()
        {
            UpdateStatus("Pressing {0}", _buttonTitle);
            if (_buttonTitle.StartsWith(HpcrAppTypes.ScanToFolder.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                VerifyDestination(_scanDestination);
                PressPanelButton(_scanDestination);
            }
            else if (_buttonTitle.StartsWith(HpcrAppTypes.PersonalDistributions.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                VerifyDestination(_scanDistribution);
                PressPanelButton(_scanDistribution);
            }
            else if (_buttonTitle.StartsWith(HpcrAppTypes.PublicDistributions.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                VerifyDestination(_scanDestination);
                PressPanelButton(_scanDistribution);
            }
        }

        /// <summary>
        /// Checks to ensure User has supplied a destination folder.
        /// </summary>
        /// <param name="destination"></param>
        private static void VerifyDestination(string destination)
        {
            // User has supplied a destination folder
            if (string.IsNullOrEmpty(destination))
            {
                throw new DeviceWorkflowException("SiriusUIv3HpcrApp.FolderDistributionButton - Unable to determine workflow destination.");
            }
        }

        /// <summary>
        /// Presses a button within an HPCR workflow.
        /// </summary>
        /// <param name="buttonTitle"></param>
        private void PressPanelButton(string buttonTitle)
        {
            UpdateStatus("Pressing button that starts with '{0}'", buttonTitle);
            _controlPanel.WaitForWidgetByValue(buttonTitle);
            _controlPanel.Press(_controlPanel.GetScreenInfo().Widgets.FindByValue(buttonTitle).Id);
        }

        /// <summary>
        /// Gets the button displayed title based on an assumed max length with "..." at the end
        /// </summary>
        /// <param name="buttonTitle">The button title.</param>
        /// <param name="maxDisplayLength">Maximum length of the display.</param>
        /// <returns>System.String.</returns>
        /// <exception cref="System.NotImplementedException"></exception>
        private static string GetButtonDisplayedTitle(string buttonTitle, int maxDisplayLength = 14)
        {
            var result = buttonTitle;
            if (buttonTitle.Length >= maxDisplayLength && !buttonTitle.EndsWith("..."))
            {
                result = buttonTitle.Substring(0, maxDisplayLength - 3) + "...";
            }
            return result;
        }

        /// <summary>
        /// Wait For Screen
        /// </summary>
        /// <param name="key"></param>
        /// <param name="waitTime"></param>
        private bool WaitForControl(string key, TimeSpan waitTime) => Wait.ForTrue(() => GetControl(key), waitTime);

        /// <summary>
        /// waits for the control
        /// </summary>
        /// <param name="key">key name</param>
        /// <returns>bool</returns>
        private bool GetControl(string key)
        {
            using (var enumerator = _controlPanel.GetScreenInfo().Widgets.GetEnumerator())
            {
                while (enumerator.MoveNext())
                    if (enumerator.Current.Values.Count > 0)
                    {
                        if (enumerator.Current.Values.Values.Any(value => value.Contains(key)))
                        {
                            return true;
                        }
                    }
            }

            return false;
        }

        /// <summary>
        /// Finishes the Job.
        /// </summary>
        public override void JobFinished()
        {
            //Nothing to do.
        }
    }
}