using System;
using System.Diagnostics;
using System.Globalization;
using System.IO;
using System.Printing;

namespace HP.ScalableTest.Print.FilePrinters
{
    /// <summary>
    /// A general-purpose <see cref="FilePrinter" /> that uses Windows print verbs.
    /// </summary>
    internal class GenericFilePrinter : ShellFilePrinter
    {
        protected enum PrintVerb
        {
            Print,
            PrintTo
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="GenericFilePrinter" /> class.
        /// </summary>
        /// <param name="file">The file to be printed by this <see cref="GenericFilePrinter" />.</param>
        public GenericFilePrinter(FileInfo file)
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
        protected override FilePrintResult PrintFile(FileInfo file, PrintQueue printQueue, FilePrintOptions printOptions)
        {
            // By default, use the PrintTo verb
            return PrintFile(file, printQueue, PrintVerb.PrintTo);
        }

        /// <summary>
        /// Prints the specified file to the specified print queue.
        /// </summary>
        /// <param name="file">The file to print.</param>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to print the file to.</param>
        /// <param name="printVerb">The <see cref="PrintVerb" /> to use.</param>
        /// <returns>A <see cref="FilePrintResult" /> object representing the outcome of the print operation.</returns>
        /// <exception cref="FilePrintException">An error occurred while printing the file.</exception>
        protected FilePrintResult PrintFile(FileInfo file, PrintQueue printQueue, PrintVerb printVerb)
        {
            OnStatusChanged($"Printing document using the {printVerb} verb.");

            // If the verb being used is the Print verb, the default printer must be changed.
            if (printVerb == PrintVerb.Print)
            {
                PrintQueueController.SetDefaultQueue(printQueue);
            }

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = file.FullName,
                Verb = printVerb.ToString().ToLower(CultureInfo.CurrentCulture),
                Arguments = string.Format("\"{0}\"", printQueue.FullName),
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true,
                ErrorDialog = false
            };

            return PrintFileViaProcess(file, processStartInfo);
        }
    }
}
