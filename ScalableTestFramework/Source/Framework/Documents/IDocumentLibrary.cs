using System;
using System.Collections.Generic;

namespace HP.ScalableTest.Framework.Documents
{
    /// <summary>
    /// Provides access to document information stored in the Document Library.
    /// </summary>
    public interface IDocumentLibrary
    {
        /// <summary>
        /// Retrieves information for all documents in the document library.
        /// </summary>
        /// <returns>A <see cref="DocumentCollection" /> containing all documents in the library.</returns>
        DocumentCollection GetDocuments();

        /// <summary>
        /// Retrieves information for documents with one of the specified file extensions.
        /// </summary>
        /// <param name="extensions">The file extensions.</param>
        /// <returns>A <see cref="DocumentCollection" /> containing documents with one of the specified file extensions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extensions" /> is null.</exception>
        DocumentCollection GetDocuments(IEnumerable<DocumentExtension> extensions);

        /// <summary>
        /// Retrieves information for documents based on the provided criteria.
        /// </summary>
        /// <param name="query">The <see cref="DocumentQuery" /> defining the criteria to be used in selecting documents.</param>
        /// <returns>A <see cref="DocumentCollection" /> containing documents matching the specified criteria.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="query" /> is null.</exception>
        DocumentCollection GetDocuments(DocumentQuery query);

        /// <summary>
        /// Retrieves information for all document sets in the document library.
        /// </summary>
        /// <returns>A list of all document sets in the document library.</returns>
        IEnumerable<DocumentSet> GetDocumentSets();

        /// <summary>
        /// Retrieves information for all document sets containing only documents with one of
        /// the specified file extensions.
        /// </summary>
        /// <param name="extensions">The file extensions.</param>
        /// <returns>A list of all document sets for the specified file extensions.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="extensions" /> is null.</exception>
        IEnumerable<DocumentSet> GetDocumentSets(IEnumerable<DocumentExtension> extensions);

        /// <summary>
        /// Retrieves information about the document extensions in the document library.
        /// </summary>
        /// <returns>A list of all document extensions in the document library.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is expensive.")]
        IEnumerable<DocumentExtension> GetExtensions();

        /// <summary>
        /// Retrieves information about the document tags in the document library.
        /// </summary>
        /// <returns>A list of all document tags in the document library.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method is expensive.")]
        IEnumerable<string> GetTags();
    }
}
