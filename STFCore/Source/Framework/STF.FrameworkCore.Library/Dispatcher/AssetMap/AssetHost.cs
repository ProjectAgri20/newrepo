using System;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Base class that represents any host used to support an asset used in a test session.
    /// </summary>
    public class AssetHost : ISessionMapElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AssetHost" /> class.
        /// </summary>
        /// <param name="asset">The asset data.</param>
        /// <param name="name">The name of this host.</param>
        /// <param name="type">The type of status element.</param>
        /// <param name="subtype">The subtype.</param>
        public AssetHost(AssetDetail asset, string name, ElementType type, string subtype)
        {
            Asset = asset;
            MapElement = new SessionMapElement(name, type, subtype);
            AssetMemoryRetrieved = DateTime.Now.AddDays(-1);
        }

        /// <summary>
        /// Gets the <see cref="AssetDetail"/> associated with this host.
        /// </summary>
        /// <value>
        /// The asset.
        /// </value>
        public AssetDetail Asset { get; private set; }

        /// <summary>
        /// Gets or sets the last time the asset's memory was retrieved.
        /// The initial value is one day old.
        /// </summary>
        /// <value>
        /// The asset memory retrieved.
        /// </value>
        public DateTime AssetMemoryRetrieved { get; set; }

        /// <summary>
        /// Gets the <see cref="SessionMapElement"/> object for this host.
        /// </summary>
        /// <value>
        /// The status.
        /// </value>
        public SessionMapElement MapElement { get; private set; }

        /// <summary>
        /// Gets the name associated with this host.
        /// </summary>
        public virtual string AssetName
        {
            get { return Asset.AssetId; }
        }

        /// <summary>
        /// Handles the runtime error for this host.
        /// </summary>
        /// <param name="error">The error information.</param>
        public virtual bool HandleError(RuntimeError error)
        {
            // Implemented as needed by child classes
            return true;
        }

        public virtual bool CanResumeActivity()
        {
            // Implemented as needed by child classes
            return true;
        }

        /// <summary>
        /// Restarts this asset.
        /// </summary>
        public virtual void Restart()
        {
            // Implemented as needed by child classes
        }

        /// <summary>
        /// Builds all data structures for this asset.
        /// </summary>
        public virtual void Stage(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Available", RuntimeState.Available);
        }

        /// <summary>
        /// Revalidates this asset host
        /// </summary>
        public virtual void Validate(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Validated", RuntimeState.Validated);
        }

        /// <summary>
        /// Validates this asset host
        /// </summary>
        public virtual void Revalidate(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Validated", RuntimeState.Validated);
        }

        /// <summary>
        /// Turns on this asset host (bootup, power on, etc.).
        /// </summary>
        public virtual void PowerUp(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Ready", RuntimeState.Ready);
        }

        /// <summary>
        /// Executes this asset host, which may mean different things.
        /// </summary>
        public virtual void Run(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Running", RuntimeState.Running);
        }

        /// <summary>
        /// Marks this asset complete
        /// </summary>
        public virtual void Completed()
        {
            if (MapElement.State != RuntimeState.Offline)
            {
                MapElement.UpdateStatus("Completed", RuntimeState.Completed);
            }
        }

        /// <summary>
        /// Shuts down this asset host
        /// </summary>
        public virtual void Shutdown(ShutdownOptions options, ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Offline", RuntimeState.Offline);
        }

        /// <summary>
        /// Pauses this asset host
        /// </summary>
        public virtual void Pause()
        {
            MapElement.UpdateStatus("Paused", RuntimeState.Paused);
        }

        /// <summary>
        /// Resumes this asset host
        /// </summary>
        public virtual void Resume()
        {
            MapElement.UpdateStatus("Running", RuntimeState.Running);
        }

        /// <summary>
        /// Suspends operations to this asset host
        /// </summary>
        public virtual void TakeOffline()
        {
            MapElement.UpdateStatus("Offline", RuntimeState.Offline);
        }

        /// <summary>
        /// Sets the status to Running
        /// </summary>
        public void BringOnline()
        {
            MapElement.UpdateStatus("Running", RuntimeState.Running);
        }
    }
}