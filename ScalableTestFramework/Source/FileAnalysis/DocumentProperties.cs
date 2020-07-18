using System;
using System.IO;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.FileAnalysis
{
    /// <summary>
    /// Contains various properties of a file or document.
    /// </summary>
    public sealed class DocumentProperties
    {
        private readonly FileInfo _file;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentProperties" /> class.
        /// </summary>
        /// <param name="file">The file.</param>
        /// <exception cref="ArgumentNullException"><paramref name="file" /> is null.</exception>
        public DocumentProperties(FileInfo file)
        {
            _file = file ?? throw new ArgumentNullException(nameof(file));
        }

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName => _file.Name;

        /// <summary>
        /// Gets the file path.
        /// </summary>
        public string FilePath => _file.FullName;

        /// <summary>
        /// Gets the file size, in bytes.
        /// </summary>
        public long FileSize => _file.Length;

        /// <summary>
        /// Gets or sets the number of pages in the file.
        /// </summary>
        public short Pages { get; set; }

        /// <summary>
        /// Gets or sets the title of the document.
        /// </summary>
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the author of the document.
        /// </summary>
        public string Author { get; set; }

        /// <summary>
        /// Gets or sets the application used to create this document.
        /// </summary>
        public string Application { get; set; }

        /// <summary>
        /// Gets or sets the page orientation of this document.
        /// </summary>
        public Orientation? Orientation { get; set; }
    }
}
