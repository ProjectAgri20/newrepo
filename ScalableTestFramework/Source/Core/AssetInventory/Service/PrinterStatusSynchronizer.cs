using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.AssetInventory.Service
{
    /// <summary>
    /// Synchronizes printer status with the asset inventory database.
    /// </summary>
    public sealed class PrinterStatusSynchronizer : IAssetInventoryMaintenanceManager
    {
        private readonly AssetInventoryConnectionString _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrinterStatusSynchronizer" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="AssetInventoryConnectionString" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public PrinterStatusSynchronizer(AssetInventoryConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Updates the status of all printers in asset inventory.
        /// </summary>
        public void UpdatePrinterStatus()
        {
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                List<Printer> printers = context.Assets.OfType<Printer>().ToList();
                LogDebug($"Updating status of {printers.Count} printers.");

                foreach (Printer printer in printers)
                {
                    if (!string.IsNullOrEmpty(printer.Address1))
                    {
                        if (IPAddress.TryParse(printer.Address1, out IPAddress printerIpAddress))
                        {
                            if (!printerIpAddress.IsIPv6LinkLocal)
                            {
                                printer.Online = PingDevice(printerIpAddress, TimeSpan.FromSeconds(5));
                            }
                        }
                    }
                }

                context.SaveChanges();
                LogDebug("Finished updating printer status.");
            }
        }

        private static bool PingDevice(IPAddress address, TimeSpan timeout)
        {
            DateTime endTime = DateTime.Now + timeout;
            using (Ping ping = new Ping())
            {
                PingReply pingStatus = ping.Send(address);
                while ((pingStatus.Status != IPStatus.Success) && (DateTime.Now < endTime))
                {
                    pingStatus = ping.Send(address);
                }

                return pingStatus.Status == IPStatus.Success;
            }
        }

        #region Explicit IAssetInventoryMaintenanceManager Members

        void IAssetInventoryMaintenanceManager.SendExpirationNotifications(ExpirationNotifier expirationNotifier)
        {
            // Nothing to do in this class
        }

        void IAssetInventoryMaintenanceManager.RunMaintenance()
        {
            UpdatePrinterStatus();
        }

        #endregion
    }
}
