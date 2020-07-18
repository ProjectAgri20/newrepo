using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Synchronization;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// An implementation of <see cref="ILockManager" /> that uses a client/server model
    /// to handle locks for resources across multiple processes on multiple systems.
    /// </summary>
    /// <remarks>
    /// This class is thread-safe and can be shared between threads in a single process.
    /// It will also work if each thread creates its own instance.
    /// 
    /// In the client/server model, the <see cref="DistributedLockManager" /> is the consumer-facing client-side component.
    /// </remarks>
    public sealed class DistributedLockManager : ILockManager
    {
        private readonly string _lockServiceAddress;
        private readonly ConcurrentDictionary<Guid, LockServiceClient> _lockClients = new ConcurrentDictionary<Guid, LockServiceClient>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DistributedLockManager"/> class.
        /// </summary>
        /// <param name="lockServiceAddress">The address where the <see cref="ILockService" /> is hosted.</param>
        public DistributedLockManager(string lockServiceAddress)
        {
            _lockServiceAddress = lockServiceAddress;
        }

        /// <summary>
        /// Waits to acquire a lock on the specified resource.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="waitTime">The amount of time to wait for the lock before returning.</param>
        /// <param name="maxConcurrent">The maximum number of consumers that can use the specified resource concurrently.</param>
        /// <param name="requestName">A name used to identify the request.</param>
        /// <returns>A <see cref="LockTicket" /> representing the lock that was acquired.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="resourceId" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="waitTime" /> is less than or equal to zero.
        /// <para>or</para>
        /// <paramref name="maxConcurrent" /> is less than or equal to zero.
        /// </exception>
        /// <exception cref="AcquireLockTimeoutException">The lock could not be acquired within the specified wait time.</exception>
        public LockTicket AcquireLock(string resourceId, TimeSpan waitTime, int maxConcurrent, string requestName)
        {
            if (resourceId == null)
            {
                throw new ArgumentNullException(nameof(resourceId));
            }

            if (waitTime <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(waitTime), "Wait time must be greater than zero.");
            }

            if (maxConcurrent <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent), "Maximum concurrency must be greater than zero.");
            }

            LogDebug($"Requesting lock for: {resourceId}");
            void requestLock(LockServiceClient client) => client.RequestLock(resourceId, maxConcurrent);
            return AcquireLock(requestLock, waitTime, requestName);
        }

        /// <summary>
        /// Waits to acquire a lock on any of the specified resources.
        /// </summary>
        /// <param name="resourceIds">The list of resource identifiers.</param>
        /// <param name="waitTime">The amount of time to wait for the lock before returning.</param>
        /// <param name="maxConcurrent">The maximum number of consumers that can use the specified resource concurrently.</param>
        /// <param name="requestName">A name used to identify the request.</param>
        /// <returns>A <see cref="LockTicket" /> representing the lock that was acquired.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="resourceIds" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="resourceIds" /> contains no elements.</exception>
        /// <exception cref="ArgumentOutOfRangeException">
        /// <paramref name="waitTime" /> is less than or equal to zero.
        /// <para>or</para>
        /// <paramref name="maxConcurrent" /> is less than or equal to zero.
        /// </exception>
        /// <exception cref="AcquireLockTimeoutException">The lock could not be acquired within the specified wait time.</exception>
        public LockTicket AcquireLock(IEnumerable<string> resourceIds, TimeSpan waitTime, int maxConcurrent, string requestName)
        {
            if (resourceIds == null)
            {
                throw new ArgumentNullException(nameof(resourceIds));
            }

            if (!resourceIds.Any())
            {
                throw new ArgumentException("At least one resource must be specified.", nameof(resourceIds));
            }

            if (waitTime <= TimeSpan.Zero)
            {
                throw new ArgumentOutOfRangeException(nameof(waitTime), "Wait time must be greater than zero.");
            }

            if (maxConcurrent <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent), "Maximum concurrency must be greater than zero.");
            }

            LogDebug($"Requesting lock for any of: {string.Join(", ", resourceIds)}");
            void requestLock(LockServiceClient client) => client.RequestLock(resourceIds, maxConcurrent);
            return AcquireLock(requestLock, waitTime, requestName);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Must dispose lock client if a call fails for any reason.")]
        private LockTicket AcquireLock(Action<LockServiceClient> requestAction, TimeSpan waitTime, string requestName)
        {
            // Create a new LockServiceClient, then request the lock and wait for a response.
            LockServiceClient lockClient = new LockServiceClient(_lockServiceAddress, requestName);
            try
            {
                requestAction(lockClient);
            }
            catch (Exception ex)
            {
                // It is crucial to dispose the lock client here to avoid any lingering callback hosts
                LogError($"Unable to send acquire request to lock service.", ex);
                lockClient.Dispose();
                throw;
            }

            LogDebug("Lock requested, waiting for grant.");
            LockTicket ticket = lockClient.WaitForLockGranted(waitTime);
            if (ticket != null)
            {
                // Request was granted - persist the lock client for this request so that the service
                // can ping it periodically to make sure it is still alive.
                LogDebug($"Acquired lock for {ticket.ResourceId}.");
                _lockClients.TryAdd(ticket.RequestId, lockClient);
                return ticket;
            }
            else
            {
                // Did not get the lock in time.  Cancel the request on the server and dispose the client.
                LogDebug($"Unable to acquire lock within {waitTime}.  Canceling request.");
                try
                {
                    lockClient.CancelRequest();
                }
                catch (Exception ex)
                {
                    LogError($"Unable to send cancel request to lock service.", ex);
                }
                finally
                {
                    // Must ensure the client is disposed even if the cancel request fails.
                    lockClient.Dispose();
                }
                throw new AcquireLockTimeoutException($"Unable to acquire lock within {waitTime}.");
            }
        }

        /// <summary>
        /// Releases the lock specified by the <see cref="LockTicket" />.
        /// </summary>
        /// <param name="ticket">The <see cref="LockTicket" /> identifying the lock to release.</param>
        /// <exception cref="ArgumentNullException"><paramref name="ticket" /> is null.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Crucial to dispose lock client if a call fails for any reason.")]
        public void ReleaseLock(LockTicket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            // Get the lock client for the granted request, release the lock, and dispose the client.
            if (_lockClients.TryRemove(ticket.RequestId, out LockServiceClient lockClient))
            {
                LogDebug($"Releasing lock for {ticket.ResourceId}.");
                try
                {
                    lockClient.ReleaseLock();
                }
                catch (Exception ex)
                {
                    LogError($"Unable to send release request to lock service.", ex);
                }
                finally
                {
                    // Must ensure the client is disposed even if the release request fails.
                    // Otherwise the client will keep responding to the ping requests from the lock service
                    // and the lock might never get cleaned up.
                    lockClient.Dispose();
                }
            }
        }
    }
}
