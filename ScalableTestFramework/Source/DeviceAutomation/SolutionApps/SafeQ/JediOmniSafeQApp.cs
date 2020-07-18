using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.SafeQ
{
    /// <summary>
    /// JediOmniSafeQApp runs SafeQ automation of the Control Panel
    /// </summary>
    public class JediOmniSafeQApp : SafeQAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniMasthead _masthead;
        private readonly TimeSpan _idleTimeoutOffset;
        private readonly string[] _okButtonIdScan = { "etui-modal-window_button-1-of-1" };
        private readonly string[] _homeButtonId = { "js-home" };
        private readonly string[] _optionButtonId = { "detailsIcon-0" };
        private readonly string[] _optionDropBoxButtonId = { "js-openFileTypes" };
        private readonly string[] _filetypeSelectButtonId = { "js-fileTypeSelect" };
        private readonly string[] _optionBackButton = { "js-back-button" };
        
        /// For SafeQPullPrinting
        private readonly string[] _deleteButtonId = { "selectionBar-delete", "etui-modal-window_button-1-of-2" };
        private readonly string[] _printBWButtonId = { "js-option-color-0" };
        private readonly string[] _printColorButtonId = { "js-option-color-1" };
        private readonly string[] _printIncreaseButtonId = { "js-increase-copies" };
        private readonly string[] _printOneSidedButtonId = { "js-option-side-0" };
        private readonly string[] _printTwoSidedButtonId = { "js-option-side-1" };
        private readonly string[] _printButtonId = { "footer-printButton" };
        private readonly string[] _printSaveButtonId = { "js-save-button" };
        private readonly string[] _printBackButtonId = { "js-back-button" };
        private readonly string[] _selectAllButtonId = { "selectionBar-selectAll" };
        private readonly string[] _selectFirstJobId = { "job-0" };
        private readonly string[] _printOptionButtonId = { "jobDetailsIcon-0" };
        private readonly string[] _okButtonId = { "js-ok-button" };

        /// <summary>
        /// The OXPD engine
        /// </summary>
        private readonly OxpdBrowserEngine _engine;
        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        public string AppName = "SafeQ Print";

        /// <param name="device"></param>
        public JediOmniSafeQApp(JediOmniDevice device)
            : base(device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _launchHelper = new JediOmniLaunchHelper(device);
            _masthead = new JediOmniMasthead(device);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
            _engine = new OxpdBrowserEngine(_controlPanel, SafeQResource.SafeQJavaScript);
        }

        /// <summary>
        /// Presses the Select All button
        /// </summary>
        public override void SelectAll()
        {
            PressButton(_selectFirstJobId[0]);
            PressButton(_selectAllButtonId[0]);
        }

        /// <summary>
        /// Presses the Print button
        /// </summary>
        public override void Print()
        {
            int threshhold = 30;
            PressButton(_printButtonId[0]);
            if (!StartedProcessingWork(TimeSpan.FromSeconds(threshhold)))
            {
                throw new DeviceWorkflowException($"SafeQ Print operation did not start printing documents within {threshhold} seconds.");
            }
        }

        /// <summary>
        /// Presses the Print All button
        /// </summary>
        public override void PrintAll()
        {
            int threshhold = 30;
            if (!StartedProcessingWork(TimeSpan.FromSeconds(threshhold)))
            {
                throw new DeviceWorkflowException($"SafeQ Print operation did not start printing documents within {threshhold} seconds.");
            }
        }

        /// <summary>
        /// Presses the Delete button
        /// </summary>
        public override void Delete()
        {
            PressButton(_deleteButtonId[0]);
            PressButton(_deleteButtonId[1]);
        }

        /// <summary>
        /// Dismisses the popup that displays after a Print or Delete operation.
        /// </summary>
        public override void DismissPostPrintOperation()
        {
            if (WaitObjectForAvailable(_okButtonId[0], TimeSpan.FromMilliseconds(100)))
            {
                PressButton(_okButtonId[0]);
            }
        }

        /// <summary>
        /// Sets the copy count.
        /// </summary>
        /// <param name="numCopie">The number copie.</param>
        public override void SetCopyCount(int numCopie)
        {
            for (int i = 1; i < numCopie; i++)
            {
                PressButton(_printIncreaseButtonId[0]);
            }
        }

        /// <summary>
        /// Select the SetColorMode.
        /// </summary>
        /// <param name="colorType">The number copie.</param>
        public override void SetColorMode(SafeQPrintColorMode colorType)
        {
            if (colorType == SafeQPrintColorMode.BW)
            {
                PressButton(_printBWButtonId[0]);
            }
            else if (colorType == SafeQPrintColorMode.Color)
            {
                PressButton(_printColorButtonId[0]);
            }
        }

        /// <summary>
        /// Sets the Sides Option.
        /// </summary>
        /// <param name="sidedMode">The number copie.</param>
        public override void SetSides(SafeQPrintSides sidedMode)
        {
            if (sidedMode == SafeQPrintSides.Onesided)
            {
                PressButton(_printOneSidedButtonId[0]);
            }
            else if (sidedMode == SafeQPrintSides.Twosided)
            {
                PressButton(_printTwoSidedButtonId[0]);
            }
        }

        /// <summary>
        /// Press Option button
        /// </summary>
        public override void SelectOption()
        {
            PressButton(_printOptionButtonId[0]);
        }

        /// <summary>
        /// Press Save button
        /// </summary>
        public override void SaveOption()
        {
            PressButton(_printSaveButtonId[0]);
        }

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="authenticator">The authenticator.</param>
        /// <returns><c>true</c> if the printer release jobs, <c>false</c> otherwise.</returns>
        public override bool SignInReleaseAll(IAuthenticator authenticator)
        {
            _launchHelper.PressSignInButton();
            Authenticate(authenticator);
            bool released = _controlPanel.WaitForState(".hp-label:contains(Releasing jobs)", OmniElementState.Exists);
            _controlPanel.WaitForAvailable(JediOmniLaunchHelper.SignInOrSignoutButton);
            return released;
        }

        /// <summary>
        /// Press SafeQ Solution button on home screen
        /// </summary>
        /// <param name="waitForm">Form for launched screen</param>
        private void PressSafeQSolutionButton(string waitForm)
        {
            string solutionTitle = (AppName.Equals(SolutionButtonPrintTitle)) ? SolutionButtonPrintTitle : SolutionButtonScanTitle;
            _launchHelper.PressSolutionButton(solutionTitle, "SafeQ " + solutionTitle, waitForm);
        }

        /// <summary>
        /// Gets the document count.
        /// </summary>
        /// <returns>Document count</returns>
        public override int GetDocumentCount()
        {
            return int.Parse(_engine.ExecuteFunction("getDocumentCount").Replace("\"", ""));
        }

        /// <summary>
        /// Get all documents Ids by JavaScript.
        /// The Ids separated by "$"
        /// </summary>
        /// <returns></returns>
        public override List<string> GetDocumentIds()
        {
            List<string> result = new List<string>();

            char[] separator = { '$' };

            string idString = _engine.ExecuteFunction("getDocumentIds").Trim('"', '$');

            if (idString.Equals("-1"))
            {
                throw new DeviceWorkflowException("Unknown solution error encountered.");
            }

            string[] items = idString.Split(separator, StringSplitOptions.RemoveEmptyEntries);

            foreach (string item in items)
            {
                result.Add(item);
            }

            return result;
        }

        /// <summary>
        /// Launches SafeQ with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator</param>
        /// <param name="authenticationMode">The authentication mode, eager or lazy</param>
        /// <param name="parameters">The authentication parameters</param>
        public override void Launch(IAuthenticator authenticator, AuthenticationMode authenticationMode, Dictionary<string, object> parameters)
        {
            _launchHelper.WorkflowLogger = authenticator.WorkflowLogger;

            if (authenticationMode.Equals(AuthenticationMode.Eager))
            {
                if (authenticator.Provider != AuthenticationProvider.Card)
                {
                    _launchHelper.PressSignInButton();
                }
                Authenticate(authenticator, JediOmniLaunchHelper.SignInOrSignoutButton, parameters);                
                PressSafeQSolutionButton(JediOmniLaunchHelper.LazySuccessScreen);
            }
            else // AuthenticationMode.Lazy
            {
                PressSafeQSolutionButton(JediOmniLaunchHelper.SignInForm);
                Authenticate(authenticator, JediOmniLaunchHelper.LazySuccessScreen, parameters);
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Authenticates the specified authenticator
        /// </summary>
        /// <param name="authenticator"></param>
        /// <param name="waitForm"></param>
        /// <param name="parameters"></param>
        private void Authenticate(IAuthenticator authenticator, string waitForm, Dictionary<string, object> parameters)
        {
            authenticator.Authenticate(parameters);
            _controlPanel.WaitForAvailable(waitForm);          
        }

        /// <summary>
        /// Authenticates the user for SafeQ
        /// </summary>
        /// <param name="authenticator">The authenticator</param>
        /// <returns>true on success</returns>
        private bool Authenticate(IAuthenticator authenticator)
        {
            bool bSuccess = false;
            try
            {
                authenticator.Authenticate();

                bSuccess = true;
            }
            catch (ElementNotFoundException ex)
            {
                List<string> currentForm = _controlPanel.GetIds("hp-smart-grid", OmniIdCollectionType.Children).ToList();
                if (currentForm.Contains("HomeScreenForm"))
                {
                    throw new DeviceWorkflowException("Application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceInvalidOperationException(string.Format("Cannot launch the application from {0}.", currentForm), ex);
                }
            }
            catch (OmniInvalidOperationException ex)
            {
                throw new DeviceInvalidOperationException(string.Format("Could not launch the application: {0}", ex.Message), ex);
            }

            return bSuccess;
        }
        /// <summary>
        /// Select Job on the SafeQScan
        /// </summary>
        /// <param name="filename">The filename.</param>
        /// <param name="scanCount">The scan count.</param>
        public override void SelectJob(string filename, int scanCount)
        {
            bool result = IsHomeScreen();
            if (result)
            {
                PressElementByText("js-scanworkflows", "span", filename);
                if (scanCount > 1)
                {
                    RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
                }
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            }
            else
            {
                DeviceWorkflowException e = new DeviceWorkflowException("Current screen is not home before selecting Job.");
                throw e;
            }
        }
        /// <summary>
        /// To scan in a SafeQScan
        /// </summary>
        public override void ScanOption_SidedJob(int ScanCount)
        {
            string reason = $"Fail to execution Sided Scan Job";
            try
            {
                for (int idx = 1; idx < ScanCount; idx++)
                {
                    if (!_controlPanel.WaitForAvailable("#hpid-button-scan", _idleTimeoutOffset))
                    {
                        throw new DeviceWorkflowException($"The scan button did not become available within {_idleTimeoutOffset.Seconds} seconds.");
                    }
                    // The first scan begin is set when the workflow button was pressed.
                    RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                    RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
                    if (_controlPanel.WaitForAvailable("#hpid-button-scan", TimeSpan.FromSeconds(90)))
                    {
                        _controlPanel.Press("#hpid-button-scan");
                    }
     
                }
                if (ScanCount >= 1 && _controlPanel.WaitForAvailable("#hpid-button-done", TimeSpan.FromSeconds(90)))
                {
                    _controlPanel.Press("#hpid-button-done");
                }
                else
                {
                    throw new DeviceWorkflowException("Fail to find Done button");
                }
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            }
            catch (Exception ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to Sided Scan job - \"Scan more\" screen is not displayed {ex.ToString()}", ex);
                throw e;
            }
        }

        /// <summary>
        /// Wait for job completed
        /// </summary>
        public override bool WaitForScanning(int scanCount)
        {
            int _count = 0;
            while (_controlPanel.CheckState("#hpid-button-Cancel", OmniElementState.Exists))
            {
                if (_count > 100)
                {
                    return false;
                }
                _count++;
                Thread.Sleep(500);
            }
            Thread.Sleep(3000);
            PressButton(_okButtonIdScan[0]);
            return _engine.WaitForHtmlContains(_homeButtonId[0], TimeSpan.FromSeconds(5));
        }

        /// <summary>
        /// To Change Filetype in a SafeQScan
        /// </summary>
        public void ChangeFileType(int fileType)
        {
            /* 
             * For changing FileType Option. This code is for new PR from manual team.
             * 
             */
            string _fileTypeClickButtonId = $"js-fileType-value-{fileType}-localized";
            _engine.WaitForHtmlContains(_optionButtonId[0], TimeSpan.FromSeconds(10));
            PressButton(_optionButtonId[0]);
            _engine.WaitForHtmlContains(_optionDropBoxButtonId[0], TimeSpan.FromSeconds(10));
            PressButton(_optionDropBoxButtonId[0]);
            _engine.WaitForHtmlContains(_fileTypeClickButtonId, TimeSpan.FromSeconds(10));
            PressButton(_fileTypeClickButtonId);
            _engine.WaitForHtmlContains(_filetypeSelectButtonId[0], TimeSpan.FromSeconds(10));
            PressButton(_filetypeSelectButtonId[0]);
            _engine.WaitForHtmlContains(_optionBackButton[0], TimeSpan.FromSeconds(10));
            PressButton(_optionBackButton[0]);
        }

        /// <summary>
        /// Checks error state of the device by looking at the devices banner.
        /// </summary>
        /// <returns>bool - true if error statement exists</returns>
        public override bool BannerErrorState()
        {
            string bannerText = _controlPanel.GetValue(".hp-masthead-title:last", "innerText", OmniPropertyType.Property);
            return bannerText.Contains("Runtime Error");
        }

        /// <summary>
        /// Checks that the current screen is home
        /// </summary>
        public override bool IsHomeScreen()
        {
            bool result = false;
            result = _engine.WaitForHtmlContains(_homeButtonId[0], TimeSpan.FromSeconds(10));
            return result;
        }

        /// <summary>
        /// Change App name by job
        /// </summary>
        public override void ChangeAppName(string appName)
        {
            AppName = appName;
        }

        /// <summary>
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public override bool StartedProcessingWork(TimeSpan ts) => _masthead.WaitForActiveJobsButtonState(true, ts);

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns></returns>
        public override bool FinishedProcessingWork() => _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);
        
    }
}
