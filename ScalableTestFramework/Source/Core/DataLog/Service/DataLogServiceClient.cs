using System;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Core.DataLog.Service
{
    /// <summary>
    /// Client class for connecting to an <see cref="IDataLogService" />.
    /// </summary>
    public sealed class DataLogServiceClient : WcfClient<IDataLogService>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogServiceClient" /> class.
        /// </summary>
        /// <param name="dataLogServiceAddress">The address where the <see cref="IDataLogService" /> is hosted.</param>
        public DataLogServiceClient(string dataLogServiceAddress)
            : base(DataLogServiceEndpoint.MessageTransferType, DataLogServiceEndpoint.BuildUri(dataLogServiceAddress))
        {
        }

        /// <summary>
        /// Inserts data from the specified <see cref="LogTableRecord" /> into the table designated by the <see cref="LogTableDefinition" />.
        /// </summary>
        /// <param name="table">The <see cref="LogTableDefinition" />.</param>
        /// <param name="record">The <see cref="LogTableRecord" />.</param>
        /// <returns><c>true</c> if submission was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="table" /> is null.
        /// <para>or</para>
        /// <paramref name="record" /> is null.
        /// </exception>
        public bool Insert(LogTableDefinition table, LogTableRecord record)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            return Channel.Insert(table, record);
        }

        /// <summary>
        /// Updates data from the specified <see cref="LogTableRecord" /> into the table designated by the <see cref="LogTableDefinition" />.
        /// </summary>
        /// <param name="table">The <see cref="LogTableDefinition" />.</param>
        /// <param name="record">The <see cref="LogTableRecord" />.</param>
        /// <returns><c>true</c> if submission was successful, <c>false</c> otherwise.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="table" /> is null.
        /// <para>or</para>
        /// <paramref name="record" /> is null.
        /// </exception>
        public bool Update(LogTableDefinition table, LogTableRecord record)
        {
            if (table == null)
            {
                throw new ArgumentNullException(nameof(table));
            }

            if (record == null)
            {
                throw new ArgumentNullException(nameof(record));
            }

            return Channel.Update(table, record);
        }
    }
}
