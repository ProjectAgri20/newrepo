using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation
{
    // <summary>
    // CopyJobOptionEnum contains different settings of JobOptions
    // </summary>

    ///////////////////////Copy options specific Enums //////////////////////////////

    #region Stamp Enums begin
    /// <summary>
    /// Stamps enum
    /// </summary>
    public enum StampType
    {
        /// <summary>
        /// Stam Type:Top Left
        /// </summary>
        [Description("Top Left")]
        TopLeft,
        /// <summary>
        /// Stamp Type:Top Center 
        /// </summary>
        [Description("Top Center")]
        TopCenter,
        /// <summary>
        /// Stamp Type:Top Right 
        /// </summary>
        [Description("Top Right")]
        TopRight,
        /// <summary>
        /// Stamp Type:Bottom Left 
        /// </summary>
        [Description("Bottom Left")]
        BottomLeft,
        /// <summary>
        /// Stamp Type:Bottom Center 
        /// </summary>
        [Description("Bottom Center")]
        BottomCenter,
        /// <summary>
        /// Stamp Type:Bottom Right 
        /// </summary>
        [Description("Bottom Right")]
        BottomRight
    }
    #endregion Stamp Enums End

    #region Scan Mode Enums
    /// <summary>
    /// Scan Mode enum
    /// </summary>
    public enum ScanMode
    {
        /// <summary>
        /// Standard Document scan mode
        /// </summary>
        [Description("Standard Document")]
        Standard = 0,

        /// <summary>
        /// Book mode scan mode
        /// </summary>
        [Description("Book Mode")]
        BookMode,

        /// <summary>
        /// 2-sided ID scan mode
        /// </summary>
        [Description("2-sided ID")]
        IDCard
    }
    #endregion
}
