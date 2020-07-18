using System.Collections.Generic;

namespace HP.ScalableTest.Plugin.DeviceInspector.Classes
{
    internal static class ListValues
    {
        public static List<string> EnableScanToEmailValues = new List<string> { "enabled", "disabled" };

        //public List<string> _fromList = new List<string> { "Blank", "User's Address"}; //Blank, Users Address
        public static List<string> DefaultFromList = new List<string> { "Default From:", "User's Address" }; //Default From, User's Address

        public static List<string> DefaultToList = new List<string> { "Blank", "User's Address" }; //Blank, Users address (UseSignedInUserAsSender

        public static Dictionary<string, string> OriginalSize = new Dictionary<string, string> {
            {"Any Size", "any"},
            {"Letter (8.5x11)", "na_letter_8.5x11in"},
            {"Letter Rotated (8.5x11)", "na_letter-rot_8.5x11in"},
            {"Legal (8.5x14)", "na_legal_8.5x14in"},
            {"Executive (7.25x10.5)", "na_executive_7.25x10.5in"},
            {"Statement (5.5.8.5)", "na_invoice_5.5x8.5in"},
            {"Ledger (11.17)", "na_ledger_11x17in" },
            {"3x5", "na_index-3x5_3x5in" },
            {"4x6", "na_index-4x6_4x6in" },
            {"5x7", "na_5x7_5x7in"},
            {"5x8", "na_index-5x8_5x8in" },
            {"Oficio (8.5x13)", "na_eightpointfivebythirteen_8.5x13in"},
            {"Oficio (216.340 mm)", "na_legal_216x340mm"},
            {"A3 (297x420 mm)", "iso_a3_297x420mm"},
            {"A4 (210x297 mm)", "iso_a4_210x297mm" },
            {"A4 Rotated (210x297 mm)", "iso_a4-rot_210x297mm"},
            {"A5 (148x210 mm)", "iso_a5_148x210mm"},
            {"A6 (105x148 mm)", "iso_a6_105x148mm"},
            {"RA4 (215x305 mm)", "iso_ra4_215x305mm"},
            {"B4 (257x364 mm)", "iso_b4_182x364mm"},
            {"B5 (182x257 mm)", "iso_b5_182x257mm"},
            {"B6 (128x182 mm)", "iso_b6_125x176mm"},
            {"10x15cm", "iso_indexcard_100x150mm"},
            {"16K (195x270 mm)", "iso_16k_195x270mm"},
            {"16K (184x260 mm)", "iso_16k_184x260mm"},
            {"DPostcard JIS (148x200 mm)", "jpn_oufuku_148x200mm"},
            {"Mixed Letter/Legal", "letter_or_legal"},
            {"Mixed A4/A3", "a4_or_a3"},
            {"Mixed Letter/Ledger", "letter_or_ledger"},
            {"Long Scan Size", "na_longscansize_8.5x34in"}
    };

        public static Dictionary<string, string> OriginalSides = new Dictionary<string, string> { { "1-sided", "Simplex" }, { "2-sided", "Duplex" } };
        public static Dictionary<string, string> ImagePreview = new Dictionary<string, string> { { "Preview Optional", "previewOptional" }, { "Preview Required", "previewRequired" }, { "Preview Disabled", "previewDisabled" } };

        public static Dictionary<string, string> FileType = new Dictionary<string, string> {
            {"PDF","pdf"},
            {"JPEG","jpeg"},
            {"TIFF","tiff"},
            {"MTIFF", "mtiff" },
            {"SPX", "spx" },
            {"PDF/A (Archivable)", "pdfa" },
            {"Text (OCR)", "txt" },
            {"Unicode Text (OCR)", "txt" },
            {"RTF (OCR)", "rtf" },
            {"Searchable PDF (OCR)", "pdf" },
            {"Searchable PDF/A (OCR)", "pdfa" },
            {"HTML (OCR)", "html" },
            {"CSV (OCR)", "csv" }
    };

        //Get A List, Check it twice
        public static Dictionary<string, string> Resolution = new Dictionary<string, string>{ {"75 dpi", "75dpi"}, {"150 dpi", "150dpi"},{"200 dpi","200dpi"}, {"300 dpi","300dpi"},{"400 dpi","400dpi"},{"600 dpi","600dpi"}};

        public static List<string> QuickSetOptions = new List<string> { "Email", "Copy", "Folder", "SharePoint", "USB", "Fax", "FTP" };

        public static List<string> ContentOrientation = new List<string> { "Portrait", "Landscape" };

        public static Dictionary<string, string> SharpnessModes = new Dictionary<string, string>{{"1-(Soft Edges)", "0"},{"2","1"},{"3-(Normal)", "2"}, {"4", "3"}, {"5-(Sharper Edges)", "4"}};

        public static Dictionary<string, string> CleanupModes = new Dictionary<string, string> { { "1-(More Specks)", "0" }, { "2", "1" }, { "3-(Normal)", "2" }, { "4", "3" }, { "5", "4" }, { "6", "5" }, { "7", "6" }, { "8", "7" }, { "9-(Fewer Specks)", "8" } };

        public static Dictionary<string, string> DarknessModes = new Dictionary<string, string> { { "1-(Lighter)", "0" }, { "2", "1" }, { "3", "2" }, { "4", "3" }, { "5-(Normal)", "4" }, { "6", "5" }, { "7", "6" }, { "8", "7" }, { "9-(Darker)", "8" } };

        public static Dictionary<string, string> ContrastModes = new Dictionary<string, string> { { "1-(Less)", "0" }, { "2", "1" }, { "3", "2" }, { "4", "3" }, { "5-(Normal)", "4" }, { "6", "5" }, { "7", "6" }, { "8", "7" }, { "9-(More)", "8" } };

        public static List<string> EmailParameters = new List<string> { "To", "From User", "Default From", "Server" };

        #region CopyDefaultOptions

        public static Dictionary<string, string> ChromaticModes = new Dictionary<string, string> { { "Automatically Detect", "autoDetect" }, { "Color", "color" }, { "Black/Gray", "grayscale" }, {"Black", "monochrome" } };

        public static Dictionary<string, string> CaptureModes = new Dictionary<string, string> { { "Standard Document", "standardDocument" }, { "Book Mode", "bookMode" }, { "2-sided ID", "IDCardScan" } };

        public static Dictionary<string, string> OptimizeModes = new Dictionary<string, string> { { "Text", "Text" }, { "Mixed", "Mixed" }, { "Printed Picture", "Photo" }, { "Photograph", "Glossy" } };

        public static Dictionary<string, string> CollateModes = new Dictionary<string, string> { { "Collate On", "Collated" }, { "Collate Off", "Uncollated" } };

        public static Dictionary<string, string> CopySidesModes = new Dictionary<string, string>{{"1:1 sided", "simplexToSimplex" },{"1:2 sided", "simplexToDuplex" }, {"2:1 sided", "duplexToSimplex" }, {"2:2 sided", "duplexToDuplex" } };

        public static Dictionary<string, string> PagesPerSheetModes = new Dictionary<string, string>{{"One", "oneUp" }, {"Two", "twoUp" }, {"Four", "fourUp" } };

        #endregion CopyDefaultOptions

        #region PrintDefaultOptions

        public static Dictionary<string, string> PaperTrayModes = new Dictionary<string, string> { { "Automatically select", "disabled" }, { "Manual Feed", "enabled" } };

        public static Dictionary<string, string> OutputBin = new Dictionary<string, string> { { "Automatically Select", "Auto" }, { "Standard bin", "Face-down" }, { "Upper bin", "OutputBin1" }, { "Lower bin", "OutputBin2" } };

        public static Dictionary<string, string> OutputSides = new Dictionary<string, string> { { "1-sided", "Simplex" }, { "2-sided", "Duplex" } };

        public static Dictionary<string, string> PaperTypes = new Dictionary<string, string>
        {
            {"Any Type","AnySupportedType" },
            {"Plain","Plain"},
            {"HP EcoFFICIENT", "HPEcoSmartLite"},
            {"Light 60-74g", "Light"},
            {"Light Bond", "LightBond"},
            {"Bond","Bond"},
            {"Recycled","Recycled"},
            {"HP Matte 105g","HPMatte105gsm"},
            {"HP Matte 120g","HPMatte120Gsm"},
            {"HP Soft Gloss 120g","HPSoftGloss120gsm"},
            {"HP Glossy 120g","HPGlossy130gsm"},
            {"Mid-Weight 96-110g","MidWeight"},
            {"Heavy 111-130g","Heavy"},
            {"Mid-WtGlossy 96-110g","MidWeightGlossy"},
            {"Hvy Glossy 111-130g","HeavyGlossy"},
            {"HP Matte 150g","HPMatte160gsm"},
            {"HP Glossy 150g","HPGlossy160gsm"},
            {"Extra Heavy 131-175g","ExtraHeavy"},
            {"XHvyGlossy 131-175g","ExtraHeavyGloss"},
            {"HP Matte 200g","HPMatte200gsm"},
            {"HP Glossy 200g","HPGlossy220gsm"},
            {"Cardstock 176-220g","Cardstock"},
            {"Card Glossy 176-220g","CardstockGlossy"},
            {"Color Transparency","Transparency"},
            {"Labels","Labels"},
            {"StationeryLetterhead","Letterhead"},
            {"Envelope","Envelope"},
            {"Preprinted","StationeryPreprinted"},
            {"Prepunched","StationeryPrepunched"},
            {"Colored","Color"},
            {"Light Rough","LightRough"},
            {"Rough","Rough"},
            {"Opaque Film","FilmOpaque"}
        };

        public static Dictionary<string, string> PrintResolution = new Dictionary<string, string>
        {
            {"Image REt 3600", "600x600x8dpi"},
            {"1200 x 1200dpi", "1200X1200x8dpi"}
        };


        #endregion PrintDefaultOptions

        #region FolderOptions

        public static Dictionary<string, string> FolderAccessType = new Dictionary<string, string>
        {
            {"Folder with Read Write Access", "false"},
            {"Folder with Write Access only", "true"}
        };

        public static List<string> OnOffList = new List<string>{"on", "off"};

        public static Dictionary<string, string> CroppingOptions = new Dictionary<string, string>
        {
            {"Do not crop", "0"}, //AutoCrop and AutoPageCrop are off
            {"Crop to paper", "1"}, //AutoCrop is off and AutoPageCrop is on
            {"Crop to content", "2"} //AutoCrop is on and AutoPageCrop is off
        };

        public static List<string> FileSuffix = new List<string>
        {
            "",
            "%DEVICE_DATE_YYYYMMDD%",
            "%DEVICE_DATE_MMDDYYYY%",
            "%DEVICE_DATE_DDMMYYYY%",
            "%DEVICE_TIME_HHMMSS%",
            "%SECURITY_USERNAME%"
        };

        public static List<string> FilePrefix = new List<string>
        {
            "",
            "%DEVICE_DATE_YYYYMMDD%",
            "%DEVICE_DATE_MMDDYYYY%",
            "%DEVICE_DATE_DDMMYYYY%",
            "%DEVICE_TIME_HHMMSS%",
            "%SECURITY_USERNAME%"
        };

        public static Dictionary<string, string> FileSizeList = new Dictionary<string, string> { {"Low (small file)", "small"},{"Medium", "standard"}, {"High (large file)", "large"}};

        public static Dictionary<string, string> FileNumbering = new Dictionary<string, string>{{"_X-Y", "x_dash_y" }, {"_XX_of_YY", "xx_of_yy" }, {"_X_Y", "x_underscore_y" } };
        #endregion

        #region JobStorage
        public static Dictionary<string, string> MinLengthPinRequirement = new Dictionary<string, string>{{"enabled","4"}, {"disabled", "1"}};

        public static List<string> TrueFalseList = new List<string>{"true", "false"};

        public static Dictionary<string, string> JobRetentionDictionary = new Dictionary<string, string> { { "Do not retain", "DoNotRetain" }, { "Personal jobs only", "RetainPersonalJobs" }, { "All temporary jobs", "RetainAll" } };


        #endregion
        //public enum _enumValues { Email, Copy, Folder, SharePoint, USB, Fax };

        public static Dictionary<string, string> FaxResolutions = new Dictionary<string, string>
        {
            {"Standard (100 x 200dpi)", "standard" },
            {"Fine (200 x 200dpi", "fine" },
            {"Superfine (300 x 300dpi)", "superfine" }
        };

        public static Dictionary<string, string> FaxMethods = new Dictionary<string, string>
        {
            { "Lan Fax Service", "lanFaxService"},
            { "Internal Modem", "internalModem"}// Removed until we actually plug the rest of the internal modem settings.
        };

        public static Dictionary<string, string> ThirdPartyLanFax = new Dictionary<string, string>
        {
            { "Generic Lan Fax w/out Notification", "lanFaxWithoutNotificationSupport"},
            { "Generic Lan Fax with Notification", "lanFaxWithNotificationSupport"}
        };

        public static Dictionary<string, string> FaxFileFormat = new Dictionary<string, string>
        {
            { @"MTIFF/G4", "MTIFFG4"}
        };

    }
}