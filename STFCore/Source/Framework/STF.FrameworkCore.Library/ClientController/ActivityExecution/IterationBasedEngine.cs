using System;
using System.Linq;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    [ObjectFactory(ExecutionMode.Iteration)]
    internal class IterationBasedEngine : EngineBase
    {
        private ActivityDelay _delay = null;
        private readonly int _totalIterations = 1;

        public IterationBasedEngine(OfficeWorkerDetail worker)
            : base(worker)
        {
            _delay = new ActivityDelay(worker);
            _totalIterations = worker.RepeatCount * worker.MetadataDetails.Sum(x => x.Plan.Value);
            RandomizeActivities = worker.RandomizeActivities;

            TraceFactory.Logger.Debug("Creating against {0} items".FormatWith(worker.MetadataDetails.Count()));
        }

        /// <summary>
        /// Process the activities iteratively.
        /// </summary>
        public override void Run()
        {
            PacingInfo.OverallStartTime = DateTime.Now;
            PacingInfo.DurationLimit = new TimeSpan(0);
            PacingInfo.IterationLimit = _totalIterations;

            TraceFactory.Logger.Debug("Total Iterations: {0}".FormatWith(_totalIterations));

            int runCount = 0;

            // Get the initial activity from the queue
            Activity currentActivity = null;
            do
            {
                if (!ExecutionHalted)
                {
                    // Honor any pause request from the client, and stop right here if there is a pause.
                    ApplicationFlowControl.Instance.CheckWait(LogActivityPaused, LogActivityResumed);

                    DateTime activityStartTime = DateTime.Now;

                    // Ensure that we have a valid activity
                    if (currentActivity == null)
                    {
                        currentActivity = GetNextActivity(ref runCount);
                    }

                    // Now execute the current activity
                    currentActivity.Execute();
                    
                    TraceFactory.Logger.Debug("Completed activity {0} of {1}".FormatWith(runCount, _totalIterations));

                    // Go get the next activity
                    var nextActivity = GetNextActivity(ref runCount);

                    // Apply the appropriate Activity delay.  It may be a Worker level delay or
                    // it may be at the Activity level, and if at the Activity level, it may be
                    // a delay every time the Activity executes, or it may be a delay only 
                    // after an Activity with an ExecutionCount > 1 completes.
                    _delay.Apply(currentActivity, nextActivity);

                    // mark end of activity (including any delay)
                    PacingInfo.MarkActivityRunEnd(currentActivity, activityStartTime);

                    // Set the current Activity to the next Activity and loop to the top.
                    currentActivity = nextActivity;
                }
                else
                {
                    break;
                }
            } while (runCount <= _totalIterations);
        }
    }
}
