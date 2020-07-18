using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Base class for information about a print queue used in a test.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{QueueName,nq}")]
    public abstract class PrintQueueInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _queueName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _associatedAssetId;

        /// <summary>
        /// Gets the name of this print queue.
        /// </summary>
        public string QueueName => _queueName;

        /// <summary>
        /// Gets the asset identifier for an asset associated with this print queue.
        /// If there is no associated asset, returns null.
        /// </summary>
        public string AssociatedAssetId => _associatedAssetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueInfo" /> class.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="queueName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="queueName" /> is an empty string.</exception>
        protected PrintQueueInfo(string queueName)
            : this(queueName, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueInfo" /> class.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <param name="associatedAssetId">The asset identifier for the asset associated with this print queue.</param>
        /// <exception cref="ArgumentNullException"><paramref name="queueName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="queueName" /> is an empty string.</exception>
        protected PrintQueueInfo(string queueName, string associatedAssetId)
        {
            if (queueName == null)
            {
                throw new ArgumentNullException(nameof(queueName));
            }

            if (string.IsNullOrWhiteSpace(queueName))
            {
                throw new ArgumentException("Queue name cannot be an empty string.", nameof(queueName));
            }

            _queueName = queueName;
            _associatedAssetId = associatedAssetId;
        }

        /// <summary>
        /// Creates a new <see cref="PrintQueueDefinition" /> representing this instance.
        /// </summary>
        /// <returns>A <see cref="PrintQueueDefinition" /> that represents this instance.</returns>
        public abstract PrintQueueDefinition CreateDefinition();
    }
}
