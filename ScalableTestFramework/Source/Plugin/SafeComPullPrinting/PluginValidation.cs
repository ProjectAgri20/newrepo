using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.SafeComPullPrinting
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the SafeCom Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            SafeComActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<SafeComActivityData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new SafeComActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, SafeComPullPrintingConfigurationControl.Version);

            return validData;
        }
    }
}
