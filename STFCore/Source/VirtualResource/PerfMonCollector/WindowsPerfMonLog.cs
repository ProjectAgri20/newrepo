using System;
using System.Diagnostics;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.VirtualResource.PerfMonCollector
{
    internal class WindowsPerfMonLog : FrameworkDataLog
    {
        public override string TableName => "WindowsPerfMon";

        public override string PrimaryKeyColumn => nameof(WindowsPerfMonId);

        public WindowsPerfMonLog(string sessionId, DateTime collectionDateTime, PerformanceCounter counter, float counterValue)
        {
            WindowsPerfMonId = SequentialGuid.NewGuid();
            SessionId = sessionId;

            CollectionDateTime = collectionDateTime;
            TargetHost = counter.MachineName;
            Category = counter.CategoryName;
            InstanceName = counter.InstanceName;
            Counter = counter.CounterName;
            CounterValue = counterValue;
        }

        [DataLogProperty]
        public Guid WindowsPerfMonId { get; }

        [DataLogProperty]
        public string SessionId { get; }

        [DataLogProperty]
        public DateTimeOffset CollectionDateTime { get; set; }

        [DataLogProperty]
        public string TargetHost { get; set; }

        [DataLogProperty(MaxLength = 255)]
        public string Category { get; set; }

        [DataLogProperty(MaxLength = 255)]
        public string InstanceName { get; set; }

        [DataLogProperty(MaxLength = 255)]
        public string Counter { get; set; }

        [DataLogProperty]
        public float CounterValue { get; set; }
    }
}
