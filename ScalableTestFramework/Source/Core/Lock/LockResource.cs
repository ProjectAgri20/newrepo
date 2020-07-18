using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using System.Threading;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Represents a resource that can be locked for exclusive use by 1 (or more) consumers.
    /// </summary>
    /// <remarks>
    /// This class handles queueing up lock requests for a particular resource and granting them as they become available.
    /// It tracks both the requests that are pending and the requests that currently have active locks.
    /// 
    /// Because a single <see cref="LockRequest" /> can be enqueued for multiple resources,
    /// each request should be locked before querying or changing its state.
    /// 
    /// In the client/server model, the <see cref="LockResource" /> classes exist on the server.
    /// </remarks>
    [DebuggerDisplay("{ResourceId,nq}")]
    internal sealed class LockResource
    {
        private int _maxConcurrent = 1;
        private readonly LockResourceLogger _logger;
        private readonly ConcurrentQueue<LockRequest> _pendingRequests = new ConcurrentQueue<LockRequest>();
        private readonly ConcurrentDictionary<Guid, LockRequest> _activeLocks = new ConcurrentDictionary<Guid, LockRequest>();

        /// <summary>
        /// Gets the identifier for this resource.
        /// </summary>
        public string ResourceId { get; }

        /// <summary>
        /// Event raised when a <see cref="LockRequest" /> is granted.
        /// </summary>
        public event EventHandler<LockGrantedEventArgs> LockGranted;

        /// <summary>
        /// Initializes a new instance of the <see cref="LockResource" /> class.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <exception cref="ArgumentNullException"><paramref name="resourceId" /> is null.</exception>
        public LockResource(string resourceId)
        {
            ResourceId = resourceId ?? throw new ArgumentNullException(nameof(resourceId));
            _logger = new LockResourceLogger(resourceId);
        }

        /// <summary>
        /// Sets the maximum number of consumers that can use this resource concurrently.
        /// </summary>
        /// <param name="maxConcurrent">The maximum number of consumers that can use this resource concurrently.</param>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxConcurrent" /> is less than or equal to zero.</exception>
        public void SetConcurrency(int maxConcurrent)
        {
            if (maxConcurrent <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(maxConcurrent), "Maximum concurrency must be greater than zero.");
            }

            // If multiple callers try to change the concurrency, only log once.
            int original = Interlocked.Exchange(ref _maxConcurrent, maxConcurrent);
            if (maxConcurrent != original)
            {
                _logger.LogDebug($"Concurrency for resource '{ResourceId}' set to {maxConcurrent}.");
            }
        }

        /// <summary>
        /// Adds the specified <see cref="LockRequest" /> to the queue of consumers waiting for a lock.
        /// </summary>
        /// <param name="request">The lock request.</param>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public void AddRequest(LockRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            lock (request)
            {
                // Don't add the request if it has already been granted, canceled, etc.
                if (request.State == LockRequestState.New || request.State == LockRequestState.Pending)
                {
                    _pendingRequests.Enqueue(request);
                    request.State = LockRequestState.Pending;
                    _logger.LogTrace($"Request '{request.Name}' added to queue for resource '{ResourceId}'.");
                }
            }

            GrantLocks();
        }

        /// <summary>
        /// Releases the lock held by the specified <see cref="LockRequest" />.
        /// </summary>
        /// <param name="request">The lock request.</param>
        /// <exception cref="ArgumentNullException"><paramref name="request" /> is null.</exception>
        public void ReleaseLock(LockRequest request)
        {
            if (request == null)
            {
                throw new ArgumentNullException(nameof(request));
            }

            bool released = false;
            lock (request)
            {
                // Check the active locks to see if the specified request is one of them.
                released = _activeLocks.TryRemove(request.RequestId, out LockRequest removed);
                if (released)
                {
                    request.State = LockRequestState.Released;
                    _logger.LogTrace($"Request '{request.Name}' released lock for resource '{ResourceId}'.");
                }
                else
                {
                    _logger.LogError($"Request '{request.Name}' attempted to release lock for resource '{ResourceId}', but did not have a lock.");
                }
            }

            if (released)
            {
                GrantLocks();
            }
        }

        private void GrantLocks()
        {
            bool lockGranted = false;
            LockRequest request = null;

            // Lock the list of active locks to be sure no other thread grants a lock request.
            lock (_activeLocks)
            {
                // If there are locks available...
                if (_activeLocks.Count < _maxConcurrent)
                {
                    // Keep pulling from the queue until we find a pending request (or it is empty).
                    while (!lockGranted && _pendingRequests.TryDequeue(out request))
                    {
                        lockGranted = AttemptToGrantLock(request);
                    }
                }
            }

            // If we found a request to grant, do so.
            if (lockGranted)
            {
                LockGranted?.Invoke(this, new LockGrantedEventArgs(request, this));
            }
        }

        private bool AttemptToGrantLock(LockRequest request)
        {
            lock (request)
            {
                if (request.State == LockRequestState.Pending)
                {
                    // Move the lock request from the "pending" queue to the "active" list.
                    _activeLocks.TryAdd(request.RequestId, request);
                    request.State = LockRequestState.Granted;
                    _logger.LogTrace($"Request '{request.Name}' granted lock for resource '{ResourceId}'.");
                    return true;
                }
                else
                {
                    _logger.LogTrace($"Request '{request.Name}' for resource '{ResourceId}' is no longer pending.");
                    return false;
                }
            }
        }
    }
}
