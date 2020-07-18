using System;
using System.Collections.Generic;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Interface for a class that acts as a mock for the <see cref="IDataLogger" /> service
    /// and allows consumers to query records sent through the service.
    /// </summary>
    public interface IDataLoggerMock
    {
        /// <summary>
        /// Occurs when a new <see cref="DataLoggerMockTable" /> is added to this instance.
        /// </summary>
        event EventHandler<DataLoggerMockTableEventArgs> TableAdded;

        /// <summary>
        /// Gets the tables storing the records sent to this instance.
        /// </summary>
        Dictionary<string, DataLoggerMockTable> Tables { get; }
    }
}
