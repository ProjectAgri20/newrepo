using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Plugin.ScanToWorkflow
{
    /// <summary>
    /// Public class implementing the IPluginValidation
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Plugin.IPluginValidation" />
    public class PluginValidation : IPluginValidation
    {
        /// <summary>
        /// Validates the given metadata against the ScanToWorkflow Activity data.
        /// </summary>
        /// <param name="configurationData">The configuration data.</param>
        /// <returns>true if valid</returns>
        public bool ValidateMetadata(ref PluginConfigurationData configurationData)
        {
            bool validData = true;
            ScanToWorkflowData activityData = null;

            try
            {
                activityData = configurationData.GetMetadata<ScanToWorkflowData>(ConverterProvider.GetMetadataConverters());
            }
            catch
            {
                activityData = new ScanToWorkflowData();
                validData = false;
            }

            configurationData = new PluginConfigurationData(activityData, ScanToWorkflowConfigControl.Version);

            return validData;
        }
    }
}
