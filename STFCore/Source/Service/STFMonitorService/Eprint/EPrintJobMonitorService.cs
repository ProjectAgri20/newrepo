using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using System.Data.SqlClient;
using System.Data.Common;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Framework.Monitor;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Service.Monitor.Eprint
{
    /// <summary>
    /// Monitors new and updated records in the ePrint database and logs the data to the STF DataLog database.
    /// </summary>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    internal class EPrintJobMonitorService :  StfMonitor
    {
        private Dictionary<string, string> _connectionStrings = new Dictionary<string, string>();
        private Dictionary<string, DateTime> _cleanupRequests = new Dictionary<string, DateTime>();

        private Timer _refreshTimer;

        private static readonly List<Type> _retryExceptions = new List<Type>() { typeof(SqlException) };


        /// <summary>
        /// Initializes a new instance of the <see cref="EPrintJobMonitorService" /> class.
        /// </summary>
        /// <param name="monitorConfig">The monitor configuration.</param>
        public EPrintJobMonitorService(MonitorConfig monitorConfig) : base(monitorConfig)
        {
            TraceFactory.Logger.Debug("Constructor");

            RefreshConfig(monitorConfig);

            TraceFactory.Logger.Debug("Constructor. after refresh");

            StringBuilder fqHostName = new StringBuilder(Configuration.MonitorLocation);
            fqHostName.Append("\\");
            fqHostName.Append(Configuration.DatabaseInstanceName);
            if (Configuration.ConnectionPort > 0)
            {
                fqHostName.Append(",");
                fqHostName.Append(Configuration.ConnectionPort.ToString());
            }
            _refreshTimer = new Timer(RefreshTimerCallback);
            TraceFactory.Logger.Debug("Constructor. Checking DBs");
            CheckMonitorDatabase(fqHostName.ToString());
            CheckCloudPrintDatabase(fqHostName.ToString());

            // trigger immediate run
            RefreshTimerCallback(null);
        }

        /// <summary>
        /// Gets the monitor location.
        /// </summary>
        /// <value>
        /// The monitor location.
        /// </value>
        public override string MonitorLocation
        {
            get { return Configuration.MonitorLocation; }
        }

        /// <summary>
        /// The configuration needed to start an output monitor.
        /// </summary>
        protected DatabaseMonitorConfig Configuration { get; private set; }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public override void StartMonitoring()
        {
            _refreshTimer.Change(TimeSpan.FromMinutes(1), TimeSpan.FromMinutes(1));
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public override void StopMonitoring()
        {
            _refreshTimer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        /// <summary>
        /// Refreshes monitor configuration for this instance.
        /// </summary>
        /// <param name="monitorConfig"></param>
        public override void RefreshConfig(MonitorConfig monitorConfig)
        {
            Configuration = LegacySerializer.DeserializeXml<DatabaseMonitorConfig>(monitorConfig.Configuration);
        }

        private void RefreshTimerCallback(object notUsed)
        {
            StopMonitoring();
            ProcessRecords();
            ClearNotifiedSessions();
            StartMonitoring();
        }
        private void ClearSession_Work(string sessionId)
        {
            List<PendingJob> jobsToProcess = null;
            List<int> jobIdsToClear = new List<int>();

            try
            {
                TraceFactory.Logger.Debug("DWA - in Clear Session Work");
                jobsToProcess = GetUnProcessedJobs();
            }
            catch (SqlException sqlEx)
            {
                TraceFactory.Logger.Error("Error retrieving list of Job IDs.  Aborting.", sqlEx);
                return;
            }

            try
            {
                using (SqlAdapter ePrintAdapter = new SqlAdapter(_connectionStrings[Resource.ePrintDatabase]))
                {
                    using (DbDataReader reader = GetDataReader(ePrintAdapter, Resource.JobDataSql.FormatWith(string.Join(",", jobsToProcess))))
                    {
                        while (reader.Read())
                        {
                            string sessionText = (string)reader["Subtitle"];
                            if (sessionText.Contains(sessionId))
                            {
                                jobIdsToClear.Add((int)reader["ID"]);
                            }
                        }
                    }
                }

                ClearJobIds(jobIdsToClear);
            }
            catch (SqlException sqlEx)
            {
                TraceFactory.Logger.Error(sqlEx);
            }
        }

        /// <summary>
        /// Attempts a connection to the ePrintMonitor database.
        /// If the connection fails, attempts to create the database, then retries the connection.
        /// </summary>
        /// <param name="hostName"></param>
        private void CheckMonitorDatabase(string hostName)
        {
            bool connected = false;
            SqlConnectionStringBuilder connectionBuilder = GetConnectionStringBuilder(hostName, Resource.MonitorDatabase);

            TraceFactory.Logger.Debug("Testing connection to " + connectionBuilder.DataPath());
            try
            {
                connected = ConnectToDatabase(connectionBuilder);
                if (! connected)
                {
                    TraceFactory.Logger.Debug("Unable to connect to " + connectionBuilder.DataPath());
                    TraceFactory.Logger.Debug("Running Creation script.");

                    connectionBuilder.InitialCatalog = "master";
                    ExecuteSql(connectionBuilder.ToString(), Resource.CreateMonitorDatabase);
                    TraceFactory.Logger.Debug("Database creation successful.");
                        
                    //Connect to the newly created database.  It make take a few seconds for it to establish a connection.
                    connectionBuilder.InitialCatalog = Resource.MonitorDatabase;
                    for (int i = 0; i < 10; i++)
                    {
                        TraceFactory.Logger.Debug("Attempting connection...  {0}".FormatWith(i));
                        connected = ConnectToDatabase(connectionBuilder);
                        if (connected)
                        {
                            break;
                        }

                        Thread.Sleep(5000);
                    }
                }
            }
            catch (SqlException sqlEx)
            {
                TraceFactory.Logger.Error(sqlEx);
            }

            if (connected)
            {
                return;
            }

            // All connection attempts failed
            throw new InvalidOperationException("Could not connect to {0} database.".FormatWith(Resource.MonitorDatabase));
        }

        /// <summary>
        /// Attempts a connection to the ePrint database.
        /// </summary>
        /// <param name="hostName"></param>
        private void CheckCloudPrintDatabase(string hostName)
        {
            SqlConnectionStringBuilder connectionBuilder = GetConnectionStringBuilder(hostName, Resource.ePrintDatabase);

            TraceFactory.Logger.Debug("Testing connection to " + connectionBuilder.DataPath());
            try
            {
                ExecuteSql(connectionBuilder.ToString(), Resource.CloudPrintTrigger);
                TraceFactory.Logger.Debug("Connected to " + connectionBuilder.DataPath());
                CacheConnectionString(connectionBuilder);
                return;
            }
            catch (SqlException sqlEx)
            {
                TraceFactory.Logger.Error(sqlEx);
            }
            
            // connection attempt failed
            throw new InvalidOperationException("Could not connect to {0} database.".FormatWith(Resource.ePrintDatabase));
        }

        /// <summary>
        /// 
        /// </summary>
        private void ClearNotifiedSessions()
        {            
            DateTime now = DateTime.Now;
            foreach (string sessionId in _cleanupRequests.Keys)
            {
                if (_cleanupRequests[sessionId] >= now)
                {
                    Task.Factory.StartNew(() => ClearSession_Work(sessionId));
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of <see cref="SqlConnectionStringBuilder"/> based on the specified parameters.
        /// </summary>
        /// <param name="hostName"></param>
        /// <param name="databaseName"></param>
        /// <returns></returns>
        private SqlConnectionStringBuilder GetConnectionStringBuilder(string hostName, string databaseName)
        {
            SqlConnectionStringBuilder result = new SqlConnectionStringBuilder()
            {
                DataSource = hostName.Trim(),
                InitialCatalog = databaseName,
                IntegratedSecurity = true,
                MultipleActiveResultSets = true
            };

            return result;
        }

        /// <summary>
        /// Attempts a connection to a database using the provided <see cref="SqlConnectionStringBuilder"/>.
        /// If connection is successful, caches the connection string for later use.
        /// </summary>
        /// <param name="connectionBuilder"></param>
        /// <returns></returns>
        private bool ConnectToDatabase(SqlConnectionStringBuilder connectionBuilder)
        {
            bool result = false;

            using (SqlAdapter adapter = new SqlAdapter(connectionBuilder.ToString()))
            {
                if (ConnectToDatabase(adapter))
                {
                    TraceFactory.Logger.Debug("Connected to " + connectionBuilder.DataPath());
                    CacheConnectionString(connectionBuilder);

                    result = true;
                }
            }

            return result;
        }
        
        /// <summary>
        /// Attempts a connection to a database using the provided <see cref="SqlAdapter"/>.
        /// </summary>
        /// <param name="adapter"></param>
        /// <returns>true if the connection attempt was successful.</returns>
        private bool ConnectToDatabase(SqlAdapter adapter)
        {
            try
            {
                adapter.Connection.Open();
                return true;
            }
            catch (SqlException sqlEx)
            {
                TraceFactory.Logger.Error(sqlEx);
            }

            return false;
        }

        /// <summary>
        /// Executes the specified SQL text on the database specified in the connection string.
        /// </summary>
        /// <param name="connectionString"></param>
        /// <param name="sqlText"></param>
        private void ExecuteSql(string connectionString, string sqlText)
        {
            string[] delimiter = new string[] { "GO{0}".FormatWith(Environment.NewLine) };
            string[] sqlCommands = sqlText.Split(delimiter, StringSplitOptions.RemoveEmptyEntries);
            
            using (SqlAdapter adapter = new SqlAdapter(connectionString))
            {
                string command = string.Empty;
                foreach (string line in sqlCommands)
                {
                    command = line.Trim();
                    if (! string.IsNullOrEmpty(command))
                    {
                        adapter.ExecuteNonQuery(command);
                    }
                }
            }
        }

        /// <summary>
        /// Caches the connection string from the specified <see cref="SqlConnectionStringBuilder"/>.
        /// </summary>
        /// <param name="connectionBuilder"></param>
        private void CacheConnectionString(SqlConnectionStringBuilder connectionBuilder)
        {
            if (!_connectionStrings.ContainsKey(connectionBuilder.InitialCatalog))
            {
                _connectionStrings.Add(connectionBuilder.InitialCatalog, connectionBuilder.ToString());
            }
        }

        /// <summary>
        /// Returns a <see cref="DbDataReader"/> using the specified SQLAdapter and SQL text.
        /// </summary>
        /// <param name="adapter">The SQL adapter</param>
        /// <param name="sqlText">The SQL text to execute</param>
        /// <returns>A <see cref="DbDataReader"/></returns>
        private DbDataReader GetDataReader(SqlAdapter adapter, string sqlText)
        {
            DbDataReader reader = null;
            Retry.WhileThrowing(() =>
            {
                reader = adapter.ExecuteReader(sqlText);
            }, 3, TimeSpan.FromSeconds(5), _retryExceptions);

            return reader;
        }

        /// <summary>
        /// Processes any unprocessed jobs that are present in the EPrintMonitor.Pending table.
        /// Retrieves list of job Ids from EPrintMonitor.Pending.
        /// Retrieves the job data from ePrint database for those Ids.
        /// Creates log records for each job item and saves it to STF DataLog database.
        /// Removes all job Ids from the above table when successful processing has occurred.
        /// </summary>
        private void ProcessRecords()
        {
            List<PendingJob> jobsToProcess = null;
            List<PendingJob> processedJobs = null;

            try
            {
                jobsToProcess = GetUnProcessedJobs();
            }
            catch (SqlException sqlEx)
            {
                TraceFactory.Logger.Error("Error retrieving list of Job IDs.  Aborting.", sqlEx);
                return;
            }

            if (jobsToProcess.Any())
            {
                TraceFactory.Logger.Debug("Jobs to process: {0}".FormatWith(jobsToProcess.Count));
                
                try
                {
                    using (SqlAdapter ePrintAdapter = new SqlAdapter(_connectionStrings[Resource.ePrintDatabase]))
                    {
                        using (DbDataReader jobsReader = GetDataReader(ePrintAdapter, Resource.JobDataSql.FormatWith(string.Join(",", jobsToProcess))))
                        {
                            processedJobs = EPrintDataLogger.LogAll(jobsToProcess, jobsReader);
                        }
                    }
                }
                catch (SqlException sqlEx)
                {
                    TraceFactory.Logger.Error("Error processing ePrint jobs.", sqlEx);
                }

                //Fix the crash if the eprint had failed to print
                if (processedJobs.Any())
                {
                    UpdateJobIds(processedJobs.Where(j => j.IsInsert == true).Select(i => i.JobId).ToList());
                    ClearJobIds(processedJobs.Where(j => j.IsInsert == false).Select(i => i.JobId).ToList());
                }
                else
                {
                    TraceFactory.Logger.Debug("No jobs to process");

                }
            }
        }

        /// <summary>
        /// Gets a list of ePrint job Ids that have not been processed.
        /// </summary>
        /// <returns><see cref="List<int>"/></returns>
        private List<PendingJob> GetUnProcessedJobs()
        {
            List<PendingJob> result = new List<PendingJob>();

            using (SqlAdapter adapter = new SqlAdapter(_connectionStrings[Resource.MonitorDatabase]))
            {
                using (DbDataReader reader = GetDataReader(adapter, Resource.GetUnprocessedJobsSql))
                {
                    while (reader.Read())
                    {
                        result.Add(new PendingJob(reader));
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Clears the specified job Ids from EPrintMonitor.Pending table.
        /// </summary>
        /// <param name="jobIdsToClear">The list of job Ids to clear.</param>
        private void ClearJobIds(List<int> jobIdsToClear)
        {
            if (jobIdsToClear.Count > 0)
            {
                string sql = Resource.DeleteProcessedJobsSql.FormatWith(string.Join(",", jobIdsToClear));
                try
                {
                    TraceFactory.Logger.Debug("Clearing jobs: {0}".FormatWith(jobIdsToClear.Count));
                    ExecuteSql(_connectionStrings[Resource.MonitorDatabase], sql);
                }
                catch (SqlException sqlEx)
                {
                    TraceFactory.Logger.Error(sql, sqlEx);
                }
            }
        }

        /// <summary>
        /// Updates jobs marked as Inserted to Updated.
        /// </summary>
        /// <param name="jobIdsToUpdate"></param>
        private void UpdateJobIds(List<int> jobIdsToUpdate)
        {
            if (jobIdsToUpdate.Count > 0)
            {
                string sql = Resource.UpdateProcessedJobsSql.FormatWith(string.Join(",", jobIdsToUpdate));
                try
                {
                    TraceFactory.Logger.Debug("Updating jobs: {0}".FormatWith(jobIdsToUpdate.Count));
                    ExecuteSql(_connectionStrings[Resource.MonitorDatabase], sql);
                }
                catch (SqlException sqlEx)
                {
                    TraceFactory.Logger.Error(sql, sqlEx);
                }
            }
        }

    }
}
