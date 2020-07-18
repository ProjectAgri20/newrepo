using System;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// Additional information stored with an <see cref="ActivityExecution" />.
    /// </summary>
    public sealed class ActivityExecutionDetail
    {
        /// <summary>
        /// Gets or sets the unique identifier for this activity execution detail.
        /// </summary>
        public Guid ActivityExecutionDetailId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the session that performed the associated activity.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the associated <see cref="ActivityExecution" />.
        /// </summary>
        public Guid ActivityExecutionId { get; set; }

        /// <summary>
        /// Gets or sets the detail label.
        /// </summary>
        public string Label { get; set; }

        /// <summary>
        /// Gets or sets the detail message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the time when this detail was recorded.
        /// </summary>
        public DateTime DetailDateTime { get; set; }
    }
}
