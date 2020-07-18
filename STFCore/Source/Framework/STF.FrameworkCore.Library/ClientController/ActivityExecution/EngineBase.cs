using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework.Automation.ActivityExecution
{

    /// <summary>
    /// Base class for an activity execution engine.
    /// </summary>
    public abstract class EngineBase : IDisposable
    {
        /// <summary>
        /// Pacing Info
        /// </summary>
        protected readonly ActivityPacingInfo PacingInfo = new ActivityPacingInfo();

        /// <summary>
        /// Activity Queue
        /// </summary>
        protected readonly ActivityQueueBase ActivityQueue = null;
        
        
        /// <summary>
        /// Occurs when an activity state has changed.
        /// </summary>
        public event EventHandler<ActivityStateEventArgs> ActivityStateChanged;

        /// <summary>
        /// Gets or sets a value indicating whether to randomize activity order.
        /// </summary>
        /// <value><c>true</c> if activity order should be randomized; otherwise, <c>false</c>.</value>
        protected bool RandomizeActivities 
        {
            get { return ActivityQueue.Randomize; }
            set { ActivityQueue.Randomize = value; }
        }

        /// <summary>
        /// Gets a value indicating whether a request has been made to halt execution.
        /// </summary>
        /// <value><c>true</c> if execution halted; otherwise, <c>false</c>.</value>
        protected bool ExecutionHalted { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineBase"/> class.
        /// </summary>
        /// <param name="worker">The worker.</param>
        /// <exception cref="System.ArgumentNullException">worker</exception>
        protected EngineBase(OfficeWorkerDetail worker)
            :this(worker.ResourceType, worker.MetadataDetails, worker)
        {
            if (worker == null)
            {
                throw new ArgumentNullException("worker");
            }     
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineBase"/> class.
        /// </summary>
        protected EngineBase(VirtualResourceType resourceType, Collection<ResourceMetadataDetail> details, OfficeWorkerDetail workerDetail)
        {
            if (details == null)
            {
                throw new ArgumentNullException("details");
            }

            // Create the queue of activities based on the resource metadata details that are enabled
            var enabledActivityDetails = new Collection<ResourceMetadataDetail>(details.Where(x => x.Enabled).ToList());
            ExecutionHalted = false;

            switch (resourceType)
            {
                case VirtualResourceType.AdminWorker:
                    ActivityQueue = new AdminWorkerActivityQueue(enabledActivityDetails, OnActivityStateChanged, workerDetail);
                    break;
                default:
                    ActivityQueue = new OfficeWorkerActivityQueue(enabledActivityDetails, OnActivityStateChanged, workerDetail);
                    break;
            }
            PacingInfo.ExecutionMode = workerDetail.ExecutionMode;
        }
        /// <summary>
        /// Initializes a new instance of the <see cref="EngineBase"/> class.
        /// </summary>
        /// <param name="activityqueue"></param>
        /// <param name="info"></param>
        protected EngineBase(ActivityQueueBase activityqueue, ActivityPacingInfo info)
        {
            if (activityqueue == null)
            {
                throw new ArgumentNullException("artivityqueue");
            }
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }

            ActivityQueue = activityqueue;
            PacingInfo = info;


        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EngineBase"/> class.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="metadataDetail">The detail.</param>
        /// <param name="workerDetail"></param>
        protected EngineBase(VirtualResourceType resourceType, LoadTesterMetadataDetail metadataDetail, OfficeWorkerDetail workerDetail)
        {
            if (metadataDetail == null)
            {
                throw new ArgumentNullException("metadataDetail");
            }

            if (!metadataDetail.Enabled)
            {
                throw new ArgumentException("MetadataDetail is not enabled", "metadataDetail");
            }

            ExecutionHalted = false;

            ActivityQueue = new OfficeWorkerActivityQueue(new Collection<ResourceMetadataDetail>() { metadataDetail }, OnActivityStateChanged, workerDetail);
        }

        /// <summary>
        /// Runs this instance.
        /// </summary>
        public abstract void Run();

        /// <summary>
        /// Gets the master list of activities for this scenario.
        /// </summary>
        /// <value>The activities list.</value>
        public List<Activity> Activities
        {
            get { return ActivityQueue.MasterList; }
        }

        /// <summary>
        /// Gets all plugins from this instance.
        /// </summary>
        public Dictionary<ActivityInfo, IPluginExecutionEngine> GetPlugins()
        {
            var plugins = new Dictionary<ActivityInfo, IPluginExecutionEngine>();

            // Get all of the plugins (useful for eager loading of UI)
            foreach (Activity activity in ActivityQueue.MasterList)
            {
                plugins.Add(new ActivityInfo(activity), activity.Plugin);
            }

            return plugins;
        }

        /// <summary>
        /// Handles the <see cref="E:ActivityStateChanged" /> event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="ActivityStateEventArgs"/> instance containing the event data.</param>
        protected void OnActivityStateChanged(object sender, ActivityStateEventArgs e)
        {
            if (ActivityStateChanged != null)
            {
                ActivityStateChanged(sender, e);
            }
        }

        /// <summary>
        /// Logs that execution has paused.
        /// </summary>
        protected void LogActivityPaused()
        {
            // This only executes if there is actually a pause.
            TraceFactory.Logger.Debug("Execution is now paused...");
        }

        /// <summary>
        /// Logs that execution has resumed.
        /// </summary>
        protected void LogActivityResumed()
        {
            // This only executes if there is actually a pause.
            TraceFactory.Logger.Debug("Execution has resumed...");
        }

        /// <summary>
        /// Gets the next activity
        /// </summary>
        /// <param name="runCount"></param>
        /// <returns></returns>
        protected Activity GetNextActivity(ref int runCount)
        {
            return ActivityQueue.GetNextActivity(ref runCount);
        }

        /// <summary>
        /// Halts this instance.
        /// </summary>
        public void Halt()
        {
            this.ExecutionHalted = true;
        }

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
                if (ActivityQueue != null)
                {
                    ActivityQueue.Dispose();
                }
            }
        }

        #endregion IDisposable Members
    }
}
