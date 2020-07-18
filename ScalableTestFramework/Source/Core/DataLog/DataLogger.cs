using System;
using System.ServiceModel;
using System.Threading.Tasks;
using HP.ScalableTest.Core.DataLog.Service;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Implementation of <see cref="IDataLogger" /> that communicates with an <see cref="IDataLogService" /> to log data.
    /// </summary>
    public class DataLogger : IDataLogger, IDataLoggerInternal
    {
        private readonly string _dataLogServiceAddress;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogger" /> class.
        /// </summary>
        /// <param name="dataLogServiceAddress">The address where the <see cref="IDataLogService" /> is hosted.</param>
        public DataLogger(string dataLogServiceAddress)
        {
            _dataLogServiceAddress = dataLogServiceAddress;
        }

        /// <summary>
        /// Submits the specified <see cref="ActivityDataLog" /> as a new log record.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <returns><c>true</c> if submission was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        public bool Submit(ActivityDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            LogTableDefinition table = DataLogTranslator.BuildTableDefinition(dataLog);
            LogTableRecord record = DataLogTranslator.BuildInsertRecord(dataLog);
            return Submit(table, record);
        }

        /// <summary>
        /// Updates an existing log record using data from the specified <see cref="ActivityDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <returns><c>true</c> if update was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        public bool Update(ActivityDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            LogTableDefinition table = DataLogTranslator.BuildTableDefinition(dataLog);
            LogTableRecord record = DataLogTranslator.BuildUpdateRecord(dataLog);
            return Update(table, record);
        }

        /// <summary>
        /// Asynchronously submits the specified <see cref="ActivityDataLog" /> as a new log record.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        public void SubmitAsync(ActivityDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            LogTableDefinition table = DataLogTranslator.BuildTableDefinition(dataLog);
            LogTableRecord record = DataLogTranslator.BuildInsertRecord(dataLog);
            Task.Factory.StartNew(() => Submit(table, record));
        }

        /// <summary>
        /// Asynchronously updates an existing log record using data from the specified <see cref="ActivityDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="ActivityDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        public void UpdateAsync(ActivityDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            LogTableDefinition table = DataLogTranslator.BuildTableDefinition(dataLog);
            LogTableRecord record = DataLogTranslator.BuildUpdateRecord(dataLog);
            Task.Factory.StartNew(() => Update(table, record));
        }

        /// <summary>
        /// Submits the specified <see cref="FrameworkDataLog" /> as a new log record.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <returns><c>true</c> if submission was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        public bool Submit(FrameworkDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            LogTableDefinition table = DataLogTranslator.BuildTableDefinition(dataLog);
            LogTableRecord record = DataLogTranslator.BuildInsertRecord(dataLog);
            return Submit(table, record);
        }

        /// <summary>
        /// Updates an existing log record using data from the specified <see cref="FrameworkDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <returns><c>true</c> if update was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        public bool Update(FrameworkDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            LogTableDefinition table = DataLogTranslator.BuildTableDefinition(dataLog);
            LogTableRecord record = DataLogTranslator.BuildUpdateRecord(dataLog);
            return Update(table, record);
        }

        /// <summary>
        /// Asynchronously submits the specified <see cref="FrameworkDataLog" /> as a new log record.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        public void SubmitAsync(FrameworkDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            LogTableDefinition table = DataLogTranslator.BuildTableDefinition(dataLog);
            LogTableRecord record = DataLogTranslator.BuildInsertRecord(dataLog);
            Task.Factory.StartNew(() => Submit(table, record));
        }

        /// <summary>
        /// Asynchronously updates an existing log record using data from the specified <see cref="FrameworkDataLog" />.
        /// </summary>
        /// <param name="dataLog">The <see cref="FrameworkDataLog" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="dataLog" /> is null.</exception>
        public void UpdateAsync(FrameworkDataLog dataLog)
        {
            if (dataLog == null)
            {
                throw new ArgumentNullException(nameof(dataLog));
            }

            LogTableDefinition table = DataLogTranslator.BuildTableDefinition(dataLog);
            LogTableRecord record = DataLogTranslator.BuildUpdateRecord(dataLog);
            Task.Factory.StartNew(() => Update(table, record));
        }

        private bool Submit(LogTableDefinition table, LogTableRecord record)
        {
            LogTrace($"Submitting INSERT to table {table.Name} (primary key = {record[table.PrimaryKey]})");

            bool success = false;
            using (DataLogServiceClient client = new DataLogServiceClient(_dataLogServiceAddress))
            {
                void insert() => success = client.Insert(table, record);
                Retry.WhileThrowing<CommunicationException>(insert, 10, TimeSpan.FromSeconds(1));
            }

            if (!success)
            {
                LogWarn($"Data Log submit for {table.Name} failed.");
            }

            return success;
        }

        private bool Update(LogTableDefinition table, LogTableRecord record)
        {
            LogTrace($"Submitting UPDATE to table {table.Name} (primary key = {record[table.PrimaryKey]})");

            bool success = false;
            using (DataLogServiceClient client = new DataLogServiceClient(_dataLogServiceAddress))
            {
                void update() => success = client.Update(table, record);
                Retry.WhileThrowing<CommunicationException>(update, 10, TimeSpan.FromSeconds(1));
            }

            if (!success)
            {
                LogWarn($"Data Log update for {table.Name} failed.");
            }

            return success;
        }
    }
}
