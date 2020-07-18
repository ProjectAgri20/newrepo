using System;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Utility;
using Microsoft.Office.Interop.Excel;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.FileAnalysis.Analyzers
{
    /// <summary>
    /// Implementation of <see cref="FileAnalyzer" /> for Microsoft Excel files.
    /// </summary>
    [ObjectFactory("XLS")]
    [ObjectFactory("XLSX")]
    internal sealed class ExcelFileAnalyzer : FileAnalyzer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ExcelFileAnalyzer" /> class.
        /// </summary>
        /// <param name="file">The file to be analyzed by this <see cref="FileAnalyzer" />.</param>
        public ExcelFileAnalyzer(FileInfo file)
            : base(file)
        {
        }

        /// <summary>
        /// Performs basic validation on the file being analyzed by this <see cref="FileAnalyzer" />.
        /// </summary>
        /// <returns>A <see cref="FileValidationResult" /> object representing the result of validation.</returns>
        public override FileValidationResult Validate()
        {
            try
            {
                using (ExcelApp app = new ExcelApp())
                {
                    app.Open(File);
                }
                return FileValidationResult.Pass;
            }
            catch (COMException ex)
            {
                LogWarn("File failed to open: " + ex.Message);
                return FileValidationResult.Fail(ex.Message);
            }
        }

        /// <summary>
        /// Gets information about a document, such as page count, title, and author.
        /// </summary>
        /// <returns>A <see cref="DocumentProperties" /> object.</returns>
        public override DocumentProperties GetProperties()
        {
            DocumentProperties properties = new DocumentProperties(File);

            try
            {
                using (ExcelApp app = new ExcelApp())
                {
                    Workbook workbook = app.Open(File);

                    // Calculate page count by summing the number of pages for each worksheet
                    properties.Pages = (short)workbook.Sheets.OfType<Worksheet>().Sum(n => n.PageSetup.Pages.Count);

                    // Get the workbook orientation
                    switch (((Worksheet)workbook.ActiveSheet).PageSetup.Orientation)
                    {
                        case XlPageOrientation.xlPortrait:
                            properties.Orientation = Orientation.Portrait;
                            break;
                        case XlPageOrientation.xlLandscape:
                            properties.Orientation = Orientation.Landscape;
                            break;
                    }

                    // Retrieve built-in workbook properties
                    properties.Title = workbook.BuiltinDocumentProperties.Item["Title"].Value;
                    properties.Author = workbook.BuiltinDocumentProperties.Item["Author"].Value;
                    properties.Application = workbook.BuiltinDocumentProperties.Item["Application Name"].Value;
                }
            }
            catch (COMException ex)
            {
                // Log the error, then return whatever properties have been collected.
                LogWarn("Failure retrieving properties: " + ex.Message);
            }

            return properties;
        }

        private sealed class ExcelApp : IDisposable
        {
            private Application _excelApp;
            private Workbook _excelBook;

            public ExcelApp()
            {
                LogDebug("Starting Excel.");
                _excelApp = new Application();
                _excelApp.DisplayAlerts = false;
            }

            public Workbook Open(FileInfo file)
            {
                LogDebug($"Opening file: {file.Name}");
                _excelBook = _excelApp.Workbooks.Open(file.FullName,
                                                      UpdateLinks: 0,
                                                      ReadOnly: true,
                                                      IgnoreReadOnlyRecommended: true,
                                                      CorruptLoad: XlCorruptLoad.xlNormalLoad);
                LogDebug("File opened successfully.");
                return _excelBook;
            }

            public void Dispose()
            {
                LogDebug("Closing Excel.");
                _excelBook?.Close(false);
                _excelApp?.Application.Quit();
            }
        }
    }
}
