using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.ScanToFolder
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the ScanToFolder Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            ScanToFolderData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<ScanToFolderData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new ScanToFolderData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, ScanToFolderConfigControl.Version);

            return validData;
        }
    }
}
