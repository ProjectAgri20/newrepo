using System;
using System.IO;
using System.Linq;
using System.Printing;
using System.Printing.IndexedProperties;
using System.Threading;
using HP.ScalableTest.Utility;
using HP.ScalableTest.WindowsAutomation.Registry;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// Installs print queues on the local machine.
    /// </summary>
    public static class PrintQueueInstaller
    {
        /// <summary>
        /// Determines whether the specified queue is installed.
        /// </summary>
        /// <param name="queueName">The name of the queue to check.</param>
        /// <returns><c>true</c> if the specified queue is installed; otherwise, <c>false</c>.</returns>
        public static bool IsInstalled(string queueName)
        {
            return PrintQueueController.GetPrintQueues().Any(n => n.FullName.EqualsIgnoreCase(queueName));
        }

        /// <summary>
        /// Creates a print queue using the specified name, driver, port, and print processor.  (The driver and port must already be installed.)
        /// </summary>
        /// <param name="queueName">The name of the queue to be created.</param>
        /// <param name="driverName">The driver name.</param>
        /// <param name="portName">The port name.</param>
        /// <param name="printProcessorName">The print processor.</param>
        /// <exception cref="PrintQueueInstallationException">An error occurred while creating the queue.</exception>
        public static void CreatePrintQueue(string queueName, string driverName, string portName, string printProcessorName)
        {
            LogInfo($"Installing print queue: {queueName}, {driverName}, {portName}, {printProcessorName}");
            try
            {
                using (LocalPrintServer server = new LocalPrintServer(PrintSystemDesiredAccess.AdministrateServer))
                {
                    server.InstallPrintQueue(queueName, driverName, new[] { portName }, printProcessorName, new PrintPropertyDictionary());
                    server.Commit();
                }
            }
            catch (Exception ex)
            {
                throw new PrintQueueInstallationException(ex.Message, ex);
            }
        }

        /// <summary>
        /// Installs a print queue using the UPD installer executable.
        /// </summary>
        /// <param name="updInstaller">The UPD installer executable.</param>
        /// <param name="queueName">The name of the queue to be created.</param>
        /// <param name="deviceAddress">The device address.</param>
        /// <exception cref="ArgumentNullException"><paramref name="updInstaller" /> is null.</exception>
        public static void InstallUpdPrinter(FileInfo updInstaller, string queueName, string deviceAddress)
        {
            string cmd = $"/q /nd /npf /sm{deviceAddress} /n\"{queueName}\"";
            InstallUpdPrinter(updInstaller, cmd);
        }

        /// <summary>
        /// Installs a print queue using the UPD installer executable.
        /// </summary>
        /// <param name="updInstaller">The UPD installer.</param>
        /// <param name="queueName">The name of the queue to be created.</param>
        /// <param name="deviceAddress">The device address.</param>
        /// <param name="cfmFile">The CFM file.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="updInstaller" /> is null.
        /// <para>or</para>
        /// <paramref name="cfmFile" /> is null.
        /// </exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1011:ConsiderPassingBaseTypesAsParameters")]
        public static void InstallUpdPrinter(FileInfo updInstaller, string queueName, string deviceAddress, FileInfo cfmFile)
        {
            if (cfmFile == null)
            {
                throw new ArgumentNullException(nameof(cfmFile));
            }

            string cmd = $"/q /nd /npf /sm{deviceAddress} /n\"{queueName}\" /gcfm\"{cfmFile.FullName}\"";
            InstallUpdPrinter(updInstaller, cmd);
        }

        private static void InstallUpdPrinter(FileInfo updInstaller, string arguments)
        {
            if (updInstaller == null)
            {
                throw new ArgumentNullException(nameof(updInstaller));
            }

            LogInfo($"Installing UPD print queue: {updInstaller} {arguments}");
            ProcessExecutionResult result = ProcessUtil.Execute(updInstaller.FullName, arguments);
            if (!result.SuccessfulExit || result.ExitCode != 0)
            {
                throw new PrintQueueInstallationException($"UPD printer installation failed: {result.StandardError}");
            }
        }

        /// <summary>
        /// Waits for installation of the specified queue to be complete.
        /// </summary>
        /// <param name="queueName">The name of the queue to wait for.</param>
        /// <param name="driverName">The name of the driver associated with the queue.</param>
        /// <exception cref="TimeoutException">A timeout was reached while waiting for installation to complete.</exception>
        public static void WaitForInstallationComplete(string queueName, string driverName)
        {
            // The InstallationComplete flag is only set for UPD drivers.
            if (string.IsNullOrEmpty(driverName) || !driverName.StartsWith("HP Universal", StringComparison.OrdinalIgnoreCase))
            {
                return;
            }

            LogInfo("Waiting for installation to complete...");
            using (AutoResetEvent queueWaitEvent = new AutoResetEvent(false))
            {
                using (var watcher = new RegistryValueEventWatcher(RegistryHive.LocalMachine, $@"SYSTEM\\CurrentControlSet\\Control\\Print\\Printers\\{queueName}\\PrinterDriverData", "InstallationComplete"))
                {
                    watcher.RegistryChanged += (s, e) =>
                    {
                        if (PrintRegistryUtil.GetQueueInstallationCompleteStatus(queueName))
                        {
                            queueWaitEvent.Set();
                        }
                    };
                    watcher.Start();

                    // Perform a manual check in case the value is already set to 0,
                    // or the change slipped in before the event watcher was started.
                    bool alreadyDone = PrintRegistryUtil.GetQueueInstallationCompleteStatus(queueName);
                    if (!alreadyDone && !queueWaitEvent.WaitOne(TimeSpan.FromMinutes(2)))
                    {
                        throw new TimeoutException("Timeout reached waiting for queue.");
                    }
                }
            }
            LogInfo("Installation complete");
        }
    }
}
