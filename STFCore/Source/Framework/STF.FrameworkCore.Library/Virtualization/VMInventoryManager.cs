using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Virtualization;
using HP.ScalableTest.Core.Lock;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Synchronization;
using HP.ScalableTest.Utility;
using VirtualMachine = HP.ScalableTest.Data.EnterpriseTest.VirtualMachine;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.Security;

namespace HP.ScalableTest.Virtualization
{
    /// <summary>
    /// Manages operations on the VM Inventory.  Updates VM statuses, sets holds, etc.
    /// </summary>
    public static class VMInventoryManager
    {
        private static readonly object _resourceLock = new object();

        /// <summary>
        /// Gets the list of all reserved VMs.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <returns></returns>
        private static Collection<VirtualMachine> GetReservedList(string sessionId)
        {
            return GetMachineList(sessionId, VMUsageState.Reserved);
        }

        /// <summary>
        /// Gets a replacement VM if one is not responding.
        /// </summary>
        /// <param name="replacedVMName">The name of the replaced VM.</param>
        /// <returns></returns>
        public static VirtualMachine GetReplacement(string replacedVMName)
        {
            VirtualMachine replacement = null;
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                //Get the SessionId that tried to power-on the failed machine
                FrameworkClient replacedVirtualMachine = Select(context, replacedVMName);

                LockToken lockToken = new GlobalLockToken("VirtualMachineReservation", new TimeSpan(0, 1, 0), new TimeSpan(0, 2, 0));
                ExecutionServices.CriticalSection.Run(lockToken, () =>
                {
                    //Get the next available machine
                    replacement = VirtualMachine.SelectReplacement(replacedVirtualMachine.PlatformUsage, replacedVirtualMachine.HoldId);

                    if (replacement != null)
                    {
                        Reserve
                        (
                            context,
                            replacement.Name,
                            replacedVirtualMachine.PlatformUsage,
                            DateTime.Now,
                            replacedVirtualMachine.SessionId,
                            GlobalSettings.Environment
                        );
                        context.SaveChanges();
                    }
                    else
                    {
                        throw new VMInventoryException("Insufficient VMs available.");
                    }
                });
            }

            return replacement;
        }

        /// <summary>
        /// Releases the VM reservation.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        public static void ReleaseReservation(string hostName)
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                SetState(hostName, VMPowerState.PoweredOff, VMUsageState.Available, true, context);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Reserves the requested block of VMs.
        /// </summary>
        /// <param name="sessionId">The session Id.</param>
        /// <param name="credential">Credentials for reserving the VMs</param>
        /// <param name="requestedVMs">The platforms of the requested VMs.</param>
        /// <param name="requiredVMQuantity">The quantity per platform of requested VMs.</param>
        /// <returns>Whether or not the requested VMs were reserved.</returns>
        public static Collection<VirtualMachine> RequestVMs(string sessionId, UserCredential credential, RequestedVMDictionary requestedVMs, VMQuantityDictionary requiredVMQuantity)
        {
            bool reserved;
            lock (_resourceLock)
            {
                TraceFactory.Logger.Debug("Reserving virtual machines...");
                reserved = VMInventoryManager.Reserve(sessionId, credential, requestedVMs, requiredVMQuantity);
            }

            if (!reserved)
            {
                throw new VMInventoryException("Unable to reserve requested virtual machines");
            }

            var reservedVMs = VMInventoryManager.GetReservedList(sessionId);

            TraceFactory.Logger.Info("Successfully reserved {0} virtual machines".FormatWith(reservedVMs.Count));

            return reservedVMs;
        }

        /// <summary>
        /// Reserves the requested VMs.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="credential">The user requesting the reservation.</param>
        /// <param name="requestedVMs">The requested VMs.</param>
        /// <param name="requiredVMQuantity">The required VM quantity.</param>
        /// <returns></returns>
        public static bool Reserve(string sessionId, UserCredential credential, RequestedVMDictionary requestedVMs, VMQuantityDictionary requiredVMQuantity)
        {
            if (requiredVMQuantity == null)
            {
                throw new ArgumentNullException("requiredVMQuantity");
            }

            if (requestedVMs == null)
            {
                throw new ArgumentNullException("requestedVMs");
            }

            DateTime currentTime = DateTime.Now;

            TraceFactory.Logger.Debug(requiredVMQuantity.ToString());

            LockToken lockToken = new GlobalLockToken("VirtualMachineReservation", new TimeSpan(0, 1, 0), new TimeSpan(0, 2, 0));
            ExecutionServices.CriticalSection.Run(lockToken, () =>
            {
                // Need to work with a master list of VMs.  If the requested set is empty, then the master list
                // will come from the database.  Otherwise it will come from the requested set.
                List<VirtualMachine> masterMachineList = GetMasterList(credential, requestedVMs);

                // Copy the required VMs (by Platform) to their own dictionary as items will be removed.
                Dictionary<string, int> remainingPlatforms = requiredVMQuantity.ToDictionary(entry => entry.Key, entry => entry.Value);

                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    // Iterate over each item in the required VM dictionary and process
                    while (remainingPlatforms.Count > 0)
                    {
                        // Get the target platform and required count for each entry
                        string targetPlatformId = remainingPlatforms.ElementAt(0).Key;
                        int targetPlatformCount = remainingPlatforms.ElementAt(0).Value;
                        TraceFactory.Logger.Debug("Target Platform {0} and machine count {1}".FormatWith(targetPlatformId, targetPlatformCount));

                        List<VirtualMachineSelection> machineSelections = GetMachineSelections(targetPlatformId, masterMachineList, remainingPlatforms);


                        // Now try to reserve the VMs.  If the required VMs are more than the available, then
                        // this is an error.  Otherwise work down the list for the number of VMs required.  Since
                        // they are now ordered the VMs selected will be the least unique and those will more
                        // special associations will be further down the list.
                        TraceFactory.Logger.Debug("Machine Platform: {0} - {1} Requested, {2} Available".FormatWith(targetPlatformId, targetPlatformCount, machineSelections.Count));

                        if (machineSelections.Count >= targetPlatformCount)
                        {
                            for (int i = 0; i < targetPlatformCount; i++)
                            {
                                Reserve
                                (
                                    context,
                                    machineSelections[i].Machine.Name,
                                    targetPlatformId,
                                    currentTime,
                                    sessionId,
                                    GlobalSettings.Environment
                                );

                                //Once the machine is reserved, remove it from the master list.                            
                                masterMachineList.Remove(machineSelections[i].Machine);
                                TraceFactory.Logger.Debug("Host {0} reserved".FormatWith(machineSelections[i].Machine.Name));
                            }
                        }
                        else
                        {
                            throw new VMInventoryException("Only {0} {1} machines available of {2} requested."
                                .FormatWith(machineSelections.Count, targetPlatformId, targetPlatformCount));
                        }

                        // Remove the platform that was just processed
                        remainingPlatforms.Remove(targetPlatformId);
                    }

                    // Save all the final changes.  Do this at the end so that if there
                    // is a problem, there is nothing to roll back.
                    context.SaveChanges();

                } //AssetInventoryContext
            }); //reservation.Run

            return true;
        }

        /// <summary>
        /// Sets the VM in use flag.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        public static void SetInUse(string hostName)
        {
            lock (_resourceLock)
            {
                using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
                {
                    SetState(hostName, VMPowerState.PoweredOn, VMUsageState.InUse, false, context);
                    SetEnvironment(hostName, GlobalSettings.Environment, context);
                    context.SaveChanges();
                }
            }
        }

        /// <summary>
        /// Sets the VM in use flag and clears the dispatcher.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        public static void SetInUseClearSession(string hostName)
        {
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                SetState(hostName, VMPowerState.PoweredOn, VMUsageState.InUse, true, context);
                SetEnvironment(hostName, GlobalSettings.Environment, context);
                context.SaveChanges();
            }
        }

        /// <summary>
        /// ReSyncs the VM Inventory.
        /// </summary>
        /// <param name="credential">The VSphere credentials to use during synchronization.</param>
        public static void SyncInventory(UserCredential credential)
        {
            UserManager.CurrentUser = credential;

            TraceFactory.Logger.Debug("Acquiring Lock.");
            var token = new GlobalLockToken("VirtualMachineInventorySynchronization", TimeSpan.FromMinutes(3), TimeSpan.FromMinutes(5));

            // Create an explicit CriticalSection here because this call is coming from the UI where ExecutionServices is not initialized.
            CriticalSection criticalSection = new CriticalSection(new DistributedLockManager(GlobalSettings.WcfHosts["Lock"]));
            criticalSection.Run(token, UpdateInventory);
        }

        /// <summary>
        /// Sets the VMs Usage state.
        /// </summary>
        /// <param name="hostNames">The host names collection</param>
        /// <param name="usageState">The usage state</param>
        public static void SetUsageState(IEnumerable<string> hostNames, VMUsageState usageState)
        {
            if (hostNames == null)
            {
                throw new ArgumentNullException("hostNames");
            }

            VMPowerState powerState = GetPowerState(usageState);

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                foreach (string hostName in hostNames)
                {
                    SetState(hostName, powerState, usageState, (usageState == VMUsageState.Available), context);
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Adds a hold on the specified VM hosts.
        /// </summary>
        /// <param name="hostNames">The VM host names.</param>
        /// <param name="id">The hold Id.</param>
        public static void AddHold(IEnumerable<string> hostNames, string id)
        {
            if (hostNames == null)
            {
                throw new ArgumentNullException("hostNames");
            }

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                var updateTime = DateTime.Now;
                foreach (string hostName in hostNames)
                {
                    var machine = Select(context, hostName);
                    if (machine != null)
                    {
                        machine.HoldId = id;
                        machine.LastUpdated = updateTime;
                    }
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Removes a hold on the specified VM hosts.
        /// </summary>
        /// <param name="hostNames">The VM host names.</param>
        public static void RemoveHold(IEnumerable<string> hostNames)
        {
            AddHold(hostNames, null);
        }

        /// <summary>
        /// Gets the VM list.
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        /// <param name="usageState">State of the usage.</param>
        /// <returns></returns>
        private static Collection<VirtualMachine> GetMachineList(string sessionId, VMUsageState usageState)
        {
            var reservedVMList = new Collection<VirtualMachine>();

            foreach (var machine in VirtualMachine.Select(sessionId: sessionId, usageState: usageState))
            {
                reservedVMList.Add(machine);
            }

            return reservedVMList;
        }

        /// <summary>
        /// Sets the state of the VM.
        /// </summary>
        /// <param name="hostName">Name of the machine.</param>
        /// <param name="powerState">State of the power.</param>
        /// <param name="usageState">State of the usage.</param>
        /// <param name="clearSession">if set to <c>true</c> [clear session].</param>
        /// <param name="entities">The entities.</param>
        private static void SetState(string hostName, VMPowerState powerState, VMUsageState usageState, bool clearSession, AssetInventoryContext entities)
        {
            FrameworkClient machine = Select(entities, hostName);

            if (machine != null)
            {
                if (clearSession)
                {
                    machine.SessionId = null;
                    machine.Environment = string.Empty;
                }
                machine.PowerState = EnumUtil.GetDescription(powerState);
                machine.UsageState = EnumUtil.GetDescription(usageState);
                machine.PlatformUsage = string.Empty;
                machine.LastUpdated = DateTime.Now;
            }
            else
            {
                TraceFactory.Logger.Error("{0} not found in database".FormatWith(hostName));
            }
        }

        private static void SetEnvironment(string hostName, DataEnvironment environment, AssetInventoryContext entities)
        {
            FrameworkClient machine = Select(entities, hostName);

            if (machine != null)
            {
                machine.Environment = environment.ToString();
            }
            else
            {
                TraceFactory.Logger.Error("{0} not found in database".FormatWith(hostName));
            }
        }

        private static List<VirtualMachineSelection> GetMachineSelections(string targetPlatformId, List<VirtualMachine> masterMachineList, Dictionary<string, int> remainingPlatforms)
        {
            // For this platform, get a list of all Virtual Machines that contain the target platform
            // Get this from the master list that was generated above
            IEnumerable<VirtualMachine> targetMachines =
                (
                    from v in masterMachineList
                    where v.Platforms.Any(e => e.FrameworkClientPlatformId == targetPlatformId)
                    select v
                );

            // For each virtual machine, get the count of associations that intersect with the current list
            // of required platforms. Order this list by the SortOrder.  It will provide the baseline for
            // choosing VMs to use of the target platform.  The machines with the lowest assocation count
            // are the targets for reservation as it reduces that change of reserving a machine that is
            // needed for another platform, where there may not be very many machines with the association.
            // In addition to the actual associations between the requested platform and the current VM, 
            // also obtain the total associations that exist for the VM.  Doing a secondary sort on this 
            // information will also ensure that for most requested platforms, we will choose from those
            // that have the fewest associations.  This will help to keep those more unique VMs around for
            // scenarios that really need them.
            List<VirtualMachineSelection> machineSelections = new List<VirtualMachineSelection>();
            foreach (VirtualMachine machine in targetMachines.OrderBy(e => e.SortOrder))
            {
                var machinePlatforms = (from p in machine.Platforms select p.FrameworkClientPlatformId).ToList();
                int matchingCount = (from p in remainingPlatforms.Keys.Intersect(machinePlatforms) select p).Count();
                int totalCount = machinePlatforms.Count();
                if (matchingCount > 0)
                {
                    machineSelections.Add(new VirtualMachineSelection(machine, matchingCount, totalCount));
                }
            }

            // Sort the list one more time, this time by the count that was calculated above.  Put the lowest numbers
            // first, again so those machines with the fewest associations are used first.
            machineSelections =
                (
                    from a in machineSelections
                    orderby a.MatchingAssociationCount ascending
                    orderby a.TotalAssociationCount ascending
                    select a
                ).ToList();
            return machineSelections;
        }

        private static List<VirtualMachine> GetMasterList(UserCredential credential, RequestedVMDictionary requestedVMs)
        {
            List<VirtualMachine> masterMachineList = null;
            if (requestedVMs.Count == 0)
            {
                TraceFactory.Logger.Debug("No specific machines requested.");

                // Add all available VMs with no hold ID to the master list
                masterMachineList = VirtualMachine.Select(VMPowerState.PoweredOff, VMUsageState.Available, holdId: null).ToList();
            }
            else
            {
                TraceFactory.Logger.Debug("Specific machines requested.");

                // Add all available VMs with no hold ID to the master list
                masterMachineList = VirtualMachine.Select(VMPowerState.PoweredOff, VMUsageState.Available).ToList();

                // The user requested specific machines, so throw out any machines that are not on that list
                IEnumerable<string> requestedVMNames = requestedVMs.SelectMany(n => n.Value);
                masterMachineList.RemoveAll(n => !requestedVMNames.Contains(n.Name, StringComparer.OrdinalIgnoreCase));
            }

            // If user is not an admin, check for user group rights to the VMs
            if (!credential.HasPrivilege(UserRole.Administrator))
            {
                TraceFactory.Logger.Debug("{0} total machines BEFORE quota".FormatWith(masterMachineList.Count));
                using (var enterpriseTestContext = DbConnect.EnterpriseTestContext())
                {
                    var allowedVMs = enterpriseTestContext.UserGroups
                        .Where(n => n.Users.Any(m => m.UserName == credential.UserName))
                        .SelectMany(n => n.FrameworkClients)
                        .Select(n => n.FrameworkClientHostName).ToList();
                    masterMachineList.RemoveAll(n => !allowedVMs.Contains(n.Name, StringComparer.OrdinalIgnoreCase));
                }
                TraceFactory.Logger.Debug("{0} total machines AFTER quota".FormatWith(masterMachineList.Count));
            }

            return masterMachineList;
        }

        /// <summary>
        /// Gets the VM Power state.
        /// </summary>
        /// <param name="virtualMachine">The vm.</param>
        /// <returns></returns>
        private static VMPowerState GetPowerState(VSphereVirtualMachine virtualMachine)
        {
            if (virtualMachine != null && virtualMachine.PowerState == VirtualMachinePowerState.PoweredOn)
            {
                return VMPowerState.PoweredOn;
            }
            return VMPowerState.PoweredOff;
        }

        private static void UpdateInventory()
        {
            int count = 0;

            TraceFactory.Logger.Debug("Synchronizing VM inventory with vSphere server.");
            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                using (var vSphereController = GetVSphereController())
                {
                    foreach (VSphereVirtualMachine serverVM in vSphereController.GetVirtualMachines())
                    {
                        FrameworkClient inventoryVM = Select(context, serverVM.HostName);
                        if (inventoryVM != null)
                        {
                            VMUsageState currentState = EnumUtil.GetByDescription<VMUsageState>(inventoryVM.UsageState);

                            if (currentState != VMUsageState.Unavailable &&
                                currentState != VMUsageState.DoNotSchedule &&
                                string.IsNullOrEmpty(inventoryVM.SessionId))
                            {
                                inventoryVM.PowerState = EnumUtil.GetDescription(GetPowerState(serverVM));
                                inventoryVM.UsageState = EnumUtil.GetDescription(GetUsageState(serverVM));
                                inventoryVM.LastUpdated = DateTime.Now;
                            }
                            count++;
                        }
                    }
                }

                context.SaveChanges();
            }
            TraceFactory.Logger.Debug("Syncronization complete.  VM Count: {0}".FormatWith(count));
        }

        private static VSphereVMController GetVSphereController()
        {
            string serverUri = GlobalSettings.Items[Setting.VMWareServerUri];
            return new VSphereVMController
            (
                new Uri(serverUri), UserManager.CurrentUser.ToNetworkCredential()
            );
        }

        private static FrameworkClient Select(AssetInventoryContext entities, string hostName)
        {
            return entities.FrameworkClients.FirstOrDefault(n => n.FrameworkClientHostName.Equals(hostName, StringComparison.OrdinalIgnoreCase));
        }

        private static void Reserve(AssetInventoryContext entities, string hostName, string platformUsage, DateTime lastUpdated, string sessionId, DataEnvironment environment)
        {
            FrameworkClient vmReservation = Select(entities, hostName);
            vmReservation.UsageState = EnumUtil.GetDescription(VMUsageState.Reserved);
            vmReservation.PlatformUsage = platformUsage;
            vmReservation.LastUpdated = lastUpdated;
            vmReservation.SessionId = sessionId;
            vmReservation.Environment = environment.ToString();
        }

        /// <summary>
        /// Gets the VM Power state.
        /// </summary>
        /// <param name="usageState">State of the usage.</param>
        /// <returns></returns>
        private static VMPowerState GetPowerState(VMUsageState usageState)
        {
            switch (usageState)
            {
                case VMUsageState.Available:
                case VMUsageState.DoNotSchedule:
                case VMUsageState.Unavailable:
                case VMUsageState.Reserved:
                    return VMPowerState.PoweredOff;
                case VMUsageState.InUse:
                    return VMPowerState.PoweredOn;
                default:
                    throw new ArgumentException("Unable to return power state for {0}.".FormatWith(usageState), usageState.ToString());
            }
        }

        /// <summary>
        /// Gets the state of the VM usage based on the power state of the VM.
        /// </summary>
        /// <param name="virtualMachine">The vm.</param>
        /// <returns></returns>
        private static VMUsageState GetUsageState(VSphereVirtualMachine virtualMachine)
        {
            if (virtualMachine != null && virtualMachine.PowerState == VirtualMachinePowerState.PoweredOn)
            {
                return VMUsageState.InUse;
            }
            return VMUsageState.Available;
        }

        private class VirtualMachineSelection
        {
            private Tuple<VirtualMachine, int, int> _data = null;

            /// <summary>
            /// Initializes a new instance of the <see cref="VirtualMachineSelection"/> class.
            /// </summary>
            /// <param name="machine">The machine.</param>
            /// <param name="remainingCount">The remaining count.</param>
            /// <param name="totalCount">The total count.</param>
            public VirtualMachineSelection(VirtualMachine machine, int remainingCount, int totalCount)
            {
                _data = new Tuple<VirtualMachine, int, int>(machine, remainingCount, totalCount);
            }

            /// <summary>
            /// Gets the virtual machine.
            /// </summary>
            public VirtualMachine Machine
            {
                get { return _data.Item1; }
            }

            /// <summary>
            /// Gets the matching association count.
            /// </summary>
            public int MatchingAssociationCount
            {
                get { return _data.Item2; }
            }

            /// <summary>
            /// Gets the total association count.
            /// </summary>
            public int TotalAssociationCount
            {
                get { return _data.Item3; }
            }

            /// <summary>
            /// Returns a <see cref="System.String"/> that represents this instance.
            /// </summary>
            /// <returns>
            /// A <see cref="System.String"/> that represents this instance.
            /// </returns>
            public override string ToString()
            {
                return "{0}, {1}, {2}".FormatWith(_data.Item1.Name, _data.Item2, _data.Item3);
            }
        }
    }
}
