using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// An authentication-capable badge tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{BadgeId,nq}")]
    public sealed class Badge
    {
        /// <summary>
        /// Gets or sets the unique identifier for the badge.
        /// </summary>
        public string BadgeId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the badge box this badge is connected to (if any).
        /// </summary>
        public string BadgeBoxId { get; set; }

        /// <summary>
        /// Gets or sets the user name associated with this badge.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the badge index in the badge box.
        /// </summary>
        public byte Index { get; set; }

        /// <summary>
        /// Gets or sets the badge box description.
        /// </summary>
        public string Description { get; set; }
    }
}
