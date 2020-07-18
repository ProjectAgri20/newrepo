using System;
using System.IO;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Manages local copies of files downloaded from a central repository.
    /// </summary>
    public interface IFileRepository
    {
        /// <summary>
        /// Downloads the file at the specified path and returns the path of the local copy.
        /// (If the file has already been downloaded, it will not be downloaded again.)
        /// </summary>
        /// <param name="path">The path of the file to download.</param>
        /// <returns>A <see cref="FileInfo" /> object with information about the downloaded file.</returns>
        FileInfo GetFile(string path);

        /// <summary>
        /// Downloads the file at the specified path and returns the path of the local copy.
        /// </summary>
        /// <param name="path">The path of the file to download.</param>
        /// <param name="forceDownload">if set to <c>true</c> download the document even if a local copy already exists.</param>
        /// <returns>A <see cref="FileInfo" /> object with information about the downloaded file.</returns>
        FileInfo GetFile(string path, bool forceDownload);

        /// <summary>
        /// Downloads the specified <see cref="Document" /> from the document library server.
        /// (If the file has already been downloaded, it will not be downloaded again.)
        /// </summary>
        /// <param name="document">The document to retrieve.</param>
        /// <returns>A <see cref="FileInfo" /> object with information about the downloaded file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is null.</exception>
        FileInfo GetFile(Document document);

        /// <summary>
        /// Downloads the specified <see cref="Document" /> from the document library server.
        /// </summary>
        /// <param name="document">The document to retrieve.</param>
        /// <param name="forceDownload">if set to <c>true</c> download the document even if a local copy already exists.</param>
        /// <returns>A <see cref="FileInfo" /> object with information about the downloaded file.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is null.</exception>
        FileInfo GetFile(Document document, bool forceDownload);
    }
}
