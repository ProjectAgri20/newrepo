using System;

namespace HP.ScalableTest.Virtualization
{
    /// <summary>
    /// Virtual Machine object that supports various VM-related actions, such as power on, power off, revert, etc.
    /// </summary>
    public class ManagedMachine : IDisposable
    {
        private bool _reservationReleased = false;

        /// <summary>
        /// Gets the name of this VM.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the type of machine.
        /// </summary>
        public ManagedMachineType MachineType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ManagedMachine"/> class.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <param name="machineType">The type of machine.</param>
        public ManagedMachine(string name, ManagedMachineType machineType)
        {
            Name = name;
            MachineType = machineType;
        }

        /// <summary>
        /// Powers on a VM.
        /// </summary>
        /// <param name="setInUse">if set to <c>true</c> then set the VM to in use in the asset inventory system.</param>
        public void PowerOn(bool setInUse = true)
        {
            VMController.PowerOnMachine(Name);
            if (setInUse)
            {
                VMInventoryManager.SetInUse(Name);
            }
        }

        /// <summary>
        /// Performs a guest shutdown on a VM.
        /// </summary>
        /// <param name="wait">if set to <c>true</c> [wait until powered off].</param>
        public void Shutdown(bool wait = true)
        {
            VMController.Shutdown(Name, wait);
        }

        /// <summary>
        /// Powers off this instance.
        /// </summary>
        public void PowerOff()
        {
            VMController.PowerOff(Name);
        }

        /// <summary>
        /// Reverts a VM to it's last snapshot.
        /// </summary>
        public void Revert(bool wait = false)
        {
            VMController.Revert(Name, wait);
        }

        /// <summary>
        /// Is a VM powered on.
        /// </summary>
        /// <returns>True if it's powered on.</returns>
        public bool IsPoweredOn()
        {
            return VMController.IsPoweredOn(Name);
        }

        /// <summary>
        /// Releases the reservation on this VM.
        /// </summary>
        public void ReleaseReservation()
        {
            if (_reservationReleased)
            {
                throw new InvalidOperationException("Reservation was never made for " + Name);
            }

            VMInventoryManager.ReleaseReservation(Name);
            _reservationReleased = true;
        }


        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (!_reservationReleased)
                {
                    ReleaseReservation();
                }
            }
        }

        #endregion IDisposable Members
    }
}