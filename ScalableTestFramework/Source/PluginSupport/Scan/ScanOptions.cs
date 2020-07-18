using System;
using System.ComponentModel;
using System.Runtime.Serialization;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.PluginSupport.Scan
{
    /// <summary>
    /// Configuration data used by a <see cref="ScanActivityManager" />.
    /// </summary>
    [DataContract]
    public class ScanOptions
    {

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        [DataMember]
        public int PageCount { get; set; }

        /// <summary>
        /// Gets or sets the lock timeouts.
        /// </summary>
        [DataMember]
        public LockTimeoutData LockTimeouts { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the ADF for simulators.
        /// </summary>
        [DataMember]
        public bool UseAdf { get; set; }

        /// <summary>
        /// Different resolution types
        /// </summary>
        [DataMember]
        public ResolutionType ResolutionType { get; set; }

        /// <summary>
        /// Different File Type
        /// </summary>
        [DataMember]
        public FileType FileType { get; set; }

        /// <summary>
        /// Original Sides types
        /// </summary>
        [DataMember]
        public OriginalSides OriginalSides { get; set; }

        /// <summary>
        /// Color types
        /// </summary>
        [DataMember]
        public ColorType Color { get; set; }

        /// <summary>
        /// Original Size
        /// </summary>
        [DataMember]
        public OriginalSize OriginalSize { get; set; }

        /// <summary>
        /// ContentOrientation
        /// </summary>
        [DataMember]
        public ContentOrientation ContentOrientationOption { get; set; }

        /// <summary>
        /// OptimizeTextorpic
        /// </summary>
        [DataMember]
        public OptimizeTextPic OptimizeTextorPic { get; set; }

        /// <summary>
        /// Cropping
        /// </summary>
        [DataMember]
        public Cropping Cropping { get; set; }

        /// <summary>
        /// BlankPageSupress
        /// </summary>
        [DataMember]
        public BlankPageSupress BlankPageSupressoption { get; set; }

        /// <summary>
        /// Notification
        /// </summary>
        [DataMember]
        public NotifyCondition notificationCondition { get; set; }

        /// <summary>
        /// Selects Scan modes 
        /// </summary>
        [DataMember]
        public ScanMode ScanModes { get; set; }

        /// <summary>
        /// Sets the booklet format
        /// </summary>
        [DataMember]
        public bool BookLetFormat { get; set; }

        /// <summary>
        /// Sets the border on page for booklet format
        /// </summary>
        [DataMember]
        public bool BorderOnEachPage { get; set; }

        /// <summary>
        /// notificationConditionType
        /// </summary>
        [DataMember]
        public string notificationConditionType { get; set; }

        /// <summary>
        /// Selects include pageflipup
        /// </summary>
        [DataMember]
        public bool PageFlipup { get; set; }

        /// <summary>
        /// Selects multipleFile option
        /// </summary>
        [DataMember]
        public bool CreateMultiFile { get; set; }

        /// <summary>
        /// MaxPagePerFile 
        /// </summary>
        [DataMember]
        public string MaxPageperFile { get; set; }

        /// <summary>
        /// Print /Email notification method
        /// </summary>
        [DataMember]
        public string PrintorEmailNotificationMethod { get; set; }

        /// <summary>
        /// Email notification text
        /// </summary>
        [DataMember]
        public string EmailNotificationText { get; set; }

        /// <summary>
        /// Sets to Erase edges object 
        /// </summary>
        [DataMember]
        public EraseEdges EraseEdgesValue;

        /// <summary>
        /// Selects the Eragseedges options
        /// </summary>
        [DataMember]
        public bool SetEraseEdges { get; set; }

        /// <summary>
        /// Selects the Imageadjustment options
        /// </summary>
        [DataMember]
        public bool SetImageAdjustment { get; set; }

        /// <summary>
        /// Image adjustment sharpness type
        /// </summary>
        [DataMember]
        public int ImageAdjustSharpness { get; set; }

        /// <summary>
        /// Image adjustment darkness type
        /// </summary>
        [DataMember]
        public int ImageAdjustDarkness { get; set; }

        /// <summary>
        /// Image adjustment contrast type
        /// </summary>
        [DataMember]
        public int ImageAdjustContrast { get; set; }

        /// <summary>
        /// Image adjustment background cleanup type
        /// </summary>
        [DataMember]
        public int ImageAdjustbackgroundCleanup { get; set; }

        /// <summary>
        /// Sign or Encrypt type
        /// </summary>
        [DataMember]
        public int SignOrEncrypt { get; set; }

        /// <summary>
        /// ScanJobType specifies the type of scanjob. Which is either scan to usb,scan to email,scan to folder
        /// </summary>
        [DataMember]
        public string ScanJobType { get; set; }

        /// <summary>
        /// apply same width for all edges
        /// </summary>
        [DataMember]
        public bool ApplySameWidth { get; set; }

        /// <summary>
        /// Mirror the front edges values to back edges
        /// </summary>
        [DataMember]
        public bool MirrorFrontSide { get; set; }

        /// <summary>
        /// Use inches settings
        /// </summary>
        [DataMember]
        public bool UseInches { get; set; }

        /// <summary>
        /// Include thumb nail for notification job
        /// </summary>
        [DataMember]
        public bool IncludeThumbNail { get; set; }

        /// <summary>
        /// Selects the Sides options
        /// </summary>
        [DataMember]
        public bool SetSides { get; set; }

        /// <summary>
        /// Selects side for Original
        /// </summary>
        [DataMember]
        public bool OriginalOneSided { get; set; }
        /// <summary>
        /// Selects side for Output
        /// </summary>
        [DataMember]
        public bool OutputOneSided { get; set; }

        /// <summary>
        /// Selects page flip for Original 
        /// </summary>
        [DataMember]
        public bool OriginalPageflip { get; set; }

        /// <summary>
        /// Selects page flip for Output 
        /// </summary>
        [DataMember]
        public bool OutputPageflip { get; set; }

        /// <summary>
        /// Collate Option
        /// </summary>
        [DataMember]
        public bool Collate { get; set; }

        /// <summary>
        /// Edge to Edge option
        /// </summary>
        [DataMember]
        public bool EdgeToEdge { get; set; }

        /// <summary>
        /// Sets the size for Reduce/Enlarge option
        /// </summary>
        [DataMember]
        public int ZoomSize { get; set; }

        /// <summary>
        /// Selects the automatic/Manual option
        /// </summary>
        [DataMember]
        public bool ReduceEnlargeOptions { get; set; }

        /// <summary>
        /// Selects include margin
        /// </summary>
        [DataMember]
        public bool IncludeMargin { get; set; }

        /// <summary>
        /// Paper selection paper size setting
        /// </summary>
        [DataMember]
        public PaperSelectionPaperSize PaperSelectionPaperSize { get; set; }

        /// <summary>
        /// Paper selection paper type setting
        /// </summary>
        [DataMember]
        public PaperSelectionPaperType PaperSelectionPaperType { get; set; }

        /// <summary>
        /// Paper selection paper tray setting
        /// </summary>
        [DataMember]
        public PaperSelectionPaperTray PaperSelectionPaperTray { get; set; }

        /// <summary>
        /// Selects the Pages Per Sheet Option
        /// </summary>
        [DataMember]
        public bool SetPagesPerSheet { get; set; }

        /// <summary>
        /// Pages per sheet element setting
        /// </summary>
        [DataMember]
        public PagesPerSheet PagesPerSheetElement { get; set; }

        /// <summary>
        /// Pages per sheet border setting
        /// </summary>
        [DataMember]
        public bool PagesPerSheetAddBorder { get; set; }

        /// <summary>
        /// Automatic tone setting
        /// </summary>
        [DataMember]
        public bool AutomaticTone { get; set; }

        /// <summary>
        /// Creates new Scan Options
        /// </summary>
        public ScanOptions()
        {
            ResolutionType = ResolutionType.None;
            FileType = FileType.DeviceDefault;
            Color = ColorType.None;
            OriginalSize = OriginalSize.None;
            OriginalSides = OriginalSides.None;
            ContentOrientationOption = ContentOrientation.None;
            OptimizeTextorPic = OptimizeTextPic.None;
            Cropping = Cropping.None;
            BlankPageSupressoption = BlankPageSupress.None;
            notificationCondition = NotifyCondition.NeverNotify;
            CreateMultiFile = false;
            EraseEdgesValue = new EraseEdges { AllEdges = "0.00", BackBottom = "0.00", BackLeft = "0.00", BackRight = "0.00", BackTop = "0.00", FrontBottom = "0.00", FrontLeft = "0.00", FrontRight = "0.00", FrontTop = "0.00" };
            UseInches = true;
            ImageAdjustSharpness = 2;
            ImageAdjustDarkness = 4;
            ImageAdjustContrast = 4;
            ImageAdjustbackgroundCleanup = 2;
            SignOrEncrypt = -1;
            ScanModes = ScanMode.Standard;
            PagesPerSheetElement = new PagesPerSheet();
            PagesPerSheetAddBorder = false;
            ScanModes = ScanMode.Standard;
            BookLetFormat = false;
            BorderOnEachPage = false;
            ReduceEnlargeOptions = true;
            OriginalOneSided = true;
            OutputOneSided = true;
            Collate = true;
            EdgeToEdge = false;
            LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5));
            PageCount = 1;
            UseAdf = false;
            AutomaticTone = false;
            SetSides = false;
            SetPagesPerSheet = false;
        }

        [OnDeserialized]
        private void SetValuesOnDeserialized(StreamingContext context)
        {
            if (LockTimeouts == null)
            {
                LockTimeouts = new LockTimeoutData(TimeSpan.FromMinutes(2), TimeSpan.FromMinutes(5));
            }
            if (PageCount <= 0)
            {
                PageCount = 1;
            }
        }
    }

    /// <summary>
    /// Sets the EraseEdges job optons
    /// </summary>
    public struct EraseEdges
    {
        #region Front Edges
        /// <summary>
        /// Front top edge
        /// </summary>
        [Description("front-top")]
        public string FrontTop { get; set; }

        /// <summary>
        /// Front bottom edge
        /// </summary>
        [Description("front-bottom")]
        public string FrontBottom { get; set; }

        /// <summary>
        /// Front left edge
        /// </summary>
        [Description("front-left")]
        public string FrontLeft { get; set; }

        /// <summary>
        /// Front right edge
        /// </summary>
        [Description("front-right")]
        public string FrontRight { get; set; }


        #endregion

        #region back edges
        /// <summary>
        /// Back top edge
        /// </summary>
        [Description("back-top")]
        public string BackTop { get; set; }

        /// <summary>
        /// Back bottom edge
        /// </summary>
        [Description("back-bottom")]
        public string BackBottom { get; set; }

        /// <summary>
        /// Back left edge
        /// </summary>
        [Description("back-left")]
        public string BackLeft { get; set; }

        /// <summary>
        /// Back right edge
        /// </summary>
        [Description("back-right")]
        public string BackRight { get; set; }

        /// <summary>
        /// All edges same width
        /// </summary>
        [Description("All Edges")]
        public string AllEdges { get; set; }
        #endregion
    }
}
