using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Printing;
using System.ServiceModel;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Print;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Plugin.Printing
{
    /// <summary>
    /// This is the Print Engine class for the printing plugin.  It does all the heavy
    /// lifting for processing the print plugin activity.
    /// </summary>
    public sealed class PrintingEngine
    {
        private DocumentCollectionIterator _documentIterator;

        /// <summary>
        /// Event for when the status changes for this controller.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Checks if the number of jobs in the given queue is under the threshold defined by the activity.
        /// </summary>
        private static bool CheckJobCountInQueue(PrintQueue printQueue, int maxJobsInQueue)
        {
            try
            {
                string serviceHost = printQueue.HostingPrintServer.Name.Trim('\\');

                TimeSpan timeout = new TimeSpan(0, 1, 0);
                TimeSpan delay = new TimeSpan(0, 0, 10);
                DateTime beginTime = DateTime.Now;
                while ((DateTime.Now - beginTime) < timeout)
                {
                    // Query the printer to see how many jobs are currently in the queue
                    if (ExecutionServices.SessionRuntime.AsInternal().RequestToSendPrintJob(serviceHost, printQueue.Name, maxJobsInQueue))
                    {
                        // There is enough room for the job
                        LogDebug("There is enough room for the job to print.");
                        return true;
                    }

                    Thread.Sleep(delay);
                }

                // If we made it this far, it means we timed out waiting to see if there was enough room.
                LogDebug("Print Queue cannot accept any more jobs.");
                return false;
            }
            catch (EndpointNotFoundException ex)
            {
                // Give a bit of context on what happened.
                string problem = string.Format("The Print Monitor Service is not running for the queue {0}.", printQueue.FullName);
                string solution = "Either disable the \"Job Throttling\" feature or install the Print Monitor Service on the print server.";
                throw new EndpointNotFoundException(string.Format("{0}  {1}", problem, solution), ex);
            }
        }

        /// <summary>
        /// Start the activity.
        /// </summary>
        /// <param name="executionData">Serialized activity data.</param>
        public PluginExecutionResult ProcessActivity(PluginExecutionData executionData)
        {
            PrintingActivityData data = executionData.GetMetadata<PrintingActivityData>();
            PrintQueueInfoCollection printQueues = executionData.PrintQueues;

            // Initialize the document iterator, if it is not already created
            if (_documentIterator == null)
            {
                CollectionSelectorMode mode = data.ShuffleDocuments ? CollectionSelectorMode.ShuffledRoundRobin : CollectionSelectorMode.RoundRobin;
                _documentIterator = new DocumentCollectionIterator(mode);
            }

            // Check to make sure we have something in the pool...
            if (printQueues.Count == 0)
            {
                return new PluginExecutionResult(PluginResult.Skipped, "None of the selected print queues are available.", "No available print queues.");
            }

            // Select a print queue and log the device/server if applicable
            PrintQueueInfo printQueueInfo = printQueues.GetRandom();
            LogDevice(executionData, printQueueInfo);
            LogServer(executionData, printQueueInfo);

            // Get the corresponding system print queue
            LogDebug(string.Format("Retrieving print queue for {0}", printQueueInfo.QueueName));
            PrintQueue printQueue;
            if (ExecutionServices.SessionRuntime.AsInternal().IsCitrixEnvironment())
            {
                printQueue = GetCitrixPrintQueue(printQueueInfo);
            }
            else
            {
                printQueue = PrintQueueController.Connect(printQueueInfo);
            }

            LogDebug(string.Format("Found queue: {0}", printQueue.FullName));

            if (data.JobThrottling)
            {
                // Make sure that there is enough room in the print queue for this job.
                if (!CheckJobCountInQueue(printQueue, data.MaxJobsInQueue))
                {
                    // Skip the activity.
                    return new PluginExecutionResult(PluginResult.Skipped, "Print Queue cannot accept any more jobs.", "Print queue throttling.");
                }
            }

            LogDebug("Executing print controller");
            if (data.PrintJobSeparator)
            {
                PrintTag(printQueue, executionData);
            }

            // Select a document to print
            Document document = _documentIterator.GetNext(executionData.Documents);
            ActivityExecutionDocumentUsageLog documentLog = new ActivityExecutionDocumentUsageLog(executionData, document);
            ExecutionServices.DataLogger.Submit(documentLog);

            // Download the document and log the starting information for the print job
            Guid jobId = SequentialGuid.NewGuid();
            FileInfo localFile = ExecutionServices.FileRepository.GetFile(document);
            PrintJobClientLog log = LogPrintJobStart(executionData, localFile, printQueue, jobId);

            // Print the job
            var engine = new Print.PrintingEngine();
            engine.StatusChanged += (s, e) => StatusChanged?.Invoke(s, e);
            var result = engine.Print(localFile, printQueue, jobId);

            // Log the ending information
            LogPrintJobEnd(log, result);
            LogDebug("Controller execution completed");
            return new PluginExecutionResult(PluginResult.Passed);
        }

        private static PrintQueue GetCitrixPrintQueue(PrintQueueInfo printQueueInfo)
        {
            // Special handling for Citrix session queues - they are connections to a remote server,
            // but don't show up when querying the local server for a list of queues.
            // Connect to the queue directly by parsing the queue name
            LocalPrintQueueInfo localPrintQueueInfo = printQueueInfo as LocalPrintQueueInfo;
            if (localPrintQueueInfo != null)
            {
                LogDebug("Attempting to parse Citrix session queue.");
                var match = Regex.Match(localPrintQueueInfo.QueueName, @"^\\\\([\S\s]+)\\([\S\s]+)$");
                if (match.Success)
                {
                    LogDebug("Parse success.");
                    var serverName = match.Groups[1];
                    var queueName = match.Groups[2];

                    LogDebug($"Server Name: {serverName}");
                    LogDebug($"Queue Name: {queueName}");

                    PrintServer server = new PrintServer($@"\\{serverName}");
                    return new PrintQueue(server, localPrintQueueInfo.QueueName);
                }
                else
                {
                    LogDebug("Parse failure.");
                }
            }

            // When Citrix auto-generates a print queue on the Citrix server, it creates a queue with the
            // same name as the local print queue on the client machine, but appends some session information
            // to the end.  To find the real name of the print queue on the Citrix server, we need to
            // find a print queue installed on the system that starts with the same text generated by the base class.
            LogDebug($"Looking for {printQueueInfo.QueueName}");

            List<string> queueNames = PrintQueueController.GetPrintQueues().Select(n => n.FullName).ToList();
            string clientName = Environment.GetEnvironmentVariable("CLIENTNAME");

            RemotePrintQueueInfo remotePrintQueueInfo = printQueueInfo as RemotePrintQueueInfo;
            if (remotePrintQueueInfo != null)
            {
                string citrixQueueName = queueNames.FirstOrDefault(
                    n => n.StartsWith(remotePrintQueueInfo.QueueName, StringComparison.OrdinalIgnoreCase)
                      && n.Contains(remotePrintQueueInfo.ServerHostName, StringComparison.OrdinalIgnoreCase)
                      && n.Contains(clientName, StringComparison.OrdinalIgnoreCase));

                if (citrixQueueName != null)
                {
                    LogDebug($"Found Citrix queue {citrixQueueName}");
                    return PrintQueueController.GetPrintQueue(citrixQueueName);
                }
                else
                {
                    LogDebug($"Did not find mapped queue.  Looking for directly attached queue.");
                    return PrintQueueController.GetPrintQueue(remotePrintQueueInfo.GetPrinterName());
                }
            }

            DynamicLocalPrintQueueInfo dynamicPrintQueueInfo = printQueueInfo as DynamicLocalPrintQueueInfo;
            if (dynamicPrintQueueInfo != null)
            {
                string citrixQueueName = queueNames.FirstOrDefault(
                    n => n.StartsWith(dynamicPrintQueueInfo.QueueName, StringComparison.OrdinalIgnoreCase)
                      && n.Contains(clientName, StringComparison.OrdinalIgnoreCase));
                if (citrixQueueName != null)
                {
                    LogDebug($"Found Citrix queue {citrixQueueName}");
                    return PrintQueueController.GetPrintQueue(citrixQueueName);
                }
                else
                {
                    throw new PrintQueueNotFoundException($"Could not find mapped queue for {dynamicPrintQueueInfo.QueueName}");
                }
            }

            // Default to the usual behavior
            return PrintQueueController.Connect(printQueueInfo);
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
            if (ExecutionServices.SessionRuntime.AsInternal().IsCitrixEnvironment())
            {
                log.PrintType = "Citrix" + log.PrintType;
            }
            ExecutionServices.DataLogger.Submit(log);
            return log;
        }

        private static void LogPrintJobEnd(PrintJobClientLog log, PrintingEngineResult result)
        {
            log.JobStartDateTime = result.JobStartTime.LocalDateTime;
            log.JobEndDateTime = result.JobEndTime.LocalDateTime;
            log.PrintStartDateTime = result.PrintStartTime.LocalDateTime;
            ExecutionServices.DataLogger.Update(log);
        }

        private static void PrintTag(PrintQueue printQueue, PluginExecutionData executionData)
        {
            StringBuilder strFileContent = new StringBuilder();
            strFileContent.AppendLine();
            strFileContent.AppendLine();
            strFileContent.AppendLine(string.Format("UserName: {0}", Environment.UserName));
            strFileContent.AppendLine(string.Format("Session ID: {0}", executionData.SessionId));
            strFileContent.AppendLine(string.Format("Activity ID:{0}", executionData.ActivityExecutionId));
            strFileContent.AppendLine(string.Format("Date: {0}", DateTime.Now.ToShortDateString()));
            strFileContent.AppendLine(string.Format("Time: {0}", DateTime.Now.ToShortTimeString()));

            string tagfileName = Path.Combine(Path.GetTempPath(), executionData.ActivityExecutionId + ".txt");
            File.WriteAllText(tagfileName, strFileContent.ToString(), Encoding.ASCII);

            FilePrinter printer = FilePrinterFactory.Create(tagfileName);
            printer.Print(printQueue);

            File.Delete(tagfileName);
        }
    }
}