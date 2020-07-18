using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A framework client VM tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{FrameworkClientHostName,nq}")]
    public class FrameworkClient
    {
        /// <summary>
        /// Gets or sets the framework client host name.
        /// </summary>
        public string FrameworkClientHostName { get; set; }

        /// <summary>
        /// Gets or sets the VM power state.
        /// </summary>
        public string PowerState { get; set; }

        /// <summary>
        /// Gets or sets the usage state of the client VM.
        /// </summary>
        public string UsageState { get; set; }

        /// <summary>
        /// Gets or sets the ID of the session that is using this client VM.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the environment that is currently using the VM.
        /// </summary>
        public string Environment { get; set; }

        /// <summary>
        /// Gets or sets the platform for which the VM is currently used.
        /// </summary>
        public string PlatformUsage { get; set; }

        /// <summary>
        /// Gets or sets the hold ID.
        /// </summary>
        public string HoldId { get; set; }

        /// <summary>
        /// Gets or sets an ordinal for sorting.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the time when the state of the VM was last synchronized with asset inventory.
        /// </summary>
        public DateTime LastUpdated { get; set; }

        /// <summary>
        /// Gets or sets the collection of platforms associated with this client.
        /// </summary>
        public virtual ICollection<FrameworkClientPlatform> Platforms { get; set; } = new HashSet<FrameworkClientPlatform>();
    }
}
