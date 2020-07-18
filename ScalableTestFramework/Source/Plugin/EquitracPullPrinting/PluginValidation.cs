using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.EquitracPullPrinting
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the Equitrac Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            EquitracActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<EquitracActivityData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new EquitracActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, EquitracConfigControl.Version);

            return validData;
        }
    }
}
