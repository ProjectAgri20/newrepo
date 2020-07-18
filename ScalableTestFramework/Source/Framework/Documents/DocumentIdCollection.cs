using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// A collection of document IDs.
    /// </summary>
    [DataContract]
    public sealed class DocumentIdCollection : Collection<Guid>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentIdCollection" /> class.
        /// </summary>
        public DocumentIdCollection()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentIdCollection" /> class.
        /// </summary>
        /// <param name="documentIds">The document IDs to include in the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documentIds" /> is null.</exception>
        public DocumentIdCollection(IEnumerable<Guid> documentIds)
        {
            if (documentIds == null)
            {
                throw new ArgumentNullException(nameof(documentIds));
            }

            foreach (Guid id in documentIds)
            {
                Add(id);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentIdCollection" /> class.
        /// </summary>
        /// <param name="documents">The <see cref="Document" /> objects whose IDs will be included in the collection.</param>
        /// <exception cref="ArgumentNullException"><paramref name="documents" /> is null.</exception>
        public DocumentIdCollection(IEnumerable<Document> documents)
        {
            if (documents == null)
            {
                throw new ArgumentNullException(nameof(documents));
            }

            foreach (Document document in documents.Where(n => n != null))
            {
                Add(document.DocumentId);
            }
        }
    }
}
