using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Sirius;
using HP.RDL.DDSContants;
using HP.RDL.EDT.AutoTestHelper.Controls;
using HP.RDL.EDT.AutoTestHelper.DDS;
using HP.RDL.EDT.ClientDDS;
using HP.ScalableTest;
using HP.ScalableTest.Core.AssetInventory.Reservation;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.UI.SessionExecution;
using HP.ScalableTest.Utility;
using HP.ScalableTest.WindowsAutomation;
using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Windows;
using System.Windows.Threading;
using System.Xml.Linq;
using HP.ScalableTest.Core.Security;

namespace HP.RDL.EDT.AutoTestHelper
{
    /// <summary>
    /// Interaction logic for ExecutionWindow.xaml
    /// </summary>
    public partial class ExecutionWindow : Window
    {
        private readonly TestInstanceData _testInstanceData;
        private string _currentDatabase;
        private string _currentDispatcher;
        private static SessionTicket _ticket;

        // private int _currentScenarioIndex;
        private SessionLogRetention _sessionRetention;

        private DateTime _startTime;
        private readonly ShutdownOptions _shutdownOptions;
        private bool _isEnded;
        private bool _isPaused;
        private bool _isScenarioExecuting;
        private bool _isAborted;

        //private Guid _testInstanceId;
        private IDevice _currentDevice;

        private PrintDeviceInfo _printAsset;
        private const int ErrorTimeOutInMinutes = 1;
        private bool _revalidated;
        private readonly AccessDDS _accessDds;
        private Queue<ScenarioQueueItem> _executionQueue = new Queue<ScenarioQueueItem>();
        private ScenarioQueueItem _currentExecutionScenario;
        private string _testType;
        private const string ApplicationName = "EDT AutoTestHelper";
        private int _baseFirmwareIndex = 0;
        private List<string> _baseFiles = new List<string>();
        private List<string> _targetFiles = new List<string>();

        public ExecutionWindow()
        {
            InitializeComponent();
        }

        public ExecutionWindow(TestInstanceData testInstanceData)
        {
            _testInstanceData = testInstanceData;
            InitializeComponent();
            ScenarioDataGrid.DataContext = _testInstanceData.ActiveScenarios;
            StatusGrid.DataContext = _testInstanceData;

            _currentDatabase = STFLoginManager.SystemDatabase;
            STFDispatcherManager.DispatcherChanged += STFDispatcherManager_DispatcherChanged;
            SessionClient.Instance.DispatcherExceptionReceived += Instance_DispatcherExceptionReceived;
            SessionClient.Instance.SessionStateReceived += Instance_SessionStateReceived;
            SessionClient.Instance.SessionStartupTransitionReceived += Instance_SessionStartupTransitionReceived;
            SessionClient.Instance.SessionMapElementReceived += Instance_SessionMapElementReceived;

            string environment = "Production";
#if DEBUG
            environment = "Development";
#endif
            _accessDds = new AccessDDS(environment);

            _shutdownOptions = new ShutdownOptions
            {
                AllowWorkerToComplete = false,
                CopyLogs = false,
                PowerOff = true,
                PowerOffOption = VMPowerOffOption.PowerOff
            };
        }

        private void STFDispatcherManager_DispatcherChanged(object sender, EventArgs e)
        {
            //ignore
        }

        private void ExecutionWindow_OnLoaded(object sender, RoutedEventArgs e)
        {
            GlobalSettings.Load(STFLoginManager.SystemDatabase);

            FrameworkServicesInitializer.InitializeConfiguration();
            FrameworkServicesInitializer.InitializeExecution();

            _printAsset = _testInstanceData.DeviceAssetInfoCollection.OfType<PrintDeviceInfo>().First();
            try
            {
                _currentDevice = DeviceFactory.Create(_printAsset.Address, _printAsset.AdminPassword);
            }
            catch (Exception exception)
            {
                MessageBox.Show(exception.JoinAllErrorMessages(), ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                Close();
            }

            if (!string.IsNullOrEmpty(_testInstanceData.BaseFirmwarePath) && !string.IsNullOrEmpty(_testInstanceData.TargetFirmwarePath))
            {
                _baseFiles = Directory.GetFiles(_testInstanceData.BaseFirmwarePath, "*.bdl", SearchOption.TopDirectoryOnly).ToList();
                _targetFiles = Directory.GetFiles(_testInstanceData.TargetFirmwarePath, "*.bdl",
                    SearchOption.TopDirectoryOnly).ToList();
                TraceFactory.Logger.Debug($"Found {_baseFiles.Count} files for Base Firmware & {_targetFiles.Count} for Target");
                TraceFactory.Logger.Debug($"Base files: {string.Join(",", _baseFiles)}");
                TraceFactory.Logger.Debug($"Target files: {string.Join(",", _targetFiles)}");
            }
            else
            {
                TraceFactory.Logger.Debug($"Firmware directory path is empty. {_testInstanceData.BaseFirmwarePath} & {_testInstanceData.TargetFirmwarePath}");
            }
        }

        private bool CheckPreRequisites()
        {
            //check if ipp tool is install
            if (!Directory.Exists(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ProgramFilesX86), "ipptool")))
            {
                OpenFileDialog ofd = new OpenFileDialog
                {
                    Multiselect = false,
                    CheckFileExists = true,
                    Filter = "Ipp Tool Installer (*.msi) | *.msi"
                };

                if (ofd.ShowDialog() == true)
                {
                    if (!ofd.FileName.Contains("ipp"))
                    {
                        MessageBox.Show("Please select the IPP Tool Installer and try again", ApplicationName);
                        return false;
                    }

                    if (!SystemSetup.Run(ofd.FileName, UserManager.CurrentUser.ToNetworkCredential(), false))
                    {
                        MessageBox.Show("Failed to Install IPP Tool, install it manually and try again.", ApplicationName);
                        return false;
                    }
                }
            }

            return true;
        }

        private void CreateRandomDistributedScenarioList()
        {
            var executionScenarios = _testInstanceData.ActiveScenarios.SelectMany(x => Enumerable.Repeat(x, x.Distribution)).ToList();
            executionScenarios.Shuffle();
            _executionQueue = new Queue<ScenarioQueueItem>(executionScenarios);
            WriteLine($"[Info]: Found {_executionQueue.Count} Tests and Shuffled the order.");
        }

        private void StartButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!CheckPreRequisites())
                return;

            if (_executionQueue.Count < 1)
            {
                CreateRandomDistributedScenarioList();
            }

            if (_testInstanceData.ActiveScenarios.Count == 0)
                return;

            if (STFDispatcherManager.Dispatcher == null)
            {
                ConnectToDispatcher();
                if (STFDispatcherManager.Dispatcher == null || string.IsNullOrEmpty(_currentDispatcher))
                {
                    return;
                }
            }
            else
            {
                _currentDispatcher = STFDispatcherManager.Dispatcher.HostName;
            }

            _isEnded = false;
            _isAborted = false;

            _sessionRetention = SessionLogRetention.ThreeMonths;
            _testInstanceData.StartPrintCount = GetPrintCount(_currentDevice);
            _testInstanceData.StartScanCount = GetScanCount(_currentDevice);
            //create a testinstance id for this session
            var testScenario = _accessDds.GetRun(_testInstanceData.RunId).TestScenario;
            _testType = "None";
            switch (testScenario)
            {
                case "CST: Operational - 30 percent Sleep Wake":
                    {
                        _testType = "ExtDuration";
                    }
                    break;

                case "CST: Firmware Update":
                    {
                        _testType = "FIM";
                    }
                    break;

                case "CST: Power Booting":
                    {
                        _testType = "PowerBoot";
                    }
                    break;
            }

            if (_accessDds.CreateTestInstance(_testType, _currentDevice.Address, UserManager.CurrentUserName,
                _testInstanceData.StartPrintCount, _testInstanceData.StartScanCount, Environment.MachineName,
                DateTime.UtcNow))
            {
                StartButton.IsEnabled = false;
                ExecuteScenario(_executionQueue.Dequeue());
            }

            if (_accessDds.IsError)
            {
                WriteLine("[Error]: Error in Initialisation of Test Instance.");
                WriteLine(_accessDds.GetLastError);
            }
        }

        private void PauseButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (_isPaused && SessionClient.Instance.GetSessionState(_currentExecutionScenario.SessionId) == SessionState.PauseComplete)
            {
                PauseButton.IsEnabled = false;
                UpdateStatus("Resuming");
                SessionClient.Instance.Resume(_currentExecutionScenario.SessionId);
                _isPaused = false;
                PauseButton.Content = "Pause";
            }
            else
            {
                if (_isEnded && !_isScenarioExecuting)
                {
                    return;
                }

                //if the scenario is not in running state, return without doing anything
                if (SessionClient.Instance.GetSessionState(_currentExecutionScenario.SessionId) != SessionState.Running)
                    return;

                PauseButton.IsEnabled = false;
                UpdateStatus("Pausing");
                try
                {
                    SessionClient.Instance.Pause(_currentExecutionScenario.SessionId);
                    _isPaused = true;
                    PauseButton.Content = "Resume";
                }
                catch (Exception exception)
                {
                    MessageBox.Show(exception.Message, ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error,
                        MessageBoxResult.OK);
                    WriteLine($"[Error]:{exception.JoinAllErrorMessages()}");
                }
            }
        }

        private void EndButton_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_isAborted)
                EndTestRun();
        }

        private void EndTestRun()
        {
            _isEnded = true;
            var stopreasons = _accessDds.GetDomainValues("StopReason");
            ObservableCollection<object> stopReasons = new ObservableCollection<object>(stopreasons);
            ListDialogBox dialog = new ListDialogBox { Items = stopReasons, Prompt = "Select the reason for Ending the sesssion" };
            if (!dialog.ShowDialog().Value)
            {
                _isEnded = false;
                return;
            }

            if (_isScenarioExecuting)
                SessionClient.Instance.Shutdown(_ticket.SessionId, _shutdownOptions);

            _testInstanceData.StopPrintCount = GetPrintCount(_currentDevice);
            _testInstanceData.StopScanCount = GetScanCount(_currentDevice);

            var reason = dialog.SelectedItem as string;
            _accessDds.CloseTestInstance(_accessDds.TestInstanceId, _testInstanceData.StopPrintCount,
                _testInstanceData.StopScanCount, DateTime.UtcNow, reason);
            WriteLine("Test Run Ended.");
            StartButton.IsEnabled = true;
            PauseButton.IsEnabled = false;
            AbortButton.IsEnabled = false;
            _isScenarioExecuting = false;
            _isAborted = false;
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

        private void Instance_SessionMapElementReceived(object sender, SessionMapElementEventArgs e)
        {
            if (e.MapElement.SessionId.Equals(_ticket.SessionId))
            {
                WriteLine("  [{0}] {1,40} : {2}/{3}/{4} -> [{5}]".FormatWith(e.MapElement.SessionId, e.MapElement.Name,
                    e.MapElement.ElementType, e.MapElement.ElementSubtype, e.MapElement.State, e.MapElement.Message));

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
                            WriteLine("[Info]: Waiting one minute before retrying, please fix the errors in the meantime");
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
                            SessionResetManager resetManager = new SessionResetManager();
                            resetManager.LogSessionReset(_ticket.SessionId);
                            SessionClient.Instance.Shutdown(_ticket.SessionId, _shutdownOptions);
                            _currentExecutionScenario.ExecutionTime =
                                DateTime.UtcNow - _startTime;
                            Dispatcher.Invoke(DispatcherPriority.Normal,
                                new Action(delegate
                                {
                                    PauseButton.IsEnabled = false;
                                    AbortButton.IsEnabled = false;
                                }));
                            UpdateStatus("Run Completed");
                        }
                        break;

                    case SessionState.Unavailable:
                        {
                            WriteLine(e.Message);
                        }
                        break;

                    case SessionState.PauseComplete:
                        {
                            UpdateStatus("Pause completed");
                            Dispatcher.Invoke(DispatcherPriority.Normal,
                                new Action(delegate { PauseButton.IsEnabled = true; }));
                        }
                        break;

                    case SessionState.Running:
                        {
                            UpdateStatus("Running");
                            Dispatcher.Invoke(DispatcherPriority.Normal,
                                new Action(delegate
                                {
                                    PauseButton.IsEnabled = true;
                                    AbortButton.IsEnabled = true;
                                }));
                        }
                        break;

                    case SessionState.ShutdownComplete:
                        {
                            UpdateTestResult();
                            LogSessionData();
                            UpdateStatus("Completed");

                            LockToken scenarioToken = new GlobalLockToken("Scenario", TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(1));
                            ExecutionServices.CriticalSection.Run(scenarioToken, () =>
                            {
                                try
                                {
                                    using (EnterpriseTestUIController controller = new EnterpriseTestUIController())
                                    {
                                        controller.Load();
                                        controller.Delete(_ticket.ScenarioIds.First());
                                        controller.Commit();
                                    }
                                }
                                catch (Exception exception)
                                {
                                    TraceFactory.Logger.Debug(exception.JoinAllErrorMessages());
                                    WriteLine(
                                        "An error occurred while deleting the scenario. Please remove it manually.");
                                }
                            });
                            //remove the cloned scenario

                            _isScenarioExecuting = false;

                            //sleep for few seconds before starting next test

                            Thread.Sleep(TimeSpan.FromSeconds(5));
                            //check if the device is running and in ready state, else don't go with the next test case
                            if (EndTestExecution())
                                return;

                            if (_executionQueue.Count > 0)
                                Dispatcher.Invoke(DispatcherPriority.Normal,
                                    new Action(delegate { ExecuteScenario(_executionQueue.Dequeue()); }));
                            else
                            {
                                WriteLine("[Info]: Test Run Complete, you may now end the test session.");
                                Dispatcher.Invoke(DispatcherPriority.Normal,
                                    new Action(delegate
                                    {
                                        PauseButton.IsEnabled = false;
                                        AbortButton.IsEnabled = false;
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
                            //  WriteLine("Current Scenario Index: {0}".FormatWith(_currentScenarioIndex));
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                MessageBox.Show(
                                    "There is an error in the Session State. Please check the device and log a fault event if needed.",
                                    ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                                var faultWindow = new FaultEventHandler(_accessDds);
                                faultWindow.ShowDialog();
                                WriteLine("[Info]: Waiting for a minute before resuming the execution.");
                                Thread.Sleep(TimeSpan.FromMinutes(ErrorTimeOutInMinutes));
                                if (_executionQueue.Count > 0 && !_isEnded)
                                    ExecuteScenario(_executionQueue.Dequeue());
                            }));
                        }
                        break;
                }
            }
        }

        private bool EndTestExecution()
        {
            if (!Retry.UntilTrue(() => IsDeviceRunning(_currentDevice), 10, TimeSpan.FromSeconds(5)))
            {
                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                    {
                        //looks like device is dead, so probably post a warning to user
                        MessageBox.Show(
                            "Unable to communicate with the device. Please check the device and ensure it is in Ready state and then press OK to continue.",
                            ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                        var result = MessageBox.Show("Do you wish to enter a fault for this event?",
                            ApplicationName,
                            MessageBoxButton.YesNo, MessageBoxImage.Question);

                        if (result == MessageBoxResult.Yes)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                var faultWindow = new FaultEventHandler(_accessDds);
                                faultWindow.ShowDialog();
                            }));
                        }
                    }
                ));
            }

            if (_isAborted)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(delegate
                    {
                        var result = MessageBox.Show("Do you want to continue with the test execution?",
                            ApplicationName, MessageBoxButton.YesNo, MessageBoxImage.Question,
                            MessageBoxResult.Yes);
                        if (result == MessageBoxResult.No)
                        {
                            EndTestRun();
                        }
                    }));
                _isAborted = false;
            }

            if (_isEnded)
            {
                Dispatcher.Invoke(DispatcherPriority.Normal,
                    new Action(delegate
                    {
                        StartButton.IsEnabled = true;
                        PauseButton.IsEnabled = false;
                    }));
                return true;
            }

            return false;
        }

        private void Instance_DispatcherExceptionReceived(object sender, ExceptionDetailEventArgs e)
        {
            WriteLine("ERROR -----{0}{1}{0}{2}".FormatWith(Environment.NewLine, e.Message, e.Detail));
        }

        private void ExecuteScenario(ScenarioQueueItem scenario)
        {
            if (string.IsNullOrEmpty(scenario.ScenarioName) || _isEnded)
                return;

            _currentExecutionScenario = _testInstanceData.ActiveScenarios.Find(x => x.ScenarioId == scenario.ScenarioId);
            if (_currentExecutionScenario == null)
            {
                WriteLine($"[Error]: Could not find the scenario in the execution list. Scenario: {scenario.ScenarioName}");
                return;
            }

            var targetScenarioId = CloneScenario(_currentExecutionScenario);

            WriteLine("[Info]: Starting session...");
            _isScenarioExecuting = true;
            _ticket = SessionTicket.Create(new[] { targetScenarioId }, _currentExecutionScenario.ScenarioName, 4);
            if (_testInstanceData.TestInstanceId == Guid.Empty)
            {
                _testInstanceData.TestInstanceId = Guid.Parse(_accessDds.TestInstanceId);
            }
            
            _ticket.SessionOwner = UserManager.CurrentUser;
            
            if (!string.IsNullOrEmpty(scenario.HoldId))
            {
                _ticket.RequestedVMs.Add("BY_HOLDID",
                    (from vm in SelectMachinesByHoldId(scenario.HoldId) select vm.Name).ToList());
            }

            _ticket.ExpirationDate = _sessionRetention.GetExpirationDate(DateTime.UtcNow);

            WriteLine("[Info]: Created ticket {0}".FormatWith(_ticket.SessionId));

            _currentExecutionScenario.SessionId = _ticket.SessionId;
            _currentExecutionScenario.StartPrintCount = GetPrintCount(_currentDevice);
            _currentExecutionScenario.StartScanCount = GetScanCount(_currentDevice);

            _ticket.SessionCycle = _testInstanceData.RunName;
            _ticket.SessionType = _accessDds.GetRun(_testInstanceData.RunId).TestScenario;
            _ticket.SessionNotes = _accessDds.GetRun(_testInstanceData.RunId).BuildValue;
            _testInstanceData.SessionIds.Add(_ticket.SessionId);

            _startTime = DateTime.UtcNow;

            LogSessionExecution();

            WriteLine("[Info]: Initializing");
            SessionClient.Instance.Initialize(_currentDispatcher);
            SessionClient.Instance.InitiateSession(_ticket);

            UpdateStatus("Initialized");
            var assetDetails = SessionClient.Instance.Reserve(_ticket.SessionId);
            WriteLine("Reserved...{0}".FormatWith(_ticket.SessionId));
            UpdateStatus("Reserved");
            foreach (var asset in assetDetails.Where(x => x.Availability != AssetAvailability.Available))
            {
                WriteLine("[Info]: Unavailable: {0}".FormatWith(asset.AssetId));
            }

            if (assetDetails.Any(x => x.Availability != AssetAvailability.Available))
            {
                WriteLine("[Error]: Shutting down the test due to asset unavailability.");
                SessionClient.Instance.Close(_ticket.SessionId);
                _isAborted = true;
                return;
            }

            WriteLine("[Info]: Overriding CRC option to false for all assets");
            foreach (var printDeviceDetail in assetDetails.OfType<PrintDeviceDetail>())
            {
                printDeviceDetail.UseCrc = false;
            }

            // This call to Stage() will kick off the process and as each event arrives to indicate
            // a step in the process has completed, the next step will automatically continue.
            SessionClient.Instance.Stage(_ticket.SessionId, assetDetails);

            WriteLine("[Info]: Staged...{0}".FormatWith(_ticket.SessionId));
            UpdateStatus("Staged");
        }

        private Guid CloneScenario(ScenarioQueueItem scenarioQueueItem)
        {
            Guid targetScenarioId;
            using (EnterpriseTestContext context = new EnterpriseTestContext(_currentDatabase))
            {
                var sourceScenario = context.EnterpriseScenarios.First(x => x.EnterpriseScenarioId == scenarioQueueItem.ScenarioId);
                EnterpriseScenario targetScenario;
                using (EnterpriseTestUIController controller = new EnterpriseTestUIController())
                {
                    controller.Load();
                    controller.GetEntityObject(sourceScenario.EnterpriseScenarioId);
                    var folder = context.ConfigurationTreeFolders.FirstOrDefault(x => x.Name == Environment.MachineName);
                    if (folder == null)
                    {
                        var folderId = controller.CreateFolder(null);
                        context.ConfigurationTreeFolders.First(x => x.ConfigurationTreeFolderId == folderId).Name =
                            Environment.MachineName;
                        folder = context.ConfigurationTreeFolders.First(x => x.ConfigurationTreeFolderId == folderId);
                    }

                    targetScenarioId = controller.CopyScenario(sourceScenario.EnterpriseScenarioId, folder.ConfigurationTreeFolderId);
                    targetScenario = context.EnterpriseScenarios.First(x => x.EnterpriseScenarioId == targetScenarioId);

                    foreach (var virtualResouce in sourceScenario.VirtualResources)
                    {
                        var targetVirtualResource = virtualResouce.Copy();
                        targetVirtualResource.EnterpriseScenarioId = targetScenarioId;
                        targetScenario.VirtualResources.Add(targetVirtualResource);
                    }
                }

                context.SaveChanges();
                var scenarioAssetUsage = context.VirtualResourceMetadataAssetUsages.Where(n => n.VirtualResourceMetadata.VirtualResource.EnterpriseScenarioId == targetScenarioId).ToList();
                foreach (var virtualResourceMetadataAssetUsage in scenarioAssetUsage)
                {
                    XElement asd = XElement.Parse(virtualResourceMetadataAssetUsage.AssetSelectionData);
                    AssetSelectionData assetSelectionData = Serializer.Deserialize<AssetSelectionData>(asd);

                    assetSelectionData.SelectedAssets.Clear();
                    foreach (var deviceAsset in _testInstanceData.DeviceAssetInfoCollection)
                    {
                        assetSelectionData.SelectedAssets.Add(deviceAsset.AssetId);
                    }
                    XElement newAsd = Serializer.Serialize(assetSelectionData);
                    virtualResourceMetadataAssetUsage.AssetSelectionData = newAsd.ToString();
                }

                //now check for firmware flash files

                foreach (var virtualResource in targetScenario.VirtualResources)
                {
                    var virtualResourceMetadataList = virtualResource.VirtualResourceMetadataSet.Select(x => x.VirtualResourceMetadataId).ToList();

                    if (_baseFiles.Count != 0 && _targetFiles.Count != 0)
                    {
                        var activities = VirtualResourceMetadata.Select(context, virtualResourceMetadataList)
                            .Where(x => x.MetadataType == "FlashFirmware");

                        foreach (var activity in activities)
                        {
                            if (activity.MetadataType == "FlashFirmware")
                            {
                                XElement metadataElement = XElement.Parse(activity.Metadata);
                                XNamespace xns = metadataElement.GetDefaultNamespace();

                                var isDownGrade =
                                    Convert.ToBoolean(metadataElement.Element(xns + "IsDowngrade")?.Value);

                                if (metadataElement.Element(xns + "FirmwareFileName").Value !=
                                    _baseFiles.ElementAt(_baseFirmwareIndex) &&
                                    metadataElement.Element(xns + "FirmwareFileName").Value !=
                                    _targetFiles.ElementAt(0))
                                {
                                    metadataElement.Element(xns + "FirmwareFileName").Value = isDownGrade
                                        ? _baseFiles.ElementAt(_baseFirmwareIndex)
                                        : _targetFiles.ElementAt(0);
                                    activity.Metadata = metadataElement.ToString();
                                    WriteLine(
                                        $"[Info]: Firmware File for activity {activity.Name} is {metadataElement.Element(xns + "FirmwareFileName").Value}.");
                                }

                                if (isDownGrade)
                                {
                                    WriteLine($"[Info]: Base Firmware File Counter is at {_baseFirmwareIndex}");
                                    _baseFirmwareIndex++;
                                    if (_baseFirmwareIndex >= _baseFiles.Count)
                                        _baseFirmwareIndex = 0;
                                }
                            }
                        }
                    }

                    //now check for orphaned driverless printing plugin and add a random file to print
                    var driverlessActivities = VirtualResourceMetadata.Select(context, virtualResourceMetadataList).Where(x => x.MetadataType == "DriverlessPrinting");
                    foreach (var driverlessActivity in driverlessActivities)
                    {
                        if (driverlessActivity.DocumentUsage == null || string.IsNullOrEmpty(driverlessActivity.DocumentUsage.DocumentSelectionData))
                        {
                            var prnCollection = ConfigurationServices.DocumentLibrary.GetDocuments(new List<DocumentExtension>() { new DocumentExtension("PRN", "Raw", "application/raw") });
                            var randomDocument = prnCollection.GetRandom();
                            var docSelectionData = new DocumentSelectionData(new List<Document> { randomDocument });
                            driverlessActivity.DocumentUsage = VirtualResourceMetadataDocumentUsage.CreateVirtualResourceMetadataDocumentUsage(driverlessActivity.VirtualResourceMetadataId, Serializer.Serialize(docSelectionData).ToString());
                            WriteLine($"[Info]: Found activity {driverlessActivity.Name} without a document, it is now set to {randomDocument.FileName}");
                        }
                    }
                }

                context.SaveChanges();
            }

            return targetScenarioId;
        }

        private void UpdateTestResult()
        {
            Collection<int> testcaseIds = new Collection<int>();
            Dictionary<int, bool> testIdResult = new Dictionary<int, bool>();

            _currentExecutionScenario.StopPrintCount = GetPrintCount(_currentDevice);
            _currentExecutionScenario.StopScanCount = GetScanCount(_currentDevice);
            //if the end count of print and scan is less than what it was at begining we can say that the firwmware was downgraded and counter reset, so let's reset the start count to 0
            if (_currentExecutionScenario.StopScanCount < _currentExecutionScenario.StartScanCount ||
                _currentExecutionScenario.StopPrintCount < _currentExecutionScenario.StartPrintCount)
            {
                _currentExecutionScenario.StartScanCount = _currentExecutionScenario.StartPrintCount = 0;
            }

            _testInstanceData.TotalImages += (_currentExecutionScenario.StopScanCount - _currentExecutionScenario.StartScanCount) +
                                          (_currentExecutionScenario.StopPrintCount - _currentExecutionScenario.StartPrintCount);
            DataLogContext traceContext = DbConnect.DataLogContext();
            {
                var activityExecutions = traceContext.SessionData(_ticket.SessionId).Activities;

                var failedActivities = activityExecutions.Where(x => x.Status == "Failed" || x.Status == "Error");

                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    foreach (var metadataId in activityExecutions.Select(x => x.ResourceMetadataId.Value).Distinct())
                    {
                        testcaseIds.Add(VirtualResource.Select(context,
                            VirtualResourceMetadata.Select(context, metadataId).VirtualResourceId).TestCaseId);
                    }

                    foreach (var testcaseId in testcaseIds.Distinct())
                    {
                        testIdResult.Add(testcaseId, true);
                    }

                    foreach (var failedActivity in failedActivities)
                    {
                        var virtualResource = VirtualResource.Select(context,
                            VirtualResourceMetadata.Select(context, failedActivity.ResourceMetadataId.Value)
                                .VirtualResourceId);
                        if (virtualResource.TestCaseId > 0)
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                MessageBox.Show(
                                    $"The activity {failedActivity.ActivityName} failed after firmware upgrade was performed. Please log a fault event for the same.",
                                    ApplicationName);
                                var faultWindow = new FaultEventHandler(_accessDds);
                                faultWindow.ShowDialog();
                            }));
                        }

                        testIdResult.Remove(VirtualResource.Select(context,
                            VirtualResourceMetadata.Select(context, failedActivity.ResourceMetadataId.Value)
                                .VirtualResourceId).TestCaseId);

                        testIdResult.Add(
                            VirtualResource.Select(context,
                                VirtualResourceMetadata.Select(context, failedActivity.ResourceMetadataId.Value)
                                    .VirtualResourceId).TestCaseId, false);
                    }
                }
            }
            _currentExecutionScenario.TestResult = testIdResult;
            _currentExecutionScenario.ExecutedCount++;

            RefreshSummary();
        }

        private void LogSessionData()
        {
            if (_testType == "FIM")
                LogFirmwareEvents();
            else if (_testType == "PowerBoot")
                LogPowerBootEvents();
            else if (_testType == "ExtDuration")
                LogSleepWakeEvents();

            LogFaultEvents();
            CollectDeviceLogs();

        }

        private void LogFirmwareEvents()
        {
            //first check for FIM operations
            using (DataLogContext traceContext = DbConnect.DataLogContext())
            {
                var activityExecutions = traceContext.SessionData(_ticket.SessionId).Activities
                    .Where(x => x.ActivityType == "FlashFirmware" || x.ActivityType == "EdtIntervention");

                foreach (var activityExecution in activityExecutions)
                {
                    if (activityExecution.ExecutionDetails.Any())
                    {
                        var upgradeDetail = activityExecution.ExecutionDetails.FirstOrDefault(x => x.Label == "FirmwareFlashUpgrade");
                        if (upgradeDetail != null && upgradeDetail.Message.Equals("true", StringComparison.OrdinalIgnoreCase))
                        {
                            var updateBeginDetail = activityExecution.ExecutionDetails.FirstOrDefault(x => x.Label == "FirmwareUpdateBegin");
                            var baseFirmwareVersion = updateBeginDetail?.Message;
                            var updateStartTime = updateBeginDetail?.DetailDateTime;

                            var fimMethod = activityExecution.ExecutionDetails.FirstOrDefault(x => x.Label == "FirmwareFlashMethod")?.Message;

                            if (activityExecution.Status != "Passed")
                            {
                                //we have a upgrade failure, insert this firmware event and show a fault event and chain it

                                _accessDds.InsertFIM(_testInstanceData.TestInstanceId.ToString(), fimMethod,
                                    activityExecution.StartDateTime.GetValueOrDefault(DateTime.Now).ToUniversalTime(),
                                    baseFirmwareVersion, "N/A", 0, _ticket.SessionName);
                                Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                                {
                                    MessageBox.Show(
                                        "The firmware upgrade activity has failed. Please check and log a fault event.",
                                        ApplicationName, MessageBoxButton.OK, MessageBoxImage.Warning);
                                    var faultWindow = new FaultEventHandler(_accessDds);
                                    faultWindow.ShowDialog();
                                }));
                            }
                            else
                            {
                                var updateEndDetail =
                                    activityExecution.ExecutionDetails.FirstOrDefault(x =>
                                        x.Label == "FirmwareUpdateEnd");
                                var targetFirmwareVersion = updateEndDetail?.Message;
                                var updateEndTime = updateEndDetail?.DetailDateTime;

                                int timeToReady = (int)(updateEndTime - updateStartTime)
                                    .GetValueOrDefault(TimeSpan.Zero).TotalSeconds;

                                if (string.IsNullOrEmpty(targetFirmwareVersion))
                                    targetFirmwareVersion = "N/A";

                                _accessDds.InsertFIM(_testInstanceData.TestInstanceId.ToString(), fimMethod,
                                    updateEndTime.GetValueOrDefault(DateTime.Now).ToUniversalTime(),
                                    baseFirmwareVersion, targetFirmwareVersion, timeToReady, _ticket.SessionName);
                            }

                            _testInstanceData.TotalFimCycles++;
                        }
                    }
                    else
                    {
                        if (activityExecution.ActivityType == "FlashFirmware" && activityExecution.Status != "Passed")
                        {
                            Dispatcher.Invoke(DispatcherPriority.Normal, new Action(delegate
                            {
                                MessageBox.Show(
                                    "The firmware upgrade activity has failed to start. Please check if the firmware bundle is valid and log a fault event if required.",
                                    ApplicationName, MessageBoxButton.OK, MessageBoxImage.Warning);
                                var faultWindow = new FaultEventHandler(_accessDds);
                                faultWindow.ShowDialog();
                            }));
                        }
                    }
                }
            }
        }

        private void LogPowerBootEvents()
        {
            using (DataLogContext traceContext = DbConnect.DataLogContext())
            {
                var activityExecutions = traceContext.SessionData(_ticket.SessionId).Activities.Where(x => x.ActivityType == "EdtIntervention");

                foreach (var activityExecution in activityExecutions)
                {
                    if (activityExecution.ExecutionDetails.Any())
                    {
                        var bootMethodDetail = activityExecution.ExecutionDetails.FirstOrDefault(x => x.Label == "PowerBootMethod");
                        var bootStartDetail = activityExecution.ExecutionDetails.FirstOrDefault(x => x.Label == "PowerBootStart");
                        var bootEndDetail = activityExecution.ExecutionDetails.FirstOrDefault(x => x.Label == "PowerBootEnd");
                        if (bootEndDetail != null && bootStartDetail != null && bootMethodDetail != null)
                        {
                            _accessDds.InsertPowerBoot(_testInstanceData.TestInstanceId.ToString(), bootMethodDetail.Message, "Script", bootStartDetail.DetailDateTime.ToUniversalTime(),
                            (int)(bootEndDetail.DetailDateTime - bootStartDetail.DetailDateTime).TotalSeconds, _ticket.SessionName);
                            _testInstanceData.TotalPowerCycles++;
                        }
                    }
                }
            }
        }

        private void LogSleepWakeEvents()
        {
            string sleepMethod = string.Empty;
            string wakeMethod = string.Empty;
            DateTime startTime = DateTime.Now;
            DateTime endTime = DateTime.Now;

            using (DataLogContext traceContext = DbConnect.DataLogContext())
            {
                var activityExecutions = traceContext.SessionData(_ticket.SessionId).Activities.Where(x => x.ActivityType == "EdtIntervention");

                foreach (var activityExecution in activityExecutions)
                {
                    if (activityExecution.ExecutionDetails.Any())
                    {
                        var sleepDetail = activityExecution.ExecutionDetails.FirstOrDefault(x => x.Label == "SleepMethod");
                        var wakeDetail = activityExecution.ExecutionDetails.FirstOrDefault(x => x.Label == "WakeMethod");
                        if (sleepDetail != null)
                        {
                            sleepMethod = sleepDetail.Message;
                            startTime = sleepDetail.DetailDateTime;
                        }
                        if (wakeDetail != null)
                        {
                            wakeMethod = wakeDetail.Message;
                            endTime = wakeDetail.DetailDateTime;
                        }
                    }
                }
                if (!string.IsNullOrEmpty(sleepMethod) && !string.IsNullOrEmpty(wakeMethod))
                {
                    _accessDds.InsertSleepWake(_testInstanceData.TestInstanceId.ToString(), sleepMethod, wakeMethod, startTime.ToUniversalTime(), (int)(endTime - startTime).TotalSeconds, _ticket.SessionName);
                }
                else
                {
                    WriteLine("[INFO]: No Sleep/Wake events found.");
                }
            }
        }

        private void LogFaultEvents()
        {
            //check for fault events entered
            using (var dataLogConnection = new SqlConnection(DbConnect.DataLogConnectionString.ToString()))
            {
                string getFaultEventQuery = $"select * from FaultEvents where SessionId = '{_ticket.SessionId}'";
                DataTable faultEventTable = new DataTable("FaultEvents");
                FillTable(getFaultEventQuery, faultEventTable, dataLogConnection);
                foreach (DataRow dataRow in faultEventTable.Rows)
                {
                    string eventType = dataRow["EventTypeId"].ToString();
                    eventType = Enum<ErrorEventTypes>.Parse(eventType, true).GetDescription();

                    string eventSubType = dataRow["EventSubTypeId"].ToString();
                    eventSubType = Enum<ErrorSubevents>.Parse(eventSubType, true).GetDescription();

                    string opInProgress = dataRow["OpInProgressId"].ToString();
                    opInProgress = Enum<FaultOpInProgress>.Parse(opInProgress, true).GetDescription();

                    string recovery = dataRow["RecoveryId"].ToString();
                    recovery = eventType == "Error"
                        ? Enum<ErrorEventRecoveries>.Parse(recovery, true).GetDescription()
                        : Enum<JamRecoveries>.Parse(recovery, true).GetDescription();

                    string jobDisposition = dataRow["JobDispositionId"].ToString();
                    jobDisposition = Enum<FaultJobDispositions>.Parse(jobDisposition, true).GetDescription();

                    string rootCause = dataRow["RootCauseId"].ToString();
                    rootCause = Enum<FaultRootCauses>.Parse(rootCause, true).GetDescription();

                    var eventTime = DateTime.Parse(dataRow["EventTime"].ToString()).ToUniversalTime();
                    string eventDetails = dataRow["EventDetails"].ToString();
                    int recoveryTime = Convert.ToInt32(dataRow["RecoveryTime"]);
                    string faultCode = dataRow["FaultCode"].ToString();

                    if (dataRow["IsLinked"].ToString() == "1" || dataRow["IsLinked"].ToString() == "true")
                    {
                        string previousEventId = string.Empty;
                        switch (_testType)
                        {
                            case "ExtDuration":
                                previousEventId = _accessDds.SleepWakeId;
                                break;

                            case "PowerBoot":
                                previousEventId = _accessDds.PowerBootId;
                                break;

                            case "FIM":
                                previousEventId = _accessDds.FirmwareId;
                                break;
                        }

                        if (!string.IsNullOrEmpty(previousEventId))
                        {
                            if (!_accessDds.InsertChainedFault(_testInstanceData.TestInstanceId.ToString(),
                                previousEventId,
                                eventType, eventSubType, opInProgress, recovery, jobDisposition, eventTime,
                                recoveryTime, eventDetails, string.Empty, faultCode, rootCause))
                            {
                                WriteLine($"[Error]: Unable to insert a chained fault event, {_accessDds.GetLastError}");
                            }
                        }

                        {
                            WriteLine($"[Error]: Unable to find previous event to link, Please check and try again. Tried to insert row at {dataRow["SessionId"]}:{dataRow["FaultEventsId"]} ");
                        }
                    }
                    else
                    {
                        if (!_accessDds.InsertFault(_testInstanceData.TestInstanceId.ToString(), eventType, eventSubType, opInProgress, recovery, jobDisposition, eventTime, recoveryTime, eventDetails, string.Empty,
                            faultCode, rootCause))
                        {
                            WriteLine($"[Error]: Unable to insert the fault event, {_accessDds.GetLastError}");
                        }
                    }
                }
            }
        }

        private void CollectDeviceLogs()
        {
            string localPath =
                $@"C:\VirtualResource\Distribution\SolutionTesterConsole\Logs\{
                        _currentExecutionScenario.SessionId
                    }\MachineDataLog";
            string remotePath = $"ftp://{_currentDevice.Address}:221/MachineData/Log";
            try
            {
                var logFiles = GetLogFiles(_currentDevice.Address);
                foreach (var logFile in logFiles)
                {
                    DownloadFtpFile($"{remotePath}/{logFile}", $"{localPath}\\{logFile}");
                }

            }
            catch (Exception e)
            {
                TraceFactory.Logger.Debug("An Error occurred while downloading machinedata logs.");
                TraceFactory.Logger.Debug(e);
                
            }
            
        }

        private List<string> GetLogFiles(string address)
        {
            var files = new List<string>();
            FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create($"ftp://{address}:221/MachineData/Log");
            downloadRequest.Method = WebRequestMethods.Ftp.ListDirectoryDetails;
            downloadRequest.Credentials = new NetworkCredential("anonymous", _currentDevice.AdminPassword);
            downloadRequest.UsePassive = true;
            downloadRequest.UseBinary = true;

            FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();

            using (var responseStream = downloadResponse.GetResponseStream())
            {
                StreamReader sReader = new StreamReader(responseStream);
                var fileContentString = sReader.ReadToEnd();
                var fileListings = fileContentString.Split(new[] { Environment.NewLine }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var fileListing in fileListings)
                {
                    if(fileListing.Contains(".log"))
                        files.Add(fileListing.Remove(0, fileListing.LastIndexOf(" ", StringComparison.OrdinalIgnoreCase) + 1));
                }
            }

            return files;
        }

        private void DownloadFtpFile(string remotePath, string localPath)
        {
            var directory = Path.GetDirectoryName(localPath);
            Directory.CreateDirectory(directory);

            FtpWebRequest downloadRequest = (FtpWebRequest)WebRequest.Create(remotePath);
            downloadRequest.Method = WebRequestMethods.Ftp.DownloadFile;
            downloadRequest.Credentials = new NetworkCredential("anonymous", _currentDevice.AdminPassword);
            downloadRequest.UsePassive = true;
            downloadRequest.UseBinary = true;

            FtpWebResponse downloadResponse = (FtpWebResponse)downloadRequest.GetResponse();

            using (var responseStream = downloadResponse.GetResponseStream())
            {
                StreamReader sReader = new StreamReader(responseStream);
                var fileContentString = sReader.ReadToEnd();
                File.WriteAllText(localPath, fileContentString);
            }
        }

        private void FillTable(string selectQuery, DataTable dataTable, SqlConnection sourceConnection)
        {
            using (SqlCommand selectCommand = new SqlCommand(selectQuery, sourceConnection))
            {
                using (SqlDataAdapter da = new SqlDataAdapter(selectCommand))
                {
                    da.Fill(dataTable);
                    WriteLine($"[Info]: Retrieved {dataTable.Rows.Count} rows of Fault Events Data.");
                }
            }
        }

        private void LogSessionExecution()
        {
            //using (EnterpriseTestContext context = new EnterpriseTestContext())
            //{
            //    var log = new ConfigurationObjectHistory
            //    {
            //        ConfigurationObjectHistoryId = SequentialGuid.NewGuid(),
            //        ConfigurationObjectId = _ticket.ScenarioIds.FirstOrDefault(),
            //        ModificationType = "Executed",
            //        ModificationTime = DateTime.Now,
            //        Modifier = _ticket.SessionOwner.UserName,
            //        ConfigurationObjectName = _ticket.SessionName,
            //        ConfigurationObjectType = "Session"
            //    };
            //    context.ConfigurationObjectHistorySet.AddObject(log);
            //}
        }

        private IEnumerable<VirtualMachine> SelectMachinesByHoldId(string holdId)
        {
            return VirtualMachine.Select(ScalableTest.Data.EnterpriseTest.VMPowerState.PoweredOff, ScalableTest.Data.EnterpriseTest.VMUsageState.Available, holdId: holdId,
                includePlatforms: true);
        }

        private void WriteLine(string message)
        {
            Dispatcher.Invoke(
                DispatcherPriority.Normal, new Action(
                    delegate
                    {
                        TraceFactory.Logger.Debug(message);
                        TextBoxDispatcherLog.Text = $"{DateTime.Now:s}:{message}{Environment.NewLine}{TextBoxDispatcherLog.Text}";
                    }));
        }

        private void UpdateStatus(string status)
        {
            Dispatcher.Invoke(
                DispatcherPriority.Normal, new Action(
                    delegate
                    {
                        ScenarioDataGrid.DataContext = null;
                        _currentExecutionScenario.Status = status;
                        ScenarioDataGrid.DataContext = _testInstanceData.ActiveScenarios;
                    }));
        }

        private void RefreshSummary()
        {
            Dispatcher.Invoke(
                DispatcherPriority.Normal, new Action(
                    delegate
                    {
                        StatusGrid.DataContext = null;
                        StatusGrid.DataContext = _testInstanceData;
                    }));
        }

        private int GetPrintCount(IDevice device)
        {
            if (!_printAsset.Attributes.HasFlag(AssetAttributes.Printer))
            {
                WriteLine("[Info]: The device does not have Print Capability, skipping Print Count.");
                return 0;
            }

            JediDevice jediDevice = new JediDevice(device.Address);
            SiriusDevice siriusDevice = new SiriusDevice(device.Address);
            if (device is JediDevice)
            {
                try
                {
                    var oidValues = jediDevice.Snmp.Walk("1.3.6.1.4.1.11.2.3.9.4.2.1.4.1.8.5.1");
                    return oidValues.Sum(x => Convert.ToInt32(x.Value));
                }
                catch (Exception exception)
                {
                    WriteLine($"[Error]: Exception while trying to get print count. Please update the print count manually. {exception.Message}");
                    return 0;
                }
            }

            if (device is SiriusDevice)
            {
                try
                {
                    return Convert.ToInt32(siriusDevice.Snmp.Get("1.3.6.1.4.1.11.2.3.9.4.2.1.4.1.2.5.0"));
                }
                catch (Exception e)
                {
                    WriteLine($"[Error]: Exception while trying to get print count. Please update the print count manually. {e.Message}");
                    return 0;
                }
            }

            return 0;
        }

        private int GetScanCount(IDevice device)
        {
            if (!_printAsset.Attributes.HasFlag(AssetAttributes.Scanner))
            {
                WriteLine("[Info]: The device does not have Scan capability, skipping Scan Count.");
                return 0;
            }
            JediDevice jediDevice = new JediDevice(device.Address);
            SiriusDevice siriusDevice = new SiriusDevice(device.Address);
            if (device is JediDevice)
            {
                int sides = 0;
                string[] scanSources = { "21", "59", "60" };

                foreach (string scanSource in scanSources)
                {
                    try
                    {
                        var oid = $"1.3.6.1.4.1.11.2.3.9.4.2.1.2.2.1.{scanSource}.0";
                        sides += Convert.ToInt32(jediDevice.Snmp.Get(oid));
                    }
                    catch (Exception exception)
                    {
                        WriteLine($"[Error]: Exception while trying to get scan count. Please update the scan count manually. {exception.Message}");
                        //ignore
                    }
                }

                return sides;
            }

            if (device is SiriusDevice)
            {
                int sides = 0;

                string[] sourceStrings = { "20", "21" }; //20 - ADF, 21 - Scanner Glass

                foreach (string sourceString in sourceStrings)
                {
                    try
                    {
                        var oid = $"1.3.6.1.4.1.11.2.3.9.4.2.1.2.2.1.{sourceString}.0";
                        sides += Convert.ToInt32(siriusDevice.Snmp.Get(oid));
                    }
                    catch (Exception exception)
                    {
                        //ignore
                        WriteLine($"[Error]: Exception while trying to get scan count. Please update the scan count manually. {exception.Message}");
                    }
                }

                return sides;
            }

            return 0;
        }

        private bool IsDeviceRunning(IDevice device)
        {
            var status = device.GetDeviceStatus();
            WriteLine($"[Info]: Device Status is {status}");
            return (status == DeviceStatus.Running || status == DeviceStatus.Warning);
        }

        private void ExecutionWindow_OnClosing(object sender, CancelEventArgs e)
        {
            if (EndButton.IsEnabled)
            {
                MessageBox.Show("Please End the current run before closing the window.", ApplicationName,
                    MessageBoxButton.OK, MessageBoxImage.Stop);
                e.Cancel = true;
            }

            STFDispatcherManager.DispatcherChanged -= STFDispatcherManager_DispatcherChanged;
            SessionClient.Instance.DispatcherExceptionReceived -= Instance_DispatcherExceptionReceived;
            SessionClient.Instance.SessionStateReceived -= Instance_SessionStateReceived;
            SessionClient.Instance.SessionStartupTransitionReceived -= Instance_SessionStartupTransitionReceived;
            SessionClient.Instance.SessionMapElementReceived -= Instance_SessionMapElementReceived;
            _testInstanceData.ActiveScenarios.Clear();
            ScenarioDataGrid.DataContext = null;
        }

        private void AbortButton_OnClick(object sender, RoutedEventArgs e)
        {
            var dialogResult = MessageBox.Show("This will abort the current test. Do you want to continue?",
                ApplicationName, MessageBoxButton.YesNo, MessageBoxImage.Stop, MessageBoxResult.No);

            if (dialogResult == MessageBoxResult.Yes)
            {
                SessionClient.Instance.Shutdown(_ticket.SessionId, _shutdownOptions);
                AbortButton.IsEnabled = false;
                PauseButton.IsEnabled = false;
                _isAborted = true;
            }
        }
    }
}