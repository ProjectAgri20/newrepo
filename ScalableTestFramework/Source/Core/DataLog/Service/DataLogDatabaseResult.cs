using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;

namespace HP.ScalableTest.Core.DataLog.Service
{
    /// <summary>
    /// The result of a <see cref="DataLogDatabaseWriter" /> operation.
    /// </summary>
    public sealed class DataLogDatabaseResult
    {
        /// <summary>
        /// Gets a value indicating whether the database operation succeeded.
        /// </summary>
        public bool Success => string.IsNullOrEmpty(Error);

        /// <summary>
        /// Gets the database table this operation affects.
        /// </summary>
        public string Table { get; }

        /// <summary>
        /// Gets the SQL command that was executed.
        /// </summary>
        public string Command { get; }

        /// <summary>
        /// Gets the SQL parameters that were defined for the command.
        /// </summary>
        public Dictionary<string, object> Parameters { get; }

        /// <summary>
        /// Gets or sets the number of times this operation has been retried.
        /// If set to -1, this operation is a special case and will never be retried.
        /// </summary>
        public int Retries { get; set; }

        /// <summary>
        /// The error from the database operation.
        /// </summary>
        public string Error { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogDatabaseResult" /> class.
        /// </summary>
        /// <param name="table">The table to which this operation applies.</param>
        /// <param name="command">The <see cref="SqlCommand" /> that was executed.</param>
        /// <exception cref="ArgumentNullException"><paramref name="command" /> is null.</exception>
        internal DataLogDatabaseResult(string table, SqlCommand command)
        {
            if (command == null)
            {
                throw new ArgumentNullException(nameof(command));
            }

            Table = table;
            Command = command.CommandText;
            Parameters = command.Parameters.Cast<SqlParameter>().ToDictionary(n => n.ParameterName, n => n.Value);
        }
    }
}
