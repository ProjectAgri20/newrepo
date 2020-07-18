using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// An <see cref="AssetHost" /> that represents any mobile device used within a test session.
    /// </summary>
    public class MobileDeviceHost : AssetHost
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MobileDeviceHost" /> class.
        /// </summary>
        /// <param name="asset">The asset.</param>
        public MobileDeviceHost(AssetDetail asset)
            : base(asset, asset.AssetId, ElementType.Device, "MobileDevice")
        {
        }
    }
}
