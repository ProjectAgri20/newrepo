using System;
using System.Collections.Generic;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// A simple implementation of <see cref="IDataLogger" /> for development.
    /// </summary>
    public sealed class DataLoggerMock : IDataLogger, IDataLoggerMock
    {
        /// <summary>
        /// Occurs when a new <see cref="DataLoggerMockTable" /> is added to this instance.
        /// </summary>
        public event EventHandler<DataLoggerMockTableEventArgs> TableAdded;

        /// <summary>
        /// Gets the tables storing the records sent to this instance.
        /// </summary>
        public Dictionary<string, DataLoggerMockTable> Tables { get; } = new Dictionary<string, DataLoggerMockTable>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoggerMock" /> class.
        /// </summary>
        public DataLoggerMock()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Submits the specified <see cref="ActivityDataLog" /> as a new log record.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <returns><c>true</c> if submission was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="DataLoggerMockException">An issue with the data log was detected.</exception>
        public bool Submit(ActivityDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            if (Tables.ContainsKey(dataLog.TableName))
            {
                Tables[dataLog.TableName].Add(dataLog);
            }
            else
            {
                DataLoggerMockTable newTable = new DataLoggerMockTable(dataLog);
                Tables.Add(dataLog.TableName, newTable);
                TableAdded?.Invoke(this, new DataLoggerMockTableEventArgs(newTable));
            }
            return true;
        }

        /// <summary>
        /// Updates an existing log record using data from the specified <see cref="ActivityDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <returns><c>true</c> if update was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="DataLoggerMockException">An issue with the data log was detected.</exception>
        public bool Update(ActivityDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            if (Tables.ContainsKey(dataLog.TableName))
            {
                Tables[dataLog.TableName].Update(dataLog);
            }
            else
            {
                throw new DataLoggerMockException($"Received an Update command for non-existent table '{dataLog.TableName}'.");
            }
            return true;
        }

        /// <summary>
        /// Submits the specified <see cref="ActivityDataLog" /> as a new log record.
        /// (This mock does not make an async call.)
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="DataLoggerMockException">An issue with the data log was detected.</exception>
        public void SubmitAsync(ActivityDataLog dataLog)
        {
            Submit(dataLog);
        }

        /// <summary>
        /// Updates an existing log record using data from the specified <see cref="ActivityDataLog" />.
        /// (This mock does not make an async call.)
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="DataLoggerMockException">An issue with the data log was detected.</exception>
        public void UpdateAsync(ActivityDataLog dataLog)
        {
            Update(dataLog);
        }
    }
}
