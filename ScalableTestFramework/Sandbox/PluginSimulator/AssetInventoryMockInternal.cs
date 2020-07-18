using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Utility;
using PluginSimulator.Properties;

namespace PluginSimulator
{
    internal sealed class AssetInventoryMockInternal : IAssetInventory, IAssetInventoryInternal
    {
        /// <summary>
        /// Gets or sets the hostname or address of the database to connect to.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for all assets in the inventory.
        /// </summary>
        /// <returns><see cref="AssetInfo" /> for all assets in the inventory.</returns>
        public AssetInfoCollection GetAssets()
        {
            List<AssetInfo> assets = new List<AssetInfo>();

            DatabaseQuery(Resources.SelectPrinters, record =>
            {
                PrintDeviceInfoInternal deviceInfo = new PrintDeviceInfoInternal
                (
                    (string)record["AssetId"],
                    (AssetAttributes)record["Attributes"],
                    record["PrinterType"] as string,
                    record["Address"] as string,
                    record["Address2"] as string,
                    record["Password"] as string,
                    record["Product"] as string,
                    (int)record["PortNumber"],
                    (bool)record["SnmpEnabled"]
                );

                deviceInfo.Description = $"{deviceInfo.ProductName} ({(string)record["Model"]}) at {deviceInfo.Address}";
                deviceInfo.ModelName = record["ModelName"] as string;
                deviceInfo.ModelNumber = record["ModelNumber"] as string;
                deviceInfo.SerialNumber = record["SerialNumber"] as string;

                assets.Add(deviceInfo);
            });

            DatabaseQuery(Resources.SelectSimulators, record =>
            {
                DeviceSimulatorInfo deviceSimulatorInfo = new DeviceSimulatorInfo
                (
                    (string)record["AssetId"],
                    (AssetAttributes)record["Attributes"],
                    "DeviceSimulator",
                    record["Address"] as string,
                    record["Password"] as string,
                    record["Product"] as string,
                    record["SimulatorType"] as string
                );

                deviceSimulatorInfo.Description = $"{deviceSimulatorInfo.SimulatorType} device simulator at {deviceSimulatorInfo.Address}";
                assets.Add(deviceSimulatorInfo);
            });

            DatabaseQuery(Resources.SelectVirtualPrinters, record =>
            {
                VirtualPrinterInfo virtualPrinterInfo = new VirtualPrinterInfo
                (
                    (string)record["AssetId"],
                    (AssetAttributes)record["Attributes"],
                    "VirtualPrinter",
                    record["Address"] as string,
                    9100,
                    false
                );

                virtualPrinterInfo.Description = $"Virtual printer at {virtualPrinterInfo.Address}";
                assets.Add(virtualPrinterInfo);
            });

            DatabaseQuery(Resources.SelectMobileDevices, record =>
            {
                MobileDeviceInfo mobileDeviceInfo = new MobileDeviceInfo
                (
                    (string)record["AssetId"],
                    (AssetAttributes)record["Attributes"],
                    "MobileDevice",
                    record["MobileEquipmentId"] as string,
                    EnumUtil.Parse<MobileDeviceType>(record["MobileDeviceType"] as string, true)
                );

                mobileDeviceInfo.Description = record["Description"] as string;
                assets.Add(mobileDeviceInfo);
            });

            return new AssetInfoCollection(assets);
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
            else
            {
                return new AssetInfoCollection(GetAssets().Where(n => n.Attributes.HasFlag(attributes) && n.Attributes != AssetAttributes.None).ToList());
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

            return new AssetInfoCollection(GetAssets().Where(n => assetIds.Contains(n.AssetId)).ToList());
        }

        /// <summary>
        /// Retrieves <see cref="AssetInfo" /> for the asset with the specified ID.
        /// </summary>
        /// <param name="assetId">The asset ID.</param>
        /// <returns><see cref="AssetInfo" /> for the asset with the specified ID.</returns>
        public AssetInfo GetAsset(string assetId)
        {
            return GetAssets().FirstOrDefault(n => n.AssetId == assetId);
        }

        /// <summary>
        /// Retrieves <see cref="ServerInfo" /> for all servers in the inventory.
        /// </summary>
        /// <returns><see cref="ServerInfo" /> for all servers in the inventory.</returns>
        public ServerInfoCollection GetServers()
        {
            List<ServerInfo> servers = new List<ServerInfo>();
            var serverSettings = new Dictionary<Guid, Dictionary<string, string>>();

            DatabaseQuery(Resources.SelectServerSettings, record =>
            {
                Guid serverId = (Guid)record["FrameworkServerId"];
                if (!serverSettings.ContainsKey(serverId))
                {
                    serverSettings[serverId] = new Dictionary<string, string>();
                }
                serverSettings[serverId].Add(record["Name"] as string, record["Value"] as string);
            });

            DatabaseQuery(Resources.SelectServers, record =>
            {
                Guid serverId = (Guid)record["FrameworkServerId"];
                var settings = serverSettings.ContainsKey(serverId) ? serverSettings[serverId] : new Dictionary<string, string>();
                ServerInfo serverInfo = new ServerInfo
                (
                    serverId,
                    record["HostName"] as string,
                    record["IPAddress"] as string,
                    record["OperatingSystem"] as string,
                    record["Architecture"] as string,
                    (int)record["Processors"],
                    (int)record["Cores"],
                    (int)record["Memory"],
                    new SettingsDictionary(settings)
                );
                servers.Add(serverInfo);
            });

            return new ServerInfoCollection(servers);
        }

        /// <summary>
        /// Retrieves <see cref="ServerInfo" /> for servers with the specified server attribute.
        /// </summary>
        /// <param name="attribute">The required attribute of the servers to retrieve.</param>
        /// <returns><see cref="ServerInfo" /> for each server with the specified attribute.</returns>
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
            List<RemotePrintQueueInfo> printQueues = new List<RemotePrintQueueInfo>();
            ServerInfoCollection servers = GetServers();

            DatabaseQuery(Resources.SelectPrintQueues, record =>
            {
                RemotePrintQueueInfo printQueueInfo = new RemotePrintQueueInfo
                (
                    (Guid)record["RemotePrintQueueId"],
                    record["Name"] as string,
                    servers.First(n => n.ServerId == (Guid)record["PrintServerId"]),
                    record["AssociatedAssetId"] as string
                );
                printQueues.Add(printQueueInfo);
            });

            return printQueues;
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

            return GetRemotePrintQueues().Where(n => n.ServerId == server.ServerId);
        }

        /// <summary>
        /// Retrieves <see cref="PrintDriverInfo" /> for all print drivers in the inventory.
        /// </summary>
        /// <returns><see cref="PrintDriverInfo" /> for all print drivers in the inventory.</returns>
        public IEnumerable<PrintDriverInfo> GetPrintDrivers()
        {
            List<PrintDriverInfo> printDrivers = new List<PrintDriverInfo>();
            DatabaseQuery(Resources.SelectPrintDrivers, record =>
            {
                PrintDriverInfo driverInfo = new PrintDriverInfo
                (
                    (Guid)record["PrintDriverId"],
                    record["DriverName"] as string,
                    record["PackageName"] as string,
                    record["PrintProcessor"] as string,
                    record["InfX86"] as string,
                    record["InfX64"] as string
                );
                printDrivers.Add(driverInfo);
            });

            return printDrivers;
        }

        /// <summary>
        /// Retrieves all print driver configurations in the inventory.
        /// </summary>
        /// <returns>All print driver configurations in the inventory.</returns>
        public IEnumerable<string> GetPrintDriverConfigurations()
        {
            List<string> printDriverConfigurations = new List<string>();

            DatabaseQuery(Resources.SelectPrintDriverConfigurations, record =>
            {
                printDriverConfigurations.Add(record["ConfigFile"] as string);
            });

            return printDriverConfigurations;
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

            // No filtering in the mock
            return GetPrintDriverConfigurations();
        }

        /// <summary>
        /// Gets a collection of <see cref="BadgeBoxInfo" /> objects that are associated with one of the specified assets.
        /// </summary>
        /// <param name="assets">The assets.</param>
        /// <returns>A collection of <see cref="BadgeBoxInfo" /> objects.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="assets" /> is null.</exception>
        public IEnumerable<BadgeBoxInfo> GetBadgeBoxes(IEnumerable<AssetInfo> assets)
        {
            if (assets == null)
            {
                throw new ArgumentNullException(nameof(assets));
            }

            Dictionary<string, List<BadgeInfo>> badges = new Dictionary<string, List<BadgeInfo>>();
            DatabaseQuery(Resources.SelectBadges, record =>
            {
                BadgeInfo badge = new BadgeInfo
                (
                    record["BadgeId"] as string,
                    record["UserName"] as string,
                    (byte)record["Index"],
                    record["Description"] as string
                );
                string badgeBoxId = record["BadgeBoxId"] as string;
                if (!badges.ContainsKey(badgeBoxId))
                {
                    badges.Add(badgeBoxId, new List<BadgeInfo>());
                }
                badges[badgeBoxId].Add(badge);
            });

            List<BadgeBoxInfo> badgeBoxes = new List<BadgeBoxInfo>();
            DatabaseQuery(Resources.SelectBadgeBoxes, record =>
            {
                string badgeBoxId = record["BadgeBoxId"] as string;
                BadgeBoxInfo badgeBox = new BadgeBoxInfo
                (
                    badgeBoxId,
                    AssetAttributes.None,
                    "BadgeBox",
                    record["IPAddress"] as string,
                    record["PrinterId"] as string,
                    badges.ContainsKey(badgeBoxId) ? badges[badgeBoxId] : new List<BadgeInfo>()
                );
                badgeBoxes.Add(badgeBox);
            });

            return badgeBoxes.Where(n => assets.Select(m => m.AssetId).Contains(n.PrinterId));
        }

        /// <summary>
        /// Gets the external credentials for the specified domain user
        /// </summary>
        public ExternalCredentialInfoCollection GetExternalCredentials(string domainUserName)
        {
            List<ExternalCredentialInfo> externalCredentials = new List<ExternalCredentialInfo>();

            DatabaseQuery(string.Format(Resources.SelectExternalCredentials, domainUserName), record =>
            {
                ExternalCredentialInfo credential = new ExternalCredentialInfo()
                {
                    UserName = record["UserName"] as string,
                    Password = record["Password"] as string,
                    Provider = (ExternalCredentialType)Enum.Parse(typeof(ExternalCredentialType), (string)record["ExternalCredentialType"])
                };
                externalCredentials.Add(credential);
            });

            return new ExternalCredentialInfoCollection(externalCredentials);
        }

        private void DatabaseQuery(string sql, Action<IDataRecord> read)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = Database,
                InitialCatalog = "AssetInventory",
                UserID = "asset_admin",
                Password = "asset_admin",
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true
            };

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    read(reader);
                }

                reader.Close();
            }
        }
    }
}
