using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web.Services.Protocols;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Defines any host used to support a virtual resource that will run
    /// as part of a test scenario.  Typically the <see cref="ResourceHost"/>
    /// will be a virtual machine, but it can be a physical machine an potentially
    /// other hosts in the future.
    /// </summary>
    public class ResourceHost : ISessionMapElement
    {
        private CancellationTokenSource _taskCancel = null;
        private bool _inShutdown = false;

        private TimeSpan _waitTimeout = default(TimeSpan);
        private ResettableCountdownEvent _registrationWaitHandle = null;
        private ResettableCountdownEvent _resourcesReadyWaitHandle = null;
        private ResettableCountdownEvent _resourcesPausedWaitHandle = null;
        private ResettableCountdownEvent _resourcesShutdownWaitHandle = null;

        /// <summary>
        /// Gets the <see cref="ResourceInstance"/> collection contains all resources running on this machine
        /// </summary>
        [SessionMapElementCollection]
        public Collection<ResourceInstance> Resources { get; private set; }

        /// <summary>
        /// Gets the system manifest being used in the scenario being executed
        /// </summary>
        public SystemManifest Manifest { get; private set; }

        /// <summary>
        /// Gets the machine associated with the host.
        /// </summary>
        public HostMachine Machine { get; set; }

        /// <summary>
        /// Gets the <see cref="SessionMapElement"/> object that contains the current status of this machine.
        /// </summary>
        public SessionMapElement MapElement { get; private set; }

        /// <summary>
        /// Occurs when all resources on this machine signal they are complete.
        /// </summary>
        public event EventHandler OnResourcesComplete;

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceHost" /> class.
        /// </summary>
        /// <param name="manifest">The manifest.</param>
        public ResourceHost(SystemManifest manifest)
        {
            Manifest = manifest;

            // Set the logging thread context
            if (manifest != null)
            {
                TraceFactory.SetThreadContextProperty("SessionId", manifest.SessionId, false);
                TraceFactory.SetThreadContextProperty("Dispatcher", manifest.Dispatcher, false);
            }

            Resources = new Collection<ResourceInstance>();

            // Define an empty machine class as simply a placeholder.  It will be replaced with an 
            // actual machine class in the validate step.
            Machine = new HostMachine(manifest);

            Machine.OnStatusChanged += Machine_OnStatusChanged;

            // Create a temporary Id for the map element as it won't be set until the machines are created
            MapElement = new SessionMapElement(Guid.NewGuid().ToString(), ElementType.Machine);
            MapElement.Enabled = false;
        }

        private void Machine_OnStatusChanged(object sender, HostMachineEventArgs e)
        {
            if (e.State == RuntimeState.None)
            {
                MapElement.UpdateStatus(e.Message);
            }
            else if (string.IsNullOrEmpty(e.Message))
            {
                MapElement.UpdateStatus(e.State);
            }
            else
            {
                MapElement.UpdateStatus(e.Message, e.State);
            }
        }

        /// <summary>
        /// Builds the objects and data structures used to support this machine.
        /// </summary>
        public virtual void Stage(ParallelLoopState loopState)
        {
            Thread.CurrentThread.SetName("Stage-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            TraceFactory.Logger.Debug("Entering...");

            // Add a resource object for each resource in the manifest
            var query =
                (
                    from r in Manifest.Resources
                    from n in r.UniqueNames
                    select new { Resource = r, InstanceId = n }
                );

            foreach (var item in query)
            {
                var instance = ObjectFactory.Create<ResourceInstance>(item.Resource.ResourceType, item.InstanceId, item.Resource);
                instance.OnStateChanged += ResourceStateChanged;

                TraceFactory.Logger.Debug("Adding {0} : {1} : {2}".FormatWith(Machine.Name, instance.Id, item.Resource.ResourceType));
                Resources.Add(instance);
            }
        }

        /// <summary>
        /// Revalidates all resources.
        /// </summary>
        /// <param name="loopState">State of the loop.</param>
        public virtual void Revalidate(ParallelLoopState loopState)
        {
            Thread.CurrentThread.SetName("Revalidate-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            // If this element is already validated, then just
            // return as nothing needs to be done.  If it is not
            // validated, then proceed to validate it.
            if (MapElement.State == RuntimeState.Validated)
            {
                MapElement.UpdateStatus("Validated", RuntimeState.Validated);
                return;
            }

            Validate(loopState);
        }

        /// <summary>
        /// Initializes this asset
        /// </summary>
        public virtual void Validate(ParallelLoopState loopState)
        {
            Thread.CurrentThread.SetName("Validate-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            MapElement.UpdateStatus("Validating", RuntimeState.Validating);

            try
            {
                TraceFactory.Logger.Debug("Validating host {0}".FormatWith(Machine.Name));
                Machine.Validate();
                
                // In case a new Machine was chosen, update the name of the map element to be consistent.
                MapElement.Name = Machine.Name;
                MapElement.UpdateStatus();
                Manifest.HostMachine = Machine.Name;
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                MapElement.UpdateStatus(ex.Message, RuntimeState.Error);
                loopState.Break();
                return;
            }

            TraceFactory.Logger.Debug("Now validating each resource running on {0}".FormatWith(Machine.Name));

            Parallel.ForEach<ResourceInstance>(Resources, (r, l) => r.Validate(l, Manifest, Machine.Name));

            if (SessionMapElement.AnyElementsSetTo<ResourceInstance>(Resources, RuntimeState.Error))
            {
                var elements = SessionMapElement.GetElements<ResourceInstance>(RuntimeState.Error, Resources).Select(x => x.Id);
                TraceFactory.Logger.Debug("Resources in ERROR: {0}".FormatWith(string.Join(", ", elements.ToArray())));

                TraceFactory.Logger.Debug("Resource validation for {0}".FormatWith(Machine.Name));
                MapElement.UpdateStatus("Resource validation failed", RuntimeState.Error);
                loopState.Break();
                return;
            }
            else if (SessionMapElement.AnyElementsSetTo<ResourceInstance>(Resources, RuntimeState.Warning))
            {
                var elements = SessionMapElement.GetElements<ResourceInstance>(RuntimeState.Warning, Resources).Select(x => x.Id);
                TraceFactory.Logger.Debug("Resources in WARNING: {0}".FormatWith(string.Join(", ", elements.ToArray())));

                TraceFactory.Logger.Debug("Resource validation caused warning on {0}".FormatWith(Machine.Name));
                MapElement.UpdateStatus("Resource validation caused a warning", RuntimeState.Warning);
                return;
            }

            MapElement.UpdateStatus("Validated", RuntimeState.Validated);
        }

        private enum PowerUpResult
        {
            None,
            Completed,
            Failed,
            Cancelled,
            Restarted,
        }

        /// <summary>
        /// Powers up the remote host.
        /// </summary>
        /// <param name="loopState">State of the loop.</param>
        public void PowerUp(ParallelLoopState loopState)
        {
            _inShutdown = false;

            MapElement.UpdateStatus("Starting", RuntimeState.Starting);
            TraceFactory.Logger.Debug("{0}: PowerUp".FormatWith(Machine.Name));
            
            PowerUpResult result = PowerUpResult.None;
            int attempt = 1;
            Thread thread = null;

            do
            {
                try
                {
                    if (_taskCancel != null)
                    {
                        _taskCancel.Dispose();
                    }
                    _taskCancel = new CancellationTokenSource();

                    TraceFactory.Logger.Debug("{0}: Creating the PowerUp Task".FormatWith(Machine.Name));
                    var task = new Task(() =>
                    {
                        thread = new Thread(new ParameterizedThreadStart(StartMachineThreadHandler));
                        if (StartMachine(thread))
                        {
                            result = PowerUpResult.Completed;
                        }
                        else
                        {
                            result = PowerUpResult.Failed;
                        }
                    });

                    task.Start();
                    task.Wait(_taskCancel.Token);
                    TraceFactory.Logger.Debug("{0}: PowerUp Task is completed".FormatWith(Machine.Name));

                    if (_taskCancel != null)
                    {
                        _taskCancel.Dispose();
                        _taskCancel = null;
                    }
                }
                catch (OperationCanceledException ex)
                {
                    TraceFactory.Logger.Error("{0}: OperationCancelledException: {1}".FormatWith(Machine.Name, ex.ToString()));

                    Abort(thread);

                    if (_taskCancel.Token.IsCancellationRequested)
                    {
                        if (_inShutdown)
                        {
                            TraceFactory.Logger.Debug("{0}: The cancellation is a Shutdown request".FormatWith(Machine.Name));
                            result = PowerUpResult.Cancelled;
                            MapElement.UpdateStatus(RuntimeState.Halted);
                            loopState.Break();
                            break;
                        }
                        else
                        {
                            TraceFactory.Logger.Debug("{0}: The client requested a Restart".FormatWith(Machine.Name));
                            MapElement.UpdateStatus("Restart");
                            result = PowerUpResult.Restarted;
                        }
                    }

                    CheckAttempts(attempt++);
                }
                catch (AggregateException ex)
                {
                    TraceFactory.Logger.Debug("{0}: AggregateException: {1}".FormatWith(Machine.Name, ex.ToString()));

                    Abort(thread);
                    result = PowerUpResult.Failed;

                    CheckAttempts(attempt++);
                }

            } while (result != PowerUpResult.Completed);

            switch (result)
            {
                case PowerUpResult.Completed:
                    TraceFactory.Logger.Debug("{0}: Startup completed".FormatWith(Machine.Name));
                    if (this.Resources.Any(x => x.MapElement.State == RuntimeState.Error))
                    {
                        MapElement.UpdateStatus(RuntimeState.Error);
                    }
                    else
                    {
                        MapElement.UpdateStatus("Ready", RuntimeState.Ready);
                    }
                    break;

                case PowerUpResult.Failed:
                    TraceFactory.Logger.Debug("{0}: Startup failed".FormatWith(Machine.Name));
                    MapElement.UpdateStatus(RuntimeState.Error);
                    loopState.Break();
                    break;
            }
        }

        private class ThreadStatus
        {
            public bool Success { get; set; }

            public ThreadStatus()
            {
                Success = false;
            }
        }

        private bool CancellationReceived()
        {
            if (_taskCancel != null && _taskCancel.Token.IsCancellationRequested)
            {
                TraceFactory.Logger.Debug("{0}: Cancellation requested".FormatWith(Machine.Name));
                return true;
            }

            return false;
        }

        private bool StartMachine(Thread thread)
        {
            ThreadStatus status = new ThreadStatus();

            if (CancellationReceived())
            {
                return false;
            }

            try
            {
                TraceFactory.Logger.Debug("{0}: Starting background thread".FormatWith(Machine.Name));
                thread.IsBackground = true;
                thread.Start(status);

                TraceFactory.Logger.Debug("{0}: Waiting for background thread to complete".FormatWith(Machine.Name));
                thread.Join();

                TraceFactory.Logger.Debug("{0}: Background thread finished".FormatWith(Machine.Name));
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("{0}: Exception: {1}".FormatWith(Machine.Name, ex.ToString()));
                status.Success = false;
            }

            TraceFactory.Logger.Debug("{0}: Background thread completed status: {1}".FormatWith(Machine.Name, status.Success));
            return status.Success;
        }

        private void StartMachineThreadHandler(object state)
        {
            ThreadStatus status = state as ThreadStatus;
            status.Success = false;

            try
            {
                Machine.PowerOn(_taskCancel);
            }
            catch (Exception ex)
            {
                // If there is a general error of some type, then log it and then rethrow the exception.  
                // This will cause the task to fail, and it will then try to restart again.
                TraceFactory.Logger.Debug("{0}: Exception: {1}".FormatWith(Machine.Name, ex.ToString()));
                status.Success = false;
                return;
            }

            if (CancellationReceived())
            {
                return;
            }

            try
            {
                TraceFactory.Logger.Debug("{0}: Starting STF download and launching controller".FormatWith(Machine.Name));
                InitializeWaitHandles();

                Retry.WhileThrowing(() => Machine.Setup(), 5, TimeSpan.FromSeconds(10), new List<Type>() { typeof(SoapException) });
               

                if (CancellationReceived())
                {
                    return;
                }

                TraceFactory.Logger.Debug("{0}: Waiting for Registration request".FormatWith(Machine.Name));
                WaitForClientControllerToRegister();

                if (CancellationReceived())
                {
                    return;
                }

                TraceFactory.Logger.Debug("{0}: Waiting for to Signal Ready".FormatWith(Machine.Name));
                WaitForResourceToSignalReady();

                status.Success = true;
            }
            catch (ThreadAbortException ex)
            {
                Thread.ResetAbort();
                TraceFactory.Logger.Debug("{0}: ThreadAbortException: {1}".FormatWith(Machine.Name, ex.ToString()));
                status.Success = false;
            }
            catch (Exception ex)
            {
                // If there is a general error of some type, then log it and then rethrow the exception.  
                // This will cause the task to fail, and it will then try to restart again.
                TraceFactory.Logger.Debug("{0}: Exception: {1}".FormatWith(Machine.Name, ex.ToString()));
                status.Success = false;
            }
        }

        private void CheckAttempts(int attempts)
        {
            TraceFactory.Logger.Debug("{0}: Attempt #{1}".FormatWith(Machine.Name, attempts));
            if (attempts % 5 == 0)
            {
                TraceFactory.Logger.Debug("{0}: Replacing the current machine".FormatWith(Machine.Name));
                // Swap the machine after some number of attempts
                MapElement.UpdateStatus("NewMachine");
                Machine.Replace();
                Manifest.HostMachine = Machine.Name;
                MapElement.Name = Machine.Name;
            }

            // Sleep for a few seconds to let things clear through.
            Thread.Sleep(5000);
        }

        private void Abort(Thread thread)
        {
            // If the boot thread is still running, then abort it.
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    TraceFactory.Logger.Debug("{0}: Aborting Thread".FormatWith(Machine.Name));
                    thread.Abort();
                    TraceFactory.Logger.Debug("{0}: Thread aborted".FormatWith(Machine.Name));
                }
                else
                {
                    TraceFactory.Logger.Debug("{0}: NOT Aborting Thread: Alive: {1}, Running: {2}"
                        .FormatWith(Machine.Name, thread.IsAlive, thread.ThreadState == ThreadState.Running));
                }
            }
        }

        /// <summary>
        /// Executes this resource host, which may mean different things.
        /// </summary>
        public void Run(ParallelLoopState loopState)
        {
            Parallel.ForEach<ResourceInstance>(Resources, (r, l) => r.Run(l));
            if (!SessionMapElement.AllElementsSetTo<ResourceInstance>(Resources, RuntimeState.Running))
            {
                TraceFactory.Logger.Debug("{0}: Not all resources reached a running state".FormatWith(Machine.Name));
                loopState.Break();
            }
            MapElement.UpdateStatus("Running", RuntimeState.Running);
        }

        /// <summary>
        /// Executes this resource host, which may mean different things.
        /// </summary>
        public void Repeat(ParallelLoopState loopState)
        {
            Parallel.ForEach<ResourceInstance>(Resources, (r, l) => r.Run(l));
            MapElement.UpdateStatus("Running", RuntimeState.Running);
        }

        /// <summary>
        /// Shuts down this resource host
        /// </summary>
        /// <param name="options">The options used to define how the machine is shutdown</param>
        /// <param name="loopState">State of the loop.</param>
        public void Shutdown(ShutdownOptions options, ParallelLoopState loopState)
        {
            _inShutdown = true;

            MapElement.UpdateStatus("Shutdown", RuntimeState.ShuttingDown);

            // If the Machine has not been configured yet, then just return
            if (!Machine.Configured)
            {
                TraceFactory.Logger.Debug("This host is not configured, returning");
                MapElement.UpdateStatus("Offline", RuntimeState.Offline);
                return;
            }

            TraceFactory.Logger.Debug("Shutting down {0}".FormatWith(Machine.Name));

            if (CancelBootup())
            {
                TraceFactory.Logger.Debug("{0} was booting, so just shutdown machine".FormatWith(Machine.Name));
                Machine.Shutdown(options);
            }
            else
            {
                TraceFactory.Logger.Debug("{0} already booted, shutdown resources, then machine".FormatWith(Machine.Name));

                // Create the wait handle first, so that it is decrementing even before the
                // wait occurs.  That way if the resources send their offline state changes
                // this count latch will receive them and appropriately decrement.
                InitializeShutdownWaitHandle();

                try
                {
                    Parallel.ForEach<ResourceInstance>(Resources, r => r.Shutdown(options));
                    WaitForResourcesToShutdown();
                    TraceFactory.Logger.Debug("{0}: All virtual resources now exited".FormatWith(Machine.Name));
                }
                catch (AggregateException ex)
                {
                    TraceFactory.Logger.Error("Error shutting down resources: {0}".FormatWith(ex.ToString()));
                }

                try
                {
                    // If we are in a validated or offline state, then the client VM hasn't fully booted and won't
                    // have a service running to call Cleanup on.  This is just here to speed up the
                    // shutdown process when in this condition.
                    if (options.PerformCleanup && MapElement.State != RuntimeState.Validated && MapElement.State != RuntimeState.Offline)
                    {
                        // Tell the host to cleanup as well.  This will provide an opportunity for a host
                        // to perform any custom clean up items.
                        using (var resourceHostClient = ClientControllerServiceConnection.Create(Machine.Name))
                        {
                            resourceHostClient.Channel.Cleanup();
                            TraceFactory.Logger.Debug("Sent Cleanup signal to {0}".FormatWith(Machine.Name));
                        }
                    }
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error("Unable to call Cleanup: {0}".FormatWith(ex.ToString()));
                }

                try
                {
                    if (options.CopyLogs)
                    {
                        CopyClientControllerLogs();
                    }
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error("Unable to copy Logs: {0}".FormatWith(ex.ToString()));
                }

                try
                {
                    MapElement.UpdateStatus("Powering off");
                    TraceFactory.Logger.Debug("Shutdown machine {0}".FormatWith(Machine.Name));
                    Machine.Shutdown(options);
                    TraceFactory.Logger.Debug("Shutdown machine {0} COMPLETE".FormatWith(Machine.Name));
                    MapElement.UpdateStatus("Shut down");
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error("Unable to shutdown {0}: {1}".FormatWith(Machine.Name, ex.ToString()));
                }
            }

            if (options.ReleaseDeviceReservation)
            {
                try
                {
                    MapElement.UpdateStatus("Release");
                    TraceFactory.Logger.Debug("Machine {0} releasing".FormatWith(Machine.Name));
                    Machine.Release();
                    TraceFactory.Logger.Debug("Machine {0} released".FormatWith(Machine.Name));
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error("Error releasing {0}: {1}".FormatWith(Machine.Name, ex.ToString()));
                }
            }

            MapElement.UpdateStatus("Offline", RuntimeState.Offline);
        }

        private bool CancelBootup()
        {
            bool cancelled = false;

            if (_taskCancel != null)
            {
                TraceFactory.Logger.Debug("Cancelling current boot process for {0}".FormatWith(Machine.Name));
                _taskCancel.Cancel();
                cancelled = true;
            }

            return cancelled;
        }

        /// <summary>
        /// This will reboot the machine and start over.
        /// </summary>
        public void Restart(bool useNewMachine)
        {
            TraceFactory.Logger.Debug("Use New Machine: {0}".FormatWith(useNewMachine));

            // If there is a cancellation object in place, then use it to
            // abort the current deployment process and to start it again.  If there
            // isn't one, then just return as the intent of this is to reset
            // the machine during the deployment process if there is a problem.
            if (CancelBootup())
            {
                TraceFactory.Logger.Debug("{0} was booting, cancelled and machine is rebooting...".FormatWith(Machine.Name));
            }
            else
            {
                TraceFactory.Logger.Debug("{0} was NOT booting, nothing will be done.".FormatWith(Machine.Name));
            }
        }

        /// <summary>
        /// Gets the wait timeout for machine state transitions.
        /// </summary>
        protected TimeSpan WaitTimeout
        {
            get
            {
                if (_waitTimeout == default(TimeSpan))
                {
                    var timeout = GlobalSettings.Items[Setting.VmStateTransitionTimeoutInSeconds];
                    _waitTimeout = TimeSpan.FromSeconds(Convert.ToInt32(timeout));
                }

                return _waitTimeout;
            }
        }

        /// <summary>
        /// Pauses this instance.
        /// </summary>
        public void Pause()
        {
            if (MapElement.State == RuntimeState.Running)
            {
                TraceFactory.Logger.Debug("Pausing all resources on {0}".FormatWith(Machine.Name));

                // Only apply a pause to the resources that are still running.  Others in a different state
                // will be ignored.  Then only have the wait handle wait for those resources that were chosen
                var runningResources = Resources.Where(e => e.MapElement.State == RuntimeState.Running);

                try
                {
                    if (runningResources.Any())
                    {
                        _resourcesPausedWaitHandle = new ResettableCountdownEvent(runningResources.Count());
                        Parallel.ForEach<ResourceInstance>(runningResources, r => r.Pause());

                        MapElement.UpdateStatus("PauseWait");
                        _resourcesPausedWaitHandle.Wait(WaitTimeout);
                        TraceFactory.Logger.Debug("All pause signals received on {0}, setting state to Paused".FormatWith(Machine.Name));
                    }
                    else
                    {
                        TraceFactory.Logger.Debug("There are no running resources to pause");
                    }

                    MapElement.UpdateStatus("Paused", RuntimeState.Paused);
                }
                catch (TimeoutException ex)
                {
                    MapElement.UpdateStatus("Error", RuntimeState.Error);
                    TraceFactory.Logger.Error(ex);
                }
                finally
                {
                    _resourcesPausedWaitHandle = null;
                }
            }
            else
            {
                TraceFactory.Logger.Debug("{0} not in a running state".FormatWith(Machine.Name));
            }
        }

        /// <summary>
        /// Resumes execution on this resource host, which will resume all running resources
        /// </summary>
        public void Resume()
        {
            if (MapElement.State == RuntimeState.Paused)
            {
                TraceFactory.Logger.Debug("Resuming all resources");

                MapElement.UpdateStatus("Resuming");
                var pausedResources = Resources.Where(e => e.MapElement.State == RuntimeState.Paused);
                Parallel.ForEach<ResourceInstance>(pausedResources, r => r.Resume());

                MapElement.UpdateStatus("Running", RuntimeState.Running);
            }
        }

        /// <summary>
        /// Sends a synchronization signal with the specified event name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public void SignalSynchronizationEvent(string eventName)
        {
            TraceFactory.Logger.Debug($"Sending synchronization signal '{eventName}'.");
            Parallel.ForEach(Resources, n => n.SignalSynchronizationEvent(eventName));
        }

        /// <summary>
        /// Suspends operations to an asset.
        /// </summary>
        /// <param name="assetId"></param>
        public void TakeOffline(string assetId)
        {
            TraceFactory.Logger.Debug("Taking {0} Offline".FormatWith(assetId));

            Parallel.ForEach<ResourceInstance>(Resources, r => r.TakeOffline(assetId));
        }

        /// <summary>
        /// Resumes operations to an asset.
        /// </summary>
        /// <param name="assetId"></param>
        public void BringOnline(string assetId)
        {
            TraceFactory.Logger.Debug("Bringing {0} Online".FormatWith(assetId));

            Parallel.ForEach<ResourceInstance>(Resources, r => r.BringOnline(assetId));
        }

        private void ResourceStateChanged(object sender, ResourceInstanceEventArgs e)
        {
            TraceFactory.Logger.Debug("{0} -> {1}".FormatWith(e.InstanceId, e.State));

            // Since we just heard from at least one resource itself, reset the wait event timeout values
            // as this is an indication that it's still alive.
            ResetWaitEventsTimeout();

            switch (e.State)
            {
                case RuntimeState.Error:
                case RuntimeState.Ready:
                    if (_resourcesReadyWaitHandle != null)
                    {
                        // Signal that this resource is ready, the latch will ensure each resource only
                        // signals once.  When the latch reaches zero, it will release.
                        _resourcesReadyWaitHandle.Signal(e.InstanceId);
                        int count = _resourcesReadyWaitHandle == null ? 0 : _resourcesReadyWaitHandle.CurrentCount;
                        TraceFactory.Logger.Debug($"{Machine.Name}/{e.InstanceId} is {e.State}, {count} remaining".FormatWith(Machine.Name, e.InstanceId, count));
                        //MapElement.UpdateStatus(RuntimeState.Ready);
                    }
                    break;

                case RuntimeState.Completed:
                    // Find all resources except EventLog and PerfMon collectors
                    if (Resources.Where(r => r.Detail.ResourceType.RunsToCompletion()).All(r => r.MapElement.State == RuntimeState.Completed || r.MapElement.State == RuntimeState.Halted || r.MapElement.State == RuntimeState.Error))
                    {
                        // Trigger the complete event if all resources are in a complete state.
                        MapElement.UpdateStatus("Completed", RuntimeState.Completed);

                        if (OnResourcesComplete != null)
                        {
                            OnResourcesComplete(this, null);
                        }
                    }
                    break;

                case RuntimeState.Offline:
                    if (_resourcesShutdownWaitHandle != null)
                    {
                        // Signal that this resource is shutdown, the latch will ensure each resource only
                        // signals once.  When the latch reaches zero, it will release.
                        _resourcesShutdownWaitHandle.Signal(e.InstanceId);
                        int count = _resourcesShutdownWaitHandle == null ? 0 : _resourcesShutdownWaitHandle.CurrentCount;
                        TraceFactory.Logger.Debug("{0}/{1} is Offline, {2} remaining".FormatWith(Machine.Name, e.InstanceId, count));
                    }
                    break;

                case RuntimeState.Paused:
                    if (_resourcesPausedWaitHandle != null)
                    {
                        _resourcesPausedWaitHandle.Signal(e.InstanceId);
                        int count = _resourcesPausedWaitHandle == null ? 0 : _resourcesPausedWaitHandle.CurrentCount;
                        TraceFactory.Logger.Debug("{0}/{1} is Paused, {2} remaining".FormatWith(Machine.Name, e.InstanceId, count));
                    }
                    else
                    {
                        // Since the wait handle is not active, set only this user's state to paused
                        Resources.First(x => x.Id.Equals(e.InstanceId)).MapElement.UpdateStatus("Paused", RuntimeState.Paused);
                    }
                    break;

                case RuntimeState.Running:
                    // Since the wait handle is not active, set only this user's state to running
                    Resources.First(x => x.Id.Equals(e.InstanceId)).MapElement.UpdateStatus("Running", RuntimeState.Running);
                    break;

                case RuntimeState.Halted:
                    Resources.First(x => x.Id.Equals(e.InstanceId)).MapElement.UpdateStatus("Halted", RuntimeState.Halted);
                    TraceFactory.Logger.Debug("{0}/{1} is Halted".FormatWith(Machine.Name, e.InstanceId));
                    break;

                case RuntimeState.SetupCompleted:
                    // Unblock this admin worker from it's prephase activity.
                    Resources.OfType<AdminWorkerInstance>().First(x => x.Id.Equals(e.InstanceId)).Release();
                    break;

                case RuntimeState.TeardownCompleted:
                    // Unblock this admin worker from it's prephase activity.
                    Resources.OfType<AdminWorkerInstance>().First(x => x.Id.Equals(e.InstanceId)).Release();
                    break;
            }
        }

        /// <summary>
        /// Handles any status update message that comes in for this host
        /// </summary>
        /// <param name="message">The message.</param>
        public void ChangeStatusMessage(string message)
        {
            // Update the status message for this client
            MapElement.UpdateStatus(message);
            ResetWaitEventsTimeout();
        }

        /// <summary>
        /// Initializes the shutdown wait handle.
        /// </summary>
        protected void InitializeShutdownWaitHandle()
        {
            TraceFactory.Logger.Debug("Resource count {0}".FormatWith(Resources.Count));
            _resourcesShutdownWaitHandle = new ResettableCountdownEvent(Resources.Count);
        }

        /// <summary>
        /// Initializes the wait handles.
        /// </summary>
        protected void InitializeWaitHandles()
        {
            _resourcesReadyWaitHandle = new ResettableCountdownEvent(Resources.Count);
            _registrationWaitHandle = new ResettableCountdownEvent(1);
        }

        /// <summary>
        /// Waits for resource to signal ready.
        /// </summary>
        protected void WaitForResourceToSignalReady()
        {
            try
            {
                MapElement.UpdateStatus("ReadyWait");
                _resourcesReadyWaitHandle.Wait(WaitTimeout);
                TraceFactory.Logger.Info("Ready signal received");
            }
            finally
            {
                _resourcesReadyWaitHandle = null;
            }
        }

        /// <summary>
        /// Waits for resources to shutdown.
        /// </summary>
        protected void WaitForResourcesToShutdown()
        {
            TimeSpan waitTimeout = TimeSpan.FromSeconds(60);
            if (GlobalSettings.Items.ContainsKey(Setting.VmShutdownWaitTimeoutInSeconds))
            {
                waitTimeout = TimeSpan.FromSeconds(int.Parse(GlobalSettings.Items[Setting.VmShutdownWaitTimeoutInSeconds]));
            }

            try
            {
                MapElement.UpdateStatus("ShutdownWait");
                _resourcesShutdownWaitHandle.Wait(waitTimeout);
                TraceFactory.Logger.Info("Offline signal received");
            }
            catch (TimeoutException ex)
            {
                // Log error, but continue
                TraceFactory.Logger.Error(ex);
            }
            finally
            {
                _resourcesShutdownWaitHandle = null;
            }
        }

        /// <summary>
        /// Waits for host to register.
        /// </summary>
        protected void WaitForClientControllerToRegister()
        {
            try
            {
                MapElement.UpdateStatus("RegWait");
                _registrationWaitHandle.Wait(WaitTimeout);
                TraceFactory.Logger.Info("Registration received");
                MapElement.UpdateStatus("Registered");
            }
            finally
            {
                _registrationWaitHandle = null;
            }
        }

        private void ResetWaitEventsTimeout()
        {
            if (_registrationWaitHandle != null)
            {
                _registrationWaitHandle.ResetTimeout();
            }

            if (_resourcesPausedWaitHandle != null)
            {
                _resourcesPausedWaitHandle.ResetTimeout();
            }

            if (_resourcesReadyWaitHandle != null)
            {
                _resourcesReadyWaitHandle.ResetTimeout();
            }

            if (_resourcesShutdownWaitHandle != null)
            {
                _resourcesShutdownWaitHandle.ResetTimeout();
            }
        }

        /// <summary>
        /// Registers the machine's endpoint with the dispatcher.
        /// </summary>
        /// <returns>A <see cref="SystemManifest"/> for the machine submitting the registration request.</returns>
        public SystemManifest Register()
        {
            TraceFactory.Logger.Debug("Received registration from {0}".FormatWith(Machine.Name));

            // If we're past the boot process, no need to try to release the block.  Just move on.
            if (_registrationWaitHandle != null)
            {
                // This entry is waiting for the machine to register, when it does, then release
                // the block and move on.
                _registrationWaitHandle.Signal(Machine.Name);
            }

            if (!Manifest.HostMachine.Equals(Machine.Name))
            {
                TraceFactory.Logger.Debug("Manifest machine is set to {0}, not {1}".FormatWith(Manifest.HostMachine, Machine.Name));
            }

            return Manifest;
        }

        /// <summary>
        /// Copies the logs.
        /// </summary>
        protected void CopyClientControllerLogs()
        {
            TraceFactory.Logger.Debug("Copying additional files from {0}".FormatWith(Machine.Name));
            LogFileDataCollection logFiles = new LogFileDataCollection();

            try
            {
                using (var clientController = ClientControllerServiceConnection.Create(Machine.Name))
                {
                    logFiles = clientController.Channel.GetLogFiles(Manifest.SessionId);
                }

            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Unable to capture client controller log files for {0} : {1}"
                    .FormatWith(Machine.Name, ex.ToString()));
            }

            if (GlobalSettings.IsDistributedSystem)
            {
                try
                {
                    using (var printMonitor = new WcfClient<IPrintMonitorService>(MessageTransferType.Http, WcfService.PrintMonitor.GetHttpUri(Machine.Name)))
                    {
                        logFiles.Append(printMonitor.Channel.GetLogFiles());
                    }
                }
                catch (Exception ex)
                {
                    TraceFactory.Logger.Error("Unable to capture print monitor log files for {0} : {1}"
                        .FormatWith(Machine.Name, ex.ToString()));
                }
            }

            TraceFactory.Logger.Debug("Done: {0}".FormatWith(Machine.Name));
            MapElement.UpdateStatus("Logs copied");
            //Write to Log Directory
            logFiles.Write(LogFileReader.DataLogPath(Manifest.SessionId));
        }
    }
}