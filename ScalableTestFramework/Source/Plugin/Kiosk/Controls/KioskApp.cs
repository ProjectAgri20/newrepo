using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.Authentication;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Plugin.Kiosk.Options;
using HP.ScalableTest.Utility;
using HP.SPS.SES.Helper;
using System;
using System.Linq;
using System.Threading;
using static HP.ScalableTest.Framework.Logger;
using BadgeBoxInfo = HP.ScalableTest.Framework.Assets.BadgeBoxInfo;
using IDeviceInfo = HP.ScalableTest.Framework.Assets.IDeviceInfo;

namespace HP.ScalableTest.Plugin.Kiosk.Controls
{
    /// <summary>
    /// Configuration to prepare job for Each Link Apps.
    /// </summary>
    public class KioskApp : DeviceWorkflowLogSource, IDisposable
    {        
        private static string _exceptionCategoryData = "ExceptionCategory";        
        public string _totalPrice = "0";
        /// <summary>
        /// Package name of Kiosk
        /// </summary>
        public string KioskPackageName;

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
        /// Set KioskOptionsManager
        /// </summary>
        public KioskOptionsManager KioskOptionManager;

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
        public KioskApp(IDevice device)
        {
            KioskPackageName = $"com.samsung.dpd.kiosk.ui.activity";
            LinkUI = new JetAdvantageLinkUI(device);
            Device = (JediOmniDevice)device;                        
            JetAdvantageLinkControlHelper = new JetAdvantageLinkControlHelper(LinkUI, KioskPackageName);            
            KioskOptionManager = new KioskOptionsManager(LinkUI, KioskPackageName);
        }

        /// <summary>
        /// Do login for using ID/Password
        /// </summary>
        /// <param name="id">The id for log in.</param>
        /// <param name="password">The password for log in.</param>
        /// <returns>IAuthenticator</returns>
        public void KioskInitialize()
        {
            if(!JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{KioskPackageName}:id/loading_msg_textview"), 500, 120))
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Initialize failed: Loading object is not disappeared.");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                throw e;
            }
            
            Device.ControlPanel.PressHome();

            if (!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/qr_code_img"), 500, 120))
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Initialize failed: Login screen is not displayed.");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Do login for using ID/Password
        /// </summary>
        /// <param name="id">The id for log in.</param>
        /// <param name="password">The password for log in.</param>
        /// <returns>IAuthenticator</returns>
        public void KioskLoginAuthenticate(string id, string password)
        {
            bool result = false;
            RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);

            result = LinkUI.Controller.SetText("//*[@resource-id=\'com.samsung.dpd.kiosk.ui.activity:id/main_login_id\']", id);
            LinkUI.Controller.PressKey(4);
            Thread.Sleep(5000);
            result &= LinkUI.Controller.Click("//*[@resource-id=\'com.samsung.dpd.kiosk.ui.activity:id/main_login_pwd\']");
            result &= LinkUI.Controller.SetText("//*[@resource-id=\'com.samsung.dpd.kiosk.ui.activity:id/main_login_pwd\']", password);
            LinkUI.Controller.PressKey(4);
            result &= LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/main_login_ok"));

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Login failed");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                throw e;
            }

            VerifyLogin(KioskAuthType.Login, id);
        }

        /// <summary>
        /// Do login for using Card(BadgeBox)
        /// </summary>
        /// <param name="asset">The asset info for Card login.</param>
        /// <param name="credential">The credential info for Card login.</param>
        /// <returns>IAuthenticator</returns>
        public void KioskCardAuthenticate(Framework.Assets.AssetInfoCollection asset, System.Net.NetworkCredential credential)
        {
            IAuthenticator auth = null;

            // Gets the authenticator for the given device and requested solution.
            BadgeBoxInfo bbi = SetBadgeBox(asset, DeviceInfo);
            auth = AuthenticatorFactory.Create(Device, credential, bbi, AuthenticationProvider.Card);
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

            VerifyLogin(KioskAuthType.Card, null);
        }

        /// <summary>
        /// Sets the badge box.
        /// </summary>
        /// <param name="availableAssets">The available assets.</param>
        /// <param name="deviceAsset">The device asset.</param>
        /// <returns></returns>
        private BadgeBoxInfo SetBadgeBox(Framework.Assets.AssetInfoCollection availableAssets, IDeviceInfo deviceAsset)
        {
            ExecutionServices.SystemTrace.LogDebug($"Number of assets: {availableAssets.Count.ToString()}");
            BadgeBoxInfo badgeBoxAsset = null;
            if (availableAssets.OfType<BadgeBoxInfo>().Any())
            {
                ExecutionServices.SystemTrace.LogDebug($"Printer ID of badge box:");
                ExecutionServices.SystemTrace.LogDebug(availableAssets.OfType<BadgeBoxInfo>().FirstOrDefault().PrinterId);
                ExecutionServices.SystemTrace.LogDebug($"Device Asset: " + deviceAsset.AssetId);

                badgeBoxAsset = availableAssets.OfType<BadgeBoxInfo>().FirstOrDefault(n => n.PrinterId == deviceAsset.AssetId);
            }
            if (badgeBoxAsset == null)
            {
                throw new Exception($"No Badge Box associated with device {deviceAsset.AssetId}, {deviceAsset.Address}");
            }
            return badgeBoxAsset;
        }

        /// <summary>
        /// Verify screen after Login 
        /// <param name="authType">authType for Login</param>
        /// <param name="id">Navigate to destination by FilePath</param>
        /// </summary>                
        private void VerifyLogin(KioskAuthType authType, string id)
        {   
            if(!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/homeLayout")))
            {
                CheckServerErrorPopup();
                DeviceWorkflowException e = new DeviceWorkflowException($"Home layout after login is not displayed :: {authType}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                throw e;
            }

            if (authType.Equals(KioskAuthType.Login))
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
        /// <param name="option">Options for copy</param>
        /// </summary>  
        public void Launch(KioskJobType jobType, KioskCopyOptions option)
        {
            bool result = false;
            UpdateStatus($"Launch start: {jobType.GetDescription()}");

            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/homeCopyButton"));

            if (result)
            {
                VerifyLaunch(jobType, new UiSelector().ResourceId($"{KioskPackageName}:id/start_tab"));
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
        /// <param name="option">Options for print</param>
        /// </summary>  
        public void Launch(KioskJobType jobType, KioskPrintOptions option)
        {
            bool result = false;
            UpdateStatus($"Launch start: {jobType.GetDescription()}");

            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/homePrintButton"));

            if (result)
            {
                NavigatePrintFilePath(option);
                VerifyLaunch(jobType, new UiSelector().ResourceId($"{KioskPackageName}:id/start_tab"));
            }
            else
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Launch failed: Selecting print source is failed: {option.PrintSource.GetDescription()}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Launch with scan
        /// <param name="jobType">JobType</param>
        /// <param name="option">Options for scan</param>
        /// </summary>  
        public void Launch(KioskJobType jobType, KioskScanOptions option)
        {
            bool result = false;
            UpdateStatus($"Launch start: {jobType.GetDescription()}");

            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/homeScanButton"));

            if (result)
            {
                switch (option.ScanDestination)
                {
                    case KioskScanDestination.USB:
                        result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/scan_main_usb_btn"));
                        break;
                    case KioskScanDestination.Email:
                        result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/scan_main_email_btn"));
                        break;
                    default:
                        UpdateStatus($"Scan destination is invalid: {option.ScanDestination.GetDescription()}");
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
                DeviceWorkflowException e = new DeviceWorkflowException($"Launch failed: Selecting scan destination is failed: {option.ScanDestination.GetDescription()}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                throw e;
            }            
        }

        /// <summary>
        /// Verify screen after Launch 
        /// <param name="jobType">JobType: Copy, Print, Scan</param>        
        /// </summary>  
        private void VerifyLaunch(KioskJobType jobType)
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
        /// </summary>  
        private void VerifyLaunch(KioskJobType jobType, string waitingObject)
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
        /// Navigate to select print file)
        /// <param name="option">Options for print</param>
        /// </summary>
        public void NavigatePrintFilePath(KioskPrintOptions option)
        {
            if (KioskPrintSource.USB.Equals(option.PrintSource))
            {
                if(!LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/usb_btn")))
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"Failed to select USB.");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                    throw e;
                }
            }

            if (!NavigateToDestination(option))
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Failed to Navigate file path.");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Navigate the location to save scan file(to select print file)
        /// <param name="filePath">Navigate to destination by FilePath</param>
        /// </summary>
        public bool NavigateToDestination(KioskPrintOptions option)
        {            
            string current_string = null;         
            string filename = null;            
            string path = option.Path;
            
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
                if (KioskPrintSource.PrinterOn.Equals(option.PrintSource))
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
        /// <param name="copyOptions">Options for running copy job</param>
        /// </summary>
        public void ExecutionJob(KioskCopyOptions copyOptions)
        {
            CopyScanExecution(copyOptions.JobBuildPageCount);
        }

        /// <summary>
        /// Start Job after SetOptions step
        /// <param name="printOptions">Options for running print job</param>
        /// </summary>
        public void ExecutionJob(KioskPrintOptions printOptions)
        {
            PrintExecution();
        }

        /// <summary>
        /// Start Job after SetOptions step
        /// <param name="scanOptions">Options for running scan job</param>
        /// </summary>
        public void ExecutionJob(KioskScanOptions scanOptions)
        {
            CopyScanExecution(scanOptions.JobBuildPageCount);
        }

        /// <summary>
        /// Start Job after SetOptions step
        /// <param name="Print Job Execution">Options for running scan job</param>
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
                (new UiSelector().ResourceId($"{KioskPackageName}:id/billing_information_title"), 500, 120)))
            {
                SetTotalPrice();
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/popup_billing_information_ok_btn"));
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
        /// <param name="scanOptions">Options for running scan job</param>
        /// </summary>
        private void CopyScanExecution(int JobBuildPageCount)
        {
            bool result = false;

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear
                (new UiSelector().ResourceId($"{KioskPackageName}:id/start_tab_start_btn"), 500, 600))
            {
                if(!LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/start_tab_start_btn")))
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"Not display a start_tab_start_btn:: Kiosk");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                    throw e;
                }
                RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
            }

            Thread.Sleep(TimeSpan.FromSeconds(5));
            
            try
            {
                for (int i = 0; i < JobBuildPageCount; i++)
                {
                    Thread.Sleep(3000);
                    Device.ControlPanel.WaitForAvailable("#hpid-button-scan", TimeSpan.FromSeconds(10));
                    if (i == (JobBuildPageCount - 1))
                    {
                        UpdateStatus($"Remained Scan Job: JobBuildPageCount : {JobBuildPageCount}, iCount : {i}, DoneButton");
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        Device.ControlPanel.Press("#hpid-button-done");
                        Thread.Sleep(TimeSpan.FromSeconds(5));
                        if(!JetAdvantageLinkControlHelper.WaitingObjectDisappear
                            (new UiSelector().ResourceId($"{KioskPackageName}:id/jobProgressbar"), 500, 120))
                        {                            
                            DeviceWorkflowException e = new DeviceWorkflowException($"Job Progress Bar Error in Scan/Copy:: Kiosk");
                            e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                            throw e;
                        }                        
                        RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                    }
                    else if (i != (JobBuildPageCount - 1))
                    {
                        UpdateStatus($"Remained Scan Job: JobBuildPageCount : {JobBuildPageCount}, iCount : {i}, ScanButton");
                        Thread.Sleep(TimeSpan.FromSeconds(2));
                        Device.ControlPanel.Press("#hpid-button-scan");
                        CheckServerErrorPopup();
                    }
                }
            }
            catch (Exception ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Setting Job Build Count Copy failed (omni) {ex.ToString()} :: Kiosk", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear
                (new UiSelector().ResourceId($"{KioskPackageName}:id/billing_information_title"), 500, 120))
            {
                SetTotalPrice();
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/popup_billing_information_ok_btn"));
            }

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Execution Copy/Scan Job failed :: popup_billing_information_ok_btn");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Set the total price variable
        /// </summary>
        private void SetTotalPrice()
        {
            string totalPrice = null;
            Char delimiter = ')';

            totalPrice = LinkUI.Controller.GetText(new UiSelector().ResourceId("com.samsung.dpd.kiosk.ui.activity:id/billing_information_total_price"));

            if (!String.IsNullOrEmpty(totalPrice))
            {
                int startIndex = totalPrice.IndexOf(delimiter);
                _totalPrice = totalPrice.Substring(startIndex + 1).Trim();

                UpdateStatus($"The total price is {_totalPrice}");
            }
            else
            {
                UpdateStatus("The total price can not be found. Total price will remaind to 0.");
            }
        }

        /// <summary>
        /// Return total price for this tesing
        /// </summary>
        public string GetTotalPrice()
        {
            return _totalPrice;
        }

        /// <summary>
        /// SignOut 
        /// </summary>
        public void SignOut()
        {
            bool result = false;
            UpdateStatus("SignOut Start");
            RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/logoutBtn"), 500, 60))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/logoutBtn"));
            }

            if (result == JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{KioskPackageName}:id/confirm_cancel_btn"), 500, 60))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{KioskPackageName}:id/confirm_cancel_btn"));
            }

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"SignOut is failed :: Kiosk");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignOut.GetDescription());
                throw e;
            }
            RecordEvent(DeviceWorkflowMarker.DeviceSignOutEnd);
            UpdateStatus("SignOut Finish");
        }

        /// <summary>
        /// Dispose Cloud Connector
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
                DeviceWorkflowException e = new DeviceWorkflowException(ex.Message.ToString() + $" :: Kiosk", ex);
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
