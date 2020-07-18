using System;
using System.IO;
using HP.ScalableTest.FileAnalysis.Analyzers;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.FileAnalysis
{
    /// <summary>
    /// Factory class for creating <see cref="FileAnalyzer" /> objects.
    /// </summary>
    public static class FileAnalyzerFactory
    {
        /// <summary>
        /// Creates a <see cref="FileAnalyzer" /> for the specified file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>A <see cref="FileAnalyzer" /> for the specified file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName" /> is empty, contains only white spaces, or contains invalid characters.</exception>
        public static FileAnalyzer Create(string fileName)
        {
            return Create(new FileInfo(fileName));
        }

        /// <summary>
        /// Creates a <see cref="FileAnalyzer" /> for the file represented by the specified <see cref="FileInfo" />.
        /// </summary>
        /// <param name="file">The <see cref="FileInfo" />.</param>
        /// <returns>A <see cref="FileAnalyzer" /> for the specified file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="file" /> is null.</exception>
        public static FileAnalyzer Create(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            LogDebug($"Creating file analyzer for {file.FullName}");
            string fileExtension = file.Extension.ToUpperInvariant().Trim('.');

            try
            {
                FileAnalyzer fileAnalyzer = ObjectFactory.Create<FileAnalyzer>(fileExtension, file);
                LogTrace($"Created {fileAnalyzer.GetType().Name}.");
                return fileAnalyzer;
            }
            catch (InvalidOperationException)
            {
                LogTrace($"No match for file type {fileExtension}. Created generic file analyzer.");
                return new GenericFileAnalyzer(file);
            }
        }
    }
}
