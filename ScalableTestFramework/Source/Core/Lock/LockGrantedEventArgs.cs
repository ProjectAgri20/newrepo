using System;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Event args for the <see cref="LockResource.LockGranted" /> event.
    /// </summary>
    internal sealed class LockGrantedEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the <see cref="LockRequest" /> that has been granted.
        /// </summary>
        public LockRequest Request { get; }

        /// <summary>
        /// Gets the <see cref="LockResource" /> that has been given to the requester.
        /// </summary>
        public LockResource Resource { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockGrantedEventArgs" /> class.
        /// </summary>
        /// <param name="lockRequest">The <see cref="LockRequest" /> that has been granted.</param>
        /// <param name="resource">The <see cref="LockResource"/> that has been given to the requester.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="lockRequest" /> is null.
        /// <para>or</para>
        /// <paramref name="resource" /> is null.</exception>
        public LockGrantedEventArgs(LockRequest lockRequest, LockResource resource)
        {
            Request = lockRequest ?? throw new ArgumentNullException(nameof(lockRequest));
            Resource = resource ?? throw new ArgumentNullException(nameof(resource));
        }
    }
}
