using System;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Runtime;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Represents the root of the session map.  The session map is a class hierarchy that 
    /// maps to the real resources (virtual machines, etc.) and assets (devices, etc) that are
    /// being used in the execution of a test scenario.
    /// </summary>
    public abstract class SessionMapObject : ISessionMapElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SessionMapObject"/> class.
        /// </summary>
        /// <param name="config">The system manifest information.</param>
        /// <param name="type">The type of element, used by the <see cref="SessionMapElement"/>.</param>
        public SessionMapObject(SystemManifestAgent config, ElementType type)
        {
            Configuration = config;
            MapElement = new SessionMapElement(type.ToString(), type);
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionMapObject"/> class.
        /// </summary>
        /// <param name="config">The configuration.</param>
        /// <param name="type">The type.</param>
        /// <param name="elementName">Name of the element.</param>
        public SessionMapObject(SystemManifestAgent config, ElementType type, string elementName)
        {
            Configuration = config;
            MapElement = new SessionMapElement(elementName, type);
        }

        /// <summary>
        /// Gets the element map for this item
        /// </summary>
        /// <value>
        /// The <see cref="SessionMapElement"/> information for this map.
        /// </value>
        public SessionMapElement MapElement { get; private set; }

        /// <summary>
        /// Gets the manifest agent containing all manifest data for this scenario.
        /// </summary>
        /// <value>
        /// The <see cref="SystemManifestAgent"/> object containing all manifest information
        /// </value>
        public SystemManifestAgent Configuration { get; private set; }

        /// <summary>
        /// Occurs when all chidren of this automation map have completed their run.
        /// </summary>
        public event EventHandler SessionMapCompleted;

        /// <summary>
        /// Builds all components in this map.
        /// </summary>
        public abstract void Stage(ParallelLoopState loopState);

        /// <summary>
        /// Initializes all components in this map.
        /// </summary>
        public abstract void Validate(ParallelLoopState loopState);

        /// <summary>
        /// Initializes all components in this map.
        /// </summary>
        public abstract void Revalidate(ParallelLoopState loopState);

        /// <summary>
        /// Turns on all components in this map.
        /// </summary>
        public abstract void PowerUp(ParallelLoopState loopState);

        /// <summary>
        /// Runs all components in this map.
        /// </summary>
        public abstract void Run(ParallelLoopState loopState);

        /// <summary>
        /// Runs all components in this map.
        /// </summary>
        public abstract void Repeat(ParallelLoopState loopState);

        /// <summary>
        /// Shuts down all maps using the specified options
        /// </summary>
        /// <param name="options">The options.</param>
        /// <param name="loopState">State of the loop.</param>
        public abstract void Shutdown(ShutdownOptions options, ParallelLoopState loopState);

        /// <summary>
        /// Pauses all components in this map.
        /// </summary>
        public virtual void Pause()
        {
            MapElement.UpdateStatus("Paused", RuntimeState.Paused);
        }

        /// <summary>
        /// Resumes all components in this map.
        /// </summary>
        public virtual void Resume()
        {
            MapElement.UpdateStatus("Running", RuntimeState.Running);
        }

        /// <summary>
        /// Sends a synchronization signal with the specified event name.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public virtual void SignalSynchronizationEvent(string eventName)
        {
            // Do nothing by default.
        }

        /// <summary>
        /// Notifies listeners that the session map is complete.
        /// </summary>
        protected void NotifySessionMapCompleted()
        {
            MapElement.UpdateStatus("Complete", RuntimeState.Completed);

            if (SessionMapCompleted != null)
            {
                SessionMapCompleted(this, null);
            }
        }
    }
}