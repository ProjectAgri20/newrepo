using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// A read-only collection of <see cref="ServerInfo" /> objects.
    /// </summary>
    public sealed class ServerInfoCollection : ReadOnlyCollection<ServerInfo>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ServerInfoCollection" /> class.
        /// </summary>
        /// <param name="servers">The list of <see cref="ServerInfo" /> objects to wrap.</param>
        /// <exception cref="ArgumentNullException"><paramref name="servers" /> is null.</exception>
        public ServerInfoCollection(IList<ServerInfo> servers)
            : base(servers)
        {
        }
    }
}
