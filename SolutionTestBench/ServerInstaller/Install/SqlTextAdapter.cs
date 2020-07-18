using System;
using System.Data;
using System.Data.Common;
using System.Data.Objects;
using System.Data.SqlClient;

namespace HP.SolutionTest.Install
{
    internal class SqlTextAdapter : IDisposable
    {
        private SqlConnection _connection;
        private SqlDataReader _reader;
        private SqlTransaction _transaction;

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlTextAdapter"/> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlTextAdapter(string connectionString)
        {
            _connection = new SqlConnection(connectionString);
        }

        public string ServerVersion
        {
            get 
            {
                var reader = ExecuteReader("SELECT @@VERSION");
                if (reader != null && reader.Read())
                {
                    var res = reader[0];
                    return res.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SqlTextAdapter"/> class.
        /// </summary>
        /// <param name="serverName">Name of the server.</param>
        /// <param name="databaseName">Name of the database.</param>
        /// <param name="userId">The user id.</param>
        /// <param name="password">The password.</param>
        public SqlTextAdapter(string serverName, string databaseName, string userId, string password)
        {
            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                InitialCatalog = databaseName,
                PersistSecurityInfo = true,
                UserID = userId,
                Password = password,
                MultipleActiveResultSets = true
            };

            _connection = new SqlConnection(builder.ConnectionString);
        }

        public SqlTextAdapter(string serverName, string databaseName)
        {
            var builder = new SqlConnectionStringBuilder()
            {
                DataSource = serverName,
                InitialCatalog = databaseName,
                PersistSecurityInfo = true,
                IntegratedSecurity = true,
                MultipleActiveResultSets = true
            };

            _connection = new SqlConnection(builder.ConnectionString);
        }

        /// <summary>
        /// Gets or sets the connection.
        /// </summary>
        /// <value>
        /// The <see cref="SqlConnection" />.
        /// </value>
        public SqlConnection Connection
        {
            get { return _connection; }
            set { _connection = value; }
        }

        /// <summary>
        /// Gets or sets the reader.
        /// </summary>
        /// <value>The <see cref="SqlDataReader"/>.</value>
        public SqlDataReader Reader
        {
            get { return _reader; }
            set { _reader = value; }
        }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        private void OpenConnection()
        {
            if (_connection.State == ConnectionState.Closed)
            {
                _connection.Open();
            }
        }

        /// <summary>
        /// Begins a transaction for this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">The connection is null</exception>
        public void BeginTransaction()
        {
            if (_connection == null)
            {
                throw new InvalidOperationException("The connection is null");
            }

            OpenConnection();
            _transaction = _connection.BeginTransaction();
        }

        /// <summary>
        /// Performs a rollback on the transaction for this instance.
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// There is no transaction established
        /// or
        /// Transaction rollback failed
        /// </exception>
        public void RollbackTransaction()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("There is no transaction established");
            }

            try
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Transaction rollback failed", ex);
            }
        }

        /// <summary>
        /// Commits the transaction for this instance
        /// </summary>
        /// <exception cref="System.InvalidOperationException">
        /// There is no transaction established
        /// or
        /// Transaction commit failed
        /// </exception>
        public void CommitTransaction()
        {
            if (_transaction == null)
            {
                throw new InvalidOperationException("There is no transaction established");
            }

            try
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = null;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Transaction commit failed", ex);
            }
        }

        /// <summary>
        /// Executes the defined SQL string.
        /// </summary>
        /// <param name="sql">The SQL <see cref="String"/>.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.  Defaults to unlimited.</param>
        public void ExecuteSql(string sql, int timeoutInSeconds = 0)
        {
            OpenConnection();
            using (SqlCommand command = new SqlCommand(sql, _connection))
            {
                if (_transaction != null)
                {
                    command.Transaction = _transaction;
                }

                command.CommandTimeout = timeoutInSeconds;
                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes a parameterized SQL statement using a syntax similar to String.Format().
        /// </summary>
        /// <param name="sql">The SQL, with {0} style format tags for each parameter.</param>
        /// <param name="parameters">Parameters in the '@paramName' format that are substituted into the SQLText.</param>
        /// <example>
        /// <para>
        /// This example shows the use of parameters in an ad-hoc SQL statement.  Each parameter in
        /// the SQL is replaced with the corresponding value passed into the method.
        /// <code>
        /// string command = "INSERT INTO SessionSummary (SessionId, StartDate, SessionName, Owner) VALUES ({0}, {1}, {2}, {3})";
        /// 
        /// using (SqlAdapter adapter = new SqlAdapter(DataLogSqlConnection.ConnectionString))
        /// {
        ///     adapter.ExecuteSql(command, sessionId, startDate.ToString("yyyy-MM-dd HH:mm:ss"), sessionName, owner);
        /// }
        /// </code>
        /// </para>
        /// </example>
        public void ExecuteSql(string sql, params string[] parameters)
        {
            if (parameters == null)
            {
                throw new ArgumentNullException("parameters");
            }

            if (parameters.Length == 0)
            {
                throw new ArgumentException("At least one parameter must be specified.");
            }

            OpenConnection();
            using (SqlCommand command = new SqlCommand(sql, _connection))
            {
                for (int i = 0; i < parameters.Length; i++)
                {
                    string formatCode = "{" + i + "}";
                    string paramCode = "@param" + i;
                    command.CommandText = command.CommandText.Replace(formatCode, paramCode);
                    command.Parameters.Add(paramCode, SqlDbType.VarChar).Value = parameters[i];
                }

                command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes the reader.
        /// </summary>
        /// <param name="sql">The <see cref="String"/> SQL statement.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <returns></returns>
        public SqlDataReader ExecuteReader(string sql, int timeoutInSeconds = 30)
        {
            OpenConnection();
            using (SqlCommand command = new SqlCommand(sql, _connection))
            {
                command.CommandTimeout = timeoutInSeconds;
                command.CommandType = CommandType.Text;
                _reader = command.ExecuteReader();
            }

            return _reader;
        }

        /// <summary>
        /// Executes the the ad-hoc statement and returns a value.
        /// </summary>
        /// <param name="sql">The <see cref="String"/> SQL statement.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <returns></returns>
        public int ExecuteScalar(string sql, int timeoutInSeconds = 30)
        {
            int result = 0;

            OpenConnection();
            using (SqlCommand cmd = new SqlCommand(sql, _connection))
            {
                cmd.CommandTimeout = timeoutInSeconds;
                result = (int)cmd.ExecuteScalar();
            }
            return result;
        }

        /// <summary>
        /// Executes the ad-hoc statement, returning the number of rows affected.
        /// </summary>
        /// <param name="sql">The <see cref="String"/> SQL statement.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        public int ExecuteNonQuery(string sql, int timeoutInSeconds = 30)
        {
            OpenConnection();
            using (SqlCommand command = new SqlCommand(sql, _connection))
            {
                command.CommandTimeout = timeoutInSeconds;
                command.CommandType = CommandType.Text;
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes the ad-hoc SQL returning a <see cref="DbDataReader"/> to iterate over results.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="sql">The SQL.</param>
        /// <param name="timeoutInSeconds">The timeout in seconds.</param>
        /// <returns></returns>
        public static DbDataReader ExecuteReader(ObjectContext entities, string sql, int timeoutInSeconds = 30)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            DbConnection conn = entities.Connection;
            ConnectionState initialState = conn.State;
            DbDataReader reader = null;

            if (initialState != ConnectionState.Open)
            {
                conn.Open();
            }

            using (DbCommand cmd = conn.CreateCommand())
            {
                cmd.CommandTimeout = timeoutInSeconds;
                cmd.CommandText = sql;
                reader = cmd.ExecuteReader();
            }

            return reader;
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (_reader != null && !_reader.IsClosed)
                {
                    _reader.Close();
                    _reader = null;
                }

                if (_transaction != null)
                {
                    _transaction.Dispose();
                }
                if (_connection != null)
                {
                    _connection.Close();
                    _connection.Dispose();
                }
            }
        }

        #endregion
    }
}
