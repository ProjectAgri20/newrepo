using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A camera tracked by Asset Inventory.
    /// </summary>
    public class Camera : Asset
    {
        /// <summary>
        /// Gets or sets the camera network address.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the asset ID of the associated print device.
        /// </summary>
        public string PrinterId { get; set; }

        /// <summary>
        /// Gets or sets the server this camera is connected to.
        /// </summary>
        public string CameraServer { get; set; }

        /// <summary>
        /// Gets or sets the camera description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Builds an <see cref="AssetInfo" /> from this instance.
        /// If not applicable for this asset type, returns null instead.
        /// </summary>
        /// <returns>An <see cref="AssetInfo" /> with the data from this instance, or null.</returns>
        public override AssetInfo ToAssetInfo()
        {
            return new CameraInfo(AssetId, Capability, AssetType, IPAddress)
            {
                Description = $"Camera at {IPAddress}"
            };
        }
    }
}
