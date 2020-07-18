using System;
using System.Linq;

namespace HP.ScalableTest.EndpointResponder
{
    public class DeviceRecorderStatusUpdateEventArgs : EventArgs
    {
        public string UpdateMessage { get; private set; }
        public DeviceRecorderStatusUpdateEventArgs(string message)
        {
            UpdateMessage = message;
        }
    }
}
