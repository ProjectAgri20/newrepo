using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Plugin.HpacClient
{
    /// <summary>
    /// Data Class for the HPAC Client Plugin
    /// </summary>
    [DataContract]
    public class HpacClientActivityData
    {
        /// <summary>
        /// Contains the Print Driver details for the selected driver
        /// </summary>
        [DataMember]
        public PrintDriverInfo PrintDriver { get; set; }

        /// <summary>
        /// LPR Queue Name as in HPAC Server
        /// </summary>
        [DataMember]
        public string LprQueueName { get; set; }

        /// <summary>
        /// Set the installed Driver/Printer as a default Printer
        /// </summary>
        [DataMember]
        public bool IsDefaultPrinter { get; set; }

        /// <summary>
        /// Enable BIDI Communication Support for Specified Print Driver
        /// </summary>
        [DataMember]
        public bool EnableBidi { get; set; }

        /// <summary>
        /// Enable Printing after the Spooling is Completed
        /// </summary>
        [DataMember]
        public bool PrintAfterSpooling { get; set; }

        /// <summary>
        /// Enables HPAC Client Installation
        /// </summary>
        [DataMember]
        public bool InstallHpacClient { get; set; }

        /// <summary>
        /// Path of HPAC Client Installer MSI file
        /// </summary>
        [DataMember]
        public string HpacClientInstallerPath { get; set; }

        /// <summary>
        /// Name / IP address of the HPAC Server responsible for Job Accounting
        /// </summary>
        [DataMember]
        public string HpacJAServerName { get; set; }

        /// <summary>
        /// Name / IP address of the HPAC Server responsible for IPM(Intelligent Print Management)
        /// </summary>
        [DataMember]
        public string HpacIPMServerName { get; set; }

        /// <summary>
        /// Name / IP address of the HPAC Server responsible for Pull Printing Jobs
        /// </summary>
        [DataMember]
        public string HpacPullPrintServerName { get; set; }

        /// <summary>
        /// Enable Quota on HPAC Client
        /// </summary>
        [DataMember]
        public bool Quota { get; set; }

        /// <summary>
        /// Enable IPM on HPAC Client
        /// </summary>
        [DataMember]
        public bool Ipm { get; set; }

        /// <summary>
        /// Enable Delegate on HPAC Client
        /// </summary>
        [DataMember]
        public bool Delegate { get; set; }

        /// <summary>
        /// Enable LocalJobStorage on HPAC Client
        /// </summary>
        [DataMember]
        public bool LocalJobStorage { get; set; }

        /// <summary>
        /// Constructor for HPAC Client Plugin Data Class
        /// </summary>
        public HpacClientActivityData()
        {
            HpacClientInstallerPath = string.Empty;
            EnableBidi = false;
            PrintAfterSpooling = true;
            InstallHpacClient = false;
            IsDefaultPrinter = true;

            HpacJAServerName = string.Empty;
            HpacIPMServerName = string.Empty;
            HpacPullPrintServerName = string.Empty;
            Quota = false;
            Ipm = false;
            Delegate = false;
            LocalJobStorage = false;
        }
    }
}
