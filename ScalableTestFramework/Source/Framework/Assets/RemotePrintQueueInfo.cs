using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a remote print queue used in a test.
    /// </summary>
    [DataContract]
    public class RemotePrintQueueInfo : PrintQueueInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Guid _printQueueId;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Guid _serverId;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _serverHostName;

        /// <summary>
        /// Gets the unique identifier for the print queue.
        /// </summary>
        public Guid PrintQueueId => _printQueueId;

        /// <summary>
        /// Gets the unique identifier of the server on which this print queue is hosted.
        /// </summary>
        public Guid ServerId => _serverId;

        /// <summary>
        /// Gets the name of the server on which this print queue is hosted.
        /// </summary>
        public string ServerHostName => _serverHostName;

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePrintQueueInfo" /> class.
        /// </summary>
        /// <param name="printQueueId">The print queue identifier.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="server">The server on which the print queue is hosted.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="queueName" /> is null.
        /// <para>or</para>
        /// <paramref name="server" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="queueName" /> is an empty string.</exception>
        public RemotePrintQueueInfo(Guid printQueueId, string queueName, ServerInfo server)
            : this(printQueueId, queueName, server, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintQueueInfo" /> class.
        /// </summary>
        /// <param name="printQueueId">The print queue identifier.</param>
        /// <param name="queueName">The queue name.</param>
        /// <param name="server">The server on which the print queue is hosted.</param>
        /// <param name="associatedAssetId">The asset identifier for the asset associated with this print queue.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="queueName" /> is null.
        /// <para>or</para>
        /// <paramref name="server" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="queueName" /> is an empty string.</exception>
        public RemotePrintQueueInfo(Guid printQueueId, string queueName, ServerInfo server, string associatedAssetId)
            : base(queueName, associatedAssetId)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            _printQueueId = printQueueId;
            _serverId = server.ServerId;
            _serverHostName = server.HostName;
        }

        /// <summary>
        /// Creates a new <see cref="PrintQueueDefinition" /> representing this instance.
        /// </summary>
        /// <returns>A <see cref="PrintQueueDefinition" /> that represents this instance.</returns>
        public override PrintQueueDefinition CreateDefinition() => new RemotePrintQueueDefinition(this);
    }
}
