using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// A server used during an <see cref="ActivityExecution" />.
    /// </summary>
    [DebuggerDisplay("{ServerName,nq}")]
    public sealed class ActivityExecutionServerUsage
    {
        /// <summary>
        /// Gets or sets the unique identifier for this activity execution server usage.
        /// </summary>
        public Guid ActivityExecutionServerUsageId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the session that performed the activity using this server.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="ActivityExecution" /> that used this server.
        /// </summary>
        public Guid ActivityExecutionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the server.
        /// </summary>
        public string ServerName { get; set; }
    }
}
