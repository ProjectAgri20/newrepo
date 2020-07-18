namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about an asset used in a test.
    /// </summary>
    public interface IAssetInfo
    {
        /// <summary>
        /// Gets the unique identifier for the asset.
        /// </summary>
        string AssetId { get; }

        /// <summary>
        /// Gets the <see cref="AssetAttributes" /> for the asset.
        /// </summary>
        AssetAttributes Attributes { get; }

        /// <summary>
        /// Gets the type of the asset.
        /// </summary>
        string AssetType { get; }
    }
}
