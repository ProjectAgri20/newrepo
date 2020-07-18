using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    [ObjectFactory(ExecutionMode.SetPaced)]
    internal class SetPaceBasedEngine : EngineBase
    {
        private TimeSpan _pace = TimeSpan.Zero;
        private TimeSpan _expiration = TimeSpan.Zero;

        public SetPaceBasedEngine(OfficeWorkerDetail detail)
            : base(detail)
        {
            RandomizeActivities = detail.RandomizeActivities;
            _pace = TimeSpan.FromSeconds(detail.MinActivityDelay);
            _expiration = TimeSpan.FromMinutes(detail.DurationTime);
        }

        public SetPaceBasedEngine(OfficeWorkerDetail detail, ActivityQueueBase activity, ActivityPacingInfo info, int expirationMinutes)
    : base(activity, info)
        {
            RandomizeActivities = detail.RandomizeActivities;
            _pace = TimeSpan.FromSeconds(detail.MinActivityDelay);
            _expiration = TimeSpan.FromMinutes(detail.DurationTime);
        }

        public SetPaceBasedEngine(LoadTesterMetadataDetail metadataDetail)
            : base(VirtualResourceType.LoadTester, metadataDetail, null)
        {
            var plan = metadataDetail.Plan as LoadTesterExecutionPlan;
            _pace = TimeSpan.FromSeconds(plan.MinActivityDelay);
            _expiration = TimeSpan.FromMinutes(plan.DurationTime);
        }

        public override void Run()
        {
            Run(_expiration);
        }


        public void Run(TimeSpan duration)
        {
            PacingInfo.OverallStartTime = DateTime.Now;
            PacingInfo.DurationLimit = duration;
            PacingInfo.IterationLimit = -1;

            TraceFactory.Logger.Debug("Will run for {0} mins".FormatWith(duration.TotalMinutes));

            // Start the monitor which will add up all pause time that may occur during execution

            int loopCount = 0;
            var startTime = DateTime.Now;
            TimeSpan totalRunTime = TimeSpan.Zero;
            TimeSpan totalDuration = duration;


            // Get the initial activity from the queue
            var currentActivity = GetNextActivity(ref loopCount);
            do
            {

                // Execute the Activity
                DateTime activityStartTime = DateTime.Now;
                currentActivity.Execute();
                PacingInfo.MarkActivityRunEnd(currentActivity, activityStartTime);
                DateTime activityEndTime = DateTime.Now;
                // Calculate the remaining time in this run by subtracting the total time the engine has
                // been running.

                totalRunTime = DateTime.Now - startTime;

                var remainingTime = totalDuration - totalRunTime;


                // Go get the next activity
                var nextActivity = GetNextActivity(ref loopCount);

                // If the start time + _pace time is passed, go to the next activity in the queue
                // Apply a delay if the endtime is less than start + _pace to hit the next start + _pace time point to start the next activity

                var timeAndPace = activityStartTime + _pace;
                if (activityEndTime < timeAndPace)
                {
                    Delay.Wait(timeAndPace - activityEndTime);
                }
                else
                {
                    
                    while (activityEndTime > timeAndPace)
                    {
                        timeAndPace = timeAndPace + _pace;
                    }
                    Delay.Wait(timeAndPace - activityEndTime);
                }


                // Set the current Activity to the next Activity and loop to the top.
                currentActivity = nextActivity;

                //var endTime = startTime.Add(duration).ToLongTimeString();
                //TraceFactory.Logger.Debug("Run Complete. Completed: {0}.  Run to {1}".FormatWith(loopCount++, endTime));

                // Run until the total run time is less than the defined duration plus any pause time.

            } while (totalRunTime < totalDuration);

            TraceFactory.Logger.Debug("Finished");

        }

    }
}
