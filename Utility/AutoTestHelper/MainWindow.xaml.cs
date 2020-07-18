using HP.RDL.DurationDataCoreLibrary;
using HP.RDL.EDT.ClientDDS;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.UI;
using HP.ScalableTest.UI.Framework;
using HP.ScalableTest.UI.ScenarioConfiguration.Import;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;
using System.Xml.Linq;
using HP.RDL.EDT.AutoTestHelper.Controls;
using HP.ScalableTest;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Utility;

//using System.Windows.Forms;
using MessageBox = System.Windows.MessageBox;
using OpenFileDialog = Microsoft.Win32.OpenFileDialog;
using SaveFileDialog = Microsoft.Win32.SaveFileDialog;
using Newtonsoft.Json;
using Extension = HP.RDL.DurationDataCoreLibrary.Extension;

namespace HP.RDL.EDT.AutoTestHelper
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private string _currentDatabase;
        private readonly AccessDDS _accessDds;
        private readonly AssetSelectionControl _edtAssetSelectionControl;
        private readonly IPAddressControl _addressControl;
        private ObservableCollection<ScenarioQueueItem> _scenarioList = new ObservableCollection<ScenarioQueueItem>();
        private bool _isFim;
        private const string ApplicationName = "EDT AutoTestHelper";
        private ObservableCollection<string> _groupCollection = new ObservableCollection<string>();
        private static readonly Regex NumericRegex = new Regex("[^0-9.-]+");
        public MainWindow()
        {
            InitializeComponent();
            AppDomain.CurrentDomain.UnhandledException += CurrentDomain_UnhandledException;
            _edtAssetSelectionControl = AssetFormHost.Child as AssetSelectionControl;
            _addressControl = BashCollectorAddressHost.Child as IPAddressControl;
            _addressControl?.DataBindings.Add("Enabled", BashCollectorCheckBox, "IsChecked");

            string environment = "Production";
#if DEBUG
            environment = "Development";
#endif
            _accessDds = new AccessDDS(environment);

        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            if (STFLoginManager.Login())
            {
                _currentDatabase = STFLoginManager.SystemDatabase;
                //Set whether STF or STB based on the worker type in the database.
                GlobalSettings.IsDistributedSystem = false;
                //string officeWorkerType = VirtualResourceType.OfficeWorker.ToString();
                //using (EnterpriseTestContext dataContext = new EnterpriseTestContext(_currentDatabase))
                //{
                //    GlobalSettings.IsDistributedSystem = dataContext.VirtualResources.Any(r => r.ResourceType == officeWorkerType);
                //}
                LoggedInTextBlock.Text = $"Logged in as: {UserManager.CurrentUserName} to {STFLoginManager.SystemDatabase}";
            }
            else
            {
                Environment.Exit(1);
            }

            _edtAssetSelectionControl?.Initialize(AssetAttributes.None);
            GlobalSettings.Load(_currentDatabase);
            FrameworkServicesInitializer.InitializeConfiguration();

       
            BuildComboBox.DataContext = _accessDds.GetBuilds();
            ProductComboBox.DataContext = _accessDds.GetProducts();
            ScenarioDataGrid.DataContext = _scenarioList;
            GroupComboBox.DataContext = _groupCollection;
        }

        private void CurrentDomain_UnhandledException(object sender, UnhandledExceptionEventArgs e)
        {
            TraceFactory.Logger.Debug(e.ExceptionObject);
            MessageBox.Show(e.ExceptionObject.ToString(), ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            
        }

        #region UIOperations

        private void BuildComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (ProductComboBox.SelectedIndex == -1)
                return;

            RunComboBox.DataContext =
                _accessDds.GetRuns((Guid)BuildComboBox.SelectedValue, (Guid)ProductComboBox.SelectedValue);
        }

        private void ProductComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (BuildComboBox.SelectedIndex == -1)
                return;

            RunComboBox.DataContext =
                _accessDds.GetRuns((Guid)BuildComboBox.SelectedValue, (Guid)ProductComboBox.SelectedValue);
        }

        private void RunComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (RunComboBox.SelectedIndex == -1)
                return;

            _isFim = false;
            var scenarioMaps = ConfigurationManager.GetSection("ScenarioMap") as NameValueCollection;
            var selectedRun = _accessDds.GetRun((Guid)RunComboBox.SelectedValue);
            var scenarioMap = scenarioMaps.Get(selectedRun.TestScenario);

            LoadScenarios(scenarioMap);

            if (selectedRun.TestScenario == "CST: Firmware Update")
                _isFim = true;
        }

        private void LoadScenarios(string testScenario)
        {

            _scenarioList = new ObservableCollection<ScenarioQueueItem>();
            _groupCollection = new ObservableCollection<string>(){"None"};
            //find out if the scenario has multiple values
            var testScenarios = testScenario.Split(',').Select(x=>x.RemoveWhiteSpace());
            using (EnterpriseTestContext context = new EnterpriseTestContext(_currentDatabase))
            {
                var scenarios = context.EnterpriseScenarios.Where(x => testScenarios.Contains(x.Company));

                foreach (var enterpriseScenario in scenarios)
                {
                    string distribution, groupName;
                    if(string.IsNullOrEmpty(enterpriseScenario.ScenarioSettings))
                        continue;
                    var settings = LegacySerializer.DeserializeDataContract<ScenarioSettings>(enterpriseScenario.ScenarioSettings);
                    try
                    {
                        if (!settings.ScenarioCustomDictionary.TryGetValue("Distribution", out distribution))
                        {
                            distribution = "1";
                        }
                    }
                    catch
                    {
                        distribution = "1";
                    }

                    try
                    {
                        if (!settings.ScenarioCustomDictionary.TryGetValue("Group", out groupName))
                        {
                            groupName = "None";
                        }

                    }
                    catch
                    {
                        groupName = "None";
                    }

                    if (groupName.Contains(","))
                    {
                        var groups = groupName.Split(',');
                        foreach (var group in groups)
                        {
                            if(!_groupCollection.Contains(group))
                                _groupCollection.Add(group);
                        }

                    }
                    else
                    {
                        if (!_groupCollection.Contains(groupName))
                            _groupCollection.Add(groupName);
                    }
                   

                    var description = enterpriseScenario.Description;

                    _scenarioList.Add(new ScenarioQueueItem
                    {
                        ScenarioId = enterpriseScenario.EnterpriseScenarioId,
                        ScenarioName = enterpriseScenario.Name,
                        Description = description,
                        Distribution = Convert.ToInt32(distribution),
                        GroupName = groupName
                    });
                }
            }

            ScenarioDataGrid.DataContext = _scenarioList;
            GroupComboBox.DataContext = _groupCollection;
            ScenarioCountTextBlock.Text = _scenarioList.Count(x => x.Active).ToString("D2");
            TextBoxTotalScenario.Text = _scenarioList.Where(x => x.Active).Sum(x => x.Distribution).ToString("D2");
        }

        private void BaseFirmwareBrowseButton_OnClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                Description = @"Select the folder containing Base firmware files"
            };

            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            if (Directory.EnumerateFiles(dialog.SelectedPath).Any(x => x.EndsWith(".bdl", StringComparison.OrdinalIgnoreCase)))
                BaseFirmwareTextBox.Text = dialog.SelectedPath;
            else
            {
                MessageBox.Show(
                    "The selected path does not contain any firmware files (*.bdl), Please choose the correct path and try again!",
                    ApplicationName, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void TargetFirmwareBrowseButton_OnClick(object sender, RoutedEventArgs e)
        {
            FolderBrowserDialog dialog = new FolderBrowserDialog
            {
                ShowNewFolderButton = false,
                Description = @"Select the folder containing Target firmware files"
            };

            var result = dialog.ShowDialog();
            if (result != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            if (Directory.EnumerateFiles(dialog.SelectedPath).Any(x => x.EndsWith(".bdl", StringComparison.OrdinalIgnoreCase)))
                TargetFirmwareTextBox.Text = dialog.SelectedPath;
            else
            {
                MessageBox.Show(
                    "The selected path does not contain any firmware files (*.bdl), Please choose the correct path and try again!",
                    ApplicationName, MessageBoxButton.OK, MessageBoxImage.Warning);
            }
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            string exceptionMessage=string.Empty;
            if (!_edtAssetSelectionControl.HasSelection)
            {
                MessageBox.Show("Please select a device to continue.", ApplicationName, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (_scenarioList.Count == 0)
            {
                MessageBox.Show("There are no scenarios to execute!", ApplicationName, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            if (_isFim && (string.IsNullOrEmpty(BaseFirmwareTextBox.Text) ||
                           string.IsNullOrEmpty(TargetFirmwareTextBox.Text) ||
                           !Directory.Exists(BaseFirmwareTextBox.Text) ||
                           !Directory.Exists(TargetFirmwareTextBox.Text)))
            {
                MessageBox.Show("Please validate the firmware locations for the device.", ApplicationName, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }
           

            //kick start the execution
            TestInstanceData activeTestInstanceData =
                new TestInstanceData
                {
                    ActiveScenarios = _scenarioList.Where(x => x.Active).ToList(),
                    DeviceAssetInfoCollection = ConfigurationServices.AssetInventory.GetAssets(_edtAssetSelectionControl.AssetSelectionData.SelectedAssets),
                    BaseFirmwarePath = BaseFirmwareTextBox.Text,
                    TargetFirmwarePath = TargetFirmwareTextBox.Text,
                    RunName = RunComboBox.Text,
                    RunId = (Guid)RunComboBox.SelectedValue
                };

            if (BashCollectorCheckBox.IsChecked.HasValue)
                activeTestInstanceData.BashCollectorHost = BashCollectorCheckBox.IsChecked.Value ? _addressControl.Text : string.Empty;

            this.Visibility = Visibility.Collapsed;
            try
            {
                ExecutionWindow executionWindow = new ExecutionWindow(activeTestInstanceData);
                executionWindow.ShowDialog();
            }
            catch(Exception ex)
            {
                exceptionMessage = Extension.JoinAllErrorMessages(ex);
            }

            this.Visibility = Visibility.Visible;
            if(!string.IsNullOrEmpty(exceptionMessage))
            MessageBox.Show($"An error occurred. {exceptionMessage}", ApplicationName, MessageBoxButton.OK,
                MessageBoxImage.Error);
        }

        private void GroupComboBox_OnSelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (GroupComboBox.SelectedIndex == -1)
                return;

            string groupName = GroupComboBox.SelectedItem.ToString();
            ScenarioDataGrid.DataContext = groupName == "None" ? _scenarioList : new ObservableCollection<ScenarioQueueItem>(_scenarioList.Where(x => x.GroupName.Contains(groupName)));

            //we have to deactivate all scenarios and enable them based on filter
            foreach (var scenarioQueueItem in _scenarioList)
            {
                scenarioQueueItem.Active = groupName == "None" || scenarioQueueItem.GroupName.Contains(groupName);
            }

            ScenarioCountTextBlock.Text = _scenarioList.Count(x => x.Active).ToString("D2");
            TextBoxTotalScenario.Text = _scenarioList.Where(x => x.Active).Sum(x => x.Distribution).ToString("D2");
        }

        #endregion UIOperations

        #region MenuOperations

        private void MenuOpen_OnClick(object sender, RoutedEventArgs e)
        {
            OpenFileDialog ofd = new OpenFileDialog
            {
                DefaultExt = "xml",
                Filter = "EDT AutoTestHelper Save (*.xml)|*.xml",
                CheckFileExists = true
            };

            var result = ofd.ShowDialog();

            if (result.HasValue && result.Value == false)
                return;

            XElement rootElement = XElement.Load(ofd.FileName);

            if (rootElement.Element("Database") == null)
            {
                MessageBox.Show("Invalid EDT AutoTestHelper Save file, Please select a valid file", ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (STFLoginManager.SystemDatabase.Equals(rootElement.Element("Database").Value))
            {
                _currentDatabase = rootElement.Element("Database")?.Value;
                _edtAssetSelectionControl.Initialize(new AssetSelectionData(ConfigurationServices.AssetInventory.GetAsset(rootElement.Element("AssetId")?.Value)), AssetAttributes.Printer);

                var fimElement = rootElement.Element("FIM");
                if (fimElement != null)
                {
                    BaseFirmwareTextBox.Text = fimElement.Element("Base").Value;
                    TargetFirmwareTextBox.Text = fimElement.Element("Target").Value;
                }

                var ddsElement = rootElement.Element("DDS");
                if (ddsElement != null)
                {
                    var buildId = Guid.Parse(ddsElement.Element("BuildId").Value);
                    var productId = Guid.Parse(ddsElement.Element("Product").Value);
                    var runId = Guid.Parse(ddsElement.Element("RunId").Value);

                    var buildList = BuildComboBox.ItemsSource as List<Build>;
                    BuildComboBox.SelectedIndex = buildList.FindIndex(x => x.BuildId == buildId);
                    var productList = ProductComboBox.ItemsSource as List<Product>;
                    ProductComboBox.SelectedIndex = productList.FindIndex(x => x.ProductId == productId);
                    var runList = RunComboBox.ItemsSource as List<Run>;
                    RunComboBox.SelectedIndex = runList.FindIndex(x => x.RunId == runId);
                }

                if (!string.IsNullOrEmpty(rootElement.Element("BashLogCollectorHost").Value))
                {
                    BashCollectorCheckBox.IsChecked = true;
                    _addressControl.Text = rootElement.Element("BashLogCollectorHost").Value;
                }
            }
            else
            {
                MessageBox.Show("Please connect to the correct environment and try loading again", ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void MenuSave_OnClick(object sender, RoutedEventArgs e)
        {
            if (_scenarioList.Count == 0)
            {
                return;
            }

            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = "xml",
                Filter = "EDT AutoTestHelper Save (*.xml)|*.xml"
            };
            if (sfd.ShowDialog().Value)
            {
                {
                    XElement rootElement = new XElement("EDT", new XElement("Database", _currentDatabase),
                        new XElement("DDS", new XElement("BuildId", BuildComboBox.SelectedValue), new XElement("Product", ProductComboBox.SelectedValue), new XElement("RunId", RunComboBox.SelectedValue)),
                        new XElement("AssetId", _edtAssetSelectionControl.AssetSelectionData.SelectedAssets),
                        new XElement("FIM", new XElement("Base", BaseFirmwareTextBox.Text), new XElement("Target", TargetFirmwareTextBox.Text)),
                        new XElement("BashLogCollectorHost", BashCollectorCheckBox.IsChecked.Value ? _addressControl.Text : string.Empty));

                    rootElement.Save(sfd.FileName);
                }
            }
        }

        private void MenuExit_OnClick(object sender, RoutedEventArgs e)
        {
            Close();
        }

      

     

        private void MenuImport_OnClick(object sender, RoutedEventArgs e)
        {
            if (!_edtAssetSelectionControl.HasSelection)
            {
                MessageBox.Show(
                    "Please select a device before importing the scenario. The selected device would be used for the imported scenarios.",
                    ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            Guid folderId = Guid.Parse("66F94649-D020-4067-B5FA-6BDD5374A293");
            using (EnterpriseTestContext context = new EnterpriseTestContext(_currentDatabase))
            {
                
                var folderList = context.ConfigurationTreeFolders.OrderBy(n => n.ParentId.Value)
                    .Select(x => x.Name).ToList();
                ObservableCollection<object> folderObjectList = new ObservableCollection<object>( folderList);
                ListDialogBox listDialogBox = new ListDialogBox
                {
                    Items = folderObjectList,
                    Prompt = "Select folder to import the scenarios."
                };
                if (listDialogBox.ShowDialog().Value)
                {
                    var folder = listDialogBox.SelectedItem as string;
                    folderId = context.ConfigurationTreeFolders.First(x => x.Name == folder).ConfigurationTreeFolderId;
                }
                else
                {
                    return;
                }
            }

                OpenFileDialog ofd = new OpenFileDialog
                {
                    AddExtension = true,
                    CheckFileExists = true,
                    DefaultExt = "edt",
                    Filter = "EDT BulkExport File (*.edt)|*.edt"
                };

            if (ofd.ShowDialog().Value)
            {
                ImportScenarios(ofd.FileName, folderId);  
            }
        }

        private void ImportScenarios(string fileName, Guid folderId)
        {
            int totalActivities = 0;
            List<Database.EnterpriseScenario> exportedScenarios;
            using (FileStream fs = File.OpenRead(fileName))
            {
                StreamReader sReader = new StreamReader(fs);
                var scenarioString = sReader.ReadToEnd();
                exportedScenarios = (List<Database.EnterpriseScenario>)JsonConvert.DeserializeObject(scenarioString, typeof(List<Database.EnterpriseScenario>));
            }

            using (EnterpriseTestContext context = new EnterpriseTestContext(_currentDatabase))
            {
                foreach (var sourceScenario in exportedScenarios)
                {
                    EnterpriseScenario targetScenario = new EnterpriseScenario
                    {
                        Name = sourceScenario.Name,
                        EnterpriseScenarioId = SequentialGuid.NewGuid(),
                        FolderId = folderId,
                        Company = sourceScenario.Company,
                        Deleted = false,
                        Description = sourceScenario.Description,
                        Owner = UserManager.CurrentUserName,
                        ScenarioSettings = sourceScenario.ScenarioSettings,
                        Vertical = sourceScenario.Vertical


                    };
                    //if (context.EnterpriseScenarios.FirstOrDefault(x =>
                    //        x.EnterpriseScenarioId == sourceScenario.EnterpriseScenarioId) != null)
                    //{
                    //    targetScenario.EnterpriseScenarioId = SequentialGuid.NewGuid();
                    //}

                    foreach (var sourceScenarioVirtualResource in sourceScenario.VirtualResources)
                    {

                        SolutionTester targetVirtualResource = new SolutionTester("SolutionTester")
                        {
                            Description = sourceScenarioVirtualResource.Description,
                            Enabled = true,
                            EnterpriseScenarioId = targetScenario.EnterpriseScenarioId,
                            Name = sourceScenarioVirtualResource.Name,
                            InstanceCount = sourceScenarioVirtualResource.InstanceCount,
                            Platform = sourceScenarioVirtualResource.Platform,
                            ResourceType = sourceScenarioVirtualResource.ResourceType,
                            ResourcesPerVM = sourceScenarioVirtualResource.ResourcePerVM,
                            TestCaseId = sourceScenarioVirtualResource.TestCaseId,
                            VirtualResourceId = SequentialGuid.NewGuid(),
                            DurationTime = sourceScenarioVirtualResource.DurationTime,
                            MaxActivityDelay = sourceScenarioVirtualResource.MaxActivityDelay,
                            MinActivityDelay = sourceScenarioVirtualResource.MinActivityDelay,
                            RandomizeActivities = sourceScenarioVirtualResource.RandomizeActivities,
                            RandomizeActivityDelay = sourceScenarioVirtualResource.RandomizeActivityDelay,
                            ExecutionMode = EnumUtil.Parse<ExecutionMode>(sourceScenarioVirtualResource.RunMode),
                            MaxStartupDelay = sourceScenarioVirtualResource.MaxStartupDelay,
                            MinStartupDelay = sourceScenarioVirtualResource.MinStartupDelay,
                            RandomizeStartupDelay = sourceScenarioVirtualResource.RandomizeStartupDelay,
                            RepeatCount = sourceScenarioVirtualResource.RepeatCount,
                            AccountType = SolutionTesterCredentialType.DefaultDesktop,
                            UseCredential = false

                        };

                        //if (context.VirtualResources.FirstOrDefault(x =>
                        //        x.VirtualResourceId == sourceScenarioVirtualResource.VirtualResourceId) != null)
                        //{
                        //    targetVirtualResource.VirtualResourceId = SequentialGuid.NewGuid();
                        //}

                        foreach (var virtualResourceMetadata in sourceScenarioVirtualResource.VirtualResourceMetadata)
                        {
                            VirtualResourceMetadata targetVirtualResourceMetadata =
                                new VirtualResourceMetadata(virtualResourceMetadata.ResourceType,
                                    virtualResourceMetadata.MetadataType)
                                {
                                    VirtualResourceId = targetVirtualResource.VirtualResourceId,
                                    Deleted = false,
                                    Enabled = true,
                                    ExecutionPlan = virtualResourceMetadata.ExecutionPlan,
                                    Metadata = virtualResourceMetadata.Metadata,
                                    MetadataVersion = virtualResourceMetadata.MetadataVersion,
                                    MetadataType = virtualResourceMetadata.MetadataType,
                                    Name = virtualResourceMetadata.Name,
                                    ResourceType = virtualResourceMetadata.ResourceType,
                                    VirtualResourceMetadataId = SequentialGuid.NewGuid(),

                                };
                            //if (context.VirtualResourceMetadataSet.FirstOrDefault(x =>
                            //        x.VirtualResourceMetadataId ==
                            //        targetVirtualResourceMetadata.VirtualResourceMetadataId) != null)
                            //{
                            //    targetVirtualResourceMetadata.VirtualResourceMetadataId = SequentialGuid.NewGuid();
                            //}

                        targetVirtualResourceMetadata.AssetUsage = VirtualResourceMetadataAssetUsage
                                .CreateVirtualResourceMetadataAssetUsage(
                                    targetVirtualResourceMetadata.VirtualResourceMetadataId,
                                    Serializer.Serialize(_edtAssetSelectionControl.AssetSelectionData).ToString());
                            targetVirtualResource.VirtualResourceMetadataSet.Add(targetVirtualResourceMetadata);
                            totalActivities++;

                        }
                        targetScenario.VirtualResources.Add(targetVirtualResource);
                    }
                    context.EnterpriseScenarios.AddObject(targetScenario);
                }

                try
                {
                    MessageBox.Show(
                        $"Found {totalActivities} activities to import. This might take few minutes to complete. Please be patient. Press OK to proceed.",
                        ApplicationName, MessageBoxButton.OK, MessageBoxImage.Information);
                    context.SaveChanges();
                    MessageBox.Show($"{exportedScenarios.Count} scenarios successfully imported.", ApplicationName,
                        MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (SqlException sqlException)
                {
                    MessageBox.Show($"Error occurred while importing the scenario. {ScalableTest.Extension.JoinAllErrorMessages(sqlException)}",
                        ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);

                }
                catch (UpdateException updateException)
                {
                    MessageBox.Show($"Error occurred while importing the scenario. {ScalableTest.Extension.JoinAllErrorMessages(updateException)}",
                        ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
                catch (Exception e)
                {
                    MessageBox.Show($"An unknown error occurred while importing scenario. {ScalableTest.Extension.JoinAllErrorMessages(e)}", ApplicationName, MessageBoxButton.OK, MessageBoxImage.Error);
                }
               
            }
        }

        private void MenuExport_OnClick(object sender, RoutedEventArgs e)
        {
            SaveFileDialog sfd = new SaveFileDialog
            {
                DefaultExt = "edt",
                Filter = "EDT BulkExport File (*.edt)|*.edt"
            };
            if (sfd.ShowDialog().Value)
            {
                //let's export the scenario details to sqlite file
                ExportDatabase(sfd.FileName);

            }
            
        }

        private void ExportDatabase(string fileName)
        {
            List<Database.EnterpriseScenario> exportedScenarios = new List<Database.EnterpriseScenario>();
            using (EnterpriseTestContext context = new EnterpriseTestContext(_currentDatabase))
            {
                foreach (var scenarioQueueItem in _scenarioList.Where(x => x.Active))
                {
                    var scenario =context.EnterpriseScenarios.First(x =>x.EnterpriseScenarioId == scenarioQueueItem.ScenarioId);
                    Database.EnterpriseScenario targetScenario = new Database.EnterpriseScenario()
                    {
                        EnterpriseScenarioId = scenario.EnterpriseScenarioId,
                        Name = scenario.Name,
                        FolderId = scenario.FolderId.GetValueOrDefault(
                            Guid.Parse("66F94649-D020-4067-B5FA-6BDD5374A293")),
                        Company = scenario.Company,
                        Deleted = scenario.Deleted,
                        Description = scenario.Description,
                        Owner = scenario.Owner,
                        ScenarioSettings = scenario.ScenarioSettings,
                        Vertical = scenario.Vertical
                    };


                    foreach (var scenarioVirtualResource in scenario.VirtualResources.OfType<SolutionTester>())
                    {
                        Database.VirtualResource targetVirtualResource = new Database.VirtualResource()
                        {
                            VirtualResourceId = scenarioVirtualResource.VirtualResourceId,
                            Deleted = false,
                            Description = scenarioVirtualResource.Description,
                            Enabled = true,
                            EnterpriseScenarioId = scenarioVirtualResource.EnterpriseScenarioId,
                            InstanceCount = scenarioVirtualResource.InstanceCount,
                            Name = scenarioVirtualResource.Name,
                            Platform = scenarioVirtualResource.Platform,
                            ResourcePerVM = scenarioVirtualResource.ResourcesPerVM.GetValueOrDefault(10),
                            ResourceType = scenarioVirtualResource.ResourceType,
                            TestCaseId = scenarioVirtualResource.TestCaseId,
                            //additional properties which i had missed out on
                            RepeatCount = scenarioVirtualResource.RepeatCount,
                            RandomizeStartupDelay = scenarioVirtualResource.RandomizeStartupDelay,
                            MinStartupDelay = scenarioVirtualResource.MinStartupDelay,
                            MaxStartupDelay = scenarioVirtualResource.MaxStartupDelay,
                            RandomizeActivities = scenarioVirtualResource.RandomizeActivities,
                            RandomizeActivityDelay = scenarioVirtualResource.RandomizeActivityDelay,
                            MinActivityDelay = scenarioVirtualResource.MinActivityDelay,
                            MaxActivityDelay = scenarioVirtualResource.MaxActivityDelay,
                            RunMode = scenarioVirtualResource.ExecutionMode.ToString(),
                            DurationTime = scenarioVirtualResource.DurationTime

                        };
                        
                        foreach (var virtualResourceMetadata in scenarioVirtualResource.VirtualResourceMetadataSet)
                        {
                            Database.VirtualResourceMetadata targetVirtualResourceMetadata =
                                new Database.VirtualResourceMetadata()
                                {
                                    Name = virtualResourceMetadata.Name,
                                    Deleted = false,
                                    Enabled = virtualResourceMetadata.Enabled,
                                    ExecutionPlan = virtualResourceMetadata.ExecutionPlan,
                                    Metadata = virtualResourceMetadata.Metadata,
                                    MetadataType = virtualResourceMetadata.MetadataType,
                                    MetadataVersion = virtualResourceMetadata.MetadataVersion,
                                    ResourceType = virtualResourceMetadata.ResourceType,
                                    VirtualResourceId = virtualResourceMetadata.VirtualResourceId,
                                    VirtualResourceMetadataId = virtualResourceMetadata.VirtualResourceMetadataId
                                };
                            targetVirtualResource.VirtualResourceMetadata.Add(targetVirtualResourceMetadata);

                        }

                        targetScenario.VirtualResources.Add(targetVirtualResource);
                    }

                    exportedScenarios.Add(targetScenario);
                }

                var serializedScenario = JsonConvert.SerializeObject(exportedScenarios, Formatting.Indented);
                using (FileStream fs = File.Create(fileName))
                {
                    var bytes = Encoding.ASCII.GetBytes(serializedScenario);
                    fs.Write(bytes, 0, bytes.Length);
                    fs.Flush();
                }
            }

            MessageBox.Show("Selected scenarios have been exported.", ApplicationName, MessageBoxButton.OK,
                MessageBoxImage.Information);
        }
        private void MenuBulkEdit_OnClick(object sender, RoutedEventArgs e)
        {
            if (_scenarioList.Count == 0 || !_scenarioList.Any(x => x.Active))
            {
                MessageBox.Show("There are no scenarios to edit!", ApplicationName, MessageBoxButton.OK,
                    MessageBoxImage.Error);
                return;
            }

            var activeScenarioList = _scenarioList.Where(x => x.Active).ToList();
            BulkEditorWindow bulkEditor = new BulkEditorWindow(activeScenarioList);
            Visibility = Visibility.Collapsed;
            try
            {
                bulkEditor.ShowDialog();
            }
            catch (Exception exception)
            {
                MessageBox.Show($"An error occurred. {Extension.JoinAllErrorMessages(exception)}", ApplicationName, MessageBoxButton.OK,
                    MessageBoxImage.Error);

            }

            Visibility = Visibility.Visible;
        }
        #endregion MenuOperations


        private void TextBoxTotalScenario_OnPreviewTextInput(object sender, TextCompositionEventArgs e)
        {
            e.Handled = NumericRegex.IsMatch(e.Text);   
        }

        private void TextBoxTotalScenario_OnLostFocus(object sender, RoutedEventArgs e)
        {
            int totalTestCases;
            if (int.TryParse(TextBoxTotalScenario.Text, out totalTestCases))
            {
                if (totalTestCases == 0)
                    return;

                var activeScenarios = _scenarioList.Where(x => x.Active).ToList();
                var currentTotalTestCases = activeScenarios.Sum(x => x.Distribution);
                foreach (var scenarioQueueItem in activeScenarios)
                {
                    scenarioQueueItem.Distribution =(int)((((double) scenarioQueueItem.Distribution) / currentTotalTestCases) * totalTestCases);
                }

                ScenarioDataGrid.DataContext = null;
                ScenarioDataGrid.DataContext = _scenarioList.Where(x => x.Active);
            }
        }

        private void MenuAbout_OnClick(object sender, RoutedEventArgs e)
        {
            MessageBox.Show($"Auto Test Helper Version: {Assembly.GetExecutingAssembly().GetName().Version}. Dated: 22-Mar-2019");
        }
    }
}