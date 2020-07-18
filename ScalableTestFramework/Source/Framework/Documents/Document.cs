using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// Represents a document used in a test.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{FileName,nq}")]
    public sealed class Document
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Guid _documentId;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _fileName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _group;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly long _fileSize;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly int _pages;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ColorMode _colorMode;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Orientation _orientation;

        /// <summary>
        /// Gets the unique identifier for the document.
        /// </summary>
        public Guid DocumentId => _documentId;

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName => _fileName;

        /// <summary>
        /// Gets the group associated with this file.
        /// </summary>
        public string Group => _group;

        /// <summary>
        /// Gets the file size, in kilobytes.
        /// </summary>
        public long FileSize => _fileSize;

        /// <summary>
        /// Gets the number of pages in the document.
        /// </summary>
        public int Pages => _pages;

        /// <summary>
        /// Gets the document color mode.
        /// </summary>
        public ColorMode ColorMode => _colorMode;

        /// <summary>
        /// Gets the document orientation.
        /// </summary>
        public Orientation Orientation => _orientation;

        /// <summary>
        /// Initializes a new instance of the <see cref="Document" /> class.
        /// </summary>
        /// <param name="documentId">The unique identifier for the document.</param>
        /// <param name="fileName">The file name.</param>
        /// <param name="group">The group associated with this file.</param>
        /// <param name="fileSize">The file size, in kilobytes.</param>
        /// <param name="pages">The number of pages in the document.</param>
        /// <param name="colorMode">The document color mode.</param>
        /// <param name="orientation">The document orientation.</param>
        /// <exception cref="ArgumentNullException"><paramref name="fileName" /> is null.</exception>
        /// <exception cref="ArgumentException"><paramref name="fileName" /> is an empty string.</exception>
        public Document(Guid documentId, string fileName, string group, long fileSize, int pages, ColorMode colorMode, Orientation orientation)
        {
            if (fileName == null)
            {
                throw new ArgumentNullException(nameof(fileName));
            }

            if (string.IsNullOrWhiteSpace(fileName))
            {
                throw new ArgumentException("File name cannot be an empty string.", nameof(fileName));
            }

            _documentId = documentId;
            _fileName = fileName;
            _group = group;
            _fileSize = fileSize;
            _pages = pages;
            _colorMode = colorMode;
            _orientation = orientation;
        }
    }
}
