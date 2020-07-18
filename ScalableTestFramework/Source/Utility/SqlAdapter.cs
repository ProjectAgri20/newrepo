using System;
using System.Data;
using System.Data.SqlClient;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// Data adapter for executing ad-hoc SQL statements.
    /// </summary>
    public class SqlAdapter : IDisposable
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SqlAdapter" /> class.
        /// </summary>
        /// <param name="connectionString">The connection string.</param>
        public SqlAdapter(string connectionString)
        {
            Connection = new SqlConnection(connectionString);
        }

        /// <summary>
        /// Gets the <see cref="SqlConnection" />.
        /// </summary>
        public SqlConnection Connection { get; private set; }

        /// <summary>
        /// Gets the <see cref="SqlDataReader" />.
        /// </summary>
        public SqlDataReader Reader { get; protected set; }

        /// <summary>
        /// Opens the connection.
        /// </summary>
        private void OpenConnection()
        {
            if (Connection.State == ConnectionState.Closed)
            {
                Connection.Open();
            }
        }

        /// <summary>
        /// Executes the specified SQL statement.
        /// </summary>
        /// <param name="sql">The SQL statement to execute.</param>
        /// <returns>The number of rows affected.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public int ExecuteNonQuery(string sql)
        {
            OpenConnection();
            using (SqlCommand command = new SqlCommand(sql, Connection))
            {
                return command.ExecuteNonQuery();
            }
        }

        /// <summary>
        /// Executes the specified SQL statement that reads rows from the database.
        /// </summary>
        /// <param name="sql">The SQL statement to execute.</param>
        /// <returns>A <see cref="SqlDataReader" /> used to read the results.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Security", "CA2100:Review SQL queries for security vulnerabilities")]
        public SqlDataReader ExecuteReader(string sql)
        {
            OpenConnection();
            using (SqlCommand command = new SqlCommand(sql, Connection))
            {
                Reader = command.ExecuteReader();
            }

            return Reader;
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
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (Reader != null && !Reader.IsClosed)
                {
                    Reader.Close();
                    Reader = null;
                }

                if (Connection != null)
                {
                    Connection.Close();
                    Connection.Dispose();
                }
            }
        }

        #endregion
    }
}
