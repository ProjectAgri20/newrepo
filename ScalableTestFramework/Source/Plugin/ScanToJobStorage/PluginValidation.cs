using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.ScanToJobStorage
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the ScanToJobStorage Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            ScanToJobStorageData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<ScanToJobStorageData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new ScanToJobStorageData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, ScanToJobStorageConfigControl.Version);

            return validData;
        }
    }
}
