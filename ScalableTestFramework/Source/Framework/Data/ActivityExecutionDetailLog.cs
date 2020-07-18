using System;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework.Data
{
    /// <summary>
    /// Logs an informational message that provides additional detail about the execution of a plugin activity.
    /// </summary>
    public sealed class ActivityExecutionDetailLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "ActivityExecutionDetail";

        /// <summary>
        /// Gets the detail label.
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string Label { get; }

        /// <summary>
        /// Gets the detail message.
        /// </summary>
        [DataLogProperty(MaxLength = 1024)]
        public string Message { get; }

        /// <summary>
        /// Gets the detail date time.
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset DetailDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityExecutionDetailLog" /> class.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <param name="label">The label.</param>
        /// <param name="message">The message.</param>
        public ActivityExecutionDetailLog(PluginExecutionData executionData, string label, string message)
            : this(executionData, label, message, DateTimeOffset.Now)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityExecutionDetailLog"/> class.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <param name="label">The label.</param>
        /// <param name="message">The message.</param>
        /// <param name="timestamp">The timestamp.</param>
        public ActivityExecutionDetailLog(PluginExecutionData executionData, string label, string message, DateTimeOffset timestamp)
            : base(executionData)
        {
            Label = label;
            Message = message;
            DetailDateTime = timestamp;
        }
    }
}
