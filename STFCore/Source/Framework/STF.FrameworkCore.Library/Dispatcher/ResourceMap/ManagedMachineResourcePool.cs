using System;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Virtualization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Manages a pool of machines that will be allocated and disbursed for a test scenario
    /// </summary>
    internal class ManagedMachineResourcePool
    {
        private MachineReservationManager _reservationManager = null;
        private readonly SystemManifestAgent _manifestAgent = null;
        private int _localMachineCount = 1;

        public ManagedMachineResourcePool(SystemManifestAgent agent)
            : base()
        {
            _manifestAgent = agent;
        }

        /// <summary>
        /// Reserves the hosts by communicating with the reservation manager.
        /// </summary>
        public void ReserveMachines()
        {
            if (GlobalSettings.IsDistributedSystem)
            {
                try
                {
                    if (_reservationManager == null)
                    {
                        var machineQuantity = new VMQuantityDictionary(_manifestAgent.Quantities.MachineQuantity);
                        var ticket = _manifestAgent.Ticket;

                        _reservationManager = new MachineReservationManager(_manifestAgent.Ticket.SessionId);
                        _reservationManager.Reserve(ticket.SessionOwner, ticket.RequestedVMs, machineQuantity);
                    }
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error(ex.Message);
                    throw;
                }
            }
            else
            {
                _localMachineCount = 1;
            }
        }

        public void AssignMachine(ResourceHost host)
        {
            ManagedMachine machine = null;
            if (GlobalSettings.IsDistributedSystem)
            {
                machine = _reservationManager.GetNext(host.Manifest.Platform);
            }
            else
            {
                if (_localMachineCount == 1)
                {
                    _localMachineCount--;
                    machine = new ManagedMachine(Environment.MachineName, ManagedMachineType.WindowsDesktop);
                }
                else
                {
                    // The only way to get here is if the maximum worker count has been exceeded for the entire scenario,
                    // which would cause more than one machine to get assigned.
                    throw new InvalidOperationException("The maximum solution tester count has been exceeded.");
                }
            }

            host.Machine = ObjectFactory.Create<HostMachine>(machine.MachineType, machine, host.Manifest);
            host.MapElement.Name = machine.Name;
            host.Manifest.HostMachine = machine.Name;
            host.MapElement.Enabled = true;
            host.Machine.Configured = true;
            TraceFactory.Logger.Debug("{0} : {1}".FormatWith(machine.Name, machine.MachineType));
        }
    }
}
