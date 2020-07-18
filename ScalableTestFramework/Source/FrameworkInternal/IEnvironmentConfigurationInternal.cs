using System.Collections.Generic;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Internal extensions to <see cref="IEnvironmentConfiguration" />.
    /// </summary>
    public interface IEnvironmentConfigurationInternal : IEnvironmentConfiguration
    {
        /// <summary>
        /// Gets the services available for the specified server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns>A collection of service names for the specified server.</returns>
        IEnumerable<string> GetServerServices(string server);

        /// <summary>
        /// Gets a collection of printer families from Asset Inventory.
        /// </summary>
        /// <returns>A collection of printer families.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is expensive.")]
        IEnumerable<string> GetPrinterFamilies();

        /// <summary>
        /// Gets a collection of printer products for the specified family from Asset Inventory.
        /// </summary>
        /// <param name="printerFamily">The printer family.</param>
        /// <returns>A collection of printer products.</returns>
        IEnumerable<string> GetPrinterProducts(string printerFamily);
    }
}
