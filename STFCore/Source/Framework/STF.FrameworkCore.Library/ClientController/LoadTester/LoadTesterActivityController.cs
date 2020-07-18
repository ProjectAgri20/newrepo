using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Automation.ActivityExecution;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Automation.LoadTester
{
    public class LoadTesterActivityController : IDisposable
    {
        private readonly Collection<LoadTestThread> _threads = null;
        private readonly SystemManifest _manifest = null;
        private volatile bool _isPaused = false;

        public LoadTesterActivityController()
        {
            _manifest = GlobalDataStore.Manifest;
            _threads = new Collection<LoadTestThread>();
        }

        public void Start()
        {
            // Subscribe to all incoming messages.  These messages will originate from the Session Proxy
            // on the dispatcher server.  They include all the activities this controller will be asked to do.
            VirtualResourceEventBus.OnStartMainRun += VirtualResourceEventBus_OnStartMainRun;
            VirtualResourceEventBus.OnPauseResource += new EventHandler(VirtualResourceEventBus_PauseResource);
            VirtualResourceEventBus.OnResumeResource += new EventHandler(VirtualResourceEventBus_ResumeResource);
            VirtualResourceEventBus.OnShutdownResource += VirtualResourceEventBus_OnShutdownResource;
            VirtualResourceEventBus.OnHaltResource += new EventHandler(VirtualResourceEventBus_HaltResource);

            // Subscribe to any events created within the Activity Execution environment
            EngineEventBus.ActivityStatusMessageChanged += EngineEventBus_ActivityStatusMessageChanged;
        }

        private void VirtualResourceEventBus_OnShutdownResource(object sender, VirtualResourceEventBusShutdownArgs e)
        {
            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.ShuttingDown);
        }

        void VirtualResourceEventBus_OnStartMainRun(object sender, VirtualResourceEventBusRunArgs e)
        {
            var resource = _manifest.Resources.First();

            TraceFactory.Logger.Debug("Metadata count: {0}".FormatWith(resource.MetadataDetails.Count));

            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Starting);

            foreach (var metadataDetail in resource.MetadataDetails.Cast<LoadTesterMetadataDetail>())
            {
                var plan = metadataDetail.Plan as LoadTesterExecutionPlan;

                if (plan.Mode != ExecutionMode.Poisson)
                {
                    TraceFactory.Logger.Debug("Mode: {0}".FormatWith(plan.RampUpMode));

                    switch (plan.RampUpMode)
                    {
                        case RampUpMode.RateBased:
                            StartWithRateBasedRampUp(metadataDetail);
                            break;
                        case RampUpMode.TimeBased:
                            StartWithTimeBasedRampUp(metadataDetail);
                            break;
                    }
                }
                else
                {
                    Task.Factory.StartNew(() => StartPoissonHandler(metadataDetail));
                }
            }

            // Notify clients that this resource is now running
            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Running);
        }

        private void VirtualResourceEventBus_PauseResource(object sender, EventArgs e)
        {
            ThreadPool.QueueUserWorkItem(PauseHandler);
        }

        private void PauseHandler(object state)
        {
            Thread.CurrentThread.SetName("PAUSE");

            // Sum up how many total threads there should be based on the number of 
            // execution engines (threads) per activity.  That is the value that will be
            // set in the ApplicationFlowControl component for the pause call.
            int workerCount = _threads.Count;

            TraceFactory.Logger.Debug("Setting the PauseWait for {0} tasks".FormatWith(workerCount));

            ApplicationFlowControl.Instance.OnExecutionPaused += Instance_OnExecutionPaused;
            ApplicationFlowControl.Instance.Pause(workerCount);

            // Now go through each activity and determine if the associated threads are still
            // active, if not then those missing threads will automatically be considered paused.

            _isPaused = false;
            Collection<int> pausedTasks = new Collection<int>();

            do
            {
                foreach (var thread in _threads)
                {
                    var status = thread.Task.Status;
                    if (!pausedTasks.Contains(thread.Task.Id) && status != TaskStatus.Running && status != TaskStatus.Created)
                    {
                        pausedTasks.Add(thread.Task.Id);

                        // Force a passive CheckWait for this missing thread so that the overall wait count
                        // in the ApplicationFlowControl will decrement by 1, but it won't block.
                        ApplicationFlowControl.Instance.PassiveCheckWait();

                        TraceFactory.Logger.Debug("Called passive CheckWait - task {0} is not running ({1})".FormatWith(thread.Task.Id, thread.Task.Status));
                    }
                }

                // Hesitate for just a second and try again.
                Thread.Sleep(1000);

            } while (!_isPaused);
        }


        private void Instance_OnExecutionPaused(object sender, EventArgs e)
        {
            // Once the wait count is met, then this event handler will be called.  Set the isPaused
            // to true which will discontinue the loop just above looking for missing threads.  Turn
            // off the event and send a signal out that the resource is now paused.
            _isPaused = true;
            ApplicationFlowControl.Instance.OnExecutionPaused -= Instance_OnExecutionPaused;
            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Paused);
        }

        private void VirtualResourceEventBus_ResumeResource(object sender, EventArgs e)
        {
            ApplicationFlowControl.Instance.Resume();
            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Running);
        }

        private void VirtualResourceEventBus_HaltResource(object sender, EventArgs e)
        {
            Parallel.ForEach<LoadTestThread>(_threads, n => HaltHandler(n));
        }

        private void EngineEventBus_ActivityStatusMessageChanged(object sender, StatusChangedEventArgs e)
        {
            SessionProxyBackendConnection.ChangeResourceStatusMessage(e.StatusMessage);
        }

        #region Thread Execution

        public static void SetThreadName()
        {
            Thread.CurrentThread.SetName("L{0:0000}".FormatWith(Thread.CurrentThread.ManagedThreadId));
        }

        private void HaltHandler(LoadTestThread activity)
        {
            LoadTesterActivityController.SetThreadName();

            if (activity.Task.Status == TaskStatus.Running)
            {
                TraceFactory.Logger.Debug("Halting task {0}".FormatWith(activity.Task.Id));
                activity.Engine.Halt();
            }
            else
            {
                TraceFactory.Logger.Debug("Task {0} is not running".FormatWith(activity.Task.Id));
            }
        }

        private void StartPoissonHandler(LoadTesterMetadataDetail metadataDetail)
        {
            var plan = metadataDetail.Plan as LoadTesterExecutionPlan;

            var duration = TimeSpan.FromMinutes(plan.DurationTime);

            
            var distribution = new PoissonDistribution().GetNormalizedValues(plan.ThreadCount);
            if (distribution.Count() == 0)
            {
                TraceFactory.Logger.Debug("NO TASKS TO RUN");
                SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Completed);
                return;
            }

            // Get the time delta between each sample point.
            var delta = TimeSpan.FromTicks(duration.Ticks / (distribution.Count() - 1));
            TraceFactory.Logger.Debug("Time Delta {0}".FormatWith(delta));

            // Change the plan to support the Poisson settings.  This will create
            // each Task with a shorter execution time and will only create a
            // DurationBased execution engine.
            plan.DurationTime = (int)delta.TotalMinutes;
            plan.Mode = ExecutionMode.Duration;

            foreach (int threadCount in distribution)
            {
                // Create all the threads that will be used.
                for (int i = 0; i < threadCount; i++)
                {
                    // There is now ramp up delay with a Poisson thread
                    _threads.Add(new LoadTestThread(metadataDetail, TimeSpan.Zero));
                }

                // Start each thread for this segment
                TraceFactory.Logger.Debug("Created {0} Tasks, now starting them...".FormatWith(threadCount));
                foreach (var thread in _threads)
                {
                    thread.Task.Start();
                }

                TraceFactory.Logger.Debug("{0} Tasks started, waiting for them to complete".FormatWith(threadCount));

                // Wait for all the current threads to complete.
                Task.WaitAll(_threads.Select(x => x.Task).ToArray());

                // Clean up the completed threads
                TraceFactory.Logger.Debug("{0} Tasks completed, disposing and clearing list".FormatWith(threadCount));
                foreach (var thread in _threads)
                {
                    thread.Dispose();
                }

                _threads.Clear();
            }

            TraceFactory.Logger.Debug("ALL TASKS COMPLETE");
            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Completed);
        }

        private void StartWithRateBasedRampUp(LoadTesterMetadataDetail metadataDetail)
        {
            var plan = metadataDetail.Plan as LoadTesterExecutionPlan;

            foreach (var setting in plan.RampUpSettings)
            {
                TraceFactory.Logger.Debug("{0} threads will start after: {1} secs".FormatWith(setting.ThreadCount, setting.Delay.TotalSeconds));
                for (int i = 0; i < setting.ThreadCount; i++)
                {
                    _threads.Add(new LoadTestThread(metadataDetail, setting.Delay));
                }
            }

            Task.Factory.StartNew(ExecuteTasks);
        }

        private void StartWithTimeBasedRampUp(LoadTesterMetadataDetail metadataDetail)
        {
            var plan = metadataDetail.Plan as LoadTesterExecutionPlan;

            plan.Mode = ExecutionMode.Duration;

            // Iterate over the total thread count, but select a unique delay value for each thread.
            for (int i = 0; i < plan.ThreadCount; i++)
            {
                TimeSpan minDelay = TimeSpan.FromSeconds(plan.MinRampUpDelay);
                TimeSpan maxDelay = TimeSpan.FromSeconds(plan.MaxRampUpDelay);
                var rampDelay = plan.RandomizeRampUpDelay ? TimeSpanUtil.GetRandom(minDelay, maxDelay) : minDelay;

                TraceFactory.Logger.Debug("Delay: {0}".FormatWith(rampDelay.TotalSeconds));

                _threads.Add(new LoadTestThread(metadataDetail, rampDelay));
            }

            Task.Factory.StartNew(ExecuteTasks);
        }

        private void ExecuteTasks()
        {
            LoadTesterActivityController.SetThreadName();

            TraceFactory.Logger.Debug("Starting parallel execution of all tasks");

            // Spin up each thread in parallel to get them all started at about the same time.
            foreach (var thread in _threads)
            {
                thread.Task.Start();
            }

            // Wait for all the tasks to complete before proceeding
            Task.WaitAll(_threads.Select(x => x.Task).ToArray());

            TraceFactory.Logger.Debug("ALL TASKS COMPLETE");
            SessionProxyBackendConnection.ChangeResourceState(RuntimeState.Completed);
        }

        #endregion

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
                if (_threads != null)
                {
                    foreach (var item in _threads)
                    {
                        item.Dispose();
                    }
                }
            }
        }

        #endregion IDisposable Members
    }
}
