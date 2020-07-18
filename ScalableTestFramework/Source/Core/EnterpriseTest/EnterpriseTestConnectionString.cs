using System.Data.SqlClient;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Provides the connection string used to communicate with the EnterpriseTest database.
    /// </summary>
    public sealed class EnterpriseTestConnectionString
    {
        private readonly SqlConnectionStringBuilder _sqlBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestConnectionString" /> class.
        /// </summary>
        /// <param name="databaseServer">The address of the server hosting the EnterpriseTest database.</param>
        public EnterpriseTestConnectionString(string databaseServer)
        {
            _sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = databaseServer,
                InitialCatalog = "EnterpriseTest",
                UserID = "enterprise_admin",
                Password = "enterprise_admin",
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,
                ApplicationName = "STF.Core.EnterpriseTest"
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
