using System;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.IO;
using System.Linq;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Dispatcher;
using System.Collections.Generic;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Documents;
using System.Xml.Linq;
using System.Text;
using HP.ScalableTest.Core.Plugin;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Contract Factory Class for converting contract data to objects and vice versa.
    /// </summary>
    public class ContractFactory
    {
        /// <summary>
        /// Event Handler for Status Change
        /// </summary>
        public static event EventHandler<StatusChangedEventArgs> OnStatusChanged;

        private static void UpdateStatus(string message)
        {
            OnStatusChanged?.Invoke(null, new StatusChangedEventArgs(message));
        }

        /// <summary>
        /// Creates composite contract data for the specified scenario.
        /// Scenario data + Printer data + Document data.
        /// </summary>
        /// <param name="scenario"></param>
        /// <param name="includePrinters">When true, causes all print device data to be serialized as part of the export process.</param>
        /// <param name="includeDocuments">When true, causes the documents to be serialized as part of the export process</param>
        /// <returns></returns>
        public static EnterpriseScenarioCompositeContract Create(EnterpriseScenario scenario, bool includePrinters, bool includeDocuments)
        {
            var scenarioContract = Create(scenario);
            var contract = new EnterpriseScenarioCompositeContract(scenarioContract);

            if (includePrinters)
            {
                using (var context = DbConnect.AssetInventoryContext())
                {
                    foreach (var printer in contract.Scenario.AllAssets.Where(x=>x.AssetType == "Printer"))
                    {
                        var asset = context.Assets.OfType<Printer>().FirstOrDefault(x => x.AssetId.Equals(printer.AssetId));
                        if (asset != null)
                        {
                            contract.Printers.Add(Create<PrinterContract>(asset));
                            UpdateStatus("Exporting {0}".FormatWith(printer.AssetId));
                        }
                        else
                        {
                            TraceFactory.Logger.Error("Asset Id {0} was NULL".FormatWith(printer.AssetId));
                            UpdateStatus("Missing {0}".FormatWith(printer.AssetId));
                        }
                    }
                }
            }

            if (includeDocuments)
            {
                using (DocumentLibraryContext context = DbConnect.DocumentLibraryContext())
                {
                    foreach (var documentContract in contract.Scenario.TestDocuments)
                    {
                        string fileName = Path.GetFileName(documentContract.Original);
                        TestDocument document = context.TestDocuments.Include(n => n.TestDocumentExtension).FirstOrDefault(x => x.FileName.Equals(fileName));
                        if (document != null)
                        {
                            contract.Documents.Load(document, includeDocuments);
                            UpdateStatus("Exporting {0}".FormatWith(fileName));
                        }
                    }
                }
            }

            return contract;            
        }

        /// <summary>
        /// Creates contract data for the specified scenario.
        /// </summary>
        /// <param name="scenario"></param>
        /// <returns></returns>
        public static EnterpriseScenarioContract Create(EnterpriseScenario scenario)
        {
            EnterpriseScenarioContract contract = new EnterpriseScenarioContract(scenario, "1.0");
            Guid scenarioId = scenario.EnterpriseScenarioId;
            SystemManifest manifest = CreateReferenceManifest(scenarioId);

            AddTestDocumentInfoToContract(scenario.EnterpriseScenarioId, contract, manifest);
            AddAssetInfoToContract(scenario.EnterpriseScenarioId, contract, manifest);

            AddActivityUsageDataToContract(contract, scenarioId);
            AddFolderDataToContract(contract, scenario);

            foreach (var resource in scenario.VirtualResources)
            {
                var resourceDetail = VirtualResourceDetailBuilder.CreateBaseDetail(resource);
                contract.ResourceDetails.Add(resourceDetail);
                UpdateStatus("Exporting {0}".FormatWith(resourceDetail.Name));
            }

            return contract;
        }

        /// <summary>
        /// Adds the folder data to the specified scenario contract.
        /// </summary>
        /// <param name="contract">The scenario contract.</param>
        /// <param name="scenario">The scenario object.</param>
        private static void AddFolderDataToContract(EnterpriseScenarioContract contract, EnterpriseScenario scenario)
        {
            try
            {
                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    // get resource and metadata objects
                    IEnumerable<VirtualResource> resources = scenario.VirtualResources.Where(v => v.FolderId.HasValue && v.FolderId.Value != Guid.Empty);
                    IEnumerable<VirtualResourceMetadata> metadatas = resources.SelectMany(v => v.VirtualResourceMetadataSet).Where(m => m.FolderId.HasValue && m.FolderId.Value != Guid.Empty);

                    // group each by the folder id
                    var groupResources = (from r in resources
                                          group r by r.FolderId into g
                                          select new { FolderId = g.Key.Value, ChildIds = g.Select(x => x.VirtualResourceId) });

                    var groupMetadata = (from m in metadatas
                                         group m by m.FolderId into g
                                         select new { FolderId = g.Key.Value, ChildIds = g.Select(z => z.VirtualResourceMetadataId) });

                    // get referenced folders
                    var allGroups = groupResources.Concat(groupMetadata);
                    var folderIds = allGroups.Select(x => x.FolderId);
                    IQueryable<ConfigurationTreeFolder> folders = ConfigurationTreeFolder.Select(context, folderIds);

                    // add contract data for each referenced folder
                    foreach (var group in allGroups)
                    {
                        ConfigurationTreeFolder folder = folders.FirstOrDefault(x => x.ConfigurationTreeFolderId.Equals(group.FolderId));
                        if (folder != null)
                        {
                            FolderContract folderContract = new FolderContract() { Name = folder.Name, FolderType = folder.FolderType, ChildIds = new Collection<Guid>(group.ChildIds.ToList()) };
                            contract.Folders.Add(folderContract);
                        }
                    }
                }
            }
            catch(Exception ex)
            {
                TraceFactory.Logger.Error("Error adding folder data.", ex);
            }
        }

        private static void AddActivityUsageDataToContract(EnterpriseScenarioContract contract, Guid scenarioId)
        {
            using (var context = new EnterpriseTestContext())
            {
                contract.ActivityAssetUsage.Clear();
                var assetUsage = context.VirtualResourceMetadataAssetUsages.Where(n => n.VirtualResourceMetadata.VirtualResource.EnterpriseScenarioId == scenarioId).ToList();
                assetUsage.ForEach(x => contract.ActivityAssetUsage.Add(new ResourceUsageContract(x.VirtualResourceMetadataId, x.AssetSelectionData)));

                contract.ActivityDocumentUsage.Clear();
                var docUsage = context.VirtualResourceMetadataDocumentUsages.Where(n => n.VirtualResourceMetadata.VirtualResource.EnterpriseScenarioId == scenarioId).ToList();
                docUsage.ForEach(x => contract.ActivityDocumentUsage.Add(new ResourceUsageContract(x.VirtualResourceMetadataId, x.DocumentSelectionData)));

                contract.ActivityPrintQueueUsage.Clear();
                var printQueueUsage = context.VirtualResourceMetadataPrintQueueUsages.Where(n => n.VirtualResourceMetadata.VirtualResource.EnterpriseScenarioId == scenarioId).ToList();
                printQueueUsage.ForEach(x => contract.ActivityPrintQueueUsage.Add(new ResourceUsageContract(x.VirtualResourceMetadataId, x.PrintQueueSelectionData)));

                contract.ActivityServerUsage.Clear();
                var serverUsage = context.VirtualResourceMetadataServerUsages.Where(n => n.VirtualResourceMetadata.VirtualResource.EnterpriseScenarioId == scenarioId).ToList();
                serverUsage.ForEach(x => contract.ActivityServerUsage.Add(new ResourceUsageContract(x.VirtualResourceMetadataId, x.ServerSelectionData)));

                contract.ActivityRetrySettings.Clear();
                var retrySettings = context.VirtualResourceMetadataRetrySettings.Where(n => n.VirtualResourceMetadata.VirtualResource.EnterpriseScenarioId == scenarioId).ToList();
                retrySettings.ForEach(x => contract.ActivityRetrySettings.Add(new RetrySettingContract(x.VirtualResourceMetadataId)
                        {
                            State = x.State,
                            Action = x.Action,
                            LimitExceededAction = x.LimitExceededAction,
                            RetryDelay = x.RetryDelay,
                            RetryLimit = x.RetryLimit,
                        }
                    ));
            }
        }

        private static SystemManifest CreateReferenceManifest(Guid scenarioId)
        {
            var manifest = new SystemManifest();
            manifest.PullFromGlobalSettings();
            var _agents = new List<IManifestComponentAgent>()
            {
                new ManifestAssetAgent(scenarioId),
                new ManifestDocumentAgent(scenarioId),
                new ManifestServerAgent(scenarioId),
                new ManifestPrintQueueAgent(scenarioId),
                new ManifestSettingsAgent()
            };

            // Have all agents populate their respective portions of the manifest
            foreach (IManifestComponentAgent agent in _agents)
            {
                agent.AssignManifestInfo(manifest);
            }

            return manifest;
        }

        private static void AddTestDocumentInfoToContract(Guid scenarioId, EnterpriseScenarioContract contract, SystemManifest manifest)
        {
            // Add any test documents referenced in the resource metadata to a collection that will
            // be resolved against the target system during import. 
            contract.TestDocuments.Clear();
            foreach(Document doc in manifest.Documents)
            {
                string relativeFilePath = doc.Group + @"\" + doc.FileName;
                contract.TestDocuments.Add(new TestDocumentContract(relativeFilePath));
                UpdateStatus("Exporting {0}".FormatWith(relativeFilePath));
            }
        }

        private static void AddAssetInfoToContract(Guid scenarioId, EnterpriseScenarioContract contract, SystemManifest manifest)
        {
            contract.AllAssets.Clear();
            foreach(var asset in manifest.AllAssets)
            {
                contract.AllAssets.Add(asset);
                UpdateStatus("Exporting {0}".FormatWith(asset.AssetId));
            }
        }


        /// <summary>
        /// Class ImportMap hold mapping information between what was imported and what will be inserted in the database
        /// </summary>
        public class ImportMap
        {
            /// <summary>
            /// Previous Id
            /// </summary>
            public Guid OldId { get; set; }
            /// <summary>
            /// New Id
            /// </summary>
            public Guid NewId { get; set; }
            /// <summary>
            /// Previous Parent Id
            /// </summary>
            public Guid? OldParentId { get; set; }
            /// <summary>
            /// New Parent Id
            /// </summary>
            public Guid NewParentId { get; set; }
        }

        /// <summary>
        /// Creates a scenario object from the specified scenario contract.
        /// </summary>
        /// <param name="contract">The scenario contract.</param>
        /// <returns></returns>
        public static EnterpriseScenario Create(EnterpriseScenarioContract contract)
        {
            var temp = new List<ImportMap>();
            return Create(contract, out temp);
        }

        /// <summary>
        /// Creates a scenario object from the specified scenario contract, building the specified import maps.
        /// </summary>
        /// <param name="contract">The scenario contract.</param>
        /// <param name="importMaps">The collection of ImportMaps</param>
        /// <returns></returns>
        public static EnterpriseScenario Create(EnterpriseScenarioContract contract, out List<ImportMap> importMaps)
        {
            EnterpriseScenario scenario = new EnterpriseScenario()
            {
                EnterpriseScenarioId = SequentialGuid.NewGuid(),
                Company = contract.Company,
                Description = contract.Description,
                Name = contract.Name,
                Owner = contract.Owner,
                Vertical = contract.Vertical,
            };

            importMaps = new List<ImportMap>();

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                foreach (var resourceDetail in contract.ResourceDetails)
                {
                    Guid oldResourceId = resourceDetail.ResourceId;

                    // This is a translator to target an OfficeWorker or SolutionTester
                    // for the right delivery platform.
                    VirtualResourceType finalResourceType = resourceDetail.ResourceType;
                    if (GlobalSettings.IsDistributedSystem)
                    {
                        if (finalResourceType == VirtualResourceType.SolutionTester)
                        {
                            finalResourceType = VirtualResourceType.OfficeWorker;
                            TraceFactory.Logger.Debug("Changed resource type to Office Worker");
                        }
                    }
                    else
                    {
                        if (finalResourceType == VirtualResourceType.OfficeWorker)
                        {
                            finalResourceType = VirtualResourceType.SolutionTester;
                            TraceFactory.Logger.Debug("Changed resource type to Solution Tester");
                        }
                    }

                    string type = finalResourceType.ToString();
                    if (!context.ResourceTypes.Any(x => x.Name.Equals(type)))
                    {
                        // Skip any invalid resource types for this system.
                        continue;
                    }

                    // Create the entity, load it from the provided detail, then add it to the scenario resource collection
                    VirtualResource resourceEntity = ObjectFactory.Create<VirtualResource>(finalResourceType, finalResourceType.ToString());

                    foreach (ResourceMetadataDetail metadataDetail in resourceDetail.MetadataDetails)
                    {
                        XElement configElement = XElement.Parse(metadataDetail.Data);
                        PluginAssembly pluginAssembly = PluginFactory.GetPluginByAssemblyName("Plugin." + metadataDetail.MetadataType + ".dll");

                        StringBuilder msg = null;
                        if (pluginAssembly.Implements<IPluginValidation>())
                        {
                            IPluginValidation validationEngine = pluginAssembly.Create<IPluginValidation>();
                            PluginConfigurationData pcd = new PluginConfigurationData(configElement, metadataDetail.MetadataVersion);
                            if (validationEngine.ValidateMetadata(ref pcd))
                            {
                                // Save the converted metadata into the database
                                metadataDetail.Data = pcd.GetMetadata().ToString();
                                metadataDetail.MetadataVersion = pcd.MetadataVersion;
                            }
                            else
                            {
                                msg = new StringBuilder("\n\rMetadata for activity type '");
                                msg.Append(metadataDetail.MetadataType);
                                msg.Append("', version ");
                                msg.Append(metadataDetail.MetadataVersion);
                                msg.Append(", Activity Name: ");
                                msg.Append(metadataDetail.Name);
                                msg.Append(" appears out of date. Using the current default settings.");

                                UpdateStatus(msg.ToString());
                            }
                        }
                        else //No validation engine
                        {
                            //We don't want to pop a message that the user won't understand.  Log this condition for now.
                            msg = new StringBuilder($"A plugin validation engine was not found for MetadataType: ");
                            msg.Append(metadataDetail.MetadataType);
                            msg.Append(".  The Metadata was imported 'as-is'.");
                            TraceFactory.Logger.Warn(msg.ToString());
                        }
                    }
                    
                    resourceEntity.LoadDetail(scenario, resourceDetail);

                    if(finalResourceType == VirtualResourceType.SolutionTester)
                    {
                        var workersPerHost = context.ResourceTypes.Where(x => x.Name == "SolutionTester").Select(x => x.MaxResourcesPerHost).FirstOrDefault();
                        resourceEntity.ResourcesPerVM = workersPerHost;
                    }

                    scenario.VirtualResources.Add(resourceEntity);
                    TraceFactory.Logger.Debug("Added {0} to Scenario".FormatWith(resourceEntity.Name));

                    if (finalResourceType != resourceDetail.ResourceType)
                    {
                        // If the final resource type was changed above because are migrating an OfficeWorker to STB
                        // or a SolutionTester to STF, then set the resource type in the loaded entity to the 
                        // final value since it was most likely set to the value of the initial detail.
                        resourceEntity.ResourceType = finalResourceType.ToString();
                        TraceFactory.Logger.Debug("Changing ResourceType to {0}".FormatWith(finalResourceType.ToString()));
                    }

                    importMaps.Add(new ImportMap()
                    {
                        OldId = oldResourceId,
                        NewId  = resourceEntity.VirtualResourceId,
                        OldParentId = null,
                        NewParentId = resourceEntity.EnterpriseScenarioId,
                    });

                    // Prep each activity for insertion into the database
                    foreach (var metadataEntity in resourceEntity.VirtualResourceMetadataSet)
                    {
                        // Change the metadata id to avoid any duplicate key issues when saving to the database
                        var oldMetadataId = metadataEntity.VirtualResourceMetadataId;
                        var newMetadataId = SequentialGuid.NewGuid();
                        metadataEntity.VirtualResourceMetadataId = newMetadataId;

                        // add the usage data as needed - asset, document, etc.
                        contract.ActivityAssetUsage.Where(x => x.VirtualResourceMetadataId == oldMetadataId).ToList().ForEach(x => metadataEntity.AssetUsage =
                            new VirtualResourceMetadataAssetUsage() { VirtualResourceMetadataId = newMetadataId, AssetSelectionData = x.XmlSelectionData });

                        contract.ActivityDocumentUsage.Where(x => x.VirtualResourceMetadataId == oldMetadataId).ToList().ForEach(x => metadataEntity.DocumentUsage =
                            new VirtualResourceMetadataDocumentUsage() { VirtualResourceMetadataId = newMetadataId, DocumentSelectionData = x.XmlSelectionData });

                        contract.ActivityPrintQueueUsage.Where(x => x.VirtualResourceMetadataId == oldMetadataId).ToList().ForEach(x => metadataEntity.PrintQueueUsage =
                            new VirtualResourceMetadataPrintQueueUsage() { VirtualResourceMetadataId = newMetadataId, PrintQueueSelectionData = x.XmlSelectionData });

                        contract.ActivityServerUsage.Where(x => x.VirtualResourceMetadataId == oldMetadataId).ToList().ForEach(x => metadataEntity.ServerUsage =
                            new VirtualResourceMetadataServerUsage() { VirtualResourceMetadataId = newMetadataId, ServerSelectionData = x.XmlSelectionData });

                        contract.ActivityRetrySettings.Where(x => x.VirtualResourceMetadataId == oldMetadataId).ToList().ForEach(x => metadataEntity.VirtualResourceMetadataRetrySettings.Add(
                            new VirtualResourceMetadataRetrySetting()
                            {
                                VirtualResourceMetadataId = newMetadataId,
                                SettingId = SequentialGuid.NewGuid(),
                                State = x.State,
                                Action = x.Action,
                                RetryLimit = x.RetryLimit,
                                RetryDelay = x.RetryDelay,
                                LimitExceededAction = x.LimitExceededAction
                            }));


                        importMaps.Add(new ImportMap()
                        {
                            OldId = oldMetadataId,
                            NewId = newMetadataId,
                            OldParentId = oldResourceId,
                            NewParentId = resourceEntity.VirtualResourceId,
                        });
                    }
                }
            }
            return scenario;
        }

        /// <summary>
        /// Creates a document contract from the specified document object.
        /// </summary>
        /// <param name="document">The Test docuement metadata from the document library database.</param>
        /// <param name="documentPath">The filepath to the document.</param>
        /// <returns></returns>
        public static DocumentContract Create(TestDocument document, string documentPath = "")
        {
            if (string.IsNullOrEmpty(documentPath))
            {
                return new DocumentContract(document);
            }
            
            return new DocumentContract(document, documentPath);
        }

        /// <summary>
        /// Creates a document object from the specified document contract.
        /// </summary>
        /// <param name="context"></param>
        /// <param name="contract"></param>
        /// <returns></returns>
        public static TestDocument Create(DocumentLibraryContext context, DocumentContract contract)
        {
            TestDocument document = new TestDocument()
            {
                TestDocumentId = SequentialGuid.NewGuid(),
                Application = contract.Application,
                AppVersion = contract.AppVersion,
                Author = contract.Author,
                AuthorType = contract.AuthorType,
                ColorMode = contract.ColorMode,
                DefectId = contract.DefectId,
                FileName = contract.FileName,
                FileSize = contract.FileSize,
                FileType = contract.FileType,
                Notes = contract.Notes,
                Orientation = contract.Orientation,
                Pages = contract.Pages,
                SubmitDate = contract.SubmitDate,
                Submitter = contract.Submitter,
                Tag = contract.Tag,
                Vertical = contract.Vertical,
            };

            TestDocumentExtension extension = context.TestDocumentExtensions.FirstOrDefault(x => x.Extension.Equals(contract.Extension));
            document.TestDocumentExtension = extension;

            return document;
        }

        /// <summary>
        /// Creates an asset contract(Printer, VirtualPrinter, Simulator, etc.) from the specified Asset.
        /// </summary>
        /// <param name="asset">The Asset.</param>
        /// <typeparam name="T">The Asset Contract type.</typeparam>
        /// <returns></returns>
        public static T Create<T>(Asset asset) where T : AssetContract, new()
        {
            var contract = new T();
            contract.Load(asset);

            return contract;
        }

        /// <summary>
        /// Creates a Printer object from a PrinterContract.
        /// </summary>
        /// <param name="contract"></param>
        /// <returns></returns>
        public static Printer Create(PrinterContract contract, AssetInventoryContext context)
        {
            var pool = context.AssetPools.FirstOrDefault(n => n.Name == contract.PoolName);

            Printer printer = new Printer()
            {
                AssetId = contract.AssetId,
                AssetType = contract.AssetType,
                Pool = pool,
                Product = contract.Product,
                Model = contract.Model,
                Address1 = contract.Address1,
                Address2 = contract.Address2,
                Description = contract.Description,
                Location = contract.Location,
                PortNumber = contract.PortNumber,
                SnmpEnabled = contract.SnmpEnabled,
                Owner = contract.Owner,
                PrinterType = contract.PrinterType,
                SerialNumber = contract.SerialNumber,
                ModelNumber = contract.ModelNumber,
                EngineType = contract.EngineType,
                FirmwareType = contract.FirmwareType,
            };

            return printer;
        }
    }
}
