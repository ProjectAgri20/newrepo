using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Base class for a component that logs device workflow performance events using <see cref="DeviceWorkflowLogger" />.
    /// </summary>
    /// <remarks>
    /// This class serves two purposes.  First, it provides a wrapper around the <see cref="DeviceWorkflowLogger" />
    /// class that minimizes the amount of code required to log a performance event.  Second, it provides a common
    /// implementation of <see cref="IDeviceWorkflowLogSource" /> to facilitate sharing of the logger instance.
    /// </remarks>
    public abstract class DeviceWorkflowLogSource : IDeviceWorkflowLogSource
    {
        /// <summary>
        /// Gets or sets the <see cref="DeviceWorkflowLogger" /> used by this component.
        /// </summary>
        public DeviceWorkflowLogger WorkflowLogger { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceWorkflowLogSource" /> class.
        /// </summary>
        protected DeviceWorkflowLogSource()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Records a performance event with the specified event name.
        /// </summary>
        /// <param name="marker">The <see cref="DeviceWorkflowMarker" />.</param>
        protected void RecordEvent(DeviceWorkflowMarker marker)
        {
            WorkflowLogger?.RecordPerformanceEvent(marker.GetDescription());
        }

        /// <summary>
        /// Records a performance event with the specified event name concatenating the given parameter to the event.
        /// </summary>
        /// <param name="marker">The <see cref="DeviceWorkflowMarker" />.</param>
        /// <param name="parameter">The parameter to include in the logged marker.</param>
        protected void RecordEvent(DeviceWorkflowMarker marker, string parameter)
        {
            string description = marker.GetDescription();
            WorkflowLogger?.RecordPerformanceEvent($"{description}_{parameter}");

            // Also log this to the detail table so that these values can be pulled for reporting
            WorkflowLogger?.RecordExecutionDetail(marker, parameter);
        }

        /// <summary>
        /// Records additional workflow details with the specified label and message.
        /// </summary>
        /// <param name="marker">The <see cref="DeviceWorkflowMarker" />.</param>
        /// <param name="message">The message.</param>
        protected void RecordInfo(DeviceWorkflowMarker marker, string message)
        {
            WorkflowLogger?.RecordExecutionDetail(marker, message);
        }
    }
}
