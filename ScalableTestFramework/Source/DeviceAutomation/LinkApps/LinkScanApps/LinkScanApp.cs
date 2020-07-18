using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Utility;
using HP.SPS.SES.Helper;
using System;
using System.Threading;
using static HP.ScalableTest.Framework.Logger;
using static HP.SPS.SES.SESLib;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.LinkScanApps
{
    /// <summary>
    /// Configuration to prepare job for Each Link Apps.
    /// </summary>
    public class LinkScanApp : DeviceWorkflowLogSource, IDisposable
    {
        private readonly string _destination;

        private LinkScanDestination Destination
        {
            get { return EnumUtil.GetByDescription<LinkScanDestination>(_destination); }
        }

        private static string _exceptionCategoryData = "ExceptionCategory";

        /// <summary>
        /// Package name of executing app
        /// </summary>
        public string LinkScanAppsPackageName;

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
        /// For running Selected job, values are defined.
        /// </summary>
        public class ScanToEmailInfo
        {
            /// <summary>
            /// Gets or sets the from field.
            /// </summary>
            public string From { get; set; }
            /// <summary>
            /// Gets or sets the to field.
            /// </summary>
            public string To { get; set; }
            /// <summary>
            /// Gets or sets the cc field.
            /// </summary>
            public string Cc { get; set; }
            /// <summary>
            /// Gets or sets the bcc field.
            /// </summary>
            public string Bcc { get; set; }
            /// <summary>
            /// Gets or sets the subject field.
            /// </summary>
            public string Subject { get; set; }
            /// <summary>
            /// Gets or sets the message field.
            /// </summary>
            public string Message { get; set; }
        }

        /// <summary>
        /// For running Selected job, values are defined.
        /// </summary>
        public class ScanToSMBFTPInfo
        {
            /// <summary>
            /// Gets or sets the Server field.
            /// </summary>
            public string Server { get; set; }

            /// <summary>
            /// Gets or sets the UserName field.
            /// </summary>
            public string UserName { get; set; }

            /// <summary>
            /// Gets or sets the Password field.
            /// </summary>
            public string Password { get; set; }

            /// <summary>
            /// Gets or sets the DomainPort field.
            /// </summary>
            public string DomainPort { get; set; }

            /// <summary>
            /// Gets or sets the FolderPath field.
            /// </summary>
            public string FolderPath { get; set; }
        }

        /// <summary>
        /// Configuration to prepare job for Each Link Apps.
        /// </summary>
        /// <param name="destinaion"></param>
        /// <param name="device"></param>
        public LinkScanApp(LinkScanDestination destinaion, IDevice device)
        {
            _destination = destinaion.GetDescription();
            LinkScanAppsPackageName = $"com.hp.print.scanapps.{_destination.ToLower()}";
            LinkUI = new JetAdvantageLinkUI(device);
            ScanOptionManager = new JetAdvantageLinkScanOptionManager(LinkUI, LinkScanAppsPackageName);
            Device = (JediOmniDevice)device;
            JetAdvantageLinkControlHelper = new JetAdvantageLinkControlHelper(LinkUI, LinkScanAppsPackageName);
        }

        /// <summary>
        /// Launches the LinkApps using App Name
        /// </summary>
        public void Launch()
        {
            string launchAppname = null;
            DateTime startTime = DateTime.Now;

            int timeOut = 0;

            try
            {
                launchAppname = GetLaunchAppId();
                UpdateStatus($"Launch App: {launchAppname}");

                Device.ControlPanel.ScrollPress($"{launchAppname}");
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, launchAppname);

                Thread.Sleep(300);

                timeOut = LinkUI.Controller.GetTimeout();
                LinkUI.Controller.SetTimeout(0);

                CloseLicenseAgreementPopup();
                if (!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/bt_send"), 200, 300))
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"Fail to launch app - Send button is not displayed after launching :: {_destination}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                    throw e;
                }

                RecordEvent(DeviceWorkflowMarker.AppShown);

                UpdateStatus($"Lauch app completed - {DateTime.Now.Subtract(startTime).TotalSeconds} sec");
            }
            catch (Exception ex)
            {
                if (ex.Data.Contains(_exceptionCategoryData))
                {
                    throw;
                }

                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to launch app :: {_destination}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                throw e;
            }
            finally
            {
                LinkUI.Controller.SetTimeout(timeOut);
            }
        }

        /// <summary>
        /// File Name and Folder Path Setting
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="emailInfo"></param>
        public void EmailJobSetting(string fileName, ScanToEmailInfo emailInfo)
        {
            UpdateStatus($"Starting Email Job Setting");
            UpdateStatus($"from : {emailInfo.From}, to : {emailInfo.To}, cc : {emailInfo.Cc}, bcc : {emailInfo.Bcc}, subject : {emailInfo.Subject}, filename : {fileName}, message : {emailInfo.Message}");

            ////////////////////////From
            if (!String.IsNullOrEmpty(emailInfo.From) && LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tagsEditTextFrom")))
            {
                ClickTextbox("tagsEditTextFrom");
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tagsEditTextFrom"), emailInfo.From))
                {
                    throw new DeviceWorkflowException($"Fail to set the From field: {emailInfo.From}");
                }
                PressBackKey();
            }
            ////////////////////////

            ////////////////////////To
            if (!String.IsNullOrEmpty(emailInfo.To) && LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tagsEditTextTo")))
            {
                ClickTextbox("tagsEditTextTo");
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tagsEditTextTo"), emailInfo.To))
                {
                    throw new DeviceWorkflowException($"Fail to set the To field: {emailInfo.To}");
                }
                PressBackKey();
            }
            ////////////////////////

            ////////////////////////Cc
            if (!String.IsNullOrEmpty(emailInfo.Cc) && LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tagsEditTextCc")))
            {
                ClickTextbox("tagsEditTextCc");
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tagsEditTextCc"), emailInfo.Cc))
                {
                    throw new DeviceWorkflowException($"Fail to set the Cc field: {emailInfo.Cc}");
                }
                PressBackKey();
            }
            ////////////////////////

            ////////////////////////Bcc
            if (!String.IsNullOrEmpty(emailInfo.Bcc) && LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tagsEditTextBcc")))
            {
                ClickTextbox("tagsEditTextBcc");
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tagsEditTextBcc"), emailInfo.Bcc))
                {
                    throw new DeviceWorkflowException($"Fail to set the Bcc field: {emailInfo.Bcc}");
                }
                PressBackKey();
            }
            ////////////////////////

            ////////////////////////Subject
            if (!String.IsNullOrEmpty(emailInfo.Subject) && LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/editTextSubject")))
            {
                ClickTextbox("editTextSubject");
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/editTextSubject"), emailInfo.Subject))
                {
                    throw new DeviceWorkflowException($"Fail to set the Subject field: {emailInfo.Subject}");
                }
                PressBackKey();
            }
            ////////////////////////

            ////////////////////////File Name
            LinkUI.Controller.Swipe(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/fileNameTextViewMessage"), To.Up);
            if (!String.IsNullOrEmpty(fileName) && LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/editTextDescription")))
            {
                ClickTextbox("editTextDescription");
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/editTextDescription"), fileName))
                {
                    throw new DeviceWorkflowException($"Fail to set the Filename filed: {fileName}");
                }
                PressBackKey();
            }
            ////////////////////////

            ///////////////////////Message
            LinkUI.Controller.Swipe(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/fileNameTextViewMessage"), To.Up);
            if (!String.IsNullOrEmpty(emailInfo.Message) && LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/editTextMessage")))
            {
                ClickTextbox("editTextMessage");
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/editTextMessage"), emailInfo.Message))
                {
                    throw new DeviceWorkflowException($"Fail to set the Message field: {emailInfo.Message}");
                }
                PressBackKey();
            }
            ////////////////////////
        }

        /// <summary>
        /// Press Back Key
        /// </summary>
        public void PressBackKey()
        {
            if (!LinkUI.Controller.PressKey(KeyCode.KEYCODE_BACK))
            {
                throw new DeviceWorkflowException($"Fail to press back button");
            }
        }

        /// <summary>
        /// Click Text Box
        /// </summary>
        /// /// <param name="selector"></param>
        public void ClickTextbox(string selector)
        {
            if (!LinkUI.Controller.Click(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/{selector}")))
            {
                throw new DeviceWorkflowException($"Fail to click {LinkScanAppsPackageName}:id/{selector}");
            }
        }

        /// <summary>
        /// File Name and Folder Path Setting
        /// </summary>
        /// <param name="fileName"></param>
        /// <param name="smbftpInfo"></param>
        public void FileNameFolderPathSetting(string fileName, ScanToSMBFTPInfo smbftpInfo)
        {
            string launchAppname = GetLaunchAppId();

            if (!String.IsNullOrEmpty(fileName))
            {
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/etFileName"), fileName))
                {
                    throw new DeviceWorkflowException($"Fail to set the file name field: {fileName}");
                }
            }

            if (!LinkUI.Controller.Click(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/ivAddBtn")))
            {
                throw new DeviceWorkflowException($"Fail to click the Add button");
            }

            if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/et_folderPaths"), smbftpInfo.Server))
            {
                throw new DeviceWorkflowException($"Fail to set the folder path: {smbftpInfo.FolderPath}");
            }

            if (!LinkUI.Controller.Click(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/bt_done")))
            {
                throw new DeviceWorkflowException($"Fail to click the Save button");
            }

            if (!String.IsNullOrEmpty(smbftpInfo.UserName))
            {
                LinkUI.Controller.Click(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/et_username"));
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/et_username"), smbftpInfo.UserName))
                {
                    throw new DeviceWorkflowException($"Fail to set the User Name: {smbftpInfo.UserName}");
                }
            }

            if (!String.IsNullOrEmpty(smbftpInfo.Password))
            {
                LinkUI.Controller.Click(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/et_password"));
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/et_password"), smbftpInfo.Password))
                {
                    throw new DeviceWorkflowException($"Fail to set the Password: {smbftpInfo.Password}");
                }
            }

            if (!String.IsNullOrEmpty(smbftpInfo.FolderPath))
            {
                LinkUI.Controller.Click(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/et_folder_path"));
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/et_folder_path"), smbftpInfo.FolderPath))
                {
                    throw new DeviceWorkflowException($"Fail to set the FTP Folder Path: {smbftpInfo.FolderPath}");
                }
            }

            if (!String.IsNullOrEmpty(smbftpInfo.DomainPort))
            {
                string resourceid = null;
                if (Destination == LinkScanDestination.FTP)
                {
                    resourceid = "et_port";
                }
                else if (Destination == LinkScanDestination.SMB)
                {
                    resourceid = "et_domain";
                }

                LinkUI.Controller.Click(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/{resourceid}"));
                if (!LinkUI.Controller.SetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/{resourceid}"), smbftpInfo.DomainPort))
                {
                    throw new DeviceWorkflowException($"Fail to set the Domain: {smbftpInfo.DomainPort}");
                }
            }

            LinkUI.Controller.PressKey(4);

            if (!LinkUI.Controller.Click(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/bt_ok")))
            {
                throw new DeviceWorkflowException($"Fail to click the OK button");
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
            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/bt_send"));

            Thread.Sleep(TimeSpan.FromSeconds(1));
            CheckServerErrorPopup();

            Thread.Sleep(TimeSpan.FromSeconds(1));
            HandleJediOmniPopup("#hpid-prompt-flatbed-autodetect", "#hpid-button-continue");

            RecordEvent(DeviceWorkflowMarker.JobBuildBegin);

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to execution scan job - Fail to click the scan button :: {_destination}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }
            string flag = originalsides.GetDescription();
            UpdateStatus($"{flag} Scan Job started :: {_destination}");
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);
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
                        DeviceWorkflowException e = new DeviceWorkflowException($"Unrecognized Original sides on Scan job:: {_destination}");
                        e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.FalseAlarm.GetDescription());
                        throw e;
                }
            }
            else
            {
                ScanOption_OnesidedJob();
            }
            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
        }

        /// <summary>
        /// Run an OneSidedJob of scan job
        /// </summary>
        private void ScanOption_OnesidedJob()
        {
            string reason = $"Fail to execution One Sided Scan Job :: {_destination}";

            int timeOut = 0;
            int testCount = 0;
            bool result = true;
            bool autoDetectPopup = false;
            bool processingJob = false;

            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(0);

            while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/pb_dialog_progress")) || LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/ll_dots")))
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

                if(!processingJob && LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/pb_dialog_progress")))
                {
                    RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
                    processingJob = true;
                }

                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tv_contents")))
                {
                    string tv_contents = LinkUI.Controller.GetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tv_contents"));

                    UpdateStatus($"Scan job status: {tv_contents}");

                    if (string.IsNullOrEmpty(tv_contents) || (!tv_contents.ToLower().Contains("scanning")))
                    {
                        break;
                    }
                }
                testCount++;
                Thread.Sleep(200);
            }
            
            RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
            RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
            if (result)
            {
                RecordEvent(DeviceWorkflowMarker.SendingJobBegin);

                testCount = 0;
                CheckServerErrorPopup();

                while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/pb_dialog_progress")))
                {
                    if (!autoDetectPopup)
                    {
                        autoDetectPopup = HandleJediOmniPopup("#hpid-prompt-flatbed-autodetect", "#hpid-button-continue");
                    }

                    if (testCount > 100)
                    {
                        result = false;
                        reason = $"Fail to One Sided Scan Job - The progress circle is not disappear :: {_destination}";
                        break;
                    }

                    if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/toolbar")))
                    {
                        break;
                    }

                    testCount++;
                    Thread.Sleep(300);
                }
                CheckServerErrorPopup();
                RecordEvent(DeviceWorkflowMarker.SendingJobEnd); 
            }
            else
            {
                reason = $"Fail to One Sided Scan Job - The \"sending\" status is not found pn screen :: {_destination}";
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
        /// <param name="ScanCount"></param>
        private void ScanOption_TwosidedJob(int ScanCount)
        {
            string reason = $"Fail to execution Two Sided Scan Job :: {_destination}";
            int timeOut = 0;
            int testCount = 0;
            bool result = true;
            bool autoDetectPopup = false;

            timeOut = LinkUI.Controller.GetTimeout();

            try
            {
                LinkUI.Controller.SetTimeout(5);
                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tv_contents")))
                {
                    RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
                }
                LinkUI.Controller.SetTimeout(timeOut);
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
                        RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
                        RecordEvent(DeviceWorkflowMarker.ScanJobEnd); 
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
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to Two Sided Scan job - \"Scan more\" screen is not displayed {ex.Message}:: {_destination}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }
            LinkUI.Controller.SetTimeout(0);

            if (result)
            {
                RecordEvent(DeviceWorkflowMarker.SendingJobBegin);
                CheckServerErrorPopup();
                testCount = 0;

                while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/pb_dialog_progress")))
                {
                    if (testCount > 100)
                    {
                        result = false;
                        reason = $"Fail to Two Sided Scan Job - The progress circle is not disappear :: {_destination}";
                        break;
                    }

                    if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/toolbar")))
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
                DeviceWorkflowException e = new DeviceWorkflowException($"Execution Two Sided Scan Job failed :: {_destination}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Get App Name from ConnectorDisplayName enum
        /// </summary>
        private string GetLaunchAppId()
        {
            LinkScanAppDisplayName _linkScanAppName = (LinkScanAppDisplayName)Enum.Parse(typeof(LinkScanAppDisplayName), _destination);

            return _linkScanAppName.GetDescription();
        }

        /// <summary>
        /// When popup - "HP App End User License Agreement" will be opened, close this popup.
        /// </summary>
        private void CloseLicenseAgreementPopup()
        {
            int timeout = 0;
            timeout = LinkUI.Controller.GetTimeout();
            LinkUI.Controller.SetTimeout(0);

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/bt_continue"), 200, 300))
            {
                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/cb_agree"), 200, 300))
                {
                    LinkUI.Controller.Click(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/cb_agree"));
                }

                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/bt_continue"), 200, 300))
                {
                    LinkUI.Controller.Click(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/bt_continue"));
                }
            }
            LinkUI.Controller.SetTimeout(timeout);
        }

        /// <summary>
        /// BacktoHomeScreen
        /// </summary>
        public void BacktoHomeScreen()
        {
            int count = 0;
            int timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(3);

            while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/bt_send")))
            {
                if (count > 10)
                {
                    return;
                }
                LinkUI.Controller.PressKey(KeyCode.KEYCODE_ESCAPE);
                count++;
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
        /// Check an Error Popup(Server Error)
        /// </summary>
        private void CheckServerErrorPopup()
        {
            int timeOut = 0;
            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(0);

            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/iv_dialog_icon")) && LinkUI.Controller.GetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tv_contents")).Contains("error"))
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Server Error Popup {LinkUI.Controller.GetText(new UiSelector().ResourceId($"{LinkScanAppsPackageName}:id/tv_contents"))}");
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
