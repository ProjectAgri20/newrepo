using System.IO;
using HP.ScalableTest.Development;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.PluginService;

namespace SDKPluginDevelopment
{
    /// <summary>
    /// Wrapper around <see cref="FileRepositoryMock" /> that also implements <see cref="IFileRepositoryInternal" />.
    /// </summary>
    public sealed class FileRepositoryMockInternal : IFileRepository, IFileRepositoryInternal
    {
        private readonly FileRepositoryMock _fileRepositoryMock;
        private readonly FileRepositoryAdapter _fileRepositoryAdapter;

        /// <summary>
        /// Gets or sets the local file path to look for documents.
        /// </summary>
        /// <value>The local file path.</value>
        public string LocalFilePath
        {
            get { return _fileRepositoryMock.LocalFilePath; }
            set { _fileRepositoryMock.LocalFilePath = value; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FileRepositoryMockInternal" /> class.
        /// </summary>
        public FileRepositoryMockInternal()
        {
            _fileRepositoryMock = new FileRepositoryMock();
            _fileRepositoryAdapter = new FileRepositoryAdapter();
        }

        // Reroute all IFileRepository calls to the mock
        public FileInfo GetFile(string path) => _fileRepositoryMock.GetFile(path);
        public FileInfo GetFile(string path, bool forceDownload) => _fileRepositoryMock.GetFile(path, forceDownload);
        public FileInfo GetFile(Document document) => _fileRepositoryMock.GetFile(document);
        public FileInfo GetFile(Document document, bool forceDownload) => _fileRepositoryMock.GetFile(document, forceDownload);

        // Use the "real" implementation of IFileRepositoryInternal
        public string GetDocumentSharePath(Document document) => _fileRepositoryAdapter.GetDocumentSharePath(document);
    }
}
