using System;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Internal extensions to <see cref="IFileRepository" />.
    /// </summary>
    public interface IFileRepositoryInternal : IFileRepository
    {
        /// <summary>
        /// Gets the public share path for the specified <see cref="Document" />.
        /// </summary>
        /// <param name="document">The document.</param>
        /// <returns>The public share path for the document.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="document" /> is null.</exception>
        string GetDocumentSharePath(Document document);
    }
}
