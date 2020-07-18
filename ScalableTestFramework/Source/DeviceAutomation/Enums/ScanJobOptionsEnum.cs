using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation
{
    // <summary>
    // JobOptionEnum contains different settings for scan specific and common for scan/copy options 
    // </summary>

    #region Resolution Type
    /// <summary>
    /// Different resolution types
    /// </summary>
    public enum ResolutionType
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// 600 dpi
        /// </summary>
        [Description("600 dpi")]
        e600dpi,
        /// <summary>
        /// 400 dpi
        /// </summary>
        [Description("400 dpi")]
        e400dpi,
        /// <summary>
        /// 300 dpi
        /// </summary>
        [Description("300 dpi")]
        e300dpi,
        /// <summary>
        /// 200 dpi
        /// </summary>
        [Description("200 dpi")]
        e200dpi,
        /// <summary>
        /// 150 dpi
        /// </summary>
        [Description("150 dpi")]
        e150dpi,
        /// <summary>
        /// 75 dpi
        /// </summary>
        [Description("75 dpi")]
        e75dpi,

        #region FaxResolutionType
        /// <summary>
        /// Fine
        /// </summary>
        [Description("Fine(200x200dpi)")]
        Fine,
        /// <summary>
        /// SuperFine
        /// </summary>
        [Description("SuperFine(300x300dpi)")]
        SuperFine,
        /// <summary>
        /// Standard
        /// </summary>
        [Description("Standard(100x200dpi)")]
        Standard
        #endregion
    }
    #endregion

    #region FileType
    /// <summary>
    /// Different File Type
    /// </summary>
    public enum FileType
    {
        //None,

        /// <summary>
        /// Use the device default, most cases is PDF.
        /// </summary>
        [Description("Device Default")]
        DeviceDefault,

        /// <summary>
        /// PDF
        /// </summary>  
        [Description("PDF")]
        PDF,

        /// <summary>
        /// JPEG
        /// </summary>   
        [Description("JPEG")]
        JPEG,

        /// <summary>
        /// TIF
        /// </summary>   
        [Description("TIFF")]
        TIFF,

        /// <summary>
        /// MTIF
        /// </summary>  
        [Description("MTIFF")]
        MTIFF,

        /// <summary>
        /// XPS
        /// </summary>   
        [Description("XPS")]
        XPS,

        /// <summary>
        /// PDFArchivable
        /// </summary>  
        [Description("PDF/A (Archivable)")]
        Archivable,

        /// <summary>
        /// File type of a searchable PDF/A OCR
        /// </summary>
        [Description("Searchable PDF (OCR)")]
        SearchablePdfOcr



    }
    #endregion

    #region OriginalSides
    /// <summary>
    /// Orginal Sides
    /// </summary>
    public enum OriginalSides
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// 1-sided
        /// </summary>
        [Description("1-sided")]
        Onesided,

        /// <summary>
        /// 2-sided
        /// </summary>
        [Description("2-sided")]
        Twosided,
    }
    #endregion

    #region ColorTypes
    /// <summary>
    /// Color types
    /// </summary>
    public enum ColorType
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// AutodetectColororBlack
        /// </summary>
        [Description("Autodetect color or black")]
        AutodetectColorBlack,

        /// <summary>
        /// AutodetectColororGray
        /// </summary>
        [Description("Autodetect color or gray")]
        AutodetectColorGray,

        /// <summary>
        ///Color
        /// </summary>
        [Description("Color")]
        Color,

        /// <summary>
        ///Black/Gray
        /// </summary>
        [Description("Black/Gray")]
        GrayScale,

        /// <summary>
        ///Black
        /// </summary>
        [Description("Black")]
        Monochrome,

    }
    #endregion

    #region OriginalSize
    /// <summary>
    /// OriginalSize
    /// </summary>
    public enum OriginalSize
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// AnySize
        /// </summary>
        [Description("AnySize")]
        Any,

        /// <summary>
        /// Letter
        /// </summary>
        [Description("Letter(8.5x11)")]
        Letter,

        /// <summary>
        /// Legal
        /// </summary>
        [Description("Legal(8.5x14)")]
        Legal,

        /// <summary>
        /// Statement
        /// </summary>
        [Description("Statement(5.5x8.5)")]
        Statement,

        /// <summary>
        /// A4(210x297mm)
        /// </summary>
        [Description("A4(210x297mm)")]
        A4,

        /// <summary>
        /// Mixed Letter/Legal
        /// </summary>
        [Description("Mixed Letter/Legal")]
        MixedLetterLegal,

        /// <summary>
        /// Executive(7.25x10.5)
        /// </summary>
        [Description("Executive(7.25x10.5)")]
        Executive,

        /// <summary>
        /// Oficio(8.5x13)
        /// </summary>
        [Description("Oficio(8.5x13)")]
        EightPointFiveByThirteen,

        ///<summary>
        ///4x6
        ///</summary>
        [Description("4x6")]
        FourXSix,

        /// <summary>
        /// RA4(215x305 mm)
        /// </summary>
        [Description("RA4(215x305 mm)")]
        RA4,

        // Have added below lines of settings for CR ID-2899	Paper size Missing for A3 Product Testing

        /// <summary>
        /// Mixed Letter/Ledger
        /// </summary>
        [Description("Mixed Letter/Ledger")]
        MixedLetterLedger,

        /// <summary>
        /// Mixed A4/A3
        /// </summary>
        [Description("Mixed A4/A3")]
        MixedA4A3,

        /// <summary>
        /// A3 (297x420 mm)
        /// </summary>
        [Description("A3 (297x420 mm)")]
        A3,

        /// <summary>
        /// Ledger (11x7)
        /// </summary>
        [Description("Ledger (11x17)")]
        Ledger,
    }
    #endregion

    #region ContentOrientation
    /// <summary>
    /// ContentOrientation
    /// </summary>
    public enum ContentOrientation
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// Portrait
        /// </summary>
        [Description("Portrait")]
        Portrait,

        /// <summary>
        /// Landscape
        /// </summary>
        [Description("Landscape")]
        Landscape
    }
    #endregion

    #region OptimizeTextpic
    /// <summary>
    /// OptimizeTextpic
    /// </summary>
    public enum OptimizeTextPic
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// Text
        /// </summary>
        [Description("Text")]
        Text,

        /// <summary>
        /// Mixed
        /// </summary>
        [Description("Mixed")]
        Mixed,

        /// <summary>
        /// Printed Picture
        /// </summary>
        [Description("Printed Picture")]
        Photo,

        /// <summary>
        /// Photograph
        /// </summary>
        [Description("Photograph")]
        Glossy
    }
    #endregion

    #region Cropping
    /// <summary>
    /// cropping
    /// </summary>
    public enum Cropping
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// Do not crop
        /// </summary>
        [Description("Do not Crop")]
        Off,

        /// <summary>
        /// Crop to Content
        /// </summary>
        [Description("Crop to Content")]
        ContentCrop
    }
    #endregion

    #region BlankPageSupress
    /// <summary>
    /// Blankpagesupress
    /// </summary>
    public enum BlankPageSupress
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// Off
        /// </summary>
        [Description("Off")]
        Disabled,

        /// <summary>
        /// Supress Blank Pages
        /// </summary>
        [Description("Supress Blank Pages")]
        Enabled
    }
    #endregion

    #region Paper Selection Paper Size
    /// <summary>
    /// Paper selection Paper size enum
    /// </summary>
    public enum PaperSelectionPaperSize
    {
        /// <summary>
        /// Match Original Size
        /// </summary>
        [Description("Match Original Size")]
        Any,
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
        /// Executive (7.25x10.25)
        /// </summary>
        [Description("Executive (7.25x10.25)")]
        Executive,
        /// <summary>
        /// Statement (5.5x8.5)
        /// </summary>
        [Description("Statement (5.5x8.5)")]
        Statement,
        /// <summary>
        /// Oficio (8.5x13)
        /// </summary>
        [Description("Oficio (8.5x13)")]
        EightPointFiveByThirteen,
        /// <summary>
        /// Statement (4x6)
        /// </summary>
        [Description("Statement (4x6)")]
        FourxSix,
        /// <summary>
        /// A4 (210x297 mm)
        /// </summary>
        [Description("A4 (210x297 mm)")]
        A4,
        /// <summary>
        /// RA4 (215x305 mm)
        /// </summary>
        [Description("RA4 (215x305 mm)")]
        RA4,
        /// <summary>
        /// A3 (297x420 mm)
        /// </summary>
        [Description("A3 (297x420 mm)")]
        A3,
        /// <summary>
        /// Ledger (11x7)
        /// </summary>
        [Description("Ledger (11x17)")]
        Ledger

    }
    #endregion

    #region PaperSelectionPaperTypes
    /// <summary>
    /// Paper selection paper type enum
    /// </summary>
    public enum PaperSelectionPaperType
    {
        /// <summary>
        /// Any type
        /// </summary>
        [Description("Any type")]
        AnySupportedType,
        /// <summary>
        /// Plain
        /// </summary>
        [Description("Plain")]
        Plain,
        /// <summary>
        /// Cardstock 176-220g
        /// </summary>
        [Description("Cardstock 176-220g")]
        Cardstock,
        /// <summary>
        /// Labels
        /// </summary>
        [Description("Labels")]
        Labels,
        /// <summary>
        /// Letterhead
        /// </summary>
        [Description("Letterhead")]
        StationeryLetterhead,
        /// <summary>
        /// Envelope
        /// </summary>
        [Description("Envelope")]
        Envelope,
        /// <summary>
        /// Preprinted
        /// </summary>
        [Description("Preprinted")]
        StationeryPreprinted,
        /// <summary>
        /// Prepunched
        /// </summary>
        [Description("Prepunched")]
        StationeryPrepunched,
        /// <summary>
        /// HP EcoEFFICIENT
        /// </summary>
        [Description("HP EcoEFFICIENT")]
        HpEcoSmartlite
    }
    #endregion

    #region Paper Selection Paper tray
    /// <summary>
    /// Paper selection Paper tray enum
    /// </summary>
    public enum PaperSelectionPaperTray
    {
        /// <summary>
        /// Automatically select
        /// </summary>
        [Description("Automatically select")]
        PrinterSelect,
        /// <summary>
        /// Manual Feed
        /// </summary>
        [Description("Manual Feed")]
        ManualFeed,
        /// <summary>
        /// Tray 1
        /// </summary>
        [Description("Tray 1")]
        MultiPurposeTray,
        /// <summary>
        /// Tray 2
        /// </summary>
        [Description("Tray 2")]
        Tray2,
        /// <summary>
        /// Tray 3
        /// </summary>
        [Description("Tray 3")]
        Tray3,
    }
    #endregion

    #region Pages per sheet
    /// <summary>
    /// Pages per Sheet enum
    /// </summary>
    public enum PagesPerSheet
    {
        /// <summary>
        /// One page per sheet
        /// </summary>
        [Description("oneup")]
        OneUp,
        /// <summary>
        /// Two Pages per sheet
        /// </summary>
        [Description("twoup")]
        TwoUp,
        /// <summary>
        /// Four (Right,then down)
        /// </summary>
        [Description("fourup_torighttobottom")]
        FourRtoB,
        /// <summary>
        /// Four (Down, then right)
        /// </summary>
        [Description("fourup_tobottomtoright")]
        FourBtoR
    }
    #endregion

    #region Erase edges
    /// <summary>
    /// Erase edges enums
    /// </summary>
    public enum EraseEdgesType
    {
        /// <summary>
        /// Front Top 
        /// </summary>
        [Description("front-top")]
        FrontTop,
        /// <summary>
        /// Front Bottom 
        /// </summary>
        [Description("front-bottom")]
        FrontBottom,

        /// <summary>
        /// Front left 
        /// </summary>
        [Description("front-left")]
        FrontLeft,
        /// <summary>
        /// Front Right 
        /// </summary>
        [Description("front-right")]
        FrontRight,
        /// <summary>
        /// Back Top 
        /// </summary>
        [Description("back-top")]
        BackTop,
        /// <summary>
        /// Back Bottom 
        /// </summary>
        [Description("back-bottom")]
        BackBottom,
        /// <summary>
        /// Back Left 
        /// </summary>
        [Description("back-left")]
        BackLeft,
        /// <summary>
        /// Back Right 
        /// </summary>
        [Description("back-right")]
        BackRight,
        /// <summary>
        /// Apply same with to all edges
        /// </summary>
        [Description("All Edges")]
        AllEdges
    }
    #endregion
}
