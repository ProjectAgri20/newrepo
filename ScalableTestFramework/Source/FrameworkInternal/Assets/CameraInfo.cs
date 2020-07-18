using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a camera asset.
    /// </summary>
    [DataContract]
    public class CameraInfo : AssetInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _address;

        /// <summary>
        /// Gets the network address (IP or hostname) of the camera.
        /// </summary>
        public string Address => _address;

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraInfo" /> class.
        /// </summary>
        /// <param name="assetId">The unique identifier for the camera.</param>
        /// <param name="attributes">The <see cref="AssetAttributes" /> for the device.</param>
        /// <param name="assetType">The asset type.</param>
        /// <param name="address">The network address of the camera.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetId" /> is null.
        /// <para>or</para>
        /// <paramref name="address" /> is null.
        /// </exception>
        public CameraInfo(string assetId, AssetAttributes attributes, string assetType, string address)
            : base(assetId, attributes, assetType)
        {
            _address = address ?? throw new ArgumentNullException(nameof(address));
        }
    }
}
