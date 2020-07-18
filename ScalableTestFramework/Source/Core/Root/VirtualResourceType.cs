namespace HP.ScalableTest.Core
{
    /// <summary>
    /// Virtual resource types that can be used in automated testing.
    /// </summary>
    public enum VirtualResourceType
    {
        /// <summary>
        /// No virtual resource type.
        /// </summary>
        None,

        /// <summary>
        /// A virtual worker that executes activities based on plugins.
        /// </summary>
        OfficeWorker,

        /// <summary>
        /// A virtual worker designed to execute setup and teardown activities for a test session.
        /// </summary>
        AdminWorker,

        /// <summary>
        /// A virtual worker that executes on a Citrix server.
        /// </summary>
        CitrixWorker,

        /// <summary>
        /// A virtual worker that executes plugins on a local machine.
        /// </summary>
        SolutionTester,

        /// <summary>
        /// A virtual worker that executes many plugin activities in parallel.
        /// </summary>
        LoadTester,

        /// <summary>
        /// A collector that retrieves performance monitor data from a server.
        /// </summary>
        PerfMonCollector,

        /// <summary>
        /// A collector that retrieves event logs from a server.
        /// </summary>
        EventLogCollector,

        /// <summary>
        /// A resource used to reserve machines in an STF environment for manual testing or other non-STF use.
        /// </summary>
        MachineReservation
    }
}
