using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr
{
    /// <summary>
    /// Jedi Windjammer application version 
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr.HpcrAppBase" />
    public class JediWindJammerHpcrApp : HpcrAppBase
    {
        private readonly string _buttonTitle = "Scan To Me";
        private readonly string _scanDestination = string.Empty;
        private readonly string _scanDistribution = string.Empty;
        private readonly string _documentName = string.Empty;
        private readonly bool _imagePreview = false;

        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;
        private readonly OxpdBrowserEngine _engine;

        private int _yOffset;
        private const int _footerOffset = 50;     // This is based on a 800 x 600 screen. If the screen is smaller, this value will likely need changing.

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindJammerHpcrApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buttonTitle">The button title.</param>
        /// <param name="scanDestination">The scan destination.</param>
        /// <param name="scanDistribution">The scan distribution.</param>
        /// <param name="documentName">Name of the document.</param>
        /// <param name="imagePreview">Whether Image preview is on/off</param>
        public JediWindJammerHpcrApp(JediWindjammerDevice device, string buttonTitle, string scanDestination, string scanDistribution, string documentName, bool imagePreview)
            : base()
        {
            _buttonTitle = buttonTitle;
            _device = device;
            _controlPanel = device.ControlPanel;
            _documentName = documentName;
            _engine = new OxpdBrowserEngine(device.ControlPanel);
            _scanDestination = scanDestination;
            _scanDistribution = scanDistribution;
            _imagePreview = imagePreview;
        }

        /// <summary>
        /// Launches The Hpcr solution with the given authenticator with either eager or lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="System.ArgumentNullException">authenticator</exception>
        /// <exception cref="DeviceWorkflowException">Application button was not found on device home screen.</exception>
        /// <exception cref="DeviceInvalidOperationException">
        /// </exception>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticator == null)
            {
                throw new ArgumentNullException(nameof(authenticator));
            }

            // Press the desired HPCR Button type
            try
            {
                if (authenticationMode.Equals(AuthenticationMode.Eager))
                {
                    UpdateStatus("Pressing {0}", JediWindjammerLaunchHelper.SIGNIN_BUTTON);
                    RecordEvent(DeviceWorkflowMarker.DeviceButtonPress, "Sign In");
                    _controlPanel.PressWait(JediWindjammerLaunchHelper.SIGNIN_BUTTON, JediWindjammerLaunchHelper.SIGNIN_FORM);
                    Authenticate(authenticator, JediWindjammerLaunchHelper.HOMESCREEN_FORM);

                    UpdateStatus("Pressing {0}", _buttonTitle);
                    RecordEvent(DeviceWorkflowMarker.AppButtonPress, "HPCR " + _buttonTitle);
                    _controlPanel.ScrollPressWait("mAccessPointDisplay", "Title", _buttonTitle, JediWindjammerLaunchHelper.OxpdFormIdentifier, StringMatch.Contains, TimeSpan.FromSeconds(30));
                }
                else // AuthenticationMode.Lazy
                {
                    UpdateStatus("Pressing {0}", _buttonTitle);
                    RecordEvent(DeviceWorkflowMarker.AppButtonPress, "HPCR " + _buttonTitle);
                    _controlPanel.ScrollPressWait("mAccessPointDisplay", "Title", _buttonTitle, JediWindjammerLaunchHelper.SIGNIN_FORM, TimeSpan.FromSeconds(30));
                    Authenticate(authenticator, JediWindjammerLaunchHelper.OxpdFormIdentifier);
                }
                RecordEvent(DeviceWorkflowMarker.AppShown);
            }
            catch (ControlNotFoundException ex)
            {
                string currentForm = _controlPanel.CurrentForm();
                if (currentForm == "HomeScreenForm")
                {
                    throw new DeviceWorkflowException("The HPCR application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceInvalidOperationException(string.Format("Could not launch the HPCR application from {0}.", currentForm), ex);
                }
            }
            catch (WindjammerInvalidOperationException ex)
            {
                var currentForm = _controlPanel.CurrentForm();
                if (currentForm.StartsWith(JediWindjammerLaunchHelper.OxpdFormIdentifier, StringComparison.OrdinalIgnoreCase))
                {
                    // The application launched without prompting for credentials.
                }

                switch (currentForm)
                {
                    case "OneButtonMessageBox":
                        string message = _controlPanel.GetProperty("mMessageLabel", "Text");
                        throw new DeviceInvalidOperationException(string.Format("Could not launch the HPCR application: {0}", message), ex);

                    default:
                        throw new DeviceInvalidOperationException(string.Format("Could not launch the HPCR application: {0}", ex.Message), ex);
                }
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
                UpdateStatus("Authenticating...");

                authenticator.Authenticate();

                bSuccess = _controlPanel.WaitForForm(waitForm, StringMatch.Contains, TimeSpan.FromSeconds(30));                                                  
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
        /// Executes the job.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        /// <exception cref="System.Exception">Unable to complete scan on device</exception>
        public override bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            if (ProcessForHpcr(executionOptions.JobBuildSegments))
            {
                HpcrFinalStatus = "Success";
            }
            else
            {
                HpcrFinalStatus = "Failed";
                throw new DeviceWorkflowException("Unable to complete scan on device");
            }
            return true;
        }

        /// <summary>
        /// Method used to determine or run the end results of the job
        /// </summary>
        /// <exception cref="System.NotImplementedException">HPCR.JediWindjammerHpcrApp.JobFinished - not implemented.</exception>
        public override void JobFinished()
        {
            //throw new NotImplementedException("HPCR.JediWindjammerHpcrApp.JobFinished - not implemented.");
        }

        /// <summary>
        /// Determines whether the devices current form is requesting the document name.
        /// </summary>
        /// <returns>bool</returns>
        private bool IsDocumentFieldName()
        {
            bool bDocumentField = true;

            string result = _engine.GetBrowserHtml();
            if (!result.Contains("id=\"FieldDeliveredDocumentName\""))
            {
                bDocumentField = false;
            }
            return bDocumentField;
        }

        /// <summary>
        /// Calls the helping methods for processing the various HPCR workflows.
        /// </summary>
        /// <returns>bool</returns>
        private bool ProcessForHpcr(int pageCount = 1)
        {
            bool bSuccess = true;
            _yOffset = OxpdBrowserEngine.GetBrowserOffset(_controlPanel);
            TimeSpan ts = TimeSpan.FromSeconds(6);

            ProcessHPCR_Workflows();

            // if the document name is required for this app...
            if (IsDocumentFieldName())
            {
                SetDocumentName();

                // Press the next button
                PressHpcrButtonByCoordinate("More Options", 0);
            }

            VerifyReadyForScanning(true);
            ProcessHPCR_Work(pageCount, ts);

            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);

            // press the done button based on its index
            _engine.WaitForHtmlContains("Done", TimeSpan.FromSeconds(6));

            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            PressHpcrButtonByCoordinate(string.Empty, 1);

            AtHomeScreen(ts);

            return bSuccess;
        }

        /// <summary>
        /// Processes the HPCR workflow.
        /// </summary>
        /// <param name="pageCount">The page count.</param>
        /// <param name="ts">time span</param>
        private void ProcessHPCR_Work(int pageCount, TimeSpan ts)
        {
            // pressing the start button and waiting for the done button
            if (_imagePreview)
            {
                ImagePreviewHpcr(ts);
            }
            else if (pageCount == 1)
            {
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                PressHpcrButtonByCoordinate("Done", 0);
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            }
            else
            {
                // job build workflow - will be pressing More Options
                JobBuilderHPCR(pageCount, ts);
            }
        }

        /// <summary>
        /// Process for implementing the Jedi Image Preview workflow.
        /// </summary>
        /// <param name="ts">The ts.</param>
        private void ImagePreviewHpcr(TimeSpan ts)
        {
            PressHpcrButtonByCoordinate("", 1);
            PressPanelButton("Image Preview:");
            PressPanelButton("On");

            // press ok
            PressHpcrButtonByCoordinate("Start", 0);

            // press the start button
            RecordEvent(DeviceWorkflowMarker.ImagePreviewBegin);
            PressHpcrButtonByCoordinate("", 0);
            WaitForFormString("ImagePreviewForm", ts);
            RecordEvent(DeviceWorkflowMarker.ImagePreviewEnd);

            WaitForEnabled("mStartButton", TimeSpan.FromSeconds(15));
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            _controlPanel.Press("mStartButton");

            _engine.WaitForHtmlContains("Done", TimeSpan.FromSeconds(66));
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
        }

        /// <summary>
        /// Users the device job builder to scan multiple pages.
        /// </summary>
        /// <param name="pageCount">The page count.</param>
        /// <param name="ts">time span</param>
        private void JobBuilderHPCR(int pageCount, TimeSpan ts)
        {
            // attempting to get the OXPD form a bit more time to present
            Thread.Sleep(1500);

            RecordEvent(DeviceWorkflowMarker.JobBuildBegin);

            // pressing the more options button
            PressHpcrButtonByCoordinate("", 1);
            PressPanelButton("Job Build:");
            PressPanelButton("On");

            // press ok
            PressHpcrButtonByCoordinate("Start", 0);

            // press the start button
            UpdateStatus("Scanning page 1 of " + pageCount.ToString());
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            PressHpcrButtonByCoordinate("", 0);
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);

            for (int pc = 2; pc <= pageCount; pc++)
            {
                string status = "Scanning page " + pc.ToString() + " of " + pageCount.ToString();
                UpdateStatus(status);
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                PressOkButton(ts, 1);
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            }

            //Wait for the job build form
            _controlPanel.WaitForForm("JobBuildPrompt", false);
            _controlPanel.Press("mFinishButton");
            _engine.WaitForHtmlContains("Done", ts);
            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
        }

        /// <summary>
        /// Presses the ok button on the scan form.
        /// </summary>
        /// <param name="ts">The ts.</param>
        /// <param name="numTimes">The number times.</param>
        private void PressOkButton(TimeSpan ts, int numTimes)
        {
            try
            {
                _engine.WaitForHtmlContains("m_OKButton", ts);
                _controlPanel.Press("m_OKButton");

                WaitForFormString("BaseJobStartPopup", ts);
            }
            catch (Exception e)
            {
                if (e.Message.Contains("m_OkButton"))
                {
                    if (numTimes < 4)
                    {
                        string status = "Finding the okay button: " + (numTimes + 1).ToString() + " of 4.";
                        UpdateStatus(status);
                        PressOkButton(ts, ++numTimes);
                    }
                    else
                    {
                        throw;
                    }
                }

            }
        }

        /// <summary>
        /// Determines and presses the button for the desired HPCR workflow.
        /// </summary>
        private void ProcessHPCR_Workflows()
        {
            // from john on working with the HPCR and HPCRShare folder
            // Fill in additional values for certain HPCR workflow types
            string buttonTitle = _buttonTitle;
            if (_buttonTitle.StartsWith(HpcrAppTypes.ScanToFolder.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                UpdateStatus("Pressing button that starts with {0} ({1})", buttonTitle, _buttonTitle);
                FolderDistributionButton(_scanDestination);
            }
            else if (_buttonTitle.StartsWith(HpcrAppTypes.PersonalDistributions.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                UpdateStatus("Pressing {0}", _buttonTitle);
                FolderDistributionButton(_scanDistribution);
            }
            else if (_buttonTitle.StartsWith(HpcrAppTypes.PublicDistributions.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                UpdateStatus("Pressing {0}", _buttonTitle);
                FolderDistributionButton(_scanDestination);
                PressPanelButton(_scanDistribution);
            }
        }

        /// <summary>
        /// Sets up to press either the panel or next button for HPCR distribution workflows.
        /// </summary>
        /// <param name="placement">Button Title</param>
        private void FolderDistributionButton(string placement)
        {
            // User has supplied a destination folder
            if (!string.IsNullOrEmpty(placement))
            {
                PressPanelButton(placement);
            }
            else
            {   // next button
                PressFolderButton(0);
            }
        }

        /// <summary>
        /// Presses the panel button.
        /// </summary>
        /// <param name="btnTitle">Title of the button to be pressed</param>
        private void PressPanelButton(string btnTitle)
        {
            if (_engine.WaitForHtmlContains(btnTitle, DefaultScreenWait))
            {
                // find the actual panel button to be pressed			
                List<string> btnTitles = GetBtnTitles(_engine.GetBrowserHtml());
                int buttonIdx = btnTitles.FindIndex(t => t.Equals(btnTitle));

                Coordinate btnCoord = GetButtonCoordinates(btnTitles, buttonIdx);
                int footerPos = GetFooterTop("footerOXPd");

                // the panel actually extends under the footer. In order to press the correct button
                // we need to scroll down so that it is visible.

                Coordinate scrollbar = GetScrollbarCoordinates();

                // if the button y coordinate is below the footer, need to press the scroll down.
                while (btnCoord.Y > footerPos)
                {
                    _controlPanel.PressScreen(scrollbar);
                    btnCoord = GetButtonCoordinates(btnTitles, buttonIdx);
                }

                _controlPanel.PressScreen(btnCoord);
            }
            else
            {
                string msg = "Unknown HPCR folder requested, " + _scanDestination;
                UpdateStatus(msg);
                throw new DeviceWorkflowException(msg);
            }
        }

        /// <summary>
        /// Gets the button coordinates for the given index into the button list.
        /// </summary>
        /// <param name="btnTitles">The BTN titles.</param>
        /// <param name="buttonIdx">Index of the button.</param>
        /// <returns></returns>
        private Coordinate GetButtonCoordinates(List<string> btnTitles, int buttonIdx)
        {
            BoundingBox boundingBox = _engine.GetBoundingAreaByClassIndex("leveledBoxOXPd", 0);

            // middle of the table containing the folder buttons
            int coordX = boundingBox.Width / 2 + boundingBox.Left;
            int folderHgt = boundingBox.Height / btnTitles.Count();
            int coordY = boundingBox.Top + (folderHgt / 2) + (buttonIdx * folderHgt) + _yOffset;

            return new Coordinate(coordX, coordY);
        }

        /// <summary>
        /// Gets the footer top pixel position .
        /// </summary>
        /// <param name="footerName">Name of the footer.</param>
        /// <returns></returns>
        private int GetFooterTop(string footerName)
        {
            BoundingBox boundingBox = _engine.GetBoundingAreaByClassIndex(footerName, 0);
            return boundingBox.Top + _yOffset - _footerOffset;
        }

        /// <summary>
        /// Gets the button titles defined in the HTML
        /// </summary>
        /// <param name="formHtml">The form HTML.</param>
        /// <returns>list of strings</returns>
        private static List<string> GetBtnTitles(string formHtml)
        {
            List<string> btnTitles = new List<string>();
            formHtml.RemoveWhiteSpace();
            int iPos = GetStartIndexPosition(formHtml); // formHtml.IndexOf("FieldSelectionID=");
            while (iPos > 0)
            {
                int iNext = formHtml.IndexOf("</td>", iPos);
                btnTitles.Add(GetBtnTitle(formHtml, iNext));

                iPos = GetStartIndexPosition(formHtml, iNext);// formHtml.IndexOf("FieldSelectionID=", iPos+50);
            }
            return btnTitles;
        }

        /// <summary>
        /// Gets the start index position for the next button title based on the FieldSelectionID.
        /// </summary>
        /// <param name="formHtml">The form HTML.</param>
        /// <param name="startPos">The start position.</param>
        /// <returns>int</returns>
        private static int GetStartIndexPosition(string formHtml, int startPos = 0)
        {
            int iStart = formHtml.IndexOf("FieldSelectionID=", startPos);
            if (iStart > 0 && formHtml.Contains(".png"))
            {
                iStart = formHtml.IndexOf(".png", iStart);
                iStart = formHtml.IndexOf("<td ", iStart);
            }
            return iStart;
        }

        /// <summary>
        /// Retrieves the button title closest to the given index.
        /// </summary>
        /// <param name="formHtml">The form HTML.</param>
        /// <param name="iLast">index just past desired button title</param>
        /// <returns>string</returns>
        private static string GetBtnTitle(string formHtml, int iLast)
        {
            string btnTitle = string.Empty;

            for (int idx = iLast - 1; idx >= 0; idx--)
            {
                if (formHtml[idx].Equals('>'))
                {
                    idx += 1;
                    btnTitle = formHtml.Substring(idx, iLast - idx);
                    break;
                }
            }

            return btnTitle;
        }

        /// <summary>
        /// Presses the folder (next) button.
        /// </summary>
        /// <param name="btnIndex">Index of the BTN.</param>
        private void PressFolderButton(int btnIndex = 0)
        {
            _engine.PressElementByClassIndex("button", btnIndex);
        }

        /// <summary>
        /// Sets the name of the document.
        /// </summary>
        private void SetDocumentName()
        {
            string result = _engine.GetBrowserHtml();
            if (result.Contains("id=\"FieldDeliveredDocumentName\""))
            {
                string objectId = "FieldDeliveredDocumentName";

                PlayKeyboard(objectId, _documentName);
                Thread.Sleep(2000);
            }
        }

        /// <summary>
        /// Presses the HPCR button by its index value based on number of buttons on form.
        /// </summary>
        /// <param name="waitForValue">String value to wait for after button is pressed.</param>
        /// <param name="btnIndex">Index of the BTN.</param>
        private void PressHpcrButtonByCoordinate(string waitForValue, int btnIndex)
        {
            _engine.PressElementByClassIndex("button", btnIndex);

            if (!string.IsNullOrEmpty(waitForValue))
            {
                _engine.WaitForHtmlContains(waitForValue, TimeSpan.FromSeconds(9));
            }
        }

        /// <summary>
        /// Gets the scrollbar coordinates.
        /// </summary>
        /// <returns>Coordinate</returns>
        private Coordinate GetScrollbarCoordinates()
        {
            BoundingBox table = _engine.GetBoundingAreaById("scrollingContent");
            BoundingBox div = _engine.GetBoundingAreaById("scrollingPanel");

            int scrollBarWidth = div.Right - table.Right;
            int scrollBarCenter = scrollBarWidth / 2 + table.Right;

            // Move the y coordinate to about the middle of the down arrow.
            // the button for the arrows have a height around 40 pixels.
            return new Coordinate(scrollBarCenter, div.Bottom + _yOffset - 25);
        }

        /// <summary>
        /// Determines if the device is at the home screen with a status of "Ready"
        /// or waits for the given time span
        /// </summary>
        /// <param name="tsWait">The TS wait.</param>
        private void AtHomeScreen(TimeSpan tsWait)
        {
            Snmp snmpDat = new Snmp(_device.Address);
            Wait.ForTrue(
                () =>
                {
                    var status = snmpDat.Get("1.3.6.1.4.1.11.2.3.9.1.1.3.0").ToLower();
                    return status.Contains("ready") || _controlPanel.CurrentForm() == "HomeScreenForm";
                }
                , tsWait);
            UpdateStatus("AtHomeScreen complete");
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

        private bool VerifyReadyForScanning(bool throwIfNotReady)
        {
            bool result = false;
            if (!_engine.WaitForHtmlContains("Ready for Scanning</title>", TimeSpan.FromSeconds(9)))
            {
                if (throwIfNotReady)
                {
                    throw new DeviceInvalidOperationException("Device is not ready for scanning");
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

        private bool WaitForFormString(string formName, TimeSpan timeToWait)
        {
            // Get the current HTML on form and check for expected form.  If not found, keep checking every 2 seconds until success or time to wait has expired
            bool result = Wait.ForTrue(() => Regex.IsMatch(GetFormControls(), formName, RegexOptions.IgnoreCase), timeToWait, TimeSpan.FromSeconds(2));
            return result;
        }

        /// <summary>
        /// Retrieves the Enabled property of the given control for the given time span.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="timeToWait">The time to wait.</param>
        /// <returns>bool</returns>
        private bool WaitForEnabled(string control, TimeSpan timeToWait)
        {
            bool result = Wait.ForTrue(() => Regex.IsMatch(GetPropertyValue(control, "Enabled"), "True", RegexOptions.IgnoreCase), timeToWait, TimeSpan.FromSeconds(2));
            return result;
        }

        /// <summary>
        /// Gets the property value for the given control if it is among the form's controls.
        /// If not, will return an empty string.
        /// </summary>
        /// <param name="control">The control.</param>
        /// <param name="property">The property.</param>
        /// <returns>string</returns>
        private string GetPropertyValue(string control, string property)
        {
            string result = string.Empty;

            if (GetFormControls().Contains(control))
            {
                result = _controlPanel.GetProperty(control, property);
            }

            return result;
        }

        /// <summary>
        /// Gets the form controls.
        /// </summary>
        /// <returns>string</returns>
        private string GetFormControls()
        {
            string ctlrs = string.Empty;

            List<string> controls = _controlPanel.GetControls().ToList();
            foreach (string control in controls)
            {
                if (!string.IsNullOrEmpty(ctlrs))
                {
                    ctlrs += ", ";
                }
                ctlrs += control;
            }

            return ctlrs;
        }
    }
}
