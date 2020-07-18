using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// A remote print queue tracked by Asset Inventory.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class RemotePrintQueue
    {
        /// <summary>
        /// Gets or sets the unique identifier for the print queue.
        /// </summary>
        public Guid RemotePrintQueueId { get; set; }

        /// <summary>
        /// Gets or sets the ID of the print server hosting this queue.
        /// </summary>
        public Guid PrintServerId { get; set; }

        /// <summary>
        /// Gets or sets the print queue name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the ID of the printer associated with this queue.
        /// </summary>
        public string PrinterId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="RemotePrintQueue" /> is active.
        /// </summary>
        public bool Active { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="FrameworkServer" /> representing the print server hosting this queue.
        /// </summary>
        public virtual FrameworkServer PrintServer { get; set; }
    }
}
