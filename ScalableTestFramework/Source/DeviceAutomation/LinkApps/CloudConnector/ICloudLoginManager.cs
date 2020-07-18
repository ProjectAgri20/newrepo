namespace HP.ScalableTest.DeviceAutomation.LinkApps.CloudConnector
{
    /// <summary>
    /// Interface for Link Apps
    /// </summary>
    public interface ICloudLoginManager : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Selects the folder quickset with the specified name.
        /// <param name="ID_LinkApp">ID of the Link application for sign in on AA</param>
        /// <param name="PW_LinkApp">PW of the Link application for sign in on AA</param>
        /// </summary> 
        bool SignIn(string ID_LinkApp, string PW_LinkApp);
    }
}