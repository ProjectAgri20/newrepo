using System;

namespace HP.ScalableTest.UI.ScenarioConfiguration
{
    /// <summary>
    /// Property class for changing print queues used in a scenario
    /// </summary>
    public class BulkPrintQueueEnt
    {
        public bool Active { get; set; }
        /// <summary>
        /// Gets or sets the current queue used in a scenario.
        /// </summary>
        /// <value>
        /// The current queue.
        /// </value>
        public string NewHostName { get; set; }
        public string OldHostName { get; set; }
        public string CurrentQueue { get; set; }
        /// <summary>
        /// Gets or sets a value indicating whether [queue changed].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [queue changed]; otherwise, <c>false</c>.
        /// </value>
        public bool QueueChanged { get; set; }
        /// <summary>
        /// Gets or sets the new queue for the scenario.
        /// </summary>
        /// <value>
        /// The new queue.
        /// </value>
        public string NewQueue { get; set; }

        /// <summary>
        /// Gets or sets the virtual meta data identifier. This is part of the PK in MetadataResourceUsage
        /// </summary>
        /// <value>
        /// The virtual meta data identifier.
        /// </value>
        public Guid VirtualResourceMetadataId { get; set; }

        public BulkPrintQueueEnt()
        {
            Active = false;
            NewHostName = string.Empty;
            OldHostName = string.Empty;
            CurrentQueue = string.Empty;
            QueueChanged = false;
            NewQueue = string.Empty;
            VirtualResourceMetadataId = Guid.Empty;
        }
    }
}
