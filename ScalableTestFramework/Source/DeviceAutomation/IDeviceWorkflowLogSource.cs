namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Interface for a component that logs device workflow performance events using <see cref="DeviceWorkflowLogger" />.
    /// </summary>
    /// <remarks>
    /// This interface is designed to allow multiple components involved in a workflow to share a single
    /// <see cref="DeviceWorkflowLogger" /> instance.  This ensures that sequential operations in the workflow
    /// will have consecutive event indexes for performance events.  Components that will log performance events
    /// should generally implement this interface, and consumers of this library should ensure that the same
    /// <see cref="DeviceWorkflowLogger" /> instance is set for each of the components in a workflow.
    /// 
    /// Implementers should consider inheriting from <see cref="DeviceWorkflowLogSource" /> if possible.
    /// </remarks>
    public interface IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Gets or sets the <see cref="DeviceWorkflowLogger" /> used by this component.
        /// </summary>
        DeviceWorkflowLogger WorkflowLogger { get; set; }
    }
}