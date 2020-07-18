using System.Collections.Generic;
using System.ComponentModel;
using System;
using HP.ScalableTest.DeviceAutomation;

namespace HP.ScalableTest.Plugin.Copy
{
    /// <summary>
    /// Copy Preference Data. 
    /// Formerly CopyPreferencs and now renamed to CopyOptions as per the new request from Boise team- 9-Aug-2017
    /// </summary>
    //public class CopyPreferences
    public class CopyOptions
    {
        /// <summary>
        /// # of Copies
        /// </summary>
        public int Copies { get; set; }
        /// <summary>
        /// Collate Option
        /// </summary>
        public bool Collate { get; set; }

        /// <summary>
        /// Edge to Edge option
        /// </summary>
        public bool EdgeToEdge { get; set; }

        /// <summary>
        /// Orientation - Portrait or Landscape
        /// </summary>
        public ContentOrientation Orientation { get; set; }

        /// <summary>
        /// Sets the Colour option to Color, Monochrome, GrayScale, AutoDetect
        /// </summary>
        public string Color { get; set; }

        /// <summary>
        /// Sets the size for Reduce/Enlarge option
        /// </summary>
        public int ZoomSize { get; set; }

        /// <summary>
        /// Selects the automatic/Manual option
        /// </summary>
        public bool ReduceEnlargeOptions { get; set; }

        /// <summary>
        /// Selects include margin
        /// </summary>
        public bool IncludeMargin { get; set; }

        /// <summary>
        /// Selects Optimize Text or Picture Options
        /// </summary>
        public OptimizeTextPic OptimizeTextPicOptions { get; set; }

        /// <summary>
        /// Selects the Sides option
        /// </summary>
        public bool SetSides { get; set; }

        /// <summary>
        /// Selects side for Original
        /// </summary>
        public bool OriginalOneSided { get; set; }
        /// <summary>
        /// Selects side for Output
        /// </summary>
        public bool OutputOneSided { get; set; }

        /// <summary>
        /// Selects page flip for Original 
        /// </summary>
        public bool OriginalPageflip { get; set; }

        /// <summary>
        /// Selects page flip for Output 
        /// </summary>
        public bool OutputPageflip { get; set; }

        /// <summary>
        /// Selects the stamps option
        /// </summary>
        public bool setStamps { get; set; }

        /// <summary>
        /// Selects Stamp Contents for Top and bottom selections 
        /// </summary>
        public List<StampContents> StampContents { get; set; }

        /// <summary>
        /// Selects Scan modes 
        /// </summary>
        public ScanMode ScanModes { get; set; }

        /// <summary>
        /// Sets the booklet format
        /// </summary>
        public bool BookLetFormat { get; set; }

        /// <summary>
        /// Sets the border on page for booklet format
        /// </summary>
        public bool BorderOnEachPage { get; set; }

        /// <summary>
        /// Sets the text selected for Watermark 
        /// </summary>
        public string WatermarkText { get; set; }

        /// <summary>
        /// Sets to Erase edges object 
        /// </summary>
        public EraseEdges EraseEdgesValue;

        /// <summary>
        /// Selects the Erase Edges option
        /// </summary>
        public bool SetEraseEdges { get; set; }

        /// <summary>
        /// Selects the Pages Per Sheet option
        /// </summary>
        public bool SetPagesPerSheet { get; set; }

        /// <summary>
        /// Sets to Erase edges object 
        /// </summary>
        public PagesPerSheet PagesPerSheetElement { get; set; }

        public bool PagesPerSheetAddBorder { get; set; }

        /// <summary>
        /// Sets Original Size
        /// </summary>
        public OriginalSize OriginalSizeType { get; set; }

        /// <summary>
        /// Sets Paper selection paper size
        /// </summary>
        public PaperSelectionPaperSize PaperSelectionPaperSize { get; set; }

        /// <summary>
        /// Sets Paper selection paper type
        /// </summary>
        public PaperSelectionPaperType PaperSelectionPaperType { get; set; }

        /// <summary>
        /// Sets Paper selection paper tray
        /// </summary>
        public PaperSelectionPaperTray PaperSelectionPaperTray { get; set; }

        /// <summary>
        /// Selects the Image Adjustment options
        /// </summary>
        public bool SetImageAdjustment { get; set; }

        /// <summary>
        /// Sets Image adjustment for sharpness
        /// </summary>
        public int ImageAdjustSharpness { get; set; }

        /// <summary>
        /// Sets Image adjustment for darkness
        /// </summary>
        public int ImageAdjustDarkness { get; set; }

        /// <summary>
        /// Sets Image adjustment for contrast
        /// </summary>
        public int ImageAdjustContrast { get; set; }

        /// <summary>
        /// Sets Image adjustment for background cleanup
        /// </summary>
        public int ImageAdjustbackgroundCleanup { get; set; }

        /// <summary>
        /// Sets optimize text/picture type
        /// </summary>
        public OptimizeTextPic OptimizeTextPicture { get; set; }


        /// <summary>
        /// Apply same width for Front Edges
        /// </summary>
        public bool ApplySameWdith { get; set; }

        /// <summary>
        /// Back edges mirror front 
        /// </summary>
        public bool MirrorFrontSide { get; set; }

        /// <summary>
        /// If set to True use inches otherwise use (milimeter) mm
        /// </summary>
        public bool UseInches { get; set; }

        /// <summary>
        /// Creates new Copy Preferences Options
        /// </summary>
        ///
        public CopyOptions()
        {
            Collate = true;
            EdgeToEdge = false;
            Color = "Automatically detect";
            Copies = 1;
            OptimizeTextPicOptions = OptimizeTextPic.Photo;
            ReduceEnlargeOptions = true;
            OriginalOneSided = true;
            OutputOneSided = true;
            StampContents = new List<StampContents>();
            ScanModes = ScanMode.Standard;
            BookLetFormat = false;
            BorderOnEachPage = false;
            WatermarkText = string.Empty;
            EraseEdgesValue = new EraseEdges();
            PagesPerSheetElement = new PagesPerSheet();
            PagesPerSheetAddBorder = false;
            ImageAdjustSharpness = 0;
            ImageAdjustDarkness = 0;
            ImageAdjustContrast = 0;
            ImageAdjustbackgroundCleanup = 0;
            EraseEdgesValue.FrontTop = "0.00";
            EraseEdgesValue.FrontLeft = "0.00";
            EraseEdgesValue.FrontBottom = "0.00";
            EraseEdgesValue.FrontRight = "0.00";
            EraseEdgesValue.BackTop = "0.00";
            EraseEdgesValue.BackBottom = "0.00";
            EraseEdgesValue.BackLeft = "0.00";
            EraseEdgesValue.BackRight = "0.00";
            EraseEdgesValue.AllEdges = "0.00";
            UseInches = true;
            SetImageAdjustment = false;
            setStamps = false;
            SetEraseEdges = false;
            SetPagesPerSheet = false;
            SetSides = false;
            OriginalSizeType = OriginalSize.Letter;
        }

    /// <summary>
    /// Struct for Erase edges
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

        }
        #endregion
    }

    public class StampContents
    {
        public StampType StampType;
        public string StampContentType;
    }

}
