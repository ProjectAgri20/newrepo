using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A print driver tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{Name,nq} [{PrintProcessor,nq}]")]
    public class PrintDriver
    {
        /// <summary>
        /// Gets or sets unique identifier for the print driver.
        /// </summary>
        public Guid PrintDriverId { get; set; }

        /// <summary>
        /// Gets or sets the driver name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the print processor.
        /// </summary>
        public string PrintProcessor { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="AssetInventory.PrintDriverPackage" /> for this print driver.
        /// </summary>
        public virtual PrintDriverPackage PrintDriverPackage { get; set; }
    }
}
