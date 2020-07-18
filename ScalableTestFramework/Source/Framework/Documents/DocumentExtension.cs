using System.Diagnostics;

namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// Information about a file extension from the document library.
    /// </summary>
    [DebuggerDisplay("{Extension,nq}")]
    public sealed class DocumentExtension
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _extension;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _fileType;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _contentType;

        /// <summary>
        /// Gets the file extension.
        /// </summary>
        public string Extension => _extension;

        /// <summary>
        /// Gets a textual description of the file type.
        /// </summary>
        public string FileType => _fileType;

        /// <summary>
        /// Gets the MIME content type.
        /// </summary>
        public string ContentType => _contentType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentExtension" /> class.
        /// </summary>
        /// <param name="extension">The extension.</param>
        /// <param name="fileType">A textual description of the file type.</param>
        /// <param name="contentType">The MIME content type.</param>
        public DocumentExtension(string extension, string fileType, string contentType)
        {
            _extension = extension;
            _fileType = fileType;
            _contentType = contentType;
        }
    }
}
