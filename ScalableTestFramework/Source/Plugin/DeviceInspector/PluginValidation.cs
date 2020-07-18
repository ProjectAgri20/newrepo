using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.DeviceInspector
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the DeviceInspector Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            DeviceInspectorActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<DeviceInspectorActivityData>();
            }
            catch
            {
                activityData = new DeviceInspectorActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, DeviceInspectorConfigurationControl.Version);

            return validData;
        }
    }
}
