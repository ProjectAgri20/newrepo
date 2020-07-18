using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A grouping of <see cref="Asset" /> information.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class AssetPool
    {
        /// <summary>
        /// Gets or sets the pool name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the pool administrator.
        /// </summary>
        public string Administrator { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to track reservation history for this pool.
        /// </summary>
        public bool? TrackReservations { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Asset" /> objects in this pool.
        /// </summary>
        public virtual ICollection<Asset> Assets { get; set; } = new HashSet<Asset>();
    }
}
