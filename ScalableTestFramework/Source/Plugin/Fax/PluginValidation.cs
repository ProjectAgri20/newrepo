using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.Fax
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the Fax Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            FaxActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<FaxActivityData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new FaxActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, FaxConfigControl.Version);

            return validData;
        }
    }
}
