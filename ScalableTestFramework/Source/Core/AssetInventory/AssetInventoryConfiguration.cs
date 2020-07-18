using System.Collections.ObjectModel;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// Contains additional configuration used when interfacing with the AssetInventory database.
    /// </summary>
    public sealed class AssetInventoryConfiguration
    {
        /// <summary>
        /// Gets a collection of allowed <see cref="AssetPool" /> names from which assets can be selected.
        /// </summary>
        public Collection<string> AssetPools { get; } = new Collection<string>();
    }
}
