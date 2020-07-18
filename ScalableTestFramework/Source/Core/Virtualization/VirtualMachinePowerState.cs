namespace HP.ScalableTest.Core.Virtualization
{
    /// <summary>
    /// The power state of a virtual machine.
    /// </summary>
    public enum VirtualMachinePowerState
    {
        /// <summary>
        /// The power state is unknown.
        /// </summary>
        Unknown,

        /// <summary>
        /// The VM is powered off.
        /// </summary>
        PoweredOff,

        /// <summary>
        /// The VM is powered on.
        /// </summary>
        PoweredOn,

        /// <summary>
        /// The VM is suspended.
        /// </summary>
        Suspended
    }
}
