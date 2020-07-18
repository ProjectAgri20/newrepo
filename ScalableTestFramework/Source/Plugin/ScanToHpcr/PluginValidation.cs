using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.ScanToHpcr
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the ScanToHpcr Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            ScanToHpcrActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<ScanToHpcrActivityData>();
            }
            catch
            {
                activityData = new ScanToHpcrActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, ScanToHpcrConfigControl.Version);

            return validData;
        }
    }
}
