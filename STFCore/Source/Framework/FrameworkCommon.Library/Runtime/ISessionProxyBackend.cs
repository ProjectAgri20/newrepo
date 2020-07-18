using System;
using System.ServiceModel;

namespace HP.ScalableTest.Framework.Runtime
{
    /// <summary>
    /// Service interface for client handlers to report to.
    /// </summary>
    [ServiceContract]
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Naming", "CA1702:CompoundWordsShouldBeCasedCorrectly", MessageId = "Backend")]
    public interface ISessionProxyBackend
    {
        /// <summary>
        /// Registers the management endpoint that is open on the specified machine
        /// </summary>
        /// <param name="machineName">Name of the machine where the management endpoint is running</param>
        /// <returns>
        /// A string representation of the DataContract serialized SystemManifest
        /// </returns>
        [OperationContract]
        string RegisterMachine(string machineName);

        /// <summary>
        /// Tells the dispatcher what endpoint the resource is listening to and that it is ready to run.
        /// </summary>
        /// <param name="machineName">Name of the machine.</param>
        /// <param name="resourceName">Name of the resource to be registered</param>
        /// <param name="endpoint">The management endpoint this resource is exposing</param>
        [OperationContract]
        void RegisterResource(string machineName, string resourceName, Uri endpoint);

        /// <summary>
        /// Indicates that there was a status change on the specified machine
        /// </summary>
        /// <param name="machineName">Name of the machine which had a status change</param>
        /// <param name="message">The message.</param>
        [OperationContract]
        void ChangeMachineStatusMessage(string machineName, string message);

        /// <summary>
        /// Indicates that there was a state change on the specified resource
        /// </summary>
        /// <param name="machineName">Name of the machine that this resource resides on.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="state">The state.</param>
        [OperationContract]
        void ChangeResourceState(string machineName, string resourceName, RuntimeState state);

        /// <summary>
        /// Indicates that there was a status change on the specified resource
        /// </summary>
        /// <param name="machineName">Name of the machine that this resource resides on.</param>
        /// <param name="resourceName">Name of the resource.</param>
        /// <param name="message">The message.</param>
        [OperationContract]
        void ChangeResourceStatusMessage(string machineName, string resourceName, string message);

        /// <summary>
        /// Signals to the listener that there was a runtime error.
        /// </summary>
        /// <param name="error">The error object.</param>
        [OperationContract]
        void HandleAssetError(RuntimeError error);
    }
}