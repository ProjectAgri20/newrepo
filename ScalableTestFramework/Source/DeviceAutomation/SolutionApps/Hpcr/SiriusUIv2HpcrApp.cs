using System;
using System.Linq;
using HP.DeviceAutomation.Sirius;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr
{
    /// <summary>
    /// HPCR driver code for Sirius UI v2 HPCR App
    /// </summary>
    public class SiriusUIv2HpcrApp : HpcrAppBase
    {
        private readonly string _buttonTitle;
        private readonly string _scanDestination;

        private readonly SiriusUIv2ControlPanel _controlPanel;

        /// <summary>
        /// Contructor
        /// </summary>
        /// <param name="device"></param>
        /// <param name="buttonTitle"></param>
        /// <param name="scanDestination"></param>
        /// <param name="scanDistribution"></param>
        /// <param name="documentName"></param>
        /// <param name="imagePreview"></param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA1801:ReviewUnusedParameters", Justification = "Constructor is invoked via reflection and requires this signature.")]
        public SiriusUIv2HpcrApp(SiriusUIv2Device device, string buttonTitle, string scanDestination, string scanDistribution, string documentName, bool imagePreview)
        {
            _buttonTitle = buttonTitle;
            _controlPanel = device.ControlPanel;
            _scanDestination = scanDestination;
        }

        /// <summary>
        /// Signs into Sirius Device using authenticator information and desired authentication Mode
        /// </summary>
        /// <param name="authenticator"></param>
        /// <param name="authenticationMode"></param>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            LaunchApp();

            if (_controlPanel.WaitForActiveScreenLabel("view_sips_form", DefaultScreenWait))
            {
                UpdateStatus("Entering credentials...");
                authenticator.Authenticate();
                _controlPanel.WaitForActiveScreenLabel("view_sips_nolistorform_txtonly", TimeSpan.FromSeconds(30));
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }

            // Fill in additional values for certain HPCR workflow types
            string buttonTitle = _buttonTitle;
            if (_buttonTitle.StartsWith(HpcrAppTypes.ScanToFolder.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                UpdateStatus("Pressing button that starts with {0} ({1})", buttonTitle, _buttonTitle);
                _controlPanel.WaitForActiveScreenLabel("view_sips_vert1colImgTxt_single", TimeSpan.FromSeconds(5));
                _controlPanel.ScrollToItemByValue("sips_single_vert1colImgTxt", _scanDestination);
                _controlPanel.PressByValue(_scanDestination);
            }
            else if (_buttonTitle.StartsWith(HpcrAppTypes.PersonalDistributions.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                UpdateStatus("Pressing {0}", _buttonTitle);
                _controlPanel.WaitForActiveScreenLabel("view_sips_vert1colTxtOnly_single", TimeSpan.FromSeconds(5));
                _controlPanel.ScrollToItemByValue("sips_single_vert1colImgTxt", _scanDestination);
                _controlPanel.PressByValue(_scanDestination);
            }
            else if (_buttonTitle.StartsWith(HpcrAppTypes.PublicDistributions.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                UpdateStatus("Pressing {0}", _buttonTitle);
                _controlPanel.WaitForActiveScreenLabel("view_sips_vert1colTxtOnly_single", TimeSpan.FromSeconds(5));
                _controlPanel.ScrollToItemByValue("sips_single_vert1colImgTxt", _scanDestination);
                _controlPanel.PressByValue(_scanDestination);
            }
        }

        /// <summary>
        /// Performs Scan Job
        /// </summary>
        /// <param name="executionOptions"></param>
        /// <returns></returns>
        public override bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            int pagesScanned = 0;
            bool done = false;
            try
            {
                // wait for start scan screen
                WaitForScreenId(_controlPanel, "Ready for Scanning", DefaultScreenWait);
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                int pagesToScan = executionOptions?.JobBuildSegments ?? 1;
                while (pagesScanned < pagesToScan)
                {
                    UpdateStatus("Scanning page {0} of {1}...", pagesScanned + 1, pagesToScan);
                    if (pagesScanned > 0)
                    {
                        _controlPanel.PressByValue("Scan More");
                        WaitForScreenId(_controlPanel, "Ready for Scanning", TimeSpan.FromSeconds(6));
                    }

                    _controlPanel.PressByValue("Start");

                    WaitForScreenId(_controlPanel, "Scan Completed", TimeSpan.FromSeconds(40));

                    pagesScanned++;
                }
            }
            catch (Exception)
            {
                HpcrFinalStatus = "Failed";
                throw;
            }
            done = true;
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            HpcrFinalStatus = "Success";
            UpdateStatus("Scanned {0} pages", pagesScanned);

            return done;
        }

        /// <summary>
        /// Current Stub, Throws Exception
        /// </summary>
        public override void JobFinished()
        {
            throw new NotImplementedException("JobFinished function not implemented for this solution");
        }

        /// <summary>
        /// Launches the HPCR Scan To XX applications on the device.
        /// </summary>
        private void LaunchApp()
        {
            UpdateStatus("Pressing Apps...");
            _controlPanel.PressByValue("Apps");

            WaitForScreenLabel(_controlPanel, "view_oxpd_home", DefaultScreenWait);

            string displayedTitle = GetButtonDisplayedTitle(_buttonTitle);
            UpdateStatus("Pressing {0} ({1})", displayedTitle, _buttonTitle);
            _controlPanel.ScrollToItemByValue("oxpd_home_table", displayedTitle);

            RecordEvent(DeviceWorkflowMarker.AppButtonPress, _buttonTitle);
            _controlPanel.PressByValue(displayedTitle);
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

        private static void WaitForScreenId(SiriusUIv2ControlPanel cp, string id, TimeSpan timeToWait)
        {
            if (!cp.WaitForActiveScreenId(id, timeToWait))
            {
                var screenInfo = cp.GetScreenInfo();
                var msg = $"Unexpected screen: expected={id}, actual={screenInfo.ScreenIds.FirstOrDefault()}";
                throw new DeviceWorkflowException(msg);
            }
        }

        private static void WaitForScreenLabel(SiriusUIv2ControlPanel cp, string label, TimeSpan waitTime)
        {
            if (!cp.WaitForActiveScreenLabel(label, waitTime))
            {
                var screenInfo = cp.GetScreenInfo();
                var msg = $"Unexpected screen: expected={label}, actual={screenInfo.ScreenLabels.FirstOrDefault()}";
                throw new DeviceWorkflowException(msg);
            }
        }
    }
}
