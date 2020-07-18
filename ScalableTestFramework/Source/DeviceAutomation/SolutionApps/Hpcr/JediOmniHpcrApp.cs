using System;
using System.Collections.Generic;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr
{
    /// <summary>
    /// Omni code for implementing the HPCR solution
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.Hpcr.HpcrAppBase" />
    public class JediOmniHpcrApp : HpcrAppBase
    {
        private readonly int _swipeBottomIndex = 8;
        private readonly int _panelBottom = 485;

        private readonly string _buttonTitle = "Scan To Me";
        private readonly string _scanDestination = string.Empty;
        private readonly string _scanDistribution = string.Empty;
        private readonly string _documentName = string.Empty;
        private readonly bool _imagePreview = false;
        private readonly TimeSpan _idleTimeoutOffset;
        private bool _imagePreviewJobBuild = false;
        private bool _version1_6 = false;

        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniMasthead _masthead;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly OxpdBrowserEngine _engine;


        private readonly int _yOffset = 60;
        private readonly int _screenWidth = 800;
        private const int _footerOffset = 50;     // This is based on a 800 x 600 screen. If the screen is smaller, this value will likely need changing.

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniHpcrApp"/> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="buttonTitle">The button title.</param>
        /// <param name="scanDestination">The scan destination.</param>
        /// <param name="scanDistribution">The scan distribution.</param>
        /// <param name="documentName">Name of the document.</param>
        /// <param name="imagePreview">if set to <c>true</c> [image preview].</param>
        public JediOmniHpcrApp(JediOmniDevice device, string buttonTitle, string scanDestination, string scanDistribution, string documentName, bool imagePreview)
            : base()
        {
            _buttonTitle = buttonTitle;
            _device = device;
            _controlPanel = device.ControlPanel;
            _documentName = documentName;
            _scanDestination = scanDestination;
            _scanDistribution = scanDistribution;
            _imagePreview = imagePreview;            
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));

            _masthead = new JediOmniMasthead(_device);
            _launchHelper = new JediOmniLaunchHelper(device);
            _engine = new OxpdBrowserEngine(_device.ControlPanel, HpcrResource.HpcrJavaScript);

            _screenWidth = _controlPanel.GetScreenSize().Width;

            if (_screenWidth.Equals(1024))
            {
                _yOffset = 75;
                _panelBottom = 640;
            }
            else if (_screenWidth.Equals(480))
            {
                _yOffset = 40;
                _swipeBottomIndex = 3;
                _panelBottom = 215;
            }
        }

        /// <summary>
        /// Launches the specified authenticator.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode)
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
                PressHPCR_WorkflowButton(authenticationMode);
            }
            else
            {
                PressHPCR_WorkflowButton(authenticationMode);
                Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen);
            }

            HpcrAppReady();

            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        private void HpcrAppReady()
        {
            if (_controlPanel.CheckState(_masthead.MastheadSpinner, OmniElementState.Exists))
            {
                if (Wait.ForTrue(() => _masthead.BusyStateActive(), TimeSpan.FromSeconds(1)))
                {
                    _masthead.WaitForBusyState(false, _idleTimeoutOffset);
                }
            }
        }

        /// <summary>
        /// Executes the HPCR job.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public override bool ExecuteJob(ScanExecutionOptions executionOptions)
        {
            bool done = false;
            ProcessHpcrWorkflows();
             
            if (IsDocumentFieldName())
            {
                SetDocumentName();
            }
            VerifyReadyForScanning(true);

            ProcessHPCR_Work(executionOptions.JobBuildSegments, TimeSpan.FromSeconds(3));

            if (!_imagePreviewJobBuild)
            {
                RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
            }
            done = _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
            return done;
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
        /// Determines and presses the button for the desired HPCR workflow.
        /// </summary>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="HP.DeviceAutomation.Jedi.ControlNotFoundException">Unable to press control,  + _buttonTitle + , at this time.</exception>
        private void PressHPCR_WorkflowButton(AuthenticationMode authenticationMode)
        {                      
            //string elementId = ".hp-homescreen-button:contains(" + _buttonTitle + ")";
            string successScreen = (authenticationMode.Equals(AuthenticationMode.Eager) == true) ? JediOmniLaunchHelper.LazySuccessScreen : JediOmniLaunchHelper.SignInForm;

            UpdateStatus("Pressing the HPCR workflow button {0}.", _buttonTitle);

            _launchHelper.PressSolutionButton(_buttonTitle, "HPCR " + _buttonTitle, successScreen);
            //RecordEvent(DeviceWorkflowMarker.AppButtonPress, "HPCR " + _buttonTitle);
            //_controlPanel.ScrollPressWait(elementId, successScreen);
        }

        /// <summary>
        /// The various workflows have different paths. Setup for processing different HPCR workflows.
        /// </summary>
        private void ProcessHpcrWorkflows()
        {
            _version1_6 = bool.Parse(_engine.ExecuteFunction("ExistButtonId", "btnIdExit"));
            UpdateStatus("Pressing {0}", _buttonTitle);

            if (_version1_6)
            {
                ProcessHpcrV6Workflows();
            }
            else
            {
                if (_buttonTitle.StartsWith(HpcrAppTypes.ScanToFolder.GetDescription(), StringComparison.OrdinalIgnoreCase))
                {
                    FolderDistributionButton(_scanDestination);
                }
                else if (_buttonTitle.StartsWith(HpcrAppTypes.PersonalDistributions.GetDescription(), StringComparison.OrdinalIgnoreCase))
                {
                    FolderDistributionButton(_scanDistribution);
                }
                else if (_buttonTitle.StartsWith(HpcrAppTypes.PublicDistributions.GetDescription(), StringComparison.OrdinalIgnoreCase))
                {
                    FolderDistributionButton(_scanDestination);
                    PressPanelButton(_scanDistribution);
                }
            }
        }

        private void ProcessHpcrV6Workflows()
        {
            if (_buttonTitle.StartsWith(HpcrAppTypes.ScanToFolder.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                FolderSelectionV6(_scanDestination);
            }
            else if (_buttonTitle.StartsWith(HpcrAppTypes.PersonalDistributions.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                FolderSelectionV6(_scanDistribution);
            }
            else if (_buttonTitle.StartsWith(HpcrAppTypes.PublicDistributions.GetDescription(), StringComparison.OrdinalIgnoreCase))
            {
                FolderSelectionV6(_scanDestination);
                FolderSelectionV6(_scanDistribution);
            }
        }

        private void FolderSelectionV6(string buttonTitle)
        {
            string[] titles = GetPanelIds();
            PressOptionButton(titles, buttonTitle);
        }

        /// <summary>
        /// Processes the HPCR workflow.
        /// </summary>
        /// <param name="pageCount">The page count.</param>
        /// <param name="ts">time span</param>
        private void ProcessHPCR_Work(int pageCount, TimeSpan ts)
        {            
            // pressing the start button and waiting for the done button
            if (_imagePreview || pageCount > 1)
            {
                TurnOnOptions(pageCount);

                // simple image preview with one scan
                if (_imagePreview && pageCount == 1)
                {
                    ImagePreviewHpcr();
                }
                else if (_imagePreview && pageCount > 1) // image preview with job build
                {
                    ImagePreviewJobBuild(pageCount, ts);
                }
                else // job build only
                {
                    JobBuilderHPCR(pageCount);
                }
            }
            else if (pageCount == 1)
            {
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                PressStartButton();
                _masthead.WaitForText("HP Capture & Route - Scan To Me: Scan Completed", TimeSpan.FromSeconds(3));
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            }
            else
            {
                throw new DeviceWorkflowException("Unable to determine HPCR workflow.");

            }
        }

        /// <summary>
        /// Turns the on options for either image preview or job build.
        /// </summary>
        /// <param name="pageCount">The page count.</param>
        private void TurnOnOptions(int pageCount)
        {
            if (_version1_6)
            {                
                _engine.PressElementById("btnIdScanSettings");

                if (_imagePreview || pageCount > 1)
                {
                    string[] onOff = null;

                    var values = _engine.ExecuteFunction("getOptionIds");
                    string[] ids = values.Replace("undefined", "").Trim().Split(';');
                    if (_imagePreview)
                    {
                        PressOptionButton(ids, "Image Preview");
                        onOff = GetPanelIds();

                        PressOptionButton(onOff, "On");
                    }

                    if (pageCount > 1)
                    {
                        PressOptionButton(ids, "Job Build");

                        if (onOff == null)
                        {
                            onOff = GetPanelIds();
                        }
                        PressOptionButton(onOff, "On");
                    }
                }
                _engine.PressElementById("btnIdOk");
            }
            else
            {
                Options1_5(pageCount);
            }
            VerifyReadyForScanning(true);
        }

        private string[] GetPanelIds()
        {
            var values = _engine.ExecuteFunction("getOptionIds");
            string[] onOff = values.Replace("undefined", "").Trim().Split(';');

            return onOff;
        }

        private void PressOptionButton(string[] ids, string buttonTitle)
        {
            int buttonIdx = 0;

            foreach (string id in ids)
            {
                string buttonId = string.Empty;
                if (id.Contains(buttonTitle))
                {
                    buttonId = GetButtonId(id);
                    PressElementById(ids, buttonId, buttonIdx);
                    return;
                }
                buttonIdx++;
            }

            // If we end up here, the button title was not found in the list of Ids
            string msg = $"HPCR button '{buttonTitle}' was not found.";
            UpdateStatus(msg);
            throw new DeviceWorkflowException(msg);
        }

        private static string GetButtonId(string id)
        {
            string buttonId;
            string[] str = id.Split('-');
            if (str.Length == 2)
            {
                //Pre-1.6.1 button Ids
                buttonId = str[0].Trim('"');
            }
            else
            {
                //Version 1.6.1 and after
                buttonId = (str[0] + "-" + str[1]).Trim('"');
            }

            return buttonId;
        }

        private void PressElementById(string[] ids, string buttonId, int buttonIdx)
        {
            Coordinate coordinate = GetButtonCoordinates(buttonId, ids.Count(), buttonIdx);
            if(coordinate.Y > _panelBottom)
            {
                coordinate = SwipeSmallScreen(ids, buttonId, buttonIdx, coordinate);
            }

            _engine.PressElementById(buttonId);
        }

        private Coordinate SwipeSmallScreen(string[] ids, string buttonId, int buttonIdx, Coordinate coordinate)
        {
            Coordinate swipeStart = GetButtonCoordinates(GetButtonId(ids[3]), ids.Count(), 3);
            Coordinate swipeEnd = GetButtonCoordinates(GetButtonId(ids[0]), ids.Count(), 0);

            while (coordinate.Y > _panelBottom)
            {
                _controlPanel.SwipeScreen(swipeStart, swipeEnd, TimeSpan.FromMilliseconds(250));
                coordinate = GetButtonCoordinates(buttonId, ids.Count(), buttonIdx);

                // To turn on image preview option in High resolution devices 
                if (_screenWidth.Equals(1024) && coordinate.Y < _panelBottom)
                {
                    _controlPanel.SwipeScreen(swipeStart, swipeEnd, TimeSpan.FromMilliseconds(250));
                }
            }

            return coordinate;
        }

        private Coordinate GetButtonCoordinates(string buttonId, int cntBtnTitles, int buttonIdx)
        {
            BoundingBox boundingBox = _engine.GetBoundingAreaById(buttonId);

            // middle of the table containing the folder buttons
            int coordX = boundingBox.Width / 2 + boundingBox.Left;
            int folderHgt = boundingBox.Height / cntBtnTitles;
            int coordY = boundingBox.Top + (folderHgt / 2) + (buttonIdx * folderHgt) + _yOffset;

            return new Coordinate(coordX, coordY);
        }

        private void Options1_5(int pageCount)
        {
            // pressing more options
            PressHpcrButtonByCoordinate(1);

            if (_imagePreview)
            {
                TurnOnImagePreview();
            }

            if (pageCount > 1)
            {
                PressPanelButton("Job Build:");
                PressPanelButton("On");
            }

            // press ok
            PressHpcrButtonByCoordinate(0);           
        }

        /// <summary>
        /// Turns the on image preview.
        /// </summary>
        private void TurnOnImagePreview()
        {
            PressPanelButton("Image Preview:");
            PressPanelButton("On");
        }

        /// <summary>
        /// Sets up to press either the panel or next button for HPCR distribution workflows.
        /// </summary>
        /// <param name="destination"></param>
        private void FolderDistributionButton(string destination)
        {
            // User has supplied a destination folder
            if (!string.IsNullOrEmpty(destination))
            {
                PressPanelButton(destination);
            }
            else
            {   // next button => was supposed to press the next button but that was commented out and only getting the bounding area ????
                throw new DeviceWorkflowException("JediOmniHpcrApp.FolderDistributionButton - Unable to determine workflow destination.");
            }
        }

        /// <summary>
        /// Process for implementing the Jedi Image Preview workflow.
        /// </summary>
        private void ImagePreviewHpcr()
        {
            // Press the start button
            PressStartButton();
            UpdateStatus("Performing Image Preview.");

            RecordEvent(DeviceWorkflowMarker.ImagePreviewBegin);

            _controlPanel.WaitForAvailable("#hpid-oxp-preview-dialog", TimeSpan.FromSeconds(10));
            _controlPanel.WaitForAvailable("#hpid-button-start", TimeSpan.FromSeconds(33));

            RecordEvent(DeviceWorkflowMarker.ImagePreviewEnd);

            PressOmniScanButton();
        }

        private void PressStartButton()
        {
            if (_version1_6)
            {
                _engine.PressElementById("btnIdSelect");
            }
            else
            {
                PressHpcrButtonByCoordinate(0);
            }
        }

        /// <summary>
        /// Uses the device job builder to scan multiple pages.
        /// </summary>
        /// <param name="pageCount">The page count.</param>
        private void JobBuilderHPCR(int pageCount)
        {            
            PressStartButton();

            UpdateStatus("Utilizing the job build option.");
            RecordEvent(DeviceWorkflowMarker.JobBuildBegin);

            // press the start button
            UpdateStatus("Scanning page 1 of " + pageCount.ToString());
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            // This is sort of a pop up that covers the HPEC and HPCR app
            _controlPanel.WaitForState("#hpid-button-scan", OmniElementState.Useable);
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            for (int pc = 2; pc <= pageCount; pc++)
            {
                string status = "Scanning page " + pc.ToString() + " of " + pageCount.ToString();
                UpdateStatus(status);
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                _controlPanel.PressWait("#hpid-button-scan", "#hpid-button-done", TimeSpan.FromSeconds(90));
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            }
            _controlPanel.PressWait("#hpid-button-done", "#hpid-oxpd-scroll-pane", TimeSpan.FromSeconds(90));

            // scan complete will show but not really done so next make sure no active jobs are processing.
            _masthead.WaitForText("HP Capture & Route - Scan To Me: Scan Completed", TimeSpan.FromSeconds(3));
            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
        }
        /// <summary>
        /// Image preview and Job build together
        /// </summary>
        /// <param name="pageCount">The page count.</param>
        /// <param name="ts">The time span.</param>
        private void ImagePreviewJobBuild(int pageCount, TimeSpan ts)
        {
            _imagePreviewJobBuild = true;
            UpdateStatus("Utilizing the job build option with image preview.");
            RecordEvent(DeviceWorkflowMarker.JobBuildBegin);

            PressStartButton();
            UpdateStatus("Performing Image Preview.");

            RecordEvent(DeviceWorkflowMarker.ImagePreviewBegin);
            // press the start button
            UpdateStatus("Scanning page 1 of " + pageCount.ToString());
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            _controlPanel.WaitForAvailable("#hpid-oxp-preview-dialog", TimeSpan.FromSeconds(10));
            _controlPanel.WaitForAvailable("#hpid-prompt-add-pages", TimeSpan.FromSeconds(33));

            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            RecordEvent(DeviceWorkflowMarker.ImagePreviewEnd);

            for (int pc = 2; pc <= pageCount; pc++)
            {
                string status = "Scanning page " + pc.ToString() + " of " + pageCount.ToString();
                UpdateStatus(status);
                RecordEvent(DeviceWorkflowMarker.ImagePreviewBegin);
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

                _controlPanel.PressWait("#hpid-button-scan", "#hpid-button-done", TimeSpan.FromSeconds(90));

                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                RecordEvent(DeviceWorkflowMarker.ImagePreviewEnd);
            }
            
            _controlPanel.PressWait("#hpid-button-done", "#hpid-button-start", TimeSpan.FromSeconds(90));

            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);

            // sometimes get here and the start button is "really" useable. 
            _controlPanel.WaitForAvailable("#hpid-button-start", ts);
            _controlPanel.PressWait("#hpid-button-start", "#hpid-oxpd-scroll-pane", TimeSpan.FromSeconds(90)); //"#hpid-oxpd-scroll-pane"

            // scan complete will show but not really done so next make sure no active jobs are processing.
            _masthead.WaitForText("HP Capture & Route - Scan To Me: Scan Completed", TimeSpan.FromSeconds(3));

        }

        /// <summary>
        /// Writes the given text into the given control ID.
        /// </summary>
        /// <param name="textToType">int</param>
        private void PlayKeyboard(string textToType)
        {
            if (_controlPanel.WaitForAvailable("#hpid-keyboard-key-enter"))
            {
                _controlPanel.TypeOnVirtualKeyboard(textToType);
                _controlPanel.Press("#hpid-keyboard-key-done");

                try
                {
                    if (_version1_6)
                    {
                        _engine.PressElementById("btnIdSelect");
                    }
                    else if (_controlPanel.WaitForAvailable("#hpid-keyboard-key-enter"))
                    {
                        _controlPanel.Press("#hpid-keyboard-key-enter");
                    }
                }
                catch
                {
                    throw new ControlNotFoundException("The keyboard is not ready.");
                }
            }
            else
            {
                throw new ControlNotFoundException("The keyboard is not ready.");
            }
        }

        /// <summary>
        /// Presses the HPCR button by its index value based on number of buttons on form.
        /// </summary>
        /// <param name="btnIndex">Index of the BTN.</param>
        /// <param name="trys">Recursive parameter used to keep track of retries before throwing</param>
        private void PressHpcrButtonByCoordinate(int btnIndex, int trys = 0)
        {
            try
            {
                _engine.PressElementByClassIndex("button", btnIndex);
            }
            catch (Exception ex)
            {
                UpdateStatus(ex.Message);
                if (ex.Message.Contains("UIContextId") && trys < 3)
                {
                    PressHpcrButtonByCoordinate(btnIndex, ++trys);
                }
                else throw;
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
                int buttonIdx = btnTitles.FindIndex(t => t.ToLower().Equals(btnTitle.ToLower()));

                int footerPos = GetFooterTop("footerOXPd");
                Coordinate btnCoord = GetButtonCoordinates(btnTitles.Count(), buttonIdx);

                if (footerPos > _panelBottom)
                {
                    btnCoord = SwipeForHpcr(btnTitles, buttonIdx, footerPos, btnCoord);
                }
                else // footer position is about 256 so on a smaller screen
                {
                    btnCoord = PressScrollBarButton(btnTitles, buttonIdx, footerPos, btnCoord);
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

        private Coordinate PressScrollBarButton(List<string> btnTitles, int buttonIdx, int footerPos, Coordinate btnCoord)
        {
            Coordinate sb = GetScrollbarCoordinates();
            footerPos -= _footerOffset;

            while (btnCoord.Y > footerPos)
            {
                _controlPanel.PressScreen(sb);
                btnCoord = GetButtonCoordinates(btnTitles.Count(), buttonIdx);
            }
            return btnCoord;
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
        private Coordinate SwipeForHpcr(List<string> btnTitles, int buttonIdx, int footerPos, Coordinate btnCoord)
        {
            if (buttonIdx > _swipeBottomIndex)
            {
                SwipeButtonPanel(btnTitles);
            }

            // if the button y coordinate is below the footer, need to press the scroll down.
            // will probably need to be converted to swipe once that is working
            while (btnCoord.Y > footerPos)
            {
                SwipeButtonPanel(btnTitles);
                btnCoord = GetButtonCoordinates(btnTitles.Count(), buttonIdx);
            }

            return btnCoord;
        }

        /// <summary>
        /// Presses the Omni start scan button.
        /// </summary>
        private void PressOmniScanButton()
        {
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            _controlPanel.Press("#hpid-button-start");
            _masthead.WaitForText("HP Capture & Route - Scan To Me: Scan Completed", TimeSpan.FromSeconds(3));
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
        }

        /// <summary>
        /// Sets the name of the document.
        /// </summary>
        private void SetDocumentName()
        {
            _engine.PressElementById("FieldDeliveredDocumentName");
            PlayKeyboard(_documentName);
        }

        private void SwipeButtonPanel(List<string> btnTitles)
        {
            Coordinate startC = GetButtonCoordinates(btnTitles.Count(), _swipeBottomIndex);
            Coordinate endC = GetButtonCoordinates(btnTitles.Count(), 0);

            _controlPanel.SwipeScreen(startC, endC, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Gets the button coordinates for the given index into the button list.
        /// </summary>
        /// <param name="cntBtnTitles">The BTN titles.</param>
        /// <param name="buttonIdx">Index of the button.</param>
        /// <returns></returns>
        private Coordinate GetButtonCoordinates(int cntBtnTitles, int buttonIdx)
        {
            BoundingBox boundingBox = _engine.GetBoundingAreaByClassIndex("leveledBoxOXPd", 0);

            // middle of the table containing the folder buttons
            int coordX = boundingBox.Width / 2 + boundingBox.Left;
            int folderHgt = boundingBox.Height / cntBtnTitles;
            int coordY = boundingBox.Top + (folderHgt / 2) + (buttonIdx * folderHgt) + _yOffset;

            return new Coordinate(coordX, coordY);
        }

        /// <summary>
        /// Gets the footer top pixel position.
        /// </summary>
        /// <param name="footerName">Name of the footer.</param>
        /// <returns>int</returns>
        private int GetFooterTop(string footerName)
        {
            BoundingBox boundingBox = _engine.GetBoundingAreaByClassIndex(footerName, 0);
            return boundingBox.Top + _yOffset + _footerOffset;
        }

        /// <summary>
        /// Method used to determine or run the end results of the job
        /// </summary>
        public override void JobFinished()
        {
            // press the done button based on its index
            if (_version1_6)
            {
                _engine.PressElementById("btnIdExit"); 
            }
            else
            {
                PressHpcrButtonByCoordinate(1);
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
        private bool VerifyReadyForScanning(bool throwIfNotReady)
        {
            bool result = false;
            DateTime dtStart = DateTime.Now;

            if (!_controlPanel.WaitForState(".hp-masthead-title:contains(Ready for Scanning)", OmniElementState.Exists, TimeSpan.FromSeconds(6)))
            {
                if (throwIfNotReady)
                {
                    DateTime dtEnd = DateTime.Now;
                    TimeSpan ts = dtEnd.Subtract(dtStart);

                    throw new DeviceInvalidOperationException("Device is not ready for scanning: " + ts.Seconds.ToString() + " seconds.");
                }
            }
            else
            {
                result = true;
            }
            return result;
        }

    }
}
