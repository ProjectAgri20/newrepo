using System.ServiceModel;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Service interface used by the dispatcher to manage virtual resources.
    /// </summary>
    [ServiceContract]
    public interface IVirtualResourceManagementService
    {
        /// <summary>
        /// Shuts down this instance of the activity console.
        /// </summary>
        [OperationContract]
        void Shutdown(bool copyLogs);

        /// <summary>
        /// Notifies the activity console to begin the main flow of activities
        /// </summary>
        [OperationContract]
        void StartMainRun();

        /// <summary>
        /// Notifies the activity console to start any pre run setup activities
        /// </summary>
        [OperationContract]
        void StartSetup();

        /// <summary>
        /// Notifies the activity console to start any post run teardown activities
        /// </summary>
        [OperationContract]
        void StartTeardown();

        /// <summary>
        /// Notifies the activity console to pause activities.
        /// </summary>
        [OperationContract]
        void PauseResource();

        /// <summary>
        /// Notifies the activity console to resume activities.
        /// </summary>
        [OperationContract]
        void ResumeResource();

        /// <summary>
        /// Notifies the activity console to halt activities.
        /// </summary>
        [OperationContract]
        void HaltResource();

        /// <summary>
        /// Suspends operations to an Asset so that no jobs are being sent to it.
        /// </summary>
        /// <param name="assetId"></param>
        [OperationContract]
        void TakeOffline(string assetId);

        /// <summary>
        /// Resumes normal operations to an Asset.
        /// </summary>
        /// <param name="assetId"></param>
        [OperationContract]
        void BringOnline(string assetId);

        /// <summary>
        /// Broadcasts a synchronization event signal.
        /// </summary>
        /// <param name="eventName">The synchronization event name.</param>
        [OperationContract]
        void SignalSynchronizationEvent(string eventName);

        /// <summary>
        /// Pings this instance.
        /// </summary>
        /// <returns></returns>
        [OperationContract]
        bool Ping();

        /// <summary>
        /// Tells this resource that it is ok to proceed and register itself with the dispatcher.
        /// </summary>
        [OperationContract]
        void ReadyToRegister();

        /// <summary>
        /// Gets the log data.
        /// </summary>
        /// <returns>System.String.</returns>
        [OperationContract]
        LogFileDataCollection GetLogFiles();
    }
}
