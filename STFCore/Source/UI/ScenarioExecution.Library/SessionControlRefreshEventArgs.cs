using System;

namespace HP.ScalableTest.UI.SessionExecution
{
    /// <summary>
    /// Arguments for refresh event on session control
    /// </summary>
    public class SessionControlRefreshEventArgs : EventArgs
    {
        public SessionControlRefreshEventArgs(string sessionId)
        {
            SessionId = sessionId;
        }

        public string SessionId { get; set; }
    }
}