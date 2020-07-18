using System.Runtime.Serialization;
using HP.ScalableTest.Plugin.DriverConfigurationPrint.Enum;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Plugin.DriverConfigurationPrint
{
    /// <summary>
    /// Represents the data needed to execute an Printer Preferences Activity.
    /// </summary>
    [DataContract]
    public class PrinterPreferenceData
    {
        /// <summary>
        /// Gets or sets the size of the Paper.
        /// </summary>
        [DataMember]
        public string PaperSize { get; set; }

        /// <summary>
        /// Gets or sets the Custom paper size Text box entered value.
        /// </summary>
        [DataMember]
        public string CustomPaperSizeName { get; set; }

        /// <summary>
        /// Gets or sets the type of the Paper.
        /// </summary>
        [DataMember]
        public string PaperType { get; set; }

        /// <summary>
        /// Gets or sets the Special Pages 
        /// </summary>
        [DataMember]
        public SpecialPages SpecialPages { get; set; }

        /// <summary>
        /// Gets or sets the quality of the Paper.
        /// </summary>
        [DataMember]
        public PrintQuality PrintQuality { get; set; }

        /// <summary>
        /// Gets or sets the Paper Source.
        /// </summary>
        [DataMember]
        public string PaperSource { get; set; }

        /// <summary>
        /// Gets or sets the actual size Radio Button checked or unchecked value.
        /// </summary>
        [DataMember]
        public bool ActualSize { get; set; }

        /// <summary>
        /// Gets or sets the Print Document on Radio Button checked or unchecked value.
        /// </summary>
        [DataMember]
        public bool PrintDocumentOn { get; set; }

        /// <summary>
        /// Gets or sets the Print Document value selected from the drop down.
        /// </summary>
        [DataMember]
        public string PrintDocumentOnValue { get; set; }

        /// <summary>
        /// Gets or sets the scale to fit Checkbox button checked or unchecked value.
        /// </summary>
        [DataMember]
        public bool ScaleToFit { get; set; }

        /// <summary>
        /// Gets or sets the Percentage Actual Size Radio Button checked or unchecked value.
        /// </summary>
        [DataMember]
        public bool PerActualSize { get; set; }

        /// <summary>
        /// Gets or sets the Percentage Actual Size Number entered value.
        /// </summary>
        [DataMember]
        public string PerActualSizeNo { get; set; }

        /// <summary>
        /// Gets or sets the Water Mark value selected from the drop down.
        /// </summary>
        [DataMember]
        public string Watermark { get; set; }

        /// <summary>
        /// Gets or sets the custom Water Mark Name Text box entered value.
        /// </summary>
        [DataMember]
        public string CustomWaterMarkName { get; set; }

        /// <summary>
        /// Gets or sets the First Page Only Checkbox button checked or unchecked value.
        /// </summary>
        [DataMember]
        public bool FirstPageOnly { get; set; }

        /// <summary>
        /// Gets or sets the Print On Both Sides Checkbox button checked or unchecked value.
        /// </summary>
        [DataMember]
        public bool PrintOnBothSides { get; set; }

        /// <summary>
        /// Gets or sets the Flip Pages Up Checkbox button checked or unchecked value.
        /// </summary>
        [DataMember]
        public bool FlipPagesUp { get; set; }

        /// <summary>
        /// Gets or sets the Booklet Layout value selected from the drop down.
        /// </summary>
        [DataMember]
        public string BookletLayout { get; set; }

        /// <summary>
        /// Gets or sets the Pages Per Sheet value selected from the drop down.
        /// </summary>
        [DataMember]
        public PagesPerSheet PagesPerSheet { get; set; }

        /// <summary>
        /// Gets or sets the Print Pages Borders Checkbox button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool PrintPageBorders { get; set; }

        /// <summary>
        /// Gets or sets the Pages Order value selected from the drop down.
        /// </summary>
        [DataMember]
        public PageOrder PageOrder { get; set; }

        /// <summary>
        /// Gets or sets the Orientation Portrait Radio button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool OrientationPortrait { get; set; }

        /// <summary>
        /// Gets or sets the Orientation Lanscape Radio button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool OrientationLanscape { get; set; }

        /// <summary>
        /// Gets or sets the Rotate By 180 Degree Checkbox button  checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool RotateBy180Degree { get; set; }

        /// <summary>
        /// Gets or sets the Output Bin value selected from the drop down.
        /// </summary>
        [DataMember]
        public string OutputBin { get; set; }

        /// <summary>
        /// Gets or sets the Staple value selected from the drop down
        /// </summary>
        [DataMember]
        public Staple Staple { get; set; }

        /// <summary>
        /// Gets or sets the Punch value selected from the drop down
        /// </summary>
        [DataMember]
        public Punch Punch { get; set; }

        /// <summary>
        /// Gets or Sets the Fold value selected from the drop down
        /// </summary>
        [DataMember]
        public Fold Fold { get; set; }

        /// <summary>
        /// Gets or sets the MaxSheetPerSet textbox entered value 
        /// </summary>
        [DataMember]
        public string MaxSheetPerSet { get; set; }

        /// <summary>
        /// Gets or sets the Off Radio button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool JobStorageOff { get; set; }

        /// <summary>
        /// Gets or sets the Proof and Hold Radio button  checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool ProofandHold { get; set; }

        /// <summary>
        /// Gets or sets the Personal Job Radio button  checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool PersonalJob { get; set; }

        /// <summary>
        /// Gets or sets the Quick Job Radio button  checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool QuickJob { get; set; }

        /// <summary>
        /// Gets or sets the Stored Job Radio button  checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool StoredJob { get; set; }

        /// <summary>
        /// Gets or sets the Make Job Private Secure value selected from the drop down.
        /// </summary>
        [DataMember]
        public string MakeJobPrivateSecure { get; set; }

        /// <summary>
        /// Gets or sets the User Name Radio button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool UserName { get; set; }

        /// <summary>
        /// Gets or sets the Custom Radio button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool Custom { get; set; }

        /// <summary>
        /// Gets or sets the Custom Text box entered value.
        /// </summary>
        [DataMember]
        public string CustomText { get; set; }

        /// <summary>
        /// Gets or sets the Job Name Automatic Radio Button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool JobNameAutomatic { get; set; }

        /// <summary>
        /// Gets or sets the Job Name Custom Radio Button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool JobNameCustom { get; set; }

        /// <summary>
        /// Gets or sets the Job Name Custom Text box entered value.
        /// </summary>
        [DataMember]
        public string JobNameCustomText { get; set; }

        /// <summary>
        /// Gets or sets the Job Name Exists value selected from the drop down.
        /// </summary>
        [DataMember]
        public string JobNameExists { get; set; }

        /// <summary>
        /// Gets or sets the number of Copies.
        /// </summary>
        [DataMember]
        public int Copies { get; set; }

        /// <summary>
        /// Gets or sets the Collate Checkbox Button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool Collate { get; set; }

        /// <summary>
        /// Gets or sets the Reverse Page Order Checkbox Button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool ReversePageOrder { get; set; }

        /// <summary>
        /// Gets or sets the Print Text as Black Checkbox Button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool PrintTextAsBlack { get; set; }

        /// <summary>
        /// Gets or sets the HP Easy Color Checkbox Button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool HPEasyColor { get; set; }

        /// <summary>
        /// Gets or sets the Edge To Edge Checkbox Button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool EdgeToEdge { get; set; }

        /// <summary>
        /// Gets or sets the Raster Compression value selected from the drop down.
        /// </summary>
        [DataMember]
        public RasterCompression RasterCompression { get; set; }

        /// <summary>
        /// Gets or sets the Pin Number.
        /// </summary>
        [DataMember]
        public string PinNumber { get; set; }

        /// <summary>
        /// Gets or sets the Password.
        /// </summary>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Confirm Password.
        /// </summary>
        [DataMember]
        public string ConfirmPassword { get; set; }

        /// <summary>
        /// Gets or sets the Save As Default Password Checkbox Button checked or unchecked bool value.
        /// </summary>
        [DataMember]
        public bool SaveAsDefaultPass { get; set; }

        /// <summary>
        /// Gets or sets the PrintInGrayScaleText value selected from the drop down.
        /// </summary>
        [DataMember]
        public GrayScale PrintInGrayScaleText { get; set; }

        /// <summary>
        /// Gets or sets the RGB Color value selected from the drop down
        /// </summary>
        [DataMember]
        public RGBColor RGBColor { get; set; }

        /// <summary>
        /// Gets or sets the Enable Custom Name paper size Checkbox Button checked or unchecked bool value
        /// </summary>
        [DataMember]
        public bool EnableCustomPaperSize { get; set; }

        /// <summary>
        /// Gets or sets the Enable Custom Name water mark Checkbox Button checked or unchecked bool value
        /// </summary>
        [DataMember]
        public bool EnableCustomWaterMark { get; set; }

        public PrinterPreferenceData()
        {
            PaperSize = Enum.PaperSize.Letter.ToString();
            PaperType = Enum.PaperType.Unspecified.ToString();
            PrintQuality = PrintQuality.Normal;
            PaperSource = EnumUtil.GetDescription(Enum.PaperSource.PrinterAutoSelect);
            SpecialPages = SpecialPages.FrontCoverNoCovers;
            ActualSize = true;
            PrintDocumentOn = false;
            PrintDocumentOnValue = Enum.PaperSize.Letter.ToString();
            ScaleToFit = true;
            PerActualSize = false;
            PerActualSizeNo = "100";
            Watermark = EnumUtil.GetDescription(WaterMark.none);
            FirstPageOnly = false;
            PrintOnBothSides = true;
            FlipPagesUp = false;
            BookletLayout = Enum.BookletLayout.Off.ToString();
            PagesPerSheet = PagesPerSheet.A1pagepersheet;
            PrintPageBorders = false;
            PageOrder = PageOrder.RightthenDown;
            OrientationPortrait = true;
            OrientationLanscape = false;
            RotateBy180Degree = false;
            Staple = Staple.None;
            Punch = Punch.None;
            Fold = Fold.None;
            MaxSheetPerSet = "1";
            OutputBin = EnumUtil.GetDescription(Enum.OutputBin.AutomaticallySelect);
            JobStorageOff = true;
            ProofandHold = false;
            PersonalJob = false;
            QuickJob = false;
            StoredJob = false;
            MakeJobPrivateSecure = EnumUtil.GetDescription(Enum.MakeJobPrivateSecure.None);
            UserName = true;
            Custom = false;
            CustomText = "Administrator";
            JobNameAutomatic = true;
            JobNameCustom = false;
            JobNameCustomText = "<Automatic>";
            JobNameExists = EnumUtil.GetDescription(IfJobNameExists.UseJobName199);
            Copies = 1;
            Collate = true;
            ReversePageOrder = false;
            PrintTextAsBlack = false;
            PrintInGrayScaleText = GrayScale.off;
            RGBColor = RGBColor.None;
            HPEasyColor = true;
            EdgeToEdge = false;
            RasterCompression = RasterCompression.Automatic;
            PinNumber = string.Empty;
            Password = string.Empty;
            ConfirmPassword = string.Empty;
            SaveAsDefaultPass = false;
            EnableCustomPaperSize = false;
            EnableCustomWaterMark = false;
        }
    }
}
