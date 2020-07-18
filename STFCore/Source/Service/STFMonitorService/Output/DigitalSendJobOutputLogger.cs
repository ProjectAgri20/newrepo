using System;
using System.IO;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Service.Monitor.Output
{
    /// <summary>
    /// Class for logging data about the output files from a digital send job.
    /// </summary>
    public class DigitalSendJobOutputLogger : FrameworkDataLog
    {
        /// <summary>
        /// The name of the table to log to.
        /// </summary>
        public override string TableName => "DigitalSendJobOutput";

        /// <summary>
        /// The primary key column name of the log table.
        /// </summary>
        public override string PrimaryKeyColumn => nameof(DigitalSendJobOutputId);

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSendJobOutputLogger"/> class.
        /// </summary>
        public DigitalSendJobOutputLogger()
        {
            DigitalSendJobOutputId = SequentialGuid.NewGuid();
            FilePrefix = string.Empty;
            SessionId = string.Empty;
            FileSentDateTime = null;
            FileReceivedDateTime = null;
            FileName = null;
            FileSizeBytes = 0;
            FileLocation = null;
            PageCount = 0;
            ErrorMessage = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSendJobOutputLogger"/> class.
        /// </summary>
        /// <param name="fileName">The path of the output file.</param>
        /// <param name="filePrefix">The scan file prefix to be used.</param>
        /// <param name="sessionId">The session Id.</param>
        public DigitalSendJobOutputLogger(string fileName, string filePrefix, string sessionId)
            : this()
        {
            FileName = fileName;
            FilePrefix = filePrefix;
            SessionId = sessionId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DigitalSendJobOutputLogger"/> class.
        /// </summary>
        /// <param name="filePrefix">The scan file prefix to be used.</param>
        /// <param name="extension">The file extension.</param>
        public DigitalSendJobOutputLogger(ScanFilePrefix filePrefix, string extension)
            : this()
        {
            if (filePrefix == null)
            {
                throw new ArgumentNullException("filePrefix");
            }

            FileName = Path.ChangeExtension(filePrefix.ToString(), extension);
            FilePrefix = filePrefix.ToString();
            SessionId = filePrefix.SessionId;
        }

        /// <summary>
        /// Gets or sets the digital send job output log id.
        /// </summary>
        /// <value>
        /// The digital send job output log id.
        /// </value>
        [DataLogProperty]
        public Guid DigitalSendJobOutputId { get; private set; }

        /// <summary>
        /// Gets or sets the session id.
        /// </summary>
        /// <value>
        /// The session id.
        /// </value>
        [DataLogProperty]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the file prefix.
        /// </summary>
        /// <value>
        /// The file prefix.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string FilePrefix { get; set; }

        /// <summary>
        /// Gets or sets the name of the file.
        /// </summary>
        /// <value>
        /// The name of the file.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file location.
        /// </summary>
        /// <value>
        /// The file location.
        /// </value>
        [DataLogProperty(MaxLength = 255)]
        public string FileLocation { get; set; }

        /// <summary>
        /// Gets or sets the file sent time.
        /// </summary>
        /// <value>
        /// The file sent.
        /// </value>
        [DataLogProperty]
        public DateTimeOffset? FileSentDateTime { get; set; }

        /// <summary>
        /// Gets or sets the file received time.
        /// </summary>
        /// <value>
        /// The file received.
        /// </value>
        [DataLogProperty]
        public DateTimeOffset? FileReceivedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the size of the file in bytes.
        /// </summary>
        /// <value>
        /// The size of the file.
        /// </value>
        [DataLogProperty]
        public long FileSizeBytes { get; set; }

        /// <summary>
        /// Gets or sets the page count.
        /// </summary>
        /// <value>
        /// The page count.
        /// </value>
        [DataLogProperty]
        public short PageCount { get; set; }

        /// <summary>
        /// Gets or sets the error.
        /// </summary>
        /// <value>
        /// The error.
        /// </value>
        [DataLogProperty(MaxLength = 1024, TruncationAllowed = true)]
        public string ErrorMessage { get; set; }

        /// <summary>
        /// Sets the Error message using the given <see cref="ValidationResult"/>.
        /// </summary>
        /// <param name="result"></param>
        public void SetErrorMessage(ValidationResult result)
        {
            ErrorMessage = result.Succeeded ? null : result.Message;
        }
    }
}
