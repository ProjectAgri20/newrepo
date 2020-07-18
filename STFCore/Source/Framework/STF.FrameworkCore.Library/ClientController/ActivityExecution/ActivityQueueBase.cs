using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{
    /// <summary>
    /// Activitiy Queue Base Class
    /// </summary>
    public abstract class ActivityQueueBase : IDisposable
    {
        /// <summary>
        /// Execution Queue of Activities
        /// </summary>
        protected Queue<Activity> ExecutionQueue { get; private set; }

        /// <summary>
        /// Master list of activities
        /// </summary>
        public List<Activity> MasterList { get; private set; }
        /// <summary>
        /// Flag for randomizing the order of activities
        /// </summary>
        public bool Randomize { get; set; }

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="details"></param>
        /// <param name="handler"></param>
        /// <param name="officeWorker"></param>
        protected ActivityQueueBase(Collection<ResourceMetadataDetail> details, EventHandler<ActivityStateEventArgs> handler, OfficeWorkerDetail officeWorker)
        {
            MasterList = new List<Activity>();
            ExecutionQueue = new Queue<Activity>();
            Randomize = false;

            foreach (var metadata in details)
            {
                Activity activity = new Activity(metadata, officeWorker);
                activity.ActivityStateChanged += handler;
                MasterList.Add(activity);

                TraceFactory.Logger.Debug("Adding activity: {0}".FormatWith(metadata.Name));
            }
        }
     
        /// <summary>
        /// Gets the next activitiy in line for execution
        /// </summary>
        /// <param name="runCount"></param>
        /// <returns></returns>
        public abstract Activity GetNextActivity(ref int runCount);

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (MasterList != null)
                {
                    foreach (var activity in MasterList)
                    {
                        activity.Dispose();
                    }
                }
            }
        }

        #endregion IDisposable Members

    }
}
