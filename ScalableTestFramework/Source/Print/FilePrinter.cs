using System;
using System.IO;
using System.Printing;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// Prints local files to print queues.
    /// </summary>
    public abstract class FilePrinter
    {
        /// <summary>
        /// Occurs when the status of the print operation has changed.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusChanged;

        /// <summary>
        /// Gets a <see cref="FileInfo" /> object representing the file being printed by this <see cref="FilePrinter" />.
        /// </summary>
        public FileInfo File { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FilePrinter" /> class.
        /// </summary>
        /// <param name="file">The file to be printed by this <see cref="FilePrinter" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="file" /> is null.</exception>
        protected FilePrinter(FileInfo file)
        {
            File = file ?? throw new ArgumentNullException(nameof(file));
        }

        /// <summary>
        /// Prints the file to the specified print queue.
        /// </summary>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to print the file to.</param>
        /// <returns>A <see cref="FilePrintResult" /> object representing the outcome of the print operation.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="printQueue" /> is null.</exception>
        /// <exception cref="FilePrintException">An error occurred while printing the file.</exception>
        public FilePrintResult Print(PrintQueue printQueue)
        {
            return Print(printQueue, new FilePrintOptions());
        }

        /// <summary>
        /// Prints the file to the specified print queue.
        /// </summary>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to print the file to.</param>
        /// <param name="printOptions">The <see cref="FilePrintOptions" /> to use for printing the file.</param>
        /// <returns>A <see cref="FilePrintResult" /> object representing the outcome of the print operation.</returns>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="printQueue" /> is null.
        /// <para>or</para>
        /// <paramref name="printOptions" /> is null.
        /// </exception>
        /// <exception cref="FilePrintException">An error occurred while printing the file.</exception>
        public FilePrintResult Print(PrintQueue printQueue, FilePrintOptions printOptions)
        {
            if (printQueue == null)
            {
                throw new ArgumentNullException(nameof(printQueue));
            }

            if (printOptions == null)
            {
                throw new ArgumentNullException(nameof(printOptions));
            }

            return PrintFile(File, printQueue, printOptions);
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
        protected abstract FilePrintResult PrintFile(FileInfo file, PrintQueue printQueue, FilePrintOptions printOptions);

        /// <summary>
        /// Called when the status of the print operation has changed.
        /// </summary>
        /// <param name="statusMessage">The status message.</param>
        protected void OnStatusChanged(string statusMessage)
        {
            LogDebug(statusMessage);
            StatusChanged?.Invoke(this, new StatusChangedEventArgs(statusMessage));
        }
    }
}
