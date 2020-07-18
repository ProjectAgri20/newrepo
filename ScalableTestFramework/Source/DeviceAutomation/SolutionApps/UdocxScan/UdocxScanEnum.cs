using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.UdocxScan
{
    #region UdocxScan JobType Enums
    /// <summary>
    /// UdocxScan Destination enum
    /// </summary>
    public enum UdocxScanJobType
    {
        /// <summary>
        /// Job Type: ScantoEmail
        /// </summary>
        [Description("Scan to Mail/Drafts")]
        ScantoMail,

        /// <summary>
        /// Job Type: ScantoOneDrive
        /// </summary>
        [Description("Scan to OneDrive")]
        ScantoOneDrive,

        /// <summary>
        /// Job Type: ScantoSharePoint365
        /// </summary>
        [Description("Scan to SharePoint 365")]
        ScantoSharePoint365
    }
    #endregion
}
