using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// Contains extension methods for queries returned from <see cref="AssetInventoryContext" />.
    /// </summary>
    public static class AssetInventoryQueryExtension
    {
        /// <summary>
        /// Creates an <see cref="AssetInfoCollection" /> from an <see cref="Asset" /> query.
        /// </summary>
        /// <param name="assetQuery">The <see cref="Asset" /> query.</param>
        /// <returns>An <see cref="AssetInfoCollection" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assetQuery" /> is null.</exception>
        public static AssetInfoCollection ToAssetInfoCollection(this IQueryable<Asset> assetQuery)
        {
            if (assetQuery == null)
            {
                throw new ArgumentNullException(nameof(assetQuery));
            }

            IEnumerable<AssetInfo> assets = assetQuery.Include(n => n.Reservations)
                                                      .AsEnumerable()
                                                      .Select(n => n.ToAssetInfo())
                                                      .Where(n => n != null);

            return new AssetInfoCollection(assets.ToList());
        }

        /// <summary>
        /// Creates a <see cref="ServerInfoCollection" /> from a <see cref="FrameworkServer" /> query.
        /// </summary>
        /// <param name="serverQuery">The <see cref="FrameworkServer" /> query.</param>
        /// <returns>A <see cref="ServerInfoCollection" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="serverQuery" /> is null.</exception>
        public static ServerInfoCollection ToServerInfoCollection(this IQueryable<FrameworkServer> serverQuery)
        {
            if (serverQuery == null)
            {
                throw new ArgumentNullException(nameof(serverQuery));
            }

            IEnumerable<ServerInfo> servers = serverQuery.Select(n => new
            {
                n.FrameworkServerId,
                n.HostName,
                n.IPAddress,
                n.OperatingSystem,
                n.Architecture,
                n.Processors,
                n.Cores,
                n.Memory,
                ServerSettings = n.ServerSettings.Select(m => new { m.Name, m.Value })
            })
            .AsEnumerable()
            .Select(n => new ServerInfo
            (
                n.FrameworkServerId,
                n.HostName,
                n.IPAddress,
                n.OperatingSystem,
                n.Architecture,
                n.Processors,
                n.Cores,
                n.Memory,
                n.ServerSettings.ToDictionary(m => m.Name, m => m.Value)
            ));

            return new ServerInfoCollection(servers.ToList());
        }

        /// <summary>
        /// Creates a collection of <see cref="RemotePrintQueueInfo" /> from a <see cref="RemotePrintQueue" /> query.
        /// </summary>
        /// <param name="remotePrintQueueQuery">The <see cref="RemotePrintQueue" /> query.</param>
        /// <returns>A collection of <see cref="RemotePrintQueueInfo" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="remotePrintQueueQuery" /> is null.</exception>
        public static IEnumerable<RemotePrintQueueInfo> ToRemotePrintQueueInfoCollection(this IQueryable<RemotePrintQueue> remotePrintQueueQuery)
        {
            if (remotePrintQueueQuery == null)
            {
                throw new ArgumentNullException(nameof(remotePrintQueueQuery));
            }

            return remotePrintQueueQuery.Select(n => new
            {
                n.RemotePrintQueueId,
                n.Name,
                n.PrinterId,
                n.PrintServerId,
                n.PrintServer.HostName
            })
            .AsEnumerable()
            .Select(n => new RemotePrintQueueInfo
            (
                n.RemotePrintQueueId,
                n.Name,
                BuildServerInfo(n.PrintServerId, n.HostName),
                n.PrinterId
            )).ToList();
        }

        private static ServerInfo BuildServerInfo(Guid serverId, string hostName)
        {
            // RemotePrintQueueInfo constructor requires a ServerInfo, but only uses the server ID and host name
            return new ServerInfo(serverId, hostName, null, null, null, 0, 0, 0, new Dictionary<string, string>());
        }

        /// <summary>
        /// Creates a collection of <see cref="PrintDriverInfo" /> from a <see cref="PrintDriver" /> query.
        /// </summary>
        /// <param name="printDriverQuery">The <see cref="PrintDriver" /> query.</param>
        /// <returns>A collection of <see cref="PrintDriverInfo" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="printDriverQuery" /> is null.</exception>
        public static IEnumerable<PrintDriverInfo> ToPrintDriverInfoCollection(this IQueryable<PrintDriver> printDriverQuery)
        {
            if (printDriverQuery == null)
            {
                throw new ArgumentNullException(nameof(printDriverQuery));
            }

            return printDriverQuery.Select(n => new
            {
                n.PrintDriverId,
                DriverName = n.Name,
                PackageName = n.PrintDriverPackage.Name,
                n.PrintProcessor,
                n.PrintDriverPackage.InfX86,
                n.PrintDriverPackage.InfX64
            })
            .AsEnumerable()
            .Select(n => new PrintDriverInfo
            (
                n.PrintDriverId,
                n.DriverName,
                n.PackageName,
                n.PrintProcessor,
                n.InfX86,
                n.InfX64
            )).ToList();
        }

        /// <summary>
        /// Creates a collection of <see cref="BadgeBoxInfo" /> from a <see cref="BadgeBox" /> query.
        /// </summary>
        /// <param name="badgeBoxQuery">The <see cref="BadgeBox" /> query.</param>
        /// <returns>A collection of <see cref="PrintDriverInfo" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="badgeBoxQuery" /> is null.</exception>
        public static IEnumerable<BadgeBoxInfo> ToBadgeBoxInfoCollection(this IQueryable<BadgeBox> badgeBoxQuery)
        {
            if (badgeBoxQuery == null)
            {
                throw new ArgumentNullException(nameof(badgeBoxQuery));
            }

            return badgeBoxQuery.Include(n => n.Badges).Select(n => new
            {
                n.BadgeBoxId,
                n.IPAddress,
                n.PrinterId,
                Badges = n.Badges.OrderBy(m => m.Index).Select(m => new
                {
                    m.BadgeId,
                    m.UserName,
                    m.Index,
                    m.Description
                })
            })
            .AsEnumerable()
            .Select(n => new BadgeBoxInfo
            (
                n.BadgeBoxId,
                AssetAttributes.None,
                "BadgeBox",
                n.IPAddress,
                n.PrinterId,
                n.Badges.Select(m => new BadgeInfo
                (
                    m.BadgeId,
                    m.UserName,
                    m.Index,
                    m.Description
                )).ToList()
            )).ToList();
        }
    }
}
