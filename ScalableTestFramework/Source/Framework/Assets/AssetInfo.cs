using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Base class for information about an asset used in a test.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{AssetId,nq}")]
    public abstract class AssetInfo : IAssetInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _assetId;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly AssetAttributes _attributes;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _assetType;

        /// <summary>
        /// Gets the unique identifier for the asset.
        /// </summary>
        public string AssetId => _assetId;

        /// <summary>
        /// Gets the <see cref="AssetAttributes" /> for the asset.
        /// </summary>
        public AssetAttributes Attributes => _attributes;

        /// <summary>
        /// Gets the type of the asset.
        /// </summary>
        public string AssetType => _assetType;

        /// <summary>
        /// Gets a description of the asset.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets the reservation key for the asset.
        /// </summary>
        public string ReservationKey { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInfo" /> class.
        /// </summary>
        /// <param name="assetId">The unique identifier for the asset.</param>
        /// <param name="attributes">The <see cref="AssetAttributes" /> for the asset.</param>
        /// <param name="assetType">The asset type.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetId" /> is null.</exception>
        protected AssetInfo(string assetId, AssetAttributes attributes, string assetType)
        {
            _assetId = assetId ?? throw new ArgumentNullException(nameof(assetId));
            _attributes = attributes;
            _assetType = assetType;
        }
    }
}
