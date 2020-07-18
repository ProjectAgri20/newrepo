using System;
using HP.DeviceAutomation.Phoenix;
using HP.ScalableTest.DeviceAutomation.Helpers.PhoenixNova;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Copy
{
    internal class PhoenixNovaCopyJobOptionsManager : PhoenixNovaJobOptionsManager, ICopyJobOptions
    {

        public PhoenixNovaCopyJobOptionsManager(PhoenixNovaDevice device)
            : base(device)
        {
        }
        /// <summary>
        /// Sets the Orientation - Portrait or Landscape
        /// </summary>
        /// <param name="orientation">Orientation type set Potrait or Landscape </param>
        public void SetOrientation(ContentOrientation orientation)
        {
            throw new NotImplementedException("Set Orientation job not implemented");
        }
        /// <summary>
        /// Sets the output to either Normal or EdgeToEdge
        /// </summary>
        /// <param name="edgetoedge">if set to <c>true</c> set Edge to edge , otherwise disable it</param>
        public void SetEdgeToEdge(bool edgetoedge)
        {
            throw new NotImplementedException("SetEdgeToEdge job not implemented");
        }
        /// <summary>
        /// Sets the collating pages On or Off
        /// </summary>
        /// <param name="collate">if set to <c>true</c> enable Collate; otherwise disable it</param>
        public void SetCollate(bool collate)
        {
            throw new NotImplementedException("Set collate job not implemented");
        }
        /// <summary>
        /// Performs Copy of Photo with Enlarge/reduction option
        /// </summary>
        /// <param name="reduceEnlargeOption">if set to <c>true</c> Manual is set ; otherwise Automatic is set</param>
        /// <param name="includeMargin">It set to <c>true</c> enable Include margin; otherwise it is disabled</param>
        /// <param name="zoomSize">Value of the Zoom size, range between 25-100%</param>
        public void SetReduceEnlarge(bool reduceEnlargeOption, bool includeMargin, int zoomSize)
        {
            //TODO
            throw new NotImplementedException("Perform Photo copy job not implemented");
        }
        /// <summary>
        /// Sets the sides for Original and Ouput sides 
        /// </summary>
        /// <param name="originalOnesided">if set to <c>true</c> Orignal side is set to 1-sided ; otherwise 2-sided</param>
        /// <param name="outputOnesided">if set to <c>true</c> Output side is set to 1-sided ; otherwise 2-sided</param>
        /// <param name="originalPageFlipUp">If set to <c>true</c> Page flip is enabled; otherwise it is disabled</param>
        /// <param name="outputPageFlipUp">If set to <c>true</c> Output page flip is enabled ; otherwise it is disabled</param>
        public void SetSides(bool originalOnesided, bool outputOnesided, bool originalPageFlipUp, bool outputPageFlipUp)
        {
            throw new NotImplementedException("Set Sides job not implemented");
        }
        /// <summary>
        /// Set the Stamps for Top and Bottom 
        /// </summary>
        /// <param name="stampList">List containing Key-value Pair for Stamps to be set;Key is the StampType Enum and Value contains string to be set for the Stamptype</param>                
        public void SetStamps(Dictionary<StampType, string> stampList)
        {
            throw new NotImplementedException("Set Stamps job not implemented");
        }
        /// <summary>
        /// Set the Scan mode
        /// </summary>
        /// <param name="scanModeSelected">Sets the selected scan mode specified in the ScanMode enum</param>
        public void SetScanMode(ScanMode scanModeSelected)
        {
            throw new NotImplementedException("Set Scan mode job not implemented");
        }
        /// <summary>
        /// Set the booklet format to On-Off
        /// </summary>
        /// <param name="bookletformat">If set to <c>true</c> booklet format is enabaled; otherwise its disabled</param>
        /// <param name="borderOnPage">If set to <c>true</c> border on page is set to true; else it is false</param>  
        public void SetBooklet(bool bookletformat, bool borderOnPage)
        {
            throw new NotImplementedException("Set Booklet job not implemented");
        }
        /// <summary>
        /// Set text for Watermark
        /// </summary>
        /// <param name="watermarkText">Text to be set for watermark</param>
        public void SetWaterMark(string watermarkText)
        {
            throw new NotImplementedException("Set Watermark job not implemented");
        }
        /// <summary>
        /// Set the Frrnt and back erase edge list for Top, Bottom, Left and Right
        /// </summary>
        /// <param name="eraseEdgeList">List cotaining the Key-value pair for the edge. Key, is the erase edge type enum and value is the string to be set for the element. Edges value must be a non empty, mumeric and a non zero string. Zeros will be ignored</param>
        /// <param name="applySameWidth">When set to <c>true</c> it applies same width for all edges for front; otherwise individual widths are applied</param>
        /// <param name="mirrorFrontSide">When set to <c>true</c>, back side will mirror the front side </param>
        /// <param name="useInches">If set to <c>true</c> inches will be used otherwise mm is used </param>
        public void SetEraseEdges(Dictionary<EraseEdgesType, decimal> eraseEdgeList, bool applySameWidth, bool mirrorFrontSide, bool useInches)
        {
            throw new NotImplementedException("Set Erase edges job not implemented");
        }
        /// <summary>
        /// Set the Pages per Sheet
        /// </summary>
        /// <param name="pages">Set the pages per sheet for the document using the PagesPerSheet enum </param>
        /// <param name="addPageBorders">If set to <c>true</c> Page borders are added ; otherwise page borders are not added </param>
        public void SetPagesPerSheet(PagesPerSheet pages, bool addPageBorders)
        {
            throw new NotImplementedException("Set Pages per sheet job not implemented");
        }
        /// <summary>
        /// Sets the Original size of paper
        /// </summary>
        /// <param name="sizeType">Set the paper size for original Size using Original Size Enum</param>
        public void SetOriginalSize(OriginalSize sizeType)
        {
            throw new NotImplementedException("Set original size job not implemented");
        }
        /// <summary>
        /// Set Paper selection for Size, type and tray 
        /// </summary>
        /// <param name="paperSize">Sets the paper size using PaperSelectionPaperSize enum</param>
        /// <param name="paperType">Sets the paper type using PaperSelectionPaperType enum</param>
        /// <param name="paperTray">Sets the paper tray to use; PaperSelectionPaperTray enum </param>
        public void SetPaperSelection(PaperSelectionPaperSize paperSize, PaperSelectionPaperType paperType, PaperSelectionPaperTray paperTray)
        {
            throw new NotImplementedException("Set Paper selection job not implemented");
        }
    }
}
