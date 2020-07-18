using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Class for logging data about a print job processed by a print server.
    /// </summary>
    public sealed class PrintServerJobLog : FrameworkDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "PrintServerJob";

        /// <summary>
        /// The name of the property representing the primary key for the table.
        /// </summary>
        public override string PrimaryKeyColumn => nameof(PrintServerJobId);

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintServerJobLog" /> class.
        /// </summary>
        /// <param name="printJobClientId">The print job client identifier.</param>
        public PrintServerJobLog(Guid printJobClientId)
        {
            PrintServerJobId = SequentialGuid.NewGuid();
            PrintJobClientId = printJobClientId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintServerJobLog" /> class.
        /// </summary>
        /// <param name="printJobClientId">The print job client identifier.</param>
        /// <param name="printJob">The <see cref="PrintJobData" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="printJob" /> is null.</exception>
        public PrintServerJobLog(Guid printJobClientId, PrintJobData printJob)
            : this(printJobClientId)
        {
            if (printJob == null)
            {
                throw new ArgumentNullException(nameof(printJob));
            }

            PrintQueue = printJob.PrinterName;
            PrintDriver = printJob.DriverName;
            DataType = printJob.DataType;
            SubmittedDateTime = printJob.Submitted;
            SpoolStartDateTime = printJob.SpoolStartTime;
            SpoolEndDateTime = printJob.SpoolEndTime;
            PrintStartDateTime = printJob.PrintStartTime;
            PrintEndDateTime = printJob.PrintEndTime;
            JobTotalPages = (short?)printJob.TotalPages;
            PrintedPages = (short?)printJob.PagesPrinted;
            JobTotalBytes = printJob.TotalBytes;
            PrintedBytes = printJob.BytesPrinted;
            EndStatus = printJob.Status.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintServerJobLog" /> class.
        /// </summary>
        /// <param name="printJobClientId">The print job client identifier.</param>
        /// <param name="printJob">The <see cref="PrintJobData" />.</param>
        /// <param name="printQueue">The <see cref="MonitoredQueueInfoCache" />.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="printJob" /> is null.
        /// <para>or</para>
        /// <paramref name="printQueue" /> is null.
        /// </exception>
        public PrintServerJobLog(Guid printJobClientId, PrintJobData printJob, MonitoredQueueInfoCache printQueue)
            : this(printJobClientId, printJob)
        {
            if (printQueue == null)
            {
                throw new ArgumentNullException(nameof(printQueue));
            }

            PrintServer = printQueue.PrintServer;
            PrintServerOS = printQueue.PrintServerOS;
            RenderOnClient = printQueue.RenderOnClient;
            ColorMode = printQueue.ColorMode;
            Copies = (short)printQueue.Copies;
            NumberUp = (short)printQueue.NumberUp;
            Duplex = printQueue.Duplex;
        }

        /// <summary>
        /// Gets the unique identifier for this print server job.
        /// </summary>
        [DataLogProperty]
        public Guid PrintServerJobId { get; private set; }

        /// <summary>
        /// Gets or sets the unique identifier for the client print job.
        /// </summary>
        [DataLogProperty]
        public Guid PrintJobClientId { get; set; }

        /// <summary>
        /// Gets or sets the name of the hosting print server.
        /// </summary>
        [DataLogProperty]
        public string PrintServer { get; set; }

        /// <summary>
        /// Gets or sets the operating system of the hosting print server.
        /// </summary>
        [DataLogProperty]
        public string PrintServerOS { get; set; }

        /// <summary>
        /// Gets or sets the print queue.
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string PrintQueue { get; set; }

        /// <summary>
        /// Gets or sets the print driver.
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string PrintDriver { get; set; }

        /// <summary>
        /// Gets or sets the type of data used to record the print job.
        /// </summary>
        [DataLogProperty(MaxLength = 10)]
        public string DataType { get; set; }

        /// <summary>
        /// Gets or sets the time the job was submitted to the queue.
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset? SubmittedDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time the job entered the spooling state.
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset? SpoolStartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time the job exited the spooling state.
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset? SpoolEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time the job entered the printing state.
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset? PrintStartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time the job exited the printing state.
        /// </summary>
        [DataLogProperty]
        public DateTimeOffset? PrintEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the size, in pages, of the job.
        /// </summary>
        [DataLogProperty]
        public short? JobTotalPages { get; set; }

        /// <summary>
        /// Gets or sets the size, in bytes, of the job.
        /// </summary>
        [DataLogProperty]
        public long? JobTotalBytes { get; set; }

        /// <summary>
        /// Gets or sets the number of pages that were printed.
        /// </summary>
        [DataLogProperty]
        public short? PrintedPages { get; set; }

        /// <summary>
        /// Gets or sets the number of bytes that were printed.
        /// </summary>
        [DataLogProperty]
        public long? PrintedBytes { get; set; }

        /// <summary>
        /// Gets or sets the end status of the print job when this data was logged.
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string EndStatus { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the queue is configured to render jobs on the client.
        /// </summary>
        [DataLogProperty]
        public bool? RenderOnClient { get; set; }

        /// <summary>
        /// Gets or sets the color mode.
        /// </summary>
        [DataLogProperty(MaxLength = 10)]
        public string ColorMode { get; set; }

        /// <summary>
        /// Gets or sets the number of copies.
        /// </summary>
        [DataLogProperty]
        public short? Copies { get; set; }

        /// <summary>
        /// Gets or sets the number of pages up.
        /// </summary>
        [DataLogProperty]
        public short? NumberUp { get; set; }

        /// <summary>
        /// Gets or sets the duplex setting.
        /// </summary>
        [DataLogProperty(MaxLength = 20)]
        public string Duplex { get; set; }
    }
}
