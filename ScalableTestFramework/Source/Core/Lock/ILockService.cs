using System;
using System.Collections.Generic;
using System.ServiceModel;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Interface for the server-side portion of a distributed <see cref="ILockManager" /> communication channel.
    /// </summary>
    /// <remarks>
    /// In a distributed <see cref="ILockManager" /> implementation, the server-side lock manager
    /// is generally hosted in a WCF service.  This interface forms the service contract for such a service.
    /// 
    /// In the client/server model, this interface should be implemented on the server side.
    /// </remarks>
    [ServiceContract]
    public interface ILockService
    {
        /// <summary>
        /// Enqueues a lock request for the specified resource.
        /// </summary>
        /// <param name="requestId">The unique identifier for the request.</param>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="requestName">The name to use for identifying the request.</param>
        /// <param name="maxConcurrent">The maximum number of consumers that can use the specified resource concurrently.</param>
        [OperationContract]
        void RequestLock(Guid requestId, string resourceId, string requestName, int maxConcurrent);

        /// <summary>
        /// Enqueues a lock request for any of the specified resources.
        /// </summary>
        /// <param name="requestId">The unique identifier for the request.</param>
        /// <param name="resourceIds">The list of resource identifiers.</param>
        /// <param name="requestName">The name to use for identifying the request.</param>
        /// <param name="maxConcurrent">The maximum number of consumers that can use the specified resource concurrently.</param>
        [OperationContract(Name = "RequestLockMultiple")]
        void RequestLock(Guid requestId, IEnumerable<string> resourceIds, string requestName, int maxConcurrent);

        /// <summary>
        /// Releases the lock with the specified request ID.
        /// </summary>
        /// <param name="requestId">The lock request identifier.</param>
        [OperationContract]
        void ReleaseLock(Guid requestId);

        /// <summary>
        /// Cancels the lock request with the specified request ID.
        /// </summary>
        /// <param name="requestId">The lock request identifier.</param>
        [OperationContract]
        void CancelRequest(Guid requestId);

        /// <summary>
        /// Pings this instance.
        /// </summary>
        /// <returns><c>true</c> if the ping was successful.</returns>
        [OperationContract]
        bool Ping();
    }
}
