using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Data.SqlClient;
using System.Text;
using System.IO;
using System.Linq;
using System.Xml.Linq;
using System.Threading;
using HP.ScalableTest.Xml;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Service.Monitor.DigitalSend
{
    internal class DigitalSendDatabaseMonitor : StfMonitor
    {
        private string _connectionString = string.Empty;
        private string _sessionId = string.Empty;
        private Timer _refreshTimer = null;
        private List<Type> _retryExceptions = new List<Type>() { typeof(SqlException) }; // List of exception types to watch for when retrying database operations
        private Version _dateOffsetVersion = new Version("5.02.01.553");
        private readonly DataLogger _dataLogger = null;

        //Enumeration of Log tables used by DSS where we could find job data
        private enum DSSJobLogTable
        {
            JobLogDevice,
            JobLogMaster
        }

        public event EventHandler<SessionEventArgs> RetrievedSession;

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSendDatabaseMonitor"/> class.
        /// The DSS Service uses Port 5213.
        /// </summary>
        /// <param name="monitorConfig">The configuration data needed to start this instance.</param>
        public DigitalSendDatabaseMonitor(MonitorConfig monitorConfig)
            : base(monitorConfig)
        {
            RefreshConfig(monitorConfig);

            _dataLogger = new DataLogger(GlobalSettings.WcfHosts[WcfService.DataLog]);
            SetupDatabase(Configuration.MonitorLocation, Configuration.DatabaseInstanceName, Configuration.ConnectionPort);
            _refreshTimer = new Timer(RefreshTimerCallback);
            StopMonitoring();
        }

        /// <summary>
        /// Refreshes monitor configuration for this instance.
        /// </summary>
        public override void RefreshConfig(MonitorConfig monitorConfig)
        {
            Configuration = LegacySerializer.DeserializeXml<DatabaseMonitorConfig>(monitorConfig.Configuration);
        }

        /// <summary>
        /// Starts monitoring.
        /// </summary>
        public override void StartMonitoring()
        {
            _refreshTimer.Change(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Stops monitoring.
        /// </summary>
        public override void StopMonitoring()
        {
            _refreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Gets the host name of the database being monitored.
        /// </summary>
        public override string MonitorLocation
        {
            get { return Configuration.MonitorLocation; }
        }

        /// <summary>
        /// The configuration needed to start an output monitor.
        /// </summary>
        protected DatabaseMonitorConfig Configuration { get; private set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="instanceName"></param>
        /// <param name="port"></param>
        private void SetupDatabase(string hostName, string instanceName, int port)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder()
            {
                InitialCatalog = "DSS_MACHINE",
                IntegratedSecurity = true,
                MultipleActiveResultSets = true
            };

            // If a port is specified, use it.  Otherwise use default port settings.
            StringBuilder dataSource = new StringBuilder(hostName);
            dataSource.Append("\\");
            dataSource.Append(instanceName);
            if (port > 0)
            {
                dataSource.Append(",");
                dataSource.Append(port);
            }
            builder.DataSource = dataSource.ToString();


            TraceFactory.Logger.Debug("Attempting connection to " + builder.DataSource);

            _connectionString = builder.ToString();
            using (SqlAdapter adapter = new SqlAdapter(_connectionString))
            {
                adapter.ExecuteNonQuery(Resource.CreateTransactionTableSql);
                adapter.ExecuteNonQuery(Resource.DeleteTriggersSql);
                adapter.ExecuteNonQuery(Resource.CreateInsertTriggerSql);
            }

            TraceFactory.Logger.Debug("Connected to database instance: " + builder.DataSource);
        }

        private void RefreshTimerCallback(object notUsed)
        {
            StopMonitoring();
            RetrieveRecords();
            StartMonitoring();
        }

        /// <summary>
        /// Gets the most recent session ID from the database.
        /// </summary>
        /// <returns></returns>
        public string GetMostRecentSession()
        {
            using (SqlAdapter adapter = new SqlAdapter(_connectionString))
            {
                DbDataReader reader = adapter.ExecuteReader(Resource.RecentSessionSql);
                while (reader.Read())
                {
                    string sessionId = GetSessionId(reader);
                    if (!string.IsNullOrEmpty(sessionId))
                    {
                        _sessionId = sessionId;
                        return _sessionId;
                    }
                }
                reader.Close();
            }

            return string.Empty;
        }

        /// <summary>
        /// Retrieves the job records from the DSS database.
        /// For the records that are not found in the database, the following may be logged to the data logger:
        /// unavailable - The job ID was found in the DSS database, and partial data was retrieved for the job.  Represents job data that could not be retrieved.
        /// unknown - While attempting to find the job ID in the database, a database error occurred, and after retrying the operation several times, processing moved on.
        /// missing - The job was not found in either the Master or the Device table.
        /// </summary>
        public void RetrieveRecords()
        {
            TraceFactory.Logger.Debug("Retrieving Job IDs from STFTransactionLog table.");
            using (SqlAdapter adapter = new SqlAdapter(_connectionString))
            {
                List<Guid> jobIds = new List<Guid>(); //List of records that have been updated

                try
                {
                    using (DbDataReader idsReader = adapter.ExecuteReader(Resource.TransactionRetrieveSql))
                    {
                        while (idsReader.Read())
                        {
                            jobIds.Add((Guid)idsReader["JobId"]);
                        }
                        idsReader.Close();
                    }
                }
                catch (SqlException sqlEx)
                {
                    TraceFactory.Logger.Error("Error retrieving list of Job IDs.  Aborting.", sqlEx);
                    return;
                }

                TraceFactory.Logger.Debug("Found {0} records.".FormatWith(jobIds.Count));

                if (jobIds.Any())
                {
                    // Capture list of job IDs now while we have the entire list, to be used to cleanup the transaction table later.
                    string jobIdList = "'" + string.Join("','", jobIds) + "'";
                    string notFoundText = "<missing>";

                    try
                    {
                        //Log data from JobLogMaster.
                        LogData(adapter, DSSJobLogTable.JobLogMaster, Resource.JobLogMasterSql.FormatWith(jobIdList), ref jobIds);
                    }
                    catch (SqlException sqlEx)
                    {
                        TraceFactory.Logger.Error("Unable to process JobLogMaster.", sqlEx);
                        notFoundText = "<unknown>";
                    }

                    //Check to see if there were any jobs that weren't processed
                    if (jobIds.Any())
                    {
                        // Recreate list of job IDs, since some of the jobs have already been processed and removed from jobIds
                        string unprocessedJobIdList = "'" + string.Join("','", jobIds) + "'";

                        try
                        {
                            //Log data from JobLogDevice.
                            LogData(adapter, DSSJobLogTable.JobLogDevice, Resource.JobLogDeviceSql.FormatWith(unprocessedJobIdList), ref jobIds);
                        }
                        catch (SqlException sqlEx)
                        {
                            TraceFactory.Logger.Error("Unable to process JobLogDevice.", sqlEx);
                            notFoundText = "<unknown>";
                        }
                    }

                    // Any job Ids left over at this point were not found in either the master nor the device tables
                    // Log an entry in the data log to flag data that was not found.
                    foreach (Guid jobId in jobIds)
                    {
                        DigitalSendServerJobLogger log = CreateLogForNotFound(jobId, notFoundText);
                        SubmitLog(log);
                    }

                    //At this point, all jobs Ids have been processed
                    jobIds.Clear();

                    //Remove the JobIds from the transaction table.
                    string sql = Resource.TransactionCleanupSql.FormatWith(jobIdList);
                    //TraceFactory.Logger.Debug(sql);
                    Retry.WhileThrowing(() =>
                    {
                        adapter.ExecuteNonQuery(sql);
                    }, 5, TimeSpan.FromSeconds(5), _retryExceptions);
                }
            }
        }

        /// <summary>
        /// Gets the data for each job ID and logs it
        /// </summary>
        private void LogData(SqlAdapter adapter, DSSJobLogTable logTable, string sqlText, ref List<Guid> jobIds)
        {
            TraceFactory.Logger.Debug("Processing {0}.".FormatWith(logTable.ToString()));

            DbDataReader jobsReader = null;
            Retry.WhileThrowing(() =>
            {
                jobsReader = adapter.ExecuteReader(sqlText);
            }, 3, TimeSpan.FromSeconds(5), _retryExceptions);

            try
            {
                bool continueRead = true;
                while (continueRead)
                {
                    try
                    {
                        continueRead = jobsReader.Read();
                    }
                    catch (SqlException sqlEx)
                    {
                        if (sqlEx.Number == 1205)
                        {
                            TraceFactory.Logger.Debug("Deadlock encountered.  Continue to process.");
                        }
                        else
                        {
                            TraceFactory.Logger.Error(sqlEx); //Unexpected SQL exception.
                        }
                        continue; //Skip to the next record
                    }

                    if (continueRead)
                    {
                        // Successful read. Log the entry
                        DigitalSendServerJobLogger log = null;
                        switch (logTable)
                        {
                            case DSSJobLogTable.JobLogDevice:
                                log = CreateLogFromDevice(jobsReader);
                                break;
                            case DSSJobLogTable.JobLogMaster:
                                log = CreateLogFromMaster(jobsReader);
                                break;
                        }

                        SubmitLog(log);
                        TraceFactory.Logger.Debug("Found job with ID " + log.DigitalSendJobId.ToString());
                        //Job successfully logged.  Remove it from the list
                        jobIds.Remove(log.DigitalSendJobId);

                        // See if we've found a new session ID
                        if (log.SessionId != _sessionId)
                        {
                            _sessionId = log.SessionId;
                            if (RetrievedSession != null)
                            {
                                RetrievedSession(this, new SessionEventArgs(log.SessionId));
                            }
                        }
                    }
                }
            }
            finally
            {
                if (jobsReader != null)
                {
                    jobsReader.Close();
                    jobsReader.Dispose();
                }
            }
        }

        private DigitalSendServerJobLogger CreateLogFromMaster(IDataRecord reader)
        {
            DigitalSendServerJobLogger log = new DigitalSendServerJobLogger((Guid)reader["JobId"]);
            log.JobType = reader["JobType"] as string;
            log.DssVersion = reader["DssSoftwareVersion"] as string;
            log.CompletionDateTime = ApplyLocalOffset((DateTime)reader["CompletionTime"], log.DssVersion);
            log.CompletionStatus = reader["JobCompletionStatus"] as string;
            log.FileName = GetFileName(reader);
            log.FileSizeBytes = reader["DeliveredFileSize"] as int?;
            log.FileType = reader["FileType"] as string;
            log.ScannedPages = (short?)(reader["ScannedPages"] as int?);

            log.ProcessedBy = reader["ProcessedBy"] as string;
            log.DeviceModel = reader["DeviceModel"] as string;

            // Figure out the session ID
            log.SessionId = GetSessionId(reader);

            return log;
        }

        private DigitalSendServerJobLogger CreateLogFromDevice(IDataRecord reader)
        {
            DigitalSendServerJobLogger log = new DigitalSendServerJobLogger((Guid)reader["Id"]);
            log.JobType = reader["JobType"] as string;
            log.FileName = GetFileName(reader);
            log.FileSizeBytes = reader["DeliveredFileSize"] as int?;
            ParseJobXml(ref log, reader["XmlJobInfo"] as string);
            log.CompletionDateTime = ApplyLocalOffset((DateTime)reader["CompletionTime"], log.DssVersion);

            // Figure out the session ID
            log.SessionId = GetSessionId(reader);

            return log;
        }

        private DigitalSendServerJobLogger CreateLogForNotFound(Guid jobId, string logText)
        {
            DigitalSendServerJobLogger log = new DigitalSendServerJobLogger(jobId);

            log.CompletionDateTime = DateTime.Now;
            log.JobType = logText;
            log.CompletionStatus = logText;
            log.FileName = logText;
            log.FileType = logText;
            log.DssVersion = logText;
            log.ProcessedBy = logText;
            log.DeviceModel = logText;

            // Use the last session ID we logged
            log.SessionId = _sessionId;

            return log;
        }

        private void ParseJobXml(ref DigitalSendServerJobLogger log, string xmlJobInfo)
        {
            if (!string.IsNullOrEmpty(xmlJobInfo))
            {
                ParseJobXml(ref log, XmlUtil.CreateXDocument(xmlJobInfo));
            }
        }

        /// <summary>
        /// The XML data received from the device table seems to be inconsistent at best.
        /// For the purpose of populating the server log, we'll populate what we can,
        /// ignoring all errors that may be thrown while attempting to retrieve the data,
        /// marking them as "unavailable" as a flag that an attempt was made to retrieve the data.
        /// </summary>
        /// <param name="log"></param>
        /// <param name="xmlJobInfo"></param>
        private void ParseJobXml(ref DigitalSendServerJobLogger log, XDocument xmlJobInfo)
        {
            string unavailable = "<unavailable>";
            StringBuilder message = null;

            XElement headerElement = xmlJobInfo.Descendants("HeaderElements").FirstOrDefault();
            if (headerElement != null)
            {
                try
                {
                    log.DeviceModel = headerElement.Element("cDeviceDescription").Value;
                }
                catch
                {
                    log.DeviceModel = unavailable;
                }
                try
                {
                    log.CompletionStatus = headerElement.Element("cJobStatusAppHeading").Value;
                }
                catch
                {
                    log.CompletionStatus = unavailable;
                }
            }
            else
            {
                message = new StringBuilder("HeaderElements ");
            }

            XElement detailsElement = xmlJobInfo.Descendants("JobLogDetails").FirstOrDefault();
            if (detailsElement != null)
            {
                try
                {
                    log.FileType = detailsElement.Element("cFileType").Value;
                }
                catch
                {
                    log.FileType = unavailable;
                }
                try
                {
                    log.ScannedPages = short.Parse(detailsElement.Element("cPagesScanned").Value);
                }
                catch { } //Just move on
                try
                {
                    log.DssVersion = detailsElement.Element("cDssSoftwareVersionColon").Value;
                }
                catch
                {
                    log.DssVersion = unavailable;
                }
                try
                {
                    log.ProcessedBy = detailsElement.Element("cJobProcessedBy").Value;
                }
                catch
                {
                    log.ProcessedBy = unavailable;
                }
            }
            else
            {
                if (message == null)
                {
                    message = new StringBuilder();
                }
                message.Append("JobLogDetails ");
            }

            //Check to see if any data was not logged because of null elements
            if (message != null)
            {
                message.Append("elements not found.");
                message.Append(xmlJobInfo.ToString());
                TraceFactory.Logger.Debug(message.ToString());
            }
        }

        private string GetFileName(IDataRecord record)
        {
            string fileName = record["DeliveredFileName"] as string;

            // In some cases, we may need to use the email subject instead of the file name
            if (!string.IsNullOrEmpty(fileName) && !ScanFilePrefix.MatchesPattern(fileName))
            {
                string emailSubject = record["EmailSubject"] as string;
                if (!string.IsNullOrEmpty(emailSubject))
                {
                    return emailSubject + Path.GetExtension(fileName);
                }
            }

            return fileName;
        }

        private string GetSessionId(IDataRecord record)
        {
            // See if we can use the name of the delivered file
            object result = record["DeliveredFileName"];
            if (result != null)
            {
                string fileName = result.ToString();
                if (ScanFilePrefix.MatchesPattern(fileName))
                {
                    ScanFilePrefix prefix = ScanFilePrefix.Parse(fileName);
                    return prefix.SessionId;
                }
            }

            // See if we can use the fax number
            result = record["FaxNumber"];
            if (result != null)
            {
                try
                {
                    ScanFilePrefix prefix = ScanFilePrefix.ParseFromFax(result.ToString(), "u");
                    return prefix.SessionId;
                }
                catch (FormatException)
                {
                    // Wrong format - do nothing
                }
            }

            // Use the last session we saw
            return _sessionId;
        }

        /// <summary>
        /// Up until version 5.02.01.553, the dates in the DSS database were entered as GMT, which required a conversion to local time.
        /// After 5.02.01.553, the dates go into the DSS database as local time.
        /// This method examines the DSS version and applies the conversion to local time, when necessary.
        /// </summary>
        /// <returns></returns>
        private DateTime ApplyLocalOffset(DateTime date, string dssVersion)
        {
            try
            {
                Version version = new Version(dssVersion);

                if (version < _dateOffsetVersion)
                {
                    return date.ToLocalTime();
                }
            }
            catch (Exception)
            {
                //Swallow the exception and just return the original date.
            }

            return date;
        }

        /// <summary>
        /// Checks to make sure we have a valid SessionId before attempting to log.
        /// </summary>
        /// <param name="log">The DigitalSendServerJobLogger.</param>
        private void SubmitLog(DigitalSendServerJobLogger log)
        {
            if (log.SessionId.Length != 8)
            {
                TraceFactory.Logger.Error($"Invalid Session Id: {log.SessionId}.  FileName: {log.FileName}.");
                return;
            }

            _dataLogger.SubmitAsync(log);
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            if (_refreshTimer != null)
            {
                StopMonitoring();
                _refreshTimer.Dispose();
                _refreshTimer = null;
            }
        }

        #endregion
    }
}
