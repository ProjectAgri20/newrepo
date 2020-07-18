using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.iManage;

namespace HP.ScalableTest.Plugin.iManage
{
    [DataContract]
    internal class iManageActivityData
    {
        /// <summary>
        /// Gets or sets the Storage Location
        /// </summary>
        [DataMember]
        public StorageLocation Location { get; set; }

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
        /// Gets or sets the ServerAddress
        /// </summary>
        [DataMember]
        public string ServerAddress { get; set; }

        /// <summary>
        /// Gets or sets the ID
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the PW
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the LogOutMethode
        /// </summary>
        [DataMember]
        public LogOutMethod LogOut { get; set; }

        /// <summary>
        /// Gets or sets the file name
        /// </summary>
        [DataMember]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the page count
        /// </summary>
        [DataMember]
        public int PageCount { get; set; }

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
        /// Gets or sets the scan options
        /// </summary>
        [DataMember]
        public LinkPrintOptions PrintOptions { get; set; }

        [DataMember]
        /// <summary>
        /// Gets or sets the Job type: copy, print, scan
        /// </summary>
        public iManageJobType JobType { get; set; }

        public iManageActivityData()
        {
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            PageCount = 1;
            ScanOptions = new LinkScanOptions();
            PrintOptions = new LinkPrintOptions();
        }
    }
}
