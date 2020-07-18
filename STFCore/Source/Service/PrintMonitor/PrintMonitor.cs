using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Printing;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Print;
using HP.ScalableTest.Print.Monitor;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Service.PrintMonitor
{
    internal sealed class PrintMonitor : IDisposable
    {
        private PrintJobMonitor _printJobMonitor = new PrintJobMonitor();
        private Dictionary<string, PrintQueueJobCount> _jobCountCache = new Dictionary<string, PrintQueueJobCount>();
        private Dictionary<string, MonitoredQueueInfoCache> _cache;
        private DataLogger _dataLogger;

        public event EventHandler<PrintJobDataEventArgs> PrintJobEnded;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintMonitor"/> class.
        /// </summary>
        public PrintMonitor()
        {
            _cache = new Dictionary<string, MonitoredQueueInfoCache>();
            _printJobMonitor.PrintJobMonitoringFinished += new EventHandler<PrintJobDataEventArgs>(_printJobMonitor_PrintJobMonitoringFinished);
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _dataLogger = new DataLogger(GlobalSettings.WcfHosts[WcfService.DataLog]);
            _printJobMonitor.StartMonitor();
        }

        private void UpdateJobCount(object sender, PrintJobDataEventArgs e)
        {
            if (e.Job.Status.HasFlag(JobStatus.Deleted))
            {
                if (!string.IsNullOrEmpty(e.Job.PrinterName))
                {
                    string key = e.Job.PrinterName.ToUpperInvariant();
                    if (_jobCountCache.ContainsKey(key))
                    {
                        // Protect from overflow.
                        if (_jobCountCache[key].JobsPrinted != int.MaxValue)
                        {
                            _jobCountCache[key].JobsPrinted++;
                        }
                    }
                    else
                    {
                        TraceFactory.Logger.Debug("Key not found: " + key);
                    }
                }
            }
        }

        private void _printJobMonitor_PrintJobMonitoringFinished(object sender, PrintJobDataEventArgs e)
        {
            LogPrintJob(e.Job);
            UpdateJobCount(sender, e);
            PrintJobEnded?.Invoke(sender, e);
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "If a print job fails to log, the print monitor cannot be allowed to crash.")]
        private void LogPrintJob(PrintJobData job)
        {
            Guid printJobClientId = Guid.Empty;
            // If extracting the ID fails, then do not report data on this job
            try
            {
                if (!string.IsNullOrEmpty(job.Document))
                {
                    printJobClientId = UniqueFile.ExtractId(job.Document.ToUpperInvariant());
                }
            }
            catch (FormatException)
            {
                TraceFactory.Logger.Warn("Bad document name: " + job.Document);
                // Do nothing.
                return;
            }

            if (printJobClientId == Guid.Empty)
            {
                return;
            }

            PrintServerJobLog logger = null;

            MonitoredQueueInfoCache printQueue = GetQueueInfo(job.PrinterName);
            if (printQueue != null)
            {
                logger = new PrintServerJobLog(printJobClientId, job, printQueue);
            }
            else
            {
                logger = new PrintServerJobLog(printJobClientId, job);
            }

            try
            {
                _dataLogger.SubmitAsync(logger);
                TraceFactory.Logger.Debug("Data posted to data log service.");
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Error("Error posting data to the data log service.", ex);
            }
        }

        private MonitoredQueueInfoCache GetQueueInfo(string queueName)
        {
            MonitoredQueueInfoCache cachedInfo = null;
            try
            {
                //Try to retrieve from the cache
                if (!_cache.TryGetValue(queueName, out cachedInfo))
                {
                    cachedInfo = new MonitoredQueueInfoCache(queueName)
                    {
                        PrintServer = Environment.MachineName,
                        PrintServerOS = Environment.OSVersion.ToString()
                    };
                    _cache.Add(queueName, cachedInfo);
                }

                // Refresh queue data if older than 5 minutes
                if (cachedInfo.QueueSettingsRetrieved < DateTime.Now.AddMinutes(-5))
                {
                    PrintQueue queue = PrintQueueController.GetPrintQueue(queueName);
                    cachedInfo.Refresh(queue);

                    PrintJobRenderLocation location = PrintQueueController.GetJobRenderLocation(queue);
                    if (location != PrintJobRenderLocation.Unknown)
                    {
                        cachedInfo.RenderOnClient = (location == PrintJobRenderLocation.Client);
                    }
                    else
                    {
                        cachedInfo.RenderOnClient = null;
                    }

                    cachedInfo.QueueSettingsRetrieved = DateTime.Now;
                }
            }
            catch (Win32Exception ex)
            {
                TraceFactory.Logger.Error("Unable to get queue data for {0}".FormatWith(queueName), ex);
            }

            return cachedInfo;
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _printJobMonitor.StopMonitor();
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            _printJobMonitor.Dispose();
        }

        internal bool RequestToSendJob(string queueName, int maxJobCount)
        {
            string key = queueName.ToUpperInvariant();

            // Check the simple case, which can be done outside of a lock.
            if (_jobCountCache.ContainsKey(key) && !_jobCountCache[key].Expired)
            {
                return _jobCountCache[key].RequestPrint(maxJobCount);
            }

            // If we've made it this far, it means that we need to figure out the count.
            // Serialize the process.
            lock (_jobCountCache)
            {
                // Try the same check once more.  It's possible that another thread completed the work for us.
                if (_jobCountCache.ContainsKey(key) && !_jobCountCache[key].Expired)
                {
                    return _jobCountCache[key].RequestPrint(maxJobCount);
                }

                // Calculate the count.  Try at least 5 times.
                TraceFactory.Logger.Debug("Calculating print job count for {0}.".FormatWith(queueName));
                int count = 0;
                Retry.WhileThrowing(() => count = GetPrintQueueJobCount(queueName),
                    5, TimeSpan.FromSeconds(1), new List<Type>() { typeof(Exception) });

                // Cache the result.
                if (!_jobCountCache.ContainsKey(key))
                {
                    PrintQueueJobCount queueJobCount = new PrintQueueJobCount(count);
                    _jobCountCache.Add(key, queueJobCount);
                }
                else
                {
                    _jobCountCache[key].Count = count;
                }

                return _jobCountCache[key].RequestPrint(maxJobCount);
            }
        }

        private static int GetPrintQueueJobCount(string queueName)
        {
            using (LocalPrintServer server = new LocalPrintServer())
            {
                using (PrintQueue queue = new PrintQueue(server, queueName, new PrintQueueIndexedProperty[] { PrintQueueIndexedProperty.NumberOfJobs }))
                {
                    return queue.NumberOfJobs;
                }
            }
        }
    }
}

