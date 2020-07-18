using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.PaperCut
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the PaperCut Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            PaperCutActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<PaperCutActivityData>();
            }
            catch
            {
                activityData = new PaperCutActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, PaperCutConfigurationControl.Version);

            return validData;
        }
    }
}
