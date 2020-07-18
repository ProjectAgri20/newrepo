namespace HP.ScalableTest.Core.Virtualization
{
    /// <summary>
    /// Represents a vSphere virtual machine.
    /// </summary>
    public sealed class VSphereVirtualMachine : VirtualMachine
    {
        private const string _nameProperty = "name";
        private const string _powerStateProperty = "runtime.powerState";
        private const string _statusProperty = "guestHeartbeatStatus";
        private static readonly string[] _standardProperties = new[] { _nameProperty, _powerStateProperty, _statusProperty };

        /// <summary>
        /// Gets the standard set of properties that should be retrieved for a vSphere virtual machine managed object.
        /// </summary>
        internal static VSpherePropertySpec StandardProperties { get; } =
            new VSpherePropertySpec(VSphereManagedObjectType.VirtualMachine, _standardProperties);

        /// <summary>
        /// Gets the <see cref="VSphereManagedObject" /> that acts as a handle to this VM.
        /// </summary>
        internal VSphereManagedObject ManagedObject { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="VSphereVirtualMachine" /> class.
        /// </summary>
        /// <param name="managedObject">The <see cref="VSphereManagedObject" /> for this VM.</param>
        internal VSphereVirtualMachine(VSphereManagedObject managedObject)
            : base(managedObject.Name)
        {
            UpdateStatus(managedObject);
        }

        /// <summary>
        /// Updates the status of this VM.
        /// </summary>
        /// <param name="managedObject">The <see cref="VSphereManagedObject" /> containing the retrieved VM properties.</param>
        internal void UpdateStatus(VSphereManagedObject managedObject)
        {
            ManagedObject = managedObject;
            PowerState = managedObject.GetPropertyOrDefault<VirtualMachinePowerState>(_powerStateProperty);
            Status = managedObject.GetPropertyOrDefault<VirtualComponentStatus>(_statusProperty);
        }
    }
}
