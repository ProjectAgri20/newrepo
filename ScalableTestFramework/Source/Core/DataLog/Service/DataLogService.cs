using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.DataLog.Service
{
    /// <summary>
    /// Standard implementation of <see cref="IDataLogService" />.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public sealed class DataLogService : IDataLogService, IDisposable
    {
        private readonly DataLogConnectionString _dataLogConnectionString;
        private readonly DataLogDatabaseWriter _writer;
        private readonly DataLogDatabaseWriter _alternateWriter;

        private DataLogCache _cache;
        private TimeSpan _cacheCheckFrequency;
        private readonly Timer _cacheCheckTimer;

        private readonly DataLogCleanup _cleanup;
        private readonly TimeSpan _cleanupCheckFrequency = TimeSpan.FromHours(6);
        private readonly Timer _cleanupCheckTimer;

        /// <summary>
        /// Occurs when cached data operations are retried.
        /// </summary>
        public event EventHandler<DataLogCacheEventArgs> CacheOperationsRetried;

        /// <summary>
        /// Occurs when session data has expired.
        /// </summary>
        public event EventHandler<DataLogCleanupEventArgs> SessionDataExpired;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogService" /> class.
        /// </summary>
        /// <param name="dataLogConnectionString">The <see cref="DataLogConnectionString" /> used to connect to the database.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLogConnectionString" /> is null.</exception>
        public DataLogService(DataLogConnectionString dataLogConnectionString)
        {
            _dataLogConnectionString = dataLogConnectionString ?? throw new ArgumentNullException(nameof(dataLogConnectionString));
            _writer = new DataLogDatabaseWriter(dataLogConnectionString);
            _cleanup = new DataLogCleanup(dataLogConnectionString);

            _cacheCheckTimer = new Timer(RetryCacheEntries, null, Timeout.Infinite, Timeout.Infinite);
            _cleanupCheckTimer = new Timer(DeleteExpiredSessions, null, _cleanupCheckFrequency, _cleanupCheckFrequency);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogService" /> class.
        /// </summary>
        /// <param name="dataLogConnectionString">The <see cref="DataLogConnectionString" /> used to connect to the database.</param>
        /// <param name="alternateConnectionString">A <see cref="DataLogConnectionString" /> used to connect to an alternate database if the primary operation fails.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="dataLogConnectionString" /> is null.
        /// <para>or</para>
        /// <paramref name="alternateConnectionString" /> is null.
        /// </exception>
        public DataLogService(DataLogConnectionString dataLogConnectionString, DataLogConnectionString alternateConnectionString)
            : this(dataLogConnectionString)
        {
            _alternateWriter = new DataLogDatabaseWriter(alternateConnectionString);
        }

        /// <summary>
        /// Initializes the cache used to store failed transaction for subsequent retries.
        /// </summary>
        /// <param name="cacheLocation">The cache location.</param>
        /// <param name="checkFrequency">The check frequency.</param>
        public void InitializeCache(DirectoryInfo cacheLocation, TimeSpan checkFrequency)
        {
            LogDebug($"Initializing cache at {cacheLocation} with retry frequency {checkFrequency}.");

            _cache = _alternateWriter == null ? new DataLogCache(cacheLocation, _writer) : new DataLogCache(cacheLocation, _writer, _alternateWriter);
            _cacheCheckFrequency = checkFrequency;
            _cacheCheckTimer.Change(_cacheCheckFrequency, _cacheCheckFrequency);
        }

        /// <summary>
        /// Inserts data from the specified <see cref="LogTableRecord" /> into the table designated by the <see cref="LogTableDefinition" />.
        /// </summary>
        /// <param name="table">The <see cref="LogTableDefinition" />.</param>
        /// <param name="record">The <see cref="LogTableRecord" />.</param>
        /// <returns><c>true</c> if submission was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="table" /> is null.
        /// <para>or</para>
        /// <paramref name="record" /> is null.
        /// </exception>
        public bool Insert(LogTableDefinition table, LogTableRecord record)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            LogTrace($"INSERT request received for {table.Name} (primary key = {record[table.PrimaryKey]})");
            DataLogDatabaseResult result = _writer.Insert(table, record);
            bool insertSuccessful = result.Success;

            // If the operation failed, try the alternate (if one exists)
            if (!insertSuccessful && _alternateWriter != null)
            {
                LogTrace($"INSERT failed.  Attempting insert to alternate database.");
                DataLogDatabaseResult alternateResult = _alternateWriter.Insert(table, record);
                insertSuccessful = alternateResult.Success;
            }

            // If the operation hasn't succeeded, check to see if we should add this to the cache.
            if (!insertSuccessful && _cache != null)
            {
                if (result.Error.Contains("Violation of PRIMARY KEY constraint", StringComparison.OrdinalIgnoreCase))
                {
                    // Bypass the cache for primary key violations
                    result.Retries = -1;
                    CacheOperationsRetried?.Invoke(this, new DataLogCacheEventArgs(new[] { result }));
                }
                else
                {
                    _cache.Add(table, record, true);
                }
            }

            return insertSuccessful;
        }

        /// <summary>
        /// Updates data from the specified <see cref="LogTableRecord" /> into the table designated by the <see cref="LogTableDefinition" />.
        /// </summary>
        /// <param name="table">The <see cref="LogTableDefinition" />.</param>
        /// <param name="record">The <see cref="LogTableRecord" />.</param>
        /// <returns><c>true</c> if submission was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="table" /> is null.
        /// <para>or</para>
        /// <paramref name="record" /> is null.
        /// </exception>
        public bool Update(LogTableDefinition table, LogTableRecord record)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            LogTrace($"UPDATE request received for {table.Name} (primary key = {record[table.PrimaryKey]})");
            DataLogDatabaseResult result = _writer.Update(table, record);
            bool updateSuccessful = result.Success;

            // If the operation failed, try the alternate (if one exists)
            if (!updateSuccessful && _alternateWriter != null)
            {
                LogTrace($"UPDATE failed.  Attempting update to alternate database.");
                DataLogDatabaseResult alternateResult = _alternateWriter.Update(table, record);
                updateSuccessful = alternateResult.Success;
            }

            // If the operation hasn't succeeded, check to see if we should add this to the cache.
            if (!updateSuccessful)
            {
                _cache?.Add(table, record, false);
            }

            return updateSuccessful;
        }

        /// <summary>
        /// Gets the activity counts for the specified session.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The activity counts for the specified session, grouped by result.</returns>
        public Dictionary<PluginResult, int> GetSessionActivityCounts(string sessionId)
        {
            using (DataLogContext context = new DataLogContext(_dataLogConnectionString))
            {
                return context.SessionData(sessionId).ActivityCounts(SessionActivityGroupFields.Status)
                    .ToDictionary(n => EnumUtil.Parse<PluginResult>(n.Key.Status), n => n.Count);
            }
        }

        private void RetryCacheEntries(object state)
        {
            _cacheCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
            IEnumerable<DataLogDatabaseResult> results = _cache?.RetryFromCache();
            if (results?.Any() == true)
            {
                CacheOperationsRetried?.Invoke(this, new DataLogCacheEventArgs(results));
            }
            _cacheCheckTimer.Change(_cacheCheckFrequency, _cacheCheckFrequency);
        }

        private void DeleteExpiredSessions(object state)
        {
            if (DateTime.Now.TimeOfDay < _cleanupCheckFrequency)
            {
                _cleanupCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
                IEnumerable<string> sessions = _cleanup.RemoveExpiredSessionData();
                if (sessions.Any())
                {
                    SessionDataExpired?.Invoke(this, new DataLogCleanupEventArgs(sessions));
                }
                _cleanupCheckTimer.Change(_cleanupCheckFrequency, _cleanupCheckFrequency);
            }
        }

        #region Explicit IDataLogService Members

        bool IDataLogService.Ping()
        {
            return true;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _cacheCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _cacheCheckTimer.Dispose();

            _cleanupCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _cleanupCheckTimer.Dispose();
        }

        #endregion
    }
}
