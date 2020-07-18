namespace HP.ScalableTest.Core.Virtualization
{
    /// <summary>
    /// Represents a virtual machine.
    /// </summary>
    public abstract class VirtualMachine
    {
        /// <summary>
        /// Gets the host name of the virtual machine.
        /// </summary>
        public string HostName { get; }

        /// <summary>
        /// Gets the <see cref="VirtualMachinePowerState" /> of the virtual machine.
        /// </summary>
        public VirtualMachinePowerState PowerState { get; protected set; } = VirtualMachinePowerState.Unknown;

        /// <summary>
        /// Gets the <see cref="VirtualComponentStatus" /> of the virtual machine.
        /// </summary>
        public VirtualComponentStatus Status { get; protected set; } = VirtualComponentStatus.Gray;

        /// <summary>
        /// Initializes a new instance of the <see cref="VirtualMachine" /> class.
        /// </summary>
        /// <param name="hostName">The virtual machine host name.</param>
        protected VirtualMachine(string hostName)
        {
            HostName = hostName;
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return HostName;
        }
    }
}
