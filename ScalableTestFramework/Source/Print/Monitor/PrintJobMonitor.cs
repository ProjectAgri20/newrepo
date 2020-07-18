using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print.Monitor
{
    /// <summary>
    /// Monitors print jobs on the local system and fires events as their statuses change.
    /// </summary>
    public sealed class PrintJobMonitor : IDisposable
    {
        private readonly WinSpoolPrintMonitor _winSpoolPrintMonitor;
        private readonly Dictionary<long, PrintJobDataBuilder> _printJobDataBuilders = new Dictionary<long, PrintJobDataBuilder>();

        private readonly Timer _jobFlushTimer;
        private readonly TimeSpan _jobFlushCheckFrequency = TimeSpan.FromMinutes(5);
        private readonly TimeSpan _jobFlushExpiration = TimeSpan.FromHours(1);

        /// <summary>
        /// Occurs when a print job transitions to the <see cref="JobStatus.Spooling" />.
        /// </summary>
        public event EventHandler<PrintJobDataEventArgs> PrintJobSpooling;

        /// <summary>
        /// Occurs when a print job transitions to the <see cref="JobStatus.Printing" /> state.
        /// </summary>
        public event EventHandler<PrintJobDataEventArgs> PrintJobPrinting;

        /// <summary>
        /// Occurs when monitoring for a print job is completed.  This may occur when
        /// a job has completed and been removed from the server, or when it is not updated
        /// for long enough that it is unlikely to have any further changes.
        /// </summary>
        public event EventHandler<PrintJobDataEventArgs> PrintJobMonitoringFinished;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintJobMonitor" /> class.
        /// </summary>
        public PrintJobMonitor()
        {
            _winSpoolPrintMonitor = new WinSpoolPrintMonitor();
            _winSpoolPrintMonitor.JobNotificationReceived += ProcessNotification;

            _jobFlushTimer = new Timer(FlushExpiredJobs, null, _jobFlushCheckFrequency, _jobFlushCheckFrequency);
        }

        /// <summary>
        /// Starts the monitor.
        /// </summary>
        public void StartMonitor()
        {
            _winSpoolPrintMonitor.Start();
        }

        /// <summary>
        /// Stops the monitor.
        /// </summary>
        public void StopMonitor()
        {
            _winSpoolPrintMonitor.Stop();
        }

        private void ProcessNotification(object sender, JobNotificationEventArgs e)
        {
            LogTrace($"New notification received for job {e.JobId}: {e.Field} = {e.Value}");
            if (!_printJobDataBuilders.TryGetValue(e.JobId, out PrintJobDataBuilder printJobDataBuilder))
            {
                LogDebug($"Begin tracking job {e.JobId}");
                printJobDataBuilder = CreatePrintJobDataBuilder(e.JobId);
                _printJobDataBuilders.Add(e.JobId, printJobDataBuilder);
            }
            printJobDataBuilder.Update(e.Field, e.Value);
        }

        private void FlushExpiredJobs(object state)
        {
            DateTimeOffset cutoff = DateTimeOffset.Now - _jobFlushExpiration;
            LogDebug($"Ending monitoring for all print jobs not updated since {cutoff}.");

            List<long> ids = _printJobDataBuilders.Where(n => n.Value.LastUpdate < cutoff).Select(n => n.Key).ToList();
            foreach (long id in ids)
            {
                if (_printJobDataBuilders.TryGetValue(id, out PrintJobDataBuilder printJobDataBuilder))
                {
                    var printJobData = printJobDataBuilder.BuildPrintJobDataEventArgs();
                    LogDebug($"Print job {printJobData.Job.Id} is no longer being monitored.");
                    ThreadPool.QueueUserWorkItem(_ => PrintJobMonitoringFinished?.Invoke(this, printJobData));
                }
                _printJobDataBuilders.Remove(id);
            }

            LogDebug($"{ids.Count} print jobs removed.");
        }

        private PrintJobDataBuilder CreatePrintJobDataBuilder(long printJobId)
        {
            PrintJobDataBuilder printJobDataBuilder = new PrintJobDataBuilder(printJobId);
            printJobDataBuilder.PrintJobSpooling += PrintJobDataBuilder_PrintJobSpooling;
            printJobDataBuilder.PrintJobPrinting += PrintJobDataBuilder_PrintJobPrinting;
            printJobDataBuilder.PrintJobComplete += PrintJobDataBuilder_PrintJobComplete;
            return printJobDataBuilder;
        }

        private void PrintJobDataBuilder_PrintJobSpooling(object sender, PrintJobDataEventArgs e)
        {
            LogDebug($"Print job {e.Job.Id} started spooling.");
            ThreadPool.QueueUserWorkItem(_ => PrintJobSpooling?.Invoke(this, e));
        }

        private void PrintJobDataBuilder_PrintJobPrinting(object sender, PrintJobDataEventArgs e)
        {
            LogDebug($"Print job {e.Job.Id} started printing.");
            ThreadPool.QueueUserWorkItem(_ => PrintJobPrinting?.Invoke(this, e));
        }

        private void PrintJobDataBuilder_PrintJobComplete(object sender, PrintJobDataEventArgs e)
        {
            LogDebug($"Print job {e.Job.Id} is finished.");
            ThreadPool.QueueUserWorkItem(_ => PrintJobMonitoringFinished?.Invoke(this, e));
            _printJobDataBuilders.Remove(e.Job.Id);
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _winSpoolPrintMonitor.Dispose();
            _jobFlushTimer.Dispose();
        }
    }
}
