using System;
using System.Collections.Generic;
using System.ServiceModel;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Core.DataLog.Service
{
    /// <summary>
    /// Service contract for the Data Log service.
    /// </summary>
    [ServiceContract]
    public interface IDataLogService
    {
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
        [OperationContract]
        bool Insert(LogTableDefinition table, LogTableRecord record);

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
        [OperationContract]
        bool Update(LogTableDefinition table, LogTableRecord record);

        /// <summary>
        /// Gets the activity counts for the specified session.
        /// </summary>
        /// <param name="sessionId">The session ID.</param>
        /// <returns>The activity counts for the specified session, grouped by result.</returns>
        [OperationContract]
        Dictionary<PluginResult, int> GetSessionActivityCounts(string sessionId);

        /// <summary>
        /// Pings this instance.
        /// </summary>
        /// <returns><c>true</c> if the ping was successful.</returns>
        [OperationContract]
        bool Ping();
    }
}
