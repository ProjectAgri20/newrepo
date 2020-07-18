using System;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class to support client side management of <see cref="SessionStartupTransition"/> changes.
    /// </summary>
    public class SessionStartupTransitionEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the next transition step.
        /// </summary>
        public SessionStartupTransition Transition { get; private set; }

        /// <summary>
        /// Gets the session id.
        /// </summary>
        public string SessionId { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SessionStartupTransitionEventArgs" /> class.
        /// </summary>
        /// <param name="transition">Defines the next transition in the startup process</param>
        /// <param name="sessionId">The session unique identifier.</param>
        public SessionStartupTransitionEventArgs(SessionStartupTransition transition, string sessionId)
        {
            Transition = transition;
            SessionId = sessionId;
        }
    }

}
