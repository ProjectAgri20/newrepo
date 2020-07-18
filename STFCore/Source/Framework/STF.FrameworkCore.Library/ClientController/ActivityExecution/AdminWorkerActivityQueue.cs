using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    internal class AdminWorkerActivityQueue : ActivityQueueBase
    {
        private int _currentActivityExecutionCount = 0;
        private Activity _currentActivity = null;

        public AdminWorkerActivityQueue(Collection<ResourceMetadataDetail> details, EventHandler<ActivityStateEventArgs> handler, OfficeWorkerDetail worker)
            :base(details, handler, worker)
        {
        }

        public override Activity GetNextActivity(ref int runCount)
        {
            if (_currentActivity == null)
            {
                // If the current activity is null, then this is the first request, so
                // pull and activity off the execution queue.
                _currentActivity = DequeueActivity(runCount == 0);
            }
            else
            {
                if (_currentActivityExecutionCount >= _currentActivity.ExecutionValue)
                {
                    // If the execution count has been reach, then dequeue the next activity
                    _currentActivity = DequeueActivity(runCount == 0);
                }
                else
                {
                    // If the execution count has not been reached, then just increment the
                    // count and return the same activity.
                    _currentActivityExecutionCount++;
                }
            }

            runCount++;

            TraceFactory.Logger.Debug("Next activity: '{0}'".FormatWith(_currentActivity.Name));
            return _currentActivity;
        }

        private Activity DequeueActivity(bool firstRun)
        {
            // Load the available activities into the queue if the queue is empty
            // or on first run (repeat session situation)
            if (!ExecutionQueue.Any() || firstRun)
            {
                // Add activities based on the execution count which describes
                // the overall distribution or weight of different activities
                // being executed.
                List<Activity> activities = new List<Activity>();
                foreach (var item in MasterList.OrderBy(n => n.ExecutionOrder))
                {
                    activities.Add(item);
                }

                if (Randomize)
                {
                    // If randomize was set for this collection of activities, then shuffle them
                    activities.Shuffle();
                    TraceFactory.Logger.Debug("Activities were shuffled");
                }

                foreach (Activity activity in activities)
                {
                    // Then enqueue each activity onto the execution queue.
                    ExecutionQueue.Enqueue(activity);
                }
            }

            _currentActivityExecutionCount = 1;

            return ExecutionQueue.Dequeue();
        }
    }
}
