using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.DirtyDevice
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the DirtyDevice Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            DirtyDeviceActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<DirtyDeviceActivityData>(new[] { new DirtyDeviceDataConverter1_1() });
            }
            catch
            {
                activityData = new DirtyDeviceActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, DirtyDeviceConfigurationControl.Version);

            return validData;
        }
    }
}
