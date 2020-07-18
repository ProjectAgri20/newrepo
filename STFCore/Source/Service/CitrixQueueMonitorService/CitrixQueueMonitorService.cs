using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Automation;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Service.Citrix
{
    /// <summary>
    /// Implementation of the Citrix Queue Monitor service.
    /// </summary>
    public class CitrixQueueMonitorService : ICitrixQueueMonitorService
    {
        //private const string _profilePath = @"SOFTWARE\Microsoft\Windows NT\CurrentVersion\ProfileList";

        /// <summary>
        /// Subscribes to the monitor service for the given client data.
        /// </summary>
        /// <param name="data">The data which represents CitrixQueueMonitorService.</param>
        public void Subscribe(CitrixQueueClientData data)
        {
            CitrixQueueMonitor.Instance.Add(data);
        }

        /// <summary>
        /// Cleans up the specified user profile path.
        /// </summary>
        /// <param name="userId">The user id.</param>
        /// <param name="userProfilePath">The user profile path.</param>
        public void Cleanup(string userId, string userProfilePath)
        {
            CitrixUserProfile.Cleanup(userId, userProfilePath);
        }


        /// <summary>
        /// Gets the log files.
        /// </summary>
        /// <param name="sessionId">The session identifier.</param>
        /// <returns>LogFileDataCollection.</returns>
        public LogFileDataCollection GetCitrixLogFiles(string sessionId)
        {
            return LogFileDataCollection.Create(LogFileReader.DataLogPath(sessionId));
        }

        /// <summary>
        /// Wrapper used to clean up the specified user profile path.
        /// </summary>
        /// <param name="credential"></param>
        public void Cleanup(OfficeWorkerCredential credential)
        {
            VirtualResourceUserProfile.Cleanup(credential);
        }
    }
}
