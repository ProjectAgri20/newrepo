using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.DataLog.Service
{
    /// <summary>
    /// Builds <see cref="SqlCommand" /> scripts from <see cref="LogTableDefinition" /> and <see cref="LogTableRecord" /> objects.
    /// </summary>
    public static class DataLogSqlBuilder
    {
        /// <summary>
        /// Builds a script that will create a new table from the specified <see cref="LogTableDefinition" />.
        /// </summary>
        /// <param name="table">The <see cref="LogTableDefinition" />.</param>
        /// <returns>A <see cref="SqlCommand" /> for a CREATE TABLE script based on the specified <see cref="LogTableDefinition" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="table" /> is null.</exception>
        public static SqlCommand BuildCreateTable(LogTableDefinition table)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            StringBuilder script = new StringBuilder();
            script.AppendLine($"CREATE TABLE [dbo].[{table.Name}](");

            // Build column definitions
            foreach (LogTableColumn column in table)
            {
                string nullConstraint = column.AllowNulls ? "NULL" : "NOT NULL";
                script.AppendLine($"[{column.Name}] {column.DataType} {nullConstraint},");
            }

            // Add primary key constraint
            script.AppendLine($"CONSTRAINT [PK_{table.Name}] PRIMARY KEY CLUSTERED([{table.PrimaryKey}] ASC)");
            script.AppendLine($"WITH(PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, IGNORE_DUP_KEY = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON[PRIMARY]");

            // Close script and return
            script.AppendLine(") ON[PRIMARY]");
            return new SqlCommand(script.ToString());
        }

        /// <summary>
        /// Builds a script that will create a new foreign key between two tables.
        /// </summary>
        /// <param name="primaryTable">The name of the table containing the primary key.</param>
        /// <param name="primaryColumn">The name of the column acting as the primary key.</param>
        /// <param name="foreignTable">The name of the table containing the foreign key.</param>
        /// <param name="foreignColumn">The name of the column acting as the foreign key.</param>
        /// <returns>A <see cref="SqlCommand" /> for an ALTER TABLE script that creates a foreign key between two tables.</returns>
        public static SqlCommand BuildForeignKey(string primaryTable, string primaryColumn, string foreignTable, string foreignColumn)
        {
            StringBuilder script = new StringBuilder();
            script.AppendLine($"ALTER TABLE [dbo].[{foreignTable}] WITH CHECK ADD CONSTRAINT [FK_{foreignTable}_{primaryTable}] FOREIGN KEY([{foreignColumn}])");
            script.AppendLine($"REFERENCES [dbo].[{primaryTable}] ([{primaryColumn}])");
            script.AppendLine($"ON UPDATE CASCADE");
            script.AppendLine($"ON DELETE CASCADE");
            script.AppendLine($"ALTER TABLE [dbo].[{foreignTable}] CHECK CONSTRAINT [FK_{foreignTable}_{primaryTable}]");
            return new SqlCommand(script.ToString());
        }

        /// <summary>
        /// Builds a script that will insert data from the specified <see cref="LogTableRecord" />.
        /// </summary>
        /// <param name="table">The <see cref="LogTableDefinition" /> for the database table.</param>
        /// <param name="record">The <see cref="LogTableRecord" /> containing the data to insert.</param>
        /// <returns>A <see cref="SqlCommand" /> for an INSERT script that inserts the data from the specified <see cref="LogTableRecord" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="table" /> is null.
        /// <para>or</para>
        /// <paramref name="record" /> is null.
        /// </exception>
        public static SqlCommand BuildInsert(LogTableDefinition table, LogTableRecord record)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            SqlParameterHelper helper = new SqlParameterHelper(table, record);

            StringBuilder script = new StringBuilder();
            script.Append($"INSERT INTO [dbo].[{table.Name}](");
            script.Append(string.Join(", ", helper.GetColumnNames()));
            script.Append(") VALUES (");
            script.Append(string.Join(", ", helper.GetColumnParams()));
            script.Append(")");

            // Add parameters for all values
            SqlCommand command = new SqlCommand(script.ToString());
            command.Parameters.AddRange(helper.GetSqlParameters().ToArray());
            return command;
        }

        /// <summary>
        /// Builds a script that will update data from the specified <see cref="LogTableRecord" />.
        /// </summary>
        /// <param name="table">The <see cref="LogTableDefinition" /> for the database table.</param>
        /// <param name="record">The <see cref="LogTableRecord" /> containing the data to update.</param>
        /// <returns>A <see cref="SqlCommand" /> for an UPDATE script that updates the data from the specified <see cref="LogTableRecord" />.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="table" /> is null.
        /// <para>or</para>
        /// <paramref name="record" /> is null.
        /// </exception>   
        public static SqlCommand BuildUpdate(LogTableDefinition table, LogTableRecord record)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            SqlParameterHelper helper = new SqlParameterHelper(table, record);

            StringBuilder script = new StringBuilder();
            script.Append($"UPDATE [dbo].[{table.Name}] SET ");
            script.Append(string.Join(", ", helper.GetUpdateAssignments()));
            script.Append(" WHERE ");
            script.Append(helper.GetPrimaryKeyWhereClause());

            // Add parameters for all values
            SqlCommand command = new SqlCommand(script.ToString());
            command.Parameters.AddRange(helper.GetSqlParameters().ToArray());
            return command;
        }

        private sealed class SqlParameterHelper
        {
            private readonly string _primaryKey;
            private readonly LogTableRecord _record;
            private readonly List<LogTableColumn> _columns = new List<LogTableColumn>();

            public SqlParameterHelper(LogTableDefinition table, LogTableRecord record)
            {
                _primaryKey = table.PrimaryKey;
                _record = record;

                // Ignore any columns that have no associated value in the LogTableRecord
                foreach (LogTableColumn column in table.Columns)
                {
                    if (_record.ContainsKey(column.Name))
                    {
                        _columns.Add(column);
                    }
                }
            }

            public IEnumerable<string> GetColumnNames()
            {
                foreach (LogTableColumn column in _columns)
                {
                    yield return $"[{column.Name}]";
                }
            }

            public IEnumerable<string> GetColumnParams()
            {
                foreach (LogTableColumn column in _columns)
                {
                    yield return $"@{column.Name}";
                }
            }

            public IEnumerable<string> GetUpdateAssignments()
            {
                foreach (LogTableColumn column in _columns)
                {
                    // Skip the primary key - it won't change during an update
                    if (!column.Name.EqualsIgnoreCase(_primaryKey))
                    {
                        yield return $"[{column.Name}] = @{column.Name}";
                    }
                }
            }

            public string GetPrimaryKeyWhereClause()
            {
                return $"[{_primaryKey}] = @{_primaryKey}";
            }

            public IEnumerable<SqlParameter> GetSqlParameters()
            {
                foreach (LogTableColumn column in _columns)
                {
                    // Parse data type to SQL type (must remove the (##) from the end of varchar, etc.)
                    string cleanType = new string(column.DataType.TakeWhile(n => n != '(').ToArray());
                    SqlDbType sqlType = EnumUtil.Parse<SqlDbType>(cleanType, true);

                    // Return SQL parameter object
                    yield return new SqlParameter($"@{column.Name}", sqlType)
                    {
                        Value = _record[column.Name] ?? DBNull.Value
                    };
                }
            }
        }
    }
}
