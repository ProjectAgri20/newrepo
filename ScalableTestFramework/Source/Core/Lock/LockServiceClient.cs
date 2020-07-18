using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.ScalableTest.Framework.Wcf;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// The client-side component that communicates with an <see cref="ILockService" /> implementation on the server.
    /// </summary>
    /// <remarks>
    /// This class serves two purposes: it communicates with an <see cref="ILockService" /> to
    /// request and release locks, and it hosts the <see cref="ILockCallback" />
    /// implementation for the service to communicate back when a lock has been granted.
    /// 
    /// In the client/server model, the <see cref="LockServiceClient" /> is a client-side component.
    /// </remarks>
    public sealed class LockServiceClient : WcfDualChannelClient<ILockService, ILockCallback>
    {
        private readonly Guid _requestId;
        private readonly string _requestName;
        private readonly AutoResetEvent _waitEvent;
        private readonly LockCallback _callback;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockServiceClient" /> class.
        /// </summary>
        /// <param name="lockServiceAddress">The address where the <see cref="ILockService" /> is hosted.</param>
        /// <param name="requestName">The name to use for identifying the request.</param>
        public LockServiceClient(string lockServiceAddress, string requestName)
            : this(LockServiceEndpoint.BuildUri(lockServiceAddress), requestName, Guid.NewGuid(), new AutoResetEvent(false))
        {
            // Constructor chaining is used here to solve problems with creating new objects (IDs, etc.) that need
            // to be stored locally but also used to create other objects needed by the base constructor.
        }

        private LockServiceClient(Uri lockServiceUri, string requestName, Guid requestId, AutoResetEvent waitEvent)
            : this(lockServiceUri, new LockCallback(requestId, waitEvent), requestName)
        {
            _requestId = requestId;
            _requestName = requestName;
            _waitEvent = waitEvent;
        }

        private LockServiceClient(Uri endpoint, LockCallback callback, string requestName)
            : base(endpoint, callback, requestName)
        {
            _callback = callback;
        }

        /// <summary>
        /// Requests a lock for the specified resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="maxConcurrent">The maximum number of consumers that can use the specified resource concurrently.</param>
        /// <exception cref="ArgumentNullException"><paramref name="resourceId" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxConcurrent" /> is less than or equal to zero.</exception>
        public void RequestLock(string resourceId, int maxConcurrent)
        {
            if (resourceId == null)
            {
                throw new ArgumentNullException(nameof(resourceId));
            }

            if (maxConcurrent <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent), "Maximum concurrency must be greater than zero.");
            }

            Channel.RequestLock(_requestId, resourceId, _requestName, maxConcurrent);
        }

        /// <summary>
        /// Requests a lock for any of the specified resources.
        /// </summary>
        /// <param name="resourceIds">The list of resource identifiers.</param>
        /// <param name="maxConcurrent">The maximum number of consumers that can use the specified resource concurrently.</param>
        /// <exception cref="ArgumentNullException"><paramref name="resourceIds" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="resourceIds" /> contains no elements.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxConcurrent" /> is less than or equal to zero.</exception>
        public void RequestLock(IEnumerable<string> resourceIds, int maxConcurrent)
        {
            if (resourceIds == null)
            {
                throw new ArgumentNullException(nameof(resourceIds));
            }

            if (!resourceIds.Any())
            {
                throw new ArgumentException("At least one resource must be specified.", nameof(resourceIds));
            }

            if (maxConcurrent <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent), "Maximum concurrency must be greater than zero.");
            }

            Channel.RequestLock(_requestId, resourceIds, _requestName, maxConcurrent);
        }

        /// <summary>
        /// Releases the lock obtained by this client.
        /// </summary>
        public void ReleaseLock()
        {
            Channel.ReleaseLock(_requestId);
        }

        /// <summary>
        /// Cancels the lock request sent from this client.
        /// </summary>
        public void CancelRequest()
        {
            Channel.CancelRequest(_requestId);
        }

        /// <summary>
        /// Waits for the lock requested by this client to be granted.
        /// </summary>
        /// <param name="waitTime">The amount of time to wait for the lock before returning.</param>
        /// <returns>A <see cref="LockTicket" /> if the request was granted, or null if the request was not granted.</returns>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="waitTime" /> is less than or equal to zero.</exception>
        public LockTicket WaitForLockGranted(TimeSpan waitTime)
        {
            if (waitTime <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(waitTime), "Wait time must be greater than zero.");
            }

            bool success = _waitEvent.WaitOne(waitTime);
            return success ? _callback.LockTicket : null;
        }

        #region IDisposable Members

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources.
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            // Deactivate the callback.  It should be cleaned up with the dispose, but this turns it off a bit sooner.
            LogDebug("Deactivating lock service callback.");
            _callback.Active = false;

            base.Dispose(disposing);

            if (disposing)
            {
                _waitEvent.Dispose();
            }
        }

        #endregion
    }
}
