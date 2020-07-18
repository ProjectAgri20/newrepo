using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.Copy
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the Copy Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            CopyData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<CopyData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new CopyData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, CopyConfigControl.Version);

            return validData;
        }
    }
}
