using System;
using System.Diagnostics;
using System.IO;

namespace HP.ScalableTest.Print.FilePrinters
{
    /// <summary>
    /// Base class for printing by creating an external process.
    /// </summary>
    internal abstract class ShellFilePrinter : FilePrinter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShellFilePrinter" /> class.
        /// </summary>
        /// <param name="file">The file to be printed by this <see cref="ShellFilePrinter" />.</param>
        protected ShellFilePrinter(FileInfo file)
            : base(file)
        {
        }

        /// <summary>
        /// Prints the file via process.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <param name="processStartInfo">The <see cref="ProcessStartInfo" /> used to run the external process.</param>
        /// <param name="checkExitCode">if set to <c>true</c> check the process exit code to ensure it is 0 and throw an exception otherwise.</param>
        /// <returns>A <see cref="FilePrintResult" /> object representing the outcome of the print operation.</returns>
        /// <exception cref="FilePrintException">An error occurred while printing the file.</exception>
        protected FilePrintResult PrintFileViaProcess(FileInfo file, ProcessStartInfo processStartInfo, bool checkExitCode = true)
        {
            OnStatusChanged($"Printing file: {file.Name}");
            DateTimeOffset startTime = DateTimeOffset.Now;
            using (Process process = Process.Start(processStartInfo))
            {
                OnStatusChanged("Waiting for the file to print.");
                if (!process.HasExited)
                {
                    TimeSpan timeout = TimeSpan.FromMinutes(10);
                    if (!process.WaitForExit((int)timeout.TotalMilliseconds))
                    {
                        process.Kill();
                        throw new FilePrintException($"Job failed to print within {timeout.Minutes} minutes.");
                    }
                }

                if (checkExitCode && process.ExitCode != 0)
                {
                    throw new FilePrintException($"Process failed with exit code: {process.ExitCode}");
                }

                OnStatusChanged($"File finished printing.");
                return new FilePrintResult(startTime, DateTimeOffset.Now);
            }
        }
    }
}
