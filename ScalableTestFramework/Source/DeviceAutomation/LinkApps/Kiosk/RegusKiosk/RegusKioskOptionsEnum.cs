using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.Kiosk.RegusKiosk
{
    // <summary>
    // RegusKioskOptionsEnum contains different settings for Kiosk App
    // </summary>

    #region RegusKioskScanDestination Enums
    /// <summary>
    /// Scan Destination enum
    /// </summary>
    public enum RegusKioskScanDestination
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

    #region RegusKioskPrintSource Enums
    /// <summary>
    /// Print source enum
    /// </summary>
    public enum RegusKioskPrintSource
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

    #region RegusKioskColorMode Enums
    /// <summary>
    /// Color Mode enum
    /// </summary>
    public enum RegusKioskColorMode
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

    #region RegusKioskOriginalSize Enums
    /// <summary>
    /// Original Size enum
    /// </summary>
    public enum RegusKioskOriginalSize
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

        /// <summary>
        /// Original Size: Letter
        /// </summary>
        [Description("Letter")]
        Letter,

        /// <summary>
        /// Original Size: Ledger
        /// </summary>
        [Description("Ledger")]
        Ledger,

        /// <summary>
        /// Original Size: Statement
        /// </summary>
        [Description("Statement")]
        Statement,

        /// <summary>
        /// Original Size: Legal
        /// </summary>
        [Description("Legal")]
        Legal,

        /// <summary>
        /// Original Size: Legal
        /// </summary>
        [Description("Executive")]
        Executive,
    }
    #endregion

    #region RegusKioskNUp Enums
    /// <summary>
    /// N up enum
    /// </summary>
    public enum RegusKioskNUp
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
    public enum RegusKioskOriginalOrientation
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

    #region RegusKioskDuplexSided Enums
    /// <summary>
    /// Duplex enum with sided
    /// </summary>
    public enum RegusKioskDuplexSided
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

    #region RegusKioskDuplexPrint Enums
    /// <summary>
    /// Duplex enum for print
    /// </summary>
    public enum RegusKioskDuplexPrint
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

    #region RegusKioskImageRotation Enums
    /// <summary>
    /// Image Rotaion enum for Scan
    /// </summary>
    public enum RegusKioskImageRotation
    {
        /// <summary>
        /// Image Rotaion: Off
        /// </summary>
        [Description("Off")]
        Off = 0,

        /// <summary>
        /// Image Rotaion: On
        /// </summary>
        [Description("On")]
        On,
    }
    #endregion

    #region RegusKioskReduce/Enlarge Enums
    /// <summary>
    /// A3 Reduce/Enlarge enum
    /// </summary>
    public enum RegusKioskA3ReduceEnlarge
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
        e61pB5,

        /// <summary>
        /// Reduce/Enlarge: 86%
        /// </summary>
        [Description("86% - B4")]
        e86p,

        /// <summary>
        /// Reduce/Enlarge: 66%
        /// </summary>
        [Description("66% - Letter")]
        e66p,

        /// <summary>
        /// Reduce/Enlarge: 93%
        /// </summary>
        [Description("93% - Ledger")]
        e93p,

        /// <summary>
        /// Reduce/Enlarge: 47% 
        /// </summary>
        [Description("47% - Statement")]
        e47p,

        /// <summary>
        /// Reduce/Enlarge: 72% 
        /// </summary>
        [Description("72% - Legal")]
        e72p,

        /// <summary>
        /// Reduce/Enlarge: 61% 
        /// </summary>
        [Description("61% - Executive")]
        e61pExecutive,
    }

    /// <summary>
    /// A4 Reduce/Enlarge enum
    /// </summary>
    public enum RegusKioskA4ReduceEnlarge
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

        /// <summary>
        /// Reduce/Enlarge: 94%
        /// </summary>
        [Description("94% - Letter")]
        e94p,

        /// <summary>
        /// Reduce/Enlarge: 132%
        /// </summary>
        [Description("132% - Ledger")]
        e132p,

        /// <summary>
        /// Reduce/Enlarge: 66% 
        /// </summary>
        [Description("66% - Statement")]
        e66p,

        /// <summary>
        /// Reduce/Enlarge: 102% 
        /// </summary>
        [Description("102% - Legal")]
        e102p,

        /// <summary>
        /// Reduce/Enlarge: 87% 
        /// </summary>
        [Description("87% - Executive")]
        e87p,
    }

    /// <summary>
    /// A5 Reduce/Enlarge enum
    /// </summary>
    public enum RegusKioskA5ReduceEnlarge
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

        /// <summary>
        /// Reduce/Enlarge: 133%
        /// </summary>
        [Description("133% - Letter")]
        e133p,

        /// <summary>
        /// Reduce/Enlarge: 187%
        /// </summary>
        [Description("187% - Ledger")]
        e187p,

        /// <summary>
        /// Reduce/Enlarge: 94% 
        /// </summary>
        [Description("94% - Statement")]
        e94p,

        /// <summary>
        /// Reduce/Enlarge: 145% 
        /// </summary>
        [Description("145% - Legal")]
        e145p,

        /// <summary>
        /// Reduce/Enlarge: 123% 
        /// </summary>
        [Description("123% - Executive")]
        e123p,
    }

    /// <summary>
    /// B4 Reduce/Enlarge enum
    /// </summary>
    public enum RegusKioskB4ReduceEnlarge
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

        /// <summary>
        /// Reduce/Enlarge: 76%
        /// </summary>
        [Description("76% - Letter")]
        e76p,

        /// <summary>
        /// Reduce/Enlarge: 108%
        /// </summary>
        [Description("108% - Ledger")]
        e108p,

        /// <summary>
        /// Reduce/Enlarge: 54% 
        /// </summary>
        [Description("54% - Statement")]
        e54p,

        /// <summary>
        /// Reduce/Enlarge: 84% 
        /// </summary>
        [Description("84% - Legal")]
        e84p,

        /// <summary>
        /// Reduce/Enlarge: 71% 
        /// </summary>
        [Description("71% - Executive")]
        e71p,
    }

    /// <summary>
    /// B5 Reduce/Enlarge enum
    /// </summary>
    public enum RegusKioskB5ReduceEnlarge
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
        /// Reduce/Enlarge: 163% 
        /// </summary>
        [Description("163% - A3")]
        e163p,

        /// <summary>
        /// Reduce/Enlarge: 141%
        /// </summary>
        [Description("141% - B4")]
        e141p,

        /// <summary>
        /// Reduce/Enlarge: 108%
        /// </summary>
        [Description("108% - Letter")]
        e108p,

        /// <summary>
        /// Reduce/Enlarge: 153%
        /// </summary>
        [Description("153% - Ledger")]
        e153p,

        /// <summary>
        /// Reduce/Enlarge: 76% 
        /// </summary>
        [Description("76% - Statement")]
        e76p,

        /// <summary>
        /// Reduce/Enlarge: 118% 
        /// </summary>
        [Description("118% - Legal")]
        e118p,

        /// <summary>
        /// Reduce/Enlarge: 101% 
        /// </summary>
        [Description("101% - Executive")]
        e101p,
    }

    /// <summary>
    /// Letter Reduce/Enlarge enum
    /// </summary>
    public enum RegusKioskLetterReduceEnlarge
    {
        /// <summary>
        /// Reduce/Enlarge: 100% (Original)
        /// </summary>
        [Description("100% - Original")]
        e100p = 0,

        /// <summary>
        /// Reduce/Enlarge: 97%
        /// </summary>
        [Description("97% - A4")]
        e97p,

        /// <summary>
        /// Reduce/Enlarge: 68% 
        /// </summary>
        [Description("68% - A5")]
        e68p,

        /// <summary>
        /// Reduce/Enlarge: 137% 
        /// </summary>
        [Description("137% - A3")]
        e137p,

        /// <summary>
        /// Reduce/Enlarge: 119%
        /// </summary>
        [Description("119% - B4")]
        e119p,

        /// <summary>
        /// Reduce/Enlarge: 84%
        /// </summary>
        [Description("84% - B5")]
        e84p,

        /// <summary>
        /// Reduce/Enlarge: 129%
        /// </summary>
        [Description("129% - Ledger")]
        e129p,

        /// <summary>
        /// Reduce/Enlarge: 64% 
        /// </summary>
        [Description("64% - Statement")]
        e64p,

        /// <summary>
        /// Reduce/Enlarge: 100% 
        /// </summary>
        [Description("100% - Legal")]
        e100pLegal,

        /// <summary>
        /// Reduce/Enlarge: 85% 
        /// </summary>
        [Description("85% - Executive")]
        e85p,
    }

    /// <summary>
    /// Legal Reduce/Enlarge enum
    /// </summary>
    public enum RegusKioskLegalReduceEnlarge
    {
        /// <summary>
        /// Reduce/Enlarge: 100% (Original)
        /// </summary>
        [Description("100% - Original")]
        e100p = 0,

        /// <summary>
        /// Reduce/Enlarge: 83%
        /// </summary>
        [Description("83% - A4")]
        e83p,

        /// <summary>
        /// Reduce/Enlarge: 59% 
        /// </summary>
        [Description("59% - A5")]
        e59p,

        /// <summary>
        /// Reduce/Enlarge: 118% 
        /// </summary>
        [Description("118% - A3")]
        e118p,

        /// <summary>
        /// Reduce/Enlarge: 102%
        /// </summary>
        [Description("102% - B4")]
        e102p,

        /// <summary>
        /// Reduce/Enlarge: 72%
        /// </summary>
        [Description("72% - B5")]
        e72p,

        /// <summary>
        /// Reduce/Enlarge: 78%
        /// </summary>
        [Description("78% - Letter")]
        e78p,

        /// <summary>
        /// Reduce/Enlarge: 60% 
        /// </summary>
        [Description("60% - Statement")]
        e60p,

        /// <summary>
        /// Reduce/Enlarge: 121% 
        /// </summary>
        [Description("121% - Ledger")]
        e121pLegal,

        /// <summary>
        /// Reduce/Enlarge: 75% 
        /// </summary>
        [Description("75% - Executive")]
        e75p,
    }

    /// <summary>
    /// Ledger Reduce/Enlarge enum
    /// </summary>
    public enum RegusKioskLedgerReduceEnlarge
    {
        /// <summary>
        /// Reduce/Enlarge: 100% (Original)
        /// </summary>
        [Description("100% - Original")]
        e100p = 0,

        /// <summary>
        /// Reduce/Enlarge: 68%
        /// </summary>
        [Description("68% - A4")]
        e68p,

        /// <summary>
        /// Reduce/Enlarge: 97% 
        /// </summary>
        [Description("48% - A5")]
        e48p,

        /// <summary>
        /// Reduce/Enlarge: 97% 
        /// </summary>
        [Description("97% - A3")]
        e97p,

        /// <summary>
        /// Reduce/Enlarge: 84%
        /// </summary>
        [Description("84% - B4")]
        e84p,

        /// <summary>
        /// Reduce/Enlarge: 59%
        /// </summary>
        [Description("59% - B5")]
        e59p,

        /// <summary>
        /// Reduce/Enlarge: 64%
        /// </summary>
        [Description("64% - Letter")]
        e64p,

        /// <summary>
        /// Reduce/Enlarge: 50% 
        /// </summary>
        [Description("50% - Statement")]
        e50p,

        /// <summary>
        /// Reduce/Enlarge: 77% 
        /// </summary>
        [Description("77% - Legal")]
        e77p,

        /// <summary>
        /// Reduce/Enlarge: 61% 
        /// </summary>
        [Description("61% - Executive")]
        e61p,
    }

    /// <summary>
    /// Statement Reduce/Enlarge enum
    /// </summary>
    public enum RegusKioskStatementReduceEnlarge
    {
        /// <summary>
        /// Reduce/Enlarge: 100% (Original)
        /// </summary>
        [Description("100% - Original")]
        e100p = 0,

        /// <summary>
        /// Reduce/Enlarge: 137%
        /// </summary>
        [Description("137% - A4")]
        e137p,

        /// <summary>
        /// Reduce/Enlarge: 97% 
        /// </summary>
        [Description("97% - A5")]
        e97p,

        /// <summary>
        /// Reduce/Enlarge: 194% 
        /// </summary>
        [Description("194% - A3")]
        e194p,

        /// <summary>
        /// Reduce/Enlarge: 168%
        /// </summary>
        [Description("168% - B4")]
        e168p,

        /// <summary>
        /// Reduce/Enlarge: 118%
        /// </summary>
        [Description("118% - B5")]
        e118p,

        /// <summary>
        /// Reduce/Enlarge: 129%
        /// </summary>
        [Description("129% - Letter")]
        e129p,

        /// <summary>
        /// Reduce/Enlarge: 199% 
        /// </summary>
        [Description("199% - Ledger")]
        e199p,

        /// <summary>
        /// Reduce/Enlarge: 154% 
        /// </summary>
        [Description("154% - Legal")]
        e154p,

        /// <summary>
        /// Reduce/Enlarge: 123% 
        /// </summary>
        [Description("123% - Executive")]
        e123p,
    }

    /// <summary>
    /// Executive Reduce/Enlarge enum
    /// </summary>
    public enum RegusKioskExecutiveReduceEnlarge
    {
        /// <summary>
        /// Reduce/Enlarge: 100% (Original)
        /// </summary>
        [Description("100% - Original")]
        e100p = 0,

        /// <summary>
        /// Reduce/Enlarge: 111%
        /// </summary>
        [Description("111% - A4")]
        e111p,

        /// <summary>
        /// Reduce/Enlarge: 78% 
        /// </summary>
        [Description("78% - A5")]
        e78p,

        /// <summary>
        /// Reduce/Enlarge: 157% 
        /// </summary>
        [Description("157% - A3")]
        e157p,

        /// <summary>
        /// Reduce/Enlarge: 136%
        /// </summary>
        [Description("136% - B4")]
        e136p,

        /// <summary>
        /// Reduce/Enlarge: 96%
        /// </summary>
        [Description("96% - B5")]
        e96p,

        /// <summary>
        /// Reduce/Enlarge: 104%
        /// </summary>
        [Description("104% - Letter")]
        e104p,

        /// <summary>
        /// Reduce/Enlarge: 151% 
        /// </summary>
        [Description("151% - Ledger")]
        e151p,

        /// <summary>
        /// Reduce/Enlarge: 117% 
        /// </summary>
        [Description("117% - Legal")]
        e117p,

        /// <summary>
        /// Reduce/Enlarge: 76% 
        /// </summary>
        [Description("76% - Statement")]
        e76p,
    }
    #endregion

    #region RegusKioskPaperSource Enums
    /// <summary>
    /// Paper Source enum
    /// </summary>
    public enum RegusKioskPaperSource
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

    #region RegusKioskFileFormat Enums
    /// <summary>
    /// File Format enum
    /// </summary>
    public enum RegusKioskFileFormat
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

    #region RegusKioskResolution Enums
    /// <summary>
    /// Resolution enum
    /// </summary>
    public enum RegusKioskResolution
    {
        /// <summary>
        /// Resolution: 75dpi
        /// </summary>
        [Description("75 dpi")]
        e75dpi = 0,

        /// <summary>
        /// Resolution: 150dpi
        /// </summary>
        [Description("150 dpi")]
        e150dpi,

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
