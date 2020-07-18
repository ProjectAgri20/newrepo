using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.DataLog.Service
{
    /// <summary>
    /// Manages a cache of failed data log transactions that will be retried later.
    /// </summary>
    public sealed class DataLogCache
    {
        private static readonly TimeSpan _maximumRetryTime = TimeSpan.FromHours(1);

        private readonly DirectoryInfo _cacheLocation;
        private readonly DataLogDatabaseWriter _databaseWriter;
        private readonly DataLogDatabaseWriter _alternateWriter;
        private readonly DataContractSerializer _serializer;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogCache" /> class.
        /// </summary>
        /// <param name="cacheLocation">The directory location to store cache files.</param>
        /// <param name="databaseWriter">The <see cref="DataLogDatabaseWriter" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cacheLocation" /> is null.
        /// <para>or</para>
        /// <paramref name="databaseWriter" /> is null.
        /// </exception>
        public DataLogCache(DirectoryInfo cacheLocation, DataLogDatabaseWriter databaseWriter)
        {
            _cacheLocation = cacheLocation ?? throw new ArgumentNullException(nameof(cacheLocation));
            _databaseWriter = databaseWriter ?? throw new ArgumentNullException(nameof(databaseWriter));
            _serializer = new DataContractSerializer(typeof(DataLogCacheData));
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogCache" /> class.
        /// </summary>
        /// <param name="cacheLocation">The directory location to store cache files.</param>
        /// <param name="databaseWriter">The <see cref="DataLogDatabaseWriter" />.</param>
        /// <param name="alternateWriter">The alternate <see cref="DataLogDatabaseWriter" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="cacheLocation" /> is null.
        /// <para>or</para>
        /// <paramref name="databaseWriter" /> is null.
        /// <para>or</para>
        /// <paramref name="alternateWriter" /> is null.
        /// </exception>
        public DataLogCache(DirectoryInfo cacheLocation, DataLogDatabaseWriter databaseWriter, DataLogDatabaseWriter alternateWriter)
            : this(cacheLocation, databaseWriter)
        {
            _alternateWriter = alternateWriter ?? throw new ArgumentNullException(nameof(alternateWriter));
        }

        /// <summary>
        /// Adds the specified log table data to the cache.
        /// </summary>
        /// <param name="table">The <see cref="LogTableDefinition" />.</param>
        /// <param name="record">The <see cref="LogTableRecord" />.</param>
        /// <param name="isInsert">if set to <c>true</c> this data should be processed as an insert.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="table" /> is null.
        /// <para>or</para>
        /// <paramref name="record" /> is null.
        /// </exception>
        public void Add(LogTableDefinition table, LogTableRecord record, bool isInsert)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            string operation = isInsert ? "INSERT" : "UPDATE";
            string fileName = $"{table.Name} {operation} {record[table.PrimaryKey]}.xml";
            FileInfo file = new FileInfo(Path.Combine(_cacheLocation.FullName, fileName));

            LogTrace($"Adding cache file {file.Name}");
            DataLogCacheData cacheData = new DataLogCacheData(table, record, isInsert);
            WriteCacheData(file, cacheData);
        }

        /// <summary>
        /// Retries all data log operations that are in the cache, and removes those that succeed.
        /// </summary>
        /// <returns>A <see cref="DataLogDatabaseResult" /> collection representing the operations that were retried.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public IEnumerable<DataLogDatabaseResult> RetryFromCache()
        {
            if (!Directory.Exists(_cacheLocation.FullName))
            {
                return Enumerable.Empty<DataLogDatabaseResult>();
            }

            // Ignore files that have exceeded the maximum retry time.
            DateTime cutoff = DateTime.Now - _maximumRetryTime;

            bool loggedStart = false;
            int fileErrors = 0;
            List<DataLogDatabaseResult> results = new List<DataLogDatabaseResult>();
            foreach (FileInfo file in _cacheLocation.EnumerateFiles("*.xml").Where(n => n.CreationTime > cutoff).OrderBy(n => n.CreationTime))
            {
                if (!loggedStart)
                {
                    LogDebug("Retrying from cache...");
                    loggedStart = true;
                }

                try
                {
                    LogTrace($"Reading cache file {file.Name}");
                    DataLogCacheData data = ReadCacheData(file);
                    data.Retries++;

                    DataLogDatabaseResult result = data.IsInsert ?
                        _databaseWriter.Insert(data.Table, data.Record) :
                        _databaseWriter.Update(data.Table, data.Record);
                    bool operationSuccessful = result.Success;

                    if (!operationSuccessful && _alternateWriter != null)
                    {
                        DataLogDatabaseResult alternateResult = data.IsInsert ?
                            _alternateWriter.Insert(data.Table, data.Record) :
                            _alternateWriter.Update(data.Table, data.Record);
                        operationSuccessful = alternateResult.Success;
                    }

                    if (operationSuccessful)
                    {
                        LogTrace($"Cache operation successful.  Deleting file {file.Name}.");
                        try
                        {
                            Retry.WhileThrowing<IOException>(() => file.Delete(), 5, TimeSpan.FromSeconds(1));
                        }
                        catch (IOException ex)
                        {
                            LogDebug($"File {file.Name} could not be deleted: {ex.Message}");
                        }
                    }
                    else
                    {
                        LogTrace($"Cache operation failed.  {file.Name} has been retried {data.Retries} times.");
                        WriteCacheData(file, data);
                    }

                    result.Retries = data.Retries;
                    results.Add(result);
                }
                catch (Exception ex)
                {
                    // Do nothing - could be a fluke, or a file that we can't read
                    LogTrace($"Unknown error: {ex.Message}");
                    fileErrors++;
                }
            }

            if (results.Any())
            {
                LogDebug($"Cache retry results: {results.Count(n => n.Success)} succeeded, {results.Count(n => !n.Success)} failed, {fileErrors} cache errors.");
            }
            return results;
        }

        private void WriteCacheData(FileInfo file, DataLogCacheData cacheData)
        {
            // Create the cache location - Create() will no-op if it already exists
            _cacheLocation.Create();

            using (FileStream stream = file.Create())
            {
                // Using a DataContractSerializer instead of HP.ScalableTest.Framework.Serializer
                // because it produces about 50% less data and will keep the cache smaller.
                _serializer.WriteObject(stream, cacheData);
            }
        }

        private DataLogCacheData ReadCacheData(FileInfo file)
        {
            using (FileStream stream = file.OpenRead())
            {
                return (DataLogCacheData)_serializer.ReadObject(stream);
            }
        }

        [DataContract]
        private sealed class DataLogCacheData
        {
            [DataMember]
            public LogTableDefinition Table { get; set; }

            [DataMember]
            public LogTableRecord Record { get; set; }

            [DataMember]
            public bool IsInsert { get; set; }

            [DataMember]
            public int Retries { get; set; }

            public DataLogCacheData(LogTableDefinition table, LogTableRecord record, bool isInsert)
            {
                Table = table;
                Record = record;
                IsInsert = isInsert;
                Retries = 0;
            }
        }
    }
}
