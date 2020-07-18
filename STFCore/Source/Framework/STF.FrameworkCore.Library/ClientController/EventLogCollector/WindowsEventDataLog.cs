using System;
using System.Diagnostics;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Framework.ClientController.EventLogCollector
{
    internal class WindowsEventDataLog : FrameworkDataLog
    {
        public override string TableName => "WindowsEventLog";

        public override string PrimaryKeyColumn => nameof(WindowsEventLogId);

        public WindowsEventDataLog(string sessionId, EventLog eventLog, EventLogEntry entry)
        {
            WindowsEventLogId = SequentialGuid.NewGuid();
            SessionId = sessionId;

            SolutionModule = entry.MachineName;
            Source1 = eventLog.Log;
            Source2 = entry.Source;
            GeneratedDateTime = entry.TimeGenerated;
            UserName = entry.UserName;
            EventId = entry.InstanceId;
            EventType = entry.EntryType.ToString();
            EventData = entry.Message;
        }

        [DataLogProperty]
        public Guid WindowsEventLogId { get; }

        [DataLogProperty]
        public string SessionId { get; }

        [DataLogProperty(MaxLength = 255)]
        public string SolutionModule { get; set; }

        [DataLogProperty(MaxLength = 255)]
        public string Source1 { get; set; }

        [DataLogProperty(MaxLength = 255)]
        public string Source2 { get; set; }

        [DataLogProperty]
        public DateTimeOffset GeneratedDateTime { get; set; }

        [DataLogProperty]
        public string UserName { get; set; }

        [DataLogProperty]
        public long EventId { get; set; }

        [DataLogProperty]
        public string EventType { get; set; }

        [DataLogProperty(MaxLength = -1)]
        public string EventData { get; set; }
    }
}
