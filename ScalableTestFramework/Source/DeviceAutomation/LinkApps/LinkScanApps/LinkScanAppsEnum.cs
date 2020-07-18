using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.LinkScanApps
{
    #region LinkScanDestinaion Enums
    /// <summary>
    /// Scan Destination enum
    /// </summary>
    public enum LinkScanDestination
    {
        /// <summary>
        /// Scan Destination: Email
        /// </summary>
        [Description("Email")]
        Email = 0,

        /// <summary>
        /// Scan Destination: SMB
        /// </summary>
        [Description("SMB")]
        SMB,

        /// <summary>
        /// Scan Destination: FTP
        /// </summary>
        [Description("FTP")]
        FTP,
    }
    #endregion

    #region LinkScanAppDisplayName
    /// <summary>
    /// Displayed on UI for Link Scan App
    /// </summary>
    public enum LinkScanAppDisplayName
    {
        /// <summary>
        /// Email
        /// </summary>
        [Description("#hpid-d3b13d6d-ef1f-464c-bb67-8cc52c1bb026-homescreen-button")]
        Email,

        /// <summary>
        /// SMB
        /// </summary>
        [Description("#hpid-368da5fe-2c81-4217-b36d-23247cea0c9f-homescreen-button")]
        SMB,

        /// <summary>
        /// FTP
        /// </summary>
        [Description("#hpid-968afa31-a6d2-425b-8651-74b44daf4dda-homescreen-button")]
        FTP
    }
    #endregion
}
