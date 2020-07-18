using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// Base class for managing lock requests and resources.
    /// </summary>
    /// <remarks>
    /// The purpose of this class is to keep track of lockable resources and the requests that have been granted.
    /// Requests are submitted through RequestLock and queued up for the requested resource(s).
    /// When the request is granted, the submitter is notified via GrantLock and the request/resource pair
    /// is added to the list of active locks.  When the submitter calls ReleaseLock, the request/resource pair
    /// is used to notify the correct <see cref="LockResource" /> that the lock has been released.
    /// 
    /// In the client/server model, this is used as the base class for the server-side implementation.
    /// </remarks>
    public abstract class LockRequestManager
    {
        // Three private fields here:
        // - _lockResources tracks all of the LockResource objects.  New ones are created as lock requests come in.
        // - _allRequests tracks all requests currently in the system.  Requests are added when made and removed when released or canceled.
        // - _activeLocks tracks lock requests that have been granted, as well as the granting resource.  Used to forward Release requests to the right resource.
        private readonly ConcurrentDictionary<string, LockResource> _lockResources = new ConcurrentDictionary<string, LockResource>(StringComparer.OrdinalIgnoreCase);
        private readonly ConcurrentDictionary<Guid, LockRequest> _allRequests = new ConcurrentDictionary<Guid, LockRequest>();
        private readonly ConcurrentDictionary<Guid, LockResource> _activeLocks = new ConcurrentDictionary<Guid, LockResource>();

        /// <summary>
        /// Enqueues a lock request for the specified resource.
        /// </summary>
        /// <param name="requestId">The unique identifier for the request.</param>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="requestName">The name to use for identifying the request.</param>
        protected void RequestLock(Guid requestId, string resourceId, string requestName)
        {
            // Create a new lock request and add it to the list of all requests.
            LockRequest request = new LockRequest(requestId, requestName);
            _allRequests.TryAdd(request.RequestId, request);

            // Find (or create) the resource and enqueue the request.
            LockResource resource = GetResource(resourceId);
            resource.AddRequest(request);
        }

        /// <summary>
        /// Enqueues a lock request for the specified resource.
        /// </summary>
        /// <param name="requestId">The unique identifier for the request.</param>
        /// <param name="resourceIds">The list of resource identifiers.</param>
        /// <param name="requestName">The name to use for identifying the request.</param>
        /// <exception cref="ArgumentNullException"><paramref name="resourceIds" /> is null.</exception>
        protected void RequestLock(Guid requestId, IEnumerable<string> resourceIds, string requestName)
        {
            if (resourceIds == null)
            {
                throw new ArgumentNullException(nameof(resourceIds));
            }

            // Create a new lock request and add it to the list of all requests.
            LockRequest request = new LockRequest(requestId, requestName);
            _allRequests.TryAdd(request.RequestId, request);

            // Find (or create) each resource and enqueue the request.
            foreach (string resourceId in resourceIds)
            {
                LockResource resource = GetResource(resourceId);
                resource.AddRequest(request);
            }
        }

        /// <summary>
        /// Releases the lock with the specified request ID.
        /// </summary>
        /// <param name="requestId">The lock request identifier.</param>
        protected void ReleaseLock(Guid requestId)
        {
            // Retrieve the lock request and the corresponding granted resource lock.
            // This also has the effect of removing them from the lists of tracked requests/locks.
            bool requestFound = _allRequests.TryRemove(requestId, out LockRequest request);
            bool activeLockFound = _activeLocks.TryRemove(requestId, out LockResource resource);

            if (requestFound && activeLockFound)
            {
                resource.ReleaseLock(request);
            }
            else
            {
                LogError($"Invalid release attempt.  Request found: {requestFound}.  Active lock found: {activeLockFound}");
            }
        }

        /// <summary>
        /// Cancels the lock request with the specified request ID.
        /// </summary>
        /// <param name="requestId">The lock request identifier.</param>
        protected void CancelRequest(Guid requestId)
        {
            LockResource resource = null;

            // Retrieve and remove the lock request from the list of all requests.
            if (_allRequests.TryRemove(requestId, out LockRequest request))
            {
                lock (request)
                {
                    // There could be a race condition where the client cancels the request just after
                    // the server grants the lock.  Check to see if the lock has already been granted.
                    // If it has, the lock will be cleaned up via a regular release below.
                    // (The actual release must happen outside of the locked region or a deadlock will occur.)
                    if (request.State == LockRequestState.Granted)
                    {
                        if (_activeLocks.TryRemove(requestId, out resource))
                        {
                            LogWarn($"Request '{request.Name}' released lock for resource '{resource.ResourceId}' via cancellation.");
                        }
                    }
                    else
                    {
                        // Once the request is flagged as "Canceled" it will be ignored by any resources that are tracking it.
                        // Eventually it will fall out of each queue and be garbage collected.
                        request.State = LockRequestState.Canceled;
                        LogTrace($"Request '{request.Name}' was canceled.");
                    }
                }
            }

            // Resource will be non-null only if the lock needs to be released.
            if (resource != null)
            {
                resource.ReleaseLock(request);
            }
        }

        /// <summary>
        /// Sets the maximum number of consumers that can use the specified resource concurrently.
        /// </summary>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="maxConcurrent">The maximum number of consumers that can use the specified resource concurrently.</param>
        /// <exception cref="ArgumentNullException"><paramref name="resourceId" /> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException"><paramref name="maxConcurrent" /> is less than or equal to zero.</exception>
        protected void SetConcurrency(string resourceId, int maxConcurrent)
        {
            LockResource resource = GetResource(resourceId);
            resource.SetConcurrency(maxConcurrent);
        }

        private LockResource GetResource(string resourceId)
        {
            return _lockResources.GetOrAdd(resourceId, CreateResource);
        }

        private LockResource CreateResource(string resourceId)
        {
            LockResource resource = new LockResource(resourceId);
            resource.LockGranted += OnLockGranted;
            return resource;
        }

        private void OnLockGranted(object sender, LockGrantedEventArgs e)
        {
            _activeLocks.TryAdd(e.Request.RequestId, e.Resource);
            GrantLock(e.Request.RequestId, e.Resource.ResourceId);
        }

        /// <summary>
        /// When overridden in a derived class, defines the action to take when a lock request has been granted.
        /// </summary>
        /// <param name="requestId">The unique identifier for the request.</param>
        /// <param name="resourceId">The identifier of the resource for which a lock has been granted.</param>
        protected abstract void GrantLock(Guid requestId, string resourceId);
    }
}
