using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A device simulator tracked by Asset Inventory.
    /// </summary>
    public class DeviceSimulator : Asset
    {
        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the type of the simulator.
        /// </summary>
        public string SimulatorType { get; set; }

        /// <summary>
        /// Gets or sets the network address.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the administrator password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the firmware version.
        /// </summary>
        public string FirmwareVersion { get; set; }

        /// <summary>
        /// Gets or sets the virtual machine where the simulator is hosted.
        /// </summary>
        public string VirtualMachine { get; set; }

        /// <summary>
        /// Builds an <see cref="AssetInfo" /> from this instance.
        /// If not applicable for this asset type, returns null instead.
        /// </summary>
        /// <returns>An <see cref="AssetInfo" /> with the data from this instance, or null.</returns>
        public override AssetInfo ToAssetInfo()
        {
            return new DeviceSimulatorInfo(AssetId, Capability, AssetType, Address, Password, Product, SimulatorType)
            {
                Description = $"{SimulatorType} device simulator at {Address}",
                ReservationKey = GetReservationKey()
            };
        }
    }
}
