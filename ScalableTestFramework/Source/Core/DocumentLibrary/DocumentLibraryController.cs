using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Documents;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.DocumentLibrary
{
    /// <summary>
    /// Implementation of <see cref="IDocumentLibrary" /> that retrieves information from a database using a <see cref="DocumentLibraryContext" />.
    /// </summary>
    public sealed class DocumentLibraryController : IDocumentLibrary
    {
        private readonly DocumentLibraryConnectionString _connectionString;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentLibraryController" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="DocumentLibraryConnectionString" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public DocumentLibraryController(DocumentLibraryConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Retrieves information for all documents in the document library.
        /// </summary>
        /// <returns>A <see cref="DocumentCollection" /> containing all documents in the library.</returns>
        public DocumentCollection GetDocuments()
        {
            using (DocumentLibraryContext context = new DocumentLibraryContext(_connectionString))
            {
                return context.TestDocuments.ToDocumentCollection();
            }
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

            using (DocumentLibraryContext context = new DocumentLibraryContext(_connectionString))
            {
                var stringExtensions = extensions.Select(n => n.Extension);
                return context.TestDocuments.Where(n => stringExtensions.Contains(n.Extension)).ToDocumentCollection();
            }
        }

        /// <summary>
        /// Retrieves information for documents based on the provided criteria.
        /// </summary>
        /// <param name="query">The <see cref="DocumentQuery" /> defining the criteria to be used in selecting documents.</param>
        /// <returns>A <see cref="DocumentCollection" /> containing documents matching the specified criteria.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="query" /> is null.</exception>
        public DocumentCollection GetDocuments(DocumentQuery query)
        {
            if (query == null)
            {
                throw new ArgumentNullException(nameof(query));
            }

            using (DocumentLibraryContext context = new DocumentLibraryContext(_connectionString))
            {
                string sqlQuery = TestDocumentQueryBuilder.BuildQuery(query);
                LogDebug($"Executing Document Library database query: {sqlQuery}");
                return context.Database.SqlQuery<TestDocumentQueryResult>(sqlQuery).ToDocumentCollection();
            }
        }

        /// <summary>
        /// Retrieves information for all document sets in the document library.
        /// </summary>
        /// <returns>A list of all document sets in the document library.</returns>
        public IEnumerable<DocumentSet> GetDocumentSets()
        {
            using (DocumentLibraryContext context = new DocumentLibraryContext(_connectionString))
            {
                return context.TestDocumentSets.ToDocumentSets();
            }
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

            using (DocumentLibraryContext context = new DocumentLibraryContext(_connectionString))
            {
                var stringSetTypes = extensions.Select(n => n.FileType);
                return context.TestDocumentSets.Where(n => stringSetTypes.Contains(n.SetType)).ToDocumentSets();
            }
        }

        /// <summary>
        /// Retrieves information about the document extensions in the document library.
        /// </summary>
        /// <returns>A list of all document extensions in the document library.</returns>
        public IEnumerable<DocumentExtension> GetExtensions()
        {
            using (DocumentLibraryContext context = new DocumentLibraryContext(_connectionString))
            {
                return context.TestDocumentExtensions.ToDocumentExtensions();
            }
        }

        /// <summary>
        /// Retrieves information about the document tags in the document library.
        /// </summary>
        /// <returns>A list of all document tags in the document library.</returns>
        public IEnumerable<string> GetTags()
        {
            using (DocumentLibraryContext context = new DocumentLibraryContext(_connectionString))
            {
                return context.TestDocuments.Select(n => n.Tag).Distinct().Where(n => !string.IsNullOrEmpty(n)).ToList();
            }
        }
    }
}
