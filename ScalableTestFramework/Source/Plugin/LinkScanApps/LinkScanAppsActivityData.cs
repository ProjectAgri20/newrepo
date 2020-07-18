using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.PluginSupport.JetAdvantageLink;
using HP.ScalableTest.DeviceAutomation.LinkApps.LinkScanApps;

namespace HP.ScalableTest.Plugin.LinkScanApps
{
    [DataContract]
    internal class LinkScanAppsActivityData
    {
        /// <summary>
        /// Gets or sets the scan destination
        /// </summary>
        [DataMember]
        public LinkScanDestination ScanDestination { get; set; }

        /// <summary>
        /// Gets or sets the locktimeouts
        /// </summary>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

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
        /// Gets or sets the from
        /// </summary>
        [DataMember]
        public string From { get; set; }

        /// <summary>
        /// Gets or sets the to
        /// </summary>
        [DataMember]
        public string To { get; set; }

        /// <summary>
        /// Gets or sets the cc
        /// </summary>
        [DataMember]
        public string Cc { get; set; }

        /// <summary>
        /// Gets or sets the bcc
        /// </summary>
        [DataMember]
        public string Bcc { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the message
        /// </summary>
        [DataMember]
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the Server
        /// </summary>
        [DataMember]
        public string Server { get; set; }

        /// <summary>
        /// Gets or sets the user name
        /// </summary>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the FolderPath
        /// </summary>
        [DataMember]
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or sets the password
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Domain and port
        /// </summary>
        [DataMember]
        public string DomainPort { get; set; }

        /// <summary>
        /// Gets or sets the scan options
        /// </summary>
        [DataMember]
        public LinkScanOptions ScanOptions { get; set; }

        /// <summary>
        /// Gets or sets the status of filename checkbox
        /// </summary>
        [DataMember]

        public bool FileNameIsChecked { get; set; }

        /// <summary>
        /// Gets or sets the status of subject checkbox
        /// </summary>
        [DataMember]
        public bool SubjectIsChecked { get; set; }

        /// <summary>
        /// Gets or sets the status of message checkbox
        /// </summary>
        [DataMember]
        public bool MessageIsChecked { get; set; }
        public LinkScanAppsActivityData()
        {
            ScanDestination = LinkScanDestination.Email;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(5), TimeSpan.FromMinutes(10));
            PageCount = 1;
            ScanOptions = new LinkScanOptions();            
        }
    }
}
