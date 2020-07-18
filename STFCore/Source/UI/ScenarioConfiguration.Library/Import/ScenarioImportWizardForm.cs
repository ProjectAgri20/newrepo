using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml;
using System.Text;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.ImportExport;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Core.UI;
using Telerik.WinControls.UI;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.UI.ScenarioConfiguration.Import
{
    public partial class ScenarioImportWizardForm : Form
    {
        private ImportCompositeControl _masterCompositeControl = null;

        private ImportWelcomeControl _welcomeControl = null;
        private ImportResolutionControl _resolutionControl = null;
        private ImportCompletionControl _completionControl = null;
        private ImportDocumentControl _documentControl = null;
        private ImportDevicePoolControl _assetPoolControl = null;
        private ImportPlatformControl _platformsControl = null;
        private ImportGroupControl _userGroupControl = null;

        EnterpriseScenarioContract _scenarioContract = null;
        EnterpriseScenarioCompositeContract _compositeContract = null;
        EnterpriseScenario _finalScenarioEntity = null;

        private bool _assetPoolFirstPass = true;
        private bool _platformFirstPass = true;
        private bool _displayPlatformPage = false;

        private StringBuilder _importMetadataMessages;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public ScenarioImportWizardForm()
        {
            InitializeComponent();

            ScenarioImported = false;

            _masterCompositeControl = new ImportCompositeControl();

            _welcomeControl = new ImportWelcomeControl();
            welcomePanel.Controls.Add(_welcomeControl);

            _assetPoolControl = new ImportDevicePoolControl();
            _userGroupControl = new ImportGroupControl();
            _resolutionControl = new ImportResolutionControl();
            _documentControl = new ImportDocumentControl();
            _platformsControl = new ImportPlatformControl();
            _completionControl = new ImportCompletionControl();

            importWizard.Next += importWizard_Next;
            importWizard.Previous += importWizard_Previous;
            importWizard.Cancel += importWizard_Cancel;
            importWizard.Finish += importWizard_Finish;
            importWizard.SelectedPageChanged += importWizard_SelectedPageChanged;

            _resolutionControl.OnAllItemsResolved += AllItemsResolved;
            _documentControl.OnAllItemsResolved += AllItemsResolved;
            _platformsControl.OnAllItemsResolved += AllItemsResolved;
            _assetPoolControl.OnAllItemsResolved += AllItemsResolved;

            _completionControl.OnNodeSelected += _completionControl_OnNodeSelected;
            ContractFactory.OnStatusChanged += ContractFactory_OnStatusChanged;

            _importMetadataMessages = new StringBuilder();
        }

        /// <summary>
        /// Imports an STF Scenario into the specified folder.
        /// </summary>
        /// <param name="scenarioFolderId"></param>
        public ScenarioImportWizardForm(Guid scenarioFolderId) : this()
        {
            _completionControl.SelectedFolderId = scenarioFolderId;
        }

        /// <summary>
        /// Whether or not the scenario was imported successfully.
        /// </summary>
        public bool ScenarioImported { get; set; }

        #region Wizard Page Events
        private void importWizard_Finish(object sender, EventArgs e)
        {
            ProcessFinish();
        }

        private void _completionControl_OnNodeSelected(object sender, EventArgs e)
        {
            importWizard.FinishButton.Enabled = true;
        }

        private void importWizard_SelectedPageChanged(object sender, SelectedPageChangedEventArgs e)
        {
            ProcessSelectedPageChange(e.SelectedPage);
        }

        private void AllItemsResolved(object sender, EventArgs e)
        {
            importWizard.NextButton.Enabled = true;
            importWizard.Refresh();
        }

        private void importWizard_Cancel(object sender, EventArgs e)
        {
            Close();
        }

        private void importWizard_Previous(object sender, WizardCancelEventArgs e)
        {
            ProcessPrevious(importWizard.Pages.Where(x => x.IsSelected).First());
            e.Cancel = true;
        }

        private void importWizard_Next(object sender, WizardCancelEventArgs e)
        {
            ProcessNext(importWizard.Pages.Where(x => x.IsSelected).First());
            e.Cancel = true;
        }
        #endregion

        #region Wizard Page Event Handlers
        private void ProcessFinish()
        {
            if (_completionControl.SelectedFolderId == Guid.Parse("00000000-0000-0000-0000-000000000000"))
            {
                MessageBox.Show("Please Select a destination folder", "Import Scenario", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                return;
            }

            StringBuilder importMessage = new StringBuilder("The Scenario '");
            importMessage.Append(_scenarioContract.Name);
            importMessage.AppendLine("' was successfully imported.");

            try
            {
                using (new BusyCursor())
                {
                    if (!_masterCompositeControl.Validate())
                    {
                        return;
                    }

                    if (_compositeContract != null)
                    {
                        using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                        {
                            bool changesMade = false;
                            HashSet<Guid> pQueues = new HashSet<Guid>();

                            foreach (var printer in _compositeContract.Printers)
                            {
                                if (!context.Assets.Any(x => x.AssetId.Equals(printer.AssetId)))
                                {
                                    context.Assets.Add(ContractFactory.Create(printer, context));
                                    changesMade = true;
                                }
                            }

                            var items = _compositeContract.Scenario.ActivityPrintQueueUsage;

                            XmlDocument doc = new XmlDocument();
                            foreach (var item in items)
                            {
                                doc.LoadXml(item.XmlSelectionData);
                                XmlNode node = doc.DocumentElement.FirstChild.FirstChild.FirstChild;
                                if (!string.IsNullOrEmpty(node.InnerText) && IsGuid(node.InnerText))
                                {
                                    if (!pQueues.Contains(Guid.Parse(node.InnerText)) && node.FirstChild.FirstChild.Name == "_printQueueId")
                                    {
                                        pQueues.Add(Guid.Parse(node.InnerText));
                                    }
                                }
                            }
                            var containsQueues = context.RemotePrintQueues.Where(x => pQueues.Contains(x.RemotePrintQueueId)).Select(x => x.Name);

                            if (pQueues.Count() != containsQueues.Count() || containsQueues.Count() == 0)
                            {
                                importMessage.AppendLine("Warning: Some Print Queues May Not Have Imported.");
                                importMessage.AppendLine("Please use Bulk Edit Tool to resolve.");
                            }

                            if (changesMade)
                            {
                                context.SaveChanges();
                            }
                        }
                    }

                    using (EnterpriseTestContext context = new EnterpriseTestContext())
                    {
                        var importMaps = new List<ContractFactory.ImportMap>();
                        if (_finalScenarioEntity == null)
                        {
                            _finalScenarioEntity = ContractFactory.Create(_scenarioContract, out importMaps);
                           
                        }

                        var emptyPlatforms =
                            (
                                from r in _finalScenarioEntity.VirtualResources
                                where (string.IsNullOrEmpty(r.Platform) || r.Platform.Equals("LOCAL"))
                                    && !r.ResourceType.Equals("SolutionTester")
                                select r
                            );

                        if (emptyPlatforms != null && emptyPlatforms.Count() > 0)
                        {
                            using (AssignPlatformDialog dialog = new AssignPlatformDialog(emptyPlatforms))
                            {
                                dialog.ShowDialog();
                                return;
                            }
                        }

                        _finalScenarioEntity.Owner = UserManager.CurrentUserName;
                        foreach (var group in _scenarioContract.UserGroups)
                        {
                            var entity = context.UserGroups.FirstOrDefault(x => x.GroupName.Equals(group));
                            if (entity != null)
                            {
                                _finalScenarioEntity.UserGroups.Add(entity);
                            }
                        }

                        _finalScenarioEntity.FolderId = _completionControl.SelectedFolderId;
                        context.AddToEnterpriseScenarios(_finalScenarioEntity);
                        CreateFolders(_scenarioContract, _finalScenarioEntity, context, importMaps);

                        context.SaveChanges();
                        ScenarioImported = true;
                    }

                    if (_importMetadataMessages.Length > 0)
                    {                      
                        importMessage.AppendLine(_importMetadataMessages.ToString());
                    }

                    MessageBox.Show(importMessage.ToString(), "Import Scenario", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                    Close();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
                TraceFactory.Logger.Error(ex);
            }
        }

        private void ContractFactory_OnStatusChanged(object sender, Utility.StatusChangedEventArgs e)
        {
            _importMetadataMessages.AppendLine(e.StatusMessage);
        }

        /// <summary>
        /// Creates the folders within a scenario during import
        /// </summary>
        /// <param name="contract">The contract.</param>
        /// <param name="scenario">The scenario.</param>
        /// <param name="context">The context.</param>
        /// <param name="importMaps">The import maps.</param>
        private void CreateFolders(EnterpriseScenarioContract contract, EnterpriseScenario scenario, EnterpriseTestContext context, List<ContractFactory.ImportMap> importMaps)
        {
            try
            {
                foreach (var folder in contract.Folders)
                {
                    // create the folder with parent defaulted to scenario
                    var newFolderEntity = ConfigurationTreeFolder.CreateConfigurationTreeFolder(SequentialGuid.NewGuid(), folder.Name, folder.FolderType);
                    newFolderEntity.ParentId = scenario.EnterpriseScenarioId;
                    context.AddToConfigurationTreeFolders(newFolderEntity);

                    // set children for folder based on import mapping
                    var childMaps = (from c in folder.ChildIds
                                         join i in importMaps on c equals i.OldId
                                         select i);

                    if (childMaps.Any())
                    {
                        newFolderEntity.ParentId = childMaps.First().NewParentId;

                        switch (folder.FolderType)
                        {
                            case "ResourceFolder":
                                var vr = (from e in scenario.VirtualResources
                                          join c in childMaps on e.VirtualResourceId equals c.NewId
                                          select e).ToList();
                                vr.ForEach(x => x.FolderId = newFolderEntity.ConfigurationTreeFolderId);
                                break;

                            case "MetadataFolder":
                                var md = (from e in scenario.VirtualResources.SelectMany(x => x.VirtualResourceMetadataSet)
                                            join c in childMaps on e.VirtualResourceMetadataId equals c.NewId
                                            select e).ToList();
                                md.ForEach(x => x.FolderId = newFolderEntity.ConfigurationTreeFolderId);
                                break;

                            default:
                                break;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error creating folders within scenario", ex);
            }
        }


        private void ProcessSelectedPageChange(WizardPage page)
        {
            if (page == resolutionWizardPage)
            {
                importWizard.NextButton.Enabled = false;
            }
            else if (page == documentWizardPage)
            {
                if (_scenarioContract.TestDocuments.Any(x => !x.Resolved))
                {
                    importWizard.NextButton.Enabled = false;
                }
            }
            else if (page == welcomeWizardPage)
            {
                _welcomeControl.IsFileReloaded = false;
            }
            else if (page == completionWizardPage)
            {
                importWizard.FinishButton.Enabled = false;
                if (_completionControl.SelectedFolderId != null || _completionControl.SelectedFolderId != Guid.Empty)
                {
                    importWizard.FinishButton.Enabled = true;
                }
            }
            else if (page == platformWizardPage)
            {                
                importWizard.NextButton.Enabled = _platformsControl.CheckItemsResolved();
            }
            else if (page == assetPoolWizardPage)
            {
                if (_compositeContract != null)
                {
                    importWizard.NextButton.Enabled = false;
                    _assetPoolControl.LoadPrinters(_compositeContract.Printers);
                }
                else
                {
                    importWizard.NextButton.Enabled = true;
                }
            }
        }
        
        private void ProcessPrevious(WizardPage page)
        {
            if (page == assetPoolWizardPage)
            {
                importWizard.SelectedPage = welcomeWizardPage;
            }
            else if (page == resolutionWizardPage)
            {
                if (AssetPoolResolutionRequired()) { return; }
                importWizard.SelectedPage = welcomeWizardPage;
            }
            else if (page == documentWizardPage)
            {
                if (ResourceResolutionRequired()) { return; }
                if (AssetPoolResolutionRequired()) { return; }
                importWizard.SelectedPage = welcomeWizardPage;
            }
            else if (page == platformWizardPage)
            {
                if (DocumentResolutionRequired()) { return; }
                if (ResourceResolutionRequired()) { return; }
                if (AssetPoolResolutionRequired()) { return; }
                importWizard.SelectedPage = welcomeWizardPage;
            }
            else if (page == groupWizardPage)
            {
                if (PlatformResolutionRequired()) { return; }
                if (DocumentResolutionRequired()) { return; }
                if (ResourceResolutionRequired()) { return; }
                if (AssetPoolResolutionRequired()) { return; }
                importWizard.SelectedPage = welcomeWizardPage;
            }
            else if (page == completionWizardPage)
            {
                if (UserGroupResolutionRequired()) { return; }
                if (PlatformResolutionRequired()) { return; }
                if (DocumentResolutionRequired()) { return; }
                if (ResourceResolutionRequired()) { return; }
                if (AssetPoolResolutionRequired()) { return; }
                importWizard.SelectedPage = welcomeWizardPage;
            }
        }

        private void ProcessNext(WizardPage page)
        {
            if (page == welcomeWizardPage)
            {
                try
                {
                    using (new BusyCursor())
                    {
                        if (string.IsNullOrEmpty(_welcomeControl.ImportFile))
                        {
                            MessageBox.Show("Select an import file before continuing");
                            return;
                        }

                        if (_welcomeControl.IsFileReloaded)
                        {
                            var fileData = XElement.Parse(File.ReadAllText(_welcomeControl.ImportFile));

                            // Reset key components that are currently persisting information from a
                            // possible prior import file. 
                            _finalScenarioEntity = null;
                            _platformsControl.Reset();
                            _assetPoolControl.Reset();
                            _assetPoolFirstPass = true;
                            _platformFirstPass = true;
                            _displayPlatformPage = false;

                            // If this is a composite contract file it may contain printer and document
                            // information in addition to the base scenario data. 
                            if (fileData.Name.LocalName == "Composite")
                            {
                                _compositeContract = Serializer.Deserialize<EnterpriseScenarioCompositeContract>(fileData);
                                _scenarioContract = _compositeContract.Scenario;

                                // Swap the OfficeWorker and SolutionTester as needed so that the import process is targeting the 
                                // right virtual resource.
                                if (GlobalSettings.IsDistributedSystem)
                                {
                                    foreach (var resource in _scenarioContract.ResourceDetails.Where(x => x.ResourceType == VirtualResourceType.SolutionTester))
                                    {
                                        resource.ResourceType = VirtualResourceType.OfficeWorker;
                                    }
                                }
                                else
                                {
                                    foreach (var resource in _scenarioContract.ResourceDetails.Where(x => x.ResourceType == VirtualResourceType.OfficeWorker))
                                    {
                                        resource.ResourceType = VirtualResourceType.SolutionTester;
                                    }
                                }

                                if (!ImportExportUtil.ProcessCompositeContractFile(_compositeContract))
                                {
                                    return;
                                }
                            }
                            else
                            {
                                _scenarioContract = Serializer.Deserialize<EnterpriseScenarioContract>(fileData);
                            }

                            EvaluateUsageAgents();

                            var invalidTypes = _scenarioContract.InvalidResourceTypes;
                            if (invalidTypes.Count > 0)
                            {
                                string types = string.Join(", ", invalidTypes.ToArray());
                                var msg = "The following resource types are not supported on this system ({0}).  They will be skipped and not imported.  Do you want to continue?"
                                    .FormatWith(types);
                                var result = MessageBox.Show(msg, "Invalid Resource Types", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
                                if (result == DialogResult.No)
                                {
                                    return;
                                }
                            }

                            _scenarioContract.ScanTestDocuments();

                            _resolutionControl.LoadContract(_scenarioContract);
                            _documentControl.LoadContract(_scenarioContract);
                            _masterCompositeControl.UpdateContractData(_scenarioContract);
                        }

                        // Determine the next stop when moving forward
                        if (AssetPoolResolutionRequired()) { return; }
                        if (ResourceResolutionRequired()) { return; }
                        if (DocumentResolutionRequired()) { return; }
                        if (PlatformResolutionRequired()) { return; }
                        if (UserGroupResolutionRequired()) { return; }
                        GotoCompletionPage();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("There was an error loading and configuring the Import File.  Check with your application administrator to solve the problem.  Error: {0}".FormatWith(ex.Message)
                        , "Unable to Read Data File", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    TraceFactory.Logger.Error(ex);
                    return;
                }
            }
            else if (page == assetPoolWizardPage)
            {
                EvaluateUsageAgents();

                // Determine the next stop when moving forward
                if (ResourceResolutionRequired()) { return; }
                if (DocumentResolutionRequired()) { return; }
                if (PlatformResolutionRequired()) { return; }
                if (UserGroupResolutionRequired()) { return; }
                GotoCompletionPage();
            }
            else if (page == resolutionWizardPage)
            {
                // Determine the next stop when moving forward
                if (DocumentResolutionRequired()) { return; }
                if (PlatformResolutionRequired()) { return; }
                if (UserGroupResolutionRequired()) { return; }
                GotoCompletionPage();
            }
            else if (page == documentWizardPage)
            {
                // Determine the next stop when moving forward
                if (PlatformResolutionRequired()) { return; }
                if (UserGroupResolutionRequired()) { return; }
                GotoCompletionPage();
            }
            else if (page == platformWizardPage)
            {
                // Only place left to go is the completion page
                if (UserGroupResolutionRequired()) { return; }
                GotoCompletionPage();
            }
            else if (page == groupWizardPage)
            {
                GotoCompletionPage();
            }
        }

        private void GotoCompletionPage()
        {
            _completionControl.LoadFolders();

            _masterCompositeControl.SetPanel(_completionControl);
            completionPanel.Controls.Add(_masterCompositeControl);
            importWizard.SelectedPage = completionWizardPage;
        }

        private bool AssetPoolResolutionRequired()
        {
            if (_compositeContract == null)
            {
                return false;
            }
            else if (_compositeContract.Printers.Count == 0)
            {
                return false;
            }

            if (_assetPoolFirstPass)
            {
                // Only run through this code the first time.  It will remove existing
                // entries first but then display the remaining entries if the user
                // goes back and forth through the wizard
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    var values = _compositeContract.Printers.Select(x => x.AssetId).ToList();
                    var source = context.Assets.Select(x => x.AssetId).ToList();
                    foreach (var assetId in values.Where(i => source.Contains(i)))
                    {
                        // Get rid of any printers referenced in the import data that are 
                        // already present in the asset inventory system.
                        var printer = _compositeContract.Printers.First(x => x.AssetId.Equals(assetId));
                        _compositeContract.Printers.Remove(printer);
                    }

                    if (_compositeContract.Printers.Count == 0)
                    {
                        // If there are no new printers left to add, return false
                        return false;
                    }
                    else if (context.AssetPools.Count() == 1)
                    {
                        // If there are new printers to add, and there is only one pool
                        // name to choose from, then add the new printers and return false
                        // There's no reason to show this wizard page.
                        var poolName = context.AssetPools.First().Name;
                        foreach (var printer in _compositeContract.Printers)
                        {
                            printer.PoolName = poolName;
                            context.Assets.Add(ContractFactory.Create(printer, context));
                        }

                        context.SaveChanges();
                        EvaluateUsageAgents();
                        return false;
                    }
                    else
                    {
                        foreach (var printer in _compositeContract.Printers)
                        {
                            printer.PoolName = string.Empty;
                        }
                    }

                    _assetPoolFirstPass = false;
                }
            }

            // There are printers still to be added, and there is more than one
            // Asset Pool to choose, so show this page.

            _masterCompositeControl.SetPanel(_assetPoolControl);
            assetPoolPanel.Controls.Add(_masterCompositeControl);
            importWizard.SelectedPage = assetPoolWizardPage;
            return true;
        }

        private void EvaluateUsageAgents()
        {
            // Since we stopped at the asset pool wizard page and are now moving
            // forward, have all the usage agents re-evaluate to ensure that any
            // printer definitions now added to the database because of the 
            // asset pool page get their status updated.
        }

        private bool UserGroupResolutionRequired()
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                if (context.UserGroups.Count() == 0)
                {
                    return false;
                }
            }

            _userGroupControl.LoadGroups(_scenarioContract);

            _masterCompositeControl.SetPanel(_userGroupControl);
            groupPanel.Controls.Add(_masterCompositeControl);
            importWizard.SelectedPage = groupWizardPage;
            return true;
        }

        private bool ResourceResolutionRequired()
        {
            return false;
        }

        private bool DocumentResolutionRequired()
        {
            if (_scenarioContract.TestDocuments.Any(x => x.ResolutionRequired))
            {
                _masterCompositeControl.SetPanel(_documentControl);
                documentPanel.Controls.Add(_masterCompositeControl);
                importWizard.SelectedPage = documentWizardPage;
                return true;
            }

            return false;
        }

        private bool PlatformResolutionRequired()
        {
            if (!GlobalSettings.IsDistributedSystem)
            {
                return false;
            }

            if (_platformFirstPass)
            {
                List<string> platformIds = null;
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    platformIds = context.FrameworkClientPlatforms.Select(x => x.FrameworkClientPlatformId).ToList();
                }

                if (_scenarioContract.ResourceDetails.Count() > 0)
                {
                    foreach (var resource in _scenarioContract.ResourceDetails)
                    {
                        if (!platformIds.Any(x => x.Equals(resource.Platform)))
                        {
                            resource.Platform = string.Empty;
                            _displayPlatformPage = true;
                        }
                    }
                }

                _platformFirstPass = false;
            }

            if (_displayPlatformPage)
            {
                _platformsControl.LoadResources(_scenarioContract.ResourceDetails);

                _masterCompositeControl.SetPanel(_platformsControl);
                platformPanel.Controls.Add(_masterCompositeControl);
                importWizard.SelectedPage = platformWizardPage;
                return true;
            }

            return false;
        }

        private bool IsGuid(string guidValue)
        {
            Guid outParam;
            return Guid.TryParse(guidValue, out outParam);
        }

        #endregion
    }
}
