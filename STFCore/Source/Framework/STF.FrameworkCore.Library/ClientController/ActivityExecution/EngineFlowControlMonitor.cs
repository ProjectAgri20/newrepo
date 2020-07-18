using System;
using System.Threading;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    internal class EngineFlowControlMonitor
    {
        private TimeSpan _totalPauseTime = TimeSpan.Zero;
        private DateTime _pauseTimeStamp = DateTime.Now;
        private bool _inPauseRequest = false;
        private bool _isStarted = false;

        public EngineFlowControlMonitor()
        {
        }

        public void Start()
        {
            if (!_isStarted)
            {
                ApplicationFlowControl.Instance.OnPauseRequested += OnPauseRequested;
                ApplicationFlowControl.Instance.OnResumeRequested += OnResumeRequested;
                _isStarted = true;
            }
        }

        public void Stop()
        {
            if (_isStarted)
            {
                ApplicationFlowControl.Instance.OnPauseRequested -= OnPauseRequested;
                ApplicationFlowControl.Instance.OnResumeRequested -= OnResumeRequested;
                _inPauseRequest = false;
                _isStarted = false;
            }
        }

        public TimeSpan PauseTime
        {
            get { return _totalPauseTime; }
        }

        void OnPauseRequested(object sender, EventArgs e)
        {
            if (!_inPauseRequest)
            {
                _pauseTimeStamp = DateTime.Now;
                _inPauseRequest = true;

                TraceFactory.Logger.Debug("Time stamped");
            }
        }

        void OnResumeRequested(object sender, EventArgs e)
        {
            if (_inPauseRequest)
            {
                var pauseTime = DateTime.Now - _pauseTimeStamp;
                _totalPauseTime += pauseTime;
                _inPauseRequest = false;

                TraceFactory.Logger.Debug("Paused {0} secs, total pause now {1} secs"
                    .FormatWith(pauseTime.TotalSeconds, _totalPauseTime.TotalSeconds));
            }
        }

        public void Pause(Func<bool> continueAction, TimeSpan duration, TimeSpan loopWaitTime)
        {
            TraceFactory.Logger.Debug("Will spin for {0} mins".FormatWith(duration.TotalMinutes + PauseTime.TotalMinutes));
            TraceFactory.Logger.Debug("Duration {0} mins".FormatWith(duration.TotalMinutes));
            TraceFactory.Logger.Debug("Pause {0} mins".FormatWith(PauseTime.TotalMinutes));
            // This spinning wait loop will take into account any additional time that occurs
            // because of a Pause.  It will spin until the amount of time is equal to the
            // specified duration plus any accumulated pause time.
            var startTime = DateTime.Now;
            TraceFactory.Logger.Debug("Start Time: {0}, End Time: {1} ".FormatWith(startTime, (startTime + duration + PauseTime)));

            do
            {
                if (continueAction())
                {
                    ApplicationFlowControl.Instance.CheckWait();
                    Thread.Sleep(loopWaitTime);
                }
                else
                {
                    throw new WorkerHaltedException("Worker has been signaled to halt");
                }

            } while ((DateTime.Now - startTime) < (duration + PauseTime));
            _totalPauseTime = TimeSpan.Zero;
        }
    }
}
