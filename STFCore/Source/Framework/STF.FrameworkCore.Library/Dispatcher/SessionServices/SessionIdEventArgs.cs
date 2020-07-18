using System;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Event arg used to carry a session id
    /// </summary>
    public class SessionIdEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the session unique identifier.
        /// </summary>
        /// <value>
        /// The session unique identifier.
        /// </value>
        public string SessionId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionIdEventArgs"/> class.
        /// </summary>
        /// <param name="sessionId">The session unique identifier.</param>
        public SessionIdEventArgs(string sessionId)
        {
            SessionId = sessionId;
        }
    }
}
