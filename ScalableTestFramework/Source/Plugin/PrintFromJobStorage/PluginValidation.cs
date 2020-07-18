using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.PrintFromJobStorage
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the PrintFromJobStorage Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            PrintFromJobStorageActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<PrintFromJobStorageActivityData>();
            }
            catch
            {
                activityData = new PrintFromJobStorageActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, PrintFromJobStorageConfigControl.Version);

            return validData;
        }
    }
}
