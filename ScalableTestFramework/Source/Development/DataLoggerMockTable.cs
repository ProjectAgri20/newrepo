using System;
using System.Collections.Generic;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// A mock table containing data log records.
    /// </summary>
    public sealed class DataLoggerMockTable
    {
        /// <summary>
        /// Occurs when the data in this table is updated.
        /// </summary>
        public event EventHandler DataUpdated;

        /// <summary>
        /// The type of data stored in the table.
        /// </summary>
        public Type LogType { get; }

        /// <summary>
        /// The table name.
        /// </summary>
        public string TableName { get; }

        /// <summary>
        /// The records stored in the table.
        /// </summary>
        public Dictionary<Guid, DataLoggerMockRecord> Records { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoggerMockTable" /> class.
        /// </summary>
        /// <param name="dataLog">The initial <see cref="ActivityDataLog" /> to insert into the table.</param>
        internal DataLoggerMockTable(ActivityDataLog dataLog)
        {
            TableName = dataLog.TableName;
            LogType = dataLog.GetType();
            Records = new Dictionary<Guid, DataLoggerMockRecord>();
            Add(dataLog);
        }

        /// <summary>
        /// Adds a new data log record to this table.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" /> to add.</param>
        internal void Add(ActivityDataLog dataLog)
        {
            ValidateType(dataLog);

            if (Records.ContainsKey(dataLog.RecordId))
            {
                throw new DataLoggerMockException(string.Format("The {0} table received a record with a duplicate key.", TableName));
            }

            Records.Add(dataLog.RecordId, new DataLoggerMockRecord(dataLog));
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }

        /// <summary>
        /// Updates the specified data log in this table.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" /> to update.</param>
        internal void Update(ActivityDataLog dataLog)
        {
            ValidateType(dataLog);

            if (!Records.ContainsKey(dataLog.RecordId))
            {
                throw new DataLoggerMockException($"The {TableName} table received an Update command for a record that does not exist.");
            }

            Records[dataLog.RecordId].Update(dataLog);
            DataUpdated?.Invoke(this, EventArgs.Empty);
        }

        private void ValidateType(ActivityDataLog dataLog)
        {
            Type currentLogType = dataLog.GetType();
            if (currentLogType != LogType)
            {
                throw new DataLoggerMockException($"The {TableName} table received a record of type '{currentLogType}' but expected records of type '{LogType}'.");
            }
        }
    }
}
