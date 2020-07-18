using System;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using System.Collections.Generic;
using HP.ScalableTest.DeviceAutomation.Enums;

namespace HP.ScalableTest.Plugin.CloudConnector
{
    /// <summary>
    /// Contains data needed to execute the CloudConnector activity.
    /// </summary>
    [DataContract]
    public class CloudConnectorActivityData
    {
        /// <summary>
        /// Gets or sets the CloudAppType: DropBox, GoogleDrvice, OneDriveforPersonal, OneDrvieforBusiness, Sharepoint
        /// </summary>
        [DataMember]
        public ConnectorName CloudAppType { get; set; }

        /// <summary>
        /// Gets or sets the LockTimeOut Data: Acquire Lock Timeouts and Hold Lock Timeouts
        /// </summary>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets the SIO
        /// </summary>
        [DataMember]
        public SIOMethod SIO { get; set; }

        /// <summary>
        /// Gets or sets the ID for Cloud Apps
        /// </summary>
        [DataMember]
        public string ID { get; set; }

        /// <summary>
        /// Gets or sets the Password for Cloud Apps
        /// </summary>
        [DataMember]
        public string PWD { get; set; }

        /// <summary>
        /// Gets or sets the SiteName for SharePpoint
        /// </summary>
        [DataMember]
        public string SharePointSite { get; set; }

        /// <summary>
        /// Gets or sets the LogOutMethode
        /// </summary>
        [DataMember]
        public LogOutMethod LogOut { get; set; }

        /// <summary>
        /// Gets or sets the folder path for scan job
        /// </summary>
        [DataMember]
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the file path for print job
        /// </summary>
        [DataMember]
        public string FilePath { get; set; }

        /// <summary>
        /// Gets or sets the file names for print job with multiple files
        /// </summary>
        [DataMember]
        public List<string> FileList { get; set; }

        /// <summary>
        /// Gets or sets the Page Count
        /// </summary>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the CloudJobType: scan, print, print with multiple files
        /// </summary>
        [DataMember]
        public string CloudJobType { get; set; }

        /// <summary>
        /// Gets or sets the Scan Job Options
        /// </summary>
        [DataMember]
        public LinkScanOptions CloudScanOptions { get; set; }

        /// <summary>
        /// Gets or sets the Print Job Options
        /// </summary>
        [DataMember]
        public LinkPrintOptions CloudPrintOptions { get; set; }

        /// <summary>
        /// Initializes a new instance of the CloudConnectorActivityData class.
        /// </summary>
        public CloudConnectorActivityData()
        {            
            PageCount = 1;
            CloudJobType = "Scan";   
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
        }
    }
}
