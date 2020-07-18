using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a local print queue used in a test.
    /// </summary>
    [DataContract]
    public class LocalPrintQueueInfo : PrintQueueInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueInfo" /> class.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <exception cref="ArgumentNullException"><paramref name="queueName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="queueName" /> is an empty string.</exception>
        public LocalPrintQueueInfo(string queueName)
            : base(queueName)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueInfo" /> class.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <param name="associatedAssetId">The asset identifier for the asset associated with this print queue.</param>
        /// <exception cref="ArgumentNullException"><paramref name="queueName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="queueName" /> is an empty string.</exception>
        public LocalPrintQueueInfo(string queueName, string associatedAssetId)
            : base(queueName, associatedAssetId)
        {
        }

        /// <summary>
        /// Creates a new <see cref="PrintQueueDefinition" /> representing this instance.
        /// </summary>
        /// <returns>A <see cref="PrintQueueDefinition" /> that represents this instance.</returns>
        public override PrintQueueDefinition CreateDefinition() => new LocalPrintQueueDefinition(this);
    }
}
