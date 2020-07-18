using System;
using System.Text;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Service.EPrintJobMonitor
{
    /// <summary>
    /// Logger class for logging ePrint Server print job data.
    /// </summary>
    public class EPrintServerJobLogger : FrameworkDataLog
    {
        private Guid? _printJobId = null;

        public override string TableName => "EPrintServerJob";

        public override string PrimaryKeyColumn => nameof(EPrintServerJobId);

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
        public DateTime EmailReceivedDateTime { get; set; }

        [DataLogProperty]
        public DateTime JobStartDateTime { get; set; }

        [DataLogProperty(IncludeInUpdates = true)]
        public DateTime LastStatusDateTime { get; set; }

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
