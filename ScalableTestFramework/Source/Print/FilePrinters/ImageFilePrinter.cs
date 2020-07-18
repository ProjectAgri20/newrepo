using System;
using System.Diagnostics;
using System.IO;
using System.Printing;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Print.FilePrinters
{
    /// <summary>
    /// A <see cref="FilePrinter" /> that can print image files.
    /// </summary>
    [ObjectFactory("JPG")]
    [ObjectFactory("JPEG")]
    [ObjectFactory("PNG")]
    [ObjectFactory("BMP")]
    [ObjectFactory("TIF")]
    internal sealed class ImageFilePrinter : ShellFilePrinter
    {
        private const string _applicationName = "mspaint";

        /// <summary>
        /// Initializes a new instance of the <see cref="ImageFilePrinter"/> class.
        /// </summary>
        /// <param name="file">The file to be printed by this <see cref="ImageFilePrinter"/>.</param>
        public ImageFilePrinter(FileInfo file)
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
        /// <paramref name="file"/> is null.
        /// <para>or</para>
        /// <paramref name="printQueue"/> is null.
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

            ProcessStartInfo processStartInfo = new ProcessStartInfo
            {
                FileName = _applicationName,
                Arguments = string.Format(" /PT \"{0}\" \"{1}\"", file.FullName, printQueue.FullName),
                CreateNoWindow = true,
                WindowStyle = ProcessWindowStyle.Hidden,
                UseShellExecute = true,
                ErrorDialog = false
            };

            return PrintFileViaProcess(file, processStartInfo);
        }
    }
}
