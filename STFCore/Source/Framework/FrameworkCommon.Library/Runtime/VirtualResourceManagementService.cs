using System.Threading;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Service for managing virtual resources.
    /// </summary>
    public class VirtualResourceManagementService : IVirtualResourceManagementService
    {
        /// <summary>
        /// Shutdowns this instance of the activity console.
        /// </summary>
        public void Shutdown(bool copyLogs)
        {
            TraceFactory.Logger.Info("Request to shutdown");
            VirtualResourceEventBus.ShutdownResource(this, copyLogs);
        }

        /// <summary>
        /// Gets the Log Files
        /// </summary>
        /// <returns></returns>
        public LogFileDataCollection GetLogFiles()
        {
            return LogFileDataCollection.Create(LogFileReader.DataLogPath());
        }

        /// <summary>
        /// Notifies the activity console to begin activities
        /// </summary>
        public void StartMainRun()
        {
            TraceFactory.Logger.Info("Request to start main run activities");
            ThreadPool.QueueUserWorkItem(t =>
            {
                Thread.CurrentThread.SetName("Start");
                VirtualResourceEventBus.StartMainRun(this);
            });
        }

        /// <summary>
        /// Notifies the activity console to begin activities
        /// </summary>
        public void StartSetup()
        {
            TraceFactory.Logger.Info("Request to start pre run activities");
            ThreadPool.QueueUserWorkItem(t =>
            {
                Thread.CurrentThread.SetName("Setup");
                VirtualResourceEventBus.StartSetup(this);
            });
        }

        /// <summary>
        /// Notifies the activity console to begin activities
        /// </summary>
        public void StartTeardown()
        {
            TraceFactory.Logger.Info("Request to start post run activity");
            ThreadPool.QueueUserWorkItem(t =>
            {
                Thread.CurrentThread.SetName("Teardown");
                VirtualResourceEventBus.StartTeardown(this);
            });
        }

        /// <summary>
        /// Notifies the activity console to suspend activity execution.
        /// </summary>
        public void PauseResource()
        {
            TraceFactory.Logger.Info("Request to pause resource.");
            VirtualResourceEventBus.PauseResource(this);
        }

        /// <summary>
        /// Notifies the activity console to resume activity execution.
        /// </summary>
        public void ResumeResource()
        {
            TraceFactory.Logger.Info("Request to resume resource.");
            VirtualResourceEventBus.ResumeResource(this);
        }

        /// <summary>
        /// Notifies the activity console to stop activity execution.
        /// </summary>
        public void HaltResource()
        {
            TraceFactory.Logger.Info("Request to halt resource.");
            VirtualResourceEventBus.HaltResource(this);
        }

        /// <summary>
        /// Suspends operations to an Asset so that no jobs are being sent to it.
        /// </summary>
        /// <param name="assetId"></param>
        public void TakeOffline(string assetId)
        {
            TraceFactory.Logger.Info("Request to take asset {0} offline.".FormatWith(assetId));
            VirtualResourceEventBus.TakeOffline(this, assetId);
        }

        /// <summary>
        /// Resumes normal operations to an Asset.
        /// </summary>
        /// <param name="assetId"></param>
        public void BringOnline(string assetId)
        {
            TraceFactory.Logger.Info("Request to bring asset {0} online.".FormatWith(assetId));
            VirtualResourceEventBus.BringOnline(this, assetId);
        }

        /// <summary>
        /// Broadcasts a synchronization event signal.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        public void SignalSynchronizationEvent(string eventName)
        {
            TraceFactory.Logger.Info($"Request to broadcast synchronization event '{eventName}'.");
            VirtualResourceEventBus.SignalSynchronizationEvent(this, eventName);
        }

        /// <summary>
        /// Pings this instance.
        /// </summary>
        /// <returns></returns>
        public bool Ping()
        {
            return true;
        }

        /// <summary>
        /// Sets the status to Ready to Register
        /// </summary>
        public void ReadyToRegister()
        {
            TraceFactory.Logger.Info("Request to complete registration.");
            VirtualResourceEventBus.ReadyToRegister(this);
        }
    }
}
