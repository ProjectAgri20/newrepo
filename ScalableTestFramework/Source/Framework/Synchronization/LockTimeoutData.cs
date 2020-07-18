using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Synchronization
{
    /// <summary>
    /// Defines timeouts used for <see cref="ICriticalSection" /> operations.
    /// </summary>
    [DataContract]
    public sealed class LockTimeoutData
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TimeSpan _acquireTimeout;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly TimeSpan _holdTimeout;

        /// <summary>
        /// The amount of time to wait to acquire a lock.  If this time is exceeded without
        /// acquiring the lock, an <see cref="AcquireLockTimeoutException" /> will be thrown.
        /// </summary>
        public TimeSpan AcquireTimeout => _acquireTimeout;

        /// <summary>
        /// The amount of time to hold a lock.  If this time is exceeded during execution,
        /// the running action will be terminated and a <see cref="HoldLockTimeoutException" /> will be thrown.
        /// </summary>
        public TimeSpan HoldTimeout => _holdTimeout;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockTimeoutData" /> class.
        /// </summary>
        /// <param name="acquireTimeout">The amount of time to wait to acquire a lock.</param>
        /// <param name="holdTimeout">The amount of time to hold the lock.</param>
        /// <exception cref="ArgumentException">
        /// <paramref name="acquireTimeout" /> is less than <see cref="TimeSpan.Zero" />.
        /// <para>or</para>
        /// <paramref name="holdTimeout" /> is less than <see cref="TimeSpan.Zero" />.
        /// </exception>
        public LockTimeoutData(TimeSpan acquireTimeout, TimeSpan holdTimeout)
        {
            if (IsInvalidTimeout(acquireTimeout))
            {
                throw new ArgumentException("Timeout cannot be negative.", nameof(acquireTimeout));
            }

            if (IsInvalidTimeout(holdTimeout))
            {
                throw new ArgumentException("Timeout cannot be negative.", nameof(holdTimeout));
            }

            _acquireTimeout = acquireTimeout;
            _holdTimeout = holdTimeout;
        }

        private static bool IsInvalidTimeout(TimeSpan timeout)
        {
            TimeSpan infiniteTimeout = TimeSpan.FromMilliseconds(-1);
            return timeout < TimeSpan.Zero && timeout != infiniteTimeout;
        }
    }
}
