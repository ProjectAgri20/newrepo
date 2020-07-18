using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.ScanToUsb
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the ScanToUsb Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            ScanToUsbData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<ScanToUsbData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new ScanToUsbData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, ScanToUsbConfigControl.Version);

            return validData;
        }
    }
}
