using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;
using HP.ScalableTest.Core;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Manifest details for the EventLogCollector virtual resource
    /// </summary>
    [DataContract]
    public class EventLogCollectorDetail : ResourceDetailBase
    {
        /// <summary>
        /// Gets or sets the host name for the EventLogCollector.
        /// </summary>
        [DataMember]
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the components this resource will monitor.
        /// </summary>
        [DataMember]
        public string ComponentsData { get; set; }

        /// <summary>
        /// Gets or sets the event log entry types this resource will monitor.
        /// </summary>
        [DataMember]
        public string EntryTypesData { get; set; }

        /// <summary>
        /// Gets or sets the interval in seconds this resource will poll the logs.
        /// </summary>
        [DataMember]
        public int PollingInterval { get; set; }

        /// <summary>
        /// Gets the unique names associated with the specific resource type.
        /// </summary>
        public override IEnumerable<string> UniqueNames
        {
            get { yield return HostName; }
        }

        /// <summary>
        /// Gets the collection of components.
        /// </summary>
        public Collection<string> Components
        {
            get
            {
                return LegacySerializer.DeserializeXml<Collection<string>>(ComponentsData);
            }
        }

        /// <summary>
        /// Gets a collection of EventLogEntryTypes to monitor.
        /// </summary>
        public Collection<string> EntryTypes
        {
            get
            {
                return LegacySerializer.DeserializeXml<Collection<string>>(EntryTypesData);
            }
        }

        /// <summary>
        /// Gets a bitwise setting of the selected EventLogEntryTypes
        /// </summary>
        public int EntryTypesBitwise
        {
            get
            {
                int result = 0;

                foreach (string entryTypeStr in EntryTypes)
                {
                    foreach (EventLogEntryType entryType in Enum.GetValues(typeof(EventLogEntryType)))
                    {
                        if (entryType.ToString().Equals(entryTypeStr, StringComparison.OrdinalIgnoreCase))
                        {
                            result += (int)entryType;
                        }
                    }
                }
                return result;
            }
        }
    }
}