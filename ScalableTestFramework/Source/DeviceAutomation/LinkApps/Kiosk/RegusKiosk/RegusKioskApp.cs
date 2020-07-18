using System;
using System.Linq;
using System.Threading;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using HP.SPS.SES;
using HP.SPS.SES.Helper;
using static HP.ScalableTest.Framework.Logger;
using BadgeBoxInfo = HP.ScalableTest.Framework.Assets.BadgeBoxInfo;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.Kiosk.RegusKiosk
{
    /// <summary>
    /// Configuration to prepare job for Each Link Apps.
    /// </summary>
    public class RegusKioskApp : DeviceWorkflowLogSource, IDisposable
    {
        private readonly OxpdBrowserEngine _engine;
        private static string _exceptionCategoryData = "ExceptionCategory";        

        /// <summary>
        /// Package name of Kiosk
        /// </summary>
        public string KioskPackageName;

        /// <summary>
        /// Package name of Regus
        /// </summary>
        public string RegusPackageName;

        /// <summary>
        /// Information about a device 
        /// </summary>
        public IDeviceInfo DeviceInfo;
        
        /// <summary>
        /// Control the JediOmniDevice
        /// </summary>
        public JediOmniDevice Device;

        /// <summary>
        /// Control the JetAdvantageLinkUI
        /// </summary>
        public JetAdvantageLinkUI LinkUI;

        /// <summary>
        /// Set RegusKioskOptionsManager
        /// </summary>
        public RegusKioskOptionsManager KioskOptionManager;

        /// <summary>
        /// Check displayed message to wait job running
        /// </summary>
        private JetAdvantageLinkControlHelper JetAdvantageLinkControlHelper;

        /// <summary>
        /// Occurs when the activity status changes.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> ActivityStatusChanged;

        /// <summary>
        /// Configuration to prepare job for Each Link Apps.
        /// <param name="device">The device</param>        
        /// </summary>
        public RegusKioskApp(IDevice device)
        {
            KioskPackageName = $"com.hp.kiosk";
            RegusPackageName = $"com.hp.auth.regus";
            LinkUI = new JetAdvantageLinkUI(device);
            Device = (JediOmniDevice)device;
            JetAdvantageLinkControlHelper = new JetAdvantageLinkControlHelper(LinkUI, KioskPackageName);            
            KioskOptionManager = new RegusKioskOptionsManager(LinkUI, KioskPackageName);
        }

        /// <summary>
        /// Initialize RegusKiosk to Home Screen
        /// </summary>
        public void RegusKioskInitialize()
        {
            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{RegusPackageName}:id/loading_msg_textview")))
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"The activity is unable to begin due to a Please Wait popup message displayed from a previous activity.");
                throw e;
            }

            NavigateHome();

            if (!AtHomeScreen())
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Initialize failed: Login screen is not displayed.");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                throw e;
            }            
        }

        /// <summary>
        /// Navigate to home screen
        /// </summary>
        public void NavigateHome()
        {
            bool homeScreen = false;
            TimeSpan waitingTime = TimeSpan.FromMilliseconds(1000);

            if ((homeScreen = AtHomeScreen()) == false)
            {
                RecordEvent(DeviceWorkflowMarker.NavigateHomeBegin);

                for (int i = 0; i < 10; i++)
                {
                    // Pressing the home button should get us back to the home screen,
                    if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/error_ok_btn"), 500, 1))
                    {
                        ErrorPopupOkButton();
                    }
                    if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{RegusPackageName}:id/ok_btn"), 500, 1))
                    {
                        PopupOkButton();
                    }
                    else if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{RegusPackageName}:id/popup_billing_information_ok_btn"), 500, 1))
                    {
                        BillingPopupOkButton();
                    }
                    else if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/btnJobcancel"), 500, 1))
                    {
                        JobCancelButton();
                    }
                    else if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{RegusPackageName}:id/webviewScreen"), 500, 1))
                    {
                        LinkUI.Controller.PressKey(4);
                    }
                    else if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/logoutBtn"), 500, 1))
                    {
                        SignOut();
                    }

                    if (AtHomeScreen())
                    {
                        homeScreen = true;
                        RecordEvent(DeviceWorkflowMarker.NavigateHomeEnd);
                        break;
                    }
                }
            }

            if (!homeScreen)
            {
                // If we're not there after 10 tries, we're not going to make it.
                throw new DeviceWorkflowException($"Unable to navigate to home screen.");
            }
        }

        /// <summary>
        /// Check HomeScreen
        /// </summary>
        /// <returns></returns>
        protected bool AtHomeScreen()
        {
            return JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().Text("Username & Password"), 500, 1);
        }


        /// <summary>
        /// Do login for using ID/Password
        /// </summary>
        /// <param name="id">The id for log in.</param>
        /// <param name="password">The password for log in.</param>
        /// <returns>IAuthenticator</returns>
        public void RegusKioskLoginAuthenticate(string id, string password)
        {
            bool result = false;
            RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);
            if (result = LinkUI.Controller.Click(new UiSelector().Text("Username & Password")))
            {
                // Check if login page is lodded.
                if (result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId("com.hp.auth.regus:id/webviewScreen"), 500, 40))
                {
                    result = (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ClassName("android.widget.Button"), 500, 120) &&
                        JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ClassName("android.widget.EditText"), 500, 120));
                }
                
                if (result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"email")))
                {
                    result = LinkUI.Controller.SetText(new UiSelector().ResourceId($"email"), id);
                }

                if (result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"password")))
                {
                    result = LinkUI.Controller.SetText(new UiSelector().ResourceId($"password"), password);
                }

                if (result)
                {
                    if (LinkUI.Controller.IsVirtualKeyboardShown() == true)
                    {
                        LinkUI.Controller.PressKey(KeyCode.KEYCODE_BACK);
                        Wait.ForTrue(() => (LinkUI.Controller.IsVirtualKeyboardShown() == false), TimeSpan.FromSeconds(3),TimeSpan.FromMilliseconds(250));
                    }
                    result = LinkUI.Controller.Click(new UiSelector().ResourceId($"LoginButton"));
                }
            }

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Login failed");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                throw e;
            }
            
            VerifyLogin(RegusKioskAuthType.Login, id);
        }

        /// <summary>
        /// Do login for using Card(BadgeBox)
        /// </summary>
        /// <param name="executionData">The plugin execution data.</param>
        /// <returns></returns>
        public void RegusKioskCardAuthenticate(PluginExecutionData executionData)
        {
            IAuthenticator auth = null;

            // Gets the authenticator for the given device and requested solution.
            auth = AuthenticatorFactory.Create(DeviceInfo.AssetId, Device, AuthenticationProvider.Card, executionData);
            auth.WorkflowLogger = WorkflowLogger;
            try
            {
                RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);
                auth.Authenticate();
            }
            catch(DeviceInvalidOperationException)
            {
                UpdateStatus($"Kiosk needs checking other way to verify login.\n ");
            }            

            VerifyLogin(RegusKioskAuthType.Card, null);
        }

        /// <summary>
        /// Do login for using World Key PIN
        /// </summary>
        /// <param name="pin">The password for log in.</param>
        /// <returns>IAuthenticator</returns>
        public void RegusKioskPinAuthenticate(string pin)
        {
            bool result = false;
            RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);
            if (result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{RegusPackageName}:id/pinText")))
            {
                result = LinkUI.Controller.SetText(new UiSelector().ResourceId($"{RegusPackageName}:id/pinText"), pin);
            }

            if (result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{RegusPackageName}:id/loginBtn")))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{RegusPackageName}:id/loginBtn"));
            }

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Login failed");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                throw e;
            }
            VerifyLogin(RegusKioskAuthType.Pin, null);
        }

        /// <summary>
        /// Verify screen after Login 
        /// <param name="authType">authType for Login</param>
        /// <param name="id">Navigate to destination by FilePath</param>
        /// </summary>                
        private void VerifyLogin(RegusKioskAuthType authType, string id)
        {   
            if(!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/homeLayout"),500,120))
            {
                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{RegusPackageName}:id/loading_msg_textview")))
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"The sign in Please Wait popup message did not disappear within 60 seconds.");
                    throw e;                    
                }
                else
                {
                    CheckServerErrorPopup();
                    DeviceWorkflowException e = new DeviceWorkflowException($"Home layout after login is not displayed :: {authType}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                    throw e;
                }
            }

            if (authType.Equals(RegusKioskAuthType.Login))
            {
                if (!LinkUI.Controller.GetText(new UiSelector().ResourceId($"{KioskPackageName}:id/userName")).Contains(id))
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"Displayed ID is wrong: {LinkUI.Controller.GetText(new UiSelector().ResourceId($"{KioskPackageName}:id/userName"))}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                    throw e;
                }
            }
            RecordEvent(DeviceWorkflowMarker.AuthenticationEnd);
        }

        /// <summary>
        /// Launch with copy 
        /// <param name="jobType">JobType</param>
        /// </summary>  
        public void LaunchCopy(RegusKioskJobType jobType)
        {
            bool result = false;
            UpdateStatus($"Launch start: {jobType.GetDescription()}");

            result = LinkUI.Controller.Click("//*[@resource-id=\'com.hp.kiosk:id/homeLayout\']/*[2]/*[1]/*[1]/*[1]/*[1]/*[1]");

            if (result)
            {
                VerifyLaunch(jobType, new UiSelector().ResourceId($"{KioskPackageName}:id/start_tab_start_btn"));
            }
            else
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Launch failed: Clicking copy object is failed: {jobType.GetDescription()}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Launch with print
        /// <param name="jobType">JobType</param>
        /// <param name="printSource">Print source</param>
        /// <param name="path">File path</param>
        /// </summary>  
        public void LaunchPrint(RegusKioskJobType jobType, RegusKioskPrintSource printSource, string path)
        {
            bool result = false;
            UpdateStatus($"Launch start: {jobType.GetDescription()}");

            result = LinkUI.Controller.Click("//*[@resource-id=\'com.hp.kiosk:id/homeLayout\']/*[2]/*[1]/*[1]/*[2]/*[1]/*[1]");

            if (result)
            {
                NavigatePrintFilePath(printSource, path);
                VerifyLaunch(jobType, new UiSelector().ResourceId($"{KioskPackageName}:id/start_tab"));
            }
            else
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Launch failed: Selecting print source is failed: {printSource.GetDescription()}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Launch with scan
        /// <param name="jobType">JobType</param>
        /// <param name="scanDestination">Destination for scan</param>
        /// </summary>  
        public void LaunchScan(RegusKioskJobType jobType, RegusKioskScanDestination scanDestination)
        {
            bool result = false;
            UpdateStatus($"Launch start: {jobType.GetDescription()}");

            result = LinkUI.Controller.Click("//*[@resource-id=\'com.hp.kiosk:id/homeLayout\']/*[2]/*[1]/*[1]/*[3]/*[1]/*[1]");

            if (result)
            {
                switch (scanDestination)
                {
                    case RegusKioskScanDestination.USB:
                        if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/scan_main_usb_btn"), 500, 120))
                        {
                            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/scan_main_usb_btn"));
                        }
                        break;
                    case RegusKioskScanDestination.Email:
                        if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/scan_main_email_btn"), 500, 120))
                        {
                            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/scan_main_email_btn"));
                        }
                        break;
                    default:
                        UpdateStatus($"Scan destination is invalid: {scanDestination.GetDescription()}");
                        result = false;
                        break;
                }
            }

            if (result)
            {
                VerifyLaunch(jobType, new UiSelector().ResourceId($"{KioskPackageName}:id/start_tab"));
            }
            else
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Launch failed: Selecting scan destination is failed: {scanDestination.GetDescription()}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                throw e;
            }            
        }

        /// <summary>
        /// Verify screen after Launch 
        /// <param name="jobType">JobType: Copy, Print, Scan</param>        
        /// </summary>  
        private void VerifyLaunch(RegusKioskJobType jobType)
        {
            if (!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/ll_price_per_page")))
            {
                CheckServerErrorPopup();
                DeviceWorkflowException e = new DeviceWorkflowException($"Launch failed: {jobType.GetDescription()} - It has no Price per page object");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                throw e;
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Verify screen after Launch 
        /// <param name="jobType">JobType: Copy, Print, Scan</param>        
        /// <param name="waitingObject">Waiting object</param>        
        /// </summary>  
        private void VerifyLaunch(RegusKioskJobType jobType, string waitingObject)
        {
            if (!JetAdvantageLinkControlHelper.WaitingObjectAppear(waitingObject))
            {
                CheckServerErrorPopup();
                DeviceWorkflowException e = new DeviceWorkflowException($"Launch failed: {jobType.GetDescription()} - It has no Price per page object");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                throw e;
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Navigate to select print file
        /// <param name="printSource">Print source</param>
        /// <param name="path">File path</param>
        /// </summary>
        public void NavigatePrintFilePath(RegusKioskPrintSource printSource, string path)
        {
            if (RegusKioskPrintSource.USB.Equals(printSource))
            {
                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/usb_btn"), 500, 120))
                {
                    if (!LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/usb_btn")))
                    {
                        DeviceWorkflowException e = new DeviceWorkflowException($"Failed to select USB.");
                        e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                        throw e;
                    }
                }
            }

            if (!NavigateToDestination(printSource, path))
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Failed to Navigate file path.");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Navigate the location to save scan file(to select print file)
        /// <param name="printSource">Navigate to destination by FilePath</param>
        /// <param name="filePath">Navigate to destination by FilePath</param>
        /// </summary>
        public bool NavigateToDestination(RegusKioskPrintSource printSource, string filePath)
        {            
            string current_string = null;         
            string filename = null;            
            string path = filePath;
            
            path = path.Trim();
            
            while (path.Contains("/"))
            {
                current_string = path.Substring(0, path.IndexOf('/'));
                path = path.Substring(path.IndexOf('/') + 1);
                filename = current_string;
                LinkUI.Controller.Click(new UiSelector().TextContains(filename));
            }

            if (!string.IsNullOrEmpty(path))
            {
                filename = path;
                UpdateStatus($"Final path : {filename}");
                if (RegusKioskPrintSource.PrinterOn.Equals(printSource))
                {
                    WaitingForUploadFile(filename);
                }
                return LinkUI.Controller.Click(new UiSelector().TextContains(filename));
            }
            
            return false;            
        }

        /// <summary>
        /// Waiting for upload file from PrintOn
        /// <param name="filename">filename uploaded from PrinterOn</param>
        /// </summary>
        private void WaitingForUploadFile(string filename)
        {
            int count = 0;
            do
            {
                LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/refresh_btn"));
                count++;
                UpdateStatus($"Waiting for uploaded file from PrinterOn. Count : {count} , Total waiting time : {count * 5}");
            } while ((!LinkUI.Controller.Click(new UiSelector().TextContains(filename))) && count * 5 < 300);
            // Waiting Uploaded file from PrinterOn for 5 min.
        }

        /// <summary>
        /// Start Job after SetOptions step
        /// <param name="jobType">Kiosk job type</param>
        /// <param name="jobBuildPageCount">Options for running copy job</param>
        /// </summary>
        public void ExecutionJob(RegusKioskJobType jobType, int jobBuildPageCount=0)
        {
            if (jobType.Equals(RegusKioskJobType.Print))
            {
                PrintExecution();
            }
            else
            {
                CopyScanExecution(jobBuildPageCount);
            }
        }

        /// <summary>
        /// Start Job after SetOptions step
        /// </summary>
        private void PrintExecution()
        {
            bool result = false;
            if (JetAdvantageLinkControlHelper.WaitingObjectAppear
                (new UiSelector().ResourceId($"{KioskPackageName}:id/start_tab_start_btn"), 500, 120))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/start_tab_start_btn"));
                RecordEvent(DeviceWorkflowMarker.PrintJobBegin);
            }
            
            if (result && (result = JetAdvantageLinkControlHelper.WaitingObjectAppear
                (new UiSelector().ResourceId($"{RegusPackageName}:id/billing_information_title"), 500, 120)))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{RegusPackageName}:id/popup_billing_information_ok_btn"));
            }
            CheckServerErrorPopup();
            RecordEvent(DeviceWorkflowMarker.PrintJobEnd);
            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Execution Print Job failed");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Start Job after SetOptions step
        /// <param name="JobBuildPageCount">Job build count for running scan job</param>
        /// </summary>
        private void CopyScanExecution(int JobBuildPageCount)
        {
            bool result = false;

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear
                (new UiSelector().ResourceId($"{KioskPackageName}:id/start_tab_start_btn"), 500, 600))
            {
                if(!LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/start_tab_start_btn")))
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"Not display a start_tab_start_btn:: RegusKiosk");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                    throw e;
                }                
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/loading_msg_textview"), 500, 4))
                {
                    JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{KioskPackageName}:id/loading_msg_textview"), 500, 20);
                }
                CheckServerErrorPopup();
            }

            try
            {
                for (int i = 0; i < JobBuildPageCount; i++)
                {
                    Device.ControlPanel.WaitForAvailable("#hpid-button-scan", TimeSpan.FromSeconds(20));
                    if (i == (JobBuildPageCount - 1))
                    {
                        UpdateStatus($"Remained Scan Job: JobBuildPageCount : {JobBuildPageCount}, iCount : {i}, DoneButton");
                        if (Device.ControlPanel.WaitForAvailable("#hpid-button-done", TimeSpan.FromSeconds(90)))
                        {
                            Device.ControlPanel.Press("#hpid-button-done");
                        }
                        if (!JetAdvantageLinkControlHelper.WaitingObjectDisappear
                            (new UiSelector().ResourceId($"{KioskPackageName}:id/jobProgressbar"), 500, 120))
                        {
                            DeviceWorkflowException e = new DeviceWorkflowException($"Job Progress Bar Error in Scan/Copy:: RegusKiosk");
                            e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                            throw e;
                        }
                        RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                    }
                    else if (i != (JobBuildPageCount - 1))
                    {
                        UpdateStatus($"Remained Scan Job: JobBuildPageCount : {JobBuildPageCount}, iCount : {i}, ScanButton");
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        if (Device.ControlPanel.WaitForAvailable("#hpid-button-scan", TimeSpan.FromSeconds(20)))
                        {
                            Device.ControlPanel.Press("#hpid-button-scan");
                        }                        
                        CheckServerErrorPopup();
                    }
                }
            }
            catch (Exception ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Setting Job Build Count Copy failed (omni) {ex.ToString()} :: RegusKiosk", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear
                (new UiSelector().ResourceId($"{RegusPackageName}:id/billing_information_title"), 500, 120))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{RegusPackageName}:id/popup_billing_information_ok_btn"));
            }

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Execution Copy/Scan Job failed :: popup_billing_information_ok_btn");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear
                (new UiSelector().ResourceId($"{RegusPackageName}:id/billing_wait_title"), 500, 4))
            {
                JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{RegusPackageName}:id/billing_wait_title"), 500, 30);
            }

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/loading_msg_textview"), 500, 4))
            {
                UpdateStatus($"Waiting to process email");
                JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{KioskPackageName}:id/loading_msg_textview"), 500, 180);
            }

            CheckServerErrorPopup();
        }

        /// <summary>
        /// SignOut 
        /// </summary>
        public void SignOut()
        {
            bool result = false;
            
            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/logoutBtn"), 500, 120))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/logoutBtn"));
            }

            if (result == JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/confirm_cancel_btn"), 500, 60))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/confirm_cancel_btn"));

                if (!result) // Retry if the logout popup disappeared due to the loading process popup.
                {
                    if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/loading_msg_textview"), 500, 30))
                    {
                        JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{KioskPackageName}:id/loading_msg_textview"), 500, 60);

                        if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/logoutBtn"), 500, 120))
                        {
                            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/logoutBtn"));
                        }

                        if (result == JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/confirm_cancel_btn"), 500, 60))
                        {
                            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/confirm_cancel_btn"));
                        }
                    }
                }
            }            

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"SignOut is failed :: RegusKiosk");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignOut.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Dispose RegusKiosk
        /// </summary>
        public void Dispose()
        {
            LinkUI.Dispose();
            Device.Dispose();
        }

        /// <summary>
        /// Force closing the app when error occured with dispose
        /// </summary>
        public void ForceStop()
        {
            JetAdvantageLinkControlHelper.ForceStop();
            Dispose();
        }

        /// <summary>
        /// Check that error popup is displayed.
        /// </summary>
        private void CheckServerErrorPopup()
        {
            int timeOut = 0;
            timeOut = LinkUI.Controller.GetTimeout();

            try
            {
                LinkUI.Controller.SetTimeout(0);
                CheckErrorPopup();
                LinkUI.Controller.SetTimeout(timeOut);
            }
            catch (DeviceWorkflowException ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException(ex.Message.ToString() + $" :: RegusKiosk", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ServerError.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Return Error Message on the Popup after checking popup visible
        /// </summary>
        /// <returns></returns>
        private void CheckErrorPopup()
        {
            bool retainPopup = false;
            string ErrorMessage = null;
            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{KioskPackageName}:id/error_ok_btn")))
            {
                ErrorMessage = LinkUI.Controller.GetText(new UiSelector().ResourceId($"{KioskPackageName}:id/error_popup_description"));
                retainPopup = true;
            }
            if (retainPopup)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Error Popup is visible, Error message is \" {ErrorMessage}\".");
                throw e;
            }
        }

        /// <summary>
        /// Click Ok button on the Popup after checking popup visible
        /// </summary>
        /// <returns></returns>
        private void PopupOkButton()
        {
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{RegusPackageName}:id/ok_btn"));
        }

        /// <summary>
        /// Click Error Message Ok button on the Popup after checking popup visible
        /// </summary>
        /// <returns></returns>
        private void ErrorPopupOkButton()
        {
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/error_ok_btn"));
        }

        /// <summary>
        /// Click Billing Ok Job buttion
        /// </summary>
        private void BillingPopupOkButton()
        {
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{RegusPackageName}:id/popup_billing_information_ok_btn"));
        }

        /// <summary>
        /// Click Job Cancel buttion
        /// </summary>
        private void JobCancelButton()
        {
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/btnJobcancel"));
        }

        /// <summary>
        /// Updates the status of the plugin execution.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void UpdateStatus(string status)
        {
            LogInfo(status);
            ActivityStatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }
    }
}
