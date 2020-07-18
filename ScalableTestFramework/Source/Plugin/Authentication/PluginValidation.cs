using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.Authentication
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the Authentication Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            AuthenticationData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<AuthenticationData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new AuthenticationData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, AuthenticationConfigControl.Version);

            return validData;
        }
    }
}
