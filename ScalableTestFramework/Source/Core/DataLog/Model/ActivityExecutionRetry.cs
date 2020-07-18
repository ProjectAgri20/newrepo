using System;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// The intermediate result of an activity execution that resulted in a retry.
    /// </summary>
    public sealed class ActivityExecutionRetry
    {
        /// <summary>
        /// Gets or sets the unique identifier for this activity execution retry.
        /// </summary>
        public Guid ActivityExecutionRetriesId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the session that performed this activity.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the activity execution to which this retry pertains.
        /// </summary>
        public Guid ActivityExecutionId { get; set; }

        /// <summary>
        /// Gets or sets the end state of the activity execution attempt.
        /// </summary>
        public string Status { get; set; }

        /// <summary>
        /// Gets or sets the message describing the result of the activity, if one was specified.
        /// </summary>
        public string ResultMessage { get; set; }

        /// <summary>
        /// Gets or sets the category used to group similar execution results, if one was specified.
        /// </summary>
        public string ResultCategory { get; set; }

        /// <summary>
        /// Gets or sets the time when the activity retry started.
        /// </summary>
        public DateTime? RetryStartDateTime { get; set; }
    }
}
