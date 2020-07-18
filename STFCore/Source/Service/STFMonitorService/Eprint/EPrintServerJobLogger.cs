using System;
using System.Text;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Service.Monitor.Eprint
{
    /// <summary>
    /// Logger class for logging ePrint Server print job data.
    /// </summary>
    public class EPrintServerJobLogger : FrameworkDataLog
    {
        private Guid? _printJobId = null;

        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "EPrintServerJob";

        /// <summary>
        /// The name of the property representing the primary key for the table.
        /// </summary>
        public override string PrimaryKeyColumn => nameof(EPrintServerJobId);

        /// <summary>
        /// Initializes a new instance of the <see cref="EPrintServerJobLogger"/> class.
        /// </summary>
        /// <param name="ePrintJobId">The e print job identifier.</param>
        /// <param name="sessionId">The session identifier.</param>
        public EPrintServerJobLogger(int ePrintJobId, string sessionId)
        {
            EPrintServerJobId = SequentialID64.Instance.GetID(sessionId, SequentialID64.MachineSeed(), ePrintJobId);
            EPrintJobId = ePrintJobId;
            SessionId = sessionId;
        }

        /// <summary>
        /// Gets or sets the ePrint Server Log Id.
        /// </summary>
        [DataLogProperty]
        public long EPrintServerJobId { get; set; }

        /// <summary>
        /// Gets or sets the ePrint job id.
        /// </summary>
        /// <value>
        /// The ePrint job id.
        /// </value>
        [DataLogProperty]
        public int EPrintJobId { get; set; }

        /// <summary>
        /// Gets or sets the print job id.
        /// </summary>
        /// <value>
        /// The print job id.
        /// </value>
        [DataLogProperty]
        public Guid? PrintJobClientId
        {
            get { return _printJobId; }
            set
            {
                if (value != Guid.Empty)
                {
                    _printJobId = value;
                }
            }
        }

        [DataLogProperty]
        public string SessionId { get; set; }

        [DataLogProperty]
        public string EmailAccount { get; set; }

        [DataLogProperty]
        public string PrinterName { get; set; }

        [DataLogProperty(MaxLength = 255)]
        public string JobName { get; set; }

        [DataLogProperty(MaxLength = 255)]
        public string JobFolderId { get; set; }

        [DataLogProperty]
        public DateTimeOffset EmailReceivedDateTime { get; set; }

        [DataLogProperty]
        public DateTimeOffset JobStartDateTime { get; set; }

        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset LastStatusDateTime { get; set; }

        [DataLogProperty(IncludeInUpdates = true)]
        public string JobStatus { get; set; }

        [DataLogProperty]
        public int EPrintTransactionId { get; set; }

        [DataLogProperty(IncludeInUpdates = true)]
        public string TransactionStatus { get; set; }
        public override string ToString()
        {
            StringBuilder result = new StringBuilder();
            result.Append("EPrintJobId:").AppendLine(EPrintJobId.ToString());
            result.Append("JobStatus:").AppendLine(JobStatus);
            result.Append("LastStatusDateTime:").AppendLine(LastStatusDateTime.ToString("G"));
            result.Append("TransactionStatus:").AppendLine(TransactionStatus);

            return result.ToString();
        }

    }
}
