using System.Diagnostics;

namespace HP.ScalableTest.Core.DocumentLibrary
{
    /// <summary>
    /// Information about a file extension for documents in the test document library.
    /// </summary>
    [DebuggerDisplay("{Extension,nq}")]
    public sealed class TestDocumentExtension
    {
        /// <summary>
        /// Gets or sets the file extension.
        /// </summary>
        public string Extension { get; set; }

        /// <summary>
        /// Gets or sets the location or group for the files with this extension.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the MIME content type.
        /// </summary>
        public string ContentType { get; set; }
    }
}
