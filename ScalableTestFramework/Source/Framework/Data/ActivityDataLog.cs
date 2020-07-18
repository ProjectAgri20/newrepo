using System;
using System.Diagnostics;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework.Data
{
    /// <summary>
    /// Base class for logging data associated with a plugin activity execution.
    /// Child classes can decorate properties with a <see cref="DataLogPropertyAttribute" />
    /// to define the schema for the data to be logged.
    /// </summary>
    public abstract class ActivityDataLog
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Guid _recordId;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _sessionId;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private Guid _activityExecutionId;

        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public abstract string TableName { get; }

        /// <summary>
        /// The unique identifier for this record.
        /// </summary>
        [DataLogProperty(-3)]
        public Guid RecordId => _recordId;

        /// <summary>
        /// The ID of the test session where this data was collected.
        /// </summary>
        [DataLogProperty(-2)]
        public string SessionId => _sessionId;

        /// <summary>
        /// The unique identifier for the activity execution that produced/collected this data.
        /// </summary>
        [DataLogProperty(-1)]
        public Guid ActivityExecutionId => _activityExecutionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityDataLog" /> class
        /// with an automatically-generated record ID.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="executionData" /> is null.</exception>
        protected ActivityDataLog(PluginExecutionData executionData)
            : this(executionData, SequentialGuid.NewGuid())
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ActivityDataLog" /> class
        /// with the specified record ID.
        /// </summary>
        /// <param name="executionData">The <see cref="PluginExecutionData" />.</param>
        /// <param name="recordId">The record ID.</param>
        /// <exception cref="ArgumentNullException"><paramref name="executionData" /> is null.</exception>
        protected ActivityDataLog(PluginExecutionData executionData, Guid recordId)
        {
            if (executionData == null)
            {
                throw new ArgumentNullException(nameof(executionData));
            }

            _recordId = recordId;
            _sessionId = executionData.SessionId;
            _activityExecutionId = executionData.ActivityExecutionId;
        }
    }
}
