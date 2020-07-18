using System;
using System.Collections.Concurrent;
using System.Threading;

namespace HP.ScalableTest.Utility
{
    /// <summary>
    /// An extension to <see cref="CountdownEvent" /> that supports resetting the timeout
    /// while waiting for the event to be signaled.  This class also supports a unique
    /// identifier to prevent the same client from signaling twice.
    /// </summary>
    public sealed class ResettableCountdownEvent : IDisposable
    {
        private readonly CountdownEvent _countdownEvent;
        private readonly ConcurrentDictionary<string, bool> _signals = new ConcurrentDictionary<string, bool>();
        private CancellationTokenSource _cancellationTokenSource = new CancellationTokenSource();

        /// <summary>
        /// Initializes a new instance of the <see cref="ResettableCountdownEvent" /> class.
        /// </summary>
        /// <param name="initialCount">The number of signals initially required to set this event.</param>
        public ResettableCountdownEvent(int initialCount)
        {
            _countdownEvent = new CountdownEvent(initialCount);
        }

        /// <summary>
        /// Gets the number of signals initially required to set the event.
        /// </summary>
        public int InitialCount => _countdownEvent.InitialCount;

        /// <summary>
        /// Gets the number of remaining signals required to set the event.
        /// </summary>
        public int CurrentCount => _countdownEvent.CurrentCount;

        /// <summary>
        /// Gets a value indicating whether this object's current count has reached zero.
        /// </summary>
        public bool IsSet => _countdownEvent.IsSet;

        /// <summary>
        /// Signals this instance to decrement the value of <see cref="CurrentCount" />.
        /// </summary>
        public void Signal()
        {
            _countdownEvent.Signal();
        }

        /// <summary>
        /// Signals this instance to decrement the value of <see cref="CurrentCount" />
        /// if the specified signal ID has not been used before.
        /// </summary>
        /// <param name="signalId">The identifier that ties this signal to a particular client.</param>
        public void Signal(string signalId)
        {
            if (_signals.TryAdd(signalId, true))
            {
                Signal();
            }
        }

        /// <summary>
        /// Blocks the current thread until the <see cref="ResettableCountdownEvent" /> is set.
        /// </summary>
        /// <param name="timeout">The amount of time to wait for the event.</param>
        /// <returns><c>true</c> if this instance is set before the timeout; otherwise, <c>false</c>.</returns>
        public bool Wait(TimeSpan timeout)
        {
            while (true)
            {
                try
                {
                    return _countdownEvent.Wait(timeout, _cancellationTokenSource.Token);
                }
                catch (OperationCanceledException)
                {
                    // Avoid ObjectDisposedException by setting the new token source before disposing the old one.
                    CancellationTokenSource oldSource = _cancellationTokenSource;
                    _cancellationTokenSource = new CancellationTokenSource();
                    oldSource.Dispose();
                }
            }
        }

        /// <summary>
        /// Resets the timeout for all consumers currently in the <see cref="Wait(TimeSpan)" /> method.
        /// </summary>
        public void ResetTimeout()
        {
            _cancellationTokenSource.Cancel();
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _countdownEvent.Dispose();
            _cancellationTokenSource.Dispose();
        }

        #endregion
    }
}
