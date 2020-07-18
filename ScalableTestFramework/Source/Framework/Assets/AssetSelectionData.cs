using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Data used for selecting assets for use in a test.
    /// </summary>
    [DataContract]
    public sealed class AssetSelectionData
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly AssetIdCollection _selectedAssets;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly AssetIdCollection _inactiveAssets;

        /// <summary>
        /// Gets the IDs of the selected assets.
        /// </summary>
        public AssetIdCollection SelectedAssets => _selectedAssets;

        /// <summary>
        /// Gets the IDs of the inactive assets.
        /// </summary>
        public AssetIdCollection InactiveAssets => _inactiveAssets;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionData" /> class.
        /// </summary>
        public AssetSelectionData()
        {
            _selectedAssets = new AssetIdCollection();
            _inactiveAssets = new AssetIdCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionData" /> class.
        /// </summary>
        /// <param name="selectedAsset">The <see cref="AssetInfo" /> for the single selected asset.</param>
        /// <exception cref="ArgumentNullException"><paramref name="selectedAsset" /> is null.</exception>
        public AssetSelectionData(AssetInfo selectedAsset)
        {
            if (selectedAsset == null)
            {
                throw new ArgumentNullException(nameof(selectedAsset));
            }

            _selectedAssets = new AssetIdCollection(new List<AssetInfo> { selectedAsset });
            _inactiveAssets = new AssetIdCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionData" /> class.
        /// </summary>
        /// <param name="selectedAssets">A collection of <see cref="AssetInfo" /> for the selected assets.</param>
        /// <exception cref="ArgumentNullException"><paramref name="selectedAssets" /> is null.</exception>
        public AssetSelectionData(IEnumerable<AssetInfo> selectedAssets)
        {
            _selectedAssets = new AssetIdCollection(selectedAssets);
            _inactiveAssets = new AssetIdCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetSelectionData" /> class.
        /// </summary>
        /// <param name="selectedAssets">A collection of <see cref="AssetInfo" /> for the selected assets.</param>
        /// <param name="inactiveAssets">A collection of <see cref="AssetInfo" /> for the inactive assets.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="selectedAssets" /> is null.
        /// <para>or</para>
        /// <paramref name="inactiveAssets" /> is null.
        /// </exception>
        public AssetSelectionData(IEnumerable<AssetInfo> selectedAssets, IEnumerable<AssetInfo> inactiveAssets)
        {
            _selectedAssets = new AssetIdCollection(selectedAssets);
            _inactiveAssets = new AssetIdCollection(inactiveAssets);
        }
    }
}
