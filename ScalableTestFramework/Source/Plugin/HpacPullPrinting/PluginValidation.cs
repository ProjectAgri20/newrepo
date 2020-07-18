using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.HpacPullPrinting
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the HPAC Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            HpacActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<HpacActivityData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new HpacActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, HpacPullPrintingConfigurationControl.Version);

            return validData;
        }
    }
}
