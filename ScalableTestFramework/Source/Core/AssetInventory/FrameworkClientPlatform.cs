using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A platform for a framework client.
    /// </summary>
    [DebuggerDisplay("{FrameworkClientPlatformId,nq}")]
    public sealed class FrameworkClientPlatform
    {
        /// <summary>
        /// Gets or sets the platform identifier.
        /// </summary>
        public string FrameworkClientPlatformId { get; set; }

        /// <summary>
        /// Gets or sets the platform name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="FrameworkClientPlatform" /> is active.
        /// </summary>
        public bool Active { get; set; }
    }
}
