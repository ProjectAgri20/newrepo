using System;

namespace HP.ScalableTest.Service.Monitor.DigitalSend
{
    internal class SessionEventArgs : EventArgs
    {
        public string SessionId { get; set; }

        public SessionEventArgs(string sessionId)
        {
            SessionId = sessionId;
        }
    }
}
