using System.ComponentModel;

namespace HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector
{
    #region ConnectorName
    /// <summary>
    /// Name of Cloud Connector
    /// </summary>
    public enum ConnectorName
    {
        /// <summary>
        /// DropBox
        /// </summary>
        [Description("DropBox")]
        DropBox,

        /// <summary>
        /// Google Drive
        /// </summary>
        [Description("GoogleDrive")]
        GoogleDrive,

        /// <summary>
        /// OneDrive
        /// </summary>
        [Description("OneDriveForPersonal")]
        OneDrive,

        /// <summary>
        /// OneDrive Business
        /// </summary>
        [Description("OneDriveForBusiness")]
        OneDriveBusiness,

        /// <summary>
        /// SharePoint
        /// </summary>
        [Description("SharePoint")]
        SharePoint,

        /// <summary>
        /// SharePoint
        /// </summary>
        [Description("Box")]
        Box,
    }
    #endregion

    #region ConnectorDisplayName
    /// <summary>
    /// Displayed on UI for Cloud Connector
    /// </summary>
    public enum ConnectorDisplayName
    {
        /// <summary>
        /// DropBox
        /// </summary>
        [Description("#hpid-a8f9c6f8-6734-42cb-bf1f-dd7b8fb628ae-homescreen-button")]
        DropBox,

        /// <summary>
        /// Google Drive
        /// </summary>
        [Description("#hpid-66029bd4-93b5-42a2-9945-4bbc723c80a1-homescreen-button")]
        GoogleDrive,

        /// <summary>
        /// OneDrive
        /// </summary>
        [Description("#hpid-3f553433-a6e9-4a70-838e-4873d18edb7f-homescreen-button")]
        OneDriveForPersonal,

        /// <summary>
        /// OneDrive Business
        /// </summary>
        [Description("#hpid-7fbeabca-cc3e-46ef-92cd-d4a00c9ee935-homescreen-button")]
        OneDriveForBusiness,

        /// <summary>
        /// SharePoint
        /// </summary>
        [Description("#hpid-a206482f-df58-4b28-893b-f120a1d9989b-homescreen-button")]
        SharePoint,

        /// <summary>
        /// SharePoint
        /// </summary>
        [Description("#hpid-ef9beb16-aceb-4b55-924b-45a5583748c4-homescreen-button")]
        Box,
    }
    #endregion

    #region ConnectAccountInfo
    /// <summary>
    /// Account Info of Cloud Connector
    /// </summary>
    public enum ConnectAccountInfo
    {
        /// <summary>
        /// DropBox
        /// </summary>
        [Description("Plugin doesn't support 'Sign in with Google' at DropBox.")]
        DropBox,

        /// <summary>
        /// Google Drive
        /// </summary>
        [Description("Plugin support Google account at Google® Drive.")]
        GoogleDrive,

        /// <summary>
        /// OneDrive
        /// </summary>
        [Description("Plugin support personal account at OneDrive")]
        OneDriveForPersonal,

        /// <summary>
        /// OneDrive Business
        /// </summary>
        [Description("Plugin doesn't support HP Account at OneDrive for Business.\nRecommend the office trial version.")]
        OneDriveForBusiness,

        /// <summary>
        /// SharePoint
        /// </summary>
        [Description("Plugin doesn't support HP Account at SharePoint.\nRecommend the office trial version.")]
        SharePoint,

        /// <summary>
        /// SharePoint
        /// </summary>
        [Description("It is just test item. Don't select it")]
        Box,
    }
    #endregion

    #region ConnectorJobType
    /// <summary>
    /// print = 0 , scan = 1
    /// </summary>
    public enum ConnectorJobType
    {
        /// <summary>
        /// Print
        /// </summary>
        [Description("Print")]
        Print,

        /// <summary>
        /// Scan
        /// </summary>
        [Description("Scan")]
        Scan,

        /// <summary>
        /// Multiple Print
        /// </summary>
        [Description("Print (Multiple files)")]
        MultiplePrint
    }
    #endregion

    #region ConnectorExceptionCategory
    /// <summary>
    /// Setting for ConnectorExceptionCategory
    /// </summary>
    public enum ConnectorExceptionCategory
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

        /// <summary>
        /// Environment error (Environment error)
        /// </summary>
        [Description("Environment error (App error, etc)")]
        EnvironmentError,
    }
    #endregion
}
