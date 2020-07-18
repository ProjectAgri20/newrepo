using System;
using System.Collections.Generic;
using System.Threading;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.AssetInventory.Service
{
    /// <summary>
    /// Manages periodic cleanup and maintenance on the Asset Inventory database.
    /// </summary>
    public sealed class AssetInventoryService : IDisposable
    {
        private readonly ExpirationNotifier _expirationNotifier;
        private readonly List<IAssetInventoryMaintenanceManager> _maintenanceManagers;

        private readonly TimeSpan _maintenanceCheckFrequency = TimeSpan.FromHours(6);
        private readonly Timer _maintenanceCheckTimer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInventoryService" /> class with all available maintenance managers.
        /// </summary>
        /// <param name="connectionString">The <see cref="AssetInventoryConnectionString" />.</param>
        /// <param name="expirationNotifier">The <see cref="ExpirationNotifier" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public AssetInventoryService(AssetInventoryConnectionString connectionString, ExpirationNotifier expirationNotifier)
            : this(GetAllManagers(connectionString), expirationNotifier)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInventoryService" /> class with the specified maintenance managers.
        /// </summary>
        /// <param name="managers">The collection of <see cref="IAssetInventoryMaintenanceManager" /> to use when running maintenance.</param>
        /// <param name="expirationNotifier">The <see cref="ExpirationNotifier" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="managers" /> is null.</exception>
        public AssetInventoryService(IEnumerable<IAssetInventoryMaintenanceManager> managers, ExpirationNotifier expirationNotifier)
        {
            if (managers == null)
            {
                throw new ArgumentNullException(nameof(managers));
            }

            _expirationNotifier = expirationNotifier;
            _maintenanceManagers = new List<IAssetInventoryMaintenanceManager>(managers);
            _maintenanceCheckTimer = new Timer(RunMaintenance, null, _maintenanceCheckFrequency, _maintenanceCheckFrequency);
        }

        private static IEnumerable<IAssetInventoryMaintenanceManager> GetAllManagers(AssetInventoryConnectionString connectionString)
        {
            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            yield return new ReservationExpirationManager(connectionString);
            yield return new LicenseExpirationManager(connectionString);
            yield return new PrinterStatusSynchronizer(connectionString);
        }

        private void RunMaintenance(object state)
        {
            if (DateTime.Now.TimeOfDay < _maintenanceCheckFrequency)
            {
                _maintenanceCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);

                LogInfo("Running Asset Inventory maintenance.");
                foreach (IAssetInventoryMaintenanceManager manager in _maintenanceManagers)
                {
                    if (_expirationNotifier != null)
                    {
                        manager.SendExpirationNotifications(_expirationNotifier);
                    }
                    manager.RunMaintenance();
                }

                _maintenanceCheckTimer.Change(_maintenanceCheckFrequency, _maintenanceCheckFrequency);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _maintenanceCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _maintenanceCheckTimer.Dispose();
        }

        #endregion
    }
}
