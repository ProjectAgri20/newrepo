using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data contract (used for import/export) used to serialize an EnterpriseScenario object.
    /// </summary>
    [DataContract(Name = "Scenario", Namespace = "")]
    public class EnterpriseScenarioContract
    {
        private Collection<string> _userGroups = null;
        private Collection<FolderContract> _folders = null;
        private Collection<string> _associatedProducts = null;

        /// <summary>
        /// Default Constructor.
        /// </summary>
        public EnterpriseScenarioContract()
        {
            ResourceDetails = new Collection<ResourceDetailBase>();
            TestDocuments = new Collection<TestDocumentContract>();
            UserGroups = new Collection<string>();
            AllAssets = new Collection<AssetInfo>();

            ActivityAssetUsage = new Collection<ResourceUsageContract>();
            ActivityDocumentUsage = new Collection<ResourceUsageContract>();
            ActivityPrintQueueUsage = new Collection<ResourceUsageContract>();
            ActivityServerUsage = new Collection<ResourceUsageContract>();
            ActivityRetrySettings = new Collection<RetrySettingContract>();
        }

        /// <summary>
        /// EnterpriseScenarioContract constructor.
        /// </summary>
        /// <param name="scenario">The EnterpriseScnenario.</param>
        /// <param name="version">The scenario version.</param>
        public EnterpriseScenarioContract(EnterpriseScenario scenario, string version)
            : this()
        {
            Company = scenario.Company;
            Description = scenario.Description;
            Name = scenario.Name;
            Owner = scenario.Owner;
            Vertical = scenario.Vertical;
            ContractVersion = version;
            
            foreach (var group in scenario.UserGroups)
            {
                UserGroups.Add(group.GroupName);
            }
        }

        /// <summary>
        /// Contract Version
        /// </summary>
        [DataMember]
        public string ContractVersion { get; set; }

        /// <summary>
        /// Name of the contract
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Description
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// The Company
        /// </summary>
        [DataMember]
        public string Company { get; set; }

        /// <summary>
        /// The vertical.
        /// </summary>
        [DataMember]
        public string Vertical { get; set; }

        /// <summary>
        /// Owner of the scenario
        /// </summary>
        [DataMember]
        public string Owner { get; set; }

        /// <summary>
        /// User Groups associated with the scenario
        /// </summary>
        [DataMember]
        public Collection<string> UserGroups
        {
            get { return _userGroups ?? (_userGroups = new Collection<string>()); }
            set { _userGroups = value; }
        }

        /// <summary>
        /// Folders associated with the scenario
        /// </summary>
        [DataMember]
        public Collection<FolderContract> Folders
        {
            get
            {
                if (_folders == null)
                {
                    _folders = new Collection<FolderContract>();
                }
                return _folders;
            }
            set { _folders = value; }
        }

        /// <summary>
        /// Associated Product Names
        /// </summary>
        [DataMember]
        public Collection<string> AssociatedProductNames
        {
            get
            {
                if (_associatedProducts == null)
                {
                    _associatedProducts = new Collection<string>();
                }
                return _associatedProducts;
            }
            set { _associatedProducts = value; }
        }

        /// <summary>
        /// Resources used in Scenario
        /// </summary>
        [DataMember]
        public Collection<ResourceDetailBase> ResourceDetails { get; private set; }

        /// <summary>
        /// Collection of Test Documents
        /// </summary>
        [DataMember]
        public Collection<TestDocumentContract> TestDocuments { get; private set; }

        /// <summary>
        /// Assets used by the scenario
        /// </summary>
        [DataMember]
        public Collection<AssetInfo> AllAssets { get; private set; }

        /// <summary>
        /// Activity Asset Usage
        /// </summary>
        [DataMember]
        public Collection<ResourceUsageContract> ActivityAssetUsage { get; private set; }

        /// <summary>
        /// Activity Document Usage
        /// </summary>
        [DataMember]
        public Collection<ResourceUsageContract> ActivityDocumentUsage { get; private set; }

        /// <summary>
        /// Activity Server Usage
        /// </summary>
        [DataMember]
        public Collection<ResourceUsageContract> ActivityServerUsage { get; private set; }

        /// <summary>
        /// Activity PrintQueue Usage
        /// </summary>
        [DataMember]
        public Collection<ResourceUsageContract> ActivityPrintQueueUsage { get; private set; }

        /// <summary>
        /// Activity Retry Settings
        /// </summary>
        [DataMember]
        public Collection<RetrySettingContract> ActivityRetrySettings { get; private set; }

        /// <summary>
        /// Scans the Test Document Library for the documents contained in this ScenarioContract.
        /// </summary>
        public void ScanTestDocuments()
        {
            List<string> databaseFiles = null;
            using (DocumentLibraryContext context = DbConnect.DocumentLibraryContext())
            {
                databaseFiles = context.TestDocuments.Select(x => x.TestDocumentExtension.Location + @"\" + x.FileName).ToList();
            }

            var missingDocuments = TestDocuments.Where(x => !databaseFiles.Any(y => x.Original.Equals(y))).Distinct();
            foreach (var document in missingDocuments)
            {
                document.ResolutionRequired = true;
            }
        }

        /// <summary>
        /// Returns serialized String
        /// </summary>
        /// <returns></returns>
        public string Save()
        {
            return Serializer.Serialize(this).ToString();
        }

        /// <summary>
        /// Writes this serialized instance to a file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        public void Save(string fileName)
        {
            File.WriteAllText(fileName, Save());
        }

        /// <summary>
        /// List of Invalid resources
        /// </summary>
        public List<string> InvalidResourceTypes
        {
            get
            {
                List<string> values = null;

                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    // Return all resource types that are in the ResourceDetails but are
                    // not available in the ResourceTypes table.  In other words they
                    // are unsupported types.
                    values = ResourceDetails.Select(detail => detail.ResourceType.ToString())                                          
                        .Where(typeName => !context.ResourceTypes.Any(name => name.Name.Equals(typeName))).Distinct().ToList();
                }

                if (GlobalSettings.IsDistributedSystem && values.Contains("SolutionTester"))
                {
                    // Remove SolutionTester from the invalid resource types as it will be changed
                    // to an OfficeWorker for an STF System
                    values.Remove("SolutionTester");
                }
                else if (!GlobalSettings.IsDistributedSystem && values.Contains("OfficeWorker"))
                {
                    // Remove OfficeWorker from the invalid resource types as it will be changed
                    // to a SolutionTester for an STF System
                    values.Remove("OfficeWorker");
                }

                return values;
            }
        }
    }
}
