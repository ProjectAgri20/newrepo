using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    [ObjectFactory(ExecutionMode.RateBased)]
    internal class RateBasedEngine : EngineBase
    {
        private readonly int _duration = 0;
        private readonly int _repeatCount = 0;
        private readonly int _activityCount = 0;

        public RateBasedEngine(OfficeWorkerDetail detail)
            : base(detail)
        {
            RandomizeActivities = detail.RandomizeActivities;
            _duration = detail.DurationTime;
            _repeatCount = detail.RepeatCount;
            _activityCount = detail.MetadataDetails.Sum(x => x.Plan.Value);
        }

        public RateBasedEngine(LoadTesterMetadataDetail metadataDetail)
            : base(VirtualResourceType.LoadTester, metadataDetail, null)
        {
            var plan = metadataDetail.Plan as LoadTesterExecutionPlan;
            RandomizeActivities = false;
            _duration = plan.DurationTime;
            _repeatCount = plan.RepeatCount;
            _activityCount = 1;
        }

        public override void Run()
        {
            PacingInfo.OverallStartTime = DateTime.Now;
            PacingInfo.DurationLimit = TimeSpan.FromMinutes(_duration);
            PacingInfo.IterationLimit = _repeatCount * _activityCount;
            var totalActivities = PacingInfo.IterationLimit;
            var totalDuration = PacingInfo.DurationLimit.Ticks;

            TraceFactory.Logger.Debug("Duration: {0} mins, Iterations: {1}".FormatWith(PacingInfo.DurationLimit, PacingInfo.IterationLimit));

            int activitiesExecuted = 0;
            do
            {
                // Check to make sure the user hasn't requested this worker to pause
                ApplicationFlowControl.Instance.CheckWait(LogActivityPaused, LogActivityResumed);

                // Record the start time, execute the activity and then record the end time.
                DateTime activityStartTime = DateTime.Now;

                // Execute the activity
                var activity = GetNextActivity(ref activitiesExecuted);
                activity.Execute();
                PacingInfo.MarkActivityRunEnd(activity, activityStartTime);

                // Reset the pacing run count to the "official" pacing count determined by _pacingInfo
                activitiesExecuted = PacingInfo.PacingRunCount;

                // Get the remaining number of activities and the remaining time left in the run.
                int remainingActivities = totalActivities - activitiesExecuted;
                long remainingTime = totalDuration - PacingInfo.GetTotalElapsedTime().Ticks;

                // Get the percentage of activities and time remaining
                double percentActivitiesLeft = (double)remainingActivities / totalActivities;
                double percentTimeLeft = (double)remainingTime / totalDuration;

                // If we have not executed the entire activity set once, we don't have enough timing data to
                // determine a reasonable delay time.
                if (activitiesExecuted < _activityCount)
                {
                    TraceFactory.Logger.Debug("Initial iteration, moving on immediately.");

                    //Total duration / total activities will give us an even split of time for each activity for the first run.
                    long timeAveragePerActivity = totalDuration / totalActivities;

                    long currentTimeTaken = PacingInfo.GetTotalElapsedTime().Ticks;
                    long allotedTimeAtCurrentActivity = timeAveragePerActivity * activitiesExecuted;
                    long timeDifferentialBetweenAllotedAndCurrentTime = allotedTimeAtCurrentActivity - currentTimeTaken;

                    //if time differential is positive, we haven't used up our buffer time, we can set a delay for the difference.
                    //if it's negative, we're over our buffer time and need to catch up ASAP.
                    if (timeDifferentialBetweenAllotedAndCurrentTime > 0)
                    {
                        Delay.Wait(new TimeSpan(timeDifferentialBetweenAllotedAndCurrentTime));
                    }
                    else
                    {
                        TraceFactory.Logger.Debug("Iteration 1 behind target, moving on immediately");
                    }


                }
                else if (percentActivitiesLeft > percentTimeLeft)
                {
                    // We are behind our target - execute the next activity immediately
                    TraceFactory.Logger.Debug("Behind target, moving on immediately.");
                }
                else
                {
                    // Get the number of activity "sets" (set of activities) remaining, this is a decimal number 
                    // meaning it includes partial set values.
                    double remainingActivitySetCount = (double)remainingActivities / _activityCount;

                    // Determine the amount of time we expect to use executing the remainder of the activities.
                    // This is found by multiplying the average set execution time by the number of remaining sets.
                    long remainingWorkingTime = (long)(PacingInfo.GetAverageExecutionTimeForSet() * remainingActivitySetCount);

                    // Estimate the number of ticks available for pacing delays, calculated by removing the elapsed time
                    // from the duration, and also removing all the times estimated to run the remaining activity sets.
                    long remainingDelayTime = remainingTime - remainingWorkingTime;

                    // The remaining number of times there should be a pacing delay applied.
                    int remainingDelayCount = remainingActivities + 1;

                    // We must not be on the last delay, and we must have available pacing delay ticks
                    // in order to delay.  If either of these are false, then we move on with no delay.
                    if (remainingDelayCount > 1 && remainingDelayTime > 0)
                    {
                        TimeSpan pacingDelay = TimeSpan.FromTicks(remainingDelayTime / remainingDelayCount);
                        TraceFactory.Logger.Debug("delay Secs: {0}".FormatWith(pacingDelay.TotalSeconds));

                        PacingInfo.RecordDelay(pacingDelay);
                        Delay.Wait(pacingDelay);
                    }
                    else
                    {
                        TraceFactory.Logger.Debug("No delay, moving on immediately.");
                    }
                }

            } while (PacingInfo.PacingRunCount < PacingInfo.IterationLimit
                     && DateTime.Now.Subtract(PacingInfo.OverallStartTime) < PacingInfo.DurationLimit
                     && !ExecutionHalted);
        }

    }
}
