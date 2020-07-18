using System;
using System.IO;
using System.Printing;
using System.Runtime.InteropServices;
using HP.ScalableTest.Utility;
using Microsoft.Office.Interop.Excel;

namespace HP.ScalableTest.Print.FilePrinters
{
    /// <summary>
    /// A <see cref="FilePrinter" /> that can print Microsoft Excel files.
    /// </summary>
    [ObjectFactory("XLS")]
    [ObjectFactory("XLSX")]
    internal sealed class ExcelFilePrinter : MSOfficeInteropFilePrinter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFilePrinter" /> class.
        /// </summary>
        /// <param name="file">The file to be printed by this <see cref="ExcelFilePrinter" />.</param>
        public ExcelFilePrinter(FileInfo file)
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
            ProcessUtil.KillProcess("EXCEL");

            Application excel = null;
            Workbooks wbooks = null;
            Workbook wbook = null;

            try
            {
                OnStatusChanged("Starting Excel");
                excel = new Application { DisplayAlerts = false };

                OnStatusChanged("Opening file: " + file.Name);
                wbooks = excel.Workbooks;
                wbook = wbooks.Open(file.FullName,
                                    UpdateLinks: 0,
                                    ReadOnly: true,
                                    IgnoreReadOnlyRecommended: true,
                                    CorruptLoad: XlCorruptLoad.xlRepairFile);

                // Excel 2010 must have the port appended to the queue name in order to print.
                string queueName = GetQueueNameWithPort(printQueue);
                try
                {
                    OnStatusChanged($"Setting active printer to {queueName}");
                    excel.ActivePrinter = queueName;
                }
                catch (Exception ex)
                {
                    throw new FilePrintException($"Excel cannot print to '{queueName}'.  Check the formatting of the queue name.", ex);
                }

                OnStatusChanged("Printing to: " + queueName);
                DateTimeOffset startTime = DateTimeOffset.Now;

                //few excel documents threw exception when using sheets.Printout method but didn't when using the workbook method. We are now using workbook method to print
                wbook.PrintOut(Copies: printOptions.Copies, Preview: false);
                DateTimeOffset endtime = DateTimeOffset.Now;

                OnStatusChanged("Closing workbook...");
                wbook.Close();
                wbooks.Close();

                OnStatusChanged("Quitting Excel.");
                excel.Quit();

                return new FilePrintResult(startTime, endtime);
            }
            finally
            {
                if (wbook != null)
                {
                    Marshal.FinalReleaseComObject(wbook);
                    wbook = null;
                }

                if (wbooks != null)
                {
                    Marshal.FinalReleaseComObject(wbooks);
                    wbooks = null;
                }

                if (excel != null)
                {
                    Marshal.FinalReleaseComObject(excel);
                    excel = null;
                }

                GarbageCollect();
            }
        }

    }
}
