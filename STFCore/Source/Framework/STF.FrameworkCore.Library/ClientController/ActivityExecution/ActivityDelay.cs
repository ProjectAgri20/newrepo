using HP.ScalableTest.Framework.Manifest;
using System;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    internal class ActivityDelay
    {
        private readonly TimeSpan _activityDelayMin = TimeSpan.Zero;
        private readonly TimeSpan _activityDelayMax = TimeSpan.Zero;
        private readonly bool _randomizeActivityDelay = false;

        public ActivityDelay(OfficeWorkerDetail worker)
        {
            _activityDelayMin = TimeSpan.FromSeconds(worker.MinActivityDelay);
            _activityDelayMax = TimeSpan.FromSeconds(worker.MaxActivityDelay);
            _randomizeActivityDelay = worker.RandomizeActivityDelay;
        }


        public ActivityDelay(LoadTesterExecutionPlan plan)
        {
            _randomizeActivityDelay = plan.RandomizeActivityDelay;
            _activityDelayMin = TimeSpan.FromSeconds(plan.MinActivityDelay);
            _activityDelayMax = TimeSpan.FromSeconds(plan.MaxActivityDelay);
        }

        public void Apply(Activity currentActivity, Activity nextActivity)
        {
            TimeSpan activityDelay = TimeSpan.Zero;

            if (currentActivity.Pacing.Enabled)
            {
                var pacing = currentActivity.Pacing;

                if (currentActivity.Pacing.DelayOnRepeat)
                {
                    activityDelay = CalculateDelay(pacing);
                    TraceFactory.Logger.Debug("Using Activity Delay: Delay On Each met, Total delay {0} secs".FormatWith(activityDelay.TotalSeconds));
                }
                else
                {
                    if (currentActivity.Id != nextActivity.Id)
                    {
                        activityDelay = CalculateDelay(pacing);
                        TraceFactory.Logger.Debug("Using Activity Delay: Delay At End met, Total delay {0} secs".FormatWith(activityDelay.TotalSeconds));
                    }
                    else
                    {
                        TraceFactory.Logger.Debug("Using Activity Delay: Delay At End not met, returning");
                        return;
                    }
                }
            }
            else
            {
                // Pause for the defined activity pacing delay
                activityDelay = GetDelay(_activityDelayMin, _activityDelayMax, _randomizeActivityDelay);
                TraceFactory.Logger.Debug("Using Worker Delay: Total delay {0} secs".FormatWith(activityDelay.TotalSeconds));
            }

            ApplicationFlowControl.Instance.Wait(activityDelay);
        }

        private static TimeSpan CalculateDelay(ActivitySpecificPacing pacing)
        {
            return GetDelay(pacing.MinDelay, pacing.MaxDelay, pacing.Randomize);
        }

        private static TimeSpan GetDelay(TimeSpan minDelay, TimeSpan maxDelay, bool random)
        {
            return random ? TimeSpanUtil.GetRandom(minDelay, maxDelay) : minDelay;
        }
    }
}
