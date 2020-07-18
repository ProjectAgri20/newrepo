using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation.SolutionApps.GeniusBytes
{
    #region GeniusBytesScan JobType Enums
    /// <summary>
    /// GeniusBytes Scan Type
    /// </summary>
    public enum GeniusBytesScanType
    {
        /// <summary>
        /// Job Type: Scan2ME
        /// </summary>
        [Description("Scan2ME")]
        Scan2ME,

        /// <summary>
        /// Job Type: Scan2Home
        /// </summary>
        [Description("Scan2Home")]
        Scan2Home,

        /// <summary>
        /// Job Type: Scan2Mail
        /// </summary>
        [Description("Scan2Mail")]
        Scan2Mail
    }
    #endregion

    #region GeniusButesScan Color Enums
    /// <summary>
    /// Defines Scan Color Option for scanning.
    /// </summary>
    public enum GeniusByteScanColorOption
    {
        /// <summary>
        /// Color
        /// </summary>
        [Description("Color")]
        Color,

        /// <summary>
        /// Grayscale
        /// </summary>
        [Description("Grayscale")]
        Grayscale,

        /// <summary>
        /// Black and White
        /// </summary>
        [Description("Black&White")]
        BlackNWhite
    }
    #endregion

    #region GeniusButesScan Sides Enums
    /// <summary>
    /// Defines Scan Sides Option for scanning.
    /// </summary>
    public enum GeniusByteScanSidesOption
    {
        /// <summary>
        /// Simplex
        /// </summary>
        [Description("Simplex")]
        Simplex,

        /// <summary>
        /// Duplex Long Edge
        /// </summary>
        [Description("Duplex Long Edge")]
        DuplexLongEdge,

        /// <summary>
        /// Duplex Short Edge
        /// </summary>
        [Description("Duplex Short Edge")]
        DuplexShortEdge
    }
    #endregion

    #region GeniusBytesScan File Type Enums
    /// <summary>
    /// Defines Genius Bytes Scan File Type Options.
    /// </summary>
    public enum GeniusByteScanFileTypeOption
    {
        /// <summary>
        /// Multipage PDF
        /// </summary>
        [Description("Multipage PDF")]
        Multipage_PDF,

        /// <summary>
        /// PDF/A
        /// </summary>
        [Description("PDF/A")]
        PDF_A,

        /// <summary>
        /// JPEG
        /// </summary>
        [Description("JPEG")]
        JPEG,

        /// <summary>
        /// Searchable PDF
        /// </summary>
        [Description("Searchable PDF")]
        Searchable_PDF,

        /// <summary>
        /// Searchable PDF/A
        /// </summary>
        [Description("Searchable PDF/A")]
        Searchable_PDF_A,

        /// <summary>
        /// Monopage TIFF
        /// </summary>
        [Description("Monopage TIFF")]
        Monopage_TIFF,

        /// <summary>
        /// Multipage TIFF
        /// </summary>
        [Description("Multipage TIFF")]
        Multipage_TIFF
    }
    #endregion
    #region GeniusBytesScan Resolution Enums
    /// <summary>
    /// Defines Scan Resolution Option for scanning.
    /// </summary>
    public enum GeniusByteScanResolutionOption
    {
        /// <summary>
        /// 200 DPI
        /// </summary>
        [Description("200 DPI")]
        DPI200,

        /// <summary>
        /// 300 DPI
        /// </summary>
        [Description("300 DPI")]
        DPI300,

        /// <summary>
        /// 400 DPI
        /// </summary>
        [Description("400 DPI")]
        DPI400,

        /// <summary>
        /// 600 DPI
        /// </summary>
        [Description("600 DPI")]
        DPI600,
    }
    #endregion
    #region GeniusBytesScan Enable or Disable

    /// <summary>
    /// Turns the Option on or off
    /// </summary>
    public enum GeniusByesScanEnableDisable
    {
        /// <summary>
        /// The disabled
        /// </summary>
        [Description("Disabled")]
        Disabled,

        /// <summary>
        /// The enabled
        /// </summary>
        [Description("Enabled")]
        Enabled
    }

    #endregion

}
