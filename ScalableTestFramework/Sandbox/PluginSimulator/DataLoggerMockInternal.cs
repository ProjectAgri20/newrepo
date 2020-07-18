using System;
using System.Collections.Generic;
using HP.ScalableTest.Development;
using HP.ScalableTest.Framework.Data;

namespace PluginSimulator
{
    internal sealed class DataLoggerMockInternal : IDataLogger, IDataLoggerInternal, IDataLoggerMock
    {
        private readonly DataLoggerMock _dataLoggerMock;

        /// <summary>
        /// Occurs when a new <see cref="DataLoggerMockTable" /> is added to this instance.
        /// </summary>
        public event EventHandler<DataLoggerMockTableEventArgs> TableAdded;

        /// <summary>
        /// Gets the tables storing the records sent to this instance.
        /// </summary>
        public Dictionary<string, DataLoggerMockTable> Tables => _dataLoggerMock.Tables;

        public DataLoggerMockInternal()
        {
            _dataLoggerMock = new DataLoggerMock();
            _dataLoggerMock.TableAdded += (s, e) => TableAdded?.Invoke(this, e);
        }

        /// <summary>
        /// Submits the specified <see cref="ActivityDataLog" /> as a new log record.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <returns><c>true</c> if submission was successful; an exception is thrown otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="DataLoggerMockException">An issue with the data log was detected.</exception>
        public bool Submit(ActivityDataLog dataLog)
        {
            return _dataLoggerMock.Submit(dataLog);
        }

        /// <summary>
        /// Updates an existing log record using data from the specified <see cref="ActivityDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <returns><c>true</c> if update was successful; an exception is thrown otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="DataLoggerMockException">An issue with the data log was detected.</exception>
        public bool Update(ActivityDataLog dataLog)
        {
            return _dataLoggerMock.Update(dataLog);
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
            _dataLoggerMock.SubmitAsync(dataLog);
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
            _dataLoggerMock.UpdateAsync(dataLog);
        }

        /// <summary>
        /// Submits the specified <see cref="FrameworkDataLog" /> as a new log record.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <returns><c>true</c> if submission was successful; an exception is thrown otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="DataLoggerMockException">An issue with the data log was detected.</exception>
        public bool Submit(FrameworkDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            // Not implemented
            return true;
        }

        /// <summary>
        /// Updates an existing log record using data from the specified <see cref="FrameworkDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <returns><c>true</c> if update was successful; an exception is thrown otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="DataLoggerMockException">An issue with the data log was detected.</exception>
        public bool Update(FrameworkDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            // Not implemented
            return true;
        }

        /// <summary>
        /// Submits the specified <see cref="FrameworkDataLog" /> as a new log record.
        /// (This mock does not make an async call.)
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="DataLoggerMockException">An issue with the data log was detected.</exception>
        public void SubmitAsync(FrameworkDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            // Not implemented
            return;
        }

        /// <summary>
        /// Updates an existing log record using data from the specified <see cref="FrameworkDataLog" />.
        /// (This mock does not make an async call.)
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        /// <exception cref="DataLoggerMockException">An issue with the data log was detected.</exception>
        public void UpdateAsync(FrameworkDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            // Not implemented
            return;
        }
    }
}