using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.DocumentLibrary
{
    /// <summary>
    /// A named set of test documents in the test document library.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class TestDocumentSet
    {
        /// <summary>
        /// Gets or sets the unique identifier for the document set.
        /// </summary>
        internal int TestDocumentSetId { get; set; }

        /// <summary>
        /// Gets or sets the name of the set.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of documents in the set.
        /// </summary>
        public string SetType { get; set; }

        /// <summary>
        /// Gets or sets the collection of <see cref="TestDocumentSetItem" />.
        /// </summary>
        public virtual ICollection<TestDocumentSetItem> TestDocumentSetItems { get; set; } = new HashSet<TestDocumentSetItem>();
    }
}
