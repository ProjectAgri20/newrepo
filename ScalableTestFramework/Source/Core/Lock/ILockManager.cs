using System;
using System.Collections.Generic;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Interface for a class that handles obtaining locks on resources.
    /// </summary>
    /// <remarks>
    /// This interface represents the consumer-facing methods for lock management.
    /// Implementers should be thread-safe such that multiple threads could share the same manager.
    /// 
    /// In the client/server model, this interface should be implemented on the client side.
    /// </remarks>
    public interface ILockManager
    {
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
        LockTicket AcquireLock(string resourceId, TimeSpan waitTime, int maxConcurrent, string requestName);

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
        LockTicket AcquireLock(IEnumerable<string> resourceIds, TimeSpan waitTime, int maxConcurrent, string requestName);

        /// <summary>
        /// Releases the lock specified by the <see cref="LockTicket" />.
        /// </summary>
        /// <param name="ticket">The <see cref="LockTicket" /> identifying the lock to release.</param>
        /// <exception cref="ArgumentNullException"><paramref name="ticket" /> is null.</exception>
        void ReleaseLock(LockTicket ticket);
    }
}
