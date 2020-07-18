using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation
{
    // <summary>
    // CloudScanOptionsEnum contains different settings for Print from Cloud 
    // </summary>

    #region Original Side Enums
    /// <summary>
    /// Original Side enum
    /// </summary>
    public enum LinkScanOriginalSides
    {
        /// <summary>
        /// 1-sided mode
        /// </summary>
        [Description("1-sided")]
        Onesided = 0,

        /// <summary>
        /// 2-sided mode
        /// </summary>
        [Description("2-sided")]
        Twosided,

        /// <summary>
        /// Pages filp up with 2-sided mode
        /// </summary>
        [Description("2-sided(pages flip up)")]
        Pagesflipup
    }
    #endregion

    #region File Type Enums
    /// <summary>
    /// File Type enum
    /// </summary>
    public enum LinkScanFileType
    {
        /// <summary>
        /// JPEG mode
        /// </summary>
        [Description("JPEG")]
        JPEG = 0,

        /// <summary>
        /// PDF mode
        /// </summary>
        [Description("PDF")]
        PDF,

        /// <summary>
        /// TIFF mode
        /// </summary>
        [Description("TIFF")]
        TIFF

    }
    #endregion

    #region Resolution Enums
    /// <summary>
    /// Resolution enum
    /// </summary>
    public enum LinkScanResolution
    {
        /// <summary>
        /// 75dpi mode
        /// </summary>
        [Description("75 dpi")]
        e75dpi = 0,
        
        /// <summary>
        /// 150dpi
        /// </summary>
        [Description("150 dpi")]
        e150dpi,

        /// <summary>
        /// 200dpi
        /// </summary>
        [Description("200 dpi")]
        e200dpi,

        /// <summary>
        /// 300dpi
        /// </summary>
        [Description("300 dpi")]
        e300dpi,

        /// <summary>
        /// 400dpi
        /// </summary>
        [Description("400 dpi")]
        e400dpi,

        /// <summary>
        /// 600dpi
        /// </summary>
        [Description("600 dpi")]
        e600dpi
    }
    #endregion

    #region Color/Balck Enums
    /// <summary>
    /// Color/Black enum
    /// </summary>
    public enum LinkScanColorBlack
    {
        /// <summary>
        /// Auto mode
        /// </summary>
        [Description("Automatically detect color or black")]
        Auto = 0,

        /// <summary>
        /// Color mode
        /// </summary>
        [Description("Color")]
        Color,

        /// <summary>
        /// Black/Gray mode
        /// </summary>
        [Description("Black/Gray")]
        BlackGray,

        /// <summary>
        /// Black mode
        /// </summary>
        [Description("Black")]
        Black

    }
    #endregion

    #region Original Size Enums
    /// <summary>
    /// Paper Selection enum
    /// </summary>
    public enum LinkScanOriginalSize
    {
        /// <summary>
        /// Any Size
        /// </summary>
        [Description("Any Size")]
        AnySize = 0,

        /// <summary>
        /// Letter (8.5x11)
        /// </summary>
        [Description("Letter (8.5x11)")]
        Letter,

        /// <summary>
        /// Legal (8.5x14)
        /// </summary>
        [Description("Legal (8.5x14)")]
        Legal,

        /// <summary>
        /// Executive (7.25x10.5)
        /// </summary>
        [Description("Executive (7.25x10.5)")]
        Executive,

        /// <summary>
        /// Statement (5.5x8.5)
        /// </summary>
        [Description("Statement (5.5x8.5)")]
        Statement,

        /// <summary>
        /// Ledger (11x17)
        /// </summary>
        [Description("Ledger (11x17)")]
        Ledger,

        /// <summary>
        /// A3 (297x420 mm)
        /// </summary>
        [Description("A3 (297x420 mm)")]
        A3,

        /// <summary>
        /// A4 (210x297 mm)
        /// </summary>
        [Description("A4 (210x297 mm)")]
        A4,

        /// <summary>
        /// A5 (148x210 mm)
        /// </summary>
        [Description("A5 (148x210 mm)")]
        A5,

        /// <summary>
        /// B4 (257x364 mm)
        /// </summary>
        [Description("B4 (257x364 mm)")]
        B4,

        /// <summary>
        /// B5 (182x257 mm)
        /// </summary>
        [Description("B5 (182x257 mm)")]
        B5
    }
    #endregion

    #region Content Orientation Enums
    /// <summary>
    /// Content Orientation enum
    /// </summary>
    public enum LinkScanContentOrientation
    {
        /// <summary>
        /// Portrait mode
        /// </summary>
        [Description("Portrait")]
        Portrait = 0,

        /// <summary>
        /// Landscape mode
        /// </summary>
        [Description("Landscape")]
        Landscape
    }
    #endregion
}
