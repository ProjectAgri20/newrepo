using System;
using System.IO;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;

namespace PluginSimulator
{
    internal sealed class FileRepositoryMockInternal : IFileRepository, IFileRepositoryInternal
    {
        /// <summary>
        /// Gets or sets the network share where documents are stored.
        /// </summary>
        public string DocumentShare { get; set; }

        /// <summary>
        /// Downloads the file at the specified path and returns the path of the local copy.
        /// </summary>
        /// <remarks>
        /// If the file has already been downloaded, it will not be downloaded again.
        /// </remarks>
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
            return DownloadFile(path, forceDownload);
        }

        /// <summary>
        /// Downloads the specified <see cref="Document" /> from the document library server.
        /// </summary>
        /// <remarks>
        /// If the document has already been downloaded, it will not be downloaded again.
        /// </remarks>
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
            return DownloadFile(GetDocumentSharePath(document), forceDownload);
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

            return Path.Combine(DocumentShare, document.Group, document.FileName);
        }

        private FileInfo DownloadFile(string file, bool forceDownload)
        {
            string localFile = Path.Combine(Path.GetTempPath(), Path.GetFileName(file));
            if (!File.Exists(localFile) || forceDownload)
            {
                File.Copy(file, localFile, true);
            }
            return new FileInfo(localFile);
        }
    }
}
