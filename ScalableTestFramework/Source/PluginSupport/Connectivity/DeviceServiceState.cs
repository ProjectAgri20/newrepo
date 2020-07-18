namespace HP.ScalableTest.PluginSupport.Connectivity
{
    /// <summary>
    /// Contains the device service state
    /// </summary>
    public enum DeviceServiceState
    {
        /// <summary>
        /// Service will be skipped, will not be validated
        /// </summary>
        Skip,

        /// <summary>
        /// Service should pass
        /// </summary>
        Pass,

        /// <summary>
        /// Service should fail
        /// </summary>
        Fail
    }
}
