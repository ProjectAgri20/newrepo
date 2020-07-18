using System;
using System.Threading;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// This class provides a simple interface over <see cref="ManualResetEvent" /> to provide methods
    /// to force a pause in VirtualResource execution (and activity plugin execution).
    /// </summary>
    public sealed class ApplicationFlowControl : IDisposable
    {
        private readonly AutoResetEvent _syncEvent = new AutoResetEvent(false);
        private readonly ManualResetEvent _pauseEvent = new ManualResetEvent(true);
        private readonly object _lock = new object();
        private int _clientsToPause = 0;
        private string _syncEventName = null;

        /// <summary>
        /// Gets the instance.
        /// </summary>
        public static ApplicationFlowControl Instance { get; } = new ApplicationFlowControl();

        /// <summary>
        /// Occurs when the execution is paused.
        /// </summary>
        public event EventHandler OnExecutionPaused;

        /// <summary>
        /// Occurs when this instance is requested to pause.
        /// </summary>
        public event EventHandler OnPauseRequested;

        /// <summary>
        /// Occurs when this instance is requested to resume.
        /// </summary>
        public event EventHandler OnResumeRequested;

        /// <summary>
        /// Resets (pauses) the <see cref="ManualResetEvent" />.
        /// </summary>
        public void Pause()
        {
            Pause(1);
        }

        /// <summary>
        /// Resets (pauses) the <see cref="ManualResetEvent" /> with a specified client count.
        /// </summary>
        /// <remarks>
        /// The count determines how many clients must pause before an event is sent
        /// </remarks>
        /// <param name="count">The count.</param>
        public void Pause(int count)
        {
            lock (_lock)
            {
                _clientsToPause = count;
                _pauseEvent.Reset();
                OnPauseRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Sets the <see cref="ManualResetEvent"/>
        /// </summary>
        public void Resume()
        {
            lock (_lock)
            {
                _clientsToPause = 0;
                _pauseEvent.Set();
                OnResumeRequested?.Invoke(this, EventArgs.Empty);
            }
        }

        /// <summary>
        /// Signals a synchronization event with the specified name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public void SignalSync(string eventName)
        {
            lock (_lock)
            {
                if (_syncEventName?.EqualsIgnoreCase(eventName) == true)
                {
                    _syncEvent.Set();
                }
            }
        }

        /// <summary>
        /// Waits for a synchronization event with the specified name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public void WaitForSync(string eventName)
        {
            lock (_lock)
            {
                _syncEventName = eventName;
            }

            _syncEvent.WaitOne();

            lock (_lock)
            {
                _syncEventName = null;
            }
        }

        /// <summary>
        /// Blocks if this instance is set to pause.
        /// </summary>
        /// <param name="pauseAction">The pause action is executed if this instance is set to pause.</param>
        /// <param name="resumeAction">The resume action is executed when this instance resumes.</param>
        /// <returns></returns>
        public void CheckWait(Action pauseAction, Action resumeAction)
        {
            bool wasPaused = false;
            lock (_lock)
            {
                wasPaused = CheckPause(pauseAction);
            }

            _pauseEvent.WaitOne();

            if (wasPaused)
            {
                resumeAction();
            }
        }

        /// <summary>
        /// Blocks if this instance is set to pause.
        /// </summary>
        /// <returns>Value indicating that the client was paused</returns>
        public void CheckWait()
        {
            lock (_lock)
            {
                CheckPause(null);
            }

            _pauseEvent.WaitOne();
        }

        /// <summary>
        /// Blocks if this instance is set to pause.
        /// </summary>
        public void PassiveCheckWait()
        {
            lock (_lock)
            {
                CheckPause(null);
            }
        }

        private bool CheckPause(Action action)
        {
            if (!_pauseEvent.WaitOne(0))
            {
                action?.Invoke();

                _clientsToPause--;
                if (_clientsToPause == 0)
                {
                    OnExecutionPaused?.Invoke(this, new EventArgs());
                }

                return true;
            }

            return false;
        }

        /// <summary>
        /// Waits for the specified amount of time, taking into account any pause requests.
        /// If a pause request is made, this method will honor that pause without impacting the total wait time.
        /// </summary>
        /// <param name="timeout">The total amount of time to wait.</param>
        /// <param name="waitInterval">The amount of time to sleep within each iteration of the loop.</param>
        public void Wait(TimeSpan timeout, int waitInterval = 500)
        {
            if (timeout > TimeSpan.Zero)
            {
                // Simple spin loop for the delay that will ignore time spent in a paused state
                TimeSpan loopTime = TimeSpan.Zero;
                var delay = TimeSpan.FromMilliseconds(waitInterval);
                do
                {
                    DateTime start = DateTime.Now;

                    CheckWait();
                    Delay.Wait(delay);

                    TimeSpan actualDelay = DateTime.Now.Subtract(start);
                    loopTime = loopTime.Add(actualDelay);

                } while (loopTime < timeout);
            }
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _pauseEvent.Dispose();
        }

        #endregion
    }
}