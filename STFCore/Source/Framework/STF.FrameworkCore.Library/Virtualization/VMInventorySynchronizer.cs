using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Virtualization;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Virtualization
{
    /// <summary>
    /// Manages synchronization of the vSphere environment with asset inventory.
    /// </summary>
    public sealed class VMInventorySynchronizer : IDisposable
    {
        private readonly NetworkCredential _credential = null;
        private readonly Uri _vCenterUri = null;
        private readonly AssetInventoryConnectionString _connectionString = null;

        private readonly TimeSpan _syncCheckFrequency = TimeSpan.FromMinutes(60);
        private readonly Timer _syncCheckTimer;

        /// <summary>
        /// Constructs a new instance of <see cref="VMInventorySynchronizer" />.
        /// </summary>
        /// <param name="credential">The VCenter account to use in the synchronization operations.</param>
        /// <param name="vCenterServerUri">The URI of the VCenter Server.</param>
        /// <exception cref="ArgumentNullException"></exception>
        /// <exception cref="UriFormatException"></exception>
        public VMInventorySynchronizer(NetworkCredential credential, Uri vCenterServerUri, AssetInventoryConnectionString connectionString)
        {
            if (vCenterServerUri == null)
            {
                throw new ArgumentNullException(nameof(vCenterServerUri));
            }

            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            if (connectionString == null)
            {
                throw new ArgumentNullException(nameof(connectionString));
            }

            _vCenterUri = vCenterServerUri;
            _credential = credential;
            _connectionString = connectionString;

            _syncCheckTimer = new Timer(SyncInventory, null, _syncCheckFrequency, _syncCheckFrequency);
        }

        public void SyncInventory(object state)
        {
            TraceFactory.Logger.Debug($"Synchronizing VM inventory with {_vCenterUri.ToString()}.");

            List<VSphereVirtualMachine> serverVMs = new List<VSphereVirtualMachine>();
            using (var vSphereController = new VSphereVMController(_vCenterUri, _credential))
            {
                serverVMs.AddRange(vSphereController.GetVirtualMachines());
            }
            TraceFactory.Logger.Debug($"Synchronizing {serverVMs.Count} virtual machines.");

            int updated = 0;
            using (AssetInventoryContext context = new AssetInventoryContext(_connectionString))
            {
                List<FrameworkClient> inventoryVMs = context.FrameworkClients.ToList();
                foreach (FrameworkClient inventoryVM in inventoryVMs)
                {
                    VSphereVirtualMachine serverVM = serverVMs.FirstOrDefault(n => n.HostName == inventoryVM.FrameworkClientHostName);
                    if (serverVM != null)
                    {
                        VMUsageState currentState = EnumUtil.GetByDescription<VMUsageState>(inventoryVM.UsageState);

                        if (currentState != VMUsageState.Unavailable && currentState != VMUsageState.DoNotSchedule && string.IsNullOrEmpty(inventoryVM.SessionId))
                        {
                            inventoryVM.PowerState = EnumUtil.GetDescription(GetPowerState(serverVM));
                            inventoryVM.UsageState = EnumUtil.GetDescription(GetUsageState(serverVM));
                            inventoryVM.LastUpdated = DateTime.Now;
                            updated++;
                        }
                    }
                }

                context.SaveChanges();
            }

            TraceFactory.Logger.Debug($"Synchronization complete. Updated {updated} virtual machines.");
        }

        private static VMPowerState GetPowerState(VSphereVirtualMachine virtualMachine)
        {
            if (virtualMachine.PowerState == VirtualMachinePowerState.PoweredOn)
            {
                return VMPowerState.PoweredOn;
            }
            return VMPowerState.PoweredOff;
        }

        private static VMUsageState GetUsageState(VSphereVirtualMachine virtualMachine)
        {
            if (virtualMachine.PowerState == VirtualMachinePowerState.PoweredOn)
            {
                return VMUsageState.InUse;
            }
            return VMUsageState.Available;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _syncCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _syncCheckTimer.Dispose();
        }

        #endregion
    }
}
