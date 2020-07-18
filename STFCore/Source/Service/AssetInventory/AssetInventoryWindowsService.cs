using System;
using System.Configuration;
using System.Net;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.AssetInventory.Service;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Virtualization;

namespace HP.ScalableTest.Service.AssetInventory
{
    /// <summary>
    /// Windows service that hosts the Asset Inventory service.
    /// </summary>
    public class AssetInventoryWindowsService : SelfInstallingServiceBase
    {
        private AssetInventoryService _service;
        private VMInventorySynchronizer _vmInventorySynchronizer;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInventoryWindowsService"/> class.
        /// </summary>
        public AssetInventoryWindowsService()
            : base("AssetInventoryService", "STF Asset Inventory Service")
        {
            Description = "STF service for listing and reserving test environment assets.";
        }

        /// <summary>
        /// Starts this service instance.
        /// </summary>
        /// <param name="args">The <see cref="CommandLineArguments" /> provided to the start command.</param>
        protected override void StartService(CommandLineArguments args)
        {
            try
            {
                string assetInventoryServer = ConfigurationManager.AppSettings["AssetInventoryDatabase"];
                AssetInventoryConnectionString connectionString = new AssetInventoryConnectionString(assetInventoryServer);

                TraceFactory.Logger.Debug($"Starting {ServiceName}.  Connecting to database on: {assetInventoryServer}");

                string adminEmailServer = ConfigurationManager.AppSettings["AdminEmailServer"];
                _service = new AssetInventoryService(connectionString, new ExpirationNotifier(adminEmailServer));

                string vmServiceAccount = ConfigurationManager.AppSettings["VMServiceAccount"];
                string vmAccountPwd = ConfigurationManager.AppSettings["VMAccountPwd"];
                NetworkCredential vmAccount = new NetworkCredential(vmServiceAccount, vmAccountPwd, Environment.UserDomainName);
                Uri vCenterServerUri = new Uri(ConfigurationManager.AppSettings["VCenterServerUri"]);
                _vmInventorySynchronizer = new VMInventorySynchronizer(vmAccount, vCenterServerUri, connectionString);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug(ex.ToString());
                //We want to throw here so the OS doesn't think the Windows Service started when it really didn't.
                throw;
            }            
        }

        /// <summary>
        /// Stops this service instance.
        /// </summary>
        protected override void StopService()
        {
            _service.Dispose();
            _vmInventorySynchronizer.Dispose();
        }
    }
}
