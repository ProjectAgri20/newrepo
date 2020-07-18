using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// A collection of asset IDs.
    /// </summary>
    [DataContract]
    public sealed class AssetIdCollection : Collection<string>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetIdCollection" /> class.
        /// </summary>
        public AssetIdCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetIdCollection" /> class.
        /// </summary>
        /// <param name="assetIds">The asset IDs to include in the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assetIds" /> is null.</exception>
        public AssetIdCollection(IEnumerable<string> assetIds)
        {
            if (assetIds == null)
            {
                throw new ArgumentNullException(nameof(assetIds));
            }

            foreach (string assetId in assetIds)
            {
                Add(assetId);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetIdCollection" /> class.
        /// </summary>
        /// <param name="assets">The <see cref="AssetInfo" /> objects whose IDs will be included in the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="assets" /> is null.</exception>
        public AssetIdCollection(IEnumerable<AssetInfo> assets)
        {
            if (assets == null)
            {
                throw new ArgumentNullException(nameof(assets));
            }

            foreach (AssetInfo asset in assets.Where(n => n != null))
            {
                Add(asset.AssetId);
            }
        }
    }
}
