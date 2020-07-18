using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A virtual printer tracked by Asset Inventory.
    /// </summary>
    public class VirtualPrinter : Asset
    {
        /// <summary>
        /// Gets or sets the network address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the port used for printing to the device.
        /// </summary>
        public int PortNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SNMP is enabled for the device.
        /// </summary>
        public bool SnmpEnabled { get; set; }

        /// <summary>
        /// Builds an <see cref="AssetInfo" /> from this instance.
        /// If not applicable for this asset type, returns null instead.
        /// </summary>
        /// <returns>An <see cref="AssetInfo" /> with the data from this instance, or null.</returns>
        public override AssetInfo ToAssetInfo()
        {
            return new VirtualPrinterInfo(AssetId, Capability, AssetType, Address, PortNumber, SnmpEnabled)
            {
                Description = $"Virtual printer at {Address}",
                ReservationKey = GetReservationKey()
            };
        }
    }
}
