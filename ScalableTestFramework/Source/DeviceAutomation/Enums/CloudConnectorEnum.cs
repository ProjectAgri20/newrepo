using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.DeviceAutomation.Enums
{
    #region Logout Enums begin
    /// <summary>
    /// LogOutMethode enum
    /// </summary>
    public enum LogOutMethod
    {
        /// <summary>
        /// PressSignOut
        /// </summary>
        [Description("Press Sign Out")]
        PressSignOut,
        /// <summary>
        /// PressBackKey
        /// </summary>
        [Description("Press Back Key")]
        PressBackKey
    }
    #endregion LogOutMethode Enum End

    #region SIO Enums begin
    /// <summary>
    /// LogOutMethode enum
    /// </summary>
    public enum SIOMethod
    {
        /// <summary>
        /// SIOWithoutIDPWD
        /// </summary>
        [Description("SIO Without ID PWD")]
        SIOWithoutIDPWD,
        /// <summary>
        /// SIOWithIDPWD
        /// </summary>
        [Description("SIO With ID PWD")]
        SIOWithIDPWD,
        /// <summary>
        /// NoneSIO
        /// </summary>
        [Description("None SIO")]
        NoneSIO
    }
    #endregion LogOutMethode Enum End
}
