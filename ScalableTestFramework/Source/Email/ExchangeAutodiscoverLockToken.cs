using System;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Email
{
    /// <summary>
    /// A <see cref="LockToken" /> that represents a machine-level lock on an Exchange autodiscover operation.
    /// </summary>
    /// <remarks>
    /// Exchange has problems if multiple users on the same machine are trying to use autodiscover concurrently.
    /// This lock token should be employed whenever autodiscover is used on a machine with multiple users.
    /// </remarks>
    public sealed class ExchangeAutodiscoverLockToken : LocalLockToken
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeAutodiscoverLockToken" /> class.
        /// </summary>
        /// <param name="acquireTimeout">The amount of time to wait to acquire the lock.</param>
        /// <param name="holdTimeout">The amount of time to hold the lock.</param>
        public ExchangeAutodiscoverLockToken(TimeSpan acquireTimeout, TimeSpan holdTimeout)
            : base("ExchangeAutodiscover", acquireTimeout, holdTimeout)
        {
        }
    }
}
