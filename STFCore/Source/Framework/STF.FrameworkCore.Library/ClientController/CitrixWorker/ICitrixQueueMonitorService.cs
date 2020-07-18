using System.ServiceModel;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework.Automation
{
    /// <summary>
    /// Service contract for the Print Monitor Service.
    /// </summary>
    [ServiceContract]
    public interface ICitrixQueueMonitorService
    {
        /// <summary>
        /// Subscribes to the monitor service for the given client data.
        /// </summary>
        /// <param name="data">The data which represents CitrixQueueMonitorService.</param>
        [OperationContract]
        void Subscribe(CitrixQueueClientData data);

        /// <summary>
        /// Cleans up the specified user profile path.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userProfilePath">The user profile path.</param>
        [OperationContract]
        void Cleanup(string userId, string userProfilePath);

        [OperationContract(Name = "CleanupWorker")]
        void Cleanup(OfficeWorkerCredential credential);

        /// <summary>
        /// Gets any log files or data files present on the Citrix server
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>LogFileDataCollection.</returns>
        [OperationContract]
        LogFileDataCollection GetCitrixLogFiles(string sessionId);
    }
}
