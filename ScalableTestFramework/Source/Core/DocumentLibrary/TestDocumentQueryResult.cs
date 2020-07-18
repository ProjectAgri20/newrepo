using System;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.DocumentLibrary
{
    /// <summary>
    /// Mapping class containing only the fields from <see cref="TestDocument" /> that are used to build a <see cref="Document" />.
    /// </summary>
    internal sealed class TestDocumentQueryResult
    {
        /// <summary>
        /// Maps to <see cref="TestDocument.TestDocumentId" />.
        /// </summary>
        public Guid TestDocumentId { get; set; }

        /// <summary>
        /// Maps to <see cref="TestDocument.FileName" />.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Maps to <see cref="TestDocumentExtension.Location" />.
        /// </summary>
        public string Location { get; set; }

        /// <summary>
        /// Maps to <see cref="TestDocument.FileSize" />.
        /// </summary>
        public long FileSize { get; set; }

        /// <summary>
        /// Maps to <see cref="TestDocument.Pages" />.
        /// </summary>
        public int Pages { get; set; }

        /// <summary>
        /// Maps to <see cref="TestDocument.ColorMode" />.
        /// </summary>
        public string ColorMode { get; set; }

        /// <summary>
        /// Maps to <see cref="TestDocument.Orientation" />.
        /// </summary>
        public string Orientation { get; set; }

        /// <summary>
        /// Builds a <see cref="Document" /> from this instance.
        /// </summary>
        /// <returns>A <see cref="Document" /> with the data from this instance.</returns>
        public Document ToDocument()
        {
            return new Document
            (
                TestDocumentId,
                FileName,
                Location,
                FileSize,
                Pages,
                EnumUtil.Parse<ColorMode>(ColorMode),
                EnumUtil.Parse<Orientation>(Orientation)
            );
        }
    }
}
