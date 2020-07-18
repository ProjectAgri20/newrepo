using System.IO;

namespace HP.ScalableTest.FileAnalysis.Analyzers
{
    /// <summary>
    /// Implementation of <see cref="FileAnalyzer" /> for a generic file.
    /// </summary>
    internal sealed class GenericFileAnalyzer : FileAnalyzer
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GenericFileAnalyzer" /> class.
        /// </summary>
        /// <param name="file">The file to be analyzed by this <see cref="FileAnalyzer" />.</param>
        public GenericFileAnalyzer(FileInfo file)
            : base(file)
        {
        }

        /// <summary>
        /// Performs basic validation on the file being analyzed by this <see cref="FileAnalyzer" />.
        /// </summary>
        /// <returns>A <see cref="FileValidationResult" /> object representing the result of validation.</returns>
        public override FileValidationResult Validate()
        {
            return FileValidationResult.Pass;
        }

        /// <summary>
        /// Gets information about a document, such as page count, title, and author.
        /// </summary>
        /// <returns>A <see cref="DocumentProperties" /> object.</returns>
        public override DocumentProperties GetProperties()
        {
            return new DocumentProperties(File);
        }
    }
}
