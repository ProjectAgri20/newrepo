using System;
using System.Collections.Generic;
using HP.ScalableTest.Development;
using HP.ScalableTest.Framework.Data;

namespace SDKPluginDevelopment
{
    /// <summary>
    /// Wrapper around <see cref="DataLoggerMock" /> that also implements <see cref="IDataLoggerInternal" />.
    /// </summary>
    public sealed class DataLoggerMockInternal : IDataLoggerInternal, IDataLoggerMock
    {
        private readonly DataLoggerMock _dataLoggerMock;

        /// <summary>
        /// Occurs when a new table is added to this instance.
        /// </summary>
        public event EventHandler<DataLoggerMockTableEventArgs> TableAdded;

        /// <summary>
        /// Gets the tables storing the records sent to this instance.
        /// </summary>
        public Dictionary<string, DataLoggerMockTable> Tables => _dataLoggerMock.Tables;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoggerMockInternal" /> class.
        /// </summary>
        public DataLoggerMockInternal()
        {
            _dataLoggerMock = new DataLoggerMock();
            _dataLoggerMock.TableAdded += (s, e) => TableAdded?.Invoke(this, e);
        }

        // Reroute all IDataLogger calls to the mock
        public bool Submit(ActivityDataLog dataLog) => _dataLoggerMock.Submit(dataLog);
        public bool Update(ActivityDataLog dataLog) => _dataLoggerMock.Update(dataLog);
        public void SubmitAsync(ActivityDataLog dataLog) => _dataLoggerMock.SubmitAsync(dataLog);
        public void UpdateAsync(ActivityDataLog dataLog) => _dataLoggerMock.UpdateAsync(dataLog);

        // IDataLoggerInternal is implemented but does nothing
        public bool Submit(FrameworkDataLog dataLog) => true;
        public bool Update(FrameworkDataLog dataLog) => true;
        public void SubmitAsync(FrameworkDataLog dataLog) { }
        public void UpdateAsync(FrameworkDataLog dataLog) { }
    }
}
