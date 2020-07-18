using System.ComponentModel;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Types of monitors that can be instantiated by STF Monitor Service.
    /// </summary>
    public enum STFMonitorType
    {
        /// <summary>
        /// Email address that is an output destination.
        /// </summary>
        [Description("Exchange Email Inbox")]
        OutputEmail,

        /// <summary>
        /// A Directory that is an output destination.
        /// </summary>
        [Description("Output Directory")]
        OutputDirectory,

        /// <summary>
        /// SharePoint document library
        /// </summary>
        [Description("SharePoint Document Library")]
        SharePoint,

        /// <summary>
        /// LANFax output directory
        /// </summary>
        [Description("LAN Fax Destination")]
        LANFax,

        /// <summary>
        /// Digital Send notification email address
        /// </summary>
        [Description("Digital Send Notification Email")]
        DigitalSendNotification,

        /// <summary>
        /// Monitor a Directory location
        /// </summary>
        [Description("Directory")]
        Directory,

        /// <summary>
        /// DSS Server.  "Digital Send" is a generic term for lots of solutions native or otherwise.
        /// DSS carries a connotation of the DSS Server Solution, which is the intent here, 
        /// hence DSS is used here not Digital Send.
        /// </summary>
        [Description("DSS Server")]
        DSSServer,

        /// <summary>
        /// AutoStore Solution Server
        /// </summary>
        [Description("AutoStore Server")]
        AutoStore,

        /// <summary>
        /// The HPCR database monitoring Server.
        /// </summary>
        [Description("Hpcr Server")]
        Hpcr,

        /// <summary>
        /// EPrint Solution Server
        /// </summary>
        [Description("EPrint Server")]
        EPrint

        /*
        /// <summary>
        /// Print Queue
        /// </summary>
        [Description("Print Queue")]
        PrintQueue,


        */
    }
}
