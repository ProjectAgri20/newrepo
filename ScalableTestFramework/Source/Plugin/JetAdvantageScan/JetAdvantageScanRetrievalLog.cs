using System;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;


namespace HP.ScalableTest.Plugin.JetAdvantageScan
{
    public class JetAdvantageScanRetrievalLog : ActivityDataLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="JetAdvantageScanRetrievalLog"/> class.
        /// </summary>
        public JetAdvantageScanRetrievalLog(PluginExecutionData executionData)
            : base(executionData)
        {
            DeviceId = null;
            JobStartDateTime = null;
            JobEndDateTime = null;
            JobEndStatus = null;
            Username = null;
            JetAdvantageLoginId = string.Empty;
        }
        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        [DataLogProperty]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the JetAdvantage Login Id.
        /// </summary>
        /// <value>
        /// The JetAdvantage Login Id.
        /// </value>
        [DataLogProperty]
        public string JetAdvantageLoginId { get; set; }
        /// <summary>
        /// Gets or sets the device id.
        /// </summary>
        /// <value>
        /// The device id.
        /// </value>
        [DataLogProperty]
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the type of the solution (Atlas, Helios).
        /// </summary>
        /// <value>The type of the solution.</value>
        [DataLogProperty]
        public string SolutionType { get; set; }

        /// <summary>
        /// Gets or sets the job start time.
        /// </summary>
        /// <value>
        /// The job start.
        /// </value>
        [DataLogProperty]
        public DateTimeOffset? JobStartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the job end time.
        /// </summary>
        /// <value>
        /// The job end.
        /// </value>
        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset? JobEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the job end status.
        /// </summary>
        /// <value>
        /// The job end status.
        /// </value>
        [DataLogProperty(IncludeInUpdates = true)]
        public string JobEndStatus { get; set; }

        /// <summary>
        /// Get or sets the activity error message
        /// </summary>
        /// <value>error message</value>
        [DataLogProperty(IncludeInUpdates = true, MaxLength = 1024, TruncationAllowed = true)]
        public string ErrorMessage { get; set; }

        public override string TableName
        {
            get { return "JetAdvantageScanRetrievalLog"; }
        }
    }
}
