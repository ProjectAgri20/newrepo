using System;
using System.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.AutoStore
{
    /// <summary>
    /// Omni version of AutoStore
    /// </summary>
    /// <seealso cref="HP.ScalableTest.DeviceAutomation.SolutionApps.AutoStore.AutoStoreAppBase" />
    public class JediOmniAutoStoreApp : AutoStoreAppBase
    {
        private readonly JediOmniDevice _device;
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniMasthead _masthead;
        private readonly OxpdBrowserEngine _engine;
        private readonly TimeSpan _idleTimeoutOffset;

        /// <summary>
        /// Initializes a new instance of the <see cref="JediOmniAutoStoreApp" /> class.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="appButtonTitle">The application button title.</param>
        /// <param name="documentName">Name of the document.</param>
        public JediOmniAutoStoreApp(JediOmniDevice device, string appButtonTitle, string documentName) : base()
        {
            ButtonTitle = appButtonTitle;
            DocumentName = documentName;

            _device = device;
            _controlPanel = device.ControlPanel;

            _masthead = new JediOmniMasthead(_device);
            _engine = new OxpdBrowserEngine(_device.ControlPanel, AutoStoreResource.AutoStoreJavaScript);

            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
            ScreenWidth = _controlPanel.GetScreenSize().Width;
        }
        /// <summary>
        /// Starts the current job and runs it to completion, using the specified <see cref="ScanExecutionOptions" />.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        /// <returns></returns>
        /// <exception cref="DeviceWorkflowException">Unknown AutoStore workflow requested: " + executionOptions.AutoStoreWorkflow</exception>
        public override bool ExecuteJob(AutoStoreExecutionOptions executionOptions)
        {
            bool success = false;

            ExecutionOptions = executionOptions;

            if (success = PressWorkflowButton() == true)
            {
                if (_controlPanel.GetScreenSize().Width > 480)
                {
                    TurnOnOffOptions(executionOptions, TimeSpan.FromSeconds(20));
                }
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
                ProcessWorkflow(executionOptions);
            }

            return success;
        }

        private void FolderFormInput()
        {
            SetTextboxValue("filename", DocumentName);
        }

        private bool ProcessWorkflow(AutoStoreExecutionOptions executionOptions)
        {
            bool success = false;
            if (executionOptions.ImagePreview || executionOptions.JobBuildSegments > 1)
            {
                // image preview one scan
                if (executionOptions.ImagePreview && executionOptions.JobBuildSegments == 1)
                {
                    ImagePreviewOnly();

                }
                else if (executionOptions.ImagePreview && executionOptions.JobBuildSegments > 1)
                {
                    ImagePreviewJobBuild(executionOptions.JobBuildSegments, TimeSpan.FromSeconds(30));
                }
                else //  Job build only
                {
                    JobBuildOnly(executionOptions);
                }
            }
            else if (executionOptions.JobBuildSegments == 1)
            {
                ScanOnly();
            }
            else
            {
                throw new DeviceWorkflowException("Unable to determine AutoStore Workflow.");
            }

            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);

            if (success = _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset) == true)
            {
                RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
            }

            return success;
        }

        private void ScanOnly()
        {
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            _controlPanel.Press("#hpid-button-oxpd-start");
            _masthead.WaitForActiveJobsButtonState(true, TimeSpan.FromSeconds(20));
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
        }

        private void JobBuildOnly(AutoStoreExecutionOptions executionOptions)
        {
            UpdateStatus("Utilizing the job build option.");
            RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
            _controlPanel.Press("#hpid-button-oxpd-start");

            UpdateStatus("Scanning page 1 of " + executionOptions.JobBuildSegments.ToString());
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            // This is sort of a pop up that covers the HPEC and HPCR app
            _controlPanel.WaitForState("#hpid-button-scan", OmniElementState.Useable);
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            for (int pc = 2; pc <= executionOptions.JobBuildSegments; pc++)
            {
                string status = "Scanning page " + pc.ToString() + " of " + executionOptions.JobBuildSegments.ToString();
                UpdateStatus(status);

                if (!_controlPanel.WaitForAvailable("#hpid-button-scan", TimeSpan.FromSeconds(20)))
                {
                    throw new DeviceWorkflowException("The scan button did not become available within 20 seconds.");
                }

                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                _controlPanel.PressWait("#hpid-button-scan", "#hpid-button-done", _idleTimeoutOffset);
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            }
            _controlPanel.PressWait("#hpid-button-done", "#hpid-oxpd-scroll-pane", _idleTimeoutOffset);
            _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);
            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
        }

        private void ImagePreviewOnly()
        {
            _controlPanel.Press("#hpid-button-oxpd-start");

            UpdateStatus("Performing Image Preview.");

            RecordEvent(DeviceWorkflowMarker.ImagePreviewBegin);

            _controlPanel.WaitForAvailable("#hpid-oxp-preview-dialog", TimeSpan.FromSeconds(15));
            _controlPanel.WaitForAvailable("#hpid-button-start", _idleTimeoutOffset);

            RecordEvent(DeviceWorkflowMarker.ImagePreviewEnd);

            _controlPanel.Press("#hpid-button-start");
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            _masthead.WaitForActiveJobsButtonState(true, _idleTimeoutOffset);

            _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
        }

        /// <summary>
        /// Image preview and Job build together
        /// </summary>
        /// <param name="pageCount">The page count.</param>
        /// <param name="ts">The time span.</param>
        private void ImagePreviewJobBuild(int pageCount, TimeSpan ts)
        {
            UpdateStatus("Utilizing the job build option with image preview.");
            RecordEvent(DeviceWorkflowMarker.JobBuildBegin);

            _controlPanel.Press("#hpid-button-oxpd-start");
            UpdateStatus("Performing Image Preview.");

            RecordEvent(DeviceWorkflowMarker.ImagePreviewBegin);
            // press the start button
            UpdateStatus("Scanning page 1 of " + pageCount.ToString());
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            _controlPanel.WaitForAvailable("#hpid-oxp-preview-dialog", TimeSpan.FromSeconds(10));
            _controlPanel.WaitForAvailable("#hpid-prompt-add-pages", _idleTimeoutOffset);
 
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            RecordEvent(DeviceWorkflowMarker.ImagePreviewEnd);

            for (int pc = 2; pc <= pageCount; pc++)
            {
                string status = "Scanning page " + pc.ToString() + " of " + pageCount.ToString();
                UpdateStatus(status);
                RecordEvent(DeviceWorkflowMarker.ImagePreviewBegin);
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

                if (!_controlPanel.WaitForAvailable("#hpid-button-scan", _idleTimeoutOffset))
                {
                    throw new DeviceWorkflowException("The scan button did not become available within 20 seconds.");
                }

                _controlPanel.PressWait("#hpid-button-scan", "#hpid-button-done", TimeSpan.FromSeconds(90));

                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                RecordEvent(DeviceWorkflowMarker.ImagePreviewEnd);
            }

            _controlPanel.PressWait("#hpid-button-done", "#hpid-button-start", TimeSpan.FromSeconds(90));

            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);

            // sometimes get here and the start button is "really" usable. 
            _controlPanel.WaitForAvailable("#hpid-button-start", ts);
            _controlPanel.PressWait("#hpid-button-start", "#hpid-oxpd-scroll-pane", TimeSpan.FromSeconds(90)); //"#hpid-oxpd-scroll-pane"
        }
        private void TurnOnOffOptions(AutoStoreExecutionOptions  executionOptions, TimeSpan ts)
        {
            _engine.PressElementById(MoreOptionsBtnId);
            WaitForButtonExist(MoreOptionsBtnId, ts);

            SetOption(ts, MoreOptionsImagePreview, executionOptions.ImagePreview);
                        
            bool turnOnOption = (executionOptions.JobBuildSegments > 1) == true ? true : false; 
            SetOption(ts, MoreOptionsJobBuild, turnOnOption);

            _engine.PressElementById(HideMoreOptionsBtnId);
        }

        private void SetOption(TimeSpan ts, string optionName, bool turnOnOption)
        {
            NeedToSwipe(optionName);

            string radioButtonOnOffId = OptionYesNoId(optionName, turnOnOption);

            PressElementById(optionName);

            WaitForButtonExist(radioButtonOnOffId, ts);
            PressElementById(radioButtonOnOffId);
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
        /// <summary>
        /// Presses the workflow button as defined in the AutoStore Execution Options.
        /// </summary>
        /// <returns>
        /// bool
        /// </returns>
        /// <exception cref="DeviceWorkflowException">Unable to determine which workflow button to press.</exception>
        protected override bool PressWorkflowButton()
        {
            bool success = false;

            var id = _engine.ExecuteFunction("getButtonIdByTextValue", ExecutionOptions.AutoStoreWorkflow);

            if (id.Length > 0)
            {
                string buttonId = id.Trim('"', ';');
                PressElementById(buttonId);

                success = _masthead.WaitForText(ExecutionOptions.AutoStoreWorkflow, TimeSpan.FromSeconds(15));
            }
            else
            {
                throw new DeviceWorkflowException("Unable to determine which workflow button to press.");
            }

            return success;
        }
        private void EmailFormInput()
        {
            int offSet = OxpdBrowserEngine.GetBrowserOffset(_controlPanel);
            int emailPanelBottom = _engine.GetBottomLocationById("inputForm");//_engine.GetBoundingAreaById("inputForm").Bottom + offSet;

            // The to address is being set by the server. I'm just commenting this out
            // in case we decide to do something different...
            //SetTextboxValue("to", _emailToAddress);

            int screenOffset = 150;
            int bottomOffset = 100;
            if (_controlPanel.GetScreenSize().Width.Equals(480))
            {
                screenOffset = 10;
                bottomOffset = 3;
            }

            int location = _engine.GetBottomLocationById("subject");
            if (location > emailPanelBottom)
            {
                SwipeScreen(offSet + screenOffset, emailPanelBottom - bottomOffset);
            }
            SetTextboxValue("subject", "Scanning via AutoStore " + ExecutionOptions.AutoStoreWorkflow);

            location = _engine.GetBottomLocationById("message"); 
            if (location > emailPanelBottom)
            {
                SwipeScreen(offSet + screenOffset, emailPanelBottom - bottomOffset);
            }
            SetTextboxValue("message", "User " + EmailToAddress + " is utilizing AutoStore Email.");

            location = _engine.GetBottomLocationById("filename"); 
            if (location > emailPanelBottom)
            {
                SwipeScreen(offSet + screenOffset, emailPanelBottom - bottomOffset);
            }
            SetTextboxValue("filename", DocumentName);
        }

        private void SwipeScreen(int offSet, int emailPanelBottom)
        {
            Coordinate topCoordinate = new Coordinate(150, offSet);
            Coordinate bottomCoordinate = new Coordinate(150, emailPanelBottom);

            _controlPanel.SwipeScreen(bottomCoordinate, topCoordinate, TimeSpan.FromMilliseconds(250));
        }

        private void SetTextboxValue(string textboxId, string textValue)
        {
            PressElementById(textboxId);
            if (!_controlPanel.WaitForAvailable("#hpid-keyboard", TimeSpan.FromSeconds(15)))
            {
                throw new DeviceWorkflowException("Keyboard did not show within 15 seconds.");
            }
            _controlPanel.TypeOnVirtualKeyboard(textValue);
            _controlPanel.Press("#hpid-keyboard-key-done");
        }

        /// <summary>
        /// Method used to clean up AutoStore settings after a run. If  options have been turned on they
        /// must be turned off. Options are NOT auto reset after logout. So any option that is set by the
        /// plugin must be reset back to default at end of run.
        /// </summary>
        /// <param name="executionOptions">The execution options.</param>
        public override void JobFinished(AutoStoreExecutionOptions executionOptions)
        {

        }

        /// <summary>
        /// Launches The AutoStore solution with the given authenticator with either eager or lazy authentication.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <param name="authenticationMode">The authentication mode.</param>
        /// <exception cref="DeviceWorkflowException">
        /// Login screen not found.
        /// or
        /// Eager authentication is not allowed by the AutoStore application at this time.
        /// </exception>
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
            AutoStoreReady();
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        private static void Authenticate(IAuthenticator authenticator)
        {
            authenticator.Authenticate();
        }

        private void AutoStoreReady()
        {
            if (!bool.Parse(_engine.ExecuteFunction("existWorkflow", ButtonTitle)))
            {
                _masthead.WaitForActiveJobsButtonState(true, TimeSpan.FromSeconds(3));
                _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);

                bool foundBtn = false;

                Wait.ForTrue(() =>
                {
                    foundBtn = bool.Parse(_engine.ExecuteFunction("existWorkflow", ButtonTitle));
                    return foundBtn;
                }
                , TimeSpan.FromSeconds(10));

                if (!foundBtn)
                {
                    throw new DeviceWorkflowException("AutoStore Authentication Failure. " + ButtonTitle + " not found.");
                }
            }
        }

        private bool PressAutoStoreButton()
        {
            string elementId = ".hp-homescreen-button:contains(" + ButtonTitle + ")";

            UpdateStatus("Pressing the AutoStore workflow button {0}.", ButtonTitle);
            RecordEvent(DeviceWorkflowMarker.AppButtonPress, ButtonTitle);
            _controlPanel.ScrollPress(elementId);

            return _masthead.WaitForText("Log in",TimeSpan.FromSeconds(15));
        }

        private bool NeedToSwipe(string buttonId)
        {            
            bool swipeNeeded = false;

            BoundingBox panel = _engine.GetBoundingAreaById("HPOXPd-options-list-panel");
            BoundingBox button = _engine.GetBoundingAreaById(buttonId);
            if (_controlPanel.GetScreenSize().Width.Equals(480))
            {
                Coordinate pl = new Coordinate(panel.Left, panel.Top);
                Size ps = new Size(panel.Width, 270);
                panel = new BoundingBox(pl, ps);
            }
            if ((panel.Bottom < button.Bottom) || (panel.Top > button.Top))
            {
                swipeNeeded = true;
                if (panel.Bottom < button.Bottom)
                {
                    SwipeOptionPanelUp();
                }
                else
                {
                    SwipeOptionPanelDown();
                }
                swipeNeeded = NeedToSwipe(buttonId);
            }
            return swipeNeeded;
        }

        private void SwipeOptionPanelDown()
        {
            Coordinate start;
            Coordinate end;
            if (_controlPanel.GetScreenSize().Width > 480)
            {
                BoundingBox panel = _engine.GetBoundingAreaById("HPOXPd-options-list-panel");

                int bottomY = panel.Bottom - 50;
                int middle = (panel.Right - panel.Left) / 2;
                int topY = panel.Top + 100;

                end = new Coordinate(middle, bottomY);
                start = new Coordinate(middle, topY);
            }
            else
            {
                end = new Coordinate(240, 265);
                start = new Coordinate(240, 105);
            }
            _controlPanel.SwipeScreen(start, end, TimeSpan.FromMilliseconds(250));
        }
        private void SwipeOptionPanelUp()
        {
            Coordinate start;
            Coordinate end;

            if (_controlPanel.GetScreenSize().Width > 480)
            {
                BoundingBox panel = _engine.GetBoundingAreaById("HPOXPd-options-list-panel");

                int bottomY = panel.Bottom - 50;
                int middle = (panel.Right - panel.Left) / 2;
                int topY = panel.Top + 50;

                start = new Coordinate(middle, bottomY);
                end = new Coordinate(middle, topY);
            }
            else
            {
                start = new Coordinate(240, 265);
                end = new Coordinate(240, 105);
            }
            _controlPanel.SwipeScreen(start, end, TimeSpan.FromMilliseconds(250));
        }

        /// <summary>
        /// Presses the element by identifier.
        /// The code for the OxpdBrowserEngine.PressElementById is slightly different than the one
        /// that uses the zoom multiplier. I'm uncertain as to the behavior across multiple uses for a
        /// zoom factor of 1. Thus I've decided to play it safe and use the original for a zoom of 1.
        /// </summary>
        /// <param name="elementId">The element identifier.</param>
        private void PressElementById(string elementId)
        {
            _engine.PressElementById(elementId);
        }

    }
}
