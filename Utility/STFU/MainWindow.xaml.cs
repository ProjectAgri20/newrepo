using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Input;
using System.Windows.Threading;
using System.Xml.Linq;
using System.Xml.Serialization;
using Microsoft.Win32;
using HP.ScalableTest.Data.AssetInventory;
using HP.ScalableTest.Data.AssetInventory.Model;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.UI.Reporting;
using HP.ScalableTest.Utility;
using HP.ScalableTest.LabConsole;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.STFU
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    ///
    public partial class STFUWindow : Window
    {
        private ObservableCollection<ScenarioQueueList> _scenarioList;
        private const string ReportTemplateFileName = "STFU Test Summary.xlsx";
        private static SessionTicket _ticket;
        private string _currentDispatcher = string.Empty, _currentDatabase = string.Empty;
        private int _currentScenarioIndex;
        private DateTime _startTime;
        private bool _isAborted;
        private bool _isScenarioExecuting;
       
        private const int ErrorTimeOutInMinutes = 1;
        private bool _revalidated;
        private const string EMailServer = "smtp3.hp.com";
        public static ObservableCollection<string> MailingList = new ObservableCollection<string>();
        private SessionLogRetention _sessionRetention;
        private readonly ShutdownOptions _shutdownOptions;
        private string _destinationReportFileName;
        //private ExcelReportFile _excelReport;

        #region DraggedItem

        /// <summary>
        /// DraggedItem Dependency Property
        /// </summary>
        public static readonly DependencyProperty DraggedItemProperty =
            DependencyProperty.Register("DraggedItem", typeof(ScenarioQueueList), typeof(STFUWindow));

        /// <summary>
        /// Gets or sets the DraggedItem property.  This dependency property 
        /// indicates ....
        /// </summary>
        public ScenarioQueueList DraggedItem
        {
            get { return (ScenarioQueueList)GetValue(DraggedItemProperty); }
            set { SetValue(DraggedItemProperty, value); }
        }

        private bool IsEditing { get; set; }
        private bool IsDragging { get; set; }
        #endregion


        public STFUWindow()
        {
            InitializeComponent();
            _scenarioList = new ObservableCollection<ScenarioQueueList>();
            ScenarioDataGrid.DataContext = _scenarioList;
            STFDispatcherManager.DispatcherChanged += STFDispatcherManager_DispatcherChanged;

            if (STFLoginManager.Login())
            {
                _currentDatabase = STFLoginManager.SystemDatabase;
                //Set whether STF or STB based on the worker type in the database.
                string officeWorkerType = VirtualResourceType.OfficeWorker.ToString();
                using (EnterpriseTestContext dataContext = new EnterpriseTestContext(_currentDatabase))
                {
                    GlobalSettings.IsDistributedSystem = dataContext.VirtualResources.Any(r => r.ResourceType == officeWorkerType);
                }
                environment_StatusLabel.Text = STFLoginManager.SystemDatabase;
            }
            else
            {
                Environment.Exit(1);
            }

            SessionClient.Instance.DispatcherExceptionReceived += Instance_DispatcherExceptionReceived;
            SessionClient.Instance.SessionStateReceived += Instance_SessionStateReceived;
            SessionClient.Instance.SessionStartupTransitionReceived += Instance_SessionStartupTransitionReceived;
            SessionClient.Instance.SessionMapElementReceived += Instance_SessionMapElementReceived;

            comboBoxRetention.ItemsSource = SessionLogRetentionHelper.ExpirationList;
            comboBoxRetention.SelectedIndex = Convert.ToInt32(GlobalSettings.Items[Setting.DefaultLogRetention]);

            _shutdownOptions = new ShutdownOptions()
            {
                AllowWorkerToComplete = false,
                CopyLogs = false,
                PowerOff = true,
                PowerOffOption = VMPowerOffOption.PowerOff
            };
        }

        private void STFDispatcherManager_DispatcherChanged(object sender, EventArgs e)
        {
            if (STFDispatcherManager.Dispatcher != null)
            {
                _currentDispatcher = STFDispatcherManager.Dispatcher.HostName;
                main_StatusLabel.Text = "Connected to {0}".FormatWith(STFDispatcherManager.Dispatcher.HostName);
                environment_StatusLabel.Text = STFLoginManager.SystemDatabase;
            }
            else
            {
                //dispatcher_ToolStripLabel.Text = "[Not Connected]";
                main_StatusLabel.Text = "Disconnected from dispatcher";
                environment_StatusLabel.Text = STFLoginManager.SystemDatabase;
            }
        }

        private void buttonAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(STFLoginManager.SystemDatabase))
            {
                if (STFLoginManager.Login())
                {
                    _currentDatabase = STFLoginManager.SystemDatabase;
                }
                else
                {
                    return;
                }
            }

            ScenarioSelection scenarios = new ScenarioSelection();
            {
                scenarios.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                if (scenarios.ShowDialog() == true)
                {
                    ScenarioQueueList temp = new ScenarioQueueList
                    {
                        ScenarioId = scenarios.ScenarioId,
                        ScenarioName = scenarios.ScenarioName,
                        Status = "Yet to Start"
                    };
                    _scenarioList.Add(temp);
                    buttonRemove.IsEnabled = true;
                }
            }
        }

        private void buttonRemove_Click(object sender, RoutedEventArgs e)
        {
            if (ScenarioDataGrid.SelectedIndex < 0 || ScenarioDataGrid.SelectedIndex == _scenarioList.Count)
            {
                return;
            }

            _scenarioList.RemoveAt(ScenarioDataGrid.SelectedIndex);
        }

        private void buttonStart_Click(object sender, RoutedEventArgs e)
        {
            if (_scenarioList.Count == 0)
            {
                return;
            }

            if (STFDispatcherManager.Dispatcher == null)
            {
                ConnectToDispatcher();
                if (STFDispatcherManager.Dispatcher == null)
                {
                    return;
                }
            }

            if (_isScenarioExecuting)
            {
                UpdateStatus("Stopping");

                SessionClient.Instance.Shutdown(_ticket.SessionId, _shutdownOptions);
            }
            else
            {
                _isAborted = false;
                buttonStart.IsEnabled = false;
                _sessionRetention = EnumUtil.GetByDescription<SessionLogRetention>(comboBoxRetention.Text);
                ExecuteScenario(_scenarioList.ElementAt(_currentScenarioIndex));
                _isScenarioExecuting = true;
                buttonStart.Content = "Stop";
                buttonStart.IsEnabled = true;
                buttonAbort.IsEnabled = true;
            }
        }

        private void buttonAbort_Click(object sender, RoutedEventArgs e)
        {
            UpdateStatus("Aborted");

            SessionClient.Instance.Shutdown(_ticket.SessionId, _shutdownOptions);
            _isAborted = true;
            buttonAbort.IsEnabled = false;
            buttonStart.IsEnabled = false;
        }

        private void menuOpen_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "xml",
                Filter = "STFU Save (*.xml)|*.xml",
                CheckFileExists = true
            };

            if (ofd.ShowDialog().Value != true)
            {
                return;
            }
            XElement rootElement = XElement.Load(ofd.FileName);

            if (rootElement.Element("Database") == null)
            {
                MessageBox.Show("Invalid STF Unattended Save file, Please select a valid file", "Invalid File", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (STFLoginManager.SystemDatabase.Equals(rootElement.Element("Database").Value))
            {
                LoadScenarios(rootElement.Element("Scenarios"));
                buttonRemove.IsEnabled = true;
            }
            else
            {
                MessageBox.Show("Please connect to the correct environment and try loading again", "STF Unattended", MessageBoxButton.OK, MessageBoxImage.Error);
                
            }
        }

        private void LoadScenarios(XElement scenarios)
        {
            _scenarioList.Clear();
            foreach (var scenario in scenarios.Elements("Scenario"))
            {
                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    var enterpriseScenario = EnterpriseScenario.Select(context, Guid.Parse(scenario.Value));
                    if (enterpriseScenario != null)
                    {
                        _scenarioList.Add(new ScenarioQueueList() { ScenarioName = enterpriseScenario.Name, ScenarioId = enterpriseScenario.EnterpriseScenarioId, Status = "Yet to Start" });
                    }
                    else
                    {
                        MessageBox.Show("Scenario:{0} not found".FormatWith(scenario.Value));
                    }
                }
            }
        }

        private void menuSave_Click(object sender, RoutedEventArgs e)
        {
            if (_scenarioList.Count == 0)
            {
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = "xml",
                Filter = "STFU Save (*.xml)|*.xml"
            };
            if (sfd.ShowDialog().Value)
            {
                {
                    XElement rootElement = new XElement("STFU", new XElement("Database", _currentDatabase),
                                                                new XElement("Scenarios",
                                                                    from p in _scenarioList
                                                                    select new XElement("Scenario", p.ScenarioId)));

                    rootElement.Save(sfd.FileName);
                }
            }
        }

        private void menuExit_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }

        private void ExecuteScenario(ScenarioQueueList scenario)
        {
            if (!string.IsNullOrEmpty(scenario.ScenarioName))
            {
                if (GlobalSettings.IsDistributedSystem)
                {
                    ExecuteSTF(scenario.ScenarioName, scenario.HoldId);
                }
                else
                {
                    ExecuteSTB(scenario.ScenarioName);
                }
            }
        }

        private void ExecuteSTB(string scenarioName)
        {
            string[] args = new string[8];

            args[0] = "-dispatcher";
            args[1] = Environment.MachineName;
            args[2] = "-database";
            args[3] = _currentDatabase;
            args[4] = "-scenario";
            args[5] = scenarioName;
            args[6] = "-owner";
            args[7] = UserManager.CurrentUser;

            WriteLine("Starting session...");
            using (CommandLineExec commandLine = new CommandLineExec(args))
            {
                try
                {
                    _ticket = commandLine.Ticket;
                    commandLine.HandleSessionClientEvents = false;
                    commandLine.StatusChanged += CommandLine_StatusChanged;
                    FrameworkServicesInitializer.InitializeExecution();
                    commandLine.StartSession();
                }
                catch (Exception ex)
                {
                    UpdateStatus($"Error: {ex.Message}");
                    WriteLine(ex.ToString());
                }
            }
        }

        private void ExecuteSTF(string scenario, string holdId)
        {
            WriteLine("Starting session...");

            _ticket = SessionTicket.Create(scenario);
            _ticket.SessionOwner = new UserCredential(UserManager.CurrentUser, UserManager.CurrentUserCredential.Password,
                UserManager.CurrentUserCredential.Domain) { Role = UserManager.CurrentUserRole };
            if (!string.IsNullOrEmpty(holdId))
            {
                _ticket.RequestedVMs.Add("BY_HOLDID", (from vm in SelectMachinesByHoldId(holdId) select vm.Name).ToList());
            }

            _ticket.ExpirationDate = _sessionRetention.GetExpirationDate(DateTime.Now);
            WriteLine("Created ticket {0}".FormatWith(_ticket.SessionId));
            _scenarioList.ElementAt(_currentScenarioIndex).SessionId = _ticket.SessionId;
            _startTime = DateTime.Now;
            LogSessionExecution();
            WriteLine("Initializing");
            SessionClient.Instance.Initialize(_currentDispatcher);
            SessionClient.Instance.InitiateSession(_ticket);

            UpdateStatus("Initialized");
            var assetDetails = SessionClient.Instance.Reserve(_ticket.SessionId);

            WriteLine("Reserved...{0}".FormatWith(_ticket.SessionId));
            UpdateStatus("Reserved");
            foreach (var asset in assetDetails.Where(x => x.Availability == AssetAvailability.NotAvailable))
            {
                WriteLine("Unavailable: {0}".FormatWith(asset.AssetId));
            }

            // This call to Stage() will kick off the process and as each event arrives to indicate
            // a step in the process has completed, the next step will automatically continue.
            SessionClient.Instance.Stage(_ticket.SessionId, assetDetails);
            WriteLine("Staged...{0}".FormatWith(_ticket.SessionId));
            UpdateStatus("Staged");
        }

        private void WriteLine(string message)
        {
            Dispatcher.Invoke(
                           DispatcherPriority.Normal, new Action(
                          delegate
                          {
                              TraceFactory.Logger.Debug(message);
                              textBox_DispatcherLog.Text = message + Environment.NewLine + textBox_DispatcherLog.Text;
                          }));
        }

        private void UpdateStatus(string status)
        {
            Dispatcher.Invoke(
                           DispatcherPriority.Normal, new Action(
                          delegate
                          {
                              ScenarioDataGrid.DataContext = null;
                              _scenarioList.ElementAt(_currentScenarioIndex).Status = status;
                              ScenarioDataGrid.DataContext = _scenarioList;
                          }));
        }

        private void CommandLine_StatusChanged(object sender, Utility.StatusChangedEventArgs e)
        {
            WriteLine(e.StatusMessage);
            UpdateStatus(e.StatusMessage);
        }

        private void Instance_SessionMapElementReceived(object sender, SessionMapElementEventArgs e)
        {
            if (e.MapElement.SessionId.Equals(_ticket.SessionId))
            {
                WriteLine("  [{0}] {1,40} : {2}/{3}/{4} -> [{5}]".FormatWith(e.MapElement.SessionId, e.MapElement.Name, e.MapElement.ElementType, e.MapElement.ElementSubtype, e.MapElement.State, e.MapElement.Message));

                if (e.MapElement.State == RuntimeState.Registered)
                {
                    UpdateStatus("Registered");
                }
                else if (e.MapElement.State == RuntimeState.Error)
                {
                    UpdateStatus("Error");
                }
                else if (e.MapElement.State == RuntimeState.Registered)
                {
                    UpdateStatus("Registered");
                }

                if (e.MapElement.State == RuntimeState.Completed && e.MapElement.ElementType == ElementType.Activity)
                {
                    WriteLine(e.MapElement.Message);
                }
            }
        }

        private void Instance_SessionStartupTransitionReceived(object sender, SessionStartupTransitionEventArgs e)
        {
            // Your code here
            WriteLine("{0}".FormatWith(e.Transition));

            switch (e.Transition)
            {
                case SessionStartupTransition.ReadyToValidate:
                    {
                        UpdateStatus("Validating session");
                        SessionClient.Instance.Validate(_ticket.SessionId);
                    }
                    break;

                case SessionStartupTransition.ReadyToPowerUp:
                    {
                        UpdateStatus("Powering up");
                        _revalidated = false;
                        SessionClient.Instance.PowerUp(_ticket.SessionId);
                    }
                    break;

                case SessionStartupTransition.ReadyToRun:
                    {
                        UpdateStatus("Starting");
                        SessionClient.Instance.Run(_ticket.SessionId);
                    }
                    break;

                case SessionStartupTransition.StartupComplete:
                    {
                        UpdateStatus("Running");
                    }
                    break;

                case SessionStartupTransition.ReadyToRevalidate:
                    {
                        if (_revalidated)
                        {
                            SessionClient.Instance.Shutdown(_ticket.SessionId, _shutdownOptions);
                        }
                        else
                        {
                            UpdateStatus("Error, check VM and retry");
                            WriteLine("Waiting one minute before retrying, please fix the errors in the meantime");
                            Delay.Wait(TimeSpan.FromMinutes(1));
                            SessionClient.Instance.Revalidate(_ticket.SessionId);
                            _revalidated = true;
                        }
                    }
                    break;
            }
        }

        private void Instance_SessionStateReceived(object sender, SessionStateEventArgs e)
        {
            if (e.SessionId.Equals(_ticket.SessionId))
            {
                WriteLine("{0}".FormatWith(e.State));

                switch (e.State)
                {
                    case SessionState.RunComplete:
                        {
                            // This is a place where you could add logic to decide if you want to
                            // Repeat() the run, or continue to shutdown.  For now, we are just
                            // going to shutdown.

                            SessionClient.Instance.Shutdown(_ticket.SessionId, _shutdownOptions);

                            _scenarioList.ElementAt(_currentScenarioIndex).ExecutionTime = DateTime.Now - _startTime;

                            UpdateTestResult();
                            UpdateStatus("Run Completed");
                            Dispatcher.Invoke(
                                DispatcherPriority.Normal, new Action(
                                    delegate
                                    {
                                        buttonStart.IsEnabled = false;
                                    }));
                        }
                        break;

                    case SessionState.Unavailable:
                        {
                            WriteLine(e.Message);
                        }
                        break;

                    case SessionState.ShutdownComplete:
                        {
                            UpdateStatus("Completed");
                            WriteLine("Current Scenario Index: {0}".FormatWith(_currentScenarioIndex));
                            if ((_currentScenarioIndex + 1) < _scenarioList.Count && !_isAborted)
                            {
                                _currentScenarioIndex++;
                                ExecuteScenario(_scenarioList.ElementAt(_currentScenarioIndex));
                                Dispatcher.Invoke(
                                DispatcherPriority.Normal, new Action(
                                    delegate
                                    {
                                        buttonStart.IsEnabled = true;
                                    }));
                            }
                            else
                            {
                                Dispatcher.Invoke(
                             DispatcherPriority.Normal, new Action(
                            delegate
                            {
                                buttonStart.Content = "Start";
                                _currentScenarioIndex = 0;
                                if (!_isAborted)
                                {
                                    GenerateReport(
                                        "STFU_Test_Report_{0}.xlsx".FormatWith(DateTime.Today.ToString("MMM dd")));
                                    buttonSendReport.IsEnabled = true;
                                }
                                _isAborted = false;
                                _isScenarioExecuting = false;
                                buttonAbort.IsEnabled = false;
                                buttonStart.IsEnabled = true;
                            }));
                            }
                        }
                        break;

                    case SessionState.ShuttingDown:
                        {
                            UpdateStatus("Shutting down...");
                        }
                        break;

                    case SessionState.Error:
                        {
                            UpdateStatus("Error");
                            WriteLine("Current Scenario Index: {0}".FormatWith(_currentScenarioIndex));
                            Delay.Wait(TimeSpan.FromMinutes(ErrorTimeOutInMinutes));
                            buttonStart_Click(null, null);
                        }
                        break;
                }
            }
        }

        private void UpdateTestResult()
        {
            Collection<int> testcaseIds = new Collection<int>();

            Dictionary<int, bool> testIdResult = new Dictionary<int, bool>();

            //using (SqlAdapter datalogsqlAdapter = new SqlAdapter(DataLogSqlConnection.ConnectionString))
            //{
            //    //string sqlText = "select (select count(*) from ActivityExecution where SessionId = '{0}' and UpdateType = 'Completed') as PassedCount, (select count(*) from ActivityExecution where SessionId = '{0}') as TotalCount".FormatWith(_ticket.SessionId);
            //    string sqlText = "select VirtualResourceMetadataId, MIN(case when UpdateType = 'Completed' then 1 else 0 end) as TestResult from ActivityExecution where SessionId = {0}  group by VirtualResourceMetadataId".FormatWith(_ticket.SessionId);
            //    var reader = datalogsqlAdapter.ExecuteReader(sqlText);
            //    if (reader != null)
            //    {
            //        while (reader.Read())
            //        {
            //            TestIdResult.Add(Guid.Parse(reader["VirtualResourceId"].ToString()), Convert.ToBoolean(reader.GetInt32(1))) ;
            //        }

            //    }

            //    reader.Close();
            //}

            DataLogContext traceContext = DbConnect.DataLogContext();
            {
                var activityExecutions = traceContext.SessionData(_ticket.SessionId).Activities;

                var failedActivities = activityExecutions.Where(x => x.Status == "Failed");

                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    foreach (var metadataId in activityExecutions.Select(x => x.ResourceMetadataId.Value).Distinct())
                    {
                        testcaseIds.Add(VirtualResource.Select(context, VirtualResourceMetadata.Select(context, metadataId).VirtualResourceId).TestCaseId);
                    }

                    foreach (var testcaseId in testcaseIds.Distinct())
                    {
                        testIdResult.Add(testcaseId, true);
                    }
                    foreach (var failedActivity in failedActivities)
                    {
                        testIdResult.Remove(VirtualResource.Select(context, VirtualResourceMetadata.Select(context, failedActivity.ResourceMetadataId.Value).VirtualResourceId).TestCaseId);

                        testIdResult.Add(VirtualResource.Select(context, VirtualResourceMetadata.Select(context, failedActivity.ResourceMetadataId.Value).VirtualResourceId).TestCaseId, false);
                    }
                }
            }
            _scenarioList.ElementAt(_currentScenarioIndex).TestResult = testIdResult;
        }

        private void EmailTestResult()
        {
            StringBuilder mailMessageBody = new StringBuilder();

            if (MailingList.Count == 0)
            {
                MessageBox.Show("Please update the mailing list and try again", "Error", MessageBoxButton.OK, MessageBoxImage.Error, MessageBoxResult.OK);
                return;
            }
            using (MailMessage message = new MailMessage())
            {
                message.From = new MailAddress("noreply@hp.com", "STF Unattended Tool");
                foreach (var emailId in MailingList)
                {
                    message.To.Add(emailId);
                }

                message.Subject = "Test Report from STF Unattend Test Session - {0} {1}".FormatWith(DateTime.Now.ToShortDateString(), DateTime.Now.ToShortTimeString());
                message.BodyEncoding = Encoding.ASCII;
                message.IsBodyHtml = true;
                mailMessageBody.AppendLine("Hello {0} <br />".FormatWith(MailingList.First().Split('.').First()));
                mailMessageBody.AppendLine("<br />");
                mailMessageBody.AppendLine("<br />");
                mailMessageBody.AppendLine("Please find the attached test summary of the unattended test execution:<br>");
                mailMessageBody.AppendLine(Environment.NewLine);
                foreach (var scenario in _scenarioList)
                {
                    mailMessageBody.AppendLine("-----------------------------------------------------------<br />");
                    mailMessageBody.AppendLine("Scenario Name: {0}<br />".FormatWith(scenario.ScenarioName));
                    mailMessageBody.AppendLine("Total Time: {0}<br />".FormatWith(scenario.ExecutionTime));
                    mailMessageBody.AppendLine("Total Test Cases Executed: {0}<br />".FormatWith(scenario.TotalActivities));
                    mailMessageBody.AppendLine("Passed Test Cases: {0} <br />".FormatWith(scenario.PassedActivities));
                    mailMessageBody.AppendLine("Detailed Test Case Result: <br />");
                    mailMessageBody.AppendLine("{0},<br />".FormatWith(TestSummary(scenario.TestResult)));
                    mailMessageBody.AppendLine("-----------------------------------------------------------<br />");
                }
                message.Body = mailMessageBody.ToString();

                using (SmtpClient mailClient = new SmtpClient(EMailServer))
                {
                    message.Attachments.Add( new Attachment(_destinationReportFileName));
                    mailClient.Send(message);
                }

                //List<string> attachments = new List<string>();

                //var controller = string.IsNullOrEmpty(GlobalSettings.Items["EwsUrl"]) ? new EmailController() : new EmailController(new Uri(GlobalSettings.Items["EwsUrl"]), ExchangeVersionSetting.v2010);

                //controller.Send(message, attachments);
            }
            TraceFactory.Logger.Info("Email message sent");
        }

        /// <summary>
        /// Generates the excel report and sends it in an email
        /// </summary>
        private void GenerateReport(string filename)
        {
            List<string> sessionIds = _scenarioList.Select(x => x.SessionId).ToList();
            
            // Prompt the user for the report destination and create the report
            string fileSession = (sessionIds.Count() == 1 ? sessionIds.First() : "Multiple Sessions");
            string reportName = Path.GetFileNameWithoutExtension(ReportTemplateFileName);

            if (string.IsNullOrEmpty(filename))
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog
                {
                    DefaultExt = "xlsx",
                    Filter = "Excel Documents (*.xlsx)|*.xlsx",
                    FileName =
                        "{0} {1} {2}".FormatWith(DateTime.Today.ToString("MMM dd", CultureInfo.CurrentCulture),
                            fileSession,
                            reportName),
                    InitialDirectory = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                };

                if (saveFileDialog.ShowDialog() == true)
                {
                    filename = saveFileDialog.FileName;
                }

            }
            else
            {
                filename = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), filename);
            }

            if (string.IsNullOrEmpty(filename))
            {
                return;
            }

            try
            {
                _destinationReportFileName = filename;
                ReportingEngine.GenerateReport(ReportTemplateFileName, filename, sessionIds);

                EmailTestResult();

                // Copy the template to the destination
                _destinationReportFileName = filename;
                File.Copy(ReportTemplateFileName, _destinationReportFileName, true);

                    // Create the report
                   //_excelReport  = new ExcelReportFile(_destinationReportFileName);

                   // //Thread the report creation
                   // BackgroundWorker generateReportWorker = new BackgroundWorker();
                   // generateReportWorker.DoWork += generateReportWorker_DoWork;
                   // generateReportWorker.RunWorkerCompleted += generateReportWorker_RunWorkerCompleted;
                   // generateReportWorker.RunWorkerAsync(new object[] { _excelReport, sessionIds, DataLogSqlConnection.ConnectionString });
                    
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "Report Creation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
                TraceFactory.Logger.Error(ex.Message, ex);
            }
        }

        //private void generateReportWorker_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        //{
        //   _excelReport.Close(); 
        //   EmailTestResult();
        //}

        //private void generateReportWorker_DoWork(object sender, DoWorkEventArgs e)
        //{
        //    object[] args = (object[])e.Argument;

        //    try
        //    {
        //        ReportingEngine.CreateExcelReport((ExcelReportFile)args[0], (IEnumerable<string>)args[1], (string)args[2]);
                
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "Report Creation Failure", MessageBoxButton.OK, MessageBoxImage.Error);
        //        TraceFactory.Logger.Error(ex.Message, ex);
        //    }
        //}

        private void Instance_DispatcherExceptionReceived(object sender, ExceptionDetailEventArgs e)
        {
            WriteLine("ERROR -----{0}{1}{0}{2}".FormatWith(Environment.NewLine, e.Message, e.Detail));
        }

        private void ConnectToDispatcher()
        {
            if (string.IsNullOrEmpty(STFLoginManager.SystemDatabase))
            {
                if (STFLoginManager.Login())
                {
                    _currentDatabase = STFLoginManager.SystemDatabase;
                }
                else
                {
                    return;
                }
            }
            if (STFDispatcherManager.ConnectToDispatcher())
            {
                _currentDispatcher = STFDispatcherManager.Dispatcher.HostName;
            }
        }

        private void buttonShowLog_Click(object sender, RoutedEventArgs e)
        {

            if (_isScenarioExecuting)
            {
                SessionLogWindow logWindow = new SessionLogWindow();
                {
                    logWindow.WindowStartupLocation = WindowStartupLocation.CenterOwner;
                    logWindow.DebugLog = SessionClient.Instance.GetLogData(_ticket.SessionId);
                    logWindow.ShowDialog();
                }
            }
        }

        private IEnumerable<VirtualMachine> SelectMachinesByHoldId(string holdId)
        {
            return VirtualMachine.Select(VMPowerState.PoweredOff, VMUsageState.Available, holdId: holdId, includePlatforms: true);
        }

        private void LogSessionExecution()
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                var log = new ConfigurationObjectHistory(_ticket.ScenarioId, "Executed", _ticket.SessionOwner.UserName)
                {
                    ConfigurationObjectName = _ticket.SessionName,
                    ConfigurationObjectType = "Session"
                };
                context.ConfigurationObjectHistorySet.AddObject(log);
            }
        }

        private string TestSummary(Dictionary<int, bool> testResult)
        {
            StringBuilder summaryText = new StringBuilder();
            foreach (var test in testResult)
            {
                summaryText.AppendLine(test.Value
                    ? "TestCase {0}: Passed <br />".FormatWith(test.Key)
                    : "TestCase {0}: Failed <br />".FormatWith(test.Key));
            }

            if (string.IsNullOrEmpty(summaryText.ToString()))
            {
                summaryText.AppendLine("Not Executed");
            }

            return summaryText.ToString();
        }

        private void menuMail_Click(object sender, RoutedEventArgs e)
        {
            MailingListWindow maillist = new MailingListWindow();
            if (maillist.ShowDialog() == true)
            {
            }
        }

        private void ButtonSendReport_OnClick(object sender, RoutedEventArgs e)
        {
            GenerateReport(string.Empty);
        }


        #region DragAndDrop
        private void ScenarioDataGrid_OnBeginningEdit(object sender, DataGridBeginningEditEventArgs e)
        {
            IsEditing = true;

            if (IsDragging)
            {
                ResetDragDrop();
            }
        }

        private void ResetDragDrop()
        {
            IsDragging = false;
            popupDrag.IsOpen = false;
            ScenarioDataGrid.IsReadOnly = false;
        }

        private void ScenarioDataGrid_OnCellEditEnding(object sender, DataGridCellEditEndingEventArgs e)
        {
            IsEditing = false;
        }

        private void ScenarioDataGrid_OnPreviewMouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (IsEditing)
            {
                return;
            }

            var row = UIHelpers.TryFindFromPoint<DataGridRow>((UIElement)sender, e.GetPosition(ScenarioDataGrid));
            if (row == null || row.IsEditing || row.IsNewItem)
            {
                return;
            }

            //set flag that indicates we're capturing mouse movements
            IsDragging = true;
            DraggedItem = (ScenarioQueueList)row.Item;
        }

        private void Scenariogrid_OnMouseLeftButtonUp(object sender, MouseButtonEventArgs e)
        {
            if (!IsDragging || IsEditing)
            {
                return;
            }

            ScenarioQueueList targetItem = (ScenarioQueueList)ScenarioDataGrid.SelectedItem;

            if (targetItem == null || !ReferenceEquals(DraggedItem, targetItem))
            {
                //remove the source from the list
                _scenarioList.Remove(DraggedItem);

                //get target index
                var targetIndex = _scenarioList.IndexOf(targetItem);

                //move source at the target's location
                _scenarioList.Insert(targetIndex, DraggedItem);

                //select the dropped item
                ScenarioDataGrid.SelectedItem = DraggedItem;
            }

            //reset
            ResetDragDrop();

        }

        private void Scenariogrid_OnMouseMove(object sender, MouseEventArgs e)
        {
            if (!IsDragging || e.LeftButton != MouseButtonState.Pressed)
            {
                return;
            }

            //display the popup if it hasn't been opened yet
            if (!popupDrag.IsOpen)
            {
                //switch to read-only mode
                ScenarioDataGrid.IsReadOnly = true;

                //make sure the popup is visible
                popupDrag.IsOpen = true;
            }


            Size popupSize = new Size(popupDrag.ActualWidth, popupDrag.ActualHeight);
            popupDrag.PlacementRectangle = new Rect(e.GetPosition(this), popupSize);

            //make sure the row under the grid is being selected
            Point position = e.GetPosition(ScenarioDataGrid);
            var row = UIHelpers.TryFindFromPoint<DataGridRow>(ScenarioDataGrid, position);
            if (row != null)
            {
                ScenarioDataGrid.SelectedItem = row.Item;
            }
        }
#endregion
    }

    [Serializable]
    public class ScenarioQueueList
    {
        public Guid ScenarioId { get; set; }

        public string ScenarioName { get; set; }

        //public string Retention { get; set; }

        [XmlIgnore]
        public string HoldId { get; set; }

        [XmlIgnore]
        public string Status { get; set; }

        [XmlIgnore]
        public string SessionId { get; set; }

        [XmlIgnore]
        public TimeSpan ExecutionTime { get; set; }

        [XmlIgnore]
        public int TotalActivities { get { return TestResult.Count; } }

        [XmlIgnore]
        public int PassedActivities { get { return TestResult.Count(x => x.Value); } }

        [XmlIgnore]
        public string Result { get { return "{0}/{1} Passed".FormatWith(PassedActivities, TotalActivities); } }

        [XmlIgnore]
        public Dictionary<int, bool> TestResult { get; set; }

        [XmlIgnore]
        public string ToolTip
        {
            get
            {
                if (string.IsNullOrEmpty(Status))
                {
                    return string.Empty;
                }
                if (Status.Equals("Completed"))
                {
                    return TestSummary();
                }
                return string.Empty;
            }
        }

        public ScenarioQueueList()
        {
            TestResult = new Dictionary<int, bool>();
        }

        private string TestSummary()
        {
            StringBuilder summaryText = new StringBuilder();
            foreach (var test in TestResult)
            {
                summaryText.AppendLine(test.Value
                    ? "TestCase {0}: Passed".FormatWith(test.Key)
                    : "TestCase {0}: Failed".FormatWith(test.Key));
            }

            return summaryText.ToString();
        }
    }

    /// <summary>
    /// retrieves the Retention List
    /// </summary>
    public class RetentionList : Collection<string>
    {
        public RetentionList()
        {
            foreach (string retention in SessionLogRetentionHelper.ExpirationList)
            {
                Add(retention);
            }
        }
    }

    /// <summary>
    /// Class to retrieve the Hold IDs
    /// </summary>
    public class HoldIds : Collection<string>
    {
        public HoldIds()
        {
            if (!IsDesignMode())
            {
                using (AssetInventoryContext context = new AssetInventoryContext())
                {
                    foreach (string holdId in VirtualMachineReservation.SelectHoldIds(context, VMPowerState.PoweredOff))
                    {
                        Add(holdId);
                    }
                }
            }
        }

        public static bool IsDesignMode()
        {
            return LicenseManager.UsageMode == LicenseUsageMode.Designtime;
        }
    }

    /// <summary>
    /// Converts visibility to negate bool
    /// </summary>
    public class BoolToOppositeBoolConverter : IValueConverter
    {
        #region IValueConverter Members

        public object Convert(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            if (targetType != typeof(bool))
                throw new InvalidOperationException("The target must be a boolean");

            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            CultureInfo culture)
        {
            throw new NotSupportedException();
        }

        #endregion IValueConverter Members
    }
}