using System;
using System.Collections.Generic;

namespace HP.ScalableTest.Core.DataLog.Service
{
    /// <summary>
    /// Event args for the <see cref="DataLogService.SessionDataExpired" /> event.
    /// </summary>
    public class DataLogCleanupEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the session IDs for the data that has expired.
        /// </summary>
        public IEnumerable<string> SessionIds { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogCleanupEventArgs" /> class.
        /// </summary>
        /// <param name="sessionIds">The session IDs.</param>
        /// <exception cref="ArgumentNullException"><paramref name="sessionIds" /> is null.</exception>
        public DataLogCleanupEventArgs(IEnumerable<string> sessionIds)
        {
            SessionIds = sessionIds ?? throw new ArgumentNullException(nameof(sessionIds));
        }
    }
}
