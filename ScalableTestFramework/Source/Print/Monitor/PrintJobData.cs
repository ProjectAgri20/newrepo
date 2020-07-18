using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Details about a print job retrieved by <see cref="PrintJobMonitor" />.
    /// </summary>
    public class PrintJobData
    {
        private readonly Dictionary<JobNotifyField, object> _fieldValues;

        /// <summary>
        /// Print job ID.
        /// </summary>
        public long Id { get; }

        /// <summary>
        /// The time the job entered the spooling state.
        /// </summary>
        public DateTimeOffset? SpoolStartTime { get; }

        /// <summary>
        /// The time the job exited the spooling state.
        /// </summary>
        public DateTimeOffset? SpoolEndTime { get; }

        /// <summary>
        /// The time the job entered the printing state.
        /// </summary>
        public DateTimeOffset? PrintStartTime { get; }

        /// <summary>
        /// The time the job exited the printing state.
        /// </summary>
        public DateTimeOffset? PrintEndTime { get; }

        /// <summary>
        /// The name of the printer for which the job is spooled.
        /// </summary>
        public string PrinterName => GetValue<string>(JobNotifyField.PrinterName);

        /// <summary>
        /// The name of the machine that created the print job.
        /// </summary>
        public string MachineName => GetValue<string>(JobNotifyField.MachineName);

        /// <summary>
        /// The port used to transmit data to the printer.
        /// </summary>
        public string PortName => GetValue<string>(JobNotifyField.PortName);

        /// <summary>
        /// The name of the user who sent the print job.
        /// </summary>
        public string UserName => GetValue<string>(JobNotifyField.UserName);

        /// <summary>
        /// The type of data used to record the print job. This is typically EMF or RAW.
        /// </summary>
        public string DataType => GetValue<string>(JobNotifyField.DataType);

        /// <summary>
        /// The name of the print processor used to record the print job.
        /// </summary>
        public string PrintProcessor => GetValue<string>(JobNotifyField.PrintProcessor);

        /// <summary>
        /// The name of the printer driver used to process the print job.
        /// </summary>
        public string DriverName => GetValue<string>(JobNotifyField.DriverName);

        /// <summary>
        /// The job status.
        /// </summary>
        public JobStatus Status => GetValue<JobStatus>(JobNotifyField.Status);

        /// <summary>
        /// The name of the print job (for example, "MS-WORD: Review.doc").
        /// </summary>
        public string Document => GetValue<string>(JobNotifyField.Document);

        /// <summary>
        /// The time when the job was submitted.
        /// </summary>
        public DateTimeOffset? Submitted => GetValue<DateTimeOffset?>(JobNotifyField.Submitted);

        /// <summary>
        /// The size, in pages, of the job.
        /// </summary>
        public long? TotalPages => GetValue<long?>(JobNotifyField.TotalPages);

        /// <summary>
        /// The number of pages that have printed.
        /// </summary>
        public long? PagesPrinted => GetValue<long?>(JobNotifyField.PagesPrinted);

        /// <summary>
        /// The size, in bytes, of the job.
        /// </summary>
        public long? TotalBytes => GetValue<long?>(JobNotifyField.TotalBytes);

        /// <summary>
        /// The number of bytes that have been printed on this job.
        /// </summary>
        public long? BytesPrinted => GetValue<long?>(JobNotifyField.BytesPrinted);

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintJobData" /> class.
        /// </summary>
        /// <param name="printJobId">The print job ID.</param>
        /// <param name="fieldValues">The <see cref="JobNotifyField" /> objects and their values.</param>.
        /// <param name="spoolStart">The time the job entered the spooling state.</param>
        /// <param name="spoolEnd">The time the job exited the spooling state.</param>
        /// <param name="printStart">The time the job entered the printing state.</param>
        /// <param name="printEnd">The time the job exited the printing state.</param>
        internal PrintJobData(long printJobId, Dictionary<JobNotifyField, object> fieldValues, DateTimeOffset? spoolStart,
                              DateTimeOffset? spoolEnd, DateTimeOffset? printStart, DateTimeOffset? printEnd)
        {
            Id = printJobId;
            _fieldValues = new Dictionary<JobNotifyField, object>(fieldValues);
            SpoolStartTime = spoolStart;
            SpoolEndTime = spoolEnd;
            PrintStartTime = printStart;
            PrintEndTime = printEnd;
        }

        private T GetValue<T>(JobNotifyField field)
        {
            if (_fieldValues.TryGetValue(field, out object value))
            {
                return (T)value;
            }
            else
            {
                return default;
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            StringBuilder builder = new StringBuilder();
            foreach (PropertyInfo info in GetType().GetProperties())
            {
                builder.Append($"[{info.Name}: {info.GetValue(this)}] ");
            }
            return builder.ToString();
        }
    }
}
