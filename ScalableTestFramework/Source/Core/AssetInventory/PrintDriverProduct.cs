using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A print driver product tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public sealed class PrintDriverProduct
    {
        /// <summary>
        /// Gets or sets the unique identifier for the print driver product.
        /// </summary>
        public Guid PrintDriverProductId { get; set; }

        /// <summary>
        /// Gets or sets the name of the print driver product.
        /// </summary>
        public string Name { get; set; }
    }
}
