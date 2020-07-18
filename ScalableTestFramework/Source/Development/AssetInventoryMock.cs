using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// A simple implementation of <see cref="IAssetInventory" /> for development.
    /// </summary>
    public sealed class AssetInventoryMock : IAssetInventory
    {
        private readonly List<AssetInfo> _assets = new List<AssetInfo>();
        private readonly List<ServerInfo> _servers = new List<ServerInfo>();
        private readonly List<RemotePrintQueueInfo> _printQueues = new List<RemotePrintQueueInfo>();
        private readonly List<BadgeBoxInfo> _badgeBoxes = new List<BadgeBoxInfo>();

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInventoryMock" /> class.
        /// </summary>
        public AssetInventoryMock()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Adds the specified asset to the inventory.
        /// </summary>
        /// <param name="asset">The asset to add to the inventory.</param>
        public void AddAsset(AssetInfo asset) => _assets.Add(asset);

        /// <summary>
        /// Adds the specified assets to the inventory.
        /// </summary>
        /// <param name="assets">The assets to add to the inventory.</param>
        public void AddAssets(IEnumerable<AssetInfo> assets) => _assets.AddRange(assets);

        /// <summary>
        /// Removes the specified asset from the inventory.
        /// </summary>
        /// <param name="asset">The asset to remove from the inventory.</param>
        public void RemoveAsset(AssetInfo asset) => _assets.Remove(asset);

        /// <summary>
        /// Removes all assets from the inventory.
        /// </summary>
        public void ClearAssets() => _assets.Clear();

        /// <summary>
        /// Adds the specified server to the inventory.
        /// </summary>
        /// <param name="server">The server to add to the inventory.</param>
        public void AddServer(ServerInfo server) => _servers.Add(server);

        /// <summary>
        /// Adds the specified servers to the inventory.
        /// </summary>
        /// <param name="servers">The servers to add to the inventory.</param>
        public void AddServers(IEnumerable<ServerInfo> servers) => _servers.AddRange(servers);

        /// <summary>
        /// Removes the specified server from the inventory.
        /// </summary>
        /// <param name="server">The server to remove from the inventory.</param>
        public void RemoveServer(ServerInfo server) => _servers.Remove(server);

        /// <summary>
        /// Removes all servers from the inventory.
        /// </summary>
        public void ClearServers() => _servers.Clear();

        /// <summary>
        /// Adds the specified print queue to the inventory.
        /// </summary>
        /// <param name="printQueue">The print queue to add to the inventory.</param>
        public void AddRemotePrintQueue(RemotePrintQueueInfo printQueue) => _printQueues.Add(printQueue);

        /// <summary>
        /// Adds the specified print queues to the inventory.
        /// </summary>
        /// <param name="printQueues">The print queues to add to the inventory.</param>
        public void AddRemotePrintQueues(IEnumerable<RemotePrintQueueInfo> printQueues) => _printQueues.AddRange(printQueues);

        /// <summary>
        /// Removes the specified print queue from the inventory.
        /// </summary>
        /// <param name="printQueue">The print queue to remove from the inventory.</param>
        public void RemoveRemotePrintQueue(RemotePrintQueueInfo printQueue) => _printQueues.Remove(printQueue);

        /// <summary>
        /// Removes all print queues from the inventory.
        /// </summary>
        public void ClearRemotePrintQueues() => _printQueues.Clear();

        /// <summary>
        /// Adds the specified badge box to the inventory.
        /// </summary>
        /// <param name="badgeBox">The badge box to add to the inventory.</param>
        public void AddBadgeBox(BadgeBoxInfo badgeBox) => _badgeBoxes.Add(badgeBox);

        /// <summary>
        /// Removes the specified badge box from the inventory.
        /// </summary>
        /// <param name="badgeBox">The badge box to remove from the inventory.</param>
        public void RemoveBadgeBox(BadgeBoxInfo badgeBox) => _badgeBoxes.Remove(badgeBox);

        /// <summary>
        /// Removes all badge boxes from the inventory.
        /// </summary>
        public void ClearBadgeBoxes() => _badgeBoxes.Clear();

        /// <summary>
        /// Gets a collection of <see cref="BadgeBoxInfo" /> objects that are associated with one of the specified assets.
        /// </summary>
        /// <param name="assets">The assets.</param>
        /// <returns>A collection of <see cref="BadgeBoxInfo" /> objects.</returns>
        internal IEnumerable<BadgeBoxInfo> GetBadgeBoxes(IEnumerable<AssetInfo> assets)
        {
            List<string> assetIds = assets.Select(n => n.AssetId).ToList();
            return _badgeBoxes.Where(n => assetIds.Contains(n.PrinterId));
        }

        #region IAssetInventory Members

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for all assets in the inventory.
        /// </summary>
        /// <returns><see cref="AssetInfo" /> for all assets in the inventory.</returns>
        public AssetInfoCollection GetAssets()
        {
            // The ToList call ensures we send a copy of the list instead of the actual inventory list
            return new AssetInfoCollection(_assets.ToList());
        }

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for assets with all of the specified <see cref="AssetAttributes" />.
        /// </summary>
        /// <param name="attributes">The required attributes of the assets to retrieve.</param>
        /// <returns><see cref="AssetInfo" /> for each asset with all of the specified attributes.</returns>
        public AssetInfoCollection GetAssets(AssetAttributes attributes)
        {
            if (attributes == AssetAttributes.None)
            {
                return GetAssets();
            }

            return new AssetInfoCollection(_assets.Where(n => n.Attributes.HasFlag(attributes)).ToList());
        }

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for assets with one of the specified IDs.
        /// </summary>
        /// <param name="assetIds">The asset IDs.</param>
        /// <returns><see cref="AssetInfo" /> for each asset with one of the specified IDs.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetIds" /> is null.</exception>
        public AssetInfoCollection GetAssets(IEnumerable<string> assetIds)
        {
            if (assetIds == null)
            {
                throw new ArgumentNullException(nameof(assetIds));
            }

            var assets = _assets.Where(n => assetIds.Contains(n.AssetId, StringComparer.OrdinalIgnoreCase));
            return new AssetInfoCollection(assets.ToList());
        }

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for the asset with the specified ID.
        /// </summary>
        /// <param name="assetId">The asset ID.</param>
        /// <returns><see cref="AssetInfo" /> for the asset with the specified ID.</returns>
        public AssetInfo GetAsset(string assetId)
        {
            return _assets.FirstOrDefault(n => n.AssetId.Equals(assetId, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Retrieves <see cref="ServerInfo" /> for all servers in the inventory.
        /// </summary>
        /// <returns><see cref="ServerInfo" /> for all servers in the inventory.</returns>
        public ServerInfoCollection GetServers()
        {
            // The ToList call ensures we send a copy of the list instead of the actual inventory list
            return new ServerInfoCollection(_servers.ToList());
        }

        /// <summary>
        /// Retrieves <see cref="ServerInfo" /> for servers with the specified server attribute.
        /// </summary>
        /// <param name="attribute">The required attribute of the servers to retrieve.</param>
        /// <returns><see cref="ServerInfo" /> for each server with the specified attribute.</returns>
        /// <remarks>
        /// This method actually returns all the servers without filtering.  This could be a future enhancement.
        /// </remarks>
        public ServerInfoCollection GetServers(string attribute)
        {
            // No filtering in the mock
            return GetServers();
        }

        /// <summary>
        /// Retrieves <see cref="ServerInfo" /> for servers with any of the specified server attributes.
        /// </summary>
        /// <param name="attributes">The required attributes of the servers to retrieve.</param>
        /// <returns><see cref="ServerInfo" /> for each server with any of the specified attributes.</returns>
        /// <remarks>
        /// This method actually returns all the servers without filtering.  This could be a future enhancement.
        /// </remarks>
        public ServerInfoCollection GetServers(IEnumerable<string> attributes)
        {
            // No filtering in the mock
            return GetServers();
        }

        /// <summary>
        /// Retrieves <see cref="RemotePrintQueueInfo" /> for all remote print queues in the inventory.
        /// </summary>
        /// <returns><see cref="RemotePrintQueueInfo" /> for all remote print queues in the inventory.</returns>
        public IEnumerable<RemotePrintQueueInfo> GetRemotePrintQueues()
        {
            // The ToList call ensures we send a copy of the list instead of the actual inventory list
            return _printQueues.ToList();
        }

        /// <summary>
        /// Retrieves <see cref="RemotePrintQueueInfo" /> for all remote print queues on the specified server.
        /// </summary>
        /// <returns><see cref="RemotePrintQueueInfo" /> for all remote print queues on the specified server.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="server" /> is null.</exception>
        public IEnumerable<RemotePrintQueueInfo> GetRemotePrintQueues(ServerInfo server)
        {
            if (server == null)
            {
                throw new ArgumentNullException(nameof(server));
            }

            return _printQueues.Where(n => n.ServerId == server.ServerId);
        }

        #endregion
    }
}
