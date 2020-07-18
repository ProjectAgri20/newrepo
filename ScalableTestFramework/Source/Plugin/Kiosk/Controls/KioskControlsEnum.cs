using System.ComponentModel;

namespace HP.ScalableTest.Plugin.Kiosk.Controls
{
    // <summary>
    // KioskControlsEnum contains enums for controlling Kiosk App
    // </summary>

    #region KioskJobType Enums
    /// <summary>
    /// Job Type enum
    /// </summary>
    public enum KioskJobType
    {
        /// <summary>
        /// Job Type: Copy
        /// </summary>
        [Description("Copy")]
        Copy = 0,

        /// <summary>
        /// Job Type: Print
        /// </summary>
        [Description("Print")]
        Print,

        /// <summary>
        /// Job Type: Scan
        /// </summary>
        [Description("Scan")]
        Scan,
    }
    #endregion

    #region KioskAuthType Enums
    /// <summary>
    /// Auth Type enum
    /// </summary>
    public enum KioskAuthType
    {
        /// <summary>
        /// Auth Type: Log in- It use the ID and Password registered on Regus Server
        /// </summary>
        [Description("Log in (ID/Password)")]
        Login = 0,

        /// <summary>
        /// Auth Type: Card - It will use the Badge Box for STB test 
        /// </summary>
        [Description("Card (Badge Box)")]
        Card,
    }
    #endregion
}
