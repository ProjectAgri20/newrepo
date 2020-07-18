using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    internal class OfficeWorkerActivityQueue : ActivityQueueBase
    {
        public OfficeWorkerActivityQueue(Collection<ResourceMetadataDetail> details, EventHandler<ActivityStateEventArgs> handler, OfficeWorkerDetail workerDetail)
            :base(details, handler, workerDetail)
        {
        }

        public override Activity GetNextActivity(ref int runCount)
        {
            runCount++;
            return DequeueActivity(runCount == 1);
        }

        private Activity DequeueActivity(bool firstRun)
        {
            //during a repeat session the previous session would have queued the execution queue, which makes it run out of order by 1 activity
            //by clearning the queue for the repeat session, we maintain the same order.
            if (firstRun)
            {
                ExecutionQueue.Clear();
            }

           //if there is any queued item return the topmost 
            if (ExecutionQueue.Any())
                return ExecutionQueue.Dequeue();

            // Load the available activities into the queue if the queue is empty
            // or on first run (repeat session situation)
            // Add activities based on the execution count which describes
            // the overall distribution or weight of different activities
            // being executed.
            List<Activity> activities = new List<Activity>();
            foreach (var activity in MasterList.OrderBy(n => n.ExecutionOrder))
            {
                for (int i = 0; i < activity.ExecutionValue; i++)
                {
                    activities.Add(activity);
                }
            }

            if (Randomize)
            {
                activities.Shuffle();
                TraceFactory.Logger.Debug("Activities were shuffled");
            }
                
            foreach (Activity activity in activities)
            {
                ExecutionQueue.Enqueue(activity);
            }

            return ExecutionQueue.Dequeue();
        }
    }
}
