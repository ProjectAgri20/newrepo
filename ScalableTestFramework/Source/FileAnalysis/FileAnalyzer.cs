using System.IO;

namespace HP.ScalableTest.FileAnalysis
{
    /// <summary>
    /// Defines methods for analyzing a file.
    /// </summary>
    public abstract class FileAnalyzer
    {
        /// <summary>
        /// Gets a <see cref="FileInfo" /> object representing the file being analyzed by this <see cref="FileAnalyzer" />.
        /// </summary>
        public FileInfo File { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileAnalyzer"/> class.
        /// </summary>
        /// <param name="file">The file to be analyzed by this <see cref="FileAnalyzer" />.</param>
        protected FileAnalyzer(FileInfo file)
        {
            File = file;
        }

        /// <summary>
        /// Performs basic validation on the file being analyzed by this <see cref="FileAnalyzer" />.
        /// </summary>
        /// <returns>A <see cref="FileValidationResult" /> object representing the result of validation.</returns>
        public abstract FileValidationResult Validate();

        /// <summary>
        /// Gets information about a document, such as page count, title, and author.
        /// </summary>
        /// <returns>A <see cref="DocumentProperties" /> object.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method creates a new object each time it is called.")]
        public abstract DocumentProperties GetProperties();
    }
}
