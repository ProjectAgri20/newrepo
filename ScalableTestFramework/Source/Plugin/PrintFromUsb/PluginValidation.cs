using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.PrintFromUsb
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the PrintFromUsb Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            PrintFromUsbActivityData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<PrintFromUsbActivityData>();
            }
            catch
            {
                activityData = new PrintFromUsbActivityData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, PrintFromUsbConfigControl.Version);

            return validData;
        }
    }
}
