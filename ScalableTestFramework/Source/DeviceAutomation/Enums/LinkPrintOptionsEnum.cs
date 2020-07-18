using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation
{
    // <summary>
    // CloudPrintOptionsEnum contains different settings for Print from Cloud 
    // </summary>

    #region Output Sides Enums
    /// <summary>
    /// Output Sides enum
    /// </summary>
    public enum LinkPrintOutputSides
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

    #region Color/Balck Enums
    /// <summary>
    /// Color/Black enum
    /// </summary>
    public enum LinkPrintColorBlack
    {
        /// <summary>
        /// Auto mode
        /// </summary>
        [Description("Auto")]
        Auto = 0,

        /// <summary>
        /// Color mode
        /// </summary>
        [Description("Color")]
        Color,

        /// <summary>
        /// Black/Gray
        /// </summary>
        [Description("Black/Gray")]
        BlackGray
    }
    #endregion

    #region Staple Enums
    /// <summary>
    /// Staple enum
    /// </summary>
    public enum LinkPrintStaple
    {
        /// <summary>
        /// None mode
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// Top left mode
        /// </summary>
        [Description("Top left")]
        Topleft,

        /// <summary>
        /// Top right mode
        /// </summary>
        [Description("Top right")]
        Topright,

        /// <summary>
        /// Two left mode
        /// </summary>
        [Description("Two left")]
        Twoleft,

        /// <summary>
        /// Two top mode
        /// </summary>
        [Description("Two top")]
        Twotop,

        /// <summary>
        /// Two right mode
        /// </summary>
        [Description("Two right")]
        Tworight
    }
    #endregion

    #region Paper Size Enums
    /// <summary>
    /// Paper Size enum
    /// </summary>
    public enum LinkPrintPaperSize
    {        
        /// <summary>
        /// Letter (8.5x11)
        /// </summary>
        [Description("Letter (8.5x11)")]
        Letter = 0,

        /// <summary>
        /// Legal (8.5x14)
        /// </summary>
        [Description("Legal (8.5x14)")]
        Legal,
        
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
        /// A6 (105x148 mm)
        /// </summary>
        [Description("A6 (105x148 mm)")]
        A6,

        /// <summary>
        /// B4 (257x364 mm)
        /// </summary>
        [Description("B4 (257x364 mm)")]
        B4,

        /// <summary>
        /// B5 (182x257 mm)
        /// </summary>
        [Description("B5 (182x257 mm)")]
        B5,

        /// <summary>
        /// Oficio (8.5x13)
        /// </summary>
        [Description("Oficio (8.5x13)")]
        Oficio
    }
    #endregion

    #region Paper Tray Enums
    /// <summary>
    /// Paper Tray enum
    /// </summary>
    public enum LinkPrintPaperTray
    {
        /// <summary>
        /// Auto
        /// </summary>
        [Description("Automatically select")]
        Auto = 0,

        /// <summary>
        /// Tray1
        /// </summary>
        [Description("Tray1")]
        Tray1,

        /// <summary>
        /// Tray2
        /// </summary>
        [Description("Tray2")]
        Tray2,

        /// <summary>
        /// Tray3
        /// </summary>
        [Description("Tray3")]
        Tray3,

        /// <summary>
        /// Tray4
        /// </summary>
        [Description("Tray4")]
        Tray4,

        /// <summary>
        /// Tray5
        /// </summary>
        [Description("Tray5")]
        Tray5,
    }
    #endregion
}
