using System;
using System.IO;
using System.Reflection;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Utility;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.DocumentLibrary
{
    /// <summary>
    /// Manages files downloaded from Document Library or other servers to a local file repository.
    /// </summary>
    public sealed class DocumentFileRepository : IFileRepository, IFileRepositoryInternal
    {
        private readonly string _documentLibraryPath;
        private readonly string _destinationPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentFileRepository" /> class.
        /// </summary>
        /// <param name="documentLibraryPath">The path to the document library repository.</param>
        public DocumentFileRepository(string documentLibraryPath)
        {
            _documentLibraryPath = documentLibraryPath;
            _destinationPath = Path.Combine(Path.GetDirectoryName(Assembly.GetEntryAssembly().Location), "FileRepository");
        }

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
            return FetchDocument(path, forceDownload);
        }

        /// <summary>
        /// Downloads the specified <see cref="Document" /> from the document library server.
        /// (If the file has already been downloaded, it will not be downloaded again.)
        /// </summary>
        /// <param name="document">The document to retrieve.</param>
        /// <returns>A <see cref="FileInfo" /> object with information about the downloaded file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is null.</exception>
        public FileInfo GetFile(Document document)
        {
            return GetFile(document, false);
        }

        /// <summary>
        /// Downloads the specified <see cref="Document" /> from the document library server.
        /// </summary>
        /// <param name="document">The document to retrieve.</param>
        /// <param name="forceDownload">if set to <c>true</c> download the document even if a local copy already exists.</param>
        /// <returns>A <see cref="FileInfo" /> object with information about the downloaded file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is null.</exception>
        public FileInfo GetFile(Document document, bool forceDownload)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            string documentPath = GetDocumentSharePath(document);
            return FetchDocument(documentPath, forceDownload);
        }

        /// <summary>
        /// Gets the public share path for the specified <see cref="Document" />.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The public share path for the document.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is null.</exception>
        public string GetDocumentSharePath(Document document)
        {
            if (document == null)
            {
                throw new ArgumentNullException(nameof(document));
            }

            return Path.Combine(_documentLibraryPath, document.Group, document.FileName);
        }

        private FileInfo FetchDocument(string sourceFile, bool forceDownload)
        {
            string destinationFile = Path.Combine(_destinationPath, Path.GetFileName(sourceFile));

            if (!File.Exists(destinationFile) || forceDownload)
            {
                LogDebug($"Downloading file: {sourceFile}");
                bool success = Retry.UntilTrue(() => DownloadFile(sourceFile, destinationFile), 2, TimeSpan.FromSeconds(5));
                if (!success)
                {
                    LogError("Unable to download file.");
                }
            }

            return new FileInfo(destinationFile);
        }

        private bool DownloadFile(string sourceFile, string destinationFile)
        {
            try
            {
                Directory.CreateDirectory(_destinationPath);
                File.Copy(sourceFile, destinationFile, true);
                File.SetAttributes(destinationFile, FileAttributes.Normal);
                return true;
            }
            catch (Exception ex) when (ex is IOException || ex is UnauthorizedAccessException)
            {
                LogWarn("File download failed.", ex);

                // Check to see if the file was downloaded by somebody else simultaneously
                return File.Exists(destinationFile);
            }
        }
    }
}
