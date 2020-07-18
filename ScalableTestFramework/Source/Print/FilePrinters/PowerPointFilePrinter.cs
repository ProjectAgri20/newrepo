using System;
using System.Globalization;
using System.IO;
using System.Printing;
using System.Runtime.InteropServices;
using HP.ScalableTest.Utility;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;

namespace HP.ScalableTest.Print.FilePrinters
{
    /// <summary>
    /// A <see cref="FilePrinter" /> that can print Microsoft PowerPoint files.
    /// </summary>
    [ObjectFactory("PPT")]
    [ObjectFactory("PPTX")]
    internal sealed class PowerPointFilePrinter : MSOfficeInteropFilePrinter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PowerPointFilePrinter" /> class.
        /// </summary>
        /// <param name="file">The file to be printed by this <see cref="PowerPointFilePrinter" />.</param>
        public PowerPointFilePrinter(FileInfo file)
            : base(file)
        {
        }

        /// <summary>
        /// Prints a document using interop services.
        /// </summary>
        /// <param name="file">The file to print.</param>
        /// <param name="printQueue">The <see cref="PrintQueue" /> to print the file to.</param>
        /// <param name="printOptions">The <see cref="FilePrintOptions" /> to use for printing the file.</param>
        /// <returns>A <see cref="FilePrintResult" /> object representing the outcome of the print operation.</returns>
        protected override FilePrintResult PrintInterop(FileInfo file, PrintQueue printQueue, FilePrintOptions printOptions)
        {
            ProcessUtil.KillProcess("POWERPNT");

            Application powerpoint = null;
            Presentations presentations = null;
            Presentation presentation = null;
            try
            {
                OnStatusChanged("Starting PowerPoint");
                powerpoint = new Application();
                powerpoint.DisplayAlerts = PpAlertLevel.ppAlertsNone;

                OnStatusChanged("Opening file: " + file.Name);
                presentations = powerpoint.Presentations;
                presentation = presentations.Open(file.FullName,
                                                  ReadOnly: MsoTriState.msoCTrue,
                                                  WithWindow: MsoTriState.msoFalse);

                // Check for the word "on" in the queue name.  If it exists, include the port in the name.
                string queueName = printQueue.FullName;
                if (queueName.Contains(" on ") || queueName.StartsWith("on ", false, CultureInfo.CurrentCulture) || queueName.EndsWith(" on", false, CultureInfo.CurrentCulture))
                {
                    queueName = GetQueueNameWithPort(printQueue);
                }

                try
                {
                    OnStatusChanged($"Setting active printer to {queueName}");
                    presentation.PrintOptions.ActivePrinter = queueName;
                }
                catch
                {
                    throw new FilePrintException($"PowerPoint cannot print to '{queueName}'.  Check the formatting of the queue name.");
                }

                OnStatusChanged("Printing to: " + queueName);
                DateTimeOffset startTime = DateTimeOffset.Now;
                presentation.PrintOptions.PrintInBackground = MsoTriState.msoFalse;
                presentation.PrintOut(Copies: printOptions.Copies, Collate: MsoTriState.msoFalse);
                DateTimeOffset endtime = DateTimeOffset.Now;

                OnStatusChanged("Closing presentation...");
                presentation.Close();

                OnStatusChanged("Quitting PowerPoint.");
                powerpoint.Quit();

                return new FilePrintResult(startTime, endtime);
            }
            finally
            {
                if (presentation != null)
                {
                    Marshal.FinalReleaseComObject(presentation);
                    presentation = null;
                }

                if (presentations != null)
                {
                    Marshal.FinalReleaseComObject(presentations);
                    presentations = null;
                }

                if (powerpoint != null)
                {
                    Marshal.FinalReleaseComObject(powerpoint);
                    powerpoint = null;
                }

                GarbageCollect();
            }
        }
    }
}
