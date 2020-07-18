using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.LowLevelConfig
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the LowLevelConfig Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            LowLevelConfigActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<LowLevelConfigActivityData>();
            }
            catch
            {
                activityData = new LowLevelConfigActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, LowLevelConfigConfigurationControl.Version);

            return validData;
        }
    }
}
