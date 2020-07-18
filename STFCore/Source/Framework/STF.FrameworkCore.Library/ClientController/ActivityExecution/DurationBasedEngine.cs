using System;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    [ObjectFactory(ExecutionMode.Duration)]
    internal class DurationBasedEngine : EngineBase
    {
        private ActivityDelay _delay = null;
        private readonly TimeSpan _expiration = TimeSpan.Zero;

        private readonly EngineFlowControlMonitor _monitor = new EngineFlowControlMonitor();

        public DurationBasedEngine(OfficeWorkerDetail worker)
            : base(worker)
        {
            RandomizeActivities = worker.RandomizeActivities;
            _delay = new ActivityDelay(worker);
            _expiration = TimeSpan.FromMinutes(worker.DurationTime);
            TraceFactory.Logger.Debug("Creating...");
        }

        public DurationBasedEngine(OfficeWorkerDetail worker, ActivityQueueBase activity, ActivityPacingInfo info, int expirationMinutes)
            : base(activity, info)
        {
            //we don't have a start/stop time
            _delay = new ActivityDelay(worker);
            _expiration = TimeSpan.FromMinutes(expirationMinutes > 0 ? expirationMinutes : 0);

        }

        public DurationBasedEngine(LoadTesterMetadataDetail metadataDetail)
            : base(VirtualResourceType.LoadTester, metadataDetail, null)
        {
            var plan = metadataDetail.Plan as LoadTesterExecutionPlan;
            _delay = new ActivityDelay(plan);
            _expiration = TimeSpan.FromMinutes(plan.DurationTime > 0 ? plan.DurationTime : 0);
        }

        public DurationBasedEngine(OfficeWorkerDetail detail, int expirationMinutes)
            : this(detail)
        {
            _expiration = TimeSpan.FromMinutes(expirationMinutes > 0 ? expirationMinutes : 0);
        }

        protected EngineFlowControlMonitor FlowMonitor
        {
            get { return _monitor; }
        }

        public override void Run()
        {
            Run(_expiration); // Allow exceptions to bubble up to the controller
        }

        /// <summary>
        /// Process activities for the set duration.
        /// </summary>
        /// <param name="duration">The duration.</param>
        /// <exception cref="WorkerHaltedException">Worker has been signaled to halt</exception>
        public void Run(TimeSpan duration)
        {
            PacingInfo.OverallStartTime = DateTime.Now;
            PacingInfo.DurationLimit = duration;
            PacingInfo.IterationLimit = -1;

            TraceFactory.Logger.Debug("Will run for {0} mins".FormatWith(duration.TotalMinutes));

            // Start the monitor which will add up all pause time that may occur during execution
            _monitor.Start();

            int loopCount = 0;
            var startTime = DateTime.Now;
            TimeSpan totalRunTime = TimeSpan.Zero;
            TimeSpan totalDuration = TimeSpan.Zero;

            // Get the initial activity from the queue
            var currentActivity = GetNextActivity(ref loopCount);
            do
            {
                if (!ExecutionHalted)
                {
                    // This will adjust the expiration time based on how long it sits in a paused state.
                    ApplicationFlowControl.Instance.CheckWait();
                    DateTime activityStartTime = DateTime.Now;

                    // Run the next activity 
                    currentActivity.Execute();

                    PacingInfo.MarkActivityRunEnd(currentActivity, activityStartTime);

                    // Calculate the remaining time in this run by subtracting the total time the engine has
                    // been running from the duration plus the pause time.  All pause time is used to shift
                    // the total duration as pause time should not impact the overall duration.
                    totalRunTime = DateTime.Now - startTime;
                    totalDuration = duration + _monitor.PauseTime;
                    var remainingTime = totalDuration - totalRunTime;

                    //TraceFactory.Logger.Debug("RUN: {0} DUR: {1} REM: {2}".FormatWith(totalRunTime.TotalSeconds, totalDuration.TotalSeconds, remainingTime.TotalSeconds));

                    // Go get the next activity
                    var nextActivity = GetNextActivity(ref loopCount);

                    // Apply the appropriate Activity delay.  It may be a Worker level delay or
                    // it may be at the Activity level, and if at the Activity level, it may be
                    // a delay every time the Activity executes, or it may be a delay only 
                    // after an Activity with an ExecutionCount > 1 completes.
                    _delay.Apply(currentActivity, nextActivity);

                    // Set the current Activity to the next Activity and loop to the top.
                    currentActivity = nextActivity;

                    var endTime = startTime.Add(duration + _monitor.PauseTime).ToLongTimeString();
                    TraceFactory.Logger.Debug("Run Complete. Completed: {0}.  Run to {1}".FormatWith(loopCount++, endTime));
                }
                else
                {
                    throw new WorkerHaltedException("Worker has been signaled to halt");
                }

                // Run until the total run time is less than the defined duration plus any pause time.

                totalRunTime = DateTime.Now - startTime;
                totalDuration = duration + _monitor.PauseTime;

                TraceFactory.Logger.Debug("RUN: {0} DUR: {1}".FormatWith(totalRunTime.TotalSeconds, totalDuration.TotalSeconds));

            } while (totalRunTime < totalDuration);

            TraceFactory.Logger.Debug("Finished");
        }
    }
}
