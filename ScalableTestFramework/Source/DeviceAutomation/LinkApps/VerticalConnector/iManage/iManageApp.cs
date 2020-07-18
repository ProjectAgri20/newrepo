using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Enums;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Utility;
using HP.SPS.SES;
using HP.SPS.SES.Helper;
using System;
using System.Threading;
using static HP.ScalableTest.Framework.Logger;
using static HP.SPS.SES.SESLib;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.iManage
{
    /// <summary>
    /// Configuration to prepare job for Each Link Apps.
    /// </summary>
    public class iManageApp : DeviceWorkflowLogSource, IDisposable
    {
        private string _appName;
        private static string _exceptionCategoryData = "ExceptionCategory";
        private int _printPages, _inactivityTimeLimit;
        /// <summary>
        /// Package name of executing app
        /// </summary>
        public string iManageAppsPackageName;

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
        public iManageApp(IDevice device)
        {
            _appName = GetLaunchAppname();
            iManageAppsPackageName = $"com.hp.imanageconnector";

            LinkUI = new JetAdvantageLinkUI(device);
            PrintOptionManager = new JetAdvantageLinkPrintOptionManager(LinkUI, iManageAppsPackageName);
            ScanOptionManager = new JetAdvantageLinkScanOptionManager(LinkUI, iManageAppsPackageName);
            Device = (JediOmniDevice)device;
            JetAdvantageLinkControlHelper = new JetAdvantageLinkControlHelper(LinkUI, iManageAppsPackageName);
            _inactivityTimeLimit = GetInactivityTimeout();
        }

        /// <summary>
        /// Launches the iManage
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

                Device.ControlPanel.ScrollPress("#hpid-a92bf006-1b76-474f-870b-e80d35921781-homescreen-button");
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, launchAppname);

                Thread.Sleep(300);

                timeOut = LinkUI.Controller.GetTimeout();
                LinkUI.Controller.SetTimeout(0);

                if (!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().PackageName(iManageAppsPackageName), 200, 300))
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"Fail to launch app - First screen at the app is not displayed :: {_appName}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                    throw e;
                }

                UpdateStatus($"Waiting to launch the {iManageAppsPackageName} : Limitation time: {GetInactivityTimeout()} ms - It determined by InactivityTime Out");
                RecordEvent(DeviceWorkflowMarker.AppShown);

                if (JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/pb_dialog_progress"), 500, 600, _inactivityTimeLimit))
                {
                    DateTime limitTime = DateTime.Now.AddMilliseconds(_inactivityTimeLimit);

                    if (SIOMethod.SIOWithIDPWD.Equals(loginmethod))//Done -> Login Page
                    {
                        if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/rl_dialog_button_pane"), 200, 300, _inactivityTimeLimit))
                        {
                            LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_dialog_left"));
                        }
                    }
                    else if (SIOMethod.NoneSIO.Equals(loginmethod))// Eula -> Done ->Login Page
                    {
                        if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_continue"), 200, 300, _inactivityTimeLimit))
                        {
                            CloseLicenseAgreementPopup();
                            limitTime = DateTime.Now.AddMilliseconds(_inactivityTimeLimit);
                        }
                        if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/rl_dialog_button_pane"), 200, 300, _inactivityTimeLimit))
                        {
                            LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_dialog_left"));
                        }
                    }
                }

                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().PackageName($"{iManageAppsPackageName}"), 500, 600, _inactivityTimeLimit))
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
                DeviceWorkflowException e = new DeviceWorkflowException($"Launch failed :: {_appName}", ex);
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

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_continue"), 200, 300))
            {
                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/cb_agree"), 200, 300))
                {
                    LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/cb_agree"));
                }

                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_continue"), 200, 300))
                {
                    LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_continue"));
                }
            }

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/rl_dialog_button_pane"), 200, 300))
            {
                LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_dialog_left"));
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
        public void ExecutionPrintJob()
        {

            int timeOut = 0;

            bool result = true;

            UpdateStatus($"Print Job started with pages documents :: {_appName}");
            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(20);
            RecordEvent(DeviceWorkflowMarker.PrintJobBegin);
            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/btn_print"));
            // Job Start

            RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);

            result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/iv_more"), 500, 120, _inactivityTimeLimit);
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
        /// <param name="UseOriginalSides">Bool for using OriginalSides</param>
        /// </summary>
        public void ExecutionScanJob(int pagecount, LinkScanOriginalSides originalsides, bool UseOriginalSides = false)
        {
            bool result = true;
            bool scanresult = false;
            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_send"));

            Thread.Sleep(TimeSpan.FromSeconds(1));
            CheckServerErrorPopup();

            Thread.Sleep(TimeSpan.FromSeconds(1));
            HandleJediOmniPopup("#hpid-prompt-flatbed-autodetect", "#hpid-button-continue");

            if (UseOriginalSides)
            {
                RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
            }
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Clicking Scan button failed :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }

            string flag = originalsides.GetDescription();
            UpdateStatus($"{flag} Scan Job started :: {_appName}");

            if (UseOriginalSides)
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
            else
            {
                ScanOption_OnesidedJob();
            }

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/iv_filter"), 500, 120, _inactivityTimeLimit))
            {
                scanresult = true;
                UpdateStatus("Scan Complete!");
            }

            if (!scanresult)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Scan failed :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }

        }

        /// <summary>
        /// SignOut 
        /// </summary>
        public void SignOut(LogOutMethod logoutmethod)
        {
            UpdateStatus("SignOut Start");

            bool result = true;

            RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);

            if (logoutmethod.Equals(LogOutMethod.PressSignOut))
            {
                if (JetAdvantageLinkControlHelper.WaitingObjectAppear($"//*[@resource-id='{iManageAppsPackageName}:id/iv_more']", 200, 100))
                {
                    result &= LinkUI.Controller.Click($"//*[@resource-id='{iManageAppsPackageName}:id/iv_more']");
                    result &= LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/action_sign_out"));
                    result &= LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_dialog_left"));
                }
            }
            Thread.Sleep(3000);
            CheckServerErrorPopup();

            try
            {
                result &= Device.ControlPanel.WaitForState($".hp-homescreen-folder-view[hp-global-top-view=true]", OmniElementState.Exists, TimeSpan.FromSeconds(10));
                result &= Device.ControlPanel.CheckState("#hpid-homescreen-logo-icon", OmniElementState.VisibleCompletely);
            }
            catch (Exception ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Sign out failed (omni) {ex.ToString()} :: {_appName}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignOut.GetDescription());
                throw e;
            }
            

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"SignOut is failed :: {_appName}");
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
            int timeOut = 0;
            int testCount = 0;
            bool result = true;
            bool autoDetectPopup = false;

            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(0);

            CheckServerErrorPopup();

            while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/pb_dialog_progress")) || LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/ll_dots")))
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

                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/tv_contents")))
                {
                    string tv_contents = LinkUI.Controller.GetText(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/tv_contents"));

                    UpdateStatus($"Scan job status: {tv_contents}");

                    if (string.IsNullOrEmpty(tv_contents) || (!tv_contents.ToLower().Contains("scanning")))
                    {
                        break;
                    }
                }
                testCount++;
                Thread.Sleep(200);
            }

            if (result)
            {
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                RecordEvent(DeviceWorkflowMarker.SendingJobBegin);

                testCount = 0;
                CheckServerErrorPopup();

                while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/pb_dialog_progress")))
                {
                    if (!autoDetectPopup)
                    {
                        autoDetectPopup = HandleJediOmniPopup("#hpid-prompt-flatbed-autodetect", "#hpid-button-continue");
                    }

                    if (testCount > 100)
                    {
                        result = false;
                        break;
                    }

                    if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/toolbar")))
                    {
                        break;
                    }

                    testCount++;
                    Thread.Sleep(300);
                }
                CheckServerErrorPopup();
                RecordEvent(DeviceWorkflowMarker.SendingJobEnd);
            }

            LinkUI.Controller.SetTimeout(timeOut);

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Execution One Sided Scan Job failed :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Run a TwoSidedJob of scan job
        /// </summary>
        /// <param name="ScanCount"></param>
        private void ScanOption_TwosidedJob(int ScanCount)
        {
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

                    Thread.Sleep(3000);
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
                DeviceWorkflowException e = new DeviceWorkflowException($"Two Sided Scan failed (omni) :: {_appName}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }

            LinkUI.Controller.SetTimeout(0);

            if (result)
            {
                RecordEvent(DeviceWorkflowMarker.SendingJobBegin);
                CheckServerErrorPopup();
                testCount = 0;

                while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/pb_dialog_progress")))
                {
                    if (testCount > 100)
                    {
                        result = false;
                        break;
                    }

                    if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/toolbar")))
                    {
                        break;
                    }

                    testCount++;
                    Thread.Sleep(300);
                }
                CheckServerErrorPopup();
                RecordEvent(DeviceWorkflowMarker.SendingJobEnd);
            }

            LinkUI.Controller.SetTimeout(timeOut);

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Execution Two Sided Scan Job failed :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Get App Name the "iManage"
        /// </summary>
        private static string GetLaunchAppname()
        {
            return "HP for iManage App";
        }

        /// <summary>
        /// Do login the LinkApps using ID/PW
        /// </summary>
        /// <param name="id">ID for doing login the application</param>
        /// <param name="password">PW for doing login the application</param>        
        public void Login(string id, string password)
        {
            int timeOut = LinkUI.Controller.GetTimeout();
            DateTime startTime = DateTime.Now;
            RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);
            RecordEvent(DeviceWorkflowMarker.EnterCredentialsBegin);

            // Input ID
            if (!String.IsNullOrEmpty(id) && LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/login_id")))
            {
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/login_id"), id))
                {
                    throw new DeviceWorkflowException($"Fail to set the From field: {id}");
                }
            }

            // Input PW
            if (!String.IsNullOrEmpty(password) && LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/login_password")))
            {

                if (LinkUI.Controller.Click("//*[@resource-id=\'com.hp.imanageconnector:id/login_password\']"))
                {
                    if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/login_password"), password))
                    {
                        throw new DeviceWorkflowException($"Fail to set the From field: {password}");
                    }
                }

                if (LinkUI.Controller.IsVirtualKeyboardShown().Equals(true))
                {
                    LinkUI.Controller.PressKey(KeyCode.KEYCODE_ESCAPE);
                }
            }
            RecordEvent(DeviceWorkflowMarker.EnterCredentialsEnd);

            // Sign In
            if (!LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/button")))
            {
                throw new DeviceWorkflowException($"Fail to click the Add button");
            }
            RecordEvent(DeviceWorkflowMarker.AuthenticationEnd);
            UpdateStatus($"Sign in completed - {DateTime.Now.Subtract(startTime).TotalSeconds} secs");

            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/buttonClose")))
            {
                CloseDetailPopup();
            }

            CloseAppGuide();
            CheckServerErrorPopup();

            LinkUI.Controller.SetTimeout(_inactivityTimeLimit / 1000);
            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/user_name"), 500, 600, _inactivityTimeLimit))
            {
                UpdateStatus($"Login completed: {LinkUI.Controller.GetText(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/user_name"))}");
            }
            else
            {
                throw new DeviceWorkflowException($"Fail to sign in");
            }
            Thread.Sleep(10000);
            //ms => second
            LinkUI.Controller.SetTimeout(timeOut);
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

            if (LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/iv_spinner_dropdown_icon")))
            {
                Thread.Sleep(300);
                switch (storageName)
                {
                    case StorageLocation.Document:
                        result = LinkUI.Controller.Click(new UiSelector().Text("Documents"));
                        break;
                    case StorageLocation.Matter:
                        result = LinkUI.Controller.Click(new UiSelector().Text("Matters"));
                        break;
                    case StorageLocation.MyFavorite:
                        result = LinkUI.Controller.Click(new UiSelector().Text("My favorites"));
                        break;
                    default:
                        DeviceWorkflowException e = new DeviceWorkflowException($"Error while selecting storage :: {storageName.GetDescription()}");
                        e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                        throw e;
                }
                errorMessage = storageName.GetDescription();
                Thread.Sleep(TimeSpan.FromSeconds(3));
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
        /// <param name="filePath">Navigate to destination by FilePath</param>a
        /// <param name="jobType">IManageJobType</param>a
        /// </summary>
        public void NavigateToDestination(string filePath, iManageJobType jobType)
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
                ClickForParsedText(filename);
                //Search and Click Parsed Item on the list by _filename                
            }

            if (!String.IsNullOrEmpty(path))
            {
                filename = path;
                UpdateStatus($"Final filename is  :: {filename}  {_appName}");
                //Final Destination(Scan : Final Folder, Print : Print Filename)
                ClickForParsedText(filename);
            }

            if (jobType == iManageJobType.Scan)//iv_filter
            {
                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/iv_filter"), 500, 60, _inactivityTimeLimit))
                {
                    LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/btn_scan"));
                }
                else
                {
                    throw new DeviceWorkflowException($"Can not found Send button");
                }

            }
        }

        /// <summary>
        /// Search a parsed item(file/folder name) on the searchtab.
        /// <param name="ParsedItem">Parsed Item from FilePath</param>
        /// </summary>
        public void ClickForParsedText(string ParsedItem)
        {
            bool result = true;

            if (result && (result = JetAdvantageLinkControlHelper.FindOnListWithScroll(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/rv_matterList"), new UiSelector().TextContains(ParsedItem), 10)))
            {
                result = JetAdvantageLinkControlHelper.ClickOnListWithScroll(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/rv_matterList"), new UiSelector().TextContains(ParsedItem), 10);
            }
            Thread.Sleep(TimeSpan.FromSeconds(3));

            CheckServerErrorPopup();
            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"SearchForParsedText failed ({ParsedItem}) :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
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

            if (result && (result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/iv_search"), 500, 60, _inactivityTimeLimit)))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/iv_search"));
            }

            if (result && (result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/searchTextView"), 500, 60, _inactivityTimeLimit)))
            {
                result = LinkUI.Controller.SetText(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/searchTextView"), ParsedItem);
            }

            if (result && (result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/iv_icon"), 500, 60, _inactivityTimeLimit)))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/iv_icon"));
            }

            if (result && (result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_dialog_left"), 500, 60, _inactivityTimeLimit)))
            {
                JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_dialog_left"), 500, 60, _inactivityTimeLimit);
            }

            CheckServerErrorPopup();

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"SearchForParsedText failed ({ParsedItem}) :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Move to Option Page(scan or print) after completing a navigation to the destination
        /// <param name="jobType">scan or print</param>
        /// </summary>
        public void SelectJobForSetOptions(iManageJobType jobType)
        {
            bool result = false;
            switch (jobType)
            {
                case iManageJobType.Scan:
                    if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/fab"), 500, 600, _inactivityTimeLimit))
                    {
                        result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/fab"));
                        Thread.Sleep(5000);
                        result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/bt_hide_options"));
                    }
                    break;
                case iManageJobType.Print:
                    IsJobOptionScreenToPrint(jobType);
                    _printPages = GetPrintPages();
                    result = true;
                    break;
                default:
                    DeviceWorkflowException e = new DeviceWorkflowException($"Unrecognized Connector Job Type: {jobType.GetDescription()} :: {_appName}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.FalseAlarm.GetDescription());
                    throw e;
            }
            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Current Screen is not Job options page after navigation ({jobType.GetDescription()}):: {_appName} ");
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

            if (!LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/tv_page_count")))
            {
                parsingText = "1";
                UpdateStatus("This pageCount is 1. That's why file format is txt");
            }
            else
            {
                parsingText = LinkUI.Controller.GetText(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/tv_page_count"));
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
        public void IsJobOptionScreenToPrint(iManageJobType jobType)
        {
            bool result = true;
            int timeOut = 0;
            string errorMessage = "No Error";

            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);

            timeOut = LinkUI.Controller.GetTimeout();
            LinkUI.Controller.SetTimeout(0);
            result = JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/pb_dialog_progress"), 500, 1000, _inactivityTimeLimit);
            
            if (!result)
            {
                errorMessage = $"Downloading file is abnormal status :: {_appName} ";
            }
            CheckServerErrorPopup();
            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerEnd);
            LinkUI.Controller.SetTimeout(30);

            if (result)
            {
                result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/btn_print"));
                // Check that Downloading is completed and Current UI is Job Option Screen on the print section
                if (!result)
                {
                    errorMessage = $"Cannot find Print Button :: {_appName} ";
                }
            }

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"{errorMessage}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.DownloadingPrintFile.GetDescription());
                throw e;
            }
            LinkUI.Controller.SetTimeout(timeOut);
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
        /// Updates the status of the plugin execution.
        /// </summary>
        /// <param name="status">The status.</param>
        protected void UpdateStatus(string status)
        {
            LogInfo(status);
            ActivityStatusChanged?.Invoke(this, new StatusChangedEventArgs(status));
        }

        /// <summary>
        /// When popup - "Do not show again" will be opened, close this popup.
        /// </summary>
        private void CloseDetailPopup()
        {
            int timeOut = 0;

            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(5);
            LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/buttonClose"));
            LinkUI.Controller.SetTimeout(0);
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/buttonClose"));
            LinkUI.Controller.SetTimeout(timeOut);
        }

        /// <summary>
        /// Close App guide page
        /// </summary>
        private void CloseAppGuide()
        {
            int timeOut = 0;

            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(5);
            LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/checkBoxDoNotShowAgain"));

            LinkUI.Controller.SetTimeout(0);
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/checkBoxDoNotShowAgain"));
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/buttonClose"));
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

            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/iv_dialog_icon")) && LinkUI.Controller.GetText(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/tv_contents")).Contains("error"))
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Server Error Popup {LinkUI.Controller.GetText(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/tv_contents"))}");
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

            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/iv_dialog_icon")) && LinkUI.Controller.GetText(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/tv_contents")).Contains("Unable"))
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Server Error Popup {LinkUI.Controller.GetText(new UiSelector().ResourceId($"{iManageAppsPackageName}:id/tv_contents"))}");
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
