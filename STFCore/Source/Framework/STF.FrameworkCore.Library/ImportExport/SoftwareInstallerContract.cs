using System;
using System.IO;
using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest.Model;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract (import/export) for SoftwareInstallers.
    /// </summary>
    [DataContract(Name="SoftwareInstaller", Namespace="")]
    public class SoftwareInstallerContract
    {
        /// <summary>
        /// Constructor.
        /// </summary>
        public SoftwareInstallerContract(SoftwareInstaller installer)
        {
            InstallerId = installer.InstallerId;
            Description = installer.Description;
            FilePath = installer.FilePath;
            Arguments = installer.Arguments;
            RebootSetting = installer.RebootSetting;
            CopyDirectory = installer.CopyDirectory;
        }

        /// <summary>
        /// Creates an instance of a SoftwareInstaller from this instance.
        /// </summary>
        public SoftwareInstaller CreateEntity(string softwareRepository)
        {
            return new SoftwareInstaller()
            {
                InstallerId = this.InstallerId,
                Description = this.Description,
                FilePath = @"{0}{1}".FormatWith(softwareRepository, this.FilePath.Replace('/', '\\')),
                Arguments = this.Arguments,
                RebootSetting = this.RebootSetting,
                CopyDirectory = this.CopyDirectory,
            };
        }

        /// <summary>
        /// Reads the data from the file specified in FilePath and sets the RawData value to the output.
        /// </summary>
        public void ReadData()
        {
            // Load the raw data for the installer file, then strip the
            // server name of the file path for the installer.
            RawData = Convert.ToBase64String(File.ReadAllBytes(FilePath));
            FilePath = new Uri(FilePath).AbsolutePath;
        }

        /// <summary>
        /// The Software Installer Id.
        /// </summary>
        [DataMember]
        public Guid InstallerId { get; set; }

        /// <summary>
        /// The Description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// The FilePath of the installer file.
        /// </summary>
        [DataMember]
        public string FilePath { get; set; }

        /// <summary>
        /// The Arguments.
        /// </summary>
        [DataMember]
        public string Arguments { get; set; }

        /// <summary>
        /// The Reboot Setting.
        /// </summary>
        [DataMember]
        public string RebootSetting { get; set; }

        /// <summary>
        /// Whether to copy the entire directory.
        /// </summary>
        [DataMember]
        public bool CopyDirectory { get; set; }

        /// <summary>
        /// The raw data of the file specified by FilePath.
        /// </summary>
        [DataMember]
        public string RawData { get; set; }
    }
}
