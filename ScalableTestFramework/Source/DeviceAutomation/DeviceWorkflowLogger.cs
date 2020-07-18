using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Logs details and performance information about device workflow operations
    /// that occur over the course of plugin execution.
    /// </summary>
    public sealed class DeviceWorkflowLogger
    {
        private int _index = 0;

        private readonly PluginExecutionData _executionData;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceWorkflowLogger" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        public DeviceWorkflowLogger(PluginExecutionData executionData)
        {
            _executionData = executionData;
        }

        /// <summary>
        /// Logs an activity exeuction performance event for the specified <see cref="DeviceWorkflowMarker" />.
        /// </summary>
        /// <param name="marker">The <see cref="DeviceWorkflowMarker" />.</param>
        public void RecordEvent(DeviceWorkflowMarker marker)
        {
            RecordPerformanceEvent(marker.GetDescription());
        }
        /// <summary>
        /// Records a performance event with the specified event name concatenating the given parameter to the event.
        /// </summary>
        /// <param name="marker">The <see cref="DeviceWorkflowMarker" />.</param>
        /// <param name="parameter">The parameter to include in the logged marker.</param>
        public void RecordEvent(DeviceWorkflowMarker marker, string parameter)
        {
            string description = marker.GetDescription();
            RecordPerformanceEvent($"{description}_{parameter}");
        }
        /// <summary>
        /// Logs an activity exeuction performance event with the specified label.
        /// </summary>
        /// <param name="label">The label.</param>
        internal void RecordPerformanceEvent(string label)
        {
            DevicePerformanceLog dataLog = new DevicePerformanceLog(_executionData, label, _index++);
            ExecutionServices.DataLogger.Submit(dataLog);
        }

        /// <summary>
        /// Records additional workflow details with the specified label and message.
        /// </summary>
        /// <param name="marker">The <see cref="DeviceWorkflowMarker" />.</param>
        /// <param name="message">The message.</param>
        public void RecordExecutionDetail(DeviceWorkflowMarker marker, string message)
        {
            ActivityExecutionDetailLog log = new ActivityExecutionDetailLog(_executionData, marker.GetDescription(), message);
            ExecutionServices.DataLogger.Submit(log);
        }

        #region DataLog Helper Classes

        private sealed class DevicePerformanceLog : ActivityDataLog
        {
            public override string TableName => "ActivityExecutionPerformance";

            [DataLogProperty(MaxLength = 255)]
            public string EventLabel { get; }

            [DataLogProperty]
            public int EventIndex { get; }

            [DataLogProperty]
            public DateTimeOffset EventDateTime { get; }

            public DevicePerformanceLog(PluginExecutionData executionData, string label, int index)
                : base(executionData)
            {
                EventLabel = label;
                EventIndex = index;
                EventDateTime = DateTimeOffset.Now;
            }
        }

        #endregion
    }
}
