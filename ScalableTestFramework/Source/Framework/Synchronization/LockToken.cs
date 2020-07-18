using System;
using System.Diagnostics;

namespace HP.ScalableTest.Framework.Synchronization
{
    /// <summary>
    /// Defines a resource to lock on for an <see cref="ICriticalSection" /> operation.
    /// </summary>
    [DebuggerDisplay("{Key,nq}")]
    public abstract class LockToken
    {
        /// <summary>
        /// The key representing the resource to lock.
        /// </summary>
        public string Key { get; }

        /// <summary>
        /// The amount of time to wait to acquire a lock.  If this time is exceeded without
        /// acquiring the lock, an <see cref="AcquireLockTimeoutException" /> will be thrown.
        /// </summary>
        public TimeSpan AcquireTimeout { get; }

        /// <summary>
        /// The amount of time to hold a lock.  If this time is exceeded during execution,
        /// the running action will be terminated and a <see cref="HoldLockTimeoutException" /> will be thrown.
        /// </summary>
        public TimeSpan HoldTimeout { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockToken" /> class with the specified lock timeouts.
        /// </summary>
        /// <param name="key">The key representing the resource to lock.</param>
        /// <param name="acquireTimeout">The amount of time to wait to acquire the lock.</param>
        /// <param name="holdTimeout">The amount of time to hold the lock.</param>
        /// <exception cref="ArgumentNullException"><paramref name="key" /> is null.</exception>
        /// <exception cref="ArgumentException">
        /// <paramref name="key" /> is an empty string.
        /// <para>or</para>
        /// <paramref name="acquireTimeout" /> is less than <see cref="TimeSpan.Zero" />.
        /// <para>or</para>
        /// <paramref name="holdTimeout" /> is less than <see cref="TimeSpan.Zero" />.
        /// </exception>
        internal LockToken(string key, TimeSpan acquireTimeout, TimeSpan holdTimeout)
            : this(key, new LockTimeoutData(acquireTimeout, holdTimeout))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockToken" /> class with the specified lock timeouts.
        /// </summary>
        /// <param name="key">The key representing the resource to lock.</param>
        /// <param name="timeouts">The <see cref="LockTimeoutData" /> that defines timeout behavior.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is null.
        /// <para>or</para>
        /// <paramref name="timeouts" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="key" /> is an empty string.</exception>
        internal LockToken(string key, LockTimeoutData timeouts)
        {
            if (key == null)
            {
                throw new ArgumentNullException(nameof(key));
            }

            if (timeouts == null)
            {
                throw new ArgumentNullException(nameof(timeouts));
            }

            if (string.IsNullOrEmpty(key))
            {
                throw new ArgumentException("Key cannot be an empty string.", nameof(key));
            }

            Key = key;
            AcquireTimeout = timeouts.AcquireTimeout;
            HoldTimeout = timeouts.HoldTimeout;
        }
    }
}
