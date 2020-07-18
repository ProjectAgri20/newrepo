namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Configuration data for a PerfMon Collector virtual resource.
    /// </summary>
    public class PerfMonCollector : VirtualResource
    {
        /// <summary>
        /// Gets or sets the name of the server to collect PerfMon data from.
        /// </summary>
        public string HostName { get; set; }
    }
}
