using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.DataLog.Model
{
    /// <summary>
    /// A document used by a test session.
    /// </summary>
    [DebuggerDisplay("{FileName,nq}")]
    public sealed class SessionDocument
    {
        /// <summary>
        /// Gets or sets the unique identifier for this session document record.
        /// </summary>
        public Guid SessionDocumentId { get; set; }

        /// <summary>
        /// Gets or sets the identifier for the session that used the document.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the document that was used.
        /// </summary>
        public Guid DocumentId { get; set; }

        /// <summary>
        /// Gets or sets the document file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the document extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the document file type.
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// Gets or sets the document file size, in kilobytes.
        /// </summary>
        public long? FileSizeKilobytes { get; set; }

        /// <summary>
        /// Gets or sets the number of pages in the document.
        /// </summary>
        public short? Pages { get; set; }

        /// <summary>
        /// Gets or sets the color mode of the document.
        /// </summary>
        public string ColorMode { get; set; }

        /// <summary>
        /// Gets or sets the orientation of the document.
        /// </summary>
        public string Orientation { get; set; }

        /// <summary>
        /// Gets or sets the defect ID associated with the document.
        /// </summary>
        public string DefectId { get; set; }

        /// <summary>
        /// Gets or sets the tag for the document.
        /// </summary>
        public string Tag { get; set; }
    }
}
