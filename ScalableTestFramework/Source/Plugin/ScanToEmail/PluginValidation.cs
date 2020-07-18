using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.ScanToEmail
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the ScanToEmail Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            ScanToEmailData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<ScanToEmailData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new ScanToEmailData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, ScanToEmailConfigControl.Version);

            return validData;
        }
    }
}
