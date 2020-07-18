using System;
using System.Collections.Generic;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Provides access to information stored in Asset Inventory.
    /// </summary>
    public interface IAssetInventory
    {
        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for all assets in the inventory.
        /// </summary>
        /// <returns><see cref="AssetInfo" /> for all assets in the inventory.</returns>
        AssetInfoCollection GetAssets();

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for assets with all of the specified <see cref="AssetAttributes" />.
        /// </summary>
        /// <param name="attributes">The required attributes of the assets to retrieve.</param>
        /// <returns><see cref="AssetInfo" /> for each asset with all of the specified attributes.</returns>
        AssetInfoCollection GetAssets(AssetAttributes attributes);

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for assets with one of the specified IDs.
        /// </summary>
        /// <param name="assetIds">The asset IDs.</param>
        /// <returns><see cref="AssetInfo" /> for each asset with one of the specified IDs.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetIds" /> is null.</exception>
        AssetInfoCollection GetAssets(IEnumerable<string> assetIds);

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for the asset with the specified ID.
        /// </summary>
        /// <param name="assetId">The asset ID.</param>
        /// <returns><see cref="AssetInfo" /> for the asset with the specified ID.</returns>
        AssetInfo GetAsset(string assetId);

        /// <summary>
        /// Retrieves <see cref="ServerInfo" /> for all servers in the inventory.
        /// </summary>
        /// <returns><see cref="ServerInfo" /> for all servers in the inventory.</returns>
        ServerInfoCollection GetServers();

        /// <summary>
        /// Retrieves <see cref="ServerInfo" /> for servers with the specified server attribute.
        /// </summary>
        /// <param name="attribute">The required attribute of the servers to retrieve.</param>
        /// <returns><see cref="ServerInfo" /> for each server with the specified attribute.</returns>
        ServerInfoCollection GetServers(string attribute);

        /// <summary>
        /// Retrieves <see cref="ServerInfo" /> for servers with any of the specified server attributes.
        /// </summary>
        /// <param name="attributes">The required attributes of the servers to retrieve.</param>
        /// <returns><see cref="ServerInfo" /> for each server with any of the specified attributes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attributes" /> is null.</exception>
        ServerInfoCollection GetServers(IEnumerable<string> attributes);

        /// <summary>
        /// Retrieves <see cref="RemotePrintQueueInfo" /> for all remote print queues in the inventory.
        /// </summary>
        /// <returns><see cref="RemotePrintQueueInfo" /> for all remote print queues in the inventory.</returns>
        IEnumerable<RemotePrintQueueInfo> GetRemotePrintQueues();

        /// <summary>
        /// Retrieves <see cref="RemotePrintQueueInfo" /> for all remote print queues on the specified server.
        /// </summary>
        /// <returns><see cref="RemotePrintQueueInfo" /> for all remote print queues on the specified server.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="server" /> is null.</exception>
        IEnumerable<RemotePrintQueueInfo> GetRemotePrintQueues(ServerInfo server);
    }
}
