using System;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract (import/export) for SoftwareInstallerPackages.
    /// </summary>
    [DataContract(Name="SoftwareInstallerPackage", Namespace="")]
    public class SoftwareInstallerPackageContract
    {
        /// <summary>
        /// Constructor
        /// </summary>
        public SoftwareInstallerPackageContract(SoftwareInstallerPackage package)
            : this()
        {
            Description = package.Description;
            PackageId = package.PackageId;
        }

        /// <summary>
        /// Creates an instance of a SoftwareInstallerPackage from this instance.
        /// </summary>       
        public SoftwareInstallerPackage CreateEntity(MetadataType type)
        {
            var package = new SoftwareInstallerPackage()
            {
                Description = this.Description,
                PackageId = SequentialGuid.NewGuid(),
            };
            package.MetadataTypes.Add(type);

            return package;
        }

        /// <summary>
        /// Constructor.
        /// </summary>
        public SoftwareInstallerPackageContract()
        {
            _settings = new Collection<SoftwareInstallerSettingContract>();
        }

        /// <summary>
        /// Collection of SoftwareInstallerSettingsContracts.
        /// </summary>
        [DataMember]
        private Collection<SoftwareInstallerSettingContract> _settings = null;

        /// <summary>
        /// The Installer Package Id.
        /// </summary>
        [DataMember]
        public Guid PackageId { get; set; }

        /// <summary>
        /// The Description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Collection of SoftwareInstallerSettingsContracts.
        /// </summary>
        public Collection<SoftwareInstallerSettingContract> Settings
        {
            get { return _settings; }
        }
    }
}
