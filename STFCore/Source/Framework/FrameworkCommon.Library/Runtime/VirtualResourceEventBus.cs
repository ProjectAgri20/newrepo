using System;
using System.Threading.Tasks;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Enables eventing for virtual resources.
    /// </summary>
    public static class VirtualResourceEventBus
    {
        private static readonly object _objectLock = new object();

        /// <summary>
        /// Occurs when is ready to start execution.
        /// </summary>
        public static event EventHandler<VirtualResourceEventBusRunArgs> OnStartMainRun;

        /// <summary>
        /// Occurs when ready to start the pre process step.
        /// </summary>
        public static event EventHandler<VirtualResourceEventBusRunArgs> OnStartSetup;

        /// <summary>
        /// Occurs when [configuration start post process].
        /// </summary>
        public static event EventHandler<VirtualResourceEventBusRunArgs> OnStartTeardown;

        /// <summary>
        /// Occurs when the VirtualResource processing must be suspended.
        /// </summary>
        public static event EventHandler OnPauseResource;

        /// <summary>
        /// Occurs when the VirtualResource can resume processing.
        /// </summary>
        public static event EventHandler OnResumeResource;
     
        /// <summary>
        /// Occurs when the VirtualResource has completed processing and is exiting.
        /// </summary>
        public static event EventHandler<VirtualResourceEventBusShutdownArgs> OnShutdownResource;

        /// <summary>
        /// Occurs when the VirtualResource processing must stop.
        /// </summary>
        public static event EventHandler OnHaltResource;

        /// <summary>
        /// Occurs when all activities to an asset must be suspended.
        /// </summary>
        public static event EventHandler<VirtualResourceEventBusAssetArgs> OnTakeOffline;

        /// <summary>
        /// Occurs when activities to an asset can resume processing.
        /// </summary>
        public static event EventHandler<VirtualResourceEventBusAssetArgs> OnBringOnline;

        /// <summary>
        /// Occurs when a synchronization event signal is received.
        /// </summary>
        public static event EventHandler<VirtualResourceEventBusSignalArgs> OnSignalSynchronizationEvent;

        /// <summary>
        /// Occurs when the user is ready to resume the registration process.
        /// </summary>
        public static event EventHandler OnReadyToRegister;

        /// <summary>
        /// Fires the start virtual resource event will tells the listener to start the Virtual Resource.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void StartMainRun(object sender)
        {
            if (OnStartMainRun != null)
            {
                OnStartMainRun(sender, new VirtualResourceEventBusRunArgs(ResourceExecutionPhase.Main));
            }
        }

        /// <summary>
        /// Triggers the event to start the pre run phase
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void StartSetup(object sender)
        {
            if (OnStartSetup != null)
            {
                OnStartSetup(sender, new VirtualResourceEventBusRunArgs(ResourceExecutionPhase.Setup));
            }
        }

        /// <summary>
        /// Triggers the event to start the post run phase
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void StartTeardown(object sender)
        {
            if (OnStartTeardown != null)
            {
                OnStartTeardown(sender, new VirtualResourceEventBusRunArgs(ResourceExecutionPhase.Teardown));
            }
        }

        /// <summary>
        /// Fires the pause virtual resource event which tells the listener to pause the Virtual Resource.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void PauseResource(object sender)
        {
            lock (_objectLock)
            {
                if (OnPauseResource != null)
                {
                    OnPauseResource(sender, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Fires the resume virtual resource event which tells the listener to resume the Virtual Resource.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void ResumeResource(object sender)
        {
            lock (_objectLock)
            {
                if (OnResumeResource != null)
                {
                    OnResumeResource(sender, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Fires the exit virtual resource event which tells the listener to stop the Virtual Resource.
        /// </summary>
        /// <param name="sender">The sender.</param>
        public static void HaltResource(object sender)
        {
            lock (_objectLock)
            {
                if (OnHaltResource != null)
                {
                    OnHaltResource(sender, new EventArgs());
                }
            }
        }

        /// <summary>
        /// Fires the take offline event which tells the listener to stop all activities to the Asset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="assetId"></param>
        public static void TakeOffline(object sender, string assetId)
        {
            lock (_objectLock)
            {
                if (OnHaltResource != null)
                {
                    OnTakeOffline(sender, new VirtualResourceEventBusAssetArgs(assetId));
                }
            }
        }

        /// <summary>
        /// Fires the bring online event which tells the listener to resume activities to the Asset.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="assetId"></param>
        public static void BringOnline(object sender, string assetId)
        {
            lock (_objectLock)
            {
                if (OnHaltResource != null)
                {
                    OnBringOnline(sender, new VirtualResourceEventBusAssetArgs(assetId));
                }
            }
        }

        /// <summary>
        /// Fires the OnSignalSynchronizationEvent that broadcasts a synchronization event.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="eventName">The event name.</param>
        public static void SignalSynchronizationEvent(object sender, string eventName)
        {
            OnSignalSynchronizationEvent?.Invoke(sender, new VirtualResourceEventBusSignalArgs(eventName));
        }

        /// <summary>
        /// Fires the shutdown resource event which tells the listener to shut down the Virtual Resource.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="copyLogs"></param>
        public static void ShutdownResource(object sender, bool copyLogs)
        {
            lock (_objectLock)
            {
                Task.Factory.StartNew(() =>
                {
                    if (OnShutdownResource != null)
                    {
                        OnShutdownResource(sender, new VirtualResourceEventBusShutdownArgs(copyLogs));
                    }
                });
            }
        }

        /// <summary>
        /// Fires the ready to register event which notifies the listener that resource is ready to register with the dispatcher.
        /// </summary>
        /// <param name="sender"></param>
        public static void ReadyToRegister(object sender)
        {
            lock (_objectLock)
            {
                if (OnReadyToRegister != null)
                {
                    OnReadyToRegister(sender, new EventArgs());
                }
            }
        }
    }
}
