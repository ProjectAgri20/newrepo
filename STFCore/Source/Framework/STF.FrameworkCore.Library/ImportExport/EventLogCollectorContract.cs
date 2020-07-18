using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data Contract (used for import/export) for EventLogCollector.
    /// </summary>
    [DataContract(Name = "EventLocCollector", Namespace = "")]
    [ObjectFactory(VirtualResourceType.EventLogCollector)]
    public class EventLogCollectorContract : ResourceContract
    {
        /// <summary>
        /// Loads the EventLogCollectorContract from the specified VirtualResource object.
        /// </summary>
        /// <param name="resource"></param>
        public override void Load(VirtualResource resource)
        {
            base.Load(resource);

            var worker = resource as EventLogCollector;

            HostName = worker.HostName;
            PollingInterval = worker.PollingInterval;
            ComponentsData = worker.ComponentsData;
            EntryTypesData = worker.EntryTypesData;
        }

        /// <summary>
        /// The host name of the target server.
        /// </summary>
        [DataMember]
        public string HostName { get; set; }

        /// <summary>
        /// The interval at which the Event Logs should be gathered.
        /// </summary>
        [DataMember]
        public int PollingInterval { get; set; }

        /// <summary>
        /// Event Log Components
        /// </summary>
        [DataMember]
        public string ComponentsData { get; set; }

        /// <summary>
        /// Event Log entry types.
        /// </summary>
        [DataMember]
        public string EntryTypesData { get; set; }
    }
}
