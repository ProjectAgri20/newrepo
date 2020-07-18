namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Configuration data for an Event Log Collector virtual resource.
    /// </summary>
    public class EventLogCollector : VirtualResource
    {
        /// <summary>
        /// Gets or sets the name of the server to collect event logs from.
        /// </summary>
        public string HostName { get; set; }

        /// <summary>
        /// Gets or sets the polling interval, in minutes.
        /// </summary>
        public int PollingInterval { get; set; }

        /// <summary>
        /// Gets or sets the XML components data.
        /// </summary>
        public string ComponentsData { get; set; }

        /// <summary>
        /// Gets or sets the XML entry types data.
        /// </summary>
        public string EntryTypesData { get; set; }
    }
}
