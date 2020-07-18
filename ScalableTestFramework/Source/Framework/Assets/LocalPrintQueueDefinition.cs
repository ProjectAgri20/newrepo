using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// A minimal identifier used to save <see cref="LocalPrintQueueInfo" />.
    /// </summary>
    [DataContract]
    public class LocalPrintQueueDefinition : PrintQueueDefinition
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
        /// </summary>
        public string AssociatedAssetId => _associatedAssetId;

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalPrintQueueDefinition" /> class.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="queueName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="queueName" /> is an empty string.</exception>
        public LocalPrintQueueDefinition(string queueName)
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
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalPrintQueueDefinition" /> class.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <param name="associatedAssetId">The asset identifier for the asset associated with this print queue.</param>
        /// <exception cref="ArgumentNullException"><paramref name="queueName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="queueName" /> is an empty string.</exception>
        public LocalPrintQueueDefinition(string queueName, string associatedAssetId)
            : this(queueName)
        {
            _associatedAssetId = associatedAssetId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LocalPrintQueueDefinition" /> class.
        /// </summary>
        /// <param name="printQueueInfo">The <see cref="LocalPrintQueueInfo" /> whose information should be stored.</param>
        /// <exception cref="ArgumentNullException"><paramref name="printQueueInfo" /> is null.</exception>
        public LocalPrintQueueDefinition(LocalPrintQueueInfo printQueueInfo)
        {
            if (printQueueInfo == null)
            {
                throw new ArgumentNullException(nameof(printQueueInfo));
            }

            _queueName = printQueueInfo.QueueName;
            _associatedAssetId = printQueueInfo.AssociatedAssetId;
        }
    }
}
