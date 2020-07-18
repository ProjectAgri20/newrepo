using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Printing;
using System.Threading;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print.FilePrinters
{
    /// <summary>
    /// A <see cref="FilePrinter" /> that can print PDF files.
    /// </summary>
    [ObjectFactory("PDF")]
    internal sealed class PdfFilePrinter : ShellFilePrinter
    {
        private const string _applicationName = "AcroRd32";

        /// <summary>
        /// Initializes a new instance of the <see cref="PdfFilePrinter" /> class.
        /// </summary>
        /// <param name="file">The file to be printed by this <see cref="PdfFilePrinter" />.</param>
        public PdfFilePrinter(FileInfo file)
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
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            if (printQueue == null)
            {
                throw new ArgumentNullException(nameof(printQueue));
            }

            // Adobe Reader provides command line arguments that can be used to launch a new instance,
            // print a document, and then close automatically.  If we wait for the process to finish,
            // we know that the job is done printing.

            // However... Adobe Reader refuses to automatically close if it is the only instance of Reader
            // running on the machine.  In order to force the process to end so that we are notified
            // when the job is finished printing, we have to make sure there is a "dummy" instance of
            // Reader running.  This "dummy" instance serves no purpose other than to make sure that the
            // "real" instances we use to print will exit when they are done.

            // NOTE: The dummy cannot be cleaned up after the print is complete.  This is because
            // there is no way to differentiate between the "dummy" we created and other legitimate
            // printing processes that may be happening on other threads.  The dummy is an unfortunate
            // leftover from the automation process.

            // To avoid spawning multiple dummies, only create one if no dummy currently exists
            if (!Process.GetProcessesByName(_applicationName).Any())
            {
                LogTrace($"Creating dummy PDF process.");
                Process.Start(_applicationName, "/h");
                Thread.Sleep(5000);
            }

            string args = string.Format(" /N /T \"{0}\" \"{1}\"", file.FullName, printQueue.FullName);
            ProcessStartInfo processStartInfo = new ProcessStartInfo(_applicationName, args);
            return PrintFileViaProcess(file, processStartInfo, false);
        }
    }
}
