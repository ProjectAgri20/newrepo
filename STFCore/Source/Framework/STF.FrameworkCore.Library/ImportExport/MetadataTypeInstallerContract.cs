using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Xml;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract (import/export) for SoftwareInstallers associated with a MetadataType.
    /// </summary>
    [DataContract(Name = "Installer", Namespace = "")]
    public class MetadataTypeInstallerContract
    {
        [DataMember]
        private Collection<SoftwareInstallerPackageContract> _packages = null;

        /// <summary>
        /// Constructs a new instance of MetadataTypeInstallerContract.
        /// </summary>
        public MetadataTypeInstallerContract()
        {
            _packages = new Collection<SoftwareInstallerPackageContract>();
        }

        /// <summary>
        /// Loads the MetadataTypeInstallerContract using the specified MetadataType name.
        /// </summary>
        /// <param name="metadataTypeName">The MetadataType name.</param>
        public void Load(string metadataTypeName)
        {
            using (var context = new EnterpriseTestContext())
            {
                var metadataType = context.MetadataTypes.First(x => x.Name.Equals(metadataTypeName));
                Name = metadataType.Name;

                foreach (var package in metadataType.SoftwareInstallerPackages)
                {
                    var packageContract = new SoftwareInstallerPackageContract(package);

                    foreach (var setting in package.SoftwareInstallerSettings)
                    {
                        var settingContract = new SoftwareInstallerSettingContract(setting);
                        settingContract.Installer = new SoftwareInstallerContract(setting.SoftwareInstaller);
                        settingContract.Installer.ReadData();

                        packageContract.Settings.Add(settingContract);
                    }

                    Packages.Add(packageContract);
                }
            }
        }

        /// <summary>
        /// Imports this instance of MetadataTypeInstallerContract into the EnterpriseTest database.
        /// </summary>
        /// <param name="context">The EnterpriseTest data context.</param>
        public void Import(EnterpriseTestContext context)
        {
            var metadataType = context.MetadataTypes.FirstOrDefault(x => x.Name.Equals(Name));

            foreach (var installer in Packages.SelectMany(x => x.Settings).Select(x => x.Installer))
            {
                var installerFileName = Path.GetFileName(installer.FilePath);
                if (!context.SoftwareInstallers.Select(x => x.FilePath).Any(x => x.EndsWith(installerFileName)))
                {
                    // Add the installer to the database as it doesn't already exist
                    var saveToPath = Path.GetDirectoryName(GlobalSettings.Items[Setting.ExternalSoftware]);
                    var newInstaller = installer.CreateEntity(saveToPath);
                    context.SoftwareInstallers.AddObject(newInstaller);
                    context.SaveChanges();

                    var directory = Path.GetDirectoryName(newInstaller.FilePath);
                    if (!Directory.Exists(directory))
                    {
                        Directory.CreateDirectory(directory);
                    }

                    // Save the actual installer file to the new location.
                    File.WriteAllBytes(newInstaller.FilePath, Convert.FromBase64String(installer.RawData));
                }
            }

            foreach (var package in Packages)
            {
                var newPackage = package.CreateEntity(metadataType);
                context.SoftwareInstallerPackages.AddObject(newPackage);

                foreach (var setting in package.Settings)
                {
                    var newSetting = setting.CreateEntity();
                    newSetting.InstallerId = setting.Installer.InstallerId;
                    newSetting.PackageId = newPackage.PackageId;

                    context.SoftwareInstallerSettings.AddObject(newSetting);
                }
            }

            context.SaveChanges();
        }

        /// <summary>
        /// Loads and Exports the MetadataTypeInstallerContract using the specified MetadataType name.
        /// </summary>
        /// <param name="metadataTypeName">The MetadataType name.</param>
        /// <param name="filePath">The file path.</param>
        public void Export(string metadataTypeName, string filePath)
        {
            Load(metadataTypeName);
            File.WriteAllText(filePath, LegacySerializer.SerializeDataContract(this).ToString());
        }

        /// <summary>
        /// The MetadataType Name.
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The Installer Packages associated with this Mewtadata Type Name.
        /// </summary>
        public Collection<SoftwareInstallerPackageContract> Packages
        {
            get { return _packages; }
        }
    }
}
