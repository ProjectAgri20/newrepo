using System;
using System.ServiceModel;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Service interface for virtual resources to report to.
    /// </summary>
    [ServiceContract]
    public interface IClientControllerService
    {
        /// <summary>
        /// Gets the manifest.
        /// </summary>
        /// <param name="instanceId">The resource id.</param>
        /// <returns></returns>
        [OperationContract]
        string GetManifest(string instanceId);

        /// <summary>
        /// Tells the service to perform any cleanup activities.
        /// </summary>
        [OperationContract]
        void Cleanup();

        /// <summary>
        /// Tells the service to copy any logs that are unique to this resource.
        /// </summary>
        [OperationContract]
        LogFileDataCollection GetLogFiles(string sessionId);

        /// <summary>
        /// Notifies the state of the worker to the main controller.
        /// </summary>
        /// <param name="endpoint">The endpoint used to communicate back to the worker.</param>
        /// <param name="state">The state of the worker being sent to the controller.</param>
        [OperationContract]
        void NotifyResourceState(Uri endpoint, RuntimeState state);
    }
}
