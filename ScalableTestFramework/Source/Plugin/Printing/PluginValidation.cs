using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.PluginSupport.Print;

namespace HP.ScalableTest.Plugin.Printing
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the Printing Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            PrintingActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<PrintingActivityData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new PrintingActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, PrintingConfigurationControl.Version);

            return validData;
        }
    }
}
