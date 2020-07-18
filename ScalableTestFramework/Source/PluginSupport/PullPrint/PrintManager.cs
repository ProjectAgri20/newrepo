using System;
using System.IO;
using System.Printing;
using System.Threading.Tasks;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Print;

namespace HP.ScalableTest.PluginSupport.PullPrint
{
    /// <summary>
    /// Class PrintManager.
    /// </summary>
    public class PrintManager
    {
        private PluginExecutionData _pluginData;
        private readonly PrintingEngine _engine = new PrintingEngine();
        private DocumentCollectionIterator _documentIterator;
        private PrintQueueInfoCollection _printQueues;

        private Guid _printJobId;
        private string _serverName = string.Empty;

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintManager"/> class.
        /// </summary>
        /// <param name="pluginExecData">The plugin execute data.</param>
        /// <param name="documentIterator">The document iterator.</param>
        public PrintManager(PluginExecutionData pluginExecData, DocumentCollectionIterator documentIterator)
        {
            _pluginData = pluginExecData;
            _printQueues = _pluginData.PrintQueues;
            _documentIterator = documentIterator;
        }

        /// <summary>
        /// Executes the print job.
        /// </summary>
        /// <exception cref="PrintQueueNotAvailableException"></exception>
        public void Execute()
        {
            try
            {
                // Thread this out so that if the Office Interop locks up and we have to kill it, it won't kill the user thread.
                Task printThread = Task.Factory.StartNew(() => ExecuteHandler());
                printThread.Wait();
            }
            catch (AggregateException ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                throw new PrintQueueNotAvailableException("Print Job Failed to print.", ex);
            }
            catch (Exception ex)
            {
                ExecutionServices.SystemTrace.LogError(ex);
                throw new PrintQueueNotAvailableException("Print Job Failed to print.", ex);
            }
        }

        private void ExecuteHandler()
        {
            ExecutionServices.SystemTrace.LogDebug($"Plugin exec data print queues = {_printQueues.Count}");
            foreach (var x in _printQueues)
            {
                ExecutionServices.SystemTrace.LogDebug($"Queue={x.QueueName}");
            }

            // Check to make sure we have something in the pool...
            if (_printQueues.Count == 0)
            {
                var msg = "None of the selected print queues are available.";

                ExecutionServices.SystemTrace.LogDebug(msg);
                throw new PrintQueueNotAvailableException(msg);
            }

            // Pick a print queue and log the device/server if applicable
            PrintQueueInfo queueInfo = _printQueues.GetRandom();
            LogDevice(_pluginData, queueInfo);
            LogServer(_pluginData, queueInfo);

            // Connect to the print queue
            ExecutionServices.SystemTrace.LogDebug($"Connecting to queue: {queueInfo.QueueName}");
            PrintQueue printQueue = PrintQueueController.Connect(queueInfo);

            _serverName = printQueue.HostingPrintServer.Name.TrimStart('\\');
            ExecutionServices.SystemTrace.LogDebug($"Connected to queue: {printQueue.FullName}");

            // Select a document to print
            Document document = _documentIterator.GetNext(_pluginData.Documents);
            ActivityExecutionDocumentUsageLog documentLog = new ActivityExecutionDocumentUsageLog(_pluginData, document);
            ExecutionServices.DataLogger.Submit(documentLog);

            // Download the document and log the starting information for the print job
            Guid jobId = SequentialGuid.NewGuid();
            FileInfo localFile = ExecutionServices.FileRepository.GetFile(document);
            PrintJobClientLog log = LogPrintJobStart(_pluginData, localFile, printQueue, jobId);

            // Print the job
            PrintingEngineResult result = _engine.Print(localFile, printQueue, jobId);
            _printJobId = result.UniqueFileId;

            if (result == null)
            {
                throw new FilePrintException($"Failed to print {localFile}.");
            }

            // Log the ending information
            LogPrintJobEnd(log, result);
            ExecutionServices.SystemTrace.LogDebug("Controller execution completed");
        }

        /// <summary>
        /// Waits the on print server notification.
        /// </summary>
        public void WaitOnPrintServerNotification()
        {
            ExecutionServices.SystemTrace.LogDebug("Calling WaitForPrintJobFinished at " + _serverName + "Time: " + DateTime.Now.ToString());

            ExecutionServices.SessionRuntime.AsInternal().WaitForPrintJob(_serverName, _printJobId, TimeSpan.FromSeconds(20));

            ExecutionServices.SystemTrace.LogDebug("Finished with WaitForPrintJobFinished at " + DateTime.Now.ToString());
        }

        private static void LogDevice(PluginExecutionData executionData, PrintQueueInfo printQueue)
        {
            if (!string.IsNullOrEmpty(printQueue.AssociatedAssetId))
            {
                var log = new ActivityExecutionAssetUsageLog(executionData, printQueue.AssociatedAssetId);
                ExecutionServices.DataLogger.Submit(log);
            }
        }

        private static void LogServer(PluginExecutionData executionData, PrintQueueInfo printQueue)
        {
            RemotePrintQueueInfo remoteQueue = printQueue as RemotePrintQueueInfo;
            if (remoteQueue != null)
            {
                var log = new ActivityExecutionServerUsageLog(executionData, remoteQueue.ServerHostName);
                ExecutionServices.DataLogger.Submit(log);
            }
        }

        private static PrintJobClientLog LogPrintJobStart(PluginExecutionData executionData, FileInfo file, PrintQueue printQueue, Guid jobId)
        {
            var log = new PrintJobClientLog(executionData, file, printQueue, jobId);
            ExecutionServices.DataLogger.Submit(log);
            return log;
        }

        private static void LogPrintJobEnd(PrintJobClientLog log, PrintingEngineResult result)
        {
            log.JobStartDateTime = result.JobStartTime;
            log.JobEndDateTime = result.JobEndTime;
            log.PrintStartDateTime = result.PrintStartTime;
            ExecutionServices.DataLogger.Update(log);
        }
    }
}
