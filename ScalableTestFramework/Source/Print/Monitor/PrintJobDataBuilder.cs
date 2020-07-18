using System;
using System.Collections.Generic;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Tracks data about a print job and fires event as its state changes.
    /// </summary>
    internal sealed class PrintJobDataBuilder
    {
        private readonly Dictionary<JobNotifyField, object> _fieldValues = new Dictionary<JobNotifyField, object>();
        private DateTimeOffset? _spoolStartTime;
        private DateTimeOffset? _spoolEndTime;
        private DateTimeOffset? _printStartTime;
        private DateTimeOffset? _printEndTime;

        /// <summary>
        /// Occurs when this print job transitions to the <see cref="JobStatus.Spooling" />.
        /// </summary>
        public event EventHandler<PrintJobDataEventArgs> PrintJobSpooling;

        /// <summary>
        /// Occurs when a print job transitions to the <see cref="JobStatus.Printing" /> state.
        /// </summary>
        public event EventHandler<PrintJobDataEventArgs> PrintJobPrinting;

        /// <summary>
        /// Occurs when a print job has completed and been removed from the server.
        /// </summary>
        public event EventHandler<PrintJobDataEventArgs> PrintJobComplete;

        /// <summary>
        /// Gets the print job ID.
        /// </summary>
        public long PrintJobId { get; }

        /// <summary>
        /// Gets the last time this builder received an update.
        /// </summary>
        public DateTimeOffset LastUpdate { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintJobDataBuilder" /> class.
        /// </summary>
        /// <param name="printJobId">The print job ID.</param>
        public PrintJobDataBuilder(long printJobId)
        {
            PrintJobId = printJobId;
        }

        /// <summary>
        /// Updates the specified field for the print job.
        /// </summary>
        /// <param name="field">The <see cref="JobNotifyField" /> to update.</param>
        /// <param name="value">The value.</param>
        public void Update(JobNotifyField field, object value)
        {
            LastUpdate = DateTimeOffset.Now;

            // PortName is a little strange.  It starts out blank, but then gets updated with a port that it's
            // printing on.  Once the job is done printing, the PortName gets cleared out.  For this reason,
            // do not set the port name unless the value is not blank.
            if (field == JobNotifyField.PortName && string.IsNullOrEmpty(value.ToString()))
            {
                return;
            }

            _fieldValues[field] = value;

            // If the status has changed, check to see if our timestamps or end status need to be updated.
            if (field == JobNotifyField.Status)
            {
                JobStatus status = (JobStatus)value;
                LogTrace($"Job {PrintJobId} status: {status}");

                ProcessStatusChange(status);
            }
        }

        private void ProcessStatusChange(JobStatus status)
        {
            if (status == JobStatus.None)
            {
                return;
            }

            bool spooling = status.HasFlag(JobStatus.Spooling);
            bool printing = status.HasFlag(JobStatus.Printing);

            if (spooling && !_spoolStartTime.HasValue)
            {
                _spoolStartTime = DateTimeOffset.Now;
                PrintJobSpooling(this, BuildPrintJobDataEventArgs());
            }

            if (!spooling && _spoolStartTime.HasValue && !_spoolEndTime.HasValue)
            {
                _spoolEndTime = DateTimeOffset.Now;
            }

            if (printing && !_printStartTime.HasValue)
            {
                _printStartTime = DateTimeOffset.Now;
                PrintJobPrinting(this, BuildPrintJobDataEventArgs());
            }

            if (!printing && _printStartTime.HasValue && !_printEndTime.HasValue)
            {
                _printEndTime = DateTimeOffset.Now;
            }

            if (status.HasFlag(JobStatus.Deleted))
            {
                PrintJobComplete(this, BuildPrintJobDataEventArgs());
            }
        }

        /// <summary>
        /// Builds a <see cref="PrintJobDataEventArgs" /> object from the data in this instance.
        /// </summary>
        /// <returns></returns>
        public PrintJobDataEventArgs BuildPrintJobDataEventArgs()
        {
            PrintJobData job = new PrintJobData(PrintJobId, _fieldValues, _spoolStartTime, _spoolEndTime, _printStartTime, _printEndTime);
            return new PrintJobDataEventArgs(job);
        }
    }
}
