using System;
using System.Threading.Tasks;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Runtime;
using System.Printing;
using HP.ScalableTest.Print;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class that represents any remote print queue used in a test scenario.
    /// </summary>
    public class RemotePrintQueueElement : ISessionMapElement
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RemotePrintQueueElement"/> class.
        /// </summary>
        /// <param name="printQueueName">Name of the print queue.</param>
        /// <param name="serverName">Name of the server that the print queue is on.</param>
        public RemotePrintQueueElement(string printQueueName, string serverName)
        {
            PrintQueueName = printQueueName;
            PrintServerName = serverName;

            MapElement = new SessionMapElement(PrintQueueName, ElementType.RemotePrintQueue, "Remote Print Queue");
        }

        /// <summary>
        /// Gets the <see cref="SessionMapElement"/> object for this print queue.
        /// </summary>
        /// <value>The element.</value>
        public SessionMapElement MapElement { get; private set; }

        /// <summary>
        /// Gets the name of the print queue.
        /// </summary>
        /// <value>The name of the print queue.</value>
        public string PrintQueueName { get; private set; }

        /// <summary>
        /// Gets the name of the print server that the queue is on.
        /// </summary>
        /// <value>The name of the print server.</value>
        public string PrintServerName { get; private set; }

        /// <summary>
        /// Handles the runtime error for this print queue.
        /// </summary>
        /// <param name="error">The error information.</param>
        public virtual void HandleError(RuntimeError error)
        {
            // Implemented as needed by child classes
        }

        /// <summary>
        /// Restarts this print queue.
        /// </summary>
        public virtual void Restart()
        {
            // Implemented as needed by child classes
        }

        /// <summary>
        /// Builds all data structures for this print queue.
        /// </summary>
        public virtual void Stage(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Available", RuntimeState.Available);
        }

        /// <summary>
        /// Revalidates this print queue
        /// </summary>
        public virtual void Validate(ParallelLoopState loopState)
        {
            ValidatePrintQueue(loopState);
        }

        private void ValidatePrintQueue(ParallelLoopState loopState)
        {
            var printServer = PrintServerName;
            if (!printServer.Contains("."))
            {
                printServer = PrintServerName + '.' + GlobalSettings.Items[Setting.DnsDomain];
            }
            MapElement.UpdateStatus("Validating", RuntimeState.Validating);

            string validationResult = VerifyPrintQueueExists(printServer, PrintQueueName);

            if (validationResult == string.Empty)
            {
                MapElement.UpdateStatus("Validated", RuntimeState.Validated);
            }
            else
            {
                TraceFactory.Logger.Error("Error validating print queue {0} on {1}: {2}".FormatWith(printServer, PrintQueueName, validationResult));
                MapElement.UpdateStatus("Print Queue Does Not Exist on {0}".FormatWith(printServer), RuntimeState.Error);
            }
        }

        /// <summary>
        /// Verifies that this print queue exists
        /// </summary>
        public virtual void Revalidate(ParallelLoopState loopState)
        {
            ValidatePrintQueue(loopState);
        }

        /// <summary>
        /// Turns on this print queue
        /// </summary>
        public virtual void PowerUp(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Ready", RuntimeState.Ready);
        }

        /// <summary>
        /// Executes this print queue, which may mean different things.
        /// </summary>
        public virtual void Run(ParallelLoopState loopState)
        {
            MapElement.UpdateStatus("Running", RuntimeState.Running);
        }

        /// <summary>
        /// Marks this print queue complete
        /// </summary>
        public void Completed()
        {
            MapElement.UpdateStatus("Completed", RuntimeState.Completed);
        }

        /// <summary>
        /// Shuts down this print queue
        /// </summary>
        public virtual void Shutdown(ShutdownOptions options, ParallelLoopState loopState)
        {
            if (options.PurgeRemotePrintQueues)
            {
                PurgeRemotePrintQueue(PrintServerName + '.' + GlobalSettings.Items[Setting.DnsDomain], PrintQueueName);
            }

            MapElement.UpdateStatus("Offline", RuntimeState.Offline);
        }

        /// <summary>
        /// Pauses this print queue
        /// </summary>
        public virtual void Pause()
        {
            MapElement.UpdateStatus("Paused", RuntimeState.Paused);
        }

        /// <summary>
        /// Resumes this print queue
        /// </summary>
        public virtual void Resume()
        {
            MapElement.UpdateStatus("Running", RuntimeState.Running);
        }

        private static string VerifyPrintQueueExists(string serverName, string printQueueName)
        {
            try
            {
                using (PrintServer server = new PrintServer(@"\\" + serverName))
                {
                    PrintQueue q = new PrintQueue(server, printQueueName);
                }
            }
            catch (Exception ex)
            {
                return ex.Message;
            }

            return string.Empty;
        }

        private static void PurgeRemotePrintQueue(string serverName, string printQueueName)
        {
            try
            {
                using (PrintServer ps = new PrintServer(@"\\" + serverName))
                {
                    using (PrintQueue queue = new PrintQueue(ps, printQueueName, PrintSystemDesiredAccess.AdministratePrinter))
                    {
                        queue.Purge();
                    }
                }

            }
            catch (Exception e)
            {
               TraceFactory.Logger.Debug(e);
            }
            
        }
    }
}
