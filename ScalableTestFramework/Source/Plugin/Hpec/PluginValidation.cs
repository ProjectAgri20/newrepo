using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.Hpec
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the HPEC Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            HpecActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<HpecActivityData>();
            }
            catch
            {
                activityData = new HpecActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, HpecConfigControl.Version);

            return validData;
        }
    }
}
