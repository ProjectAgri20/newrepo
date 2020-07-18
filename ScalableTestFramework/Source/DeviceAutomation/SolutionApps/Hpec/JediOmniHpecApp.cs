using System;
using System.Text.RegularExpressions;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpec
{
    /// <summary>
    /// Omni version of the Hpec application
    /// </summary>
    public class JediOmniHpecApp : DeviceWorkflowLogSource, IHpecApp
    {
        private const string _hpecNotificationPanel = ".hp-masthead-title:last";

        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniNotificationPanel _notificationPanel;
        private readonly OxpdBrowserEngine _engine;

        private readonly string[] _workflowOptionTitles = { "Select scan options", "Select an option" };

        /// <summary>
        /// Gets or sets the reported hpec page count.
        /// </summary>
        /// <value>
        /// The reported hpec page count.
        /// </value>
        public int ReportedHpecPageCount { get; set; } = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniHpecApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediOmniHpecApp(JediOmniDevice device)
        {
            _device = device;
            _controlPanel = _device.ControlPanel;
            _launchHelper = new JediOmniLaunchHelper(device);
            _notificationPanel = new JediOmniNotificationPanel(device);
            _engine = new OxpdBrowserEngine(_device.ControlPanel, HpecResource.HpecJavaScript);
        }

        /// <summary>
        /// Presses the HPEC solution button on the main screen and waits for
        /// the desired screen to present itself
        /// </summary>
        /// <param name="desiredScreen">The desired screen.</param>
        public void PressHpecSolutionButton(string desiredScreen)
        {
            _launchHelper.PressSolutionButton("My workflow (FutureSmart)", "HPEC", desiredScreen);
        }

        /// <summary>
        /// Launches HPEC with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
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

                PressHpecSolutionButton(_hpecNotificationPanel);
                
            }
            else // AuthenticationMode.Lazy
            {
                PressHpecSolutionButton("#hpid-signin-app-screen");
                Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="documentName">The document name.</param>
        public bool ExecuteJob(string workflow, string documentName)
        {
            bool done = false;
            if (_engine.WaitForHtmlContains("Send to", TimeSpan.FromSeconds(20)))
            {
                _engine.ExecuteFunction("pressWorkflowButton", workflow);

                ProcessDocumentName(documentName);

                _controlPanel.WaitForValue(_hpecNotificationPanel, "innerText", OmniPropertyType.Property, _workflowOptionTitles[0], StringMatch.Contains);
                done = true;
            }
            else
            {
                throw new ControlNotFoundException("The HPEC workflow button, " + workflow + ", could not be found.");
            }
            return done;
        }

        /// <summary>
        /// Presses the Hpec the scan button
        /// </summary>
        public void HpecStartScan(int pageCount)
        {
            if (pageCount > 1)
            {
                RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
            }

            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            _engine.ExecuteFunction("pressScanButton", "Scan");

            // This is sort of a pop up that covers the HPEC app
            _controlPanel.WaitForState("#hpid-button-scan", OmniElementState.Useable);

            for (int numScans = 2; numScans <= pageCount; numScans++)
            {
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                if (_controlPanel.WaitForAvailable("#hpid-button-scan"))
                {
                    RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                    _controlPanel.PressWait("#hpid-button-scan", "#hpid-button-done", TimeSpan.FromSeconds(90));
                }
            }
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            if (pageCount > 1)
            {
                RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
            }
            _controlPanel.PressWait("#hpid-button-done", "#hpid-oxpd-scroll-pane", TimeSpan.FromSeconds(90));
        }

        /// <summary>
        /// Stays in a holding pattern with Hpec processes the job after scan.
        /// </summary>
        public bool ProcessJobAfterScan()
        {
            _controlPanel.WaitForValue(".hp-masthead-title:last", "innerText", OmniPropertyType.Property, "Processing...", StringMatch.Contains, TimeSpan.FromSeconds(3));

            // Doing the check here just in case the popup occurs first. This will save waiting 30 seconds for the notification panel to fail.
            if (_controlPanel.CheckState(".hp-popup-modal-overlay", OmniElementState.Exists))
            {
                ProcessScanError();
            }
            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
            if (_notificationPanel.WaitForState(OmniElementState.VisibleCompletely))
            {
                _notificationPanel.WaitForDisplaying("Processing...");
                _notificationPanel.WaitForNotDisplaying("Processing...");
            }
            else if (_controlPanel.CheckState(".hp-popup-modal-overlay", OmniElementState.Exists))
            {
                ProcessScanError();
            }

            _controlPanel.WaitForValue(".hp-masthead-title:last", "innerText", OmniPropertyType.Property, "Result", StringMatch.Contains, TimeSpan.FromSeconds(120));
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            if (!_engine.WaitForHtmlContains("Scanned pages", TimeSpan.FromSeconds(6)))
            {
                throw new DeviceWorkflowException("Unable to determine number of pages scanned.");

            }
            return true;
        }

        /// <summary>
        /// Processes the scan error for blank pages.
        /// </summary>
        /// <exception cref="DeviceWorkflowException">Device de Brian reports \"Scanning failed. All pages were blank.\"</exception>
        private void ProcessScanError()
        {
            string value = _controlPanel.GetValue(".hp-popup-content:last", "textContent", OmniPropertyType.Property);
            if (value.Contains("Scanning failed.  All pages were blank."))
            {
                if (_controlPanel.WaitForAvailable("#hpid-button-Ok"))
                {
                    _controlPanel.Press("#hpid-button-Ok");
                    bool bExist = _controlPanel.CheckState(".hp-popup-content", OmniElementState.Exists);
                    int count = 0;
                    while (bExist && count < 3)
                    {
                        Thread.Sleep(250);
                        bExist = _controlPanel.CheckState(".hp-popup-content", OmniElementState.Exists);
                        count++;
                    }
                    _engine.ExecuteFunction("pressScanButton", "Exit");

                    throw new DeviceWorkflowException("Device de Brian reports \"Scanning failed. All pages were blank.\"");
                }
            }
        }

        /// <summary>
        /// Jobs the finished.
        /// </summary>
        public void JobFinished()
        {
            ReportedHpecPageCount = GetReportedPageCount(_engine.GetBrowserHtml());
            _engine.ExecuteFunction("pressScanButton", "Exit");
        }

        /// <summary>
        /// Workflow process to send to network folder.
        /// </summary>
        private void ProcessDocumentName(string documentName)
        {
            _controlPanel.WaitForValue(".hp-masthead-title:last", "innerText", OmniPropertyType.Property, _workflowOptionTitles[1], StringMatch.Contains, TimeSpan.FromSeconds(5));
            string textboxId = GetTextboxId(_engine.GetBrowserHtml());
            _engine.PressElementById(textboxId);
            PlayKeyBoard(documentName);
        }

        /// <summary>
        /// Gets the text box identifier for the document name.
        /// </summary>
        /// <param name="htmlResult">The HTML result.</param>
        /// <param name="count">int</param>
        /// <returns></returns>
        private string GetTextboxId(string htmlResult, int count = 0)
        {
            string docId = string.Empty;

            int iPos = htmlResult.IndexOf("textfield", 500);
            if (iPos > 500)
            {
                iPos = htmlResult.IndexOf("id=", iPos);
                if (iPos > 0)
                {
                    iPos += 4;
                    int idx = iPos;
                    while (htmlResult[idx] != '"' && idx < htmlResult.Length)
                    {
                        idx++;
                    }
                    docId = htmlResult.Substring(iPos, idx - iPos).Trim();
                }
            }
            if (string.IsNullOrEmpty(docId) && count < 3)
            {
                Thread.Sleep(250);
                docId = GetTextboxId(_engine.GetBrowserHtml(), ++count);
            }
            return docId;
        }

        /// <summary>
        /// Writes the given text into the given control ID.
        /// </summary>
        /// <param name="textToType">int</param>
        private void PlayKeyBoard(string textToType)
        {
            if (_controlPanel.WaitForAvailable("#hpid-keyboard-key-enter"))
            {
                _controlPanel.TypeOnVirtualKeyboard(textToType);
                _controlPanel.Press("#hpid-keyboard-key-enter");
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
        /// Gets the page count reported by the device
        /// </summary>
        /// <param name="stringToMatch">The string to match.</param>
        /// <returns></returns>
        private static int GetReportedPageCount(string stringToMatch)
        {
            int result = 0;
            try
            {
                var match = Regex.Match(stringToMatch, "Scanned pages:\\s*([0-9]+)", RegexOptions.IgnoreCase).Groups[1].Value;
                result = Convert.ToInt32(match);
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogError("Error getting reported page count: " + stringToMatch, ex);
                throw;
            }
            return result;
        }
    }
}
