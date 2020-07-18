using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A mobile device tracked by Asset Inventory.
    /// </summary>
    public class MobileDevice : Asset
    {
        /// <summary>
        /// Gets or sets the mobile equipment ID (IMEI) of the device.
        /// </summary>
        public string MobileEquipmentId { get; set; }

        /// <summary>
        /// Gets or sets the mobile device type.
        /// </summary>
        public string MobileDeviceType { get; set; }

        /// <summary>
        /// Gets or sets the mobile device description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Builds an <see cref="AssetInfo" /> from this instance.
        /// If not applicable for this asset type, returns null instead.
        /// </summary>
        /// <returns>An <see cref="AssetInfo" /> with the data from this instance, or null.</returns>
        public override AssetInfo ToAssetInfo()
        {
            return new MobileDeviceInfo(AssetId, Capability, AssetType, MobileEquipmentId, EnumUtil.Parse<MobileDeviceType>(MobileDeviceType, true))
            {
                Description = Description,
                ReservationKey = GetReservationKey()
            };
        }
    }
}
