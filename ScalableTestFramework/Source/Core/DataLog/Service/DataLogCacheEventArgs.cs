using System;
using System.Collections.Generic;

namespace HP.ScalableTest.Core.DataLog.Service
{
    /// <summary>
    /// Event args for the <see cref="DataLogService.CacheOperationsRetried" /> event.
    /// </summary>
    public sealed class DataLogCacheEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="DataLogDatabaseResult" /> collection representing the outcomes of the cache retry operation(s).
        /// </summary>
        public IEnumerable<DataLogDatabaseResult> Results { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogCacheEventArgs"/> class.
        /// </summary>
        /// <param name="results">The <see cref="DataLogDatabaseResult" /> collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="results" /> is null.</exception>
        public DataLogCacheEventArgs(IEnumerable<DataLogDatabaseResult> results)
        {
            Results = results ?? throw new ArgumentNullException(nameof(results));
        }
    }
}
