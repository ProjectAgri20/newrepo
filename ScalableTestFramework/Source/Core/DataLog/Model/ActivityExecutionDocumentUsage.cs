using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// A document used during an <see cref="ActivityExecution" />.
    /// </summary>
    [DebuggerDisplay("{DocumentName,nq}")]
    public sealed class ActivityExecutionDocumentUsage
    {
        /// <summary>
        /// Gets or sets the unique identifier for this activity execution document usage.
        /// </summary>
        public Guid ActivityExecutionDocumentUsageId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the session that performed the activity using this document.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="ActivityExecution" /> that used this document.
        /// </summary>
        public Guid ActivityExecutionId { get; set; }

        /// <summary>
        /// Gets or sets the name of the document.
        /// </summary>
        public string DocumentName { get; set; }
    }
}
