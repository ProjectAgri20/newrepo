using System.Collections.Generic;
namespace HP.ScalableTest.DeviceAutomation.NativeApps.Copy
{
    /// <summary>
    /// Manages advanced job options for an <see cref="ICopyApp" />.
    /// </summary>
    public interface ICopyJobOptions
    {
        /// <summary>
        /// Selects the color for printing.
        /// </summary>
        /// <param name="color">The color to select </param>
        void SetColor(string color);

        /// <summary>
        /// Sets the number of copies to print
        /// </summary>
        /// <param name="copies">No of copies to be set</param>
        void SetNumCopies(int copies);

        /// <summary>
        /// Sets the state of the Job Build option.
        /// </summary>
        /// <param name="enable">if set to <c>true</c> enable Job Build; otherwise, disable it.</param>
        void SetJobBuildState(bool enable);

        /// <summary>
        /// Sets the Orientation - Portrait or Landscape
        /// </summary>
        /// <param name="orientation">Orientation type set Potrait or Landscape </param>
        void SetOrientation(ContentOrientation orientation);

        /// <summary>
        /// Sets the output to either Normal or EdgeToEdge
        /// </summary>
        /// <param name="edgetoedge">if set to <c>true</c> set Edge to edge , otherwise disable it</param>
        void SetEdgeToEdge(bool edgetoedge);

        /// <summary>
        /// Sets the collating pages On or Off
        /// </summary>
        /// <param name="collate">if set to <c>true</c> enable Collate; otherwise disable it</param>
        void SetCollate(bool collate);

        /// <summary>
        /// Selects the Optimize text or pitcure  for the scanned file
        /// </summary>
        /// <param name="optimizeTextOrPicture">Selects the text or pitcure</param>
        void SelectOptimizeTextOrPicture(OptimizeTextPic optimizeTextOrPicture);

        /// <summary>
        /// Performs Copy of Photo with either enlarge or reduction
        /// </summary>
        /// <param name="reduceEnlargeOption">if set to <c>true</c> Manual is set ; otherwise Automatic is set</param>
        /// <param name="includeMargin">It set to <c>true</c> enable Include margin; otherwise it is disabled</param>
        /// <param name="zoomSize">Value of the Zoom size, range between 25-100%</param>
        void SetReduceEnlarge(bool reduceEnlargeOption, bool includeMargin, int zoomSize);

        /// <summary>
        /// Sets the sides for Original and Ouput sides 
        /// </summary>
        /// <param name="originalOnesided">if set to <c>true</c> Orignal side is set to 1-sided ; otherwise 2-sided</param>
        /// <param name="outputOnesided">if set to <c>true</c> Output side is set to 1-sided ; otherwise 2-sided</param>
        /// <param name="originalPageFlipUp">If set to <c>true</c> Page flip is enabled; otherwise it is disabled</param>
        /// <param name="outputPageFlipUp">If set to <c>true</c> Output page flip is enabled ; otherwise it is disabled</param>
        void SetSides(bool originalOnesided, bool outputOnesided, bool originalPageFlipUp, bool outputPageFlipUp);

        /// <summary>
        /// Set the Stamps for Top and Bottom 
        /// </summary>
        /// <param name="stampList">List containing Key-value Pair for Stamps to be set;Key is the StampType Enum and Value contains string to be set for the Stamptype</param>                
        void SetStamps(Dictionary<StampType, string> stampList);

        /// <summary>
        /// Set the Scan mode
        /// </summary>
        /// <param name="scanModeSelected">Sets the selected scan mode specified in the ScanMode enum</param>
        void SetScanMode(ScanMode scanModeSelected);

        /// <summary>
        /// Set the booklet format to On-Off
        /// </summary>
        /// <param name="bookletformat">If set to <c>true</c> booklet format is enabaled; otherwise its disabled</param>
        /// <param name="borderOnPage">If set to <c>true</c> border on page is set to true; else it is false</param>                                     
        void SetBooklet(bool bookletformat, bool borderOnPage);

        /// <summary>
        /// Set text for Watermark
        /// </summary>
        /// <param name="watermarkText">Text to be set for watermark</param>
        void SetWaterMark(string watermarkText);

        /// <summary>
        /// Set the Frrnt and back erase edge list for Top, Bottom, Left and Right
        /// </summary>
        /// <param name="eraseEdgeList">List cotaining the Key-value pair for the edge. Key, is the erase edge type enum and value is the string to be set for the element. Edges value must be a non empty, mumeric and a non zero string. Zeros will be ignored</param>
        /// <param name="applySameWidth">When set to <c>true</c> it applies same width for all edges for front; otherwise individual widths are applied</param>
        /// <param name="mirrorFrontSide">When set to <c>true</c>, back side will mirror the front side </param>
        /// <param name="useInches">If set to <c>true</c> inches will be used otherwise edges are set in milimeter </param>
        void SetEraseEdges(Dictionary<EraseEdgesType, decimal> eraseEdgeList, bool applySameWidth, bool mirrorFrontSide, bool useInches);

        /// <summary>
        /// Set the Pages per Sheet 
        /// </summary>
        /// <param name="pages">Set the pages per sheet for the document using the PagesPerSheet enum </param>
        /// <param name="addPageBorders">If set to <c>true</c> Page borders are added ; otherwise page borders are not added </param>
        void SetPagesPerSheet(PagesPerSheet pages, bool addPageBorders);

        /// <summary>
        /// Sets the Original size of paper
        /// </summary>
        /// <param name="sizeType">Set the paper size for original Size using Original Size Enum</param>
        void SetOriginalSize(OriginalSize sizeType);

        /// <summary>
        /// Set Paper selection for Size, type and tray 
        /// </summary>
        /// <param name="paperSize">Sets the paper size </param>
        /// <param name="paperType">Sets the paper type</param>
        /// <param name="paperTray">Sets the paper tray to use </param>
        void SetPaperSelection(PaperSelectionPaperSize paperSize, PaperSelectionPaperType paperType, PaperSelectionPaperTray paperTray);

        /// <summary>
        /// Set image adjustment for sharpness, darkness, contrast and Background cleanup
        /// </summary>
        /// <param name="imageAdjustSharpness">Sets the value for Image adjustment sharpness </param>
        /// <param name="imageAdjustDarkness">Sets the value for Image adjustment darkness </param>
        /// <param name="imageAdjustContrast">Sets the value for Image adjustment contrast </param>
        /// <param name="imageAdjustBackgroundCleanup">Sets the value for Image adjustment background cleanup </param>
        void SetImageAdjustments(int imageAdjustSharpness, int imageAdjustDarkness, int imageAdjustContrast, int imageAdjustBackgroundCleanup);
    }
}