using System;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// 
    /// </summary>
    public class VirtualResourceEventBusAssetArgs : EventArgs
    {
        /// <summary>
        /// Gets the Asset Id
        /// </summary>
        public string AssetId { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="assetId"></param>
        public VirtualResourceEventBusAssetArgs(string assetId)
        {
            AssetId = assetId;
        }
    }
}
