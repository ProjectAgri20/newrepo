﻿using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text.RegularExpressions;
using System.Threading;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Service.Monitor.DigitalSend;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Service.HpcrProxy
{
    internal class HpcrDatabaseMonitor : IDisposable
    {
        private string _connectionString = string.Empty;
        private Timer _refreshTimer;
        private readonly DataLogger _dataLogger;

        /// <summary>
        /// Initializes a new instance of the <see cref="HpcrDatabaseMonitor"/> class.
        /// </summary>
        public HpcrDatabaseMonitor()
            : this(Environment.MachineName, "HPCRArchive")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="HpcrDatabaseMonitor"/> class.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        public HpcrDatabaseMonitor(string hostName, string instanceName, int connectionPort = -1)
        {
            SetupDatabase(hostName, instanceName);
            _dataLogger = new DataLogger(GlobalSettings.WcfHosts[WcfService.DataLog]);

            _refreshTimer = new Timer(RefreshTimerCallback);
            _refreshTimer.Change(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));

            // trigger immediate run
            RefreshTimerCallback(null);
        }

        private void SetupDatabase(string hostName, string instanceName)
        {
            TraceFactory.Logger.Info("Connecting to {0}\\{1} ...".FormatWith(hostName, instanceName));

            try
            {
                TraceFactory.Logger.Info("Attempting connection to " + instanceName);
                var conn = "Server={0}; Database={1}; Trusted_Connection=true; MultipleActiveResultSets=true;"
                                .FormatWith(hostName.Trim(), instanceName);

                TraceFactory.Logger.Info("Connection string: " + conn);
                using (SqlAdapter adapter = new SqlAdapter(conn))
                {
                    TraceFactory.Logger.Info("Adding column StfDigitalSendServerJobId to table ArchiveTable if not existing...");
                    var sql = @"
                        IF COL_LENGTH('ArchiveTable', 'StfDigitalSendServerJobId') IS NULL
                        BEGIN
                            ALTER TABLE ArchiveTable
                            ADD [StfDigitalSendServerJobId] [uniqueidentifier] NULL
                        END
                    ";
                    adapter.ExecuteNonQuery(sql);

                    TraceFactory.Logger.Info("Adding column StfLogId to table ArchiveTable if not existing...");
                    sql = @"
                        IF COL_LENGTH('ArchiveTable', 'StfLogId') IS NULL
                        BEGIN
                            ALTER TABLE ArchiveTable
                            ADD [StfLogId] [uniqueidentifier] NULL
                        END
                    ";
                    adapter.ExecuteNonQuery(sql);

                    sql = @"
                        IF COL_LENGTH('ArchiveTable', 'StfLogStatus') IS NULL
                        BEGIN
                            ALTER TABLE ArchiveTable
                            ADD [StfLogStatus] nvarchar(15) NULL
                        END
                    ";
                    adapter.ExecuteNonQuery(sql);

                }

                TraceFactory.Logger.Info("Connected to database instance {0}\\{1}".FormatWith(hostName, instanceName));
                _connectionString = conn;
                return;
            }
            catch (SqlException ex)
            {
                TraceFactory.Logger.Error("Unsuccessful", ex);
            }

            // None of these connections worked
            throw new InvalidOperationException("Could not connect to database.");
        }

        private void RefreshTimerCallback(object notUsed)
        {
            _refreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
            ProcessRecords();
            _refreshTimer.Change(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        }

        private void ProcessRecords()
        {
            try
            {
                var allRecords = RetrieveRecords().ToList();

                TraceFactory.Logger.Debug("{0} HPCR All records found to process: {1} to insert, {2} to update".FormatWith(allRecords.Count, 0, 0));

                var firstTimeRecords = allRecords.Where(x =>
                        !string.IsNullOrEmpty(x.DocumentName)
                        && string.IsNullOrEmpty(x.StfLogStatus)
                    ).ToList();

                TraceFactory.Logger.Debug("{0} HPCR FirstTime records found to process: {1} to insert, {2} to update".FormatWith(allRecords.Count, firstTimeRecords.Count, 0));

                var updateRecords = allRecords.Where(x =>
                        x.StfLogStatus.EqualsIgnoreCase(StfLogStatus.Partial)
                        && x.DateDelivered.HasValue
                    ).ToList();

                TraceFactory.Logger.Debug("{0} HPCR records found to process: {1} to insert, {2} to update".FormatWith(allRecords.Count, firstTimeRecords.Count, updateRecords.Count));
                using (SqlAdapter adapter = new SqlAdapter(_connectionString))
                {

                    var count = 0;
                    foreach (var record in firstTimeRecords)
                    {
                        count++;
                        TraceFactory.Logger.Debug("Inserting record {0} of {1}"
                            .FormatWith(count, firstTimeRecords.Count));

                        CreateDatalogEntry(record);
                        UpdateHpcrDatabase(adapter, record);
                    }

                    count = 0;
                    foreach (var record in updateRecords)
                    {
                        count++;
                        TraceFactory.Logger.Debug("Updating record {0} of {1}"
                            .FormatWith(count, updateRecords.Count));
                        UpdateDatalogEntry(record);
                        UpdateHpcrDatabase(adapter, record);
                    }
                }
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error processing", ex);
            }
        }

        private void UpdateHpcrDatabase(SqlAdapter adapter, HpcrArchiveRecord record)
        {
            string sql = string.Empty;

            if (string.IsNullOrEmpty(record.SessionId))
            {
                record.StfLogStatus = StfLogStatus.Ignore;
            }
            if ((record.JobType.Equals("HpcrScanToMe") || record.JobType.Equals("HpcrPublicDistributions")) && record.DateDelivered.HasValue)
            {
                sql = @"
                update ArchiveTable 
                set StfDigitalSendServerJobId = '{0}', StfLogId = '{1}', StfLogStatus = '{2}',  prDateDelivered = '{3}'
                where RowIdentifier = {4}
                ".FormatWith(record.StfDigitalSendServerJobId.ToString(), record.StfLogId.ToString(), record.StfLogStatus, record.DateDelivered, record.RowIdentifier);
            }
            else
            {
                sql = @"
                update ArchiveTable 
                set StfDigitalSendServerJobId = '{0}', StfLogId = '{1}', StfLogStatus = '{2}' 
                where RowIdentifier = {3}
                ".FormatWith(record.StfDigitalSendServerJobId.ToString(), record.StfLogId.ToString(), record.StfLogStatus, record.RowIdentifier);
            }
            TraceFactory.Logger.Debug(sql);
            adapter.ExecuteNonQuery(sql);
        }

        public class HpcrArchiveRecord
        {
            public Guid StfLogId { get; set; }

            public Guid StfDigitalSendServerJobId { get; set; }

            public string StfLogStatus { get; set; }
            public int RowIdentifier { get; set; }
            public DateTime? DateSubmitted { get; set; }
            public DateTime? DateDelivered { get; set; }
            public string DocumentName { get; set; }
            public string JobType { get; set; }
            public string FinalStatus { get; set; }
            public string FileType { get; set; }
            public int ScannedPages { get; set; }
            public int FileSizeBytes { get; set; }
            public string DeviceModel { get; set; }
            public string DssVersion { get; set; }

            public string SessionId
            {
                get
                {
                    string result = null;
                    string[] data = DocumentName.Split('-');
                    if (data.Length > 0 && data[0].Length == 8)
                    {
                        result = data[0];
                    }
                    return result;
                }
            }

        }

        /// <summary>
        /// Retrieves the job records from the HPCR database.
        /// </summary>
        public List<HpcrArchiveRecord> RetrieveRecords()
        {
            var result = new List<HpcrArchiveRecord>();
            TraceFactory.Logger.Debug("Retrieving records from HPCR database that should be logged to STF");
            using (SqlAdapter adapter = new SqlAdapter(_connectionString))
            {
                try
                {
                    // Get records with a DocumentName that either haven't been logged (StfLogId = null)
                    // or have an updated delivery date within an expiration period
                    var sql = @"
                        select * from ArchiveTable 
                        where 
                        DATALENGTH([DocumentName]) > 0
                        and ([StfLogStatus] is null or [StfLogStatus] != 'Ignore')
                        and
                        (
                            [StfLogId] is null 
                            or (
                                [StfLogStatus] = 'Partial' 
                                and [prDateDelivered] is not null
                                and [ArchiveDate] < DATEADD(hh, -6, GETDATE())
                            )
							or(
								[StfLogStatus] = 'Partial' 
								and [prDateDelivered] is null								
								and ([prDestination] = 'scantome' or prDestination like '%-%-%-%-%')
							)
                        )
                    ";

                    using (DbDataReader reader = adapter.ExecuteReader(sql))
                    {
                        while (reader.Read())
                        {
                            Guid stfLogId = GetStfLogId(reader);
                            Guid stfDigitalSendServerJobid = GetStfDigitalSendServerJobId(reader);

                            HpcrArchiveRecord hpcrRecord = new HpcrArchiveRecord();

                            hpcrRecord.RowIdentifier = (int)reader[HpcrDatabaseColumns.RowIdentifier];
                            hpcrRecord.DateDelivered = GetValueAsDateTime(reader, HpcrDatabaseColumns.prDateDelivered);
                            hpcrRecord.DateSubmitted = GetValueAsDateTime(reader, HpcrDatabaseColumns.prDateSubmitted);
                            hpcrRecord.JobType = TranslateJobType(GetValueAsString(reader, HpcrDatabaseColumns.prDestination));
                            hpcrRecord.DocumentName = GetValueAsString(reader, HpcrDatabaseColumns.DocumentName);
                            hpcrRecord.FileType = GetValueAsString(reader, HpcrDatabaseColumns.prFinalFormCode);
                            hpcrRecord.FileSizeBytes = GetValueAsInt(reader, HpcrDatabaseColumns.DocumentBytes);
                            hpcrRecord.ScannedPages = GetValueAsInt(reader, HpcrDatabaseColumns.DocumentCount);
                            hpcrRecord.DssVersion = GetValueAsString(reader, HpcrDatabaseColumns.prHPCRServerVersion);
                            hpcrRecord.FinalStatus = GetValueAsString(reader, HpcrDatabaseColumns.prFinalStatusText);
                            hpcrRecord.StfLogStatus = GetValueAsString(reader, HpcrDatabaseColumns.StfLogStatus);
                            hpcrRecord.DeviceModel = GetValueAsString(reader, HpcrDatabaseColumns.DeviceModelName);
                            hpcrRecord.StfLogId = stfLogId;
                            hpcrRecord.StfDigitalSendServerJobId = stfDigitalSendServerJobid;


                            // public distributions and Send to Me are currently inserting two records into the archive table.
                            // ignore the one that has a Guid for the destination
                            if (!hpcrRecord.JobType.StartsWith("second"))
                            {
                                result.Add(hpcrRecord);
                            }
                            else
                            {
                                if (ProcessRecordForwarding(result, hpcrRecord))
                                {
                                    UpdateHpcrDatabase(adapter, hpcrRecord);
                                }
                            }
                        }
                        reader.Close();
                    }

                }
                catch (SqlException sqlEx)
                {
                    TraceFactory.Logger.Error("Error retrieving list of records.  Aborting.", sqlEx);
                    return result;
                }

                TraceFactory.Logger.Debug("Found {0} records.".FormatWith(result.Count));
            }
            return result;
        }

        private static Guid GetStfLogId(DbDataReader reader)
        {
            Guid stfLogId;
            if (reader.IsDBNull(reader.GetOrdinal(HpcrDatabaseColumns.StfLogId)))
            {
                stfLogId = SequentialGuid.NewGuid();
            }
            else
            {
                stfLogId = (Guid)reader[HpcrDatabaseColumns.StfLogId];
            }

            return stfLogId;
        }

        private static Guid GetStfDigitalSendServerJobId(DbDataReader reader)
        {
            Guid stfDigitalSendServerJobid;
            if (!reader.IsDBNull(reader.GetOrdinal(HpcrDatabaseColumns.StfDigitalSendServerJobId)))
            {
                stfDigitalSendServerJobid = (Guid)reader[HpcrDatabaseColumns.StfDigitalSendServerJobId];
            }
            else
            {
                stfDigitalSendServerJobid = Guid.Empty;
            }

            return stfDigitalSendServerJobid;
        }

        private bool ProcessRecordForwarding(List<HpcrArchiveRecord> result, HpcrArchiveRecord hpcrRecord)
        {
            bool processed = false;

            foreach (HpcrArchiveRecord har in result)
            {
                if (har.DocumentName.Equals(hpcrRecord.DocumentName))
                {
                    har.DateDelivered = hpcrRecord.DateDelivered;

                    hpcrRecord.StfLogStatus = StfLogStatus.Completed;
                    hpcrRecord.StfLogId = har.StfLogId;

                    processed = true;
                    break;
                }
            }
            return processed;
        }

        private static int GetValueAsInt(DbDataReader reader, string columnName)
        {
            int result = 0;

            var value = GetValueAsString(reader, columnName);
            int.TryParse(value, out result);

            return result;
        }

        private static string TranslateJobType(string destination)
        {
            string jobType = string.Empty;
            Guid tempGuid = Guid.Empty;

            switch (destination.ToLower())
            {
                case HpcrWorkflows.myfiles:
                    jobType = "ScanToMyFiles";
                    break;
                case HpcrWorkflows.scantofolder:
                    jobType = "ScanToFolder";
                    break;
                case HpcrWorkflows.publicdistributions:
                    jobType = "secondPublicDistributions";
                    break;
                case HpcrWorkflows.personaldistributions:
                    jobType = "PersonalDistributions";
                    break;
                case HpcrWorkflows.scantome:
                    jobType = "HpcrScanToMe";
                    break;
                default:
                    if (destination.Contains("etl.boi.rd.hpicorp"))
                    {
                        jobType = "secondScanToMe";
                    }
                    else if (Guid.TryParse(destination, out tempGuid))
                    {
                        jobType = "HpcrPublicDistributions";
                    }
                    else
                    {
                        jobType = "Unknown HPCR workflow";
                    }
                    break;
            }

            return jobType;
        }

        private static DateTime? GetValueAsDateTime(DbDataReader reader, string columnName)
        {
            return (reader[columnName] != DBNull.Value ? (DateTime?)reader[columnName] : null);
        }

        private static string GetValueAsString(DbDataReader reader, string columnName)
        {
            return (reader[columnName] == DBNull.Value ? string.Empty : reader[columnName].ToString());
        }

        /// <summary>
        /// Creates datalog entry from record information
        /// </summary>
        /// <param name="record">The record.</param>
        private void CreateDatalogEntry(HpcrArchiveRecord record)
        {
            TraceFactory.Logger.Debug("Send datalog entry for RowIdentifier={0}, StfLogId={1} to {2}"
                                .FormatWith(record.RowIdentifier, record.StfLogId, GlobalSettings.WcfHosts[WcfService.DataLog]));
            if (!string.IsNullOrEmpty(record.SessionId))
            {
                DigitalSendServerJobLogger log = new DigitalSendServerJobLogger(record.StfLogId);

                log.SessionId = record.SessionId;
                log.JobType = record.JobType;
                log.ProcessedBy = "HPCR";
                log.DeviceModel = record.DeviceModel;
                log.FileSizeBytes = record.FileSizeBytes;
                log.ScannedPages = (short)record.ScannedPages;
                log.FileType = record.FileType;

                log.DssVersion = record.DssVersion.Equals("") ? "Unknown" : record.DssVersion;

                if (record.DateDelivered.HasValue)
                {
                    log.CompletionDateTime = record.DateDelivered.Value;
                    record.StfLogStatus = StfLogStatus.Completed;
                }
                else
                {
                    record.StfLogStatus = StfLogStatus.Partial;
                    log.CompletionDateTime = DateTime.Parse("01/01/1800 12:00:00 AM");
                }
                record.StfDigitalSendServerJobId = log.DigitalSendServerJobId;
                
                log.CompletionStatus = record.FinalStatus;
                log.FileName = record.DocumentName;
                _dataLogger.Submit(log);
            }
        }

        /// <summary>
        /// Updates a previously made datalog entry with updated completion time and completion status
        /// </summary>
        /// <param name="record">The record.</param>
        private void UpdateDatalogEntry(HpcrArchiveRecord record)
        {
            TraceFactory.Logger.Debug("Update datalog entry for RowIdentifier={0}, StfLogId={1}"
                                .FormatWith(record.RowIdentifier, record.StfLogId));
            if (!string.IsNullOrEmpty(record.SessionId))
            {
                DigitalSendServerJobLogger log = new DigitalSendServerJobLogger(record.StfLogId);
                log.DigitalSendServerJobId = record.StfDigitalSendServerJobId;
                log.SessionId = record.SessionId;

                if (record.DateDelivered.HasValue)
                {
                    log.CompletionDateTime = record.DateDelivered.Value;
                    log.CompletionStatus = record.FinalStatus;
                    _dataLogger.Update(log);

                    record.StfLogStatus = StfLogStatus.Completed;
                }
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_refreshTimer != null)
            {
                _refreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
                _refreshTimer.Dispose();
                _refreshTimer = null;
            }
        }

        #endregion
    }
    internal class HpcrWorkflows
    {
        public const string myfiles = "myfiles";
        public const string scantofolder = "scantofolder";
        public const string publicdistributions = "publicdistributions";
        public const string personaldistributions = "personaldistributions";
        public const string scantome = "scantome";
    }
    internal class HpcrDatabaseColumns
    {
        public const string StfLogId = "StfLogId";
        public const string RowIdentifier = "RowIdentifier";
        public const string prDateDelivered = "prDateDelivered";
        public const string prDateSubmitted = "prDateSubmitted";
        public const string prDestination = "prDestination";
        public const string DocumentName = "DocumentName";
        public const string prFinalFormCode = "prFinalFormCode";
        public const string DocumentBytes = "DocumentBytes";
        public const string DocumentCount = "DocumentCount";
        public const string prHPCRServerVersion = "prHPCRServerVersion";
        public const string prFinalStatusText = "prFinalStatusText";
        public const string StfLogStatus = "StfLogStatus";
        public const string DeviceModelName = "DeviceModelName";
        public const string StfDigitalSendServerJobId = "StfDigitalSendServerJobId";
    }
    internal class StfLogStatus
    {
        public const string Completed = "Completed";
        public const string Partial = "Partial";
        public const string Ignore = "Ignore";
    }
}
