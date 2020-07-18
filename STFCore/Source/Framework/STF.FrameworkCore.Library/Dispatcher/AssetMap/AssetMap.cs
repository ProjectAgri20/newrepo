using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Represents the base class for all assets used in a running test scenario.  Contains
    /// a collection of <see cref="AssetHost"/>s that are used to host the running assets.
    /// These <see cref="AssetHost"/>s may be virtual machines to host simulators, or 
    /// physical devices, etc.
    /// </summary>
    public class AssetMap : SessionMapObject
    {
        /// <summary>
        /// Gets the hosts for this asset map.
        /// </summary>
        [SessionMapElementCollection]
        public Collection<AssetHost> Hosts { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetMap"/> class.
        /// </summary>
        /// <param name="config">The system manifest data.</param>
        public AssetMap(SystemManifestAgent config)
            : base(config, ElementType.Assets)
        {
            Hosts = new Collection<AssetHost>();
            MapElement.ElementSubtype = "Asset";
        }

        /// <summary>
        /// Builds all components in this map.
        /// </summary>
        public override void Stage(ParallelLoopState loopState)
        {
            foreach (var asset in Configuration.ManifestSet.SelectMany(x => x.Assets.Devices).Distinct())
            {
                var className = string.Empty;
                var attribute =
                (
                    from a in asset.GetType().GetCustomAttributes(true)
                    where a.GetType() == typeof(AssetHostAttribute)
                    select a as AssetHostAttribute

                ).FirstOrDefault();

                if (attribute != null)
                {
                    className = "{0}.{1}".FormatWith(GetType().Namespace, attribute.ClassName);
                }
                else
                {
                    var assetName = asset.GetType().Name;
                    var exception = new DispatcherOperationException("Invalid AssetHost class for {0}".FormatWith(assetName));
                    MapElement.UpdateStatus(RuntimeState.Error, "Invalid asset: ".FormatWith(assetName), exception);
                }
                TraceFactory.Logger.Debug(string.Format("Type:{0} Asset: {1}", className, asset.Description));

                var assetHost = (AssetHost)Activator.CreateInstance(Type.GetType(className), new object[] { asset });
                TraceFactory.Logger.Debug("Adding {0}".FormatWith(assetHost.Asset.AssetId));
                Hosts.Add(assetHost);
            }

            try
            {
                Parallel.ForEach<AssetHost>(Hosts, (h, l) => h.Stage(l));
                if (!SessionMapElement.AllElementsSetTo<AssetHost>(Hosts, RuntimeState.Available))
                {
                    loopState.Break();
                    return;
                }

                MapElement.UpdateStatus("Available", RuntimeState.Available);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, the throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Staging error", ex);
                throw;
            }

        }

        /// <summary>
        /// Revalidates the state
        /// </summary>
        /// <param name="loopState"></param>
        public override void Revalidate(ParallelLoopState loopState)
        {
            try
            {
                MapElement.UpdateStatus("Validating", RuntimeState.Validating);
                Parallel.ForEach<AssetHost>(Hosts, (h, l) => h.Revalidate(l)); // Used to call Validate()
                if (SessionMapElement.AnyElementsSetTo(Hosts, RuntimeState.Error, RuntimeState.AggregateError))
                {
                    TraceFactory.Logger.Debug("Some elements not validated");
                    MapElement.UpdateStatus(RuntimeState.AggregateError);
                    loopState.Break();
                    return;
                }

                MapElement.UpdateStatus("Validated", RuntimeState.Validated);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, the throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Validation error", ex);
                throw;
            }
        }

        /// <summary>
        /// Initializes all components in this map.
        /// </summary>
        public override void Validate(ParallelLoopState loopState)
        {
            try
            {
                MapElement.UpdateStatus("Validating", RuntimeState.Validating);
                Parallel.ForEach<AssetHost>(Hosts, (h, l) => h.Validate(l));

                if (SessionMapElement.AnyElementsSetTo(Hosts, RuntimeState.Error, RuntimeState.AggregateError))
                {
                    TraceFactory.Logger.Debug("Some elements not validated");
                    MapElement.UpdateStatus(RuntimeState.AggregateError);
                    loopState.Break();
                    return;
                }

                MapElement.UpdateStatus("Validated", RuntimeState.Validated);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, the throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Validation error", ex);
                throw;
            }
        }

        /// <summary>
        /// Turns on all components in this map.
        /// </summary>
        public override void PowerUp(ParallelLoopState loopState)
        {
            try
            {
                TraceFactory.Logger.Debug("Starting...");
                MapElement.UpdateStatus("Starting", RuntimeState.Starting);
                Parallel.ForEach<AssetHost>(Hosts, (h, l) => h.PowerUp(l));
                if (!SessionMapElement.AllElementsSetTo<AssetHost>(Hosts, RuntimeState.Ready))
                {
                    loopState.Break();
                }

                MapElement.UpdateStatus("Ready", RuntimeState.Ready);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, the throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Power up error", ex);
                throw;
            }
        }

        /// <summary>
        /// Runs all components in this map.
        /// </summary>
        public override void Run(ParallelLoopState loopState)
        {
            try
            {
                Parallel.ForEach<AssetHost>(Hosts, (h, l) => h.Run(l));
                if (!SessionMapElement.AllElementsSetTo<AssetHost>(Hosts, RuntimeState.Running))
                {
                    loopState.Break();
                }

                MapElement.UpdateStatus("Running", RuntimeState.Running);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, the throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Run error", ex);
                throw;
            }
        }

        /// <summary>
        /// Runs all components in this map.
        /// </summary>
        public override void Repeat(ParallelLoopState loopState)
        {
            try
            {
                Parallel.ForEach<AssetHost>(Hosts, (h, l) => h.Run(l));
                MapElement.UpdateStatus("Running", RuntimeState.Running);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, the throw again to catch it higher
                MapElement.UpdateStatus(RuntimeState.Error, "Run error", ex);
                throw;
            }
        }

        /// <summary>
        /// Marks this asset complete
        /// </summary>
        public void Finished()
        {
            // Spin through the hosts and mark them complete first.
            Parallel.ForEach<AssetHost>(Hosts, h => h.Completed());
            MapElement.UpdateStatus("Completed", RuntimeState.Completed);
        }

        /// <summary>
        /// Shuts down all maps
        /// </summary>
        public override void Shutdown(ShutdownOptions options, ParallelLoopState loopState)
        {
            // The shutdown options are not used by the asset map so just ignore them
            try
            {
                MapElement.UpdateStatus("Shutdown", RuntimeState.ShuttingDown);

                Parallel.ForEach<AssetHost>(Hosts, (h, l) => h.Shutdown(options, l));
                if (!SessionMapElement.AllElementsSetTo<AssetHost>(Hosts, RuntimeState.Offline))
                {
                    loopState.Break();
                }

                MapElement.UpdateStatus("Offline", RuntimeState.Offline);
            }
            catch (AggregateException ex)
            {
                // Log the exception at this element level, the throw again to catch it higher
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
                Parallel.ForEach<AssetHost>(Hosts, h => h.Pause());
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
                Parallel.ForEach<AssetHost>(Hosts, h => h.Resume());
                MapElement.UpdateStatus("Running", RuntimeState.Running);
            }
        }

    }
}