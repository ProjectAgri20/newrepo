using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// A minimal identifier used to save <see cref="RemotePrintQueueInfo" />.
    /// </summary>
    [DataContract]
    public class RemotePrintQueueDefinition : PrintQueueDefinition
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Guid _printQueueId;

        /// <summary>
        /// Gets the unique identifier for the print queue.
        /// </summary>
        public Guid PrintQueueId => _printQueueId;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePrintQueueDefinition" /> class.
        /// </summary>
        /// <param name="printQueueId">The print queue identifier.</param>
        public RemotePrintQueueDefinition(Guid printQueueId)
        {
            _printQueueId = printQueueId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePrintQueueDefinition" /> class.
        /// </summary>
        /// <param name="printQueueInfo">The <see cref="RemotePrintQueueInfo" /> whose information should be stored.</param>
        /// <exception cref="ArgumentNullException"><paramref name="printQueueInfo" /> is null.</exception>
        public RemotePrintQueueDefinition(RemotePrintQueueInfo printQueueInfo)
        {
            if (printQueueInfo == null)
            {
                throw new ArgumentNullException(nameof(printQueueInfo));
            }

            _printQueueId = printQueueInfo.PrintQueueId;
        }
    }
}
