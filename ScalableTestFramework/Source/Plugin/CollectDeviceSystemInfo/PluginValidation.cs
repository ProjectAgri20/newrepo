using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.CollectDeviceSystemInfo
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the CollectDeviceSystemInfo Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            CollectDeviceSystemInfoActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<CollectDeviceSystemInfoActivityData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new CollectDeviceSystemInfoActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, CollectDeviceSystemInfoConfigurationControl.Version);

            return validData;
        }
    }
}
