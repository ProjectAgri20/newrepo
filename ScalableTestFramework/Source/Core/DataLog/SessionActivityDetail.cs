using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Additional information from an activity performed during a test session.
    /// </summary>
    [DebuggerDisplay("{Label,nq}")]
    public sealed class SessionActivityDetail
    {
        /// <summary>
        /// Gets the detail label.
        /// </summary>
        public string Label { get; internal set; }

        /// <summary>
        /// Gets the detail message.
        /// </summary>
        public string Message { get; internal set; }

        /// <summary>
        /// Gets the time when this detail was recorded.
        /// </summary>
        public DateTimeOffset DetailDateTime { get; internal set; }
    }
}
