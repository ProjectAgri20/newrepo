using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.NativeApps.TraySettings
{
    /// <summary>
    /// Settings
    /// </summary>
    public struct TraySettings
    {
        /// <summary>
        /// if set to <c>true</c> , 'Exclusively' is selected otherwise 'When Avaiable' is set
        /// </summary>
        public bool UseRequesetedTray { get; set; }

        /// <summary>
        /// if set to <c>true</c> ,'Always Prompt' is selected otherwise 'Prompt On Mismatch'is set
        /// </summary>
        public bool ManualFeedPrompt { get; set; }

        /// <summary>
        /// if set to <c>true</c> ,'Display' is selected otherwise 'Do not display' is set
        /// </summary>
        public bool SizeTypePrompt { get; set; }

        /// <summary>
        /// if set to <c>true</c> ,'Allow' is selected otherwise 'Do not Allow' is set
        /// </summary>
        public bool UseAnotherTray { get; set; }

        /// <summary>
        ///  if set to <c>true</c> ,'Off' is selected otherwise 'On' is set
        /// </summary>
        public bool AlternativeLetterheadMode { get; set; }

        /// <summary>
        ///   if set to <c>true</c> ,'Automatic' is selected otherwise 'Always' is set
        /// </summary>
        public bool DuplexBlankPages { get; set; }

        /// <summary>
        /// Specifies the Roatation type to be set
        /// </summary>
        public ImageRoationType ImageRotation { get; set; }

        /// <summary>
        /// if set to <c>true</c> ,'No' is set otherwise 'Yes' is set
        /// </summary>
        public bool OverrideA4Letter { get; set; }

        /// <summary>
        /// If set to <c>true</c>, User Requested tray is set for the device
        /// </summary>        
        public bool IsUseRequesetedTraySet { get; set; }

        /// <summary>
        /// If set to <c>true</c>, Manual Feed prompt is set for the device
        /// </summary>
        public bool IsManualFeedPromptSet { get; set; }

        /// <summary>
        /// If set to <c>true</c>, Size Type prompt is set for the device
        /// </summary>
        public bool IsSizeTypePromptSet { get; set; }

        /// <summary>
        /// If set to <c>true</c>, Use another tray is set for the device
        /// </summary>
        public bool IsUseAnotherTraySet { get; set; }

        /// <summary>
        /// If set to <c>true</c>, Alternative letter head mode is set for the device
        /// </summary>
        public bool IsAlternativeLetterheadModeSet { get; set; }

        /// <summary>
        /// If set to <c>true</c>, Duplex Blank page is set for the device
        /// </summary>
        public bool IsDuplexBlankPagesSet { get; set; }

        /// <summary>
        /// If set to <c>true</c>, Image Rotation is set for the device
        /// </summary>
        public bool IsImageRotationSet { get; set; }

        /// <summary>
        /// If set to <c>true</c>, Override A4 letter is set for the device
        /// </summary>
        public bool IsOverrideA4LetterSet { get; set; }
    }

    /// <summary>
    /// 
    /// </summary>
    public enum ImageRoationType
    {
        /// <summary>
        /// Rotate image from Left to Right
        /// </summary>
        [Description("LeftToRight")]
        LeftToRight=0,

        /// <summary>
        /// Rotate image from Right to Left
        /// </summary>
        [Description("RightToLeft")]
        RightToLeft,

        /// <summary>
        /// Rotate image Alternate
        /// </summary>
        [Description("Alternate")]
        Alternate,

    }
}
