using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A badge box tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{BadgeBoxId,nq}")]
    public class BadgeBox
    {
        /// <summary>
        /// Gets or sets the unique identifier for the badge box.
        /// </summary>
        public string BadgeBoxId { get; set; }

        /// <summary>
        /// Gets or sets the badge box network address.
        /// </summary>
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the asset ID of the associated print device.
        /// </summary>
        public string PrinterId { get; set; }

        /// <summary>
        /// Gets or sets the badge box description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="Badge" /> objects connected to this badge box.
        /// </summary>
        public virtual ICollection<Badge> Badges { get; set; } = new HashSet<Badge>();
    }
}
