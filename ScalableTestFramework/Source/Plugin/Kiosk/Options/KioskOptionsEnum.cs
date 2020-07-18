using System.ComponentModel;

namespace HP.ScalableTest.Plugin.Kiosk
{
    // <summary>
    // KioskOptionsEnum contains different settings for Kiosk App
    // </summary>

    #region KioskScanDestination Enums
    /// <summary>
    /// Scan Destination enum
    /// </summary>
    public enum KioskScanDestination
    {
        /// <summary>
        /// Scan Destination: USB
        /// </summary>
        [Description("USB")]
        USB = 0,

        /// <summary>
        /// Scan Destination: Email
        /// </summary>
        [Description("Email")]
        Email,
    }
    #endregion

    #region KioskPrintSource Enums
    /// <summary>
    /// Print source enum
    /// </summary>
    public enum KioskPrintSource
    {
        /// <summary>
        /// Print Source: USB
        /// </summary>
        [Description("USB")]
        USB = 0,

        /// <summary>
        /// Print Source: PrinterOn
        /// </summary>
        [Description("PrinterOn")]
        PrinterOn,
    }
    #endregion

    #region KioskColorMode Enums
    /// <summary>
    /// Color Mode enum
    /// </summary>
    public enum KioskColorMode
    {
        /// <summary>
        /// Color mode: Color
        /// </summary>
        [Description("Color")]
        Color = 0,

        /// <summary>
        /// Color mode: Mono
        /// </summary>
        [Description("Mono")]
        Mono,
    }
    #endregion

    #region KioskOriginalSize Enums
    /// <summary>
    /// Original Size enum
    /// </summary>
    public enum KioskOriginalSize
    {
        /// <summary>
        /// Original Size: A3
        /// </summary>
        [Description("A3")]
        A3 = 0,

        /// <summary>
        /// Original Size: A4
        /// </summary>
        [Description("A4")]
        A4,

        /// <summary>
        /// Original Size: A5
        /// </summary>
        [Description("A5")]
        A5,

        /// <summary>
        /// Original Size: B4
        /// </summary>
        [Description("B4")]
        B4,

        /// <summary>
        /// Original Size: B5
        /// </summary>
        [Description("B5")]
        B5,
    }
    #endregion

    #region KioskNUp Enums
    /// <summary>
    /// N up enum
    /// </summary>
    public enum KioskNUp
    {
        /// <summary>
        /// Original Size: 1 Up
        /// </summary>
        [Description("1 Up")]
        OneUp = 0,

        /// <summary>
        /// Original Size: 2 Up
        /// </summary>
        [Description("2 Up")]
        TwoUp,

        /// <summary>
        /// Original Size: 4 Up
        /// </summary>
        [Description("4 Up")]
        FourUp,
    }
    #endregion

    #region KioskOriginalOrientation Enums
    /// <summary>
    /// Original Orientation enum
    /// </summary>
    public enum KioskOriginalOrientation
    {
        /// <summary>
        /// Original Orientation: Upright Images
        /// </summary>
        [Description("Upright Images")]
        UprightImages = 0,

        /// <summary>
        /// Original Orientation: Sideways Images
        /// </summary>
        [Description("Sideways Images")]
        SidewaysImages,
    }
    #endregion

    #region KioskDuplexSided Enums
    /// <summary>
    /// Duplex enum with sided
    /// </summary>
    public enum KioskDuplexSided
    {
        /// <summary>
        /// Duplex sided: 1 Sided
        /// </summary>
        [Description("1 Sided")]
        OneSided = 0,

        /// <summary>
        /// Duplex sided: 2 Sided, Book
        /// </summary>
        [Description("2 Sided, Book")]
        TwoSidedBook,

        /// <summary>
        /// Duplex sided: 2 Sided, Calendar
        /// </summary>
        [Description("2 Sided, Calendar")]
        TwoSidedCalendar,
    }
    #endregion

    #region KioskDuplexPrint Enums
    /// <summary>
    /// Duplex enum for print
    /// </summary>
    public enum KioskDuplexPrint
    {
        /// <summary>
        /// Duplex print: Off
        /// </summary>
        [Description("Off")]
        Off = 0,

        /// <summary>
        /// Duplex print: Long Edge
        /// </summary>
        [Description("Long Edge")]
        LongEdge,

        /// <summary>
        /// Duplex print: Short Edge
        /// </summary>
        [Description("Short Edge")]
        ShortEdge,
    }
    #endregion

    /// <summary>
    /// A3 Reduce/Enlarge enum
    /// </summary>
    public enum KioskA3ReduceEnlarge
    {
        /// <summary>
        /// Reduce/Enlarge: 100% (Original)
        /// </summary>
        [Description("100% - Original")]
        e100p = 0,

        /// <summary>
        /// Reduce/Enlarge: 70%
        /// </summary>
        [Description("70% - A4")]
        e70p,

        /// <summary>
        /// Reduce/Enlarge: 50% 
        /// </summary>
        [Description("50% - A5")]
        e50p,

        /// <summary>
        /// Reduce/Enlarge: 61% 
        /// </summary>
        [Description("61% - B5")]
        e61p,

        /// <summary>
        /// Reduce/Enlarge: 86%
        /// </summary>
        [Description("86% - B4")]
        e86p,
    }

    /// <summary>
    /// A4 Reduce/Enlarge enum
    /// </summary>
    public enum KioskA4ReduceEnlarge
    {
        /// <summary>
        /// Reduce/Enlarge: 100% (Original)
        /// </summary>
        [Description("100% - Original")]
        e100p = 0,

        /// <summary>
        /// Reduce/Enlarge: 86% (A4 -> B5)
        /// </summary>
        [Description("86% - B5")]
        e86p,

        /// <summary>
        /// Reduce/Enlarge: 70% (A4 -> A5)
        /// </summary>
        [Description("70% - A5")]
        e70p,

        /// <summary>
        /// Reduce/Enlarge: 141% (A4 -> A3)
        /// </summary>
        [Description("141% - A3")]
        e141p,

        /// <summary>
        /// Reduce/Enlarge: 122% (A4 -> B4)
        /// </summary>
        [Description("122% - B4")]
        e122p,
    }

    /// <summary>
    /// A5 Reduce/Enlarge enum
    /// </summary>
    public enum KioskA5ReduceEnlarge
    {
        /// <summary>
        /// Reduce/Enlarge: 100% (Original)
        /// </summary>
        [Description("100% - Original")]
        e100p = 0,

        /// <summary>
        /// Reduce/Enlarge: 141%
        /// </summary>
        [Description("141% - A4")]
        e141p,

        /// <summary>
        /// Reduce/Enlarge: 122% 
        /// </summary>
        [Description("122% - B5")]
        e122p,

        /// <summary>
        /// Reduce/Enlarge: 200% 
        /// </summary>
        [Description("200% - A3")]
        e200p,

        /// <summary>
        /// Reduce/Enlarge: 173%
        /// </summary>
        [Description("173% - B4")]
        e173p,
    }

    /// <summary>
    /// B4 Reduce/Enlarge enum
    /// </summary>
    public enum KioskB4ReduceEnlarge
    {
        /// <summary>
        /// Reduce/Enlarge: 100% (Original)
        /// </summary>
        [Description("100% - Original")]
        e100p = 0,

        /// <summary>
        /// Reduce/Enlarge: 81%
        /// </summary>
        [Description("81% - A4")]
        e81p,

        /// <summary>
        /// Reduce/Enlarge: 70% 
        /// </summary>
        [Description("70% - B5")]
        e70p,

        /// <summary>
        /// Reduce/Enlarge: 115% 
        /// </summary>
        [Description("115% - A3")]
        e115p,

        /// <summary>
        /// Reduce/Enlarge: 57%
        /// </summary>
        [Description("57% - A5")]
        e57p,
    }

    /// <summary>
    /// B5 Reduce/Enlarge enum
    /// </summary>
    public enum KioskB5ReduceEnlarge
    {
        /// <summary>
        /// Reduce/Enlarge: 100% (Original)
        /// </summary>
        [Description("100% - Original")]
        e100p = 0,

        /// <summary>
        /// Reduce/Enlarge: 115%
        /// </summary>
        [Description("115% - A4")]
        e115p,

        /// <summary>
        /// Reduce/Enlarge: 81% 
        /// </summary>
        [Description("81% - A5")]
        e81p,

        /// <summary>
        /// Reduce/Enlarge: 162% 
        /// </summary>
        [Description("162% - A3")]
        e162p,

        /// <summary>
        /// Reduce/Enlarge: 141%
        /// </summary>
        [Description("141% - B4")]
        e141p,
    }    

    #region KioskPaperSource Enums
    /// <summary>
    /// Paper Source enum
    /// </summary>
    public enum KioskPaperSource
    {
        /// <summary>
        /// Paper Source: Auto
        /// </summary>
        [Description("Auto")]
        Auto = 0,

        /// <summary>
        /// Paper Source: Tray1
        /// </summary>
        [Description("Tray1")]
        Tray1,

        /// <summary>
        /// Paper Source: Tray2
        /// </summary>
        [Description("Tray2")]
        Tray2,

        /// <summary>
        /// Paper Source: Tray3
        /// </summary>
        [Description("Tray3")]
        Tray3,

        /// <summary>
        /// Paper Source: Tray4
        /// </summary>
        [Description("Tray4")]
        Tray4,
    }
    #endregion

    #region KioskFileFormat Enums
    /// <summary>
    /// File Format enum
    /// </summary>
    public enum KioskFileFormat
    {
        /// <summary>
        /// File Format: PDF
        /// </summary>
        [Description("PDF")]
        PDF = 0,

        /// <summary>
        /// File Format: TIFF
        /// </summary>
        [Description("TIFF")]
        TIFF,

        /// <summary>
        /// File Format: JPEG
        /// </summary>
        [Description("JPEG")]
        JPEG,
    }
    #endregion

    #region KioskResolution Enums
    /// <summary>
    /// Resolution enum
    /// </summary>
    public enum KioskResolution
    {
        /// <summary>
        /// Resolution: 150dpi
        /// </summary>
        [Description("150 dpi")]
        e150dpi = 0,

        /// <summary>
        /// Resolution: 200dpi
        /// </summary>
        [Description("200 dpi")]
        e200dpi,

        /// <summary>
        /// Resolution: 300dpi
        /// </summary>
        [Description("300 dpi")]
        e300dpi,

        /// <summary>
        /// Resolution: 600dpi
        /// </summary>
        [Description("600 dpi")]
        e600dpi,
    }
    #endregion
}
