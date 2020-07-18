using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// A simple implementation of <see cref="IDocumentLibrary" /> for development.
    /// </summary>
    public sealed class DocumentLibraryMock : IDocumentLibrary
    {
        private readonly List<Document> _documents = new List<Document>();
        private readonly List<DocumentSet> _documentSets = new List<DocumentSet>();
        private readonly List<DocumentExtension> _extensions = new List<DocumentExtension>();

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentLibraryMock" /> class.
        /// </summary>
        public DocumentLibraryMock()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Adds the specified document to the library.
        /// </summary>
        /// <param name="document">The document to add to the library.</param>
        public void AddDocument(Document document) => _documents.Add(document);

        /// <summary>
        /// Adds the specified documents to the library.
        /// </summary>
        /// <param name="documents">The documents to add to the library.</param>
        public void AddDocuments(IEnumerable<Document> documents) => _documents.AddRange(documents);

        /// <summary>
        /// Removes the specified document from the library.
        /// </summary>
        /// <param name="document">The document to remove from the library.</param>
        public void RemoveDocument(Document document) => _documents.Remove(document);

        /// <summary>
        /// Removes all documents from the library.
        /// </summary>
        public void ClearDocuments() => _documents.Clear();

        /// <summary>
        /// Adds the specified document set to the library.
        /// </summary>
        /// <param name="documentSet">The document set to add to the library.</param>
        public void AddDocumentSet(DocumentSet documentSet) => _documentSets.Add(documentSet);

        /// <summary>
        /// Adds the specified document sets to the library.
        /// </summary>
        /// <param name="documentSets">The document sets to add to the library.</param>
        public void AddDocumentSets(IEnumerable<DocumentSet> documentSets) => _documentSets.AddRange(documentSets);

        /// <summary>
        /// Removes the specified document set from the library.
        /// </summary>
        /// <param name="documentSet">The document set to remove from the library.</param>
        public void RemoveDocumentSet(DocumentSet documentSet) => _documentSets.Remove(documentSet);

        /// <summary>
        /// Removes all document sets from the library.
        /// </summary>
        public void ClearDocumentSets() => _documentSets.Clear();

        /// <summary>
        /// Adds the specified extension to the library.
        /// </summary>
        /// <param name="extension">The extension to add to the library.</param>
        public void AddExtension(DocumentExtension extension) => _extensions.Add(extension);

        /// <summary>
        /// Adds the specified extensions to the library.
        /// </summary>
        /// <param name="extensions">The extensions to add to the library.</param>
        public void AddExtensions(IEnumerable<DocumentExtension> extensions) => _extensions.AddRange(extensions);

        /// <summary>
        /// Removes the specified extension from the library.
        /// </summary>
        /// <param name="extension">The extension to remove from the library.</param>
        public void RemoveExtension(DocumentExtension extension) => _extensions.Remove(extension);

        /// <summary>
        /// Removes all extensions from the library.
        /// </summary>
        public void ClearExtensions() => _extensions.Clear();

        #region IDocumentLibrary Members

        /// <summary>
        /// Retrieves information for all documents in the document library.
        /// </summary>
        /// <returns>A <see cref="DocumentCollection" /> containing all documents in the library.</returns>
        public DocumentCollection GetDocuments()
        {
            // The ToList call ensures we send a copy of the list instead of the actual document list
            return new DocumentCollection(_documents.ToList());
        }

        /// <summary>
        /// Retrieves information for documents with one of the specified file extensions.
        /// </summary>
        /// <param name="extensions">The file extensions.</param>
        /// <returns>A <see cref="DocumentCollection" /> containing documents with one of the specified file extensions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extensions" /> is null.</exception>
        public DocumentCollection GetDocuments(IEnumerable<DocumentExtension> extensions)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            var documents = _documents.Where(n => Matches(n, extensions));
            return new DocumentCollection(documents.ToList());
        }

        /// <summary>
        /// Retrieves information for documents based on the provided criteria.
        /// </summary>
        /// <param name="query">The <see cref="DocumentQuery" /> defining the criteria to be used in selecting documents.</param>
        /// <returns>A <see cref="DocumentCollection" /> containing documents matching the specified criteria.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="query" /> is null.</exception>
        /// <remarks>
        /// This method actually returns all the documents without filtering.  This could be a future enhancement.
        /// </remarks>
        public DocumentCollection GetDocuments(DocumentQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            // No filtering in the mock
            return GetDocuments();
        }

        /// <summary>
        /// Retrieves information for all document sets in the document library.
        /// </summary>
        /// <returns>A list of all document sets in the document library.</returns>
        public IEnumerable<DocumentSet> GetDocumentSets()
        {
            return _documentSets;
        }

        /// <summary>
        /// Retrieves information for all document sets containing only documents with one of
        /// the specified file extensions.
        /// </summary>
        /// <param name="extensions">The file extensions.</param>
        /// <returns>A list of all document sets for the specified file extensions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extensions" /> is null.</exception>
        public IEnumerable<DocumentSet> GetDocumentSets(IEnumerable<DocumentExtension> extensions)
        {
            if (extensions == null)
            {
                throw new ArgumentNullException(nameof(extensions));
            }

            return _documentSets.Where(n => n.Documents.All(d => Matches(d, extensions)));
        }

        /// <summary>
        /// Retrieves information about the document extensions in the document library.
        /// </summary>
        /// <returns>A list of all document extensions in the document library.</returns>
        public IEnumerable<DocumentExtension> GetExtensions()
        {
            return _extensions;
        }

        /// <summary>
        /// Retrieves information about the document tags in the document library.
        /// </summary>
        /// <returns>A list of all document tags in the document library.</returns>
        public IEnumerable<string> GetTags()
        {
            yield return "No Tags";
        }

        private static bool Matches(Document document, IEnumerable<DocumentExtension> extensions)
        {
            string extension = Path.GetExtension(document.FileName).Trim('.');
            return extensions.Select(n => n.Extension).Contains(extension, StringComparer.OrdinalIgnoreCase);
        }

        #endregion
    }
}
