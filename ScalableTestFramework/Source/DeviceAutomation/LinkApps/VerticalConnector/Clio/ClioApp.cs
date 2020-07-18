using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Enums;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Utility;
using HP.SPS.SES;
using HP.SPS.SES.Helper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.Clio
{
    /// <summary>
    /// Configuration to prepare job for Each Link Apps.
    /// </summary>
    public class ClioApp : DeviceWorkflowLogSource, IDisposable
    {
        private string _appName;
        private static string _exceptionCategoryData = "ExceptionCategory";
        private int _printPages;
        private int _inactivityTimeLimit;

        /// <summary>
        /// Package name of executing app
        /// </summary>
        public string ClioAppsPackageName;

        /// <summary>
        /// Control the JediOmniDevice
        /// </summary>
        public JediOmniDevice Device;

        /// <summary>
        /// Control the JetAdvantageLinkUI
        /// </summary>
        public JetAdvantageLinkUI LinkUI;

        /// <summary>
        /// Get the JetAdvantageLinkPrintOptionManager
        /// </summary>
        public JetAdvantageLinkPrintOptionManager PrintOptionManager;

        /// <summary>
        /// Get the JetAdvantageLinkScanOptionManager
        /// </summary>
        public JetAdvantageLinkScanOptionManager ScanOptionManager;

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
        /// </summary>
        /// <param name="device"></param>
        public ClioApp(IDevice device)
        {
            _appName = GetLaunchAppname();
            ClioAppsPackageName = $"com.hp.print.clio";

            LinkUI = new JetAdvantageLinkUI(device);
            PrintOptionManager = new JetAdvantageLinkPrintOptionManager(LinkUI, ClioAppsPackageName);
            ScanOptionManager = new JetAdvantageLinkScanOptionManager(LinkUI, ClioAppsPackageName);
            Device = (JediOmniDevice)device;
            JetAdvantageLinkControlHelper = new JetAdvantageLinkControlHelper(LinkUI, ClioAppsPackageName);
            _inactivityTimeLimit = GetInactivityTimeout();
        }

        /// <summary>
        /// Launches the LinkApps using App Name
        /// </summary>
        public void Launch(SIOMethod loginmethod)
        {
            string launchAppname = null;
            int timeOut = 0;
            DateTime startTime = DateTime.Now;

            try
            {
                launchAppname = GetLaunchAppname();
                UpdateStatus($"Launch App: {launchAppname}");

                Device.ControlPanel.ScrollPress($"#hpid-66a9808c-dfce-438a-9723-a8f9f017710d-homescreen-button");
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, launchAppname);

                Thread.Sleep(300);

                timeOut = LinkUI.Controller.GetTimeout();
                LinkUI.Controller.SetTimeout(0);

                if (!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().PackageName(ClioAppsPackageName), 200, 300))
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"Fail to launch app - First screen at the app is not displayed :: {_appName}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                    throw e;
                }

                UpdateStatus($"Waiting to launch the {ClioAppsPackageName} : Limitation time: {GetInactivityTimeout()} ms - It determined by InactivityTime Out");
                RecordEvent(DeviceWorkflowMarker.AppShown);

                if (JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/iv_dots_bg"), 500, 600, _inactivityTimeLimit))
                {
                    DateTime limitTime = DateTime.Now.AddMilliseconds(_inactivityTimeLimit);
                 
                    if (SIOMethod.SIOWithIDPWD.Equals(loginmethod))//Done -> Login Page
                    {
                        if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/rl_dialog_button_pane"), 200, 300, _inactivityTimeLimit))
                        {
                            LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_dialog_left"));
                        }
                    }
                    else if (SIOMethod.NoneSIO.Equals(loginmethod))// Eula -> Done ->Login Page
                    {
                        if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_continue"), 200 , 300 , _inactivityTimeLimit))
                        { 
                            CloseLicenseAgreementPopup();
                            limitTime = DateTime.Now.AddMilliseconds(_inactivityTimeLimit);
                        }
                        if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/rl_dialog_button_pane"), 200, 300, _inactivityTimeLimit))
                        {
                            LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_dialog_left"));
                        }
                    }
                }

                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().PackageName($"{ClioAppsPackageName}"), 500, 600, _inactivityTimeLimit))
                {
                    UpdateStatus($"Launch app completed - {DateTime.Now.Subtract(startTime).TotalSeconds} secs");
                }
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains(_exceptionCategoryData))
                {
                    throw;
                }
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to launch app :: {_appName}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                throw e;
            }
            finally
            {
                LinkUI.Controller.SetTimeout(timeOut);
            }
        }

        /// <summary>
        /// When popup - "HP App End User License Agreement" will be opened, close this popup.
        /// </summary>
        private void CloseLicenseAgreementPopup()
        {
            int timeout = 0;
            timeout = LinkUI.Controller.GetTimeout();
            LinkUI.Controller.SetTimeout(0);

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_continue"), 200, 300))
            {
                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/cb_agree"), 200, 300))
                {
                    LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/cb_agree"));
                }

                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_continue"), 200, 300))
                {
                    LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_continue"));
                }
            }

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/rl_dialog_button_pane"), 200, 300))
            {
                LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_dialog_left"));
            }
            LinkUI.Controller.SetTimeout(timeout);
        }

        /// <summary>
        /// Get Inactivity Timeout (ms)
        /// </summary>
        private int GetInactivityTimeout()
        {
            int inactivityTimeOut = 0;
            string activityUrn = "urn:hp:imaging:con:service:uiconfiguration:UIConfigurationService";
            string endpoint = "uiconfiguration";

            WebServiceTicket tic = Device.WebServices.GetDeviceTicket(endpoint, activityUrn);

            if (!Int32.TryParse(tic.FindElement("InactivityTimeoutInSeconds").Value, out inactivityTimeOut))
            {
                UpdateStatus("Get Inactivity Timeout failed. It will use 60 for Inactivity Timeout.");
                inactivityTimeOut = 60;
            }

            return inactivityTimeOut * 1000;
        }

        /// <summary>
        /// Start Print job and check job finish
        /// </summary>
        /// <param name="printCount">count for print</param>
        public void ExecutionPrintJob(int printCount)
        {

            int timeOut = 0;

            bool result = true;

            UpdateStatus($"Print Job started with pages documents :: {_appName}");
            timeOut = LinkUI.Controller.GetTimeout();

            RecordEvent(DeviceWorkflowMarker.PrintJobBegin);
            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/btn_print"));

            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);

            result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/btn_actions"), 500, 200, _inactivityTimeLimit);
            if (result)
            {
                RecordEvent(DeviceWorkflowMarker.PrintJobEnd);
            }
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
            LinkUI.Controller.SetTimeout(timeOut);

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Execution Print Job failed :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Start Scan job and check job finish
        /// <param name="pagecount">Options of scan build job</param>
        /// <param name="originalsides">Options of scan build job</param>
        /// </summary>
        public void ExecutionScanJob(int pagecount, LinkScanOriginalSides originalsides)
        {
            bool result = true;
            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_send"));

            Thread.Sleep(TimeSpan.FromSeconds(1));
            CheckServerErrorPopup();

            Thread.Sleep(TimeSpan.FromSeconds(1));
            HandleJediOmniPopup("#hpid-prompt-flatbed-autodetect", "#hpid-button-continue");

            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to execution scan job - Fail to click the scan button :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }

            string flag = originalsides.GetDescription();
            UpdateStatus($"{flag} Scan Job started :: {_appName}");

            if (result)
            {
                switch (originalsides)
                {
                    case (LinkScanOriginalSides.Onesided):
                        ScanOption_OnesidedJob();
                        break;
                    case (LinkScanOriginalSides.Twosided):
                    case (LinkScanOriginalSides.Pagesflipup):
                        ScanOption_TwosidedJob(pagecount);
                        break;
                    default:
                        DeviceWorkflowException e = new DeviceWorkflowException($"Unrecognized Original sides on Scan job:: {_appName}");
                        e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.FalseAlarm.GetDescription());
                        throw e;
                }
            }

        }

        /// <summary>
        /// SignOut 
        /// </summary>
        public void SignOut(LogOutMethod logoutmethod)
        {
            UpdateStatus("SignOut Start");

            string reason = $"Fail to sign out :: {_appName}";
            bool result = true;
            
            RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);

            UpdateStatus($"logoutmethod :: {logoutmethod}");
            if (logoutmethod.Equals(LogOutMethod.PressBackKey))
            {
                while (!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_dialog_right"), 200, 20))
                {
                    result &= LinkUI.Controller.PressKey(KeyCode.KEYCODE_BACK);
                    Thread.Sleep(1000);
                }
                result &= LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_dialog_left"));
            }
            else if (logoutmethod.Equals(LogOutMethod.PressSignOut))
            {
                while(!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/btn_more"), 200, 20))
                {
                    result &= LinkUI.Controller.PressKey(KeyCode.KEYCODE_BACK);
                    Thread.Sleep(1000);
                }
                result &= LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/btn_more"));
                result &= LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/action_sign_out"));
                result &= LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_dialog_left"));
            }
            Thread.Sleep(3000);
            CheckServerErrorPopup();

            if (result)
            {
                try
                {
                    result &= Device.ControlPanel.WaitForState($".hp-homescreen-folder-view[hp-global-top-view=true]", OmniElementState.Exists, TimeSpan.FromSeconds(10));
                    result &= Device.ControlPanel.CheckState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely);
                }
                catch (Exception ex)
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"Fail to sign out - Fail to return the home screen after sign out {ex.Message} :: {_appName}", ex);
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignOut.GetDescription());
                    throw e;
                }
            }
            else
            {
                reason = $"Fail to sign out - Can not press sign out on the app :: {_appName}";
            }

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException(reason);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignOut.GetDescription());
                throw e;
            }
            RecordEvent(DeviceWorkflowMarker.DeviceSignOutEnd);
            UpdateStatus("SignOut Finish");

        }

        /// <summary>
        /// Run an OneSidedJob of scan job
        /// </summary>
        private void ScanOption_OnesidedJob()
        {
            string reason = $"Fail to execution One Sided Scan Job";

            int timeOut = 0;
            int testCount = 0;
            bool result = true;
            bool autoDetectPopup = false;

            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(0);

            CheckServerErrorPopup();
            while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/pb_dialog_progress")) || LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/ll_dots")))
            {
                if (!autoDetectPopup)
                {
                    autoDetectPopup = HandleJediOmniPopup("#hpid-prompt-flatbed-autodetect", "#hpid-button-continue");
                }

                if (testCount > 150)
                {
                    result = false;
                    break;
                }

                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_contents")))
                {
                    string tv_contents = LinkUI.Controller.GetText(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_contents"));

                    UpdateStatus($"Scan job status: {tv_contents}");

                    if (string.IsNullOrEmpty(tv_contents) || (!tv_contents.ToLower().Contains("scanning")))
                    {
                        break;
                    }
                }

                testCount++;
                Thread.Sleep(300);
            }

            if (result)
            {
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                RecordEvent(DeviceWorkflowMarker.SendingJobBegin);

                testCount = 0;
                CheckServerErrorPopup();
                while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/pb_dialog_progress")))
                {
                    if (!autoDetectPopup)
                    {
                        autoDetectPopup = HandleJediOmniPopup("#hpid-prompt-flatbed-autodetect", "#hpid-button-continue");
                    }

                    if (testCount > 600)
                    {
                        result = false;
                        break;
                    }

                    Thread.Sleep(500);
                }
                RecordEvent(DeviceWorkflowMarker.SendingJobEnd);
                RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
                CheckServerErrorPopup();
            }
            else
            {
                reason = $"Fail to One Sided Scan Job - The \"sending\" status is not found pn screen";
            }

            LinkUI.Controller.SetTimeout(timeOut);

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException(reason);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Run a TwoSidedJob of scan job
        /// </summary>
        /// <param name="ScanCount">count for scan</param>
        private void ScanOption_TwosidedJob(int ScanCount)
        {
            string reason = $"Fail to execution Two Sided Scan Job :: {_appName}";

            int timeOut = 0;
            int testCount = 0;

            bool result = true;
            bool autoDetectPopup = false;

            timeOut = LinkUI.Controller.GetTimeout();

            try
            {
                for (int i = 0; i < ScanCount; i++)
                {
                    if (i == 0)
                    {
                        while (!Device.ControlPanel.WaitForAvailable("#hpid-button-scan", TimeSpan.FromSeconds(1)))
                        {
                            autoDetectPopup = HandleJediOmniPopup("#hpid-prompt-flatbed-autodetect", "#hpid-button-continue");

                            testCount++;

                            if (testCount >= 20)
                            {
                                break;
                            }
                        }
                    }
                    else
                    {
                        Device.ControlPanel.WaitForAvailable("#hpid-button-scan", TimeSpan.FromSeconds(20));
                    }

                    CheckServerErrorPopup();

                    if (i == (ScanCount - 1))
                    {
                        UpdateStatus($"Scan completed: {i + 1} of {ScanCount.ToString()}");
                        Device.ControlPanel.Press("#hpid-button-done");
                        RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                        RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
                    }
                    else if (i != (ScanCount - 1))
                    {
                        UpdateStatus($"Scanning page {i + 1} of {ScanCount.ToString()}");
                        Device.ControlPanel.Press("#hpid-button-scan");
                    }
                }
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains(_exceptionCategoryData))
                {
                    throw;
                }
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to Two Sided Scan job - \"Scan more\" screen is not displayed :: {_appName}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }

            RecordEvent(DeviceWorkflowMarker.SendingJobBegin);
            LinkUI.Controller.SetTimeout(0);
            CheckServerErrorPopup();
            testCount = 0;


            while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/pb_dialog_progress")))
            {
                if (testCount > 300 * ScanCount)
                {
                    result = false;
                    break;
                }

                testCount++;
                Thread.Sleep(200);
            }

            if (result)
            {
                CheckServerErrorPopup();
                LinkUI.Controller.SetTimeout(123);
                result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/btn_more"));

                if (!result)
                {
                    reason = $"Fail to Two Sided Scan Job - Screen is not return to prior screen after scan job :: {_appName}";
                }

                RecordEvent(DeviceWorkflowMarker.SendingJobEnd);
            }
            else
            {
                reason = $"Fail to Two Sided Scan Job - The progress circle is not disappear :: {_appName}";
            }
            LinkUI.Controller.SetTimeout(timeOut);
            CheckServerErrorPopup();

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException(reason);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Get App Name from ConnectorDisplayName enum
        /// </summary>
        private static string GetLaunchAppname()
        {
            string appName = "HP for Clio App";
            return appName;
        }

        /// <summary>
        /// Do login the LinkApps using ID/PW
        /// </summary>
        /// <param name="id">ID for doing login the application</param>
        /// <param name="pw">PW for doing login the application</param>        
        public void Login(string id, string pw)
        {
            int timeOut = LinkUI.Controller.GetTimeout();
            bool result = false;
            DateTime startTime = DateTime.Now;

            RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);
            try
            {
                result = JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/iv_dots_bg"), 500, 80);

                if (!result)
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"Fail to find progress bar");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                    throw e;
                }

                Thread.Sleep(TimeSpan.FromSeconds(3));
                using (WebviewObject loginPanel = LinkUI.Controller.GetWebView())
                {
                    if (loginPanel.IsExist("//*[@name=\"email\"]", TimeSpan.FromSeconds(30)))
                    {
                        RecordEvent(DeviceWorkflowMarker.EnterCredentialsBegin);
                        UpdateStatus($"id = {id}, pwd = {pw}");
                        result = loginPanel.SetText("//*[@name=\"email\"]", id, false);
                        if (result)
                        {
                            result = loginPanel.SetText("//*[@name=\"password\"]", pw, false);
                        }
                        RecordEvent(DeviceWorkflowMarker.EnterCredentialsEnd);
                        if (result)
                        {
                            result = loginPanel.Click("//*[@id=\"submit\"]");
                        }

                        if (result)
                        {
                            result = loginPanel.Click("//*[@id=\"main\"]/div[1]/div/span/th-row/th-column[2]/div/div[2]/div/form[1]/th-button/button", TimeSpan.FromSeconds(20));
                        }
                    }
                    else
                    {
                        result = false;
                    }
                }
            }
            catch (NullReferenceException ex)
            {
                DeviceCommunicationException e = new DeviceCommunicationException("Fail to get webview - You need check used hpk file is \"webview debuggable option\" enabled", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.EnvironmentError.GetDescription());
                throw e;
            }
            if (result)
            {
                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/buttonClose")))
                {
                    CloseDetailPopup();
                }
                CloseAppGuide();
                CheckServerErrorPopup();
                RecordEvent(DeviceWorkflowMarker.AuthenticationEnd);
                UpdateStatus($"Sign in completed - {DateTime.Now.Subtract(startTime).TotalSeconds} secs");
                LinkUI.Controller.SetTimeout(_inactivityTimeLimit / 1000);
                //ms => second
                LinkUI.Controller.SetTimeout(timeOut);
            }
            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to log in by ({id}) :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                throw e;
            }
            RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// BacktoHomeScreen
        /// </summary>
        public void BacktoHomeScreen()
        {
            UpdateStatus("BacktoHomeScreen Start");

            int count = 0;
            while (!LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_send")))
            {
                if (count > 10)
                {
                    LinkUI.Controller.PressKey(4);
                    Thread.Sleep(3000);
                    return;
                }
                count++;
            }
        }

        /// <summary>
        /// Select a storage
        /// </summary>
        /// <param name="storageName"></param>
        public void SelectStorageLocation(StorageLocation storageName)
        {
            bool result = false;
            string errorMessage = "iv_spinner_dropdown_icon";
            int timeOut = LinkUI.Controller.GetTimeout();

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/iv_spinner_dropdown_icon"), 500, 30, _inactivityTimeLimit))
            {
                LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/iv_spinner_dropdown_icon"));
                switch (storageName)
                {
                    case StorageLocation.RecentMatters:
                        result = LinkUI.Controller.Click(new UiSelector().Text("Recent matters"));
                        break;
                    case StorageLocation.Matters:
                        result = LinkUI.Controller.Click(new UiSelector().Text("Matters"));
                        break;
                    case StorageLocation.Document:
                        result = LinkUI.Controller.Click(new UiSelector().Text("Documents"));
                        break;
                    default:
                        DeviceWorkflowException e = new DeviceWorkflowException($"Error while selecting storage :: {storageName.GetDescription()}");
                        e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                        throw e;
                }
                errorMessage = storageName.GetDescription();
                Thread.Sleep(TimeSpan.FromSeconds(10));
            }
            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Storage select failed <{errorMessage}>");
                //e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory..GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Navigate the location to save scan file(to select print file)
        /// <param name="matter">Matter name to select</param>
        /// </summary>
        public void SelectMatter(string matter)
        {
            bool result = false;
            int timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(30);

            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_display_number").TextContains(matter)))
            {
                UpdateStatus($"Click the {matter} by using displayed list");
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_display_number").TextContains(matter));
                LinkUI.Controller.SetTimeout(timeOut);
            }

            if (!result)
            {
                DeviceWorkflowException ex = new DeviceWorkflowException($"Fail to select the Matter: {matter}");
                ex.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                throw ex;
            }
        }

        /// <summary>
        /// Navigate the location to save scan file(to select print file)
        /// <param name="filePath">Navigate to destination by FilePath</param>
        /// <param name="jobType">ClioJobType</param>
        /// </summary>
        public void NavigateToDestination(string filePath, ClioJobType jobType)
        {
            //Start - Init_Values
            string current_string = null;
            //Click Folder/File For navigating
            string filename = null;
            //parsed Filename/foldername
            string path = filePath;
            //Folder/File Path
            path = path.Trim();
            //Remove whitespace in front/end of path
            //It will return "abc def" if path is "abc def"
            //It will return "abc def" if path is " abc def" or "abc def "
            //End - Init_Values

            //Using Flag-"/", Parse Item to click for moving to destination
            while (path.Contains("/"))
            {
                current_string = path.Substring(0, path.IndexOf('/'));
                path = path.Substring(path.IndexOf('/') + 1);
                filename = current_string;
                SearchForParsedText(filename);
                //Search and Click Parsed Item on the list by _filename                
            }

            if (!String.IsNullOrEmpty(path))
            {
                filename = path;
                UpdateStatus($"Final filename is  :: {filename}  {_appName}");
                //Final Destination(Scan : Final Folder, Print : Print Filename)
                SearchForParsedText(filename);
            }
        }

        /// <summary>
        /// Press Scan Button
        /// </summary>
        public void PressScanButton()
        {
            bool result = false;
            int timeOut = LinkUI.Controller.GetTimeout();
            LinkUI.Controller.SetTimeout(10);

            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/btn_scan")))
            {
                UpdateStatus($"Click the Scan Button");
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/btn_scan"));
                LinkUI.Controller.SetTimeout(timeOut);
            }

        }

        /// <summary>
        /// Search a parsed item(file/folder name) on the searchtab.
        /// <param name="fileName">Scan File Name</param>
        /// </summary>
        public void ScanDocumentOptions(string fileName)
        {
            try
            {
                if (!String.IsNullOrEmpty(fileName) && JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/et_filename"), 500, 30, _inactivityTimeLimit))
                {
                    LinkUI.Controller.SetText(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/et_filename"), fileName);
                }
                Thread.Sleep(TimeSpan.FromSeconds(2));
            }
            catch (Exception)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Cannot Select Scan File name :: {fileName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Search a parsed item(file/folder name) on the searchtab.
        /// <param name="ParsedItem">Parsed Item from FilePath</param>
        /// </summary>
        public void SearchForParsedText(string ParsedItem)
        {
            bool result = true;

            int timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(0);

            JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/layout_docitem_row"), 200, 50);

            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_description").TextContains(ParsedItem)))
            {
                UpdateStatus($"Click the {ParsedItem} by using displayed list");
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_description").TextContains(ParsedItem));
                LinkUI.Controller.SetTimeout(timeOut);
            }
            else if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_file_column_info").TextContains(ParsedItem)))
            {
                UpdateStatus($"Click the {ParsedItem} by using displayed list");
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_file_column_info").TextContains(ParsedItem));
                LinkUI.Controller.SetTimeout(timeOut);
            }
            else
            {
                UpdateStatus($"Click the {ParsedItem} by using Search text box");
                LinkUI.Controller.SetTimeout(timeOut);

                if (result && (result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/searchImageBtn"), 500, 60, _inactivityTimeLimit)))
                {
                    result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/searchImageBtn"));
                }

                if (result && (result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/searchTextView"), 500, 60, _inactivityTimeLimit)))
                {
                    result = LinkUI.Controller.SetText(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/searchTextView"), ParsedItem);
                }

                if (result && (result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_display_first"), 500, 60, _inactivityTimeLimit)))
                {
                    result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_display_first").TextContains(ParsedItem));
                }
            }

            CheckServerErrorPopup();

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to click objects for navigate to specific path ({ParsedItem}) :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                throw e;
            }
        }


        /// <summary>
        /// Move to Option Page(scan or print) after completing a navigation to the destination
        /// <param name="JobType">scan or print</param>
        /// </summary>
        public void SelectJobForSetOptions(ClioJobType JobType)
        {
            bool result = false;
            switch (JobType)
            {
                case ClioJobType.Scan:
                    result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/bt_hide_options"));
                    break;
                case ClioJobType.Print:
                    IsJobOptionScreenToPrint(JobType);
                    _printPages = GetPrintPages();
                    result = true;
                    break;
                default:
                    DeviceWorkflowException e = new DeviceWorkflowException($"Unrecognized Connector Job Type: {JobType.GetDescription()} :: {_appName}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.FalseAlarm.GetDescription());
                    throw e;
            }
            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Current Screen is not Job options page after navigation ({JobType.GetDescription()}):: {_appName} ");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SelectOptions.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// GetPrintPages 
        /// </summary>
        private int GetPrintPages()
        {
            int pageCount = 0, totalPages = 0;
            string parsingText = null;

            if (!LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_page_count")))
            {
                parsingText = "1";
                UpdateStatus("This pageCount is 1. That's why file format is txt");
            }
            else
            {
                parsingText = LinkUI.Controller.GetText(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_page_count"));
                parsingText = parsingText.Substring(parsingText.IndexOf('f') + 1);
            }

            if (!Int32.TryParse(parsingText, out pageCount))
            {
                UpdateStatus("Get Page Count failed. it will use 1 for page counts.");
                pageCount = 1;
            }
            totalPages = pageCount;
            UpdateStatus($"Calculated printing pages is {totalPages}");

            return totalPages;
        }

        /// <summary>
        /// Check that current UI is Job Option status or not.
        /// </summary>
        public void IsJobOptionScreenToPrint(ClioJobType JobType)
        {
            string reason = $"Fail to check UI screen for printing job :: {_appName}";
            bool result = true;
            int timeOut = 0;

            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);

            timeOut = LinkUI.Controller.GetTimeout();
            LinkUI.Controller.SetTimeout(0);

            CheckServerErrorPopup();
            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerEnd);
            LinkUI.Controller.SetTimeout(60);

            if (result)
            {
                result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/btn_print"));
                // Check that Downloading is completed and Current UI is Job Option Screen on the print section
                if (!result)
                {
                    reason = $"Fail to check UI screen for printing job - Print button is not displayed :: {_appName} ";
                }
            }

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException(reason);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.DownloadingPrintFile.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Dispose Clio
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
        /// Updates the status of the plugin execution.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void UpdateStatus(string status)
        {
            LogInfo(status);
            ActivityStatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }

        /// <summary>
        /// Close App guide page
        /// </summary>
        private void CloseAppGuide()
        {
            int timeOut = 0;

            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(5);
            LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/checkBoxDoNotShowAgain"));

            LinkUI.Controller.SetTimeout(0);
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/checkBoxDoNotShowAgain"));
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/buttonClose"));
            LinkUI.Controller.SetTimeout(timeOut);
        }

        /// <summary>
        /// When popup - "Do not show again" will be opened, close this popup.
        /// </summary>
        private void CloseDetailPopup()
        {
            int timeOut = 0;

            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(5);
            LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/buttonClose"));
            LinkUI.Controller.SetTimeout(0);
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/buttonClose"));
            LinkUI.Controller.SetTimeout(timeOut);
        }

        /// <summary>
        /// Check an Error Popup(Server Error)
        /// </summary>
        private void WaitDownlodingPopup()
        {
            int timeOut = 0;
            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(0);

            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/iv_dialog_icon")) && LinkUI.Controller.GetText(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_contents")).Contains("error"))
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Server Error Popup {LinkUI.Controller.GetText(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_contents"))}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ServerError.GetDescription());
                throw e;
            }

            LinkUI.Controller.SetTimeout(timeOut);
        }

        /// <summary>
        /// Check an Error Popup(Server Error)
        /// </summary>
        private void CheckServerErrorPopup()
        {
            int timeOut = 0;
            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(0);

            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/iv_dialog_icon")) && LinkUI.Controller.GetText(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_contents")).Contains("Unable"))
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Server Error Popup {LinkUI.Controller.GetText(new UiSelector().ResourceId($"{ClioAppsPackageName}:id/tv_contents"))}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ServerError.GetDescription());
                throw e;
            }
            LinkUI.Controller.SetTimeout(timeOut);
        }

        /// <summary>
        /// Handle Jediomni popup
        /// </summary>
        /// <param name="popupSelector"> Method Press the buttonSelector when this variable is exist </param>
        /// <param name="buttonSelector"> Selector for Press Button </param>
        /// <returns></returns>
        private bool HandleJediOmniPopup(string popupSelector, string buttonSelector)
        {
            if (Device.ControlPanel.CheckState(popupSelector, OmniElementState.Exists) && Device.ControlPanel.WaitForState(buttonSelector, OmniElementState.Useable))
            {
                UpdateStatus($"{popupSelector} is displayed. Press {buttonSelector}");
                Device.ControlPanel.Press(buttonSelector);
                return true;
            }
            else
            {
                return false;
            }
        }
    }
}
