using System.Data.SqlClient;

namespace HP.ScalableTest.Core.DocumentLibrary
{
    /// <summary>
    /// Provides the connection string used to communicate with the TestDocumentLibrary database.
    /// </summary>
    public sealed class DocumentLibraryConnectionString
    {
        private readonly SqlConnectionStringBuilder _sqlBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="DocumentLibraryConnectionString" /> class.
        /// </summary>
        /// <param name="databaseServer">The address of the server hosting the TestDocumentLibrary database.</param>
        public DocumentLibraryConnectionString(string databaseServer)
        {
            _sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = databaseServer,
                InitialCatalog = "TestDocumentLibrary",
                UserID = "document_admin",
                Password = "document_admin",
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,
                ApplicationName = "STF.Core.DocumentLibrary"
            };
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return _sqlBuilder.ToString();
        }
    }
}
