using System;
using System.ServiceModel;
using System.Threading;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// The client-side implementation of the <see cref="ILockCallback" /> interface.
    /// </summary>
    /// <remarks>
    /// This class is used by the <see cref="LockServiceClient" /> as the implementation of the
    /// callback interface; the <see cref="ILockService" /> communicates back on this channel.
    /// 
    /// In the client/server model, this is instantiated on the client side.
    /// </remarks>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Single)]
    public sealed class LockCallback : ILockCallback
    {
        private readonly Guid _requestId;
        private readonly AutoResetEvent _waitEvent;

        /// <summary>
        /// Gets the lock ticket created when the request is granted.
        /// </summary>
        public LockTicket LockTicket { get; private set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="LockCallback" /> is active.
        /// </summary>
        public bool Active { get; set; } = true;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockCallback" /> class.
        /// </summary>
        /// <param name="requestId">The unique identifier for the request.</param>
        /// <param name="waitEvent">The <see cref="AutoResetEvent" /> to use to signal the owning <see cref="LockServiceClient" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="waitEvent" /> is null.</exception>
        public LockCallback(Guid requestId, AutoResetEvent waitEvent)
        {
            _requestId = requestId;
            _waitEvent = waitEvent ?? throw new ArgumentNullException(nameof(waitEvent));
        }

        /// <summary>
        /// Signals the client that the lock request has been granted.
        /// </summary>
        /// <param name="resourceId">The identifier for the locked resource.</param>
        public void GrantLock(string resourceId)
        {
            LogTrace($"Received lock grant signal for {resourceId}.");
            try
            {
                LockTicket = new LockTicket(_requestId, resourceId);
                _waitEvent.Set();
            }
            catch (ObjectDisposedException)
            {
                // Race condition - ignore.
            }
        }

        /// <summary>
        /// Pings this instance.
        /// </summary>
        /// <returns><c>true</c> if the ping was successful.</returns>
        public bool Ping()
        {
            LogTrace($"Ping requested for request {_requestId}.  Callback is {(Active ? "Active" : "Not Active")}");
            return Active;
        }
    }
}
