using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.Clio;
using HP.ScalableTest.DeviceAutomation.Enums;

namespace HP.ScalableTest.Plugin.Clio
{
    [DataContract]
    internal class ClioActivityData
    {
        /// <summary>
        /// Gets or sets the locktimeouts
        /// </summary>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets the SIO
        /// </summary>
        [DataMember]
        public SIOMethod SIO { get; set; }

        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the PW
        /// </summary>
        [DataMember]
        public string PW { get; set; }

        /// <summary>
        /// Gets or sets the LogOutMethode
        /// </summary>
        [DataMember]
        public LogOutMethod LogOut { get; set; }

        /// <summary>
        /// Gets or sets the Storage Location
        /// </summary>
        [DataMember]
        public StorageLocation Location { get; set; }

        /// <summary>
        /// Gets or sets the page count
        /// </summary>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the matter
        /// </summary>
        [DataMember]
        public string Matter { get; set; }

        /// <summary>
        /// Gets or sets the folder path
        /// </summary>
        [DataMember]
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the scan options
        /// </summary>
        [DataMember]
        public LinkScanOptions ScanOptions { get; set; }

        /// <summary>
        /// Gets or sets the print options
        /// </summary>
        [DataMember]
        public LinkPrintOptions PrintOptions { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the Job type: print, scan
        /// </summary>
        public ClioJobType JobType { get; set; }

        public ClioActivityData()
        {
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            Location = StorageLocation.Matters;
            PageCount = 1;
            ScanOptions = new LinkScanOptions();
            PrintOptions = new LinkPrintOptions();
        }
    }
}
