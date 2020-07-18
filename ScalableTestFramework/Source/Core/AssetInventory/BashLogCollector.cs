using System;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A bash log collector tracked by Asset Inventory.
    /// </summary>
    public sealed class BashLogCollector
    {
        /// <summary>
        /// Gets or sets the unique identifier for the bash log collector.
        /// </summary>
        public Guid BashLogCollectorId { get; set; }

        /// <summary>
        /// Gets or sets the bash log collector address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the port to use for connecting to the bash log collector.
        /// </summary>
        public int? Port { get; set; }

        /// <summary>
        /// Gets or sets the asset ID of the associated print device.
        /// </summary>
        public string PrinterId { get; set; }
    }
}
