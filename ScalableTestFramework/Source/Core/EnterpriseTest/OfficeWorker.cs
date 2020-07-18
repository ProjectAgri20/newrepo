namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Configuration data for an Office Worker virtual resource.
    /// </summary>
    public class OfficeWorker : VirtualResource
    {
        /// <summary>
        /// Gets or sets the office worker run mode.
        /// </summary>
        public string RunMode { get; set; }

        /// <summary>
        /// Gets or sets the repeat count used for iteration-based execution.
        /// </summary>
        public int RepeatCount { get; set; }

        /// <summary>
        /// Gets or sets the duration time, in seconds, used for duration-based execution.
        /// </summary>
        public int DurationTime { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to randomize the activity execution order.
        /// </summary>
        public bool RandomizeActivities { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to randomize the startup delay.
        /// </summary>
        public bool RandomizeStartupDelay { get; set; }

        /// <summary>
        /// Gets or sets the minimum startup delay.
        /// </summary>
        public int MinStartupDelay { get; set; }

        /// <summary>
        /// Gets or sets the maximum startup delay.
        /// </summary>
        public int MaxStartupDelay { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to randomize the activity delay.
        /// </summary>
        public bool RandomizeActivityDelay { get; set; }

        /// <summary>
        /// Gets or sets the minimum activity delay.
        /// </summary>
        public int MinActivityDelay { get; set; }

        /// <summary>
        /// Gets or sets the maximum activity delay.
        /// </summary>
        public int MaxActivityDelay { get; set; }

        /// <summary>
        /// Gets or sets the user pool.
        /// </summary>
        public string UserPool { get; set; }

        /// <summary>
        /// Gets or sets the security groups.
        /// </summary>
        public string SecurityGroups { get; set; }

        /// <summary>
        /// Gets or sets the XML execution schedule.
        /// </summary>
        public string ExecutionSchedule { get; set; }
    }
}
