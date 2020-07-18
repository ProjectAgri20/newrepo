using System;
using System.Diagnostics;

namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// A named set of documents used in a test.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public sealed class DocumentSet
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _name;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _group;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DocumentCollection _documents;

        /// <summary>
        /// Gets the name of this document set.
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the group associated with this document set.
        /// </summary>
        public string Group => _group;

        /// <summary>
        /// Gets the documents in the set.
        /// </summary>
        public DocumentCollection Documents => _documents;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentSet" /> class.
        /// </summary>
        /// <param name="name">The name of the document set.</param>
        /// <param name="group">The group associated with this document set.</param>
        /// <param name="documents">The documents in the set.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documents" /> is null.</exception>
        public DocumentSet(string name, string group, DocumentCollection documents)
        {
            _name = name;
            _group = group;
            _documents = documents ?? throw new ArgumentNullException(nameof(documents));
        }
    }
}
