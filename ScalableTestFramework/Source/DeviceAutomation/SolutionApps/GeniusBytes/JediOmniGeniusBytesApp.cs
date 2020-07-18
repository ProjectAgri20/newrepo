using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.Utility;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JediOmni;
using System;
using System.Collections.Generic;
using System.Threading;
using System.Linq;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.GeniusBytes
{
    /// <summary>
    /// JediOmniGeniusBytesApp runs GeniusBytes automation of the Control Panel
    /// </summary>
    public class JediOmniGeniusBytesApp : GeniusBytesAppBase
    {
        private readonly JediOmniControlPanel _controlPanel;
        private readonly JediOmniLaunchHelper _launchHelper;
        private readonly JediOmniMasthead _masthead;
        private readonly TimeSpan _idleTimeoutOffset;

        /// <summary>
        /// Releases all documents on a device configured to release all documents on sign in.
        /// </summary>
        /// <param name="device"></param>
        public JediOmniGeniusBytesApp(JediOmniDevice device) : base(device.ControlPanel)
        {
            _controlPanel = device.ControlPanel;
            _launchHelper = new JediOmniLaunchHelper(device);
            _masthead = new JediOmniMasthead(device);
            _idleTimeoutOffset = device.PowerManagement.GetInactivityTimeout().Subtract(TimeSpan.FromSeconds(10));
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
        /// Launches GeinusBytes with the specified authenticator using the given authentication mode
        /// </summary>
        /// <param name="authenticator">The authenticator</param>
        public override void Launch(IAuthenticator authenticator)
        {
            //
            PressGeniusBytesAuthButton(authenticator.Provider);
            Authenticate(authenticator); // You may need to pass in the provider here, too

            string userInfo = $"User: {authenticator.Credential.UserName}";

            if (!Wait.ForTrue(()=> ExistElementText(userInfo), _idleTimeoutOffset, TimeSpan.FromMilliseconds(250)))
            {
                throw new DeviceWorkflowException($"GeniusBytes application did not show within {_idleTimeoutOffset.ToString()} seconds.");
            }
            
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Press Auth Button for Genius Bytes
        /// </summary>
        /// <param name="provider"></param>
        private void PressGeniusBytesAuthButton(AuthenticationProvider provider)
        {
            switch (provider)
            {
                case AuthenticationProvider.GeniusBytesGuest:
                    PressElementIDbyTextContains("td", "Guest Login");
                    break;
                case AuthenticationProvider.GeniusBytesManual:
                    PressElementIDbyTextContains("td", "Manual Login");
                    break;
                case AuthenticationProvider.GeniusBytesPin:
                    PressElementIDbyTextContains("td", "PIN Login");
                    break;
            }
        }
        
        private void PressGeniusBytesActionButton(string text)
        {
            PressElementIDbyText("button", text);
        }

        /// <summary>
        /// Authenticates the specified authenticator
        /// </summary>
        /// <param name="authenticator"></param>
        /// <param name="waitForm"></param>
        private void Authenticate(IAuthenticator authenticator, string waitForm)
        {
            authenticator.Authenticate();
            _controlPanel.WaitForAvailable(waitForm);
        }

        /// <summary>
        /// Authenticates the user for GeniusBytes
        /// </summary>
        /// <param name="authenticator">The authenticator</param>
        /// <returns>
        /// true on success
        /// </returns>
        /// <exception cref="DeviceWorkflowException">The GeniusBytes application button was not found on device home screen.</exception>
        /// <exception cref="DeviceInvalidOperationException"></exception>
        private bool Authenticate(IAuthenticator authenticator)
        {
            bool success = true;
            try
            {
                authenticator.Authenticate();
            }
            catch (ElementNotFoundException ex)
            {
                success = false;
                List<string> currentForm = _controlPanel.GetIds("hp-smart-grid", OmniIdCollectionType.Children).ToList();
                if (currentForm.Contains("HomeScreenForm"))
                {
                    throw new DeviceWorkflowException("The GeniusBytes application button was not found on device home screen.", ex);
                }
                else
                {
                    throw new DeviceInvalidOperationException(string.Format("Could not launch the GeniusBytes application from {0}.", currentForm), ex);
                }
            }
            return success;
        }

        /// <summary>
        /// Clear and Set filetype
        /// </summary>
        /// <returns></returns>
        public override void SetFileType(GeniusByteScanFileTypeOption fileType, TimeSpan timeOut)
        {
            string elementText = "File format";
            if (! WaitForElementReady(elementText, timeOut))
            {
                throw new DeviceWorkflowException($"'{elementText}' element was not available within {timeOut.TotalSeconds} seconds.");

            }
            PressElementIDbyText("td", elementText);

            elementText = fileType.GetDescription();
            if (! ScrollToObject(elementText, true))
            {
                throw new DeviceWorkflowException($"Unable to scroll to '{elementText}'.  Element not found.");
            }
            PressElementIDbyText("td", elementText);
        }

        /// <summary>
        /// Clear and Set filename
        /// It appears that the document title may be changed at the server. This could cause issues
        /// 
        /// Other workflows used the below at one time...
        /// WaitForElementReady("MyDocument_", TimeSpan.FromSeconds(3));
        /// PressElementIDbyTextContains("td", "MyDocument_");
        /// 
        /// </summary>
        /// <returns></returns>
        public override void SetFileName(string filename, GeniusBytesScanType scanType)
        {
            WaitForElementReady("GeniusMFP Document", TimeSpan.FromSeconds(3));
            PressElementIDbyText("td", "GeniusMFP Document");

            //clear text value
            WaitForObjectAvailable("kbInput", TimeSpan.FromSeconds(3));
            _controlPanel.WaitForAvailable("#hpid-keyboard-key-backspace", TimeSpan.FromSeconds(5));

            for (int i = 0; i< 30;i++)
            {
                _controlPanel.Press("#hpid-keyboard-key-backspace");
            }
            _controlPanel.TypeOnVirtualKeyboard(filename);
            _controlPanel.Press("#hpid-keyboard-key-enter");
        }

        /// <summary>
        /// Sets the email address.
        /// </summary>
        /// <param name="emailAddress">The email address.</param>
        /// <exception cref="NotImplementedException"></exception>
        public override void SetEmailAddress(string emailAddress)
        {
            if (WaitForElementReady("Add manually", TimeSpan.FromSeconds(5)))
            {
                PressElementIDbyTextContains("td", "Add manually");
                WaitForObjectAvailable("kbInput", TimeSpan.FromSeconds(3));

                _controlPanel.TypeOnVirtualKeyboard(emailAddress);
                _controlPanel.Press("#hpid-keyboard-key-enter");
            }
            else
            {
                throw new DeviceWorkflowException("Unable to press the 'Add manually' email button.");
            }
        }

        /// <summary>
        /// Set color options for the scan job.
        /// </summary>
        public override void SelectColorOption(GeniusByteScanColorOption value, TimeSpan timeOut)
        {
            string elementText = "Color mode";  // English - US 

            if (!WaitForElementReady(elementText, timeOut))
            {
                elementText = "Colour mode";  // English - UK
                if (!WaitForElementReady(elementText, timeOut))
                {
                    throw new DeviceWorkflowException($"'{elementText}' element was not available within {timeOut.TotalSeconds} seconds.");
                }
            }

            PressElementIDbyText("td", elementText);
            SelectOptionValue(value.GetDescription());
        }

        /// <summary>
        /// Sets the image preview.
        /// </summary>
        /// <param name="enabled">if set to <c>true</c> [enabled].</param>
        public override void SetImagePreview(bool enabled)
        {
            UseImagePreview = enabled;
        }

        /// <summary>
        /// Set sides options for the scan job.
        /// </summary>
        public override void SelectSidesOption(GeniusByteScanSidesOption value, TimeSpan timeOut)
        {
            string elementText = "Sides";
            if (!WaitForElementReady(elementText, timeOut))
            {
                throw new DeviceWorkflowException($"'{elementText}' element was not available within {timeOut.TotalSeconds} seconds.");

            }
            PressElementIDbyText("td", elementText);
            SelectOptionValue(value.GetDescription());
        }

        /// <summary>
        /// Set resolution options for the scan job.
        /// </summary>
        /// <returns>The result of the scan.</returns>
        public override void SelectResolutionOption(GeniusByteScanResolutionOption value, TimeSpan timeOut)
        {
            string elementText = "Resolution";
            if (!WaitForElementReady(elementText, timeOut))
            {
                throw new DeviceWorkflowException($"'{elementText}' element was not available within {timeOut.TotalSeconds} seconds.");

            }
            PressElementIDbyText("td", elementText);
            SelectOptionValue(value.GetDescription());
        }

        /// <summary>
        /// Press the Start-Scan button
        /// </summary>
        public override void StartScan(string sides, int pageCount)
        {
            if (Wait.ForTrue(() => ExistElementText("Start Scan"), TimeSpan.FromSeconds(10)))
            {
                if (UseImagePreview || pageCount > 1)
                {
                    TurnOnOptions(pageCount);
                    if (UseImagePreview && pageCount == 1)
                    {
                        ImagePreviewProcess();
                    }
                    else if (UseImagePreview && pageCount > 1)
                    {
                        ImagePreviewJobBuild(pageCount, TimeSpan.FromSeconds(30));
                    }
                    else // Job build only
                    {
                        JobBuildProcess(pageCount);
                    }
                }
                else if (pageCount == 1)
                {
                    UpdateStatus("Performing Single page scan.");
                    WaitForNotBlocked();

                    RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

                    PressElementIDbyText("td", "Start Scan");
                    MessageDisplayed("Job started");

                    RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                    RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);    
                }
                else
                {
                    throw new DeviceWorkflowException("Unable to determine GeniusBytes workflow.");

                }

                FinishProcessingJob(pageCount);
                RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);

            }               
        }

        private void JobBuildProcess(int pageCount)
        {
            UpdateStatus($"Performing Job Build with a page count of {pageCount.ToString()}.");
            PressElementIDbyText("td", "Start Scan");

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


            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
        }

        private void ImagePreviewProcess()
        {
            UpdateStatus("Performing Image Preview.");
            WaitForNotBlocked();
            PressElementIDbyText("td", "Start Scan");


            RecordEvent(DeviceWorkflowMarker.ImagePreviewBegin);

            _controlPanel.WaitForAvailable("#hpid-oxp-preview-dialog", TimeSpan.FromSeconds(10));
            RecordEvent(DeviceWorkflowMarker.ImagePreviewEnd);
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            _controlPanel.WaitForAvailable("#hpid-button-start", TimeSpan.FromSeconds(33));
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);


            PressOmniScanButton();
        }

        /// <summary>
        /// Presses the Omni start scan button.
        /// </summary>
        private void PressOmniScanButton()
        {
            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
            _controlPanel.Press("#hpid-button-start");            
        }

        private void TurnOnOptions(int pageCount)
        {
            if (pageCount > 1)
            {
                if (ScrollToObject("Continuous Scan", false))
                {
                    WaitForNotBlocked();
                    PressElementIDbyText("td", "Continuous Scan");
                    SetEnabledDisabled(GeniusByesScanEnableDisable.Enabled.GetDescription());
                }
                else
                {
                    throw new DeviceWorkflowException($"Unable to scroll to the option 'Continuous Scan'");
                }
            }
            if (UseImagePreview)
            {
                if (ScrollToObject("td", "Preview", 53))
                {
                    WaitForNotBlocked();
                    PressElementIDbyTextandZIndex("td", "Preview", 53);
                    SetEnabledDisabled(GeniusByesScanEnableDisable.Enabled.GetDescription());
                }
            }
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

            PressElementIDbyText("td", "Start Scan");

            //PressStartButton();
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

                _controlPanel.WaitForAvailable("#hpid-button-scan", _idleTimeoutOffset);
                _controlPanel.PressWait("#hpid-button-scan", "#hpid-button-done", TimeSpan.FromSeconds(90));

                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                RecordEvent(DeviceWorkflowMarker.ImagePreviewEnd);
            }

            _controlPanel.PressWait("#hpid-button-done", "#hpid-button-start", TimeSpan.FromSeconds(90));

            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);

            // sometimes get here and the start button is "really" usable. 
            _controlPanel.WaitForAvailable("#hpid-button-start", ts);
            _controlPanel.PressWait("#hpid-button-start", "#hpid-oxpd-scroll-pane", TimeSpan.FromSeconds(90)); //"#hpid-oxpd-scroll-pane"         
        }

        private void FinishProcessingJob(int pageCount)
        {
            DateTime startDt = DateTime.Now.AddSeconds(_idleTimeoutOffset.Seconds);
            int count = 1;

            // Sometimes even one page takes to long...Could be device related
            _controlPanel.SignalUserActivity();

            try
            {
                while (ExistElementText("Job in progress"))
                {
                    if (startDt <= DateTime.Now)
                    {
                        if (count < 3 && UseImagePreview)
                        {
                            count++;
                            startDt = DateTime.Now.AddSeconds(_idleTimeoutOffset.Seconds);
                            _controlPanel.SignalUserActivity();
                        }
                    }
                    // Calling into the OXPD browser to quick back to back can cause unexpected results...
                    Thread.Sleep(250);
                }
            }
            catch (Exception e)
            {
                UpdateStatus(e.Message);
                throw e;
            }
        }

        private void MessageDisplayed(string message)
        {
            // just in case we check prior to message being displayed.
            Wait.ForTrue(() => ExistElementText(message), TimeSpan.FromSeconds(2), TimeSpan.FromMilliseconds(250));

            Wait.ForFalse(() => ExistElementText(message), TimeSpan.FromSeconds(15), TimeSpan.FromMilliseconds(250));
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
        /// Checks to see if the processing of work as started. Default time to check is six (6) seconds
        /// </summary>
        /// <param name="ts">Time to continue checking</param>
        /// <returns></returns>
        public override bool StartedProcessingWork(TimeSpan ts) => _masthead.WaitForActiveJobsButtonState(true, ts);

        /// <summary>
        /// Returns true when finished processing the current work.
        /// </summary>
        /// <returns></returns>
        public override bool FinishedProcessingWork()
        {
            bool finished = false;            
            int count = 0;

            _controlPanel.SignalUserActivity();
            while (count < 4 && !finished)
            {
                finished = _masthead.WaitForActiveJobsButtonState(false, _idleTimeoutOffset);
                if (!finished)
                {
                    count++;
                    _controlPanel.SignalUserActivity();
                }
            }
            
            return finished;
        }


    }
}
