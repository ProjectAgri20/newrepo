using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.HpacScan
{
    #region PaperSupply Enums begin
    /// <summary>
    /// LogOutMethode enum
    /// </summary>
    public enum PaperSupplyType
    {
        /// <summary>
        /// Simplex
        /// </summary>
        [Description("Simplex")]
        Simplex,
        /// <summary>
        /// Duplex
        /// </summary>
        [Description("Duplex")]
        Duplex
    }
    #endregion PaperSupply Enum End
    #region ColorMode Enums begin
    /// <summary>
    /// ColorMode enum
    /// </summary>
    public enum ColorModeType
    {
        /// <summary>
        /// Color
        /// </summary>
        [Description("Color")]
        Color,
        /// <summary>
        /// Greyscale
        /// </summary>
        [Description("Greyscale")]
        Greyscale,
        /// <summary>
        /// Monochrome
        /// </summary>
        [Description("Monochrome")]
        Monochrome
    }
    #endregion ColorMode Enum End
    #region Quality Enums begin
    /// <summary>
    /// Quality enum
    /// </summary>
    public enum QualityType
    {
        /// <summary>
        /// Low
        /// </summary>
        [Description("Low")]
        Low,
        /// <summary>
        /// Normal
        /// </summary>
        [Description("Normal")]
        Normal,
        /// <summary>
        /// High
        /// </summary>
        [Description("High")]
        High
    }
    #endregion Quality Enum End
}
