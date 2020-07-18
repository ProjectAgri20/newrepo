using System;

namespace HP.ScalableTest.Framework.Runtime
{
    public class VirtualResourceEventBusSignalArgs : EventArgs
    {
        public string EventName { get; }

        public VirtualResourceEventBusSignalArgs(string eventName)
        {
            EventName = eventName;
        }
    }
}
