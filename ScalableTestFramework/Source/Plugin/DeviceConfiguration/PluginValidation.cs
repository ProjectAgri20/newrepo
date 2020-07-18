using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.DeviceConfiguration
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the DeviceConfiguration Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            DeviceConfigurationActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<DeviceConfigurationActivityData>();
            }
            catch
            {
                activityData = new DeviceConfigurationActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, DeviceConfigurationConfigurationControl.Version);

            return validData;
        }
    }
}
