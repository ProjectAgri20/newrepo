using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// A collection of server IDs.
    /// </summary>
    [DataContract]
    public sealed class ServerIdCollection : Collection<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerIdCollection" /> class.
        /// </summary>
        public ServerIdCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerIdCollection" /> class.
        /// </summary>
        /// <param name="serverIds">The server IDs to include in the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="serverIds" /> is null.</exception>
        public ServerIdCollection(IEnumerable<Guid> serverIds)
        {
            if (serverIds == null)
            {
                throw new ArgumentNullException(nameof(serverIds));
            }

            foreach (Guid id in serverIds)
            {
                Add(id);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerIdCollection" /> class.
        /// </summary>
        /// <param name="servers">The <see cref="ServerInfo" /> objects whose IDs will be included in the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="servers" /> is null.</exception>
        public ServerIdCollection(IEnumerable<ServerInfo> servers)
        {
            if (servers == null)
            {
                throw new ArgumentNullException(nameof(servers));
            }

            foreach (ServerInfo server in servers.Where(n => n != null))
            {
                Add(server.ServerId);
            }
        }
    }
}
