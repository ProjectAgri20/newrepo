using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A printer (or similar physical device) tracked by Asset Inventory.
    /// </summary>
    public class Printer : Asset
    {
        /// <summary>
        /// Gets or sets the product name.
        /// </summary>
        public string Product { get; set; }

        /// <summary>
        /// Gets or sets the model name.
        /// </summary>
        public string Model { get; set; }

        /// <summary>
        /// Gets or sets the primary network address.
        /// </summary>
        public string Address1 { get; set; }

        /// <summary>
        /// Gets or sets the secondary network address.
        /// </summary>
        public string Address2 { get; set; }

        /// <summary>
        /// Gets or sets the administrator password for this device.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the port used for printing to the device.
        /// </summary>
        public int PortNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether SNMP is enabled for the device.
        /// </summary>
        public bool SnmpEnabled { get; set; }

        /// <summary>
        /// Gets or sets the printer description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the location of the device.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the device owner.
        /// </summary>
        public string Owner { get; set; }

        /// <summary>
        /// Gets or sets the printer type (e.g. MFP, SFP, etc.).
        /// </summary>
        public string PrinterType { get; set; }

        /// <summary>
        /// Gets or sets the firmware type.
        /// </summary>
        public string FirmwareType { get; set; }

        /// <summary>
        /// Gets or sets the engine type.
        /// </summary>
        public string EngineType { get; set; }

        /// <summary>
        /// Gets or sets the model number.
        /// </summary>
        public string ModelNumber { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        public string SerialNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the device is online.
        /// </summary>
        public bool Online { get; set; }

        /// <summary>
        /// Builds an <see cref="AssetInfo" /> from this instance.
        /// If not applicable for this asset type, returns null instead.
        /// </summary>
        /// <returns>An <see cref="AssetInfo" /> with the data from this instance, or null.</returns>
        public override AssetInfo ToAssetInfo()
        {
            string description = $"{Product} ({Model}) at {Address1}";
            if (!string.IsNullOrWhiteSpace(Description))
            {
                description += $": {Description}";
            }

            return new PrintDeviceInfoInternal(AssetId, Capability, PrinterType, Address1, Address2, Password, Product, PortNumber, SnmpEnabled)
            {
                Description = description,
                ReservationKey = GetReservationKey(),
                ModelName = Model,
                ModelNumber = ModelNumber,
                SerialNumber = SerialNumber
            };
        }
    }
}
