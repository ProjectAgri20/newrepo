using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework.Runtime;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// This automation map supports any machines running automation components.  This will typically
    /// be Virtual Machines and the <see cref="WindowsMachineVirtual"/> objects supports them.  It can
    /// also be physical machines.
    /// </summary>
    internal class ResourceMap : SessionMapObject
    {
        private readonly ManagedMachineResourcePool _resourcePool = null;

        /// <summary>
        /// Gets the collection of <see cref="ResourceHost"/>s that represent the machines running
        /// in this machine map.
        /// </summary>
        [SessionMapElementCollection]
        public Collection<ResourceHost> Hosts { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ResourceMap"/> class.
        /// </summary>
        /// <param name="agent">The system manifest data.</param>
        public ResourceMap(SystemManifestAgent agent)
            : base(agent, ElementType.Workers, "Resources")
        {
            Hosts = new Collection<ResourceHost>();
            MapElement.ElementSubtype = "VirtualResource";

            _resourcePool = new ManagedMachineResourcePool(agent);
        }

        private void ResourceHostsCompleted(object sender, EventArgs e)
        {
            // Find all of the hosts that are not running an Event Log or a PerfMon collector
            if (Hosts.SelectMany(r => r.Resources).Where(r => r.Detail.ResourceType.RunsToCompletion()).All(r => r.MapElement.State == RuntimeState.Completed))
            {
                MapElement.UpdateStatus("Completed", RuntimeState.Completed);
                NotifySessionMapCompleted();
            }
        }

        /// <summary>
        /// Builds all components in this map.
        /// </summary>
        public override void Stage(ParallelLoopState loopState)
        {
            Thread.CurrentThread.SetName("Stage-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            TraceFactory.Logger.Debug("Entering...");

            // Use the index to create an initial unique hostname that will be 
            // replaced during the validation stage with an actual hostname.
            foreach (var manifest in Configuration.ManifestSet)
            {
                ResourceHost host = new ResourceHost(manifest);
                host.OnResourcesComplete += new EventHandler(ResourceHostsCompleted);
                Hosts.Add(host);
            }

            try
            {
                Parallel.ForEach<ResourceHost>(Hosts, (h, l) => h.Stage(l));
                if (!SessionMapElement.AllElementsSetTo<ResourceHost>(Hosts, RuntimeState.Available))
                {
                    loopState.Break();
                    return;
                }
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, then throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Staging error", ex);
                throw;
            }
        }

        /// <summary>
        /// Initializes all components in this map.
        /// </summary>
        public override void Revalidate(ParallelLoopState loopState)
        {
            Thread.CurrentThread.SetName("Revalidate-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            // If we got into a hard error state (not an aggregate error), we won't be able to recover
            if (MapElement.State == RuntimeState.Error)
            {
                MapElement.UpdateStatus();
                return;
            }

            MapElement.UpdateStatus("Validating", RuntimeState.Validating);

            try
            {
                Parallel.ForEach<ResourceHost>(Hosts, (h, l) => h.Revalidate(l));
                if (!SessionMapElement.AllElementsSetTo<ResourceHost>(Hosts, RuntimeState.Validated))
                {
                    MapElement.UpdateStatus(RuntimeState.AggregateError);
                    loopState.Break();
                    return;
                }

                MapElement.UpdateStatus("Validated", RuntimeState.Validated);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, then throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Validation error", ex);
                throw;
            }
        }

        /// <summary>
        /// Initializes all components in this map.
        /// </summary>
        public override void Validate(ParallelLoopState loopState)
        {
            Thread.CurrentThread.SetName("Validate-{0}".FormatWith(Thread.CurrentThread.ManagedThreadId));

            TraceFactory.Logger.Debug("Entering...");

            MapElement.UpdateStatus("Validating", RuntimeState.Validating);

            try
            {
                TraceFactory.Logger.Debug("Reserving required VMs");

                // Try to allocate all the needed VMs if this fails then an error
                // state will be sent back to the client.
                _resourcePool.ReserveMachines();
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error(ex);
                MapElement.UpdateStatus(ex.Message, RuntimeState.Error);
                loopState.Break();
                return;
            }

            try
            {
                // First assign a machine instance to each host.  This needs to be done before
                // calling validate on each host.
                foreach (var host in Hosts)
                {
                    TraceFactory.Logger.Debug("Assigning machine {0}".FormatWith(host.Machine.Name));
                    _resourcePool.AssignMachine(host);
                }

                Parallel.ForEach<ResourceHost>(Hosts, (h, l) => h.Validate(l));

                // Both "Validated" and "Warning" are valid states to allow the system to continue.
                if (!SessionMapElement.AllElementsSetTo<ResourceHost>(Hosts, RuntimeState.Validated, RuntimeState.Warning))
                {
                    MapElement.UpdateStatus(RuntimeState.AggregateError);
                    loopState.Break();
                    return;
                }

                MapElement.UpdateStatus("Validated", RuntimeState.Validated);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, then throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Validation error", ex);
                throw;
            }
        }

        /// <summary>
        /// Turns on all resource hosts in this map.
        /// </summary>
        public override void PowerUp(ParallelLoopState loopState)
        {
            try
            {
                MapElement.UpdateStatus("Starting", RuntimeState.Starting);

                TraceFactory.Logger.Debug("Calling PowerUp() in parallel on each Host"); 
                Parallel.ForEach<ResourceHost>(Hosts, (h, l) => h.PowerUp(l));
                if (!SessionMapElement.AllElementsSetTo<ResourceHost>(Hosts, RuntimeState.Ready))
                {
                    MapElement.UpdateStatus(RuntimeState.Error);
                    loopState.Break();
                    return;
                }

                MapElement.UpdateStatus("Ready", RuntimeState.Ready);
            }
            catch (AggregateException ex)
            {
                TraceFactory.Logger.Error(ex.Message);
                // Log the exception at this element level, then throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Power up error", ex);
                throw;
            }
        }

        /// <summary>
        /// Runs all resource hosts in this map.
        /// </summary>
        public override void Run(ParallelLoopState loopState)
        {
            try
            {
                Parallel.ForEach<ResourceHost>(Hosts, (h, l) => h.Run(l));
                MapElement.UpdateStatus("Running", RuntimeState.Running);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, then throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Run error", ex);
                throw;
            }
        }

        /// <summary>
        /// Runs all resource hosts in this map.
        /// </summary>
        public override void Repeat(ParallelLoopState loopState)
        {
            try
            {
                Parallel.ForEach<ResourceHost>(Hosts, (h, l) => h.Repeat(l));
                MapElement.UpdateStatus("Running", RuntimeState.Running);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, then throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Run error", ex);
                throw;
            }
        }

        /// <summary>
        /// Shuts down all resource hosts in this map using the specified options
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="loopState">State of the loop.</param>
        public override void Shutdown(ShutdownOptions options, ParallelLoopState loopState)
        {
            try
            {
                MapElement.UpdateStatus("Shutdown", RuntimeState.ShuttingDown);

                Parallel.ForEach<ResourceHost>(Hosts, (h, l) => h.Shutdown(options, l));
                MapElement.UpdateStatus("Offline", RuntimeState.Offline);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, then throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Shutdown error", ex);
                throw;
            }
        }

        /// <summary>
        /// Pauses all resource hosts in this map
        /// </summary>
        public override void Pause()
        {
            try
            {
                if (MapElement.State == RuntimeState.Running)
                {
                    // Only apply a pause to the resources that are still running.  Others in a different state
                    // will be ignored.  Then only have the wait handle wait for those resources that were chosen
                    var runningHosts = Hosts.Where(e => e.MapElement.State == RuntimeState.Running);
                    if (runningHosts.Count() > 0)
                    {
                        MapElement.UpdateStatus("Pausing", RuntimeState.Pausing);
                        Parallel.ForEach<ResourceHost>(runningHosts, h => h.Pause());
                    }

                    MapElement.UpdateStatus("Paused", RuntimeState.Paused);
                }
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, then throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Pause error", ex);
                throw;
            }
        }

        /// <summary>
        /// Resumes all components in this map.
        /// </summary>
        public override void Resume()
        {
            try
            {
                if (MapElement.State == RuntimeState.Paused)
                {
                    MapElement.UpdateStatus("Resuming");
                    Parallel.ForEach<ResourceHost>(Hosts, h => h.Resume());
                    MapElement.UpdateStatus("Running", RuntimeState.Running);
                }
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, then throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Resume error", ex);
                throw;
            }
        }

        /// <summary>
        /// Sends a synchronization signal with the specified event name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public override void SignalSynchronizationEvent(string eventName)
        {
            try
            {
                Parallel.ForEach(Hosts, n => n.SignalSynchronizationEvent(eventName));
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, then throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Sync point error", ex);
                throw;
            }
        }
    }
}