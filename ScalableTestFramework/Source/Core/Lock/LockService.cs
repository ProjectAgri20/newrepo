using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.ServiceModel;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.Lock
{
    /// <summary>
    /// The server-side portion of a distributed <see cref="ILockManager" /> implementation.
    /// </summary>
    /// <remarks>
    /// This class acts as a centralized manager for locks in a distributed system.
    /// It is intended to be hosted in a WCF service, with a single instance being used
    /// for all components in the distributed system.  Clients request locks using a dual-channel WCF
    /// client, which includes a callback for notification when the lock is granted.
    /// 
    /// In the client/server model, the <see cref="LockService" /> is the primary server-side component.
    /// </remarks>
    [ServiceBehavior(InstanceContextMode = InstanceContextMode.Single, ConcurrencyMode = ConcurrencyMode.Multiple)]
    public sealed class LockService : LockRequestManager, ILockService, IDisposable
    {
        private readonly Timer _activeLockCheckTimer;
        private readonly TimeSpan _lockCheckFrequency = TimeSpan.FromSeconds(30);
        private readonly ConcurrentDictionary<Guid, Uri> _callbacks = new ConcurrentDictionary<Guid, Uri>();
        private readonly ConcurrentDictionary<Guid, string> _pollingClients = new ConcurrentDictionary<Guid, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="LockService" /> class.
        /// </summary>
        public LockService()
        {
            _activeLockCheckTimer = new Timer(CheckActiveLocks, null, _lockCheckFrequency, _lockCheckFrequency);
        }

        /// <summary>
        /// Enqueues a lock request for the specified resource.
        /// </summary>
        /// <param name="requestId">The unique identifier for the request.</param>
        /// <param name="resourceId">The resource identifier.</param>
        /// <param name="requestName">The name to use for identifying the request.</param>
        /// <param name="maxConcurrent">The maximum number of consumers that can use the specified resource concurrently.</param>
        public void RequestLock(Guid requestId, string resourceId, string requestName, int maxConcurrent)
        {
            SetConcurrency(resourceId, maxConcurrent);

            // Retrieve the callback URI from the WCF context and save for later.
            Uri callbackUri = OperationContext.Current.IncomingMessageHeaders.ReplyTo.Uri;
            if (_callbacks.TryAdd(requestId, callbackUri))
            {
                LogTrace($"Lock request from {requestName}.  Callback URI: {callbackUri}");

                // Use the base method to request the lock.
                // Thread the request so that the client won't time out waiting for this method to return.
                Task.Factory.StartNew(() =>
                {
                    RequestLock(requestId, resourceId, requestName);
                });
            }
            else
            {
                LogError($"Request from {requestName} has a duplicate request ID.");
            }
        }

        /// <summary>
        /// Enqueues a lock request for any of the specified resources.
        /// </summary>
        /// <param name="requestId">The unique identifier for the request.</param>
        /// <param name="resourceIds">The list of resource identifiers.</param>
        /// <param name="requestName">The name to use for identifying the request.</param>
        /// <param name="maxConcurrent">The maximum number of consumers that can use the specified resource concurrently.</param>
        /// <exception cref="ArgumentNullException"><paramref name="resourceIds" /> is null.</exception>
        public void RequestLock(Guid requestId, IEnumerable<string> resourceIds, string requestName, int maxConcurrent)
        {
            if (resourceIds == null)
            {
                throw new ArgumentNullException(nameof(resourceIds));
            }

            foreach (string resourceId in resourceIds)
            {
                SetConcurrency(resourceId, maxConcurrent);
            }

            // Retrieve the callback URI from the WCF context and save for later.
            Uri callbackUri = OperationContext.Current.IncomingMessageHeaders.ReplyTo.Uri;
            if (_callbacks.TryAdd(requestId, callbackUri))
            {
                LogTrace($"Received lock request {requestId} from {requestName}.  Callback URI: {callbackUri}");

                // Use the base method to request the lock.
                // Thread the request so that the client won't time out waiting for this method to return.
                Task.Factory.StartNew(() =>
                {
                    RequestLock(requestId, resourceIds, requestName);
                });
            }
            else
            {
                LogError($"Request from {requestName} has a duplicate request ID.");
            }
        }

        /// <summary>
        /// Releases the lock with the specified request ID.
        /// </summary>
        /// <param name="requestId">The lock request identifier.</param>
        public new void ReleaseLock(Guid requestId)
        {
            LogTrace($"Request {requestId} released.");

            // Remove the lock from the list of clients being polled.
            _pollingClients.TryRemove(requestId, out string found);

            // Remove the callback URI - it is no longer needed.
            _callbacks.TryRemove(requestId, out Uri callback);

            // Thread the release request so that the caller isn't blocked waiting for it to complete.
            Task.Factory.StartNew(() =>
            {
                base.ReleaseLock(requestId);
            });
        }

        /// <summary>
        /// Cancels the lock request with the specified request ID.
        /// </summary>
        /// <param name="requestId">The lock request identifier.</param>
        public new void CancelRequest(Guid requestId)
        {
            LogTrace($"Request {requestId} canceled.");

            // Remove the lock from the list of clients being polled.
            // (Generally it won't be found, but it is possible with a race condition.)
            _pollingClients.TryRemove(requestId, out string found);

            // Remove the callback URI - it is no longer needed.
            _callbacks.TryRemove(requestId, out Uri callback);

            // Thread the cancellation request so that the caller isn't blocked waiting for it to complete.
            Task.Factory.StartNew(() =>
            {
                base.CancelRequest(requestId);
            });
        }

        /// <summary>
        /// When overridden in a derived class, defines the action to take when a lock request has been granted.
        /// </summary>
        /// <param name="requestId">The unique identifier for the request.</param>
        /// <param name="resourceId">The identifier of the resource for which a lock has been granted.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Must ensure lock is released if it cannot be granted for any reason.")]
        protected override void GrantLock(Guid requestId, string resourceId)
        {
            LogTrace($"Request {requestId} given lock for {resourceId}. Attempting grant callback...");

            // Check to make sure the callback is there - a race condition is possible where
            // the client cancels the request just after the resource grants it, but before it is notified.
            if (_callbacks.TryGetValue(requestId, out Uri callbackUri))
            {
                using (var client = new WcfClient<ILockCallback>(MessageTransferType.Http, callbackUri))
                {
                    try
                    {
                        // Grant the lock via the callback channel, and add to the list of clients
                        // that should be polled to make sure they are still alive.
                        client.Channel.GrantLock(resourceId);
                        _pollingClients.TryAdd(requestId, resourceId);
                        LogTrace($"Granting lock for resource '{resourceId}' success.");
                    }
                    catch (Exception ex)
                    {
                        // Client could not be contacted - release the lock so it can be given to somebody else.
                        LogError($"Granting lock for resource '{resourceId}' failed.  Callback: {callbackUri}", ex);
                        ReleaseLock(requestId);
                    }
                }
            }
            else
            {
                LogWarn($"Callback for {requestId} was not found.");
                ReleaseLock(requestId);
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes", Justification = "Must ensure lock is released if it cannot be granted for any reason.")]
        private void CheckActiveLocks(object state)
        {
            LogTrace($"Checking active locks...");
            _activeLockCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);

            // Loop through all the clients whose lock requests have been granted.
            foreach (Guid requestId in _pollingClients.Keys)
            {
                // Race condition possible if the lock was released after the foreach was started.
                // Only proceed if we can still find the callback URI.
                if (_callbacks.TryGetValue(requestId, out Uri callbackUri))
                {
                    LogTrace($"Attempting to ping callback {callbackUri}.");

                    bool pingSuccessful = false;

                    using (var client = new WcfClient<ILockCallback>(MessageTransferType.Http, callbackUri))
                    {
                        try
                        {
                            void pingAction() => pingSuccessful = client.Channel.Ping();
                            Retry.WhileThrowing<CommunicationException>(pingAction, 3, TimeSpan.FromSeconds(2));
                        }
                        catch (Exception ex)
                        {
                            LogError($"Attempt to ping callback {callbackUri} failed.", ex);
                            pingSuccessful = false;
                        }
                    }

                    // If we couldn't ping the client, assume the process has died (or something).
                    // The lock should be reclaimed and given to somebody else.
                    if (!pingSuccessful)
                    {
                        // Again, race condition possible if the lock was released.
                        // Only release the lock if it is still one of the active clients.
                        if (_pollingClients.TryGetValue(requestId, out string resourceId))
                        {
                            LogWarn($"Client with lock on resource '{resourceId}' is not active. Releasing lock.");
                            ReleaseLock(requestId);
                        }
                    }
                    else
                    {
                        LogTrace($"Lock is still active.");
                    }
                }
                else
                {
                    _pollingClients.TryRemove(requestId, out string removed);
                }
            }

            try
            {
                _activeLockCheckTimer.Change(_lockCheckFrequency, _lockCheckFrequency);
            }
            catch (ObjectDisposedException)
            {
                // Race condition - ignore.
            }
            LogTrace($"Done checking active locks.");
        }

        #region Explicit ILockService Members

        bool ILockService.Ping()
        {
            return true;
        }

        #endregion

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _activeLockCheckTimer.Change(Timeout.Infinite, Timeout.Infinite);
            _activeLockCheckTimer.Dispose();
        }

        #endregion
    }
}
