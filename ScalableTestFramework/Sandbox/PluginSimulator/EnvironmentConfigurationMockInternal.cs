using System.Collections.Generic;
using System.Collections.ObjectModel;
using HP.ScalableTest.Framework;

namespace PluginSimulator
{
    internal sealed class EnvironmentConfigurationMockInternal : IEnvironmentConfiguration, IEnvironmentConfigurationInternal
    {
        /// <summary>
        /// Gets the digital send destinations.
        /// </summary>
        public Collection<string> OutputMonitorDestinations { get; } = new Collection<string>();

        /// <summary>
        /// Gets the server services.
        /// </summary>
        public Collection<string> ServerServices { get; } = new Collection<string>();

        /// <summary>
        /// Gets the printer families.
        /// </summary>
        public Collection<string> PrinterFamilies { get; } = new Collection<string>();

        /// <summary>
        /// Gets the printer products.
        /// </summary>
        public Collection<string> PrinterProducts { get; } = new Collection<string>();

        /// <summary>
        /// Retrieves a collection of configured STF output monitor destinations of the specified type.
        /// </summary>
        /// <param name="destinationType">The type of output monitor destination to retrieve.</param>
        /// <returns>A collection of output monitor destinations.</returns>
        public IEnumerable<string> GetOutputMonitorDestinations(string destinationType)
        {
            return OutputMonitorDestinations;
        }

        /// <summary>
        /// Gets the services available for the specified server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns>A collection of service names for the specified server.</returns>
        public IEnumerable<string> GetServerServices(string server)
        {
            return ServerServices;
        }

        /// <summary>
        /// Gets a collection of printer families from Asset Inventory.
        /// </summary>
        /// <returns>A collection of printer families.</returns>
        public IEnumerable<string> GetPrinterFamilies()
        {
            return PrinterFamilies;
        }

        /// <summary>
        /// Gets a collection of printer products for the specified family from Asset Inventory.
        /// </summary>
        /// <param name="printerFamily">The printer family.</param>
        /// <returns>A collection of printer products.</returns>
        public IEnumerable<string> GetPrinterProducts(string printerFamily)
        {
            return PrinterProducts;
        }
    }
}
