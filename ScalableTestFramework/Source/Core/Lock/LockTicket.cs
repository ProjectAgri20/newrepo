using System;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Represents a lock that has been granted.
    /// </summary>
    public sealed class LockTicket
    {
        /// <summary>
        /// Gets the unique identifier for the lock request that was granted.
        /// </summary>
        public Guid RequestId { get; }

        /// <summary>
        /// Gets the unique identifier for the resource this lock pertains to.
        /// </summary>
        public string ResourceId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockTicket" /> class.
        /// </summary>
        /// <param name="requestId">The request identifier.</param>
        /// <param name="resourceId">The resource identifier.</param>
        /// <exception cref="ArgumentNullException"><paramref name="resourceId" /> is null.</exception>
        public LockTicket(Guid requestId, string resourceId)
        {
            RequestId = requestId;
            ResourceId = resourceId ?? throw new ArgumentNullException(nameof(resourceId));
        }
    }
}
