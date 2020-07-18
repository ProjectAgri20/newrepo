using System;
using System.Net;
using System.Collections.Generic;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Virtualization;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core.Security;

namespace HP.ScalableTest.Framework.Dispatcher
{
    internal sealed class MachineReservationManager : IDisposable
    {
        private readonly string _sessionId;
        private readonly Dictionary<string, Queue<ManagedMachine>> _machinesByPlatform = null;

        public MachineReservationManager(string sessionId)
        {
            _sessionId = sessionId;
            _machinesByPlatform = new Dictionary<string, Queue<ManagedMachine>>();
        }

        private Queue<ManagedMachine> GetMachineQueueForPlatform(string platform)
        {
            Queue<ManagedMachine> queue = null;
            if (!_machinesByPlatform.TryGetValue(platform, out queue))
            {
                queue = new Queue<ManagedMachine>();
                _machinesByPlatform.Add(platform, queue);
            }

            return queue;
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Reliability", "CA2000:Dispose objects before losing scope")]
        public void Reserve(UserCredential credential, RequestedVMDictionary requestedVMs, VMQuantityDictionary requiredVMQuantity)
        {
            var virtualMachines = VMInventoryManager.RequestVMs(_sessionId, credential, requestedVMs, requiredVMQuantity);

            try
            {
                foreach (VirtualMachine machine in virtualMachines)
                {
                    TraceFactory.Logger.Debug("{0} : {1}".FormatWith(machine.Name, machine.MachineType));

                    var machineType = EnumUtil.GetByDescription<ManagedMachineType>(machine.MachineType);
                    var machineInstance = new ManagedMachine(machine.Name, machineType);
                    GetMachineQueueForPlatform(machine.PlatformUsage).Enqueue(machineInstance);
                }

            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);

                // Something bad happened.  Release everything.
                foreach (var machine in virtualMachines)
                {
                    using (var machineInstance = new ManagedMachine(machine.Name, EnumUtil.GetByDescription<ManagedMachineType>(machine.MachineType)))
                    {
                        machineInstance.ReleaseReservation();
                    }
                }

                Dispose();
                throw;
            }
        }

        public ManagedMachine GetNext(string platform)
        {
            return GetMachineQueueForPlatform(platform).Dequeue();
        }

        public void Dispose()
        {
            // Release the rest of the VMs.
            foreach (var queue in _machinesByPlatform.Values)
            {
                while (queue.Count != 0)
                {
                    var machine = queue.Dequeue();
                    machine.Dispose();
                }
            }
        }
    }
}
