using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Data used for selecting servers for use in a test.
    /// </summary>
    [DataContract]
    public sealed class ServerSelectionData
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ServerIdCollection _selectedServers;

        /// <summary>
        /// Gets the IDs of the selected servers.
        /// </summary>
        public ServerIdCollection SelectedServers => _selectedServers;

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSelectionData" /> class.
        /// </summary>
        public ServerSelectionData()
        {
            _selectedServers = new ServerIdCollection();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSelectionData" /> class.
        /// </summary>
        /// <param name="selectedServer">The <see cref="ServerInfo" /> for the single selected server.</param>
        /// <exception cref="ArgumentNullException"><paramref name="selectedServer" /> is null.</exception>
        public ServerSelectionData(ServerInfo selectedServer)
        {
            if (selectedServer == null)
            {
                throw new ArgumentNullException(nameof(selectedServer));
            }

            _selectedServers = new ServerIdCollection(new List<ServerInfo> { selectedServer });
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ServerSelectionData" /> class.
        /// </summary>
        /// <param name="selectedServers">A collection of <see cref="ServerInfo" /> for the selected servers.</param>
        /// <exception cref="ArgumentNullException"><paramref name="selectedServers" /> is null.</exception>
        public ServerSelectionData(IEnumerable<ServerInfo> selectedServers)
        {
            _selectedServers = new ServerIdCollection(selectedServers);
        }
    }
}
