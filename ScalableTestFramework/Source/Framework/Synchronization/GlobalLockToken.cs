using System;

namespace HP.ScalableTest.Framework.Synchronization
{
    /// <summary>
    /// A <see cref="LockToken" /> that represents a system-wide lock on a resource.
    /// </summary>
    public class GlobalLockToken : LockToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalLockToken" /> class with the specified lock timeouts.
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
        public GlobalLockToken(string key, TimeSpan acquireTimeout, TimeSpan holdTimeout)
            : base(key, acquireTimeout, holdTimeout)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalLockToken" /> class with the specified lock timeouts.
        /// </summary>
        /// <param name="key">The key representing the resource to lock.</param>
        /// <param name="timeouts">The <see cref="LockTimeoutData" /> that defines timeout behavior.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="key" /> is null.
        /// <para>or</para>
        /// <paramref name="timeouts" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="key" /> is an empty string.</exception>
        public GlobalLockToken(string key, LockTimeoutData timeouts)
            : base(key, timeouts)
        {
        }
    }
}
