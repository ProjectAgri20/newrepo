namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Configuration data for a Citrix Worker virtual resource.
    /// </summary>
    public class CitrixWorker : OfficeWorker
    {
        /// <summary>
        /// Gets or sets the Citrix server host name.
        /// </summary>
        public string ServerHostName { get; set; }

        /// <summary>
        /// Gets or sets the Citrix execution mode.
        /// </summary>
        public string CitrixExecutionMode { get; set; }

        /// <summary>
        /// Gets or sets the Citrix published application to run.
        /// </summary>
        public string PublishedApp { get; set; }
    }
}
