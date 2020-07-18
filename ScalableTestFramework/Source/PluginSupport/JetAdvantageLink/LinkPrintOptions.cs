using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation;

namespace HP.ScalableTest.PluginSupport.JetAdvantageLink
{
    /// <summary>
    /// Configuration data used by a <see cref="LinkPrintActivityManager" />.
    /// </summary>
    [DataContract]
    public class LinkPrintOptions
    {
        /// <summary>
        /// Gets or sets the app name.
        /// </summary>
        [DataMember]
        public string AppName { get; set; }

        /// <summary>
        /// Gets or sets the page count using.
        /// </summary>
        [DataMember]
        public bool UsePageCount { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the Output Sides using.
        /// </summary>
        [DataMember]
        public bool UseOutputSides { get; set; }

        /// <summary>
        /// Gets or sets the Output Sides.
        /// </summary>
        [DataMember]
        public LinkPrintOutputSides OutputSides { get; set; }

        /// <summary>
        /// Gets or sets the Color/Black using.
        /// </summary>
        [DataMember]
        public bool UseColorBlack { get; set; }

        /// <summary>
        /// Gets or sets the Color/Black.
        /// </summary>
        [DataMember]
        public LinkPrintColorBlack ColorBlack { get; set; }

        /// <summary>
        /// Gets or sets the Staple using.
        /// </summary>
        [DataMember]
        public bool UseStaple { get; set; }

        /// <summary>
        /// Gets or sets the Staple.
        /// </summary>
        [DataMember]
        public LinkPrintStaple Staple { get; set; }

        /// <summary>
        /// Gets or sets the PaperSelection using.
        /// </summary>
        [DataMember]
        public bool UsePaperSelection { get; set; }

        /// <summary>
        /// Gets or sets the Paper Size.
        /// </summary>
        [DataMember]
        public LinkPrintPaperSize PaperSize { get; set; }

        /// <summary>
        /// Gets or sets the Paper Tray.
        /// </summary>
        [DataMember]
        public LinkPrintPaperTray PaperTray { get; set; }

        /// <summary>
        /// Creates new CloudPrintOptions
        /// </summary>
        public LinkPrintOptions()
        {
            AppName = "Link";
            PageCount = 1;
            OutputSides = LinkPrintOutputSides.Onesided;
            ColorBlack = LinkPrintColorBlack.Color;
            Staple = LinkPrintStaple.None;
            PaperSize = LinkPrintPaperSize.Letter;
            PaperTray = LinkPrintPaperTray.Auto;

            UsePageCount = false;
            UseOutputSides = false;
            UseColorBlack = false;            
            UseStaple = false;
            UsePaperSelection = false;
        }        
    }
}
