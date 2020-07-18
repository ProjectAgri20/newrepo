using System;
using System.IO;
using System.Runtime.InteropServices;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Utility;
using Microsoft.Office.Core;
using Microsoft.Office.Interop.PowerPoint;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.FileAnalysis.Analyzers
{
    /// <summary>
    /// Implementation of <see cref="FileAnalyzer" /> for Microsoft PowerPoint files.
    /// </summary>
    [ObjectFactory("PPT")]
    [ObjectFactory("PPTX")]
    internal sealed class PowerPointFileAnalyzer : FileAnalyzer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PowerPointFileAnalyzer" /> class.
        /// </summary>
        /// <param name="file">The file to be analyzed by this <see cref="FileAnalyzer" />.</param>
        public PowerPointFileAnalyzer(FileInfo file)
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
                using (PowerPointApp app = new PowerPointApp())
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
                using (PowerPointApp app = new PowerPointApp())
                {
                    Presentation presentation = app.Open(File);
                    properties.Pages = (short)presentation.Slides.Count;

                    // Get the page orientation
                    switch (presentation.PageSetup.SlideOrientation)
                    {
                        case MsoOrientation.msoOrientationVertical:
                            properties.Orientation = Orientation.Portrait;
                            break;
                        case MsoOrientation.msoOrientationHorizontal:
                            properties.Orientation = Orientation.Landscape;
                            break;
                        case MsoOrientation.msoOrientationMixed:
                            properties.Orientation = Orientation.Mixed;
                            break;
                    }

                    // Retrieve built-in document properties
                    properties.Title = presentation.BuiltInDocumentProperties.Item["Title"].Value;
                    properties.Author = presentation.BuiltInDocumentProperties.Item["Author"].Value;
                    properties.Application = presentation.BuiltInDocumentProperties.Item["Application Name"].Value;
                }
            }
            catch (COMException ex)
            {
                // Log the error, then return whatever properties have been collected.
                LogWarn("Failure retrieving properties: " + ex.Message);
            }

            return properties;
        }

        private sealed class PowerPointApp : IDisposable
        {
            private Application _powerPointApp;
            private Presentation _presentation;

            public PowerPointApp()
            {
                LogDebug("Starting PowerPoint.");
                _powerPointApp = new Application();
                _powerPointApp.DisplayAlerts = PpAlertLevel.ppAlertsNone;
            }

            public Presentation Open(FileInfo file)
            {
                LogDebug($"Opening file: {file.Name}");
                _presentation = _powerPointApp.Presentations.Open(file.FullName,
                                                                  ReadOnly: MsoTriState.msoTrue,
                                                                  Untitled: MsoTriState.msoFalse,
                                                                  WithWindow: MsoTriState.msoFalse);
                LogDebug("File opened successfully.");
                return _presentation;
            }

            public void Dispose()
            {
                LogDebug("Closing PowerPoint.");
                _presentation?.Close();
                _powerPointApp?.Quit();
            }
        }
    }
}
