namespace HP.ScalableTest.Core.AssetInventory.Service
{
    /// <summary>
    /// Interface for a class that performs notification and maintenance activities on asset inventory.
    /// </summary>
    public interface IAssetInventoryMaintenanceManager
    {
        /// <summary>
        /// Sends any expiration notifications required by this manager.
        /// </summary>
        /// <param name="expirationNotifier">The <see cref="ExpirationNotifier" />.</param>
        void SendExpirationNotifications(ExpirationNotifier expirationNotifier);

        /// <summary>
        /// Runs any maintenance activities required by this manager.
        /// </summary>
        void RunMaintenance();
    }
}
