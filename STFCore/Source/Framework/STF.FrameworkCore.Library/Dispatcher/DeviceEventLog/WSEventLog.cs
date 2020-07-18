using System;

namespace HP.ScalableTest.Framework.Dispatcher.DeviceEventLog
{
    /// <summary>
    /// Object used to store event data from the device for possible further processing
    /// </summary>
    public class WSEventLog
    {
        public long EventId { get; set; }

        public string EventType { get; set; }// Error, Warning, Info

        public DateTime EventDateTime { get; set; }

        public string EventCode { get; set; }

        public string EventDescription { get; set; }

        public WSEventLog()
        {
            EventId = 0;
            EventType = string.Empty;
            EventDateTime = DateTime.Now;
            EventCode = string.Empty;
            EventDescription = string.Empty;
        }
    }
}