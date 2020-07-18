using System;
using System.Collections.Generic;

namespace HP.ScalableTest.Framework.Synchronization
{
    /// <summary>
    /// Provides a mechanism for executing code sections serially (or with limited concurrency) across multiple processes.
    /// </summary>
    public interface ICriticalSection
    {
        /// <summary>
        /// Obtains an exclusive lock according to the specified <see cref="LockToken" />,
        /// then executes the specified <see cref="Action" /> before releasing the lock.
        /// </summary>
        /// <param name="token">The token dictating the scope and behavior of execution.</param>
        /// <param name="action">The action to execute.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="token" /> is null.
        /// <para>or</para>
        /// <paramref name="action" /> is null.
        /// </exception>
        /// <exception cref="AcquireLockTimeoutException">The lock could not be obtained within the acquisition timeout specified by <paramref name="token" />.</exception>
        /// <exception cref="HoldLockTimeoutException"><paramref name="action" /> did not complete within the hold timeout specified by <paramref name="token" />.</exception>
        void Run(LockToken token, Action action);

        /// <summary>
        /// Selects a <see cref="LockToken" /> from the provided list, based on current
        /// availability of the associated resources, then obtains an exlusive lock
        /// and executes the specified <see cref="Action{LockToken}" /> before releasing the lock.
        /// </summary>
        /// <param name="tokens">The collection of tokens to choose from.</param>
        /// <param name="action">The action to execute; takes the acquired token as a parameter.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="tokens" /> is null.
        /// <para>or</para>
        /// <paramref name="action" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="tokens" /> contains no elements.</exception>
        /// <exception cref="AcquireLockTimeoutException">The lock could not be obtained within the acquisition timeout specified by the selected token.</exception>
        /// <exception cref="HoldLockTimeoutException"><paramref name="action" /> did not complete within the hold timeout specified by the selected token.</exception>
        void Run(IEnumerable<LockToken> tokens, Action<LockToken> action);

        /// <summary>
        /// Obtains a semaphore-based lock according to the specified <see cref="LockToken" />,
        /// then executes the specified <see cref="Action" /> before releasing the lock.
        /// The number of clients allowed to simultaneously hold the lock is defined by the concurrency count.
        /// </summary>
        /// <param name="token">The token dictating the scope and behavior of execution.</param>
        /// <param name="action">The action to execute.</param>
        /// <param name="concurrencyCount">The number of clients that are allowed to execute concurrently.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="token" /> is null.
        /// <para>or</para>
        /// <paramref name="action" /> is null.
        /// </exception>
        /// <exception cref="AcquireLockTimeoutException">The lock could not be obtained within the acquisition timeout specified by <paramref name="token" />.</exception>
        /// <exception cref="HoldLockTimeoutException"><paramref name="action" /> did not complete within the hold timeout specified by <paramref name="token" />.</exception>
        void RunConcurrent(LockToken token, Action action, int concurrencyCount);
    }
}
