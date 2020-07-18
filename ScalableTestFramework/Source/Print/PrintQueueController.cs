using System;
using System.Linq;
using System.Printing;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// Interfaces with print queues on the local machine.
    /// </summary>
    public static class PrintQueueController
    {
        /// <summary>
        /// Gets a collection of print queues available on the local machine.
        /// </summary>
        /// <returns>A collection of <see cref="PrintQueue" /> objects.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method creates a new object each time it is called.")]
        public static PrintQueueCollection GetPrintQueues()
        {
            LogTrace("Retrieving print queues.");
            var queueTypes = new[] { EnumeratedPrintQueueTypes.Connections, EnumeratedPrintQueueTypes.Local };
            using (PrintServer server = new LocalPrintServer())
            {
                return server.GetPrintQueues(queueTypes);
            }
        }

        /// <summary>
        /// Gets a <see cref="PrintQueue" /> on the local machine based on the queue name.
        /// </summary>
        /// <param name="queueName">The queue name.</param>
        /// <returns>A <see cref="PrintQueue" /> object.</returns>
        /// <exception cref="PrintQueueNotFoundException">The print queue could not be found on the local machine.</exception>
        public static PrintQueue GetPrintQueue(string queueName)
        {
            LogTrace($"Retrieving print queues {queueName}");
            PrintQueue result = GetPrintQueues().FirstOrDefault(n => n.FullName.EqualsIgnoreCase(queueName));
            if (result == null)
            {
                throw new PrintQueueNotFoundException($"Could not find print queue '{queueName}'.");
            }
            return result;
        }

        /// <summary>
        /// Connects to and returns a <see cref="PrintQueue" /> object represented by the specified <see cref="PrintQueueInfo" />.
        /// </summary>
        /// <param name="printQueueInfo">The <see cref="PrintQueueInfo" /> specifying the print queue.</param>
        /// <returns>A <see cref="PrintQueue" /> object.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="printQueueInfo" /> is null.</exception>
        /// <exception cref="PrintQueueNotFoundException">The queue specified by <paramref name="printQueueInfo" /> could not be found.</exception>
        public static PrintQueue Connect(PrintQueueInfo printQueueInfo)
        {
            if (printQueueInfo == null)
            {
                throw new ArgumentNullException(nameof(printQueueInfo));
            }

            if (printQueueInfo is RemotePrintQueueInfo remotePrintQueueInfo)
            {
                string printerName = remotePrintQueueInfo.GetPrinterName();
                ConnectToRemoteQueue(printerName);
                return GetPrintQueue(printerName);
            }
            else
            {
                return GetPrintQueue(printQueueInfo.QueueName);
            }
        }

        /// <summary>
        /// Connects to a remote print queue at the specified path.
        /// </summary>
        /// <param name="printerPath">The printer path.</param>
        /// <returns><c>true</c> if the remote queue connection was successful, <c>false</c> otherwise.</returns>
        public static bool ConnectToRemoteQueue(string printerPath)
        {
            LogDebug($"Adding connection to remote queue '{printerPath}'.");

            // In busy situations it may take a few attempts to connect to the print server.
            // Retry a few times with a delay in between.
            bool connect()
            {
                try
                {
                    using (LocalPrintServer server = new LocalPrintServer())
                    {
                        return server.ConnectToPrintQueue(printerPath);
                    }
                }
                catch (SystemException ex) when (ex.Message.Contains("Win32"))
                {
                    LogWarn($"Remote queue connection failure: {ex.Message}");
                    return false;
                }
            }

            bool success = Retry.UntilTrue(connect, 10, TimeSpan.FromSeconds(5));
            if (success)
            {
                LogInfo($"Successfully connected to remote queue '{printerPath}'.");
            }
            else
            {
                LogWarn($"Unable to connect to remote queue '{printerPath}'.");
            }
            return success;
        }

        /// <summary>
        /// Sets the default print queue to the specified queue.
        /// </summary>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to set as the default.</param>
        /// <exception cref="ArgumentNullException"><paramref name="printQueue" /> is null.</exception>
        public static void SetDefaultQueue(PrintQueue printQueue)
        {
            if (printQueue == null)
            {
                throw new ArgumentNullException(nameof(printQueue));
            }

            LogDebug($"Setting {printQueue.FullName} as the default queue.");
            string printerPortKey = PrintRegistryUtil.GetPrinterPortValue(printQueue) ?? "winspool,Ne00:";
            PrintRegistryUtil.SetDefaultDeviceKey(printQueue, printerPortKey);
        }

        /// <summary>
        /// Changes the attributes of the specified print queue.
        /// </summary>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to modify.</param>
        /// <param name="attributes">The attributes to modify.</param>
        /// <param name="setAttributes">if set to <c>true</c> add the specified attributes; otherwise, remove those attributes.</param>
        /// <exception cref="ArgumentNullException"><paramref name="printQueue" /> is null.</exception>
        public static void ChangeAttributes(PrintQueue printQueue, PrintQueueAttributes attributes, bool setAttributes)
        {
            if (printQueue == null)
            {
                throw new ArgumentNullException(nameof(printQueue));
            }

            PrintQueueAttributes originalAttributes = printQueue.QueueAttributes;
            PrintQueueAttributes modifiedAttributes = setAttributes ?
                originalAttributes | attributes :
                originalAttributes & ~attributes;

            if (modifiedAttributes != originalAttributes)
            {
                PrintRegistryUtil.SetPrinterAttributes(printQueue, modifiedAttributes);

                // The print spooler must be restarted for this change to take effect.
                PrintSpooler.RestartSpooler();
            }
        }

        /// <summary>
        /// Gets the job render location for the specified print queue.
        /// </summary>
        /// <param name="queue">The <see cref="PrintQueue" /> to query.</param>
        /// <returns>The <see cref="PrintJobRenderLocation" /> for the queue.</returns>
        public static PrintJobRenderLocation GetJobRenderLocation(PrintQueue queue)
        {
            // This feature only applies to Vista and greater
            if (Environment.OSVersion.Version.Major >= 6)
            {
                return PrintRegistryUtil.GetJobRenderLocation(queue);
            }
            else
            {
                return PrintJobRenderLocation.Unknown;
            }
        }

        /// <summary>
        /// Sets the job render location for the specified print queue.
        /// </summary>
        /// <param name="queue">The <see cref="PrintQueue" /> to modify.</param>
        /// <param name="location">The <see cref="PrintJobRenderLocation" />.</param>
        /// <exception cref="ArgumentException"><paramref name="location" /> is <see cref="PrintJobRenderLocation.Unknown" /></exception>
        public static void SetJobRenderLocation(PrintQueue queue, PrintJobRenderLocation location)
        {
            // This feature only applies to Vista and greater
            if (Environment.OSVersion.Version.Major >= 6)
            {
                PrintRegistryUtil.SetJobRenderLocation(queue, location);
            }
        }

        /// <summary>
        /// Constructs the full printer name for the specified <see cref="RemotePrintQueueInfo" />.
        /// </summary>
        /// <param name="queueInfo">The <see cref="RemotePrintQueueInfo" /> object.</param>
        /// <returns>The full printer name for the specified <see cref="RemotePrintQueueInfo" />.</returns>
        public static string GetPrinterName(this RemotePrintQueueInfo queueInfo)
        {
            if (queueInfo == null)
            {
                throw new ArgumentNullException(nameof(queueInfo));
            }

            return string.Format(@"\\{0}\{1}", queueInfo.ServerHostName, queueInfo.QueueName);
        }
    }
}
