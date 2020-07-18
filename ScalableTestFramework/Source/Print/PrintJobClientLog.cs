using System;
using System.IO;
using System.Printing;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// Class for logging data about a print job sent from a device.
    /// </summary>
    public sealed class PrintJobClientLog : ActivityDataLog
    {
        /// <summary>
        /// The name of the table into which this data will be logged.
        /// </summary>
        public override string TableName => "PrintJobClient";

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintJobClientLog" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="file">The file that was printed.</param>
        /// <param name="printQueue">The print queue to which the file was printed.</param>
        /// <param name="jobId">The <see cref="Guid" /> to use for the print job ID.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executionData" /> is null.
        /// <para>or</para>
        /// <paramref name="file" /> is null.
        /// <para>or</para>
        /// <paramref name="printQueue" /> is null.
        /// </exception>
        public PrintJobClientLog(PluginExecutionData executionData, FileInfo file, PrintQueue printQueue, Guid jobId)
            : base(executionData, jobId)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (printQueue == null)
            {
                throw new ArgumentNullException(nameof(printQueue));
            }

            FileName = file.Name;
            FileSizeBytes = file.Length;
            PrintQueue = printQueue.Name;
            PrintType = GetPrintType(printQueue);
            ClientOS = Environment.OSVersion.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintJobClientLog" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="file">The file that was printed.</param>
        /// <param name="printQueue">The print queue to which the file was printed.</param>
        /// <param name="printType">The type of </param>
        /// <param name="jobId">The <see cref="Guid" /> to use for the print job ID.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executionData" /> is null.
        /// <para>or</para>
        /// <paramref name="file" /> is null.
        /// </exception>
        public PrintJobClientLog(PluginExecutionData executionData, FileInfo file, string printQueue, string printType, Guid jobId)
            : base(executionData, jobId)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            FileName = file.Name;
            FileSizeBytes = file.Length;
            PrintQueue = printQueue;
            PrintType = printType;
            ClientOS = Environment.OSVersion.ToString();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintJobClientLog" /> class.
        /// </summary>
        /// <param name="executionData">The execution data.</param>
        /// <param name="file">The file that was printed.</param>
        /// <param name="printQueue">The print queue to which the file was printed.</param>
        /// <param name="result">A <see cref="PrintingEngineResult" /> with data about the printing operation.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="executionData" /> is null.
        /// <para>or</para>
        /// <paramref name="result" /> is null.
        /// </exception>
        public PrintJobClientLog(PluginExecutionData executionData, FileInfo file, PrintQueue printQueue, PrintingEngineResult result)
            : this(executionData, file, printQueue, GetFileId(result))
        {
            JobStartDateTime = result.JobStartTime;
            JobEndDateTime = result.JobEndTime;
            PrintStartDateTime = result.PrintStartTime;
        }

        private static Guid GetFileId(PrintingEngineResult result)
        {
            if (result == null)
            {
                throw new ArgumentNullException(nameof(result));
            }
            return result.UniqueFileId;
        }

        private static string GetPrintType(PrintQueue queue)
        {
            string serverName = queue.HostingPrintServer.Name.TrimStart('\\');
            return serverName.Equals(Environment.MachineName) ? "Local" : "Remote";
        }

        /// <summary>
        /// Gets or sets the name of the printed file.
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the file size, in bytes.
        /// </summary>
        [DataLogProperty]
        public long FileSizeBytes { get; set; }

        /// <summary>
        /// Gets or sets the name of the print queue.
        /// </summary>
        [DataLogProperty(MaxLength = 255)]
        public string PrintQueue { get; set; }

        /// <summary>
        /// Gets or sets the print type (e.g. local or remote).
        /// </summary>
        [DataLogProperty]
        public string PrintType { get; set; }

        /// <summary>
        /// Gets or sets the client operating system.
        /// </summary>
        [DataLogProperty]
        public string ClientOS { get; set; }

        /// <summary>
        /// Gets or sets the time when the <see cref="PrintingEngine" /> began processing the print request.
        /// </summary>
        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset? JobStartDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time when the <see cref="PrintingEngine" /> finished processing the print request.
        /// </summary>
        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset? JobEndDateTime { get; set; }

        /// <summary>
        /// Gets or sets the time when the application or operating system indicated printing was completed.
        /// </summary>
        [DataLogProperty(IncludeInUpdates = true)]
        public DateTimeOffset? PrintStartDateTime { get; set; }
    }
}
