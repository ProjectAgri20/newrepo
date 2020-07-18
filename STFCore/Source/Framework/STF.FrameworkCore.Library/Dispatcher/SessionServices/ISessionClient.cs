using System.ServiceModel;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Defines the service contract for a session status subscriber.
    /// </summary>
    [ServiceContract]
    public interface ISessionClient
    {
        /// <summary>
        /// Posts a session state change.
        /// </summary>
        /// <param name="state">The current session state.</param>
        /// <param name="sessionId">The session unique identifier.</param>
        /// <param name="message"></param>
        [OperationContract]
        void PublishSessionState(SessionState state, string sessionId, string message);

        /// <summary>
        /// Posts a system status change.
        /// </summary>
        /// <param name="element">The current runtime status.</param>
        [OperationContract]
        void PublishSessionMapElement(SessionMapElement element);

        /// <summary>
        /// Notifies the host that an error has occurred.
        /// </summary>
        /// <param name="message">The error message.</param>
        /// <param name="detail">The exception detail.</param>
        [OperationContract]
        void PublishDispatcherException(string message, string detail);

        /// <summary>
        /// Posts a session startup transition change.
        /// </summary>
        /// <param name="transition">The current session transition.</param>
        /// <param name="sessionId">The session unique identifier.</param>
        [OperationContract]
        void PublishSessionStartupTransition(SessionStartupTransition transition, string sessionId);

        /// <summary>
        /// Requests the listening client to clear information on the specified session
        /// </summary>
        /// <param name="sessionId">The session id.</param>
        [OperationContract]
        void ClearSession(string sessionId);

        /// <summary>
        /// Test to see if the subscriber service is still alive.
        /// </summary>
        [OperationContract]
        void Ping();
    }
}
