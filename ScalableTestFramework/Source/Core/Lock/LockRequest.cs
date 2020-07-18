using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Represents a request to obtain a lock on a <see cref="LockResource" />.
    /// </summary>
    /// <remarks>
    /// This class is the central object used by the lock service to track requests on resources.
    /// A request is instantiated by a lock manager, and then queued up to obtain a lock on a resource.
    /// 
    /// By design, the lock request does not specify the resource that is being requested.
    /// This is so that one request can be queued up for multiple resources, and then be
    /// granted by the first available resource and discarded by the rest.
    /// 
    /// In the client/server model, the <see cref="LockRequest" /> classes exist on the server.
    /// </remarks>
    [DebuggerDisplay("{Name,nq} [{State}]")]
    internal sealed class LockRequest
    {
        /// <summary>
        /// Gets the unique identifier for this request.
        /// </summary>
        public Guid RequestId { get; }

        /// <summary>
        /// Gets a friendly name for this request.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the current state of this request.
        /// </summary>
        /// <remarks>
        /// It is crucial that this property is accessed and modified in a thread-safe manner.
        /// Do not access or modify this property without a lock on this <see cref="LockRequest" />.
        /// </remarks>
        internal LockRequestState State { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LockRequest" /> class.
        /// </summary>
        /// <param name="requestId">The unique identifier for the request.</param>
        /// <param name="name">The name to use for identifying the request.</param>
        public LockRequest(Guid requestId, string name)
        {
            RequestId = requestId;
            Name = name ?? RequestId.ToString();
            State = LockRequestState.New;
        }
    }
}
