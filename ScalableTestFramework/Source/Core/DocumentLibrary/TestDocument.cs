using System;
using System.Diagnostics;

namespace HP.ScalableTest.Core.DocumentLibrary
{
    /// <summary>
    /// Information about a document available in the test document library.
    /// </summary>
    [DebuggerDisplay("{FileName,nq}")]
    public class TestDocument
    {
        /// <summary>
        /// Gets or sets the unique identifier for the document.
        /// </summary>
        public Guid TestDocumentId { get; set; }

        /// <summary>
        /// Gets or sets the extension for the document.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the file name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets the type of content contained in this file.
        /// </summary>
        public string FileType { get; set; }

        /// <summary>
        /// Gets or sets the file size, in kilobytes.
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Gets or sets the number of pages in the document.
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Gets or sets the document color mode.
        /// </summary>
        public string ColorMode { get; set; }

        /// <summary>
        /// Gets or sets the document orientation.
        /// </summary>
        public string Orientation { get; set; }

        /// <summary>
        /// Gets or sets the author of the document.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the vertical attribute for the document.
        /// </summary>
        public string Vertical { get; set; }

        /// <summary>
        /// Gets or sets the notes associated with this document.
        /// </summary>
        public string Notes { get; set; }

        /// <summary>
        /// Gets or sets the user that submitted this document.
        /// </summary>
        public string Submitter { get; set; }

        /// <summary>
        /// Gets or sets the date when this document was submitted.
        /// </summary>
        public DateTime SubmitDate { get; set; }

        /// <summary>
        /// Gets or sets the type of the author.
        /// </summary>
        public string AuthorType { get; set; }

        /// <summary>
        /// Gets or sets the application associated with the document.
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets the application version associated with the document.
        /// </summary>
        public string AppVersion { get; set; }

        /// <summary>
        /// Gets or sets the defect ID associated with the document.
        /// </summary>
        public string DefectId { get; set; }

        /// <summary>
        /// Gets or sets the tag associated with the document.
        /// </summary>
        public string Tag { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DocumentLibrary.TestDocumentExtension" /> for the document.
        /// </summary>
        public virtual TestDocumentExtension TestDocumentExtension { get; set; }
    }
}
