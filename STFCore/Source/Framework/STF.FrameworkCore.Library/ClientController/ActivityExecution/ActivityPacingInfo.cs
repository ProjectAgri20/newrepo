using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    /// <summary>
    /// Class for Activity Pacing Info
    /// </summary>
    public class ActivityPacingInfo
    {
        private readonly Dictionary<Guid, List<long>> _activityRuntimeHistory = new Dictionary<Guid, List<long>>();
        private TimeSpan _totalDelayApplied = TimeSpan.Zero;

        /// <summary>
        /// 
        /// </summary>
        public int PacingRunCount { get; private set; }

        /// <summary>
        /// Total Run Count
        /// </summary>
        public int TotalRunCount { get; set; }

        /// <summary>
        /// Overall Start Time
        /// </summary>
        public DateTime OverallStartTime { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public int IterationLimit { get; set; }

        /// <summary>
        /// Execution Duration Limit
        /// </summary>
        public TimeSpan DurationLimit { get; set; }

        /// <summary>
        /// Execution Mode
        /// </summary>
        public ExecutionMode ExecutionMode { get; set; }

        /// <summary>
        /// Returns the total time elapsed since Execution started
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTotalElapsedTime()
        {
            return DateTime.Now.Subtract(OverallStartTime);
        }

        /// <summary>
        /// Gets average execution time for an activity
        /// </summary>
        /// <param name="activityId"></param>
        /// <returns></returns>
        public TimeSpan GetAverageExecutionTimeForActivity(Guid activityId)
        {
            // default to 10 minutes
            var result = new TimeSpan(0, 10, 0);
            if (_activityRuntimeHistory.ContainsKey(activityId))
            {
                result = TimeSpan.FromTicks((long)_activityRuntimeHistory[activityId].Average());
            }
            return result;
        }

        /// <summary>
        /// Get average execution time for the set
        /// </summary>
        /// <returns></returns>
        public long GetAverageExecutionTimeForSet()
        {
            return (long)_activityRuntimeHistory.Values.Sum(n => n.Average());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public TimeSpan GetTotalDelayApplied()
        {
            return _totalDelayApplied;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="delay"></param>
        public void RecordDelay(TimeSpan delay)
        {
            _totalDelayApplied = _totalDelayApplied + delay;
        }

        /// <summary>
        /// 
        /// </summary>
        public ActivityPacingInfo()
        {
            PacingRunCount = 0;
            TotalRunCount = 0;
        }

        internal void MarkActivityRunEnd(Activity activity, DateTime activityRunStartTime)
        {
            TotalRunCount++;
            PacingRunCount++;
            DateTime activityEndTime = DateTime.Now;

            TraceFactory.Logger.Debug("Start: {0}, End: {0}".FormatWith(activityRunStartTime.ToLongTimeString(), activityEndTime.ToLongTimeString()));

            // Create a new indexed entry if it does not exist
            if (!_activityRuntimeHistory.ContainsKey(activity.Id))
            {
                _activityRuntimeHistory.Add(activity.Id, new List<long>());
            }

            // Record the amount of time it just took to execute the given activity.
            var activityRunElapsedTime = activityEndTime.Subtract(activityRunStartTime);
            _activityRuntimeHistory[activity.Id].Add(activityRunElapsedTime.Ticks);
        }
    }
}