using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.DSSInstall
{
    /// <summary>
    /// Contains data needed to execute a DSSInstall activity.
    /// </summary>
    [DataContract]
    public class DSSInstallActivityData
    {
        /// <summary>
        /// Initializes a new instance of the DSSInstallActivityData class.
        /// </summary>
        ///
        [DataMember]
        public string SetupFilePath { get; set; }

        [DataMember]
        public string InstallPath { get; set; }

        [DataMember]
        public bool ValidateInstall { get; set; }

        [DataMember]
        public bool ExternalDatabase { get; set; }

        [DataMember]
        public bool CancelInstall { get; set; }

        [DataMember]
        public bool LaunchApplication { get; set; }

        [DataMember]
        public bool ViewReadme { get; set; }

        [DataMember]
        public TimeSpan TransitionDelay { get; set; }

        [DataMember]
        public InstallOptions InstallOption { get; set; }

        [DataMember]
        public bool SaveSettings { get; set; }
        public DSSInstallActivityData()
        {
            TransitionDelay = TimeSpan.FromSeconds(1);
            InstallOption = InstallOptions.FullInstall;
            ValidateInstall = false;
            CancelInstall = false;
        }
    }

    public enum InstallOptions
    {
        FullInstall = 1,
        Configuration = 2,
        Uninstall = 3
    }
}