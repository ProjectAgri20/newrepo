using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// A simple lock manager that handles locks for resources within a single process.
    /// </summary>
    /// <remarks>
    /// This is an implementation of <see cref="ILockManager" /> that is useful for local debugging.
    /// Rather than a client/server model, this implementation handles all the locks in-process.
    /// As such, it inherits from <see cref="LockRequestManager" /> for the "server" functionality
    /// and implements <see cref="ILockManager" /> for the "client" functionality.
    /// 
    /// All process threads must use the same <see cref="ProcessLockManager" /> instance.
    /// This class will not work if each thread creates its own instance.
    /// </remarks>
    public sealed class ProcessLockManager : LockRequestManager, ILockManager
    {
        private readonly ConcurrentDictionary<Guid, LockTicket> _lockTickets = new ConcurrentDictionary<Guid, LockTicket>();
        private readonly ConcurrentDictionary<Guid, AutoResetEvent> _waitEvents = new ConcurrentDictionary<Guid, AutoResetEvent>();

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

            SetConcurrency(resourceId, maxConcurrent);
            void requestAction(Guid requestId) => RequestLock(requestId, resourceId, requestName);
            return AcquireLock(requestAction, waitTime);
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

            foreach (string resourceId in resourceIds)
            {
                SetConcurrency(resourceId, maxConcurrent);
            }

            void requestAction(Guid requestId) => RequestLock(requestId, resourceIds, requestName);
            return AcquireLock(requestAction, waitTime);
        }

        private LockTicket AcquireLock(Action<Guid> requestAction, TimeSpan waitTime)
        {
            // Create a new request ID and corresponding reset event.
            // Add the reset event to the dictionary so that it can be found in the GrantLock method.
            Guid requestId = Guid.NewGuid();
            AutoResetEvent waitEvent = new AutoResetEvent(false);
            _waitEvents.TryAdd(requestId, waitEvent);

            // Use the specified method to request the lock, then wait for success.
            requestAction(requestId);
            bool success = waitEvent.WaitOne(waitTime);

            if (success)
            {
                // Dispose of the wait event
                waitEvent.Dispose();

                // Return the ticket representing the granted lock.
                _lockTickets.TryRemove(requestId, out LockTicket ticket);
                return ticket;
            }
            else
            {
                // Cancel the request
                CancelRequest(requestId);

                // Remove and dispose of the wait event
                if (_waitEvents.TryRemove(requestId, out AutoResetEvent removed))
                {
                    removed.Dispose();
                }

                // Cancel the request and throw an exception.
                throw new AcquireLockTimeoutException($"Unable to acquire lock within {waitTime}.");
            }
        }

        /// <summary>
        /// Releases the lock specified by the <see cref="LockTicket" />.
        /// </summary>
        /// <param name="ticket">The <see cref="LockTicket" /> identifying the lock to release.</param>
        /// <exception cref="ArgumentNullException"><paramref name="ticket" /> is null.</exception>
        public void ReleaseLock(LockTicket ticket)
        {
            if (ticket == null)
            {
                throw new ArgumentNullException(nameof(ticket));
            }

            ReleaseLock(ticket.RequestId);
        }

        /// <summary>
        /// When overridden in a derived class, defines the action to take when a lock request has been granted.
        /// </summary>
        /// <param name="requestId">The unique identifier for the request.</param>
        /// <param name="resourceId">The identifier of the resource for which a lock has been granted.</param>
        protected override void GrantLock(Guid requestId, string resourceId)
        {
            // Remove the wait event from the dictionary - it's not needed any more
            // and will be disposed back in the AcquireLock method.
            if (_waitEvents.TryRemove(requestId, out AutoResetEvent waitEvent))
            {
                // Add the corresponding lock ticket, then set the event.
                if (_lockTickets.TryAdd(requestId, new LockTicket(requestId, resourceId)))
                {
                    waitEvent.Set();
                }
            }
        }
    }
}
