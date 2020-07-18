using System;
using System.Text.RegularExpressions;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpec
{
    /// <summary>
    /// HPEC solution Object
    /// </summary>
    public class JediWindJammerHpecApp : DeviceWorkflowLogSource, IHpecApp
    {
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
        private readonly OxpdBrowserEngine _engine;

        private const string _entryButtonId = "C8E2EC23-F6A3-46ea-8A89-CC30FA0725FA";
        private const string _oxpMainForm = "OxpUIAppMainForm";
        private const string _homeScreenForm = "HomeScreenForm";

        /// <summary>
        /// Gets or sets the reported hpec page count.
        /// </summary>
        /// <value>
        /// The reported hpec page count.
        /// </value>
        public int ReportedHpecPageCount { get; set; } = 0;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindJammerHpecApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        public JediWindJammerHpecApp(JediWindjammerDevice device)
        {
            _device = device;
            _controlPanel = _device.ControlPanel;

            _engine = new OxpdBrowserEngine(_controlPanel, HpecResource.HpecJavaScript);

        }

        /// <summary>
        /// Launches HPEC with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy.</param>
        public void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);

                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "HPEC");
                _controlPanel.ScrollPressWait("mAccessPointDisplay", _entryButtonId, JediWindjammerLaunchHelper.OxpdFormIdentifier, StringMatch.Contains, TimeSpan.FromSeconds(30));
            }
            else // AuthenticationMode.Lazy
            {
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, "HPEC");
                _controlPanel.ScrollPressWait("mAccessPointDisplay", _entryButtonId, JediWindjammerLaunchHelper.SIGNIN_FORM, TimeSpan.FromSeconds(30));
                Authenticate(authenticator, JediWindjammerLaunchHelper.OxpdFormIdentifier);
            }
        }

        /// <summary>
        /// Executes the job.
        /// </summary>
        /// <param name="workflow">The workflow.</param>
        /// <param name="documentName">The document name.</param>
        public bool ExecuteJob(string workflow, string documentName)
        {
            bool docNameFound = false;
            if (_engine.WaitForHtmlContains(workflow, TimeSpan.FromSeconds(20)))
            {
                _engine.ExecuteFunction("pressWorkflowButton", workflow);
                string fieldName = "name=\"Document Name:\"";
                docNameFound = _engine.WaitForHtmlContains(fieldName, TimeSpan.FromSeconds(10));

                if (docNameFound)
                {
                    string documentProcess = _engine.GetBrowserHtml();
                    docNameFound = ProcessDocumentFlow(documentProcess, documentName);
                }

                if (!docNameFound)
                {
                    throw new ElementNotFoundException("Unable to find text box for entering document name: " + documentName);
                }                
            }
            else
            {
                throw new DeviceWorkflowException("Unable to execute workflow '" + workflow + "'.");
            }
            return docNameFound;
        }

        /// <summary>
        /// Sends to network folder workflow.
        /// </summary>
        /// <exception cref="DeviceInvalidOperationException">Unexpected HTML: expecting 'Media size'</exception>
        private bool ProcessDocumentFlow(string documentProcess, string documentName)
        {
            SetDocumentName(documentProcess, documentName);

            _engine.PressElementById("NEXT_BUTTON");
            if (!_engine.WaitForHtmlContains(">Media size<", TimeSpan.FromSeconds(30)))
            {
                throw new DeviceInvalidOperationException("Unexpected HTML: expecting 'Media size'");
            }
            return true;
        }

        /// <summary>
        /// Sets the name of the document.
        /// </summary>
        /// <param name="documentProcess">html string</param>
        /// <param name="documentName">The document name.</param>
        private void SetDocumentName(string documentProcess, string documentName)
        {
            string objectId = GetTextboxId(documentProcess);

            PlayKeyboard(objectId, documentName);
            Thread.Sleep(2000);
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
        /// <param name="objectId">string</param>
        /// <param name="textToType">int</param>
        private void PlayKeyboard(string objectId, string textToType)
        {
            _engine.PressElementById(objectId);

            _controlPanel.WaitForControl("mKeyboard", TimeSpan.FromSeconds(3));
            _controlPanel.TypeOnVirtualKeyboard("mKeyboard", textToType);

            Thread.Sleep(2000);

            _controlPanel.Press("ok");
        }

        /// <summary>
        /// Presses the Hpec the scan button.
        /// </summary>
        /// <exception cref="DeviceInvalidOperationException">
        /// </exception>
        public void HpecStartScan(int pageCount)
        {
            var pages = 1;

            _engine.PressElementById("NEXT_BUTTON");

            if (pageCount > 1)
            {
                RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
            }

            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            var searchFormNames = "JobBuildPrompt";
            if (_controlPanel.WaitForForm(searchFormNames, StringMatch.Regex, TimeSpan.FromSeconds(45)))
            {

                while (pages < pageCount)
                {
                    RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                    RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                    _controlPanel.Press("m_OKButton");
                    pages++;

                    if (!_controlPanel.WaitForForm(searchFormNames, StringMatch.Regex, TimeSpan.FromSeconds(45)))
                    {
                        throw new DeviceInvalidOperationException(string.Format("Unexpected form: expecting {0}, got {1}", searchFormNames, _engine.GetBrowserHtml()));
                    }
                }
            }
            else
            {
                throw new DeviceInvalidOperationException(string.Format("Unexpected form: expecting {0}", searchFormNames));
            }
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);

            if (pageCount > 1)
            {
                RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
            }

            _controlPanel.Press("mFinishButton");
        }

        /// <summary>
        /// Stays in a holding pattern with Hpec processes the job after scan.
        /// </summary>
        public bool ProcessJobAfterScan()
        {
            bool done = false;
            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
            bool processing = _engine.HtmlContains("Processing...");

            if (!processing)
            {
                if (_engine.HtmlContains("Scan process has encountered an issue."))
                {
                    ProcessScanError();
                }
            }

            while (processing)
            {
                Thread.Sleep(250);
                processing = _engine.HtmlContains("Processing...");
            }
            if (_engine.HtmlContains("Scan process has encountered an issue."))
            {
                ProcessScanError();
            }
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
            done = true;
            return done;
        }

        private void ProcessScanError()
        {
            _engine.ExecuteFunction("pressScanButton", "Exit");
            throw new DeviceWorkflowException("Device de Brian reports \"Scan process has encountered an issue. No page in flatbed?\"");
        }

        /// <summary>
        /// Jobs are finished.
        /// </summary>
        /// <exception cref="DeviceInvalidOperationException">
        /// HPEC reported an error:
        /// or
        /// Unexpected HTML: expecting 'Scanned pages'
        /// </exception>
        public void JobFinished()
        {
            // Wait for Scanned pages output
            if (_engine.WaitForHtmlContains("Scanned pages", TimeSpan.FromSeconds(90)))
            {
                if (_engine.HtmlContains("Success"))
                {
                    ReportedHpecPageCount = GetReportedPageCount(_engine.GetBrowserHtml());

                    _engine.ExecuteFunction("pressScanButton", "Exit");

                }
                else
                {
                    throw new DeviceWorkflowException("HPEC reported an error:");
                }
            }
            else
            {
                throw new DeviceWorkflowException("Unexpected HTML: expecting 'Scanned pages'");
            }
        }

        /// <summary>
        /// Authenticates the user for HPCR.
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

        /// <summary>
        /// Gets the page count reported by the device
        /// </summary>
        /// <param name="stringToMatch">The string to match.</param>
        /// <returns></returns>
        private static int GetReportedPageCount(string stringToMatch)
        {
            int result = 0;
            var match = Regex.Match(stringToMatch, "Scanned pages:\\s*([0-9]+)", RegexOptions.IgnoreCase).Groups[1].Value;
            result = Convert.ToInt32(match);

            return result;
        }
    }
}
