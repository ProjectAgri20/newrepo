using System;
using System.IO;
using System.Printing;
using System.Threading.Tasks;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// Simple class for handling print jobs.
    /// </summary>
    public sealed class PrintingEngine
    {
        /// <summary>
        /// Occurs when the status of the print operation has changed.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Gets the <see cref="FilePrintOptions" /> that will be used when printing a file.
        /// </summary>
        public FilePrintOptions PrintOptions { get; } = new FilePrintOptions();

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintingEngine" /> class.
        /// </summary>
        public PrintingEngine()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Prints the specified file to the specified <see cref="PrintQueue" />.
        /// </summary>
        /// <param name="fileName">The file to print.</param>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to print the file to.</param>
        /// <returns>A <see cref="PrintingEngineResult" /> object representing the outcome of the print operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName" /> is null.
        /// <para>or</para>
        /// <paramref name="printQueue" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="fileName" /> is empty, contains only white spaces, or contains invalid characters.</exception>
        public PrintingEngineResult Print(string fileName, PrintQueue printQueue)
        {
            return Print(new FileInfo(fileName), printQueue);
        }

        /// <summary>
        /// Prints the specified file to the specified <see cref="PrintQueue" />.
        /// </summary>
        /// <param name="fileName">The file to print.</param>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to print the file to.</param>
        /// <param name="fileId">The <see cref="Guid" /> to use for the unique file name.</param>
        /// <returns>A <see cref="PrintingEngineResult" /> object representing the outcome of the print operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="fileName" /> is null.
        /// <para>or</para>
        /// <paramref name="printQueue" /> is null.
        /// </exception>
        /// <exception cref="ArgumentException"><paramref name="fileName" /> is empty, contains only white spaces, or contains invalid characters.</exception>
        public PrintingEngineResult Print(string fileName, PrintQueue printQueue, Guid fileId)
        {
            return Print(new FileInfo(fileName), printQueue, fileId);
        }

        /// <summary>
        /// Prints the specified file to the specified <see cref="PrintQueue" />.
        /// </summary>
        /// <param name="file">The file to print.</param>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to print the file to.</param>
        /// <returns>A <see cref="PrintingEngineResult" /> object representing the outcome of the print operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file" /> is null.
        /// <para>or</para>
        /// <paramref name="printQueue" /> is null.
        /// </exception>
        public PrintingEngineResult Print(FileInfo file, PrintQueue printQueue)
        {
            return Print(file, printQueue, SequentialGuid.NewGuid());
        }

        /// <summary>
        /// Prints the specified file to the specified <see cref="PrintQueue" />.
        /// </summary>
        /// <param name="file">The file to print.</param>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to print the file to.</param>
        /// <param name="fileId">The <see cref="Guid" /> to use for the unique file name.</param>
        /// <returns>A <see cref="PrintingEngineResult" /> object representing the outcome of the print operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="file" /> is null.
        /// <para>or</para>
        /// <paramref name="printQueue" /> is null.
        /// </exception>
        public PrintingEngineResult Print(FileInfo file, PrintQueue printQueue, Guid fileId)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (printQueue == null)
            {
                throw new ArgumentNullException(nameof(printQueue));
            }

            UniqueFile uniqueFile = null;
            try
            {
                LogStatus($"Printing {file.Name} to {printQueue.FullName}");

                // Create a uniquely named file to identify the print job
                uniqueFile = UniqueFile.Create(file, fileId);

                // Create the file printer
                FilePrinter filePrinter = FilePrinterFactory.Create(uniqueFile);
                filePrinter.StatusChanged += (s, e) => StatusChanged?.Invoke(s, e);

                // Print the file and return the result
                DateTimeOffset jobStart = DateTimeOffset.Now;
                FilePrintResult printResult = filePrinter.Print(printQueue, PrintOptions);
                DateTimeOffset jobEnd = DateTimeOffset.Now;

                LogStatus($"Finished printing {file.Name}");
                return new PrintingEngineResult(uniqueFile, printResult, jobStart, jobEnd);
            }
            finally
            {
                Task.Factory.StartNew(() => DeleteTemporaryFile(uniqueFile));
            }
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes")]
        private static void DeleteTemporaryFile(UniqueFile file)
        {
            if (file != null)
            {
                try
                {
                    Retry.WhileThrowing(() => File.Delete(file.FileInfo.FullName), 100, TimeSpan.FromSeconds(3), new[] { typeof(UnauthorizedAccessException), typeof(IOException) });
                }
                catch (Exception ex)
                {
                    // Don't allow the parent thread to crash if the delete is unsuccessful.
                    LogWarn($"Unable to delete temporary file {file.FileInfo.FullName}: {ex.Message}");
                }
            }
        }

        private void LogStatus(string message)
        {
            LogInfo(message);
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(message));
        }
    }
}
