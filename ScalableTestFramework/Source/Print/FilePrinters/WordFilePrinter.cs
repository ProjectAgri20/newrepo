using System;
using System.Globalization;
using System.IO;
using System.Printing;
using System.Runtime.InteropServices;
using HP.ScalableTest.Utility;
using Microsoft.Office.Interop.Word;

namespace HP.ScalableTest.Print.FilePrinters
{
    /// <summary>
    /// A <see cref="FilePrinter" /> that can print Microsoft Word files.
    /// </summary>
    [ObjectFactory("DOC")]
    [ObjectFactory("DOCX")]
    [ObjectFactory("RTF")]
    internal sealed class WordFilePrinter : MSOfficeInteropFilePrinter
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WordFilePrinter" /> class.
        /// </summary>
        /// <param name="file">The file to be printed by this <see cref="WordFilePrinter" />.</param>
        public WordFilePrinter(FileInfo file)
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
            ProcessUtil.KillProcess("WINWORD");

            Application word = null;
            Documents documents = null;
            Document document = null;
            try
            {
                OnStatusChanged("Starting Word");
                word = new Application();

                OnStatusChanged("Opening file: " + file.Name);
                documents = word.Documents;
                document = documents.Open(file.FullName, ReadOnly: true, Visible: true);

                // Check for the word "on" in the queue name.  If it exists, include the port in the name.
                string queueName = printQueue.FullName;
                if (queueName.Contains(" on ") || queueName.StartsWith("on ", false, CultureInfo.CurrentCulture) || queueName.EndsWith(" on", false, CultureInfo.CurrentCulture))
                {
                    queueName = GetQueueNameWithPort(printQueue);
                }

                try
                {
                    OnStatusChanged($"Setting active printer to {queueName}");
                    word.ActivePrinter = queueName;
                }
                catch
                {
                    throw new FilePrintException($"Word cannot print to '{queueName}'.  Check the formatting of the queue name.");
                }

                OnStatusChanged("Printing to: " + queueName);
                DateTimeOffset startTime = DateTimeOffset.Now;
                document.PrintOut(Background: false,
                                  Append: false,
                                  Range: WdPrintOutRange.wdPrintAllDocument,
                                  Item: WdPrintOutItem.wdPrintDocumentContent,
                                  Copies: printOptions.Copies.ToString(),
                                  PageType: WdPrintOutPages.wdPrintAllPages,
                                  PrintToFile: false,
                                  Collate: true,
                                  ManualDuplexPrint: false);
                DateTimeOffset endTime = DateTimeOffset.Now;

                OnStatusChanged("Closing document...");
                document.Close(WdSaveOptions.wdDoNotSaveChanges, WdOriginalFormat.wdOriginalDocumentFormat, false);

                OnStatusChanged("Quitting Word.");
                word.Quit(WdSaveOptions.wdDoNotSaveChanges, WdOriginalFormat.wdOriginalDocumentFormat, false);

                return new FilePrintResult(startTime, endTime);
            }
            finally
            {
                if (document != null)
                {
                    Marshal.FinalReleaseComObject(document);
                    document = null;
                }

                if (documents != null)
                {
                    Marshal.FinalReleaseComObject(documents);
                    documents = null;
                }

                if (word != null)
                {
                    Marshal.FinalReleaseComObject(word);
                    word = null;
                }

                GarbageCollect();
            }
        }
    }
}
