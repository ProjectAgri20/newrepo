using System;
using System.Collections.ObjectModel;
using System.Printing;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Print;
using HP.ScalableTest.Print.Monitor;

namespace HP.ScalableTest.Service.PrintMonitor
{
    internal class PrintMonitorService : IPrintMonitorService
    {
        static PrintMonitor _printMonitor = new PrintMonitor();

        /// <summary>
        /// Returns an Instance of PrintMonitor
        /// </summary>
        public static PrintMonitor Instance
        {
            get { return _printMonitor; }
        }

        /// <summary>
        /// Initializes the static data for PrintMonitorService
        /// </summary>
        public static void Initialize()
        {
            // All that is needed is for the static variables to get populated (which won't happen until a method
            // in this class is called).  For that reason, there is nothing to do in this method.
        }

        /// <summary>
        /// Starts this instance.
        /// </summary>
        public void Start()
        {
            _printMonitor.Start();
        }

        /// <summary>
        /// Stops this instance.
        /// </summary>
        public void Stop()
        {
            _printMonitor.Stop();
        }

        /// <summary>
        /// Requests to send job.
        /// </summary>
        /// <param name="queueName">Name of the queue.</param>
        /// <param name="maxJobCount">The max job count.</param>
        /// <returns></returns>
        public bool RequestToSendJob(string queueName, int maxJobCount)
        {
            return _printMonitor.RequestToSendJob(queueName, maxJobCount);
        }

        /// <summary>
        /// Determines if the print job is rendered on the client or on the server.
        /// </summary>
        /// <param name="queues">The list of queues.</param>
        /// <returns>
        ///   <c>true</c> if Render Print Jobs on Client is set for the specified queue name; otherwise, <c>false</c>.
        /// </returns>
        /// <exception cref="System.ArgumentNullException">queues</exception>
        public SerializableDictionary<string, string> GetJobRenderLocation(Collection<string> queues)
        {
            if (queues == null)
            {
                throw new ArgumentNullException("queues");
            }

            SerializableDictionary<string, string> values = new SerializableDictionary<string, string>();
            foreach (string queueName in queues)
            {
                PrintQueue queue = PrintQueueController.GetPrintQueue(queueName);
                var location = PrintQueueController.GetJobRenderLocation(queue);
                values.Add(queueName, location.ToString());
                TraceFactory.Logger.Debug("Render On Client {0}:{1}".FormatWith(queueName, location));
            }

            return values;
        }

        /// <summary>
        /// Waits for the print job with the specified ID to be finished printing.
        /// </summary>
        /// <param name="printJobId"></param>
        /// <param name="timeout"></param>
        /// <returns></returns>
        public bool WaitForPrintJobFinished(Guid printJobId, TimeSpan timeout)
        {
            bool isFinished = false;

            TraceFactory.Logger.Debug("In the PrintMonitorService.WaitFroPrintJobFinished at " + DateTime.Now.ToString());

            using (AutoResetEvent autoResetEvent = new AutoResetEvent(false))
            {
                EventHandler<PrintJobDataEventArgs> printJobFinished = (s, e) => PrintJobFinished(printJobId, e.Job, autoResetEvent);
                _printMonitor.PrintJobEnded += printJobFinished;

                isFinished = autoResetEvent.WaitOne(timeout);

                _printMonitor.PrintJobEnded -= printJobFinished;
            }
            TraceFactory.Logger.Debug("Finished with the PrintMonitorService.WaitFroPrintJobFinished at " + DateTime.Now.ToString());

            return isFinished;
        }

        private void PrintJobFinished(Guid printJobId, PrintJobData printJobData, AutoResetEvent autoResetEvent)
        {
            TraceFactory.Logger.Debug("PrintMonitorService.PrintJobFinished Current JobId = " + printJobData.Document + " - Passed in Job Id = " + printJobId);
            TraceFactory.Logger.Debug("PrintMonitorService.PrintJobFinished In the PrintMonitorService.PrintJobFinished at " + DateTime.Now.ToString());
            if (!string.IsNullOrEmpty(printJobData.Document) && printJobData.Document.Contains(printJobId.ToString()))
            {
                TraceFactory.Logger.Debug("Getting ready to call Set in the PrintMonitorService.PrintJobFinished at " + DateTime.Now.ToString());
                autoResetEvent.Set();
                TraceFactory.Logger.Debug("After the to call Set in the PrintMonitorService.PrintJobFinished at " + DateTime.Now.ToString());
            }
            TraceFactory.Logger.Debug("Finished with the PrintMonitorService.PrintJobFinished at " + DateTime.Now.ToString());
        }
        /// <summary>
        /// Creates a new <see cref="LogFileDataCollection"/> instance containing all files in the specified folder.
        /// </summary>
        /// <returns></returns>
        public LogFileDataCollection GetLogFiles()
        {
            return LogFileDataCollection.Create(LogFileReader.DataLogPath());
        }
    }
}
