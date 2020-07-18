using System;
using System.Data.SqlClient;
using System.Linq;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.DataLog.Service
{
    /// <summary>
    /// Writes data from <see cref="LogTableDefinition" /> and <see cref="LogTableRecord" /> objects to a database.
    /// </summary>
    public sealed class DataLogDatabaseWriter
    {
        private const string _sessionSummaryTable = "SessionSummary";
        private const string _sessionIdColumn = "SessionId";

        private readonly DataLogConnectionString _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogDatabaseWriter" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="DataLogConnectionString" /> used to connect to the database.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public DataLogDatabaseWriter(DataLogConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Inserts data from the specified <see cref="LogTableRecord" /> into the table designated by the <see cref="LogTableDefinition" />.
        /// </summary>
        /// <param name="table">The <see cref="LogTableDefinition" />.</param>
        /// <param name="record">The <see cref="LogTableRecord" />.</param>
        /// <returns>A <see cref="DataLogDatabaseResult" /> representing the result of this operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="table" /> is null.
        /// <para>or</para>
        /// <paramref name="record" /> is null.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public DataLogDatabaseResult Insert(LogTableDefinition table, LogTableRecord record)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            LogTrace($"INSERT {table.Name} (primary key = {record[table.PrimaryKey]})");

            DataLogDatabaseResult result;
            using (SqlCommand insert = DataLogSqlBuilder.BuildInsert(table, record))
            {
                result = new DataLogDatabaseResult(table.Name, insert);
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString.ToString()))
                    {
                        connection.Open();
                        insert.Connection = connection;

                        try
                        {
                            insert.ExecuteNonQuery();
                        }
                        catch (SqlException ex) when (ex.Message.Contains("Invalid object"))
                        {
                            // Table doesn't exist - create it and try again
                            LogDebug($"Generating new table {table.Name}");
                            if (CreateTable(connection, table))
                            {
                                LogDebug($"Table {table.Name} created.");
                                LogTrace($"Retrying INSERT {table.Name}");
                                insert.ExecuteNonQuery();
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Error = ex.Message;
                }
            }

            if (!result.Success)
            {
                LogWarn($"INSERT {table.Name} failed: {result.Error}");
                LogTrace($"SQL command: " + result.Command);
                LogTrace($"SQL parameters: {string.Join("; ", result.Parameters.Select(n => $"{n.Key}={n.Value}"))}");
            }

            return result;
        }

        /// <summary>
        /// Updates data from the specified <see cref="LogTableRecord" /> into the table designated by the <see cref="LogTableDefinition" />.
        /// </summary>
        /// <param name="table">The <see cref="LogTableDefinition" />.</param>
        /// <param name="record">The <see cref="LogTableRecord" />.</param>
        /// <returns>A <see cref="DataLogDatabaseResult" /> representing the result of this operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="table" /> is null.
        /// <para>or</para>
        /// <paramref name="record" /> is null.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        public DataLogDatabaseResult Update(LogTableDefinition table, LogTableRecord record)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            LogTrace($"UPDATE {table.Name} (primary key = {record[table.PrimaryKey]})");

            DataLogDatabaseResult result;
            using (SqlCommand update = DataLogSqlBuilder.BuildUpdate(table, record))
            {
                result = new DataLogDatabaseResult(table.Name, update);
                try
                {
                    using (SqlConnection connection = new SqlConnection(_connectionString.ToString()))
                    {
                        connection.Open();
                        update.Connection = connection;

                        int affectedRows = update.ExecuteNonQuery();
                        if (affectedRows == 0)
                        {
                            result.Error = "No rows were affected.";
                        }
                    }
                }
                catch (Exception ex)
                {
                    result.Error = ex.Message;
                }
            }

            if (!result.Success)
            {
                LogWarn($"UPDATE {table.Name} failed: {result.Error}");
                LogTrace($"SQL command: " + result.Command);
                LogTrace($"SQL parameters: {string.Join("; ", result.Parameters.Select(n => $"{n.Key}={n.Value}"))}");
            }

            return result;
        }

        private static bool CreateTable(SqlConnection connection, LogTableDefinition table)
        {
            try
            {
                using (SqlCommand create = DataLogSqlBuilder.BuildCreateTable(table))
                {
                    LogTrace($"SQL command: {create.CommandText}");
                    create.Connection = connection;
                    create.ExecuteNonQuery();
                }

                if (table.Any(n => n.Name == _sessionIdColumn))
                {
                    using (SqlCommand key = DataLogSqlBuilder.BuildForeignKey(_sessionSummaryTable, _sessionIdColumn, table.Name, _sessionIdColumn))
                    {
                        LogTrace($"Building foreign key: {key.CommandText}");
                        key.Connection = connection;
                        key.ExecuteNonQuery();
                    }
                }

                return true;
            }
            catch (SqlException ex)
            {
                LogError($"Table {table.Name} could not be created: {ex.Message}");
                return false;
            }
        }
    }
}
