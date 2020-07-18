using System;
using System.ServiceModel;

namespace HP.ScalableTest.Framework.Monitor
{
    /// <summary>
    /// Interface for communicating with  STF Monitor service.
    /// </summary>
    [ServiceContract]
    public interface ISTFMonitorService
    {
        /// <summary>
        /// Registers the given Session Id and log service host name with the STF monitor service.
        /// </summary>
        /// <param name="sessionId">The session Id</param>
        /// <param name="logServiceHostName">The host name of the server where the data log service is running</param>
        [OperationContract]
        void Register(string sessionId, string logServiceHostName);

        /// <summary>
        /// Returns whether the specified directory path exists on the service host.
        /// </summary>
        /// <param name="directoryPath">The directory path to validate</param>
        /// <returns>true if the directory path exists, false otherwise.</returns>
        [OperationContract]
        bool IsValidDirectoryPath(string directoryPath);

        /// <summary>
        /// Reloads all the MonitorConfig items for each service running on the current service host.  
        /// This would be called when a user updates an item in the MonitorConfig table.
        /// </summary>
        [OperationContract]
        void RefreshConfig();
    }
}
