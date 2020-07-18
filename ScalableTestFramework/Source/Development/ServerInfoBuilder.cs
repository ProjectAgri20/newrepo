using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Builder class for creating mock <see cref="ServerInfo" /> objects.
    /// </summary>
    public sealed class ServerInfoBuilder
    {
        /// <summary>
        /// Gets or sets the address to be applied to constructed <see cref="ServerInfo" /> objects.
        /// </summary>
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the operating system to be applied to constructed <see cref="ServerInfo" /> objects.
        /// </summary>
        public string OperatingSystem { get; set; } = "Operating System";

        /// <summary>
        /// Gets or sets the architecture to be applied to constructed <see cref="ServerInfo" /> objects.
        /// </summary>
        public string Architecture { get; set; } = "x64";

        /// <summary>
        /// Gets or sets the number of processors to be applied to constructed <see cref="ServerInfo" /> objects.
        /// </summary>
        public int Processors { get; set; } = 1;

        /// <summary>
        /// Gets or sets the number of cores to be applied to constructed <see cref="ServerInfo" /> objects.
        /// </summary>
        public int Cores { get; set; } = 1;

        /// <summary>
        /// Gets or sets the memory to be applied to constructed <see cref="ServerInfo" /> objects.
        /// </summary>
        public int Memory { get; set; } = 512;

        /// <summary>
        /// Gets the server settings to be applied to constructed <see cref="ServerInfo" /> objects.
        /// </summary>
        public Dictionary<string, string> Settings { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerInfoBuilder" /> class.
        /// </summary>
        public ServerInfoBuilder()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Builds the a generic <see cref="ServerInfo" /> object with the specified hostname
        /// and the properties specified in this <see cref="ServerInfoBuilder" />.
        /// </summary>
        /// <param name="hostName">The host name.</param>
        /// <returns>A generic <see cref="ServerInfo" /> object with the specified hostname.</returns>
        public ServerInfo BuildServer(string hostName)
        {
            return BuildServer(hostName, Guid.NewGuid());
        }

        /// <summary>
        /// Builds the a generic <see cref="ServerInfo" /> object with the specified hostname and ID
        /// and the properties specified in this <see cref="ServerInfoBuilder" />.
        /// </summary>
        /// <param name="hostName">The host name.</param>
        /// <param name="serverId">The server identifier.</param>
        /// <returns>A generic <see cref="ServerInfo" /> object with the specified hostname and ID.</returns>
        public ServerInfo BuildServer(string hostName, Guid serverId)
        {
            return new ServerInfo(serverId, hostName, Address, OperatingSystem, Architecture, Processors, Cores, Memory, Settings);
        }

        /// <summary>
        /// Builds a list of generic <see cref="ServerInfo" /> objects with the specified hostnames
        /// and the properties specified in this <see cref="ServerInfoBuilder" />.
        /// </summary>
        /// <param name="hostNames">The host names.</param>
        /// <returns>A list of generic <see cref="ServerInfo" /> objects with the specified hostnames.</returns>
        public IEnumerable<ServerInfo> BuildServers(params string[] hostNames)
        {
            return hostNames.Select(BuildServer);
        }
    }
}
