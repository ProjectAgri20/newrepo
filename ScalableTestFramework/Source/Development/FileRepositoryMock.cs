using System;
using System.IO;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// A mock implementation of <see cref="IFileRepository" /> for development.
    /// </summary>
    public sealed class FileRepositoryMock : IFileRepository
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FileRepositoryMock" /> class.
        /// </summary>
        public FileRepositoryMock()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Gets or sets the local file path to look for documents.
        /// </summary>
        public string LocalFilePath { get; set; }

        /// <summary>
        /// Downloads the file at the specified path and returns the path of the local copy.
        /// (If the file has already been downloaded, it will not be downloaded again.)
        /// </summary>
        /// <param name="path">The path of the file to download.</param>
        /// <returns>A <see cref="FileInfo" /> object with information about the downloaded file.</returns>
        public FileInfo GetFile(string path)
        {
            return GetFile(path, false);
        }

        /// <summary>
        /// Downloads the file at the specified path and returns the path of the local copy.
        /// </summary>
        /// <param name="path">The path of the file to download.</param>
        /// <param name="forceDownload">if set to <c>true</c> download the document even if a local copy already exists.</param>
        /// <returns>A <see cref="FileInfo" /> object with information about the downloaded file.</returns>
        public FileInfo GetFile(string path, bool forceDownload)
        {
            string fileName = Path.GetFileName(path);

            if (forceDownload || !File.Exists(fileName))
            {
                File.Copy(path, fileName, true);
            }
            return new FileInfo(fileName);
        }

        /// <summary>
        /// Mock implementation that returns <see cref="FileInfo" /> for a document in the local execution directory.
        /// </summary>
        /// <param name="document">The document, which must exist in the local execution directory or the path specified by <see cref="LocalFilePath" />.</param>
        /// <returns>A <see cref="FileInfo" /> object with information about the local file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is null.</exception>
        public FileInfo GetFile(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            string localRepositoryPath = Path.Combine(LocalFilePath, document.FileName);

            if (File.Exists(localRepositoryPath))
            {
                return new FileInfo(localRepositoryPath);
            }
            else if (File.Exists(document.FileName))
            {
                return new FileInfo(document.FileName);
            }
            else
            {
                throw new FileNotFoundException($"The document '{document.FileName}' was not found in the execution directory.");
            }
        }

        /// <summary>
        /// Mock implementation that returns <see cref="FileInfo" /> for a document in the local execution directory.
        /// </summary>
        /// <param name="document">The document, which must already exist in the local execution directory.</param>
        /// <param name="forceDownload">Ignored.</param>
        /// <returns>A <see cref="FileInfo" /> object with information about the local file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is null.</exception>
        public FileInfo GetFile(Document document, bool forceDownload)
        {
            return GetFile(document);
        }
    }
}
