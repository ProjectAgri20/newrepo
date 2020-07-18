using System.ComponentModel;

namespace HP.ScalableTest.Plugin.DriverConfigurationPrint.Enum
{
    #region Paper Quality Tab

    #region Paper Size Enums
    /// <summary>
    /// Paper Size Enum
    /// </summary>
    public enum PaperSize
    {
        /// <summary>
        /// Letter
        /// </summary>
        [Description("Letter")]
        Letter = 0,

        /// <summary>
        /// Legal
        /// </summary>
        [Description("Legal")]
        Legal,

        /// <summary>
        /// Executive
        /// </summary>
        [Description("Executive")]
        Executive,

        /// <summary>
        /// Statement
        /// </summary>
        [Description("Statement")]
        Statement,

        /// <summary>
        /// 11x17
        /// </summary>
        [Description("11x17")]
        A11x17,

        /// <summary>
        /// 4x6
        /// </summary>
        [Description("4x6")]
        A4x6,

        /// <summary>
        /// 5x7
        /// </summary>
        [Description("5x7")]
        A5x7,

        /// <summary>
        /// 5x8
        /// </summary>
        [Description("5x8")]
        A5x8,


        /// <summary>
        /// A3
        /// </summary>
        [Description("A3")]
        A3,

        /// <summary>
        /// A4
        /// </summary>
        [Description("A4")]
        A4,

        /// <summary>
        /// A5
        /// </summary>
        [Description("A5")]
        A5,

        /// <summary>
        /// A6
        /// </summary>
        [Description("A6")]
        A6,

        /// <summary>
        /// RA4
        /// </summary>
        [Description("RA4")]
        RA4,

        /// <summary>
        /// SRA4
        /// </summary>
        [Description("SRA4")]
        SRA4,

        /// <summary>
        /// B4 (JIS)
        /// </summary>
        [Description("B4 (JIS)")]
        B4,

        /// <summary>
        /// B5 (JIS)
        /// </summary>
        [Description("B5 (JIS)")]
        B5,

        /// <summary>
        /// B6 (JIS)
        /// </summary>
        [Description("B6 (JIS)")]
        B6,

        /// <summary>
        /// 10x15cm
        /// </summary>
        [Description("10x15cm")]
        A10x15cm,

        /// <summary>
        /// Oficio 216x340 mm
        /// </summary>
        [Description("Oficio 216x340 mm")]
        Oficio,

        /// <summary>
        /// 8K 270x390 mm
        /// </summary>
        [Description("8K 270x390 mm")]
        A8K270x390mm,

        /// <summary>
        /// 16K 195x270 mm
        /// </summary>
        [Description("16K 195x270 mm")]
        A16K195x270mm,

        /// <summary>
        /// 8K 260x368 mm
        /// </summary>
        [Description("8K 260x368 mm")]
        A8K260x368mm,

        /// <summary>
        /// 16K 184x260 mm
        /// </summary>
        [Description("16K 184x260 mm")]
        A16K184x260mm,

        /// <summary>
        /// 8K 273x394 mm
        /// </summary>
        [Description("8K 273x394 mm")]
        A8K273x394mm,

        /// <summary>
        /// 16K 197x273 mm
        /// </summary>
        [Description("16K 197x273 mm")]
        A16K197x273mm,

        /// <summary>
        /// Japanese Postcard
        /// </summary>
        [Description("Japanese Postcard")]
        JapanesePostcard,

        /// <summary>
        /// Double Japan Postcard Rotated
        /// </summary>
        [Description("Double Japan Postcard Rotated")]
        DoubleJapanPostcardRotated,

        /// <summary>
        /// Envelope #9
        /// </summary>
        [Description("Envelope #9")]
        Envelope9,

        /// <summary>
        /// Envelope #10
        /// </summary>
        [Description("Envelope #10")]
        Envelope10,

        /// <summary>
        /// Envelope Monarch
        /// </summary>
        [Description("Envelope Monarch")]
        EnvelopeMonarch,

        /// <summary>
        /// Envelope B5
        /// </summary>
        [Description("Envelope B5")]
        EnvelopeB5,

        /// <summary>
        /// Envelope C5
        /// </summary>
        [Description("Envelope C5")]
        EnvelopeC5,

        /// <summary>
        /// Envelope C6
        /// </summary>
        [Description("Envelope C6")]
        EnvelopeC6,

        /// <summary>
        /// Envelope DL
        /// </summary>
        [Description("Envelope DL")]
        EnvelopeDL,

        /// <summary>
        /// 3X5
        /// </summary>
        [Description("3X5")]
        A3X5,

        /// <summary>
        /// Oficio 8.5x13
        /// </summary>
        [Description("Oficio 8.5x13")]
        Oficio85x13,
    }
    #endregion

    /// <summary>
    /// Paper Type Enum
    /// </summary>
    public enum PaperType
    {
        /// <summary>
        /// Unspecified
        /// </summary>
        [Description("Unspecified")]
        Unspecified = 0,

        /// <summary>
        /// Plain
        /// </summary>
        [Description("Plain")]
        Plain,

        /// <summary>
        /// HP EcoFFICIENT
        /// </summary>
        [Description("HP EcoFFICIENT")]
        HPEcoFFICIENT,

        /// <summary>
        /// HP LaserJet 90g
        /// </summary>
        [Description("HP LaserJet 90g")]
        HPLaserJet90g,

        /// <summary>
        /// HP Color Laser Matte 105g
        /// </summary>
        [Description("HP Color Laser Matte 105g")]
        HPColorLaserMatte105g,

        /// <summary>
        /// HP Premium Choice Matte 120g
        /// </summary>
        [Description("HP Premium Choice Matte 120g")]
        HPPremiumChoiceMatte120g,

        /// <summary>
        /// HP Brochure Matte 150g
        /// </summary>
        [Description("HP Brochure Matte 150g")]
        HPBrochureMatte150g,

        /// <summary>
        /// HP Cover Matte 200g
        /// </summary>
        [Description("HP Cover Matte 200g")]
        HPCoverMatte200g,

        /// <summary>
        /// HP Matte Photo 200g
        /// </summary>
        [Description("HP Matte Photo 200g")]
        HPMattePhoto200g,

        /// <summary>
        /// Light 60-74g
        /// </summary>
        [Description("Light 60-74g")]
        Light6074g,

        /// <summary>
        /// Mid-Weight 96-110g
        /// </summary>
        [Description("Mid-Weight 96-110g")]
        MidWeight96110g,

        /// <summary>
        /// Heavy 111-130g
        /// </summary>
        [Description("Heavy 111-130g")]
        Heavy111130g,

        /// <summary>
        /// Extra Heavy 131-175g
        /// </summary>
        [Description("Extra Heavy 131-175g")]
        ExtraHeavy131175g,

        /// <summary>
        /// Cardstock 176-220g
        /// </summary>
        [Description("Cardstock 176-220g")]
        Cardstock176220g,

        /// <summary>
        /// Labels
        /// </summary>
        [Description("Labels")]
        Labels,

        /// <summary>
        /// Letterhead
        /// </summary>
        [Description("Letterhead")]
        Letterhead,

        /// <summary>
        /// Envelope
        /// </summary>
        [Description("Envelope")]
        Envelope,

        /// <summary>
        /// Preprinted,
        /// </summary>
        [Description("Preprinted")]
        Preprinted,

        /// <summary>
        /// Prepunched
        /// </summary>
        [Description("Prepunched")]
        Prepunched,

        /// <summary>
        /// Colored
        /// </summary>
        [Description("Colored")]
        Colored,

        /// <summary>
        /// Bond
        /// </summary>
        [Description("Bond")]
        Bond,

        /// <summary>
        /// Recycled
        /// </summary>
        [Description("Recycled")]
        Recycled,

        /// <summary>
        /// Intermediate 85-96g
        /// </summary>
        [Description("Intermediate 85-96g")]
        Intermediate8596g,

        /// <summary>
        /// Monochrome Laser Transparency
        /// </summary>
        [Description("Monochrome Laser Transparency")]
        MonochromeLaserTransparency,
    }

    /// <summary>
    /// Paper Source Enum
    /// </summary>
    public enum PaperSource
    {
        /// <summary>
        /// Printer auto select
        /// </summary>
        [Description("Printer auto select")]
        PrinterAutoSelect = 0,

        /// <summary>
        /// Manual Feed
        /// </summary>
        [Description("Manual Feed")]
        ManualFeed,

        /// <summary>
        /// Tray 1
        /// </summary>
        [Description("Tray 1")]
        Tray1,

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

        /// <summary>
        /// Tray 4
        /// </summary>
        [Description("Tray 4")]
        Tray4,

        /// <summary>
        /// Tray 5
        /// </summary>
        [Description("Tray 5")]
        Tray5
    }

    /// <summary>
    /// Paper Quality Enum
    /// </summary>
    public enum PrintQuality
    {
        /// <summary>
        /// Normal
        /// </summary>
        [Description("Normal")]
        Normal = 0,

        /// <summary>
        /// Fine Lines
        /// </summary>
        [Description("Fine Lines")]
        FineLines,

        /// <summary>
        /// Quick View
        /// </summary>
        [Description("Quick View")]
        QuickView,

        /// <summary>
        /// FastRes 1200
        /// </summary>
        [Description("FastRes 1200")]
        FastRes,

        /// <summary>
        /// 1200 X 1200
        /// </summary>
        [Description("1200 x 1200")]
        A12001X200,

        /// ImageREt 3600
        /// </summary>
        [Description("ImageREt 3600")]
        ImageREt3600,

        /// <summary>
        /// General Office
        /// </summary>
        [Description("General Office")]
        GeneralOffice,

        /// <summary>
        /// Professional
        /// </summary>
        [Description("Professional")]
        Professional,

        /// <summary>
        /// Presentation
        /// </summary>
        [Description("Presentation")]
        Presentation,
    }

    /// <summary>
    /// Special Pages enum
    /// </summary>
    public enum SpecialPages
    {
        /// <summary>
        /// Front Cover:No Covers
        /// </summary>
        [Description("Front Cover:No Covers")]
        FrontCoverNoCovers,

        /// <summary>
        /// Back Cover:No Covers
        /// </summary>
        [Description("Back Cover:No Covers")]
        BackCoverNoCovers,

        /// <summary>
        /// Print pages on different paper:None
        /// </summary>
        [Description("Print pages on different paper:None")]
        PrintPagesondifferentPaper,

        /// <summary>
        /// Insert blank or preprinted sheets:None 
        /// </summary>
        [Description("Insert blank or preprinted sheets:None")]
        Insertblankorpreprintedsheets
    }

    #endregion

    #region Effects
    /// <summary>
    /// Water Mark Enum
    /// </summary>
    public enum WaterMark
    {
        /// <summary>
        /// [none]
        /// </summary>
        [Description(" [none]")]
        none = 0,

        /// <summary>
        /// Confidential
        /// </summary>
        [Description("Confidential")]
        Confidential,

        /// <summary>
        /// Draft
        /// </summary>
        [Description("Draft")]
        Draft,

        /// <summary>
        /// SAMPLE
        /// </summary>
        [Description("SAMPLE")]
        SAMPLE,
    }
    #endregion

    #region Finishing Tab
    /// <summary>
    /// Booklet Layout Enum
    /// </summary>
    public enum BookletLayout
    {
        /// <summary>
        /// Off
        /// </summary>
        [Description("Off")]
        Off = 0,

        /// <summary>
        /// Left binding
        /// </summary>
        [Description("Left binding")]
        Leftbinding,

        /// <summary>
        /// Right binding
        /// </summary>
        [Description("Right binding")]
        Rightbinding,
    }

    /// <summary>
    /// Pages Per Sheet Enum
    /// </summary>
    public enum PagesPerSheet
    {
        /// <summary>
        /// 1 page per sheet
        /// </summary>
        [Description("1 page per sheet")]
        A1pagepersheet = 0,

        /// <summary>
        /// 2 pages per sheet
        /// </summary>
        [Description("2 pages per sheet")]
        A2pagespersheet,

        /// <summary>
        /// 4 pages per sheet
        /// </summary>
        [Description("4 pages per sheet")]
        A4pagespersheet,

        /// <summary>
        /// 6 pages per sheet
        /// </summary>
        [Description("6 pages per sheet")]
        A6pagespersheet,

        /// <summary>
        /// 9 pages per sheet
        /// </summary>
        [Description("9 pages per sheet")]
        A9pagespersheet,

        /// <summary>
        /// 16 pages per sheet
        /// </summary>
        [Description("16 pages per sheet")]
        A16pagespersheet,
    }

    #endregion

    #region Output Tab
    /// <summary>
    /// Page Order Enum
    /// </summary>
    public enum PageOrder
    {
        /// <summary>
        /// Right, then Down
        /// </summary>
        [Description("Right, then Down")]
        RightthenDown = 0,

        /// <summary>
        /// Down, then Right
        /// </summary>
        [Description("Down, then Right")]
        Confidential,

        /// <summary>
        /// Left, then Down
        /// </summary>
        [Description("Left, then Down")]
        LeftthenDown,

        /// <summary>
        /// Down, then Left
        /// </summary>
        [Description("Down, then Left")]
        DownthenLeft,
    }

    /// <summary>
    /// Output Bin Enum
    /// </summary>
    public enum OutputBin
    {
        /// <summary>
        /// Automatically Select
        /// </summary>
        [Description("Automatically Select")]
        AutomaticallySelect = 0,

        /// <summary>
        /// Output Bin 1
        /// </summary>
        [Description("Output Bin 1")]
        OutputBin1,

        /// <summary>
        /// Output Bin 2
        /// </summary>
        [Description("Output Bin 2")]
        OutputBin2,

        /// <summary>
        /// Standard Bin
        /// </summary>
        [Description("Standard Bin")]
        StandardBin,

        /// <summary>
        /// Alternate Bin
        /// </summary>
        [Description("Alternate Bin")]
        AlternateBin,
    }

    /// <summary>
    /// Staple Enum
    /// </summary>
    public enum Staple
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// One Staple
        /// </summary>
        [Description("One Staple")]
        OneStaple,

        /// <summary>
        /// One Staple Angled
        /// </summary>
        [Description("One Staple Angled")]
        oneStapleAngled,

        /// <summary>
        /// One Staple Left
        /// </summary>
        [Description("One Staple Left")]
        OneStapleLeft,

        /// <summary>
        /// One Staple Right
        /// </summary>
        [Description("One Staple Right")]
        OneStapleRight,

        /// <summary>
        /// Two Staples Left
        /// </summary>
        [Description("Two Staples Left")]
        TwoStaplesLeft,

        /// <summary>
        /// Two Staples Right
        /// </summary>
        [Description("Two Staples Right")]
        TwoStaplesRight,

        /// <summary>
        /// Two Staples Top
        /// </summary>
        [Description("Two Staples Top")]
        TwoStaplesTop,

        /// <summary>
        /// Two Staples Left or Top
        /// </summary>
        [Description("Two Staples Left or Top")]
        TwoStaplesLeftorRight,

        /// <summary>
        /// Fold and Stitch
        /// </summary>
        [Description("Fold and Stitch")]
        FoldandStitch
    }

    /// <summary>
    /// Punch Enum
    /// </summary>
    public enum Punch
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None = 0,

        ///<summary>
        /// 2 Hole Punch Left
        ///</summary>
        [Description("2-Hole Punch Left")]
        TwoHolePunchLeft,

        /// <summary>
        /// 2 Hole Punch Right
        /// </summary>
        [Description("2-Hole Punch Right")]
        TwoHolePunchRight,
        /// <summary>
        /// 2 hole Punch Top
        /// </summary>
        [Description("2-Hole Punch Top")]
        TwoHolePunchTop,

        /// <summary>
        /// 2 Hole Punch Bottom
        /// </summary>
        [Description("2-Hole Punch Bottom")]
        TwoHolePunchBottom,

        /// <summary>
        /// 2 Hole Punch Left or Top
        /// </summary>
        [Description("2-Hole Punch Left or Top")]
        TwoHolePunchLeftorTop,

        /// <summary>
        /// 4 Hole Punch Left
        /// </summary>
        [Description("4-Hole Punch Left")]
        FourHolePunchLeft,

        /// <summary>
        /// 4 Hole Punch Right
        /// </summary>
        [Description("4-Hole Punch Right")]
        FourHolePunchRight,

        /// <summary>
        /// 4 Hole Punch Top
        /// </summary>
        [Description("4-Hole Punch Top")]
        FourHolePunchTop,

        /// <summary>
        /// 4 Hole Punch Left or Top
        /// </summary>
        [Description("4-Hole Punch Left or Top")]
        FourHolePunchLeftorTop
    }

    public enum Fold
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// Inward C-fold,opens to left or up
        /// </summary>
        [Description("Inward C-fold, opens to left or up")]
        InwardCfoldOpensToLeftorUp,

        /// <summary>
        /// Inward C-fold,opens to right or down
        /// </summary>
        [Description("Inward C-fold, opens to right or down")]
        InwardCfoldOpensToRightorDown,

        /// <summary>
        /// Outward C-fold,opens to left or up
        /// </summary>
        [Description("Outward C-fold, opens to left or up")]
        OutwardCfoldOpensToLeftorUp,

        /// <summary>
        /// Outward C-fold,opens to right or down
        /// </summary>
        [Description("Outward C-fold, opens to right or down")]
        OutwardCfoldOpensToRightorDown,

        /// <summary>
        /// Inward V-fold
        /// </summary>
        [Description("Inward V-fold")]
        InwardVfold,

        /// <summary>
        /// Outward V-fold
        /// </summary>
        [Description("Outward V-fold")]
        OutwardVfold
    }
    #endregion

    #region Job Storage
    ///<summary>
    /// Job Storage
    ///</summary>
    public enum MakeJobPrivateSecure
    {
        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None = 0,

        /// <summary>
        /// PIN to print
        /// </summary>
        [Description("PIN to print")]
        PINtoprint,

        /// <summary>
        /// Encrypt Job (with password)
        /// </summary>
        [Description("Encrypt Job (with password)")]
        EncryptJobwithpassword,
    }

    /// <summary>
    /// If Job Name Exists Enum
    /// </summary>
    public enum IfJobNameExists
    {
        /// <summary>
        /// Use Job Name + (1-99)
        /// </summary>
        [Description("Use Job Name + (1-99)")]
        UseJobName199 = 0,

        /// <summary>
        /// Replace Existing File
        /// </summary>
        [Description("Replace Existing File")]
        ReplaceExistingFile,
    }
    #endregion

    #region Color
    public enum GrayScale
    {
        /// <summary>
        /// Off
        /// </summary>
        [Description("Off")]
        off,

        /// <summary>
        /// On
        /// </summary>
        [Description("On")]
        On,

        /// <summary>
        /// Black only
        /// </summary>
        [Description("Black Only")]
        Blackonly,

        /// <summary>
        /// High Quality CMYK GrayScale
        /// </summary>
        [Description("High Quality CMYK Grayscale")]
        HighQualityCMYK

    }

    public enum RGBColor
    {
        /// <summary>
        /// Default (sRGB)
        /// </summary>
        [Description("Default (sRGB)")]
        DefaultsRGB,

        /// <summary>
        /// Photo (sRGB)
        /// </summary>
        [Description("Photo (sRGB)")]
        PhotosRGB,

        /// <summary>
        /// Photo (Adobe RGB 1998
        /// </summary>
        [Description("Photo (Adobe RGB 1998)")]
        PhotoAdobeRGB1998,

        /// <summary>
        /// Vivid (sRGB)
        /// </summary>
        [Description("Vivid (sRGB)")]
        VividsRGB,

        /// <summary>
        /// None
        /// </summary>
        [Description("None")]
        None,

        /// <summary>
        /// Custom Profile
        /// </summary>
        [Description("Custom Profile")]
        CustomProfile
    }
    #endregion

    #region Advanced
    public enum RasterCompression
    {
        /// <summary>
        /// Automatic
        /// </summary>
        [Description("Automatic")]
        Automatic = 0,

        /// <summary>
        /// Best Quality
        /// </summary>
        [Description("Best Quality")]
        BestQuality,

        /// <summary>
        /// Maximum Compression
        /// </summary>
        [Description("Maximum Compression")]
        MaximumCompression,
    }
    #endregion

    #region Printer Properties
    /// <summary>
    /// Tray Duplex Enum
    /// </summary>
    public enum TrayDuplex
    {
        /// <summary>
        /// Not Installed
        /// </summary>
        [Description("Not Installed")]
        NotInstalled = 0,

        /// <summary>
        /// Installed
        /// </summary>
        [Description("Installed")]
        Installed,
    }

    /// <summary>
    /// Secure Printing Enum
    /// </summary>
    public enum SecurePrinting
    {
        /// <summary>
        /// Automatic
        /// </summary>
        [Description("<Automatic>")]
        Automatic = 0,

        /// <summary>
        /// Disable
        /// </summary>
        [Description("Disable")]
        Disable,
    }
    #endregion

    #region Printer Preference Action
    /// <summary>
    /// Printer Preference Action Enum
    /// </summary>
    public enum PrinterPreferenceAction
    {
        /// <summary>
        /// Off
        /// </summary>
        [Description("Off")]
        Off = 0,

        /// <summary>
        /// Proof and Hold
        /// </summary>
        [Description("Proof and Hold")]
        ProofandHold = 1,

        /// <summary>
        /// Personal Job
        /// </summary>
        [Description("Personal Job")]
        PersonalJob = 2,

        /// <summary>
        /// Quick Copy
        /// </summary>
        [Description("Quick Copy")]
        QuickCopy = 3,

        /// <summary>
        /// Stored Job
        /// </summary>
        [Description("Stored Job")]
        StoredJob = 4,

        /// <summary>
        /// User Name
        /// </summary>
        [Description("User Name")]
        UserName = 5,

        /// <summary>
        /// Custom
        /// </summary>
        [Description("Custom")]
        Custom = 6,

        /// <summary>
        /// Automatic
        /// </summary>
        [Description("Automatic")]
        Automatic = 7,

        /// <summary>
        /// Automatic
        /// </summary>
        [Description("Actual Size")]
        ActualSize = 8,

        /// <summary>
        /// Print document on
        /// </summary>
        [Description("Print document on")]
        PrintDocumentOn = 9,

        /// <summary>
        /// % on actual size
        /// </summary>
        [Description("% on actual size")]
        PercentageOnActualSize = 10
    }

    #endregion
}
