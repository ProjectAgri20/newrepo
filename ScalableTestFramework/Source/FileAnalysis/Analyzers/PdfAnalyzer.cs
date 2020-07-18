using System;
using System.IO;
using System.Linq;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Utility;
using PdfSharp;
using PdfSharp.Pdf;
using PdfSharp.Pdf.IO;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.FileAnalysis.Analyzers
{
    /// <summary>
    /// Implementation of <see cref="FileAnalyzer" /> for Adobe PDF files.
    /// </summary>
    [ObjectFactory("PDF")]
    internal sealed class PdfAnalyzer : FileAnalyzer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PdfAnalyzer"/> class.
        /// </summary>
        /// <param name="file">The file to be analyzed by this <see cref="FileAnalyzer" />.</param>
        public PdfAnalyzer(FileInfo file)
            : base(file)
        {
        }

        /// <summary>
        /// Performs basic validation on the file being analyzed by this <see cref="FileAnalyzer" />.
        /// </summary>
        /// <returns>A <see cref="FileValidationResult" /> object representing the result of validation.</returns>
        public override FileValidationResult Validate()
        {
            string firstLine = System.IO.File.ReadLines(File.FullName).FirstOrDefault();

            // If the first line is empty, the file is invalid.
            if (string.IsNullOrEmpty(firstLine))
            {
                return FileValidationResult.Fail("Empty file.");
            }

            // A well-formed PDF must start with this text.
            if (!firstLine.StartsWith("%PDF-", StringComparison.OrdinalIgnoreCase))
            {
                return FileValidationResult.Fail("Malformed first line.");
            }

            // There should be an EOF flag at the end.
            string lastLine = System.IO.File.ReadLines(File.FullName).Last();
            if (lastLine != "%%EOF")
            {
                return FileValidationResult.Fail("File did not include EOF flag.");
            }

            // Finally, use the PDF library to load the file and ensure there were no other problems.
            try
            {
                LogDebug($"Loading PDF file: {File.Name}");
                using (PdfDocument doc = PdfReader.Open(File.FullName, PdfDocumentOpenMode.ReadOnly))
                {
                    return FileValidationResult.Pass;
                }
            }
            catch (Exception ex) when (ex is PdfSharpException || ex is InvalidOperationException)
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
                LogDebug($"Loading PDF file: {File.Name}");
                using (PdfDocument document = PdfReader.Open(File.FullName, PdfDocumentOpenMode.ReadOnly))
                {
                    properties.Pages = (short)document.PageCount;
                    properties.Title = document.Info.Title;
                    properties.Author = document.Info.Author;
                    properties.Application = document.Info.Producer;

                    if (document.PageCount > 0)
                    {
                        switch (document.Pages[0].Orientation)
                        {
                            case PageOrientation.Portrait:
                                properties.Orientation = Orientation.Portrait;
                                break;

                            case PageOrientation.Landscape:
                                properties.Orientation = Orientation.Landscape;
                                break;
                        }
                    }
                }
            }
            catch (Exception ex) when (ex is PdfSharpException || ex is InvalidOperationException)
            {
                // Log the error, then return whatever properties have been collected.
                LogWarn("Failure retrieving properties: " + ex.Message);
            }

            return properties;
        }
    }
}
