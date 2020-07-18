using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.VerticalConnector.iManage
{
    #region SIO Enums begin
    /// <summary>
    /// LogOutMethode enum
    /// </summary>
    public enum SIOMethod
    {
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
    #endregion SIO Enum End

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
        PressSignOut
    }
    #endregion LogOutMethod Enum End

    #region iManageJobType Enums
    /// <summary>
    /// iManageJobType enum
    /// </summary>
    public enum iManageJobType
    {
        /// <summary>
        /// Job Type: Print
        /// </summary>
        [Description("Print")]
        Print = 0,

        /// <summary>
        /// Job Type: Scan
        /// </summary>
        [Description("Scan")]
        Scan,
    }
    #endregion iManageJobType

    #region Kind of Storage locations
    /// <summary>
    /// Kind of Storage locations
    /// </summary>
    public enum StorageLocation
    {
        /// <summary>
        /// Location : Matter
        /// </summary>
        [Description("Matter")]
        Matter = 0,

        /// <summary>
        /// Location : Document
        /// </summary>
        [Description("Document")]
        Document,

        /// <summary>
        /// Job Type: Scan
        /// </summary>
        [Description("My Favorite")]
        MyFavorite,
    }
    #endregion  Kind of Storage locations

    #region iManageExceptionCategory
    /// <summary>
    /// Setting for iManageExceptionCategory
    /// </summary>
    public enum iManageExceptionCategory
    {
        /// <summary>
        /// App Launch failed
        /// </summary>
        [Description("App Launch failed")]
        AppLaunch,

        /// <summary>
        /// Sign in failed
        /// </summary>
        [Description("Sign in failed")]
        SignIn,

        /// <summary>
        /// Downloading Print files failed
        /// </summary>
        [Description("Downloading Print files failed")]
        DownloadingPrintFile,

        /// <summary>
        /// Navigate to file path failed
        /// </summary>
        [Description("Navigation to file path failed")]
        NavigateFilePath,

        /// <summary>
        /// Selecting options failed
        /// </summary>
        [Description("Selecting options failed")]
        SelectOptions,

        /// <summary>
        /// Execution Job failed
        /// </summary>
        [Description("Execution Job failed")]
        ExecutionJob,

        /// <summary>
        /// Sign out failed
        /// </summary>
        [Description("Sign out failed")]
        SignOut,

        /// <summary>
        /// Server Error
        /// </summary>
        [Description("Server Error")]
        ServerError,

        /// <summary>
        /// False alarm (Plugin error)
        /// </summary>
        [Description("False alarm (Plugin error)")]
        FalseAlarm,
    }
    #endregion iManageExceptionCategory
}
