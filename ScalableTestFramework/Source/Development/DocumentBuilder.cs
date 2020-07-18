using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Builder class for creating mock <see cref="Document" /> objects.
    /// </summary>
    public sealed class DocumentBuilder
    {
        private readonly Random _rand = new Random();

        /// <summary>
        /// Gets or sets the group to be applied to constructed <see cref="Document" /> objects.
        /// </summary>
        public string Group { get; set; }

        /// <summary>
        /// Gets or sets the minimum file size generated for constructed <see cref="Document" /> objects.
        /// Default value is 0 KB.
        /// </summary>
        public int MinFileSize { get; set; } = 0;

        /// <summary>
        /// Gets or sets the maximum file size generated for constructed <see cref="Document" /> objects.
        /// Default value is 1024 KB.
        /// </summary>
        public int MaxFileSize { get; set; } = 1024;

        /// <summary>
        /// Gets or sets the minimum page count generated for constructed <see cref="Document" /> objects.
        /// Default value is 1 page.
        /// </summary>
        public int MinPageCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the maximum page count generated for constructed <see cref="Document" /> objects.
        /// Default value is 100 pages.
        /// </summary>
        public int MaxPageCount { get; set; } = 100;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentBuilder" /> class.
        /// </summary>
        public DocumentBuilder()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Builds a mock <see cref="Document" /> object with the specified file name
        /// and properties randomly generated from the specifications in this <see cref="DocumentBuilder" />.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <returns>A mock <see cref="Document" /> object with the specified file name.</returns>
        public Document BuildDocument(string fileName)
        {
            return BuildDocument(fileName, SequentialGuid.NewGuid());
        }

        /// <summary>
        /// Builds a mock <see cref="Document" /> object with the specified file name and ID
        /// and properties randomly generated from the specifications in this <see cref="DocumentBuilder" />.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="documentId">The document identifier.</param>
        /// <returns>A mock <see cref="Document" /> object with the specified file name and ID.</returns>
        public Document BuildDocument(string fileName, Guid documentId)
        {
            int fileSize = _rand.Next(MinFileSize, MaxFileSize);
            int pageCount = _rand.Next(MinPageCount, MaxPageCount);
            ColorMode colorMode = Random<ColorMode>();
            Orientation orientation = Random<Orientation>();

            return new Document(documentId, fileName, Group, fileSize, pageCount, colorMode, orientation);
        }

        /// <summary>
        /// Builds a list of mock <see cref="Document" /> objects with the specified file names
        /// and properties randomly generated from the specifications in this <see cref="DocumentBuilder" />.
        /// </summary>
        /// <param name="fileNames">The file names.</param>
        /// <returns>A list of mock <see cref="Document" /> objects with the specified file names.</returns>
        public IEnumerable<Document> BuildDocuments(params string[] fileNames)
        {
            return fileNames.Select(BuildDocument);
        }

        /// <summary>
        /// Builds a list of mock <see cref="Document" /> objects with generated file names
        /// and properties randomly generated from the specifications in this <see cref="DocumentBuilder" />.
        /// </summary>
        /// <param name="fileNameBase">The base string to use for generating file names.</param>
        /// <param name="count">The number of documents to generate.</param>
        /// <returns>A list of mock <see cref="Document" /> objects with generated file names.</returns>
        public IEnumerable<Document> BuildDocuments(string fileNameBase, int count)
        {
            string name = Path.GetFileNameWithoutExtension(fileNameBase);
            string extension = Path.GetExtension(fileNameBase);

            var names = Enumerable.Range(1, count).Select(n => name + n + extension);
            return BuildDocuments(names.ToArray());
        }

        private T Random<T>() where T : Enum
        {
            var values = Enum.GetValues(typeof(T));
            int index = _rand.Next(values.Length);
            return (T)values.GetValue(index);
        }
    }
}
