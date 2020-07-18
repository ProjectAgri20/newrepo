using System;
using System.Collections.Generic;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.AutoStore
{
    class JediWindjammerAutoStoreApp : AutoStoreAppBase
    {
        private readonly JediWindjammerDevice _device;
        private readonly JediWindjammerControlPanel _controlPanel;

        private readonly OxpdBrowserEngine _engine;

        private int _yOffset;
        private const int _footerOffset = 50;     // This is based on a 800 x 600 screen. If the screen is smaller, this value will likely need changing.
        private int _footerPos;

        private List<AutoStoreButtons> _autoStoreButtons = new List<AutoStoreButtons>();

        /// <summary>
        /// Initializes a new instance of the <see cref="JediWindjammerAutoStoreApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="appButtonTitle">The application button title.</param>
        /// <param name="documentName">Name of the document.</param>
        public JediWindjammerAutoStoreApp(JediWindjammerDevice device, string appButtonTitle, string documentName) : base()
        {
            ButtonTitle = appButtonTitle;
            DocumentName = documentName;

            _device = device;
            _controlPanel = device.ControlPanel;

            _engine = new OxpdBrowserEngine(_device.ControlPanel, AutoStoreResource.AutoStoreJavaScript);

            MoreOptionsBtnId = "HPOXPDMOREOPTIONS";         
            MoreOptionsDownArrowBtnId = "HPOXPDMOREOPTIONSPAGEDOWN"; 
            MoreOptionsUpArrowBtnId = "HPOXPDMOREOPTIONSPAGEUP"; 

            MoreOptionsImagePreview = "PreviewModeOption-button";
            PreviewModeOffId = "HPOXPDDROPDOWNID#1OXPDID0";
            PreviewModeOnId = "HPOXPDDROPDOWNID#1OXPDID1";

            MoreOptionsJobBuild = "JobAssemblyModeOption-button";
            JobBuildModeOffId = "HPOXPDDROPDOWNID#1OXPDID1";
            JobBuildModeOnId = "HPOXPDDROPDOWNID#1OXPDID0";
        }
        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        /// <returns></returns>
        /// <exception cref="DeviceWorkflowException">
        /// Unknown AutoStore workflow requested: " + executionOptions.AutoStoreWorkflow
        /// or
        /// Unable to press the AutoStore workflow requested: " + executionOptions.AutoStoreWorkflow
        /// </exception>
        public override bool ExecuteJob(AutoStoreExecutionOptions executionOptions)
        {
            bool success = false;
            
            ExecutionOptions = executionOptions;
            _yOffset = OxpdBrowserEngine.GetBrowserOffset(_controlPanel);
            _footerPos = GetFooterTop("footerOXPd");

            GetButtonIdsTitles();

            if (success = PressWorkflowButton() == true)
            {
                TurnOnOffOptions(executionOptions, TimeSpan.FromSeconds(20));

                if (executionOptions.AutoStoreWorkflow.Contains("Email"))
                {
                    EmailFormInput();
                }
                else if (executionOptions.AutoStoreWorkflow.Contains("Folder"))
                {
                    FolderFormInput();
                }
                else
                {
                    throw new DeviceWorkflowException("Unknown AutoStore workflow requested: " + executionOptions.AutoStoreWorkflow);
                }
                ProcessWorkflow();
            }
            else
            {
                throw new DeviceWorkflowException("Unable to press the AutoStore workflow requested: " + executionOptions.AutoStoreWorkflow);
            }

            return success;
        }

        private bool ProcessWorkflow()
        {
            bool success = false;

            ScanExecutionOptions seo = new ScanExecutionOptions();
            string appWaitForm = "OxpUIAppMainForm800X600";  // "EmailForm";
            if (ExecutionOptions.ImagePreview)
            {
                seo.ImagePreview = ImagePreviewOption.GeneratePreview;
                appWaitForm = "ImagePreviewForm";
            }
            seo.JobBuildSegments = ExecutionOptions.JobBuildSegments;

            JediWindjammerJobExecutionManager jwMgr = new JediWindjammerJobExecutionManager(_device);
            jwMgr.WorkflowLogger = WorkflowLogger;

            success = jwMgr.ExecuteScanJob(seo, appWaitForm); //OxpUIAppMainForm800X600"
            string curForm = _controlPanel.CurrentForm();

            if (curForm.Equals("ImagePreviewForm"))
            {
                success = ProcessScanJob();
            }
            return success;
        }

        private void TurnOnOffOptions(AutoStoreExecutionOptions executionOptions, TimeSpan ts)
        {
            WaitForButtonExist(MoreOptionsBtnId, ts);
            _engine.PressElementById(MoreOptionsBtnId);

            WaitForButtonExist(MoreOptionsDownArrowBtnId, ts);

            SetOption(ts, MoreOptionsImagePreview, executionOptions.ImagePreview);

            bool turnOnOption = (executionOptions.JobBuildSegments > 1) == true ? true : false;
            SetOption(ts, MoreOptionsJobBuild, turnOnOption);

            WaitForButtonExist(MoreOptionsUpArrowBtnId, ts);
            _engine.PressElementById(MoreOptionsUpArrowBtnId);
        }
        private void WaitForButtonExist(string buttonId, TimeSpan ts)
        {
            bool found = Wait.ForTrue(() => ExistAutoStoreId(buttonId), ts);

            if (!found)
            {
                throw new DeviceWorkflowException("unable to find and press AutoStore button ID " + buttonId + " within " + ts.Seconds.ToString() + " seconds.");
            }

        }
        private bool ExistAutoStoreId(string id)
        {
            bool exist = false;
            string existId = _engine.ExecuteFunction("ExistButtonId", id);

            exist = (existId.Equals("true")) ? true : false;

            return exist;
        }
        private void SetOption(TimeSpan ts, string optionName, bool turnOnOption)
        {
            bool pressPageDown = false;
            if (!_engine.HtmlContains(optionName))
            {
               _engine.PressElementById(MoreOptionsDownArrowBtnId);
                pressPageDown = true;
            }
            
            string radioButtonOnOffId = OptionYesNoId(optionName, turnOnOption);
            _engine.PressElementById(optionName);

            WaitForButtonExist(radioButtonOnOffId, ts);
            _engine.PressElementById(radioButtonOnOffId);

            _engine.PressElementById(MoreOptionsOkId);

            if (pressPageDown)
            {
                _engine.PressElementById(MoreOptionsUpArrowBtnId);
            }
        }
        private string OptionYesNoId(string optionName, bool turnOnOption)
        {
            string id = string.Empty;

            if (optionName.Equals(MoreOptionsJobBuild))
            {
                if (turnOnOption)
                {
                    id = JobBuildModeOnId;
                }
                else
                {
                    id = JobBuildModeOffId;
                }
            }
            else if (optionName.Equals(MoreOptionsImagePreview))
            {
                if (turnOnOption)
                {
                    id = PreviewModeOnId;
                }
                else
                {
                    id = PreviewModeOffId;
                }
            }
            return id;
        }

        private void GetButtonIdsTitles()
        {
            string html = _engine.GetBrowserHtml();
            int pos = html.IndexOf("<td id=");
            while (pos > 1)
            {
                AutoStoreButtons autoStoreButtons = new AutoStoreButtons();
                autoStoreButtons.ButtonId = html.Substring(pos + 8, 36);

                pos = html.IndexOf("name=", pos + 43) + 6;
                int end = html.IndexOf('"', pos);
                autoStoreButtons.ButtonTitle = html.Substring(pos, end - pos);

                _autoStoreButtons.Add(autoStoreButtons);
                pos = html.IndexOf("<td id=", pos);
            }
        }

        public override void JobFinished(AutoStoreExecutionOptions executionOptions)
        {

        }

        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
        {
            if (authenticationMode.Equals(AuthenticationMode.Lazy))
            {
                EmailToAddress = authenticator.Credential.UserName + "@" + authenticator.Credential.Domain;
                if (PressAutoStoreButton())
                {
                    authenticator.LazyAuthOnly = true;
                    authenticator.Authenticate();
                }
                else
                {
                    throw new DeviceWorkflowException("Login screen not found within 15 seconds.");
                }

            }
            else // AuthenticationMode.Eager
            {
                throw new DeviceWorkflowException("Eager authentication is not allowed by the AutoStore application at this time.");
            }

            if (!_engine.WaitForHtmlContains(ButtonTitle, TimeSpan.FromSeconds(25)))
            {
                throw new DeviceWorkflowException("AutoStore application did not show within 25 seconds.");
            }
            
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        private bool PressAutoStoreButton()
        {
            RecordEvent(DeviceWorkflowMarker.AppButtonPress, ButtonTitle);
            _controlPanel.ScrollPress("mAccessPointDisplay", "Title", ButtonTitle);

            bool success =_engine.WaitForHtmlContains("Log in", TimeSpan.FromSeconds(15));

            return success;
        }

        /// <summary>
        /// Presses the workflow button as defined in the AutoStore Execution Options.
        /// </summary>
        /// <returns>
        /// bool
        /// </returns>
        protected override bool PressWorkflowButton()
        {
            bool success = false;

            string buttonId = GetButtonId();
            if (!string.IsNullOrEmpty(buttonId))
            {
                BoundingBox bb = _engine.GetBoundingAreaById(buttonId);
                _engine.PressElementByBoundingArea(bb);

                success = true;
            }
            return success;
        }

        private void FolderFormInput()
        {
            SetTextBoxData("filename", DocumentName);
        }
        private void EmailFormInput()
        {            
            Coordinate scrollDownCoordinate = GetScrollbarCoordinates();

            // inserting the subject
            Coordinate txtBoxCoordinate = GetElementIdCoordinates("subject");
            if (txtBoxCoordinate.Y > _footerPos)
            {
                PressScrollDown("subject", scrollDownCoordinate, _footerPos, scrollDownCoordinate);
            }
            SetTextBoxData("subject", "Scanning via AutoStore " + ExecutionOptions.AutoStoreWorkflow);


            // inserting the message
            txtBoxCoordinate = GetElementIdCoordinates("message");
            if (txtBoxCoordinate.Y > _footerPos)
            {
                PressScrollDown("message", scrollDownCoordinate, _footerPos, scrollDownCoordinate);
            }
            SetTextBoxData("message", "User " + EmailToAddress + " is utilizing AutoStore Email.");

            // inserting the filename
            txtBoxCoordinate = GetElementIdCoordinates("filename");
            if (txtBoxCoordinate.Y > _footerPos)
            {
                PressScrollDown("filename", scrollDownCoordinate, _footerPos, scrollDownCoordinate);
            }
            SetTextBoxData("filename", DocumentName);

        }
        private bool ProcessScanJob()
        {
            bool success = false;
            RecordEvent(DeviceWorkflowMarker.SendingJobBegin);

            _controlPanel.WaitForPropertyValue("mStartButton", "Enabled", true);
            
            _controlPanel.PressToNavigate("mStartButton", "BaseJobStartPopup", ignorePopups: false);


            success = _engine.WaitForHtmlContains("Job completed", TimeSpan.FromSeconds(60));
            if (success)
            {
                RecordEvent(DeviceWorkflowMarker.SendingJobEnd);
            }
            return success;
        }
        private void PressScrollDown(string elementId, Coordinate elementcoordinate, int footer_Y_Location, Coordinate scrollDownCoordinate)
        {            
            while (elementcoordinate.Y > footer_Y_Location)
            {
                _controlPanel.PressScreen(scrollDownCoordinate.X, scrollDownCoordinate.Y);
                elementcoordinate = GetElementIdCoordinates(elementId);
            }
        }

        private void SetTextBoxData(string textboxId, string data)
        {
            _engine.PressElementById(textboxId);
            _controlPanel.TypeOnVirtualKeyboard("mKeyboard", data);
            _controlPanel.Press("ok");
        }

        /// <summary>
        /// Gets the Element coordinates for the given element ID.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        /// <returns></returns>
        private Coordinate GetElementIdCoordinates(string elementId)
        {
            BoundingBox bbBtn = _engine.GetBoundingAreaById(elementId);

            int coordX = bbBtn.Width / 2 + bbBtn.Left;
            int coordY = bbBtn.Top + bbBtn.Height / 2 + _yOffset;

            return new Coordinate(coordX, coordY);
        }

        /// <summary>
        /// Gets the scrollbar coordinates.
        /// </summary>
        /// <returns>Coordinate</returns>
        private Coordinate GetScrollbarCoordinates()
        {
            BoundingBox table = _engine.GetBoundingAreaById("asScrollingContent");
            BoundingBox div = _engine.GetBoundingAreaById("asScrollingPanel");

            int scrollBarWidth = div.Right - table.Right;
            int scrollBarCenter = scrollBarWidth / 2 + table.Right;

            // Move the y coordinate to about the middle of the down arrow.
            // the button for the arrows have a height around 40 pixels.
            return new Coordinate(scrollBarCenter, div.Bottom + _yOffset - 25);
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
        private string GetButtonId()
        {
            string buttonId = string.Empty;

            foreach (AutoStoreButtons auto in _autoStoreButtons)
            {
                if (auto.ButtonTitle.Equals(ExecutionOptions.AutoStoreWorkflow))
                {
                    buttonId = auto.ButtonId;
                    break;
                }
            }
                return buttonId;
        }
        private class AutoStoreButtons
        {
            /// <summary>
            /// Gets or sets the button identifier.
            /// </summary>
            /// <value>
            /// The button identifier.
            /// </value>
            public string ButtonId { get; set; }
            /// <summary>
            /// Gets or sets the button title.
            /// </summary>
            /// <value>
            /// The button title.
            /// </value>
            public string ButtonTitle { get; set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="AutoStoreButtons"/> class.
            /// </summary>
            public AutoStoreButtons()
            {
                ButtonId = string.Empty;
                ButtonTitle = string.Empty;
            }
        }
    }
}
