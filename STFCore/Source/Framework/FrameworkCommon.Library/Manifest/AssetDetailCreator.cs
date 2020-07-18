using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.AssetInventory.Reservation;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Converts <see cref="TestAsset"/> instances to corresponding <see cref="AssetDetail"/> instances.
    /// </summary>
    public static class AssetDetailCreator
    {
        public static IEnumerable<AssetDetail> CreateAssetDetails(IEnumerable<AssetReservationResult> testAssets)
        {
            List<AssetDetail> assetDetails = new List<AssetDetail>();
            List<string> assetIds = testAssets.Select(n => n.AssetId).ToList();
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                List<Asset> assets = context.Assets.Where(n => assetIds.Contains(n.AssetId)).ToList();
                
                foreach (Asset asset in assets)
                {
                    AssetReservationResult testAsset = testAssets.First(n => n.AssetId == asset.AssetId);
                    AssetDetail assetDetail = CreateAssetDetail(asset, testAsset);
                    assetDetail.Description = asset.ToAssetInfo().Description;
                    assetDetails.Add(assetDetail);
                }
                List<BadgeBox> badgeBoxes = context.BadgeBoxes.Where(n => assetIds.Contains(n.PrinterId)).ToList();
                foreach (BadgeBox badgeBox in badgeBoxes)
                {
                    assetDetails.Add(CreateBadgeBoxDetail(badgeBox));
                }
            }

            return assetDetails;
        }

        private static AssetDetail CreateAssetDetail(Asset asset, AssetReservationResult testAsset)
        {
            Printer printer = asset as Printer;
            if (printer != null)
            {
                return CreatePrintDeviceDetail(testAsset, printer);
            }

            VirtualPrinter virtualPrinter = asset as VirtualPrinter;
            if (virtualPrinter != null)
            {
                return CreatePrintDeviceDetail(testAsset, virtualPrinter);
            }

            Camera camera = asset as Camera;
            if (camera != null)
            {
                return CreateCameraDetail(testAsset, camera);
            }

            DeviceSimulator simulator = asset as DeviceSimulator;
            if (simulator != null)
            {
                if (simulator.SimulatorType.Equals("Jedi", StringComparison.InvariantCultureIgnoreCase))
                {
                    return CreateJediSimulatorDetail(testAsset, simulator);
                }
                else
                {
                    return CreateSiriusSimulatorDetail(testAsset, simulator);
                }
            }

            MobileDevice mobileDevice = asset as MobileDevice;
            if (mobileDevice != null)
            {
                return CreateMobileDeviceDetail(testAsset, mobileDevice);
            }

            // If we get to this point, just create a plain old asset
            return new AssetDetail(testAsset.AssetId, testAsset.AvailabilityStart, testAsset.AvailabilityEnd);
        }

        private static PrintDeviceDetail CreatePrintDeviceDetail(AssetReservationResult asset, Printer printer)
        {
            return new PrintDeviceDetail(asset.AssetId, asset.AvailabilityStart, asset.AvailabilityEnd)
            {
                Address = printer.Address1,
                Address2 = printer.Address2,
                PortNumber = printer.PortNumber,
                SnmpEnabled = printer.SnmpEnabled,
                Product = printer.Product,
                FirmwareType = printer.FirmwareType,
                Capability = printer.Capability,
                Availability = asset.Availability,
                AdminPassword = printer.Password
            };
        }

        private static PrintDeviceDetail CreatePrintDeviceDetail(AssetReservationResult asset, VirtualPrinter printer)
        {
            return new PrintDeviceDetail(asset.AssetId, asset.AvailabilityStart, asset.AvailabilityEnd)
            {
                Address = printer.Address,
                PortNumber = printer.PortNumber,
                SnmpEnabled = printer.SnmpEnabled,
                Product = string.Empty,
                Capability = printer.Capability,
                Availability = asset.Availability
            };
        }

        /// <summary>
        /// Helper to create the camera detail info.
        /// </summary>
        /// <param name="asset"></param>
        /// <param name="camera"></param>
        /// <returns></returns>
        private static CameraDetail CreateCameraDetail(AssetReservationResult asset, Camera camera)
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                FrameworkServer server = context.FrameworkServers.Include(n => n.ServerSettings).FirstOrDefault(n => n.HostName == camera.CameraServer);
                Dictionary<string, string> serverSettings = server?.ServerSettings.ToDictionary(n => n.Name, n => n.Value, StringComparer.OrdinalIgnoreCase);

                if (serverSettings == null)
                {
                    // If there are no ServerSettings for the specified camera server, return as much data as we have.
                    // Even if it's incomplete, we don't want to fail here, we want it to fail later in the process when
                    // the framework attempts to contact the camera server during the Validate stage.  Log a warning.
                    TraceFactory.Logger.Warn($"Server settings for Camera Server '{camera.CameraServer}' not found in configuration.");
                }

                return new CameraDetail(asset.AssetId, asset.AvailabilityStart, asset.AvailabilityEnd)
                {
                    Address = serverSettings?["ServerAPIUrl"] ?? string.Empty,
                    CameraId = camera.AssetId,
                    PrinterId = camera.PrinterId,
                    CameraServer = camera.CameraServer,
                    ServerUser = serverSettings?["ServerAPIUserName"] ?? string.Empty,
                    ServerPassword = serverSettings?["ServerAPIPassword"] ?? string.Empty,
                    Availability = asset.Availability
                };
            }
        }

        private static JediSimulatorDetail CreateJediSimulatorDetail(AssetReservationResult asset, DeviceSimulator simulator)
        {
            return new JediSimulatorDetail(asset.AssetId, asset.AvailabilityStart, asset.AvailabilityEnd)
            {
                Address = simulator.Address,
                Product = simulator.Product,
                MachineName = simulator.VirtualMachine,
                AdminPassword = simulator.Password,
                Availability = asset.Availability
            };
        }

        private static SiriusSimulatorDetail CreateSiriusSimulatorDetail(AssetReservationResult asset, DeviceSimulator simulator)
        {
            return new SiriusSimulatorDetail(asset.AssetId, asset.AvailabilityStart, asset.AvailabilityEnd)
            {
                Address = simulator.Address,
                Product = simulator.Product,
                AdminPassword = simulator.Password,
                Availability = asset.Availability
            };
        }

        private static MobileDeviceDetail CreateMobileDeviceDetail(AssetReservationResult asset, MobileDevice device)
        {
            return new MobileDeviceDetail(asset.AssetId, asset.AvailabilityStart, asset.AvailabilityEnd)
            {
                Availability = asset.Availability
            };
        }

        private static BadgeBoxDetail CreateBadgeBoxDetail(BadgeBox badgeBox)
        {
            return new BadgeBoxDetail(badgeBox.BadgeBoxId, DateTime.MaxValue)
            {
                BadgeBoxId = badgeBox.BadgeBoxId,
                IPAddress = badgeBox.IPAddress,
                Availability = AssetAvailability.Available,
                PrinterId = badgeBox.PrinterId,
                Description = badgeBox.Description
            };
        }
    }
}
