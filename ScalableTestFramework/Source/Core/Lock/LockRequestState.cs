namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Represents the state of a <see cref="LockRequest" />.
    /// </summary>
    internal enum LockRequestState
    {
        /// <summary>
        /// The request has just been created.
        /// </summary>
        New,

        /// <summary>
        /// The request is queued up for a resource.
        /// </summary>
        Pending,

        /// <summary>
        /// The request has been granted and the lock is active.
        /// </summary>
        Granted,

        /// <summary>
        /// The request is complete and the lock has been released.
        /// </summary>
        Released,

        /// <summary>
        /// The request has been canceled and is no longer needed.
        /// </summary>
        Canceled
    }
}
