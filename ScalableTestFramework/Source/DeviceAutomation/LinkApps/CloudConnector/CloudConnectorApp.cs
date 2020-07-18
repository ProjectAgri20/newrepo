using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JetAdvantageLink;
using HP.ScalableTest.Utility;
using HP.SPS.SES.Helper;
using System;
using System.Threading;
using System.Collections.Generic;
using static HP.ScalableTest.Framework.Logger;
using HP.ScalableTest.DeviceAutomation.Enums;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector
{
    /// <summary>
    /// Configuration to prepare job for Each Link Apps.
    /// </summary>
    public class CloudConnectorApp : DeviceWorkflowLogSource, IDisposable
    {
        private string _appName;
        private int _printPages;
        private int _inactivityTimeLimit;

        private static string _exceptionCategoryData = "ExceptionCategory";

        /// <summary>
        /// Package name of executing app
        /// </summary>
        public string CloudConnectorPackageName;

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
        public CloudConnectorApp(ConnectorName appName, IDevice device)
        {
            _appName = appName.GetDescription();
            CloudConnectorPackageName = $"com.hp.print.horizontalconnector.{_appName.ToLower()}";
            LinkUI = new JetAdvantageLinkUI(device);
            PrintOptionManager = new JetAdvantageLinkPrintOptionManager(LinkUI, CloudConnectorPackageName);
            ScanOptionManager = new JetAdvantageLinkScanOptionManager(LinkUI, CloudConnectorPackageName);
            Device = (JediOmniDevice)device;
            JetAdvantageLinkControlHelper = new JetAdvantageLinkControlHelper(LinkUI, CloudConnectorPackageName);
            _inactivityTimeLimit = GetInactivityTimeout();
        }

        /// <summary>
        /// For running Selected job, values are defined.
        /// </summary>
        public class JobExecutionOptions
        {
            /// <summary>
            /// Gets or sets the type of job build. JobType - scan or print.
            /// </summary>
            public ConnectorJobType JobType { get; set; }

            /// <summary>
            /// Documents Count
            /// </summary>
            public int DocumentCount { get; set; }

            /// <summary>
            /// ScanCount
            /// </summary>
            public int PageCount { get; set; }

            /// <summary>
            /// 0 : 1-Sided, 1 : 2-Sided , 2 : 2-Sided Flip Up
            /// </summary>
            public LinkScanOriginalSides OriginalSides { get; set; }
        }

        /// <summary>
        /// Launches the LinkApps using App Name
        /// </summary>
        public void Launch(SIOMethod loginmethod)
        {
            string launchAppname = null;

            int timeOut = 0;
            bool isDropbox = false;
            DateTime startTime = DateTime.Now;

            try
            {
                launchAppname = GetLaunchAppname();
                UpdateStatus($"Launch App: {launchAppname}");

                Device.ControlPanel.ScrollPress($"{launchAppname}");
                RecordEvent(DeviceWorkflowMarker.AppButtonPress, launchAppname);
                Thread.Sleep(300);

                timeOut = LinkUI.Controller.GetTimeout();
                LinkUI.Controller.SetTimeout(0);

                if (SIOMethod.SIOWithoutIDPWD.Equals(loginmethod))
                {
                    if (!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/menu_more_ac"), 200, 300))
                    {
                        DeviceWorkflowException e = new DeviceWorkflowException($"Fail to launch app - main page is not appeared :: {_appName}");
                        e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                        throw e;
                    }
                }
                else if (SIOMethod.NoneSIO.Equals(loginmethod))
                {
                    if (!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_continue"), 200, 300))
                    {
                        DeviceWorkflowException e = new DeviceWorkflowException($"Fail to launch app - EULA page is not appeared :: {_appName}");
                        e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                        throw e;
                    }
                }
                else if (SIOMethod.SIOWithIDPWD.Equals(loginmethod))
                {
                    if (!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/rl_dialog_button_pane"), 200, 300))
                    {
                        DeviceWorkflowException e = new DeviceWorkflowException($"Fail to launch app - DONE is not appeared :: {_appName}");
                        e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                        throw e;
                    }
                }

                if (LinkUI.Controller.IsEnabled("#hpid-keyboard-key-done"))
                {
                    LinkUI.Controller.PressKey(KeyCode.KEYCODE_BACK);
                }

                UpdateStatus($"Waiting to launch the {CloudConnectorPackageName} : Limitation time: {GetInactivityTimeout()} ms - It determined by InactivityTime Out");
                RecordEvent(DeviceWorkflowMarker.AppShown);
                if (JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/pb_dialog_progress"), 500, 600, _inactivityTimeLimit))
                {
                    DateTime limitTime = DateTime.Now.AddMilliseconds(_inactivityTimeLimit);
                    bool isEula = false;
              
                    if (!SIOMethod.SIOWithoutIDPWD.Equals(loginmethod))
                    {
                        while (!LinkUI.Controller.DoesScreenContains(new UiSelector().PackageName(CloudConnectorPackageName).ClassName("android.webkit.WebView"))
                        && !LinkUI.Controller.DoesScreenContains(new UiSelector().PackageName(CloudConnectorPackageName).ClassName("android.widget.EditText")))
                        {
                            if (!isEula && SIOMethod.SIOWithIDPWD.Equals(loginmethod))
                            {
                                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/rl_dialog_button_pane"), 200, 300))
                                {
                                    LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_dialog_left"));
                                }
                                isEula = true;
                            }
                            if (!isEula && LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_continue")))
                            {
                                CloseLicenseAgreementPopup();
                                limitTime = DateTime.Now.AddMilliseconds(_inactivityTimeLimit);
                                isEula = true;
                            }
                            CheckServerErrorPopup();

                            if (DateTime.Now >= limitTime)
                            {
                                ConnectorDisplayName connectorName = (ConnectorDisplayName)Enum.Parse(typeof(ConnectorDisplayName), _appName);
                                isDropbox = ConnectorDisplayName.DropBox.Equals(connectorName);

                                if (isDropbox)
                                {
                                    UpdateStatus($"{_appName} webview is not displayed :: It will check from the webview control");
                                    break;
                                }
                                else
                                {
                                    UpdateStatus($"Fail to launch app - It took over {DateTime.Now.Subtract(startTime).TotalSeconds} secs");

                                    DeviceWorkflowException e = new DeviceWorkflowException($"Fail to launch app - First screen at the app is not displayed over {_inactivityTimeLimit} ms :: {_appName}");
                                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());

                                    throw e;
                                }
                            }
                            Thread.Sleep(200);
                        }
                    }  
                }
                else
                {
                    DeviceWorkflowException e = new DeviceWorkflowException($"Fail to launch app - Progress circle is not disappeared over ({_inactivityTimeLimit} ms) :: {_appName}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
                    throw e;
                }
                if (!isDropbox)
                {
                    UpdateStatus($"Launch app completed - {DateTime.Now.Subtract(startTime).TotalSeconds} sec");
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
        /// Get App Name from ConnectorDisplayName enum
        /// </summary>
        private string GetLaunchAppname()
        {
            ConnectorDisplayName _connectorName = (ConnectorDisplayName)Enum.Parse(typeof(ConnectorDisplayName), _appName);

            return _connectorName.GetDescription();
        }

        /// <summary>
        /// Do login the LinkApps using ID/PW
        /// </summary>
        /// <param name="connectorName"></param>
        /// <param name="id">ID for doing login the application</param>
        /// <param name="pw">PW for doing login the application</param>        
        public void Login(ConnectorName connectorName, string id, string pw)
        {
            bool result = true;
            int countLimit = 60;
            int timeOut = LinkUI.Controller.GetTimeout();
            ICloudLoginManager loginManager = CreateLoginManager(connectorName);
            DateTime startTime = DateTime.Now;

            if (WorkflowLogger != null)
            {
                loginManager.WorkflowLogger = WorkflowLogger;                
            }
        
            RecordEvent(DeviceWorkflowMarker.AuthenticationBegin);

            try
            {
                result = loginManager.SignIn(id, pw);
            }     
            catch(NullReferenceException ex)
            {
                DeviceCommunicationException e = new DeviceCommunicationException("Fail to get webview - You need check used hpk file is \"webview debuggable option\" enabled", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.EnvironmentError.GetDescription());
                throw e;
            }
            RecordEvent(DeviceWorkflowMarker.AuthenticationEnd);
            UpdateStatus($"Sign in completed - {DateTime.Now.Subtract(startTime).TotalSeconds} secs");

            LinkUI.Controller.SetTimeout(0);

            while (result)
            {
                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/buttonClose")))
                {
                    CloseDetailPopup();
                }

                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/checkBoxDoNotShowAgain")))
                {
                    CloseAppGuide();
                }

                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_dialog_left")))
                {
                    CloseAccountSavePopup();
                }

                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/iv_dialog_icon")))
                {
                    CheckServerErrorPopup();
                }

                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/menu_more_ac")))
                {
                    result = true;
                    LinkUI.Controller.SetTimeout(timeOut);
                    break;
                }

                Thread.Sleep(TimeSpan.FromMilliseconds(500));
                countLimit--;

                if(countLimit < 0)
                {
                    result = false;
                }
            }

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to log in by ({id}) :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SignIn.GetDescription());
                throw e;
            }
            //RecordEvent(DeviceWorkflowMarker.AppShown);
        }

        /// <summary>
        /// Distinguish signin function for each apps to do signin
        /// </summary>
        /// <returns></returns>
        private ICloudLoginManager CreateLoginManager(ConnectorName connectorName)
        {
            switch (connectorName)
            {
                case ConnectorName.DropBox:
                    return new DropBoxLoginManager(LinkUI);
                case ConnectorName.OneDrive:
                    return new MicrosoftLoginManager(LinkUI);
                case ConnectorName.OneDriveBusiness:
                    return new MicrosoftLoginManager(LinkUI, true);
                case ConnectorName.GoogleDrive:
                    return new GoogleLoginManager(LinkUI);
                case ConnectorName.SharePoint:
                    return new MicrosoftLoginManager(LinkUI);
                case ConnectorName.Box:
                    return new BoxLoginManager(LinkUI);
            }
            ArgumentOutOfRangeException e = new ArgumentOutOfRangeException(nameof(_appName), "Invalid App Name");
            e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.AppLaunch.GetDescription());
            throw e;
        }

        /// <summary>
        /// Navigate the location to save scan file(to select print file)
        /// <param name="filePath">Navigate to destination by FilePath</param>
        /// </summary>
        public void NavigateToDestination(string filePath)
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

            if(!String.IsNullOrEmpty(path))
            {
                filename = path;
                UpdateStatus($"Final filename is  :: {filename}  {_appName}");
                //Final Destination(Scan : Final Folder, Print : Print Filename)
                SearchForParsedText(filename);
            }
            
        }

        /// <summary>
        /// In case of multiple file selection, this function will run for selecting them
        /// </summary>
        /// <param name="fileList">Selection each file in this list</param>
        public void MultiplefileSelection(List<string> fileList)
        {
            // Multiple File Selection Case
            FileSortAndSelection("A to Z", fileList);
            UpdateStatus($"MultiplefileSelection in {_appName}");
        }

        /// <summary>
        /// In case of multiple file selection for SharePoint, this function will run for selecting them
        /// </summary>
        /// <param name="fileList">Selection each file in this list</param>
        public void MultiplefileSelectionForSharePoint(List<string> fileList)
        {
            // Multiple File Selection Case
            FileSortAndSelectionForSharePoint(fileList);
            UpdateStatus($"MultiplefileSelection in {_appName}");
        }


        /// <summary>
        /// Set the pre-condition for sorting and selecting
        /// <param name="sortMethod">How to sort file on the UI of solution</param>
        /// <param name="fileList">Selection each file in this list</param> 
        /// </summary>
        private void FileSortAndSelection(string sortMethod, List<string> fileList)
        {
            UpdateStatus($"Sorting and selecting files in {_appName}");
            bool result;

            fileList.Sort();

            // Sort filelist by "A to Z"
            result = FileSort(sortMethod);
            // Sort files in the file(folder) management status on the UI

            if (result)
            {
                FileSelection(fileList);
                //Select all files in filelist after filesort is sorted by "A to Z" option => Multiple file
            }
        }

        /// <summary>
        /// Set the pre-condition for sorting and selecting for SharePoint        
        /// <param name="fileList">Selection each file in this list</param> 
        /// </summary>
        private void FileSortAndSelectionForSharePoint(List<string> fileList)
        {
            UpdateStatus($"Sorting and selecting files in {_appName}");
            bool result = false;
            fileList.Sort();
            
            string errorMessage = "Sorting of file list is failed";
            // Sort files in the file(folder) management status on the UI

            result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_sort_column"), 500, 600);

            if (result && JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_sort_column"), 500, 600))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_sort_column").TextContains("Name"));
            }
            else
            {
                result = false;
            }

            if (result && JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/column_sort_asc"), 500, 600))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/column_sort_asc"));
            }
            else
            {
                result = false;
            }

            if (result)
            {
                FileSelection(fileList);
                //Select all files in filelist after filesort is sorted by "A to Z" option => Multiple file
            }
            else
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to sort files :: Clicking Fail <{errorMessage}>");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.DownloadingPrintFile.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sortMethod"></param>
        /// <returns></returns>
        private bool FileSort(string sortMethod)
        {
            UpdateStatus($"Sorting files in {_appName}");
            bool result = false;
            string errorMessage = "iv_dropdown_btn";

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear
                (new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/iv_dropdown_btn"), 500, 600, _inactivityTimeLimit))
            {
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/iv_dropdown_btn"));
            }

            if (result)
            {
                switch (sortMethod)
                {
                    case "Newest":
                        result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/newest"));
                        break;

                    case "Oldest":
                        result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/oldest"));
                        break;

                    case "A to Z":
                        result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/atoz"));
                        break;

                    case "Z to A":
                        result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/ztoa"));
                        break;
                }
                errorMessage = sortMethod;
                Thread.Sleep(TimeSpan.FromSeconds(5));
            }
            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to sort files :: Clicking Fail <{errorMessage}>");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.DownloadingPrintFile.GetDescription());
                throw e;
            }
            return result;
        }

        /// <summary>
        /// For selecting files in filelist
        /// </summary>
        /// <param name="fileList"></param>
        /// <returns></returns>
        private void FileSelection(List<string> fileList)
        {
            string reason = $"Fail to select files :: {_appName}";
            bool result;
            int listCount = 1;

            fileList.Sort();

            UpdateStatus($"First File in filelist is {fileList[0]}");
            result = JetAdvantageLinkControlHelper.FindOnListWithScroll(
                new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/rv_fileList"),
                new UiSelector().TextContains(fileList[0]), 10);

            if (result)
            {
                result = LinkUI.Controller.LongClick(new UiSelector().TextContains(fileList[0]));
                // For going to selection(file/folder management screen)
            }
            else
            {
                reason = $"Fail to select files - Fail to long click on the first file at the list ({fileList[0]}):: {_appName}";
            }

            if (result)
            {
                while (listCount < fileList.Count)
                {
                    result = JetAdvantageLinkControlHelper.ClickOnSequencialFileListWithScroll(
                        new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/rv_fileList"),
                        new UiSelector().TextContains(fileList[listCount]), 10);

                    if (!result)
                    {
                        reason = $"Fail to select files - Can not select the file (filename[{fileList[listCount]}], filecount[{listCount}] :: {_appName}";
                        break;
                    }

                    listCount++;
                }
            }
            else
            {
                reason = $"Fail to select files - Fail to long click on the first file at the list ({fileList[0]}):: {_appName}";
            }
            
            // Select one by one from fileList

            if (result)
            {                
                result = LinkUI.Controller.Click
                    (new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/iv_print_image"));
                
                if (!result)
                {
                    reason = $"Fail to click the Print button in printing by multiple file :: {_appName}";
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
        /// Check that current UI is Job Option status or not.
        /// </summary>
        public void IsJobOptionScreenToPrint(ConnectorJobType JobType)
        {
            string reason = $"Fail to check UI screen for printing job :: {_appName}";
            bool result = true;
            int timeOut = 0;
            
            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerBegin);

            timeOut = LinkUI.Controller.GetTimeout();
            LinkUI.Controller.SetTimeout(0);

            switch (JobType)
            {
                case ConnectorJobType.Print:
                    result = JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/pb_dialog_progress"), 500, 1200, _inactivityTimeLimit);
                    break;
                case ConnectorJobType.MultiplePrint:
                    result = JetAdvantageLinkControlHelper.WaitingObjectDisappear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_contents"), 500, 1200, _inactivityTimeLimit);
                    break;
            }

            if (!result)
            {
                reason = $"Fail to check UI screen for printing job - Fail to downloading the selected file :: {_appName} ";
            }

            CheckServerErrorPopup();
            RecordEvent(DeviceWorkflowMarker.PullingJobFromServerEnd);
            LinkUI.Controller.SetTimeout(60);

            if (result)
            {
                result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/btn_print"));

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
            LinkUI.Controller.SetTimeout(timeOut);
        }

        /// <summary>
        /// Select SharePoint Site and Move to Documents Library in the site
        /// </summary>
        /// <param name="siteName"></param>
        public void SelectSharePointSite(string siteName)
        {
            int timeOut = LinkUI.Controller.GetTimeout();

            if (!_appName.Equals("SharePoint"))
            {
                InvalidOperationException e = new InvalidOperationException($"This routine is for only SharePoint :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                throw e;
            }

            bool result = JetAdvantageLinkControlHelper.ClickOnListWithScroll(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/rv_fileList"), new UiSelector().TextContains(siteName), 8);
            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to find the site name - Can not find the Site name ({siteName}) :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                throw e;
            }

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().TextContains("Documents").ResourceId($"{CloudConnectorPackageName}:id/tv_file_name"), 500, 60, _inactivityTimeLimit))
            {
                result = LinkUI.Controller.Click(new UiSelector().TextContains("Documents").ResourceId($"{CloudConnectorPackageName}:id/tv_file_name"));
            }
            else
            {
                result = false;
            }            

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to select Documents library - Error while selecting Documents library :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.NavigateFilePath.GetDescription());
                throw e;
            }

            CloseAppGuide();

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/fab"), 500, 600, _inactivityTimeLimit))
            {
                LinkUI.Controller.SetTimeout(timeOut);
            }
            else
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to find scan button after selecting Documents library :: {_appName}");
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

            int timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(0);

            JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/layout_files_item"), 200, 50);

            if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_file_name").TextContains(ParsedItem)))
            {
                UpdateStatus($"Click the {ParsedItem} by using displayed list");
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_file_name").TextContains(ParsedItem));
                LinkUI.Controller.SetTimeout(timeOut);
            }
            else if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_file_column_info").TextContains(ParsedItem)))
            {
                UpdateStatus($"Click the {ParsedItem} by using displayed list");
                result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_file_column_info").TextContains(ParsedItem));
                LinkUI.Controller.SetTimeout(timeOut);
            }
            else
            {
                UpdateStatus($"Click the {ParsedItem} by using Search text box");
                LinkUI.Controller.SetTimeout(timeOut);

                if (result && (result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/iv_search_image"), 500, 60, _inactivityTimeLimit)))
                {
                    result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/iv_search_image"));
                }

                if (result && (result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/searchTextView"), 500, 60, _inactivityTimeLimit)))
                {
                    result = LinkUI.Controller.SetText(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/searchTextView"), ParsedItem);
                }

                if (result && (result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/iv_list_icon"), 500, 60, _inactivityTimeLimit)))
                {                    
                    result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_file_name").TextContains(ParsedItem));
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
        public void SelectJobForSetOptions(ConnectorJobType JobType)
        {            
            bool result = false;
            int timeOut = LinkUI.Controller.GetTimeout();

            switch (JobType)
            {
                case ConnectorJobType.Scan:
                    if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/fab"), 500, 600, _inactivityTimeLimit))
                    {
                        result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/fab"));
                        LinkUI.Controller.SetTimeout(10);
                        result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_hide_options"));
                    }
                    break;
                case ConnectorJobType.Print:
                case ConnectorJobType.MultiplePrint:
                    IsJobOptionScreenToPrint(JobType);
                    _printPages = GetPrintPages();
                    result = true;
                    break;
                default:
                    DeviceWorkflowException e = new DeviceWorkflowException($"Unrecognized Connector Job Type: {JobType.GetDescription()} :: {_appName}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.FalseAlarm.GetDescription());
                    throw e;
            }
            LinkUI.Controller.SetTimeout(timeOut);

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to check Job options page after navigate path ({JobType.GetDescription()}):: {_appName} ");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.SelectOptions.GetDescription());
                throw e;
            }
        }

        /// <summary>
        /// Start Job after SetOptions step
        /// <param name="ExecutionOptions">Conditions for running scan or print job</param>
        /// <param name="UseOriginalSides">SetOption Enable Flag</param>
        /// </summary>
        public void ExecutionJob(JobExecutionOptions ExecutionOptions, bool UseOriginalSides=false)
        {
            switch (ExecutionOptions.JobType)
            {
                case ConnectorJobType.Scan:
                    ExecutionScanJob(ExecutionOptions, UseOriginalSides);
                    break;
                case ConnectorJobType.Print:
                    ExecutionPrintJob(ExecutionOptions);
                    break;
                default:
                    DeviceWorkflowException e = new DeviceWorkflowException($"Unrecognized Connector Job Type: {ExecutionOptions.JobType.GetDescription()} :: {_appName}");
                    e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.FalseAlarm.GetDescription());
                    throw e;
            }
        }

        /// <summary>
        /// Start Scan job and check job finish
        /// <param name="ScanOption">Options of scan build job</param>
        /// <param name="UseOriginalSides">SetOption Enable Flag</param>
        /// </summary>
        public void ExecutionScanJob(JobExecutionOptions ScanOption, bool UseOriginalSides)
        {
            bool result = true;
            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_send"));

            Thread.Sleep(TimeSpan.FromSeconds(1));
            CheckServerErrorPopup();

            Thread.Sleep(TimeSpan.FromSeconds(1));
            HandleJediOmniPopup("#hpid-prompt-flatbed-autodetect", "#hpid-button-continue");

            RecordEvent(DeviceWorkflowMarker.JobBuildBegin);
            RecordEvent(DeviceWorkflowMarker.ScanJobBegin);

            if (!result)
            {
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to click the Scan button :: {_appName}");
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }

            string flag = ScanOption.OriginalSides.GetDescription();
            UpdateStatus($"{flag} Scan Job started :: {_appName}");

            if (UseOriginalSides)
            {
                switch (ScanOption.OriginalSides)
                {
                    case (LinkScanOriginalSides.Onesided):
                        ScanOption_OnesidedJob();
                        break;
                    case (LinkScanOriginalSides.Twosided):
                    case (LinkScanOriginalSides.Pagesflipup):
                        ScanOption_TwosidedJob(ScanOption.PageCount);
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

            RecordEvent(DeviceWorkflowMarker.JobBuildEnd);
        }

        /// <summary>
        /// Run an OneSidedJob of scan job
        /// </summary>
        private void ScanOption_OnesidedJob()
        {
            string reason = $"Fail to execution One Sided Scan Job :: {_appName}";

            int timeOut = 0;
            int testCount = 0;
            bool result = true;
            bool autoDetectPopup = false;
            bool processingJob = false;

            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(0);
                        
            while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/pb_dialog_progress")) || LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/ll_dots")))
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

                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_contents")))
                {
                    if (!processingJob)
                    {
                        RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
                        processingJob = true;
                    }
                    string tv_contents = LinkUI.Controller.GetText(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_contents"));

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
                RecordEvent(DeviceWorkflowMarker.ProcessingJobEnd);
                RecordEvent(DeviceWorkflowMarker.ScanJobEnd);
                RecordEvent(DeviceWorkflowMarker.SendingJobBegin);

                testCount = 0;

                while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/pb_dialog_progress")))
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
            }
            else
            {
                reason = $"Fail to One Sided Scan Job - The \"sending\" status is not found pn screen :: {_appName}";
            }

            if (result)
            {
                CheckServerErrorPopup();
                LinkUI.Controller.SetTimeout(100);
                result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/menu_more_ac"));

                if (!result)
                {
                    reason = $"Fail to One Sided Scan Job - Screen is not return to prior screen after scan job :: {_appName}";
                }

                RecordEvent(DeviceWorkflowMarker.SendingJobEnd);
            }
            else
            {
                reason = $"Fail to One Sided Scan Job - The progress circle is not disappear :: {_appName}";
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
        /// Run a TwoSidedJob of scan job
        /// </summary>
        /// <param name="ScanCount"></param>
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
                LinkUI.Controller.SetTimeout(5);
                if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_contents")))
                {
                    RecordEvent(DeviceWorkflowMarker.ProcessingJobBegin);
                }
                LinkUI.Controller.SetTimeout(timeOut);
                for (int i = 0; i < ScanCount; i++)
                {
                    if(i == 0)
                    {
                        while (!Device.ControlPanel.WaitForAvailable("#hpid-button-scan", TimeSpan.FromSeconds(1)))
                        {   
                            autoDetectPopup = HandleJediOmniPopup("#hpid-prompt-flatbed-autodetect", "#hpid-button-continue");
                            
                            testCount++;
                            
                            if(testCount >= 20)
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
                        UpdateStatus($"Scan completed: {i+1} of {ScanCount.ToString()}");
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
                DeviceWorkflowException e = new DeviceWorkflowException($"Fail to Two Sided Scan job - \"Scan more\" screen is not displayed :: {_appName}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ExecutionJob.GetDescription());
                throw e;
            }

            RecordEvent(DeviceWorkflowMarker.SendingJobBegin);
            LinkUI.Controller.SetTimeout(0);
            CheckServerErrorPopup();
            testCount = 0;


            while (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/pb_dialog_progress")))
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
                result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/menu_more_ac"));

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
        /// Start Print job and check job finish
        /// </summary>
        public void ExecutionPrintJob(JobExecutionOptions execOption)
        {
            string reason = $"Fail to execution Print Job :: {_appName}";

            int timeOut = 0;

            bool result = true;

            UpdateStatus($"Print Job started");

            _printPages = _printPages * execOption.PageCount * execOption.DocumentCount;

            UpdateStatus($"First Document has {_printPages} pages, Total Count of selecting documents is {execOption.DocumentCount}.");
            timeOut = LinkUI.Controller.GetTimeout();


            LinkUI.Controller.SetTimeout(20);
            RecordEvent(DeviceWorkflowMarker.PrintJobBegin);
            result = LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/btn_print"));
            // Job Start

            if (result)
            {
                result = JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_dialog_title"), 200, 150);
                //result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_dialog_title"));
                // Running PopUp is displayed
            }
            else
            {
                reason = $"Fail to execution Print Job - Fail to click the print button :: {_appName}";
            }

            if (result)
            {
                int waitingCount =  60 * _printPages;

                LinkUI.Controller.SetTimeout(0);

                while (waitingCount > 0)
                {
                    CheckServerErrorPopup();

                    if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/searchTextView")))
                    {
                        RecordEvent(DeviceWorkflowMarker.PrintJobEnd);
                        LinkUI.Controller.PressKey(4);
                        // When plugIn have checked stable status

                        LinkUI.Controller.SetTimeout(10);
                        result = LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/menu_more_ac"));
                        // Is completed to prepare SignOut?

                        if (!result)
                        {
                            reason = $"Fail to the Print Job - Screen is not return to prior screen by back key after print job :: {_appName}";
                        }

                        break;
                    }
                    else if (LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/menu_more_ac")))
                    {
                        RecordEvent(DeviceWorkflowMarker.PrintJobEnd);
                        break;
                    }
                    Thread.Sleep(1000);
                    waitingCount--;

                    if (waitingCount == 0)
                    {
                        result = false;
                        break;
                    }
                }            
            }
            else
            {
                reason = $"Fail to execution Print Job - The dialog popup for printing job is not displayed :: {_appName}";
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
        /// GetPrintPages 
        /// </summary>
        private int GetPrintPages()
        {
            int pageCount = 0, totalPages = 0;

            string parsingText = null;

            if (!LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_page_count")))
            {
                parsingText = "1";
                UpdateStatus("This pageCount is 1. That's why file format is txt");
            }
            else
            {
                parsingText = LinkUI.Controller.GetText(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/tv_page_count"));
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
        /// SignOut 
        /// </summary>
        public void SignOut(LogOutMethod logoutmethode)
        {
            UpdateStatus("SignOut Start");
            string reason = $"Fail to sign out :: {_appName}";
            bool result = true;

            RecordEvent(DeviceWorkflowMarker.DeviceSignOutBegin);
            if (logoutmethode.Equals(LogOutMethod.PressBackKey))
            {
                while (!JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_dialog_right"), 200, 20))
                {
                    result &= LinkUI.Controller.PressKey(KeyCode.KEYCODE_BACK);
                    Thread.Sleep(1000);
                }
                result &= LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_dialog_right"));
            }
            else if(logoutmethode.Equals(LogOutMethod.PressSignOut))
            {
                result &= LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/menu_more_ac"));
                result &= LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/action_sign_out"));
                result &= LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_dialog_right"));
            }

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
        /// When popup - "HP App End User License Agreement" will be opened, close this popup.
        /// </summary>
        private void CloseLicenseAgreementPopup()
        {
            int timeout = 0;
            timeout = LinkUI.Controller.GetTimeout();
            LinkUI.Controller.SetTimeout(0);

            if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_continue"), 200, 300))
            {
                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/cb_agree"), 200, 300))
                {
                    LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/cb_agree"));
                }

                if (JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_continue"), 200, 300))
                {
                    LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_continue"));
                }
            }
            
            if(JetAdvantageLinkControlHelper.WaitingObjectAppear(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/rl_dialog_button_pane"), 200, 300))
            {
                LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_dialog_left"));
            }
            LinkUI.Controller.SetTimeout(timeout);
        }

        /// <summary>
        /// When popup - "Do not show again" will be opened, close this popup.
        /// </summary>
        private void CloseDetailPopup()
        {
            int timeOut = 0;

            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(5);
            LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/buttonClose"));
            LinkUI.Controller.SetTimeout(0);
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/buttonClose"));
            LinkUI.Controller.SetTimeout(timeOut);
        }

        /// <summary>
        /// When popup - "Do not show again" will be opened, close this popup.
        /// </summary>
        private void CloseAppGuide()
        {
            int timeOut = 0;

            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(5);
            LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/checkBoxDoNotShowAgain"));

            LinkUI.Controller.SetTimeout(0);
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/checkBoxDoNotShowAgain"));
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/buttonClose"));
            LinkUI.Controller.SetTimeout(timeOut);
        }

        /// <summary>
        /// When popup - "Do you want to save your Google Drive account to your device?" will be opened, close this popup.
        /// </summary>
        private void CloseAccountSavePopup()
        {
            int timeOut = 0;

            timeOut = LinkUI.Controller.GetTimeout();

            LinkUI.Controller.SetTimeout(5);
            LinkUI.Controller.DoesScreenContains(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_dialog_left"));

            LinkUI.Controller.SetTimeout(0);
            LinkUI.Controller.Click(new UiSelector().ResourceId($"{CloudConnectorPackageName}:id/bt_dialog_left"));
            LinkUI.Controller.SetTimeout(timeOut);
        }

        /// <summary>
        /// Check an Error Popup(Server Error)
        /// </summary>
        private void CheckServerErrorPopup()
        {
            int timeOut = 0;
            timeOut = LinkUI.Controller.GetTimeout();

            try
            {
                LinkUI.Controller.SetTimeout(0);
                JetAdvantageLinkControlHelper.CheckErrorPopup();
                LinkUI.Controller.SetTimeout(timeOut);
            }
            catch (DeviceWorkflowException ex)
            {
                DeviceWorkflowException e = new DeviceWorkflowException(ex.Message.ToString() + $" :: {_appName}", ex);
                e.Data.Add(_exceptionCategoryData, ConnectorExceptionCategory.ServerError.GetDescription());
                throw e;
            }
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
