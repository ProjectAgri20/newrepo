using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.Blueprint
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the Blueprint Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            BlueprintActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<BlueprintActivityData>();
            }
            catch
            {
                activityData = new BlueprintActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, BlueprintConfigurationControl.Version);

            return validData;
        }
    }
}
