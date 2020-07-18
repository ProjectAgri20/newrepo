using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.PaperCutInstaller
{
    /// <summary>
    /// Holds the Metadata  between Configuration and Execution Control
    /// </summary>
    [DataContract]
    public class PaperCutInstallerActivityData
    {

        [DataMember]
        public string BundleFile { get; set; }

        [DataMember]
        public string AdminUserName { get; set; }

        [DataMember]
        public string AdminPassword { get; set; }

        [DataMember]
        public string SourcePrintQueue { get; set; }

        [DataMember]
        public PaperCutInstallerAction Action { get; set; }

        [DataMember]
        public PaperCutAuthentication AuthenticationMethod { get; set; }

        [DataMember]
        public PaperCutTracking Tracking { get; set; }

        [DataMember]
        public bool AutoRelease { get; set; }
        /// <summary>
        /// Intialize the new instance of the PaperCutInstallerActivityData class
        /// </summary>
        public PaperCutInstallerActivityData()
        {
            
        }
    }

    public enum PaperCutInstallerAction
    {
        Install,
        Register,
        ConfigureCredentials,
        ConfigureSettings,
    }

    [Flags]
    public enum PaperCutAuthentication
    {
        None =0x0,
        Password = 0x1,
        Identity= 0x2,
        SwipeCard= 0x4,
        Guest = 0x8
    }

    [Flags]
    public enum PaperCutTracking
    {
        None = 0x0,
        Print = 0x1,
        Scan = 0x2,
        Fax = 0x4
    }
}
