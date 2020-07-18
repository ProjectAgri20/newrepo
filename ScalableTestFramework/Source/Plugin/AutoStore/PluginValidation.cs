using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.AutoStore
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the AutoStore Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            AutoStoreActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<AutoStoreActivityData>();
            }
            catch
            {
                activityData = new AutoStoreActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, AutoStoreConfigControl.Version);

            return validData;
        }
    }
}
