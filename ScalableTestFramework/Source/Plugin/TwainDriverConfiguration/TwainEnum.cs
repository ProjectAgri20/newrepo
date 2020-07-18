using System.ComponentModel;

namespace HP.ScalableTest.Plugin.TwainDriverConfiguration
{
    /// <summary>
    /// Different Twain Configuration 
    /// </summary>
    public enum TwainConfiguration
    {
        /// <summary>
        /// Saves As PDF
        /// </summary>
        SavesAsPdf,
        /// <summary>
        /// Save As JPEG
        /// </summary>
        SaveAsJpeg,
        /// <summary>
        /// Email As PDF
        /// </summary>
        EmailAsPdf,
        /// <summary>
        /// Email As JPEG
        /// </summary>
        EmailAsJpeg,
        /// <summary>
        /// EveryDay Scan
        /// </summary>
        EveryDayScan,
        /// <summary>
        /// New Scan ShortCut
        /// </summary>
        NewScanShortCut,
    }

    /// <summary>
    /// Type of Selection 
    /// </summary>
    public enum TwainOperation
    {
        /// <summary>
        /// Install
        /// </summary>
        Install,
        /// <summary>
        /// Device Selection
        /// </summary>
        DeviceAddition,

        /// <summary>
        /// Device Selection
        /// </summary>
        ScanConfiguration,
    }

    /// <summary>
    /// Type of PageSides 
    /// </summary>
    public enum PageSides
    {
        /// <summary>
        /// 1 - sided
        /// </summary>
        [Description("1 - sided")]
        OneSided,

        /// <summary>
        /// 2 - sided(book)
        /// </summary>
        [Description("2 - sided(book)")]
        TwoSidedBook,

        /// <summary>
        /// 2 - sided(tablet)
        /// </summary>
        [Description("2 - sided(tablet)")]
        TwoSidedTablet,
    }

    /// <summary>
    /// Type of PageSides 
    /// </summary>
    public enum PageSize
    {
        /// <summary>
        /// Detect Content on Page
        /// </summary>
        [Description("Detect Content on Page")]
        DetectContentOnPage,

        /// <summary>
        /// Entire Scan Area
        /// </summary>
        [Description("Entire Scan Area")]
        EntireScanArea,

        /// <summary>
        /// Letter(8.5 x 11 inches)
        /// </summary>
        [Description("Letter(8.5 x 11 inches)")]
        Letter11Inches,

        /// <summary>
        /// Legal(8.5 x 14 inches)
        /// </summary>
        [Description("Legal(8.5 x 14 inches)")]
        Legal14Inches,

        /// <summary>
        /// A4 (210 x 297 mm)
        /// </summary>
        [Description("A4 (210 x 297 mm)")]
        A4,

        /// <summary>
        /// 8 x 10 in (20 x 25 cm)
        /// </summary>
        [Description("8 x 10 in (20 x 25 cm)")]
        TwentyCrossTwentyFive,
    }

    /// <summary>
    /// Type of Item 
    /// </summary>
    public enum ItemType
    {
        /// <summary>
        /// Document
        /// </summary>      
        [Description("Document")]
        Document,

        /// <summary>
        /// Photo
        /// </summary>       
        [Description("Photo")]
        Photo,
    }

    /// <summary>
    /// Type of Source
    /// </summary>
    public enum Source
    {
        /// <summary>
        /// Document
        /// </summary> 
        [Description("Document Feeder if loaded)")]
        DocumentFeederifloaded,

        /// <summary>
        /// Flatbed
        /// </summary>       
        [Description("Flatbed")]
        Flatbed,
    }

    /// <summary>
    /// Mode of Color
    /// </summary>
    public enum ColorMode
    {
        /// <summary>
        /// Color
        /// </summary>        
        [Description("Color")]
        Color,

        /// <summary>
        /// Gray
        /// </summary>       
        [Description("Gray")]
        Gray,

        /// <summary>
        /// Halftone
        /// </summary>       
        [Description("Halftone")]
        Halftone,

        /// <summary>
        /// Black/White
        /// </summary>
        [Description("Black/White")]
        BlackWhite,

        /// <summary>
        /// Auto Detect Color
        /// </summary>   
        [Description("Auto Detect Color")]
        AutoDetectColor,
    }

    /// <summary>
    /// Type of File
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// PDF
        /// </summary>        
        [Description("Pdf")]
        Pdf,

        /// <summary>
        /// JPEG
        /// </summary>       
        [Description("Jpeg")]
        Jpeg,

        /// <summary>
        /// PNG
        /// </summary>        
        [Description("Png")]
        Png,

        /// <summary>
        /// BMP
        /// </summary>   
        [Description("Bmp")]
        Bmp,

        /// <summary>
        /// TIF
        /// </summary>   
        [Description("Tif")]
        Tif,
    }

    /// <summary>
    /// Type Of SendTo
    /// </summary>
    public enum SendTo
    {
        /// <summary>
        /// Black/White
        /// </summary>
        [Description("Local or Network folder")]
        LocalorNetworkfolder,

        /// <summary>
        /// Email
        /// </summary>          
        [Description("Email")]
        Email,
    }

    /// <summary>
    /// Type Of SendTo
    /// </summary>
    public enum ShortcutSettings
    {
        /// <summary>
        /// Saves As PDF
        /// </summary>
        [Description("SavesAsPdf")]
        SavesAsPdf,
        /// <summary>
        /// Save As JPEG
        /// </summary>
        [Description("SaveAsJpeg")]
        SaveAsJpeg,
        /// <summary>
        /// Email As PDF
        /// </summary>
        [Description("EmailAsPdf")]
        EmailAsPdf,
        /// <summary>
        /// Email As JPEG
        /// </summary>
        [Description("EmailAsJpeg")]
        EmailAsJpeg,
    }
}
