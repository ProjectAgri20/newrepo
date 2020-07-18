using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.EnterpriseTest;
using HP.ScalableTest.Framework.Monitor;

namespace HP.ScalableTest.Framework.PluginService
{
    /// <summary>
    /// Provides access to test environment configuration information.
    /// </summary>
    public sealed class EnvironmentConfigurationAdapter : IEnvironmentConfiguration, IEnvironmentConfigurationInternal
    {
        /// <summary>
        /// Retrieves a collection of output monitor destinations of the specified type.
        /// </summary>
        /// <param name="monitorType">The type of output monitor destinations to retrieve.</param>
        /// <returns>A collection of output monitor destinations.</returns>
        public IEnumerable<string> GetOutputMonitorDestinations(string monitorType)
        {
            STFMonitorType stfMonitorType = (STFMonitorType)Enum.Parse(typeof(STFMonitorType), monitorType);
            Collection<string> result = new Collection<string>();
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (MonitorConfig monitorConfig in context.MonitorConfigs.Where(m => m.MonitorType == monitorType))
                {
                    if (monitorConfig.Configuration.StartsWith("<OutputMonitorConfig"))
                    {
                        OutputMonitorConfig outputConfig = LegacySerializer.DeserializeXml<OutputMonitorConfig>(monitorConfig.Configuration);

                        if (stfMonitorType.Equals(STFMonitorType.OutputEmail) || stfMonitorType.Equals(STFMonitorType.DigitalSendNotification))
                        {
                            result.Add(outputConfig.MonitorLocation);
                        }
                        else
                        {
                            result.Add($@"\\{monitorConfig.ServerHostName}\{outputConfig.MonitorLocation}");
                        }
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Gets the services available for the specified server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns>A collection of service names for the specified server.</returns>
        public IEnumerable<string> GetServerServices(string server)
        {
            using (EnterpriseTestContext context = DbConnect.EnterpriseTestContext())
            {
                CategoryValue serverCategory = context.CategoryValues.FirstOrDefault(n => n.Category == "ServerService" && n.Value == server);
                return serverCategory?.Children.Select(n => n.Value).ToList() ?? Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Gets a collection of printer families from Asset Inventory.
        /// </summary>
        /// <returns>A collection of printer families.</returns>
        public IEnumerable<string> GetPrinterFamilies()
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                return context.PrinterProducts.Select(n => n.Family).Distinct().ToList();
            }
        }

        /// <summary>
        /// Gets a collection of printer products for the specified family from Asset Inventory.
        /// </summary>
        /// <param name="printerFamily">The printer family.</param>
        /// <returns>A collection of printer products.</returns>
        public IEnumerable<string> GetPrinterProducts(string printerFamily)
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                return context.PrinterProducts.Where(n => n.Family == printerFamily).Select(n => n.Name).Distinct().ToList();
            }
        }
    }
}
