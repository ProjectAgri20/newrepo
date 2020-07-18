using System;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Class used for marshalling data for software installation files 
    /// that need to be installed on the client before activity execution begins.
    /// </summary>
    [DataContract]
    public class SoftwareInstallerDetail
    {
        /// <summary>
        /// Gets or sets the InstallerId.
        /// </summary>
        [DataMember]
        public Guid InstallerId { get; set; }

        /// <summary>
        /// Gets or sets the Installer FilePath.
        /// </summary>
        [DataMember]
        public string Path { get; set; }

        /// <summary>
        /// Gets or sets the Installer Arguments.
        /// </summary>
        [DataMember]
        public string Arguments { get; set; }

        /// <summary>
        /// Gets or sets whether a reboot is requred after installation.
        /// </summary>
        [DataMember]
        public RebootMode RebootMode { get; set; }

        /// <summary>
        /// Gets or sets whether to copy the installer file's entire directory.
        /// </summary>
        [DataMember]
        public bool CopyDirectory { get; set; }

        /// <summary>
        /// Gets or sets the installation order number of this Installer.
        /// </summary>
        [DataMember]
        public int InstallOrderNumber { get; set; }

        /// <summary>
        /// Gets or sets the Installer Description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }
    }
}
