using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// Implementation of <see cref="IAssetInventory" /> that retrieves information from a database using an <see cref="AssetInventoryContext" />.
    /// </summary>
    public sealed class AssetInventoryController : IAssetInventory, IAssetInventoryInternal
    {
        private readonly AssetInventoryConnectionString _connectionString;
        private readonly AssetInventoryConfiguration _configuration;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInventoryController" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="AssetInventoryConnectionString" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public AssetInventoryController(AssetInventoryConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInventoryController" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="AssetInventoryConnectionString" />.</param>
        /// <param name="configuration">The <see cref="AssetInventoryConfiguration" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="connectionString" /> is null.
        /// <para>or</para>
        /// <paramref name="configuration" /> is null.
        /// </exception>
        public AssetInventoryController(AssetInventoryConnectionString connectionString, AssetInventoryConfiguration configuration)
            : this(connectionString)
        {
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for all assets in the inventory.
        /// </summary>
        /// <returns><see cref="AssetInfo" /> for all assets in the inventory.</returns>
        public AssetInfoCollection GetAssets()
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                IQueryable<Asset> assets = _configuration?.AssetPools.Any() == true ?
                                           context.Assets.Where(n => _configuration.AssetPools.Contains(n.Pool.Name)) :
                                           context.Assets;

                return assets.ToAssetInfoCollection();
            }
        }

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for assets with all of the specified <see cref="AssetAttributes" />.
        /// </summary>
        /// <param name="attributes">The required attributes of the assets to retrieve.</param>
        /// <returns><see cref="AssetInfo" /> for each asset with all of the specified attributes.</returns>
        public AssetInfoCollection GetAssets(AssetAttributes attributes)
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                IQueryable<Asset> assets = _configuration?.AssetPools.Any() == true ?
                                           context.Assets.Where(n => _configuration.AssetPools.Contains(n.Pool.Name)) :
                                           context.Assets;

                return assets.Where(n => n.Capability.HasFlag(attributes)).ToAssetInfoCollection();
            }
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

            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                return context.Assets.Where(n => assetIds.Contains(n.AssetId)).ToAssetInfoCollection();
            }
        }

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for the asset with the specified ID.
        /// </summary>
        /// <param name="assetId">The asset ID.</param>
        /// <returns><see cref="AssetInfo" /> for the asset with the specified ID.</returns>
        public AssetInfo GetAsset(string assetId)
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                return context.Assets.FirstOrDefault(n => n.AssetId == assetId).ToAssetInfo();
            }
        }

        /// <summary>
        /// Retrieves <see cref="ServerInfo" /> for all servers in the inventory.
        /// </summary>
        /// <returns><see cref="ServerInfo" /> for all servers in the inventory.</returns>
        public ServerInfoCollection GetServers()
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                return context.FrameworkServers.Where(n => n.Active).ToServerInfoCollection();
            }
        }

        /// <summary>
        /// Retrieves <see cref="ServerInfo" /> for servers with the specified server attribute.
        /// </summary>
        /// <param name="attribute">The required attribute of the servers to retrieve.</param>
        /// <returns><see cref="ServerInfo" /> for each server with the specified attribute.</returns>
        public ServerInfoCollection GetServers(string attribute)
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                return context.FrameworkServers.Where(n => n.Active && n.ServerTypes.Any(m => m.Name == attribute)).ToServerInfoCollection();
            }
        }

        /// <summary>
        /// Retrieves <see cref="ServerInfo" /> for servers with any of the specified server attributes.
        /// </summary>
        /// <param name="attributes">The required attributes of the servers to retrieve.</param>
        /// <returns><see cref="ServerInfo" /> for each server with any of the specified attributes.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="attributes" /> is null.</exception>
        public ServerInfoCollection GetServers(IEnumerable<string> attributes)
        {
            if (attributes == null)
            {
                throw new ArgumentNullException(nameof(attributes));
            }

            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                return context.FrameworkServers.Where(n => n.Active && n.ServerTypes.Any(m => attributes.Contains(m.Name))).ToServerInfoCollection();
            }
        }

        /// <summary>
        /// Retrieves <see cref="RemotePrintQueueInfo" /> for all remote print queues in the inventory.
        /// </summary>
        /// <returns><see cref="RemotePrintQueueInfo" /> for all remote print queues in the inventory.</returns>
        public IEnumerable<RemotePrintQueueInfo> GetRemotePrintQueues()
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                return context.RemotePrintQueues.ToRemotePrintQueueInfoCollection();
            }
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

            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                return context.RemotePrintQueues.Where(n => n.PrintServerId == server.ServerId).ToRemotePrintQueueInfoCollection();
            }
        }

        /// <summary>
        /// Retrieves <see cref="PrintDriverInfo" /> for all print drivers in the inventory.
        /// </summary>
        /// <returns><see cref="PrintDriverInfo" /> for all print drivers in the inventory.</returns>
        public IEnumerable<PrintDriverInfo> GetPrintDrivers()
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                return context.PrintDrivers.ToPrintDriverInfoCollection();
            }
        }

        /// <summary>
        /// Retrieves all print driver configurations in the inventory.
        /// </summary>
        /// <returns>All print driver configurations in the inventory.</returns>
        public IEnumerable<string> GetPrintDriverConfigurations()
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                return context.PrintDriverConfigs.Select(n => n.ConfigFile).ToList();
            }
        }

        /// <summary>
        /// Retrieves print driver configurations that match the specified device and driver.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="driver">The driver.</param>
        /// <returns>Print driver configurations for the specified device and driver.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="device" /> is null.
        /// <para>or</para>
        /// <paramref name="driver" /> is null.
        /// </exception>
        public IEnumerable<string> GetPrintDriverConfigurations(IDeviceInfo device, PrintDriverInfo driver)
        {
            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            if (driver == null)
            {
                throw new ArgumentNullException(nameof(driver));
            }

            // Extract the version value out of the selected package's version
            string version = driver.PackageName.Split('\\').First();

            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                return context.PrintDriverConfigs.Where(n => n.PrintDriverProducts.Any(m => m.Name == device.ProductName)
                                                          && n.PrintDriverVersions.Any(m => m.Value == version))
                                                 .Select(n => n.ConfigFile).ToList();
            }
        }
    }
}
