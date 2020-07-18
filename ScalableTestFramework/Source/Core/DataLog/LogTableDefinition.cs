using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Defines the schema for a database table containing log data.
    /// </summary>
    [DataContract]
    public sealed class LogTableDefinition : IEnumerable<LogTableColumn>
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _name;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private string _primaryKey;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.RootHidden)]
        private List<LogTableColumn> _columns = new List<LogTableColumn>();  // List<T> ensures column order is maintained

        /// <summary>
        /// Gets the name of the table.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the name of the primary key column for the table.
        /// </summary>
        public string PrimaryKey => _primaryKey;

        /// <summary>
        /// Gets the <see cref="LogTableColumn" /> definitions for the table.
        /// </summary>
        public IEnumerable<LogTableColumn> Columns => _columns;

        /// <summary>
        /// Initializes a new instance of the <see cref="LogTableDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the table.</param>
        /// <exception cref="ArgumentException"><paramref name="name" /> is null or empty.</exception>
        public LogTableDefinition(string name)
            : this(name, $"{name}Id")
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LogTableDefinition" /> class.
        /// </summary>
        /// <param name="name">The name of the table.</param>
        /// <param name="primaryKey">The name of the primary key column for the table.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="name" /> is null or empty.
        /// <para>or</para>
        /// <paramref name="primaryKey" /> is null or empty.
        /// </exception>
        public LogTableDefinition(string name, string primaryKey)
        {
            if (string.IsNullOrEmpty(name))
            {
                throw new ArgumentException("Table name must be defined.", nameof(name));
            }

            if (string.IsNullOrEmpty(primaryKey))
            {
                throw new ArgumentException("Primary key must be defined.", nameof(primaryKey));
            }

            _name = name;
            _primaryKey = primaryKey;
        }

        /// <summary>
        /// Adds the specified <see cref="LogTableColumn" /> to this table.
        /// </summary>
        /// <param name="column">The <see cref="LogTableColumn" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="column" /> is null.</exception>
        public void Add(LogTableColumn column)
        {
            if (column == null)
            {
                throw new ArgumentNullException(nameof(column));
            }

            _columns.Add(column);
        }

        #region IEnumerable Members

        IEnumerator<LogTableColumn> IEnumerable<LogTableColumn>.GetEnumerator() => _columns.GetEnumerator();
        IEnumerator IEnumerable.GetEnumerator() => _columns.GetEnumerator();

        #endregion
    }
}
