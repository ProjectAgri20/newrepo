using System;
using System.Collections.Generic;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Internal extensions to <see cref="IAssetInventory" />.
    /// </summary>
    public interface IAssetInventoryInternal : IAssetInventory
    {
        /// <summary>
        /// Retrieves <see cref="PrintDriverInfo" /> for all print drivers in the inventory.
        /// </summary>
        /// <returns><see cref="PrintDriverInfo" /> for all print drivers in the inventory.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is expensive.")]
        IEnumerable<PrintDriverInfo> GetPrintDrivers();

        /// <summary>
        /// Retrieves all print driver configurations in the inventory.
        /// </summary>
        /// <returns>All print driver configurations in the inventory.</returns>
        IEnumerable<string> GetPrintDriverConfigurations();

        /// <summary>
        /// Retrieves print driver configurations that match the specified device and driver.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="driver">The driver.</param>
        /// <returns>Print driver configurations for the specified device and driver.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="device" /> is null.
        /// <para>or</para>
        /// <paramref name="driver" /> is null.
        /// </exception>
        IEnumerable<string> GetPrintDriverConfigurations(IDeviceInfo device, PrintDriverInfo driver);
    }
}
