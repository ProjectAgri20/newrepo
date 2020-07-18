using System;
using System.IO;
using System.Runtime.InteropServices;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Utility;
using Microsoft.Office.Interop.Word;
using static HP.ScalableTest.Framework.Logger;
using WordDocument = Microsoft.Office.Interop.Word.Document;

namespace HP.ScalableTest.FileAnalysis.Analyzers
{
    /// <summary>
    /// Implementation of <see cref="FileAnalyzer" /> for Microsoft Word files.
    /// </summary>
    [ObjectFactory("DOC")]
    [ObjectFactory("DOCX")]
    internal sealed class WordFileAnalyzer : FileAnalyzer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WordFileAnalyzer"/> class.
        /// </summary>
        /// <param name="file">The file to be analyzed by this <see cref="FileAnalyzer" />.</param>
        public WordFileAnalyzer(FileInfo file)
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
                using (WordApp app = new WordApp())
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
                using (WordApp app = new WordApp())
                {
                    WordDocument document = app.Open(File);

                    // Page count must be calculated first - strange results otherwise
                    properties.Pages = (short)document.ComputeStatistics(WdStatistic.wdStatisticPages);

                    // Get the page orientation
                    switch (document.PageSetup.Orientation)
                    {
                        case WdOrientation.wdOrientPortrait:
                            properties.Orientation = Orientation.Portrait;
                            break;
                        case WdOrientation.wdOrientLandscape:
                            properties.Orientation = Orientation.Landscape;
                            break;
                    }

                    // Retrieve built-in document properties
                    properties.Title = document.BuiltInDocumentProperties.Item["Title"].Value;
                    properties.Author = document.BuiltInDocumentProperties.Item["Author"].Value;
                    properties.Application = document.BuiltInDocumentProperties.Item["Application Name"].Value;
                }
            }
            catch (COMException ex)
            {
                // Log the error, then return whatever properties have been collected.
                LogWarn("Failure retrieving properties: " + ex.Message);
            }

            return properties;
        }

        private sealed class WordApp : IDisposable
        {
            private Application _wordApp;
            private WordDocument _wordDoc;

            public WordApp()
            {
                LogDebug("Starting Word.");
                _wordApp = new Application();
                _wordApp.DisplayAlerts = WdAlertLevel.wdAlertsNone;
            }

            public WordDocument Open(FileInfo file)
            {
                LogDebug($"Opening file: {file.Name}");
                _wordDoc = _wordApp.Documents.Open(file.FullName,
                                                   ReadOnly: true,
                                                   Visible: false);
                LogDebug("File opened successfully.");
                return _wordDoc;
            }

            public void Dispose()
            {
                LogDebug("Closing Word.");
                _wordDoc?.Close(WdSaveOptions.wdDoNotSaveChanges);
                _wordApp?.Quit(WdSaveOptions.wdDoNotSaveChanges);
            }
        }
    }
}
