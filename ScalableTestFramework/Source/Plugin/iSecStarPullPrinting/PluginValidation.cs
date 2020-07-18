using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.iSecStarPullPrinting
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the ISecStar Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            iSecStarActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<iSecStarActivityData>();
            }
            catch
            {
                activityData = new iSecStarActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, iSecStarPullPrintingConfigurationControl.Version);

            return validData;
        }
    }
}
