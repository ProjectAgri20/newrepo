using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation;

namespace HP.ScalableTest.PluginSupport.JetAdvantageLink
{
    /// <summary>
    /// Configuration data used by a <see cref="LinkScanOptions" />.
    /// </summary>
    [DataContract]
    public class LinkScanOptions
    {
        /// <summary>
        /// Gets or sets the app name.
        /// </summary>
        [DataMember]
        public string AppName { get; set; }
        
        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the Original Sidesusing.
        /// </summary>
        [DataMember]
        public bool UseOriginalSides { get; set; }

        /// <summary>
        /// Gets or sets the Original Sides.
        /// </summary>
        [DataMember]
        public LinkScanOriginalSides OriginalSides { get; set; }

        /// <summary>
        /// Gets or sets the File Type and Resolution using.
        /// </summary>
        [DataMember]
        public bool UseFileTypeandResolution { get; set; }

        /// <summary>
        /// Gets or sets the FileType.
        /// </summary>
        [DataMember]
        public LinkScanFileType FileType { get; set; }

        /// <summary>
        /// Gets or sets the Resolution.
        /// </summary>
        [DataMember]
        public LinkScanResolution Resolution { get; set; }

        /// <summary>
        /// Gets or sets the Color/Black using.
        /// </summary>
        [DataMember]
        public bool UseColorBlack { get; set; }

        /// <summary>
        /// Gets or sets the Color/Black.
        /// </summary>
        [DataMember]
        public LinkScanColorBlack ColorBlack { get; set; }

        /// <summary>
        /// Gets or sets the Original Size using.
        /// </summary>
        [DataMember]
        public bool UseOriginalSize { get; set; }

        /// <summary>
        /// Gets or sets the Original Size.
        /// </summary>
        [DataMember]
        public LinkScanOriginalSize OriginalSize { get; set; }

        /// <summary>
        /// Gets or sets the Content Orientation using.
        /// </summary>
        [DataMember]
        public bool UseContentOrientation { get; set; }

        /// <summary>
        /// Gets or sets the Content Orientation.
        /// </summary>
        [DataMember]
        public LinkScanContentOrientation ContentOrientation { get; set; }

        /// <summary>
        /// Gets or sets the Send to Email.
        /// </summary>
        [DataMember]
        public bool SendtoEmail { get; set; }

        /// <summary>
        /// Text on "To" field for Send to Email.
        /// </summary>
        [DataMember]
        public string SendtoEmailText { get; set; }

        /// <summary>
        /// Creates new CloudPrintOptions
        /// </summary>
        public LinkScanOptions()
        {
            AppName = "Link";
            PageCount = 1;
            OriginalSides = LinkScanOriginalSides.Onesided;
            FileType = LinkScanFileType.JPEG;
            Resolution = LinkScanResolution.e300dpi;
            ColorBlack = LinkScanColorBlack.Color;
            OriginalSize = LinkScanOriginalSize.Letter;
            ContentOrientation = LinkScanContentOrientation.Portrait;
            SendtoEmail = false;
                        
            UseOriginalSides = false;
            UseFileTypeandResolution = false;
            UseColorBlack = false;
            UseOriginalSize = false;
            UseContentOrientation = false;
        }
    }
}
