using System;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework.Data
{
    /// <summary>
    /// Logging class used by <see cref="PluginRetryManager" /> to track retried plugin executions.
    /// </summary>
    internal sealed class ActivityRetryLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "ActivityExecutionRetries";

        /// <summary>
        /// Gets the status of the activity execution that is being logged.
        /// </summary>
        [DataLogProperty(MaxLength = 20)]
        public string Status { get; }

        /// <summary>
        /// Gets the result message of the activity execution that is being logged.
        /// </summary>
        [DataLogProperty(MaxLength = 1024, TruncationAllowed = true)]
        public string ResultMessage { get; }

        /// <summary>
        /// Gets the result category of the activity execution that is being logged.
        /// </summary>
        [DataLogProperty(MaxLength = 1024, TruncationAllowed = true)]
        public string ResultCategory { get; }

        /// <summary>
        /// Gets the time when this activity execution was started.
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset RetryStartDateTime { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityRetryLog" /> class.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <param name="result">The <see cref="PluginExecutionResult" />.</param>
        public ActivityRetryLog(PluginExecutionData executionData, PluginExecutionResult result)
            : base(executionData)
        {
            Status = result.Result.ToString();
            ResultMessage = result.Message;
            ResultCategory = result.Category;
            RetryStartDateTime = DateTimeOffset.Now;
        }
    }
}
