using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Core.DocumentLibrary
{
    /// <summary>
    /// Contains extension methods for queries returned from <see cref="DocumentLibraryContext" />.
    /// </summary>
    public static class DocumentLibraryQueryExtension
    {
        /// <summary>
        /// Creates a <see cref="DocumentCollection" /> from a <see cref="TestDocument" /> query.
        /// </summary>
        /// <param name="testDocumentQuery">The <see cref="TestDocument" /> query.</param>
        /// <returns>A <see cref="DocumentCollection" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="testDocumentQuery" /> is null.</exception>
        public static DocumentCollection ToDocumentCollection(this IQueryable<TestDocument> testDocumentQuery)
        {
            if (testDocumentQuery == null)
            {
                throw new ArgumentNullException(nameof(testDocumentQuery));
            }

            return testDocumentQuery.Select(n => new TestDocumentQueryResult
            {
                TestDocumentId = n.TestDocumentId,
                FileName = n.FileName,
                Location = n.TestDocumentExtension.Location,
                FileSize = n.FileSize,
                Pages = n.Pages,
                ColorMode = n.ColorMode,
                Orientation = n.Orientation
            })
            .AsEnumerable()
            .ToDocumentCollection();
        }

        /// <summary>
        /// Creates a collection of <see cref="DocumentSet" /> from a <see cref="TestDocumentSet" /> query.
        /// </summary>
        /// <param name="testDocumentSetQuery">The <see cref="TestDocumentSet" /> query.</param>
        /// <returns>A collection of <see cref="DocumentSet" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="testDocumentSetQuery" /> is null.</exception>
        public static IEnumerable<DocumentSet> ToDocumentSets(this IQueryable<TestDocumentSet> testDocumentSetQuery)
        {
            if (testDocumentSetQuery == null)
            {
                throw new ArgumentNullException(nameof(testDocumentSetQuery));
            }

            return testDocumentSetQuery.Select(n => new
            {
                n.Name,
                n.SetType,
                Documents = n.TestDocumentSetItems.OrderBy(m => m.SortOrder).Select(m => new TestDocumentQueryResult
                {
                    TestDocumentId = m.TestDocument.TestDocumentId,
                    FileName = m.TestDocument.FileName,
                    Location = m.TestDocument.TestDocumentExtension.Location,
                    FileSize = m.TestDocument.FileSize,
                    Pages = m.TestDocument.Pages,
                    ColorMode = m.TestDocument.ColorMode,
                    Orientation = m.TestDocument.Orientation
                })
            })
            .AsEnumerable()
            .Select(n => new DocumentSet
            (
                n.Name,
                n.SetType,
                n.Documents.ToDocumentCollection()
            )).ToList();
        }

        /// <summary>
        /// Creates a <see cref="DocumentCollection" /> from a <see cref="TestDocumentQueryResult" /> collection.
        /// </summary>
        /// <param name="testDocumentQueryResult">The <see cref="TestDocumentQueryResult" /> collection.</param>
        /// <returns>A collection of <see cref="DocumentSet" />.</returns>
        internal static DocumentCollection ToDocumentCollection(this IEnumerable<TestDocumentQueryResult> testDocumentQueryResult)
        {
            return new DocumentCollection(testDocumentQueryResult.Select(n => n.ToDocument()).ToList());
        }

        /// <summary>
        /// Creates a collection of <see cref="DocumentExtension" /> from a <see cref="TestDocumentExtension" /> query.
        /// </summary>
        /// <param name="testDocumentExtensionQuery">The <see cref="TestDocumentExtension" /> query.</param>
        /// <returns>A collection of <see cref="DocumentExtension" />.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="testDocumentExtensionQuery" /> is null.</exception>
        public static IEnumerable<DocumentExtension> ToDocumentExtensions(this IQueryable<TestDocumentExtension> testDocumentExtensionQuery)
        {
            if (testDocumentExtensionQuery == null)
            {
                throw new ArgumentNullException(nameof(testDocumentExtensionQuery));
            }

            return testDocumentExtensionQuery.AsEnumerable().Select(n => new DocumentExtension
            (
                n.Extension,
                n.Location,
                n.ContentType
            )).ToList();
        }
    }
}
