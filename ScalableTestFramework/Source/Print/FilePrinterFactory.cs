using System;
using System.IO;
using HP.ScalableTest.Print.FilePrinters;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Print
{
    /// <summary>
    /// Factory class for creating <see cref="FilePrinter" /> objects.
    /// </summary>
    public static class FilePrinterFactory
    {
        /// <summary>
        /// Creates a <see cref="FilePrinter" /> for the specified file.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>A <see cref="FilePrinter" /> for the specified file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="fileName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName" /> is empty, contains only white spaces, or contains invalid characters.</exception>
        public static FilePrinter Create(string fileName)
        {
            return Create(new FileInfo(fileName));
        }

        /// <summary>
        /// Creates a <see cref="FilePrinter" /> for the file represented by the specified <see cref="FileInfo" />.
        /// </summary>
        /// <param name="file">The <see cref="FileInfo" />.</param>
        /// <returns>A <see cref="FilePrinter" /> for the specified file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="file" /> is null.</exception>
        public static FilePrinter Create(FileInfo file)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }

            LogDebug($"Creating file printer for {file.FullName}");
            string fileExtension = file.Extension.ToUpperInvariant().Trim('.');

            try
            {
                FilePrinter filePrinter = ObjectFactory.Create<FilePrinter>(fileExtension, file);
                LogTrace($"Created {filePrinter.GetType().Name}.");
                return filePrinter;
            }
            catch (InvalidOperationException)
            {
                LogTrace($"No match for file type {fileExtension}. Created generic file printer.");
                return new GenericFilePrinter(file);
            }
        }
    }
}
