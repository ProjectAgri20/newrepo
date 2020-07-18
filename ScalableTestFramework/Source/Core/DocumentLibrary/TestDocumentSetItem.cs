using System;

namespace HP.ScalableTest.Core.DocumentLibrary
{
    /// <summary>
    /// An association between a <see cref="TestDocumentSet" /> and a <see cref="DocumentLibrary.TestDocument" />.
    /// </summary>
    public class TestDocumentSetItem
    {
        /// <summary>
        /// Gets or sets the unique identifier for the document set.
        /// </summary>
        internal int TestDocumentSetId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the document.
        /// </summary>
        public Guid TestDocumentId { get; set; }

        /// <summary>
        /// Gets or sets the order of the document within the document set.
        /// </summary>
        public int SortOrder { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DocumentLibrary.TestDocument" />.
        /// </summary>
        public virtual TestDocument TestDocument { get; set; }
    }
}
