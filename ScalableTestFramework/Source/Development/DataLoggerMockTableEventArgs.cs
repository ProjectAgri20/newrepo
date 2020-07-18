using System;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Event args containing a <see cref="DataLoggerMockTable" /> instance.
    /// </summary>
    public sealed class DataLoggerMockTableEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="DataLoggerMockTable" />.
        /// </summary>
        public DataLoggerMockTable Table { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLoggerMockTableEventArgs" /> class.
        /// </summary>
        /// <param name="table">The <see cref="DataLoggerMockTable" />.</param>
        public DataLoggerMockTableEventArgs(DataLoggerMockTable table)
        {
            Table = table;
        }
    }
}
