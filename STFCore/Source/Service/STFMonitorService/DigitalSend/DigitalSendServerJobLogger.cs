using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Service.Monitor.DigitalSend
{
    /// <summary>
    /// Data logger class for logging to DigitalSendServerJob table.
    /// </summary>
    public class DigitalSendServerJobLogger : FrameworkDataLog
    {
        /// <summary>
        /// The name of the data log table logged to by this class.
        /// </summary>
        public override string TableName => "DigitalSendServerJob";

        /// <summary>
        /// The name of the primary key column for the DigitalSendServerJob table.
        /// </summary>
        public override string PrimaryKeyColumn => nameof(DigitalSendServerJobId);

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSendServerJobLogger"/> class.
        /// </summary>
        /// <param name="jobId"></param>
        public DigitalSendServerJobLogger(Guid jobId)
        {
            DigitalSendServerJobId = SequentialGuid.NewGuid();
            DigitalSendJobId = jobId;
            SessionId = string.Empty;
        }

        /// <summary>
        /// Gets or sets the digital send server job id.
        /// </summary>
        /// <value>
        /// The digital send server job id.
        /// </value>
        [DataLogProperty]
        public Guid DigitalSendServerJobId { get; set; }

        /// <summary>
        /// Gets or sets the digital send job id.
        /// </summary>
        /// <value>
        /// The digital send job id.
        /// </value>
        [DataLogProperty]
        public Guid DigitalSendJobId { get; set; }

        /// <summary>
        /// Gets or sets the session id.
        /// </summary>
        /// <value>
        /// The session id.
        /// </value>
        [DataLogProperty]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the type of the job.
        /// </summary>
        /// <value>
        /// The type of the job.
        /// </value>
        [DataLogProperty]
        public string JobType { get; set; }

        /// <summary>
        /// Gets or sets the completion time.
        /// </summary>
        /// <value>
        /// The completion time.
        /// </value>
        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset CompletionDateTime { get; set; }

        /// <summary>
        /// Gets or sets the completion status.
        /// </summary>
        /// <value>
        /// The completion status.
        /// </value>
        [DataLogProperty(IncludeInUpdates = true)]
        public string CompletionStatus { get; set; }

        /// <summary>
        /// Gets or sets the type of the file.
        /// </summary>
        /// <value>
        /// The type of the file.
        /// </value>
        [DataLogProperty]
        public string FileType { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the size of the file in bytes.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataLogProperty]
        public long? FileSizeBytes { get; set; }

        /// <summary>
        /// Gets or sets the scanned pages.
        /// </summary>
        /// <value>
        /// The scanned pages.
        /// </value>
        [DataLogProperty]
        public short? ScannedPages { get; set; }

        /// <summary>
        /// Gets or sets the device model.
        /// </summary>
        /// <value>
        /// The device model.
        /// </value>
        [DataLogProperty]
        public string DeviceModel { get; set; }

        /// <summary>
        /// Gets or sets the DSS version.
        /// </summary>
        /// <value>
        /// The DSS version.
        /// </value>
        [DataLogProperty]
        public string DssVersion { get; set; }

        /// <summary>
        /// Gets or sets the processed by.
        /// </summary>
        /// <value>
        /// The processed by.
        /// </value>
        [DataLogProperty]
        public string ProcessedBy { get; set; }
    }
}
