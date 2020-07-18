using System;
using System.Collections.Generic;
using System.IO;
using System.Printing;
using System.Runtime.InteropServices;
using System.Threading;
using HP.ScalableTest.Utility;
using HP.ScalableTest.WindowsAutomation;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print.FilePrinters
{
    /// <summary>
    /// Base class for Microsoft Office application printing using COM interop.
    /// </summary>
    internal abstract class MSOfficeInteropFilePrinter : FilePrinter
    {
        // HRESULT that is returned when a timeout occurs.
        private const int _hResultTimeout = -2146959355; // 0x80080005

        // Tracks which users have printed an Office document.
        private static readonly List<string> _users = new List<string>();

        // Cache for print queue names with port info appended
        private static readonly Dictionary<string, string> _printQueueNamesWithPorts = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="MSOfficeInteropFilePrinter" /> class.
        /// </summary>
        /// <param name="file">The file to be printed by this <see cref="MSOfficeInteropFilePrinter" />.</param>
        protected MSOfficeInteropFilePrinter(FileInfo file)
            : base(file)
        {
        }

        /// <summary>
        /// Prints the specified file to the specified print queue.
        /// </summary>
        /// <param name="file">The file to print.</param>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to print the file to.</param>
        /// <param name="printOptions">The <see cref="FilePrintOptions" /> to use for printing the file.</param>
        /// <returns>A <see cref="FilePrintResult" /> object representing the outcome of the print operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file" /> is null.
        /// <para>or</para>
        /// <paramref name="printQueue" /> is null.
        /// </exception>
        /// <exception cref="FilePrintException">An error occurred while printing the file.</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        protected sealed override FilePrintResult PrintFile(FileInfo file, PrintQueue printQueue, FilePrintOptions printOptions)
        {
            // Check to see if this is the first time this user has printed with MS office.
            if (!_users.Contains(Environment.UserName))
            {
                OnStatusChanged($"Applying first run policies for {Environment.UserName}.");
                ApplyFirstRunPolicies();
                _users.Add(Environment.UserName);
            }

            // Print the document using COM interop, retrying in the event of timeouts and enforcing a 10 minute time limit.
            try
            {
                int retries = 0;
                while (true)
                {
                    try
                    {
                        FilePrintResult result = null;
                        void action() => result = PrintInterop(file, printQueue, printOptions);
                        using (InteropTimedTask task = new InteropTimedTask(TimeSpan.FromMinutes(10)))
                        {
                            OnStatusChanged("Beginning interop print.");
                            task.Run(action);
                            OnStatusChanged("Document finished printing.");
                        }
                        return result;
                    }
                    catch (COMException ex) when (ex.ErrorCode == _hResultTimeout && retries < 5)
                    {
                        retries++;
                        OnStatusChanged($"Caught exception: {ex.Message}, trying again (attempt #{retries})");
                    }
                    catch (COMException ex)
                    {
                        OnStatusChanged($"Caught exception: {ex.Message}, aborting print.");
                        throw new FilePrintException(ex.Message, ex);
                    }
                }
            }
            finally
            {
                DeleteTempFiles();
            }
        }

        /// <summary>
        /// Prints a document using interop services.  Must be implemented by inheriting classes.
        /// </summary>
        /// <param name="file">The file to print.</param>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to print the file to.</param>
        /// <param name="printOptions">The <see cref="FilePrintOptions" /> to use for printing the file.</param>
        /// <returns>A <see cref="FilePrintResult" /> object representing the outcome of the print operation.</returns>
        protected abstract FilePrintResult PrintInterop(FileInfo file, PrintQueue printQueue, FilePrintOptions printOptions);

        /// <summary>
        /// Gets a detailed queue name with network port, as sometimes required by Office interop.
        /// (Results are cached to minimize registry access.)
        /// </summary>
        /// <param name="printQueue">The <see cref="PrintQueue" />.</param>
        /// <returns>The full name of the print queue with the port appended to the end.</returns>
        protected static string GetQueueNameWithPort(PrintQueue printQueue)
        {
            string queueName = printQueue.FullName;

            // Retrieving the port name for a queue requires digging into (and sometimes modifying) the registry.
            // Cache the values to minimize the number of times we do this.
            if (!_printQueueNamesWithPorts.ContainsKey(queueName))
            {
                string queueNameWithPort = PrintRegistryUtil.GetQueueNameWithPort(printQueue);
                _printQueueNamesWithPorts.Add(queueName, queueNameWithPort);
            }

            return _printQueueNamesWithPorts[queueName];
        }

        /// <summary>
        /// Forces garbage collection to reclaim app resources.
        /// </summary>
        protected static void GarbageCollect()
        {
            // Force garbage collection repeatedly per recommendation from Microsoft
            for (int i = 0; i < 2; i++)
            {
                GC.Collect();
                GC.WaitForPendingFinalizers();
            }
        }

        private static void DeleteTempFiles()
        {
            // Office will generate temp files, but the files are not cleaned up.  As a result, the temp files build up
            // until the client runs out of space.  We need to remove these files.
            string path = Path.Combine(Path.GetTempPath(), @"\..\Temporary Internet Files\Content.MSO");

            if (Directory.Exists(path))
            {
                try
                {
                    LogTrace("Deleting temporary files...");
                    DirectoryInfo directory = new DirectoryInfo(path);
                    FileSystem.EmptyDirectory(directory);
                }
                catch (UnauthorizedAccessException)
                {
                    // Do nothing
                }
            }
        }

        private static void ApplyFirstRunPolicies()
        {
            // Office 2010 has a new (since Office 2007) Opt-in wizard which prompts the user to set up automatic updates.
            // A group policy setting can suppress it, but with multiple processes running Office on the same machine,
            // a race condition is created where some policies are set before others.  This method ensures that the OS
            // has enough time to apply the policies before printing operations begin.
            var wordApp = new Microsoft.Office.Interop.Word.Application();
            try
            {
                Delay.Wait(TimeSpan.FromSeconds(3));
            }
            finally
            {
                wordApp.Quit(Microsoft.Office.Interop.Word.WdSaveOptions.wdDoNotSaveChanges);
            }
        }

        /// <summary>
        /// Encapsulates an interop task and enforces a time limit.
        /// </summary>
        private sealed class InteropTimedTask : IDisposable
        {
            private readonly TimeSpan _timeout;
            private readonly Thread _target;
            private Thread _watchdog;

            public InteropTimedTask(TimeSpan timeout)
            {
                _timeout = timeout;
                _target = Thread.CurrentThread;
            }

            public void Run(Action task)
            {
                StartWatchdog();
                try
                {
                    task();
                }
                catch (ThreadAbortException)
                {
                    Thread.ResetAbort();
                }
                finally
                {
                    StopWatchdog();
                }
            }

            private void StartWatchdog()
            {
                _watchdog = new Thread(WatchdogTask);
                _watchdog.IsBackground = true;
                _watchdog.Start();
            }

            private void StopWatchdog()
            {
                if (_watchdog?.IsAlive == true)
                {
                    _watchdog.Abort();
                }
            }

            private void WatchdogTask(object notUsed)
            {
                // Wait for the specified amount of time
                Thread.Sleep(_timeout);

                // If the other thread finishes within the timeout, it will abort this thread.
                // Otherwise, terminate the watched thread.
                LogWarn("Timeout reached - aborting worker thread.");
                _target.Abort();
            }

            public void Dispose()
            {
                StopWatchdog();
            }
        }
    }
}
