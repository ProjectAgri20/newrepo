using System;
using System.Threading.Tasks;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Core.Lock;
using HP.ScalableTest.Framework.Wcf;
using System.ServiceModel;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DataLog.Service;
using HP.ScalableTest.Core.AssetInventory.Service;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core;
using HP.ScalableTest.Utility;

namespace HP.SolutionTest
{
    /// <summary>
    /// Windows service that hosts the Central Log service.
    /// </summary>
    public class SolutionTestWindowsService : SelfInstallingServiceBase
    {
        private AssetInventoryService _assetInventory = null;
        private ServiceHost _dataLogService = null;
        private readonly ServiceHost _lock = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogWindowsService"/> class.
        /// </summary>
        public SolutionTestWindowsService()
            : base("hpstbds", "HP Solution Test Bench Data Service")
        {
            Description = "HP Solution Test Bench Data Service";

            GlobalSettings.IsDistributedSystem = false;
            _lock = new WcfHost<ILockService>(new LockService(), LockServiceEndpoint.MessageTransferType, LockServiceEndpoint.BuildUri("localhost"));
        }

        /// <summary>
        /// Starts this service instance.
        /// </summary>
        /// <param name="args">The <see cref="CommandLineArguments" /> provided to the start command.</param>
        protected override void StartService(CommandLineArguments args)
        {
            if (args == null)
            {
                throw new ArgumentNullException("args");
            }

            // Load the settings, either from the command line or the local cache
            FrameworkServiceHelper.LoadSettings(args);

            ReservationExpirationManager expirationManager = new ReservationExpirationManager(DbConnect.AssetInventoryConnectionString);
            ExpirationNotifier expirationNotifier = new ExpirationNotifier(GlobalSettings.Items[Setting.AdminEmailServer]);
            _assetInventory = new AssetInventoryService(new[] { expirationManager }, expirationNotifier);

            Task.Factory.StartNew(() => _lock.Open());

            DataLogConnectionString connectionString = new DataLogConnectionString(GlobalSettings.Items[Setting.DataLogDatabase]);
            _dataLogService = new WcfHost<IDataLogService>(new DataLogService(connectionString), DataLogServiceEndpoint.MessageTransferType, DataLogServiceEndpoint.BuildUri("localhost"));
            Task.Factory.StartNew(() => _dataLogService.Open());
        }

        /// <summary>
        /// Stops this service instance.
        /// </summary>
        protected override void StopService()
        {
            Task.Factory.StartNew(() => _assetInventory.Dispose());
            Task.Factory.StartNew(() => _lock.Close());
            Task.Factory.StartNew(() => _dataLogService.Close());
        }
    }
}
