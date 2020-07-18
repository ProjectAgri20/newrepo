using System;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.PluginSupport.PullPrint
{
    /// <summary>
    /// 
    /// </summary>
    /// <seealso cref="HP.ScalableTest.Framework.Data.ActivityDataLog" />
    public class PullPrintJobRetrievalLog : ActivityDataLog
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PullPrintJobRetrievalLog"/> class.
        /// </summary>
        public PullPrintJobRetrievalLog(PluginExecutionData executionData) : base(executionData)
        {
            DeviceId = null;
            JobStartDateTime = null;
            JobEndDateTime = null;
            JobEndStatus = null;
            UserName = executionData.Credential.UserName;
            NumberOfCopies = 1;
        }

        /// <summary>
        /// The The name of the table, PullPrintJobRetrieval
        /// </summary>
        public override string TableName => "PullPrintJobRetrieval";

        /// <summary>
        /// Gets or sets the sender.
        /// </summary>
        /// <value>
        /// The sender.
        /// </value>
        [DataLogProperty]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the device id.
        /// </summary>
        /// <value>
        /// The device id.
        /// </value>
        [DataLogProperty]
        public string DeviceId { get; set; }

        /// <summary>
        /// Gets or sets the type of the solution (HPAC, SafeCom).
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
        [DataLogProperty]
        public DateTimeOffset? JobEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the job end status.
        /// </summary>
        /// <value>
        /// The job end status.
        /// </value>
        [DataLogProperty]
        public string JobEndStatus { get; set; }

        /// <summary>
        /// Gets or sets the initial job count.
        /// </summary>
        /// <value>The initial job count.</value>
        [DataLogProperty]
        public short? InitialJobCount { get; set; }

        /// <summary>
        /// Gets or sets the final job count.
        /// </summary>
        /// <value>The final job count.</value>
        [DataLogProperty]
        public short? FinalJobCount { get; set; }

        /// <summary>
        /// Gets or sets the number of copies.
        /// </summary>
        /// <value>
        /// The number of copies.
        /// </value>
        [DataLogProperty]
        public short NumberOfCopies { get; set; }
    }
}
