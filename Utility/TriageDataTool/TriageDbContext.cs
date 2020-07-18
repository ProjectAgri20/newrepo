using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.SqlClient;
using System.Linq;

namespace HP.STF.TriageDataTool
{
    /// <summary>
    /// This class is used to make the connection to the requested SQL server
    /// and database. It also performs all the queries of the database to
    /// return the triage information.
    /// </summary>
    internal class TriageDbContext : DbContext
    {
        /// <summary>
        /// This constructor builds and stores the connection string to the
        /// requested server & database.
        /// </summary>
        /// <param name="dbServer">The fully qualified name or IP address of
        /// the SQL Server.</param>
        /// <param name="dbName">The name of the database that contains the
        /// triage data.</param>
        public TriageDbContext(string dbServer, string dbName)
        {
            SqlConnectionStringBuilder sqlConnectionStringBuilder = new SqlConnectionStringBuilder
            {
                DataSource = dbServer,
                InitialCatalog = dbName,
                PersistSecurityInfo = true,
                Password = "report_viewer",
                UserID = "report_viewer"
            };

            Database.Connection.ConnectionString = sqlConnectionStringBuilder.ConnectionString;
        }

        /// <summary>
        /// Performs the query to get the control panel image.
        /// </summary>
        /// <param name="triageDataId">The ID of the row in the table that
        /// holds the image of interest.</param>
        /// <returns>A byte array containing the image data.</returns>
        internal byte[] GetControlPanelImage(string triageDataId)
        {
            string query = "select ControlPanelImage " +
                           "from TriageData with(nolock) " +
                           "where TriageDataId = '" + triageDataId + "'";

            return Database.SqlQuery<byte[]>(query).FirstOrDefault();
        }

        /// <summary>
        /// Performs the query to get the list of devices that have records in
        /// the TriageData table.
        /// </summary>
        /// <param name="sessionId">The Session ID of interest.</param>
        /// <returns>A list of device id strings.</returns>
        internal List<string> GetDeviceIdsBySession(string sessionId)
        {
            string query = "select distinct aeau.AssetId " +
                           "from ActivityExecutionAssetUsage aeau with(nolock) " +
                           "join TriageData td with(nolock) on td.ActivityExecutionId = aeau.ActivityExecutionId " +
                           "where td.SessionId = '" + sessionId + "' " +
                           "order by aeau.AssetId";

            return Database.SqlQuery<string>(query).ToList();
        }

        /// <summary>
        /// Performs the query to get information about a device that has a
        /// record in the TriageData table.
        /// </summary>
        /// <param name="sessionId">The Session ID of interest.</param>
        /// <param name="deviceId">The ID string that specifies the device of
        /// interest.</param>
        /// <returns>A <see cref="TriageDeviceInfo"/> object that contains the
        /// information about the device of interest.</returns>
        internal TriageDeviceInfo GetDeviceInfoBySessionAndDeviceIds(string sessionId, string deviceId)
        {
            string query = "select distinct sd.DeviceName, sd.ProductName, sd.ModelNumber, sd.FirmwareRevision, sd.FirmwareDatecode, sd.IpAddress " +
                           "from TriageData td with(nolock) " +
                           "join ActivityExecutionAssetUsage aeau with(nolock) on td.ActivityExecutionId = aeau.ActivityExecutionId " +
                           "join SessionDevice sd with(nolock) on aeau.AssetId = sd.DeviceId and td.SessionId = sd.SessionId " +
                           "where td.SessionId = '" + sessionId + "' and sd.DeviceId = '" + deviceId + "'";

            return Database.SqlQuery<TriageDeviceInfo>(query).FirstOrDefault();
        }

        /// <summary>
        /// Performs the query to get the performance marker data for the
        /// activity specified.
        /// </summary>
        /// <param name="activityExecutionId">The requested activity ID.</param>
        /// <returns>A <see cref="DataTable"/> containing the date/time, index,
        /// and label for each performance marker recorded for the given
        /// activity.</returns>
        internal DataTable GetPerformanceEventsByActivityExecutionId(string activityExecutionId)
        {
            string query = "select EventDateTime as [Event Date/Time], EventIndex as [Event Index], EventLabel as [Event Label] " +
                           "from ActivityExecutionPerformance with(nolock) " +
                           "where ActivityExecutionId = '" + activityExecutionId + "' " +
                           "order by [Event Index]";

            return FillDataTable("PerformanceTable", query);
        }

        /// <summary>
        /// Performs the query to get session information for all sessions that
        /// recorded triage data between the start aand end dates.
        /// </summary>
        /// <param name="startDate">The beginning date that bounds the search.</param>
        /// <param name="endDate">The ending date that bounds the search.</param>
        /// <returns>A <see cref="Dictionary{TKey, TValue}"/> that contains the
        /// SessionId and STFVersion string for every session found within the
        /// date limits.</returns>
        internal Dictionary<string, string> GetSessionInfoByDateRange(DateTime startDate, DateTime endDate)
        {
            string query = "select distinct td.SessionId, ss.STFVersion " +
                           "from TriageData td with(nolock) " +
                           "join SessionSummary ss with(nolock) on td.SessionId = ss.SessionId " +
                           "where td.TriageDateTime between '" + startDate.ToString("d") + "' and '" +endDate.AddDays(1).ToString("d") + "' " +
                           "order by td.SessionId";

            return Database.SqlQuery<SessionInfo>(query).ToDictionary(v => v.SessionId, v => v.STFVersion);
        }

        /// <summary>
        /// Performs the query that get the triage data (except the control
        /// panel image) for the specified session and device ID's.
        /// </summary>
        /// <param name="sessionId">The Session ID of interest.</param>
        /// <param name="deviceId">The ID string that specifies the device of
        /// interest.</param>
        /// <returns>A <see cref="DataTable"/> containg the information
        /// requested.</returns>
        internal DataTable GetTriageEventsBySessionAndDeviceIds(string sessionId, string deviceId)
        {
            string query = "select td.TriageDataId, dbo.fn_CalcLocalDateTime(td.TriageDateTime) as [Triage Date/Time], ae.ActivityType as [Activity Type], ae.ActivityName as [Activity Name], td.Reason as [Exception Message], td.DeviceWarnings as [Device Warnings], td.Thumbnail, td.UIDumpData as [UI Dump Data], td.ActivityExecutionId, td.ControlIds " +
                           "from TriageData td with(nolock) " +
                           "join ActivityExecutionAssetUsage aeau with(nolock) on td.ActivityExecutionId = aeau.ActivityExecutionId " +
                           "join ActivityExecution ae with(nolock) on td.ActivityExecutionId = ae.ActivityExecutionId " +
                           "where td.SessionId = '" + sessionId + "' and aeau.AssetId = '" + deviceId + "' " +
                           "order by td.TriageDateTime";

            return FillDataTable("EventsTable", query);
        }

        /// <summary>
        /// This method fills a <see cref="DataTable"/> with the records
        /// returned by the specified SQL query.
        /// </summary>
        /// <param name="tableName">The name of the table to be returned.</param>
        /// <param name="sqlQuery">The SQL query that is performed to collect
        /// the records from the database.</param>
        /// <returns>A <see cref="DataTable"/> containg the records returned by
        /// the query.</returns>
        private DataTable FillDataTable(string tableName, string sqlQuery)
        {
            DataTable resultTable = new DataTable(tableName);

            using (SqlConnection dbConn = new SqlConnection(Database.Connection.ConnectionString))
            {
                using (SqlCommand cmd = new SqlCommand(sqlQuery, dbConn))
                {
                    using (SqlDataAdapter da = new SqlDataAdapter(cmd))
                    {
                        da.Fill(resultTable);
                    }
                }
            }

            return resultTable;
        }
    }
}
