using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Utility;
using PluginSimulator.Properties;

namespace PluginSimulator
{
    internal sealed class DocumentLibraryMockInternal : IDocumentLibrary
    {
        /// <summary>
        /// Gets or sets the hostname or address of the database to connect to.
        /// </summary>
        public string Database { get; set; }

        /// <summary>
        /// Retrieves information for all documents in the document library.
        /// </summary>
        /// <returns>A <see cref="DocumentCollection" /> containing all documents in the library.</returns>
        public DocumentCollection GetDocuments()
        {
            List<Document> documents = new List<Document>();

            DatabaseQuery(Resources.SelectDocuments, record =>
            {
                Document document = BuildDocument(record);
                documents.Add(document);
            });

            return new DocumentCollection(documents);
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

            List<Document> documents = new List<Document>();

            DatabaseQuery(Resources.SelectDocuments, record =>
            {
                if (extensions.Select(n => n.Extension).Contains(record["Extension"] as string))
                {
                    Document document = BuildDocument(record);
                    documents.Add(document);
                }
            });

            return new DocumentCollection(documents);
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

            // No filtering in the mock
            return GetDocuments();
        }

        private static Document BuildDocument(IDataRecord record)
        {
            return new Document
            (
                (Guid)record["TestDocumentId"],
                record["FileName"] as string,
                record["Group"] as string,
                (long)record["FileSize"],
                (int)record["Pages"],
                EnumUtil.Parse<ColorMode>((string)record["ColorMode"]),
                EnumUtil.Parse<Orientation>((string)record["Orientation"])
            );
        }

        /// <summary>
        /// Retrieves information for all document sets in the document library.
        /// </summary>
        /// <returns>A list of all document sets in the document library.</returns>
        public IEnumerable<DocumentSet> GetDocumentSets()
        {
            // Not implemented
            yield break;
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
            // Not implemented
            yield break;
        }

        /// <summary>
        /// Retrieves information about the document extensions in the document library.
        /// </summary>
        /// <returns>A list of all document extensions in the document library.</returns>
        public IEnumerable<DocumentExtension> GetExtensions()
        {
            List<DocumentExtension> extensions = new List<DocumentExtension>();

            DatabaseQuery(Resources.SelectDocumentExtensions, record =>
            {
                DocumentExtension extension = new DocumentExtension
                (
                    record["Extension"] as string,
                    record["FileType"] as string,
                    record["ContentType"] as string
                );
                extensions.Add(extension);
            });

            return extensions;
        }

        /// <summary>
        /// Retrieves information about the document tags in the document library.
        /// </summary>
        /// <returns>A list of all document tags in the document library.</returns>
        public IEnumerable<string> GetTags()
        {
            List<string> tags = new List<string>();

            DatabaseQuery(Resources.SelectDocumentTags, record =>
            {
                string tag = record["Tag"] as string;
                if (!string.IsNullOrEmpty(tag))
                {
                    tags.Add(tag);
                }
            });
            return tags;
        }

        private void DatabaseQuery(string sql, Action<IDataRecord> read)
        {
            SqlConnectionStringBuilder builder = new SqlConnectionStringBuilder
            {
                DataSource = Database,
                InitialCatalog = "TestDocumentLibrary",
                UserID = "document_admin",
                Password = "document_admin",
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true
            };

            using (SqlConnection connection = new SqlConnection(builder.ConnectionString))
            {
                SqlCommand command = new SqlCommand(sql, connection);
                connection.Open();

                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    read(reader);
                }

                reader.Close();
            }
        }
    }
}
