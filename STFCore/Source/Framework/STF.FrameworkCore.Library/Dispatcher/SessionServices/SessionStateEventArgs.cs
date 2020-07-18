using System;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class to support client side management of <see cref="State"/> changes.
    /// </summary>
    public class SessionStateEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the state.
        /// </summary>
        public SessionState State { get; private set; }

        /// <summary>
        /// Gets the session id.
        /// </summary>
        public string SessionId { get; private set; }

        public string Message { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStateEventArgs" /> class.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <param name="sessionId">The session unique identifier.</param>
        public SessionStateEventArgs(SessionState state, string sessionId, string message)
        {
            State = state;
            SessionId = sessionId;
            Message = message;
        }
    }
}
