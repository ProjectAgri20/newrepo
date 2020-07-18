using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation.Helpers.JediWindjammer;
using System;
using System.Globalization;
using System.Linq;
using System.Collections.Generic;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.Copy
{
    internal class JediWindjammerCopyJobOptionsManager : JediWindjammerJobOptionsManager, ICopyJobOptions
    {
        private readonly JediWindjammerControlPanel _controlPanel;
        private readonly TimeSpan _waitTimeSpan = TimeSpan.FromSeconds(2);

        public JediWindjammerCopyJobOptionsManager(JediWindjammerDevice device)
            : base(device, "CopyAppMainForm")
        {
            _controlPanel = device.ControlPanel;
        }
        /// <summary>
        /// Sets the Orientation - Portrait or Landscape
        /// </summary>
        /// <param name="orientation">Orientation type set Potrait or Landscape </param>
        public void SetOrientation(ContentOrientation orientation)
        {
            ScrollToOption("CopyContentOrientationDialogButton");
            _controlPanel.PressWait("CopyContentOrientationDialogButton", "ContentOrientationDialog", _waitTimeSpan);
            _controlPanel.Press(orientation.ToString().ToLower(CultureInfo.InvariantCulture));
            _controlPanel.PressWait("m_OKButton", "CopyAppMainForm", _waitTimeSpan);
        }

        /// <summary>
        /// Sets the output to either Normal or EdgeToEdge
        /// </summary>
        /// <param name="edgetoedge">if set to <c>true</c> set Edge to edge , otherwise disable it</param>
        public override void SetEdgeToEdge(bool edgetoedge)
        {
            ScrollToOption("CopyEdgeToEdgeButton");
            _controlPanel.PressWait("CopyEdgeToEdgeButton", "EdgeToEdgeDialog", _waitTimeSpan);
            _controlPanel.Press(edgetoedge ? "OptionEnable" : "OptionDisable");
            _controlPanel.PressWait("m_OKButton", "CopyAppMainForm", _waitTimeSpan);
        }

        /// <summary>
        /// Sets the collating pages On or Off
        /// </summary>
        /// <param name="collate">if set to <c>true</c> enable Collate; otherwise disable it</param>
        public override void SetCollate(bool collate)
        {
            if (_controlPanel.GetControls().Contains("CopyStapleCollateDialogButton"))
            {
                _controlPanel.PressWait("CopyStapleCollateDialogButton", "CopyStapleCollateDialog", _waitTimeSpan);
                if (!collate)
                {
                    _controlPanel.Press("mCollateCheckBox");
                }
            }
            else
            {
                _controlPanel.PressWait("CopyCollateDialogButton", "CollateDialog", _waitTimeSpan);
                _controlPanel.Press(collate ? "collated" : "uncollated");
            }

            _controlPanel.PressWait("m_OKButton", "CopyAppMainForm", _waitTimeSpan);
        }

        /// <summary>
        /// Sets the sides for Original and Ouput sides 
        /// </summary>
        /// <param name="originalOnesided">if set to <c>true</c> Orignal side is set to 1-sided ; otherwise 2-sided</param>
        /// <param name="outputOnesided">if set to <c>true</c> Output side is set to 1-sided ; otherwise 2-sided</param>
        /// <param name="originalPageFlipUp">If set to <c>true</c> Page flip is enabled; otherwise it is disabled</param>
        /// <param name="outputPageFlipUp">If set to <c>true</c> Output page flip is enabled ; otherwise it is disabled</param>
        public override void SetSides(bool originalOnesided, bool outputOnesided, bool originalPageFlipUp, bool outputPageFlipUp)
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
        public override void SetScanMode(ScanMode scanModeSelected)
        {
            throw new NotImplementedException("Set Scan mode job not implemented");
        }

        /// <summary>
        /// Set the booklet format to On-Off
        /// </summary>
        /// <param name="bookletformat">If set to <c>true</c> booklet format is enabaled; otherwise its disabled</param>
        /// <param name="borderOnPage">If set to <c>true</c> border on page is set to true; else it is false</param>      
        public override void SetBooklet(bool bookletformat, bool borderOnPage)
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
        /// Set the Pages per Sheet
        /// </summary>
        /// <param name="pages">Set the pages per sheet for the document using the PagesPerSheet enum </param>
        /// <param name="addPageBorders">If set to <c>true</c> Page borders are added ; otherwise page borders are not added </param>
        public override void SetPagesPerSheet(PagesPerSheet pages, bool addPageBorders)
        {
            throw new NotImplementedException("Set Pages per sheet job not implemented");
        }

        /// <summary>
        /// Sets the Original size of paper
        /// </summary>
        /// <param name="sizeType">Set the paper size for original Size using Original Size Enum</param>
        public void SetOriginalSize(OriginalSize sizeType)
        {
            throw new NotImplementedException("Set Original size job not implemented");
        }

        /// <summary>
        /// Set Paper selection for Size, type and tray 
        /// </summary>
        /// <param name="paperSize">Sets the paper size </param>
        /// <param name="paperType">Sets the paper type</param>
        /// <param name="paperTray">Sets the paper tray to use </param>
        public override void SetPaperSelection(PaperSelectionPaperSize paperSize, PaperSelectionPaperType paperType, PaperSelectionPaperTray paperTray)
        {
            throw new NotImplementedException("Set Paper selection job not implemented");
        }
    }
}