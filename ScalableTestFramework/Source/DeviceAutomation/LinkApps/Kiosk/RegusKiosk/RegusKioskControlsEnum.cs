using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.Kiosk.RegusKiosk
{
    // <summary>
    // KioskControlsEnum contains enums for controlling Kiosk App
    // </summary>

    #region RegusKioskJobType Enums
    /// <summary>
    /// Job Type enum
    /// </summary>
    public enum RegusKioskJobType
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

    #region RegusKioskAuthType Enums
    /// <summary>
    /// Auth Type enum
    /// </summary>
    public enum RegusKioskAuthType
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

        /// <summary>
        /// Auth Type: PIN - It use the World Key PIN registered on Regus Server
        /// </summary>
        [Description("PIN (World Key PIN)")]
        Pin,
    }
    #endregion
}
