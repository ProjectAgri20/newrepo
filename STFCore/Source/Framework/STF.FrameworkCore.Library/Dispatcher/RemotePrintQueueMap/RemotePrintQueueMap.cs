using System;
using System.Linq;
using System.Collections.ObjectModel;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using System.Threading.Tasks;
using HP.ScalableTest.Print;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Represent a map of remote print queues used in each test scenario.
    /// Contains a collection of <see cref="RemotePrintQueueElement"/>s to represent
    /// individual remote print queues.
    /// </summary>
    public class RemotePrintQueueMap : SessionMapObject
    {
        /// <summary>
        /// Gets the print queues for this map.
        /// </summary>
        [SessionMapElementCollection]
        public Collection<RemotePrintQueueElement> RemotePrintQueueElements { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePrintQueueMap"/> class
        /// </summary>
        /// <param name="config">The system manifest data.</param>
        public RemotePrintQueueMap(SystemManifestAgent config)
            : base(config, ElementType.RemotePrintQueues)
        {
            RemotePrintQueueElements = new Collection<RemotePrintQueueElement>();
            MapElement.ElementSubtype = "Remote Print Queue";
        }

        /// <summary>
        /// Builds all components in this map.
        /// </summary>
        /// <param name="loopState">State of the loop.</param>
        public override void Stage(ParallelLoopState loopState)
        {
            var allQueues = Configuration.ManifestSet.SelectMany(n => n.ActivityPrintQueues.Values.SelectMany(m => m.OfType<RemotePrintQueueInfo>()));
            foreach (RemotePrintQueueInfo queue in allQueues.GroupBy(n => n.PrintQueueId).Select(n => n.First()))
            {
                RemotePrintQueueElements.Add(new RemotePrintQueueElement(queue.QueueName, queue.ServerHostName));
            }

            try
            {
                Parallel.ForEach<RemotePrintQueueElement>(RemotePrintQueueElements, (h, l) => h.Stage(l));
                if (!SessionMapElement.AllElementsSetTo<RemotePrintQueueElement>(RemotePrintQueueElements, RuntimeState.Available))
                {
                    loopState.Break();
                    return;
                }

                MapElement.UpdateStatus("Available", RuntimeState.Available);
            }
            catch (AggregateException ex)
            {
                MapElement.UpdateStatus(RuntimeState.Error, "Staging error", ex);
                throw;
            }
        }

        /// <summary>
        /// Initializes all components in this map.
        /// </summary>
        /// <param name="loopState">State of the loop.</param>
        public override void Validate(ParallelLoopState loopState)
        {
            try
            {
                MapElement.UpdateStatus("Validating", RuntimeState.Validating);
                
                Parallel.ForEach<RemotePrintQueueElement>(RemotePrintQueueElements, (h, l) => h.Validate(l));
                
                if (SessionMapElement.AnyElementsSetTo(RemotePrintQueueElements, RuntimeState.Error, RuntimeState.AggregateError))
                {
                    TraceFactory.Logger.Error("Some elements not validated");
                    MapElement.UpdateStatus(RuntimeState.AggregateError);
                    loopState.Break();
                    return;
                }

                MapElement.UpdateStatus("Validated", RuntimeState.Validated);
            }
            catch (AggregateException ex)
            {
                MapElement.UpdateStatus(RuntimeState.Error, "Validation error", ex);
                throw;
            }
        }

        /// <summary>
        /// Initializes all components in this map.
        /// </summary>
        /// <param name="loopState">State of the loop.</param>
        public override void Revalidate(ParallelLoopState loopState)
        {
            try
            {
                MapElement.UpdateStatus("Validating", RuntimeState.Validating);
                
                Parallel.ForEach<RemotePrintQueueElement>(RemotePrintQueueElements, (h, l) => h.Validate(l));

                if (SessionMapElement.AnyElementsSetTo(RemotePrintQueueElements, RuntimeState.Error, RuntimeState.AggregateError))
                {
                    TraceFactory.Logger.Error("Some elements not validated");
                    MapElement.UpdateStatus(RuntimeState.AggregateError);
                    loopState.Break();
                    return;
                }

                MapElement.UpdateStatus("Validated", RuntimeState.Validated);
            }
            catch (AggregateException ex)
            {
                MapElement.UpdateStatus(RuntimeState.Error, "Validation error", ex);
                throw;
            }
        }

        /// <summary>
        /// Turns on all components in this map.
        /// </summary>
        /// <param name="loopState">State of the loop.</param>
        public override void PowerUp(ParallelLoopState loopState)
        {
            try
            {
                MapElement.UpdateStatus("Starting", RuntimeState.Starting);
                
                Parallel.ForEach<RemotePrintQueueElement>(RemotePrintQueueElements, (h, l) => h.PowerUp(l));
                
                if (!SessionMapElement.AllElementsSetTo<RemotePrintQueueElement>(RemotePrintQueueElements, RuntimeState.Ready))
                {
                    loopState.Break();
                }

                MapElement.UpdateStatus("Ready", RuntimeState.Ready);
            }
            catch (AggregateException ex)
            {
                MapElement.UpdateStatus(RuntimeState.Error, "Power up error", ex);
                throw;
            }
        }

        /// <summary>
        /// Runs all components in this map.
        /// </summary>
        /// <param name="loopState">State of the loop.</param>
        public override void Run(ParallelLoopState loopState)
        {
            try
            {
                Parallel.ForEach<RemotePrintQueueElement>(RemotePrintQueueElements, (h, l) => h.Run(l));
                
                if (!SessionMapElement.AllElementsSetTo<RemotePrintQueueElement>(RemotePrintQueueElements, RuntimeState.Running))
                {
                    loopState.Break();
                }

                MapElement.UpdateStatus("Running", RuntimeState.Running);
            }
            catch (AggregateException ex)
            {
                MapElement.UpdateStatus(RuntimeState.Error, "Run error", ex);
                throw;
            }
        }

        /// <summary>
        /// Runs all components in this map.
        /// </summary>
        /// <param name="loopState">State of the loop.</param>
        public override void Repeat(ParallelLoopState loopState)
        {
            try
            {
                Parallel.ForEach<RemotePrintQueueElement>(RemotePrintQueueElements, (h, l) => h.Run(l));
                MapElement.UpdateStatus("Running", RuntimeState.Running);
            }
            catch (AggregateException ex)
            {
                MapElement.UpdateStatus(RuntimeState.Error, "Run error", ex);
                throw;
            }
        }

        /// <summary>
        /// Marks this map complete
        /// </summary>
        public void Finished()
        {
            // Spin through the hosts and mark them complete first.
            Parallel.ForEach<RemotePrintQueueElement>(RemotePrintQueueElements, h => h.Completed());
            MapElement.UpdateStatus("Completed", RuntimeState.Completed);
        }

        /// <summary>
        /// Shuts down all maps using the specified options
        /// </summary>
        /// <param name="options">The shutdown options (unused).</param>
        /// <param name="loopState">State of the loop.</param>
        public override void Shutdown(ShutdownOptions options, ParallelLoopState loopState)
        {
            try
            {
                MapElement.UpdateStatus("Shutdown", RuntimeState.ShuttingDown);
                
                Parallel.ForEach<RemotePrintQueueElement>(RemotePrintQueueElements, (h, l) => h.Shutdown(options, l));
                
                if (!SessionMapElement.AllElementsSetTo<RemotePrintQueueElement>(RemotePrintQueueElements, RuntimeState.Offline))
                {
                    loopState.Break();
                }

                MapElement.UpdateStatus("Offline", RuntimeState.Offline);
            }
            catch (AggregateException ex)
            {
                MapElement.UpdateStatus(RuntimeState.Error, "Shutdown error", ex);
                throw;
            }
        }

        /// <summary>
        /// Pauses all components in this map.
        /// </summary>
        public override void Pause()
        {
            if (MapElement.State == RuntimeState.Running)
            {
                Parallel.ForEach<RemotePrintQueueElement>(RemotePrintQueueElements, h => h.Pause());
                MapElement.UpdateStatus("Paused", RuntimeState.Paused);
            }
        }

        /// <summary>
        /// Resumes all components in this map.
        /// </summary>
        public override void Resume()
        {
            if (MapElement.State == RuntimeState.Paused)
            {
                Parallel.ForEach<RemotePrintQueueElement>(RemotePrintQueueElements, h => h.Resume());
                MapElement.UpdateStatus("Running", RuntimeState.Running);
            }
        }
    }
}
