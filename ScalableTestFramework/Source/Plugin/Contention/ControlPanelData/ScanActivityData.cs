using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Contention
{
    /// <summary>
    /// Contains data about the Scan activity in the Contention Plugin
    /// </summary>
    [ControlPanelActivity("Scan")]
    public class ScanActivityData
    {
        /// <summary>
        /// Gets or sets a value for Page Count
        /// </summary>
        /// <value>Page Count</value>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets a value for the Type of Scan Job (Email, Folder, Usb or Job Storage)
        /// </summary>
        [DataMember]
        public ContentionScanActivityTypes ScanJobType { get; set; }

        /// <summary>
        /// Gets or sets the email address (for ScanToEmail)
        /// </summary>
        /// <value>The email address</value>
        [DataMember]
        public string EmailAddress { get; set; }

        /// <summary>
        /// Gets or sets the Folder path (for ScanToFolder)
        /// </summary>
        /// <value>The folder path</value>
        [DataMember]
        public string FolderPath { get; set; }

        /// <summary>
        /// Gets or Sets the USB name (for ScanToUsb)
        /// </summary>
        /// <value>The USB name</value>
        [DataMember]
        public string UsbName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScanActivityData"/> class
        /// </summary>
        public ScanActivityData()
        {
            PageCount = 1;
            EmailAddress = string.Empty;
            FolderPath = string.Empty;
            UsbName = string.Empty;
        }
    }
}
