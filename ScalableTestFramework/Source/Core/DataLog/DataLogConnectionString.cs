using System.Data.SqlClient;

namespace HP.ScalableTest.Core.DataLog
{
    /// <summary>
    /// Provides the connection string used to communicate with the ScalableTestDataLog database.
    /// </summary>
    public sealed class DataLogConnectionString
    {
        private readonly SqlConnectionStringBuilder _sqlBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="DataLogConnectionString" /> class.
        /// </summary>
        /// <param name="databaseServer">The address of the server hosting the ScalableTestDataLog database.</param>
        public DataLogConnectionString(string databaseServer)
        {
            _sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = databaseServer,
                InitialCatalog = "ScalableTestDataLog",
                UserID = "enterprise_data",
                Password = "enterprise_data",
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,
                ApplicationName = "STF.Core.DataLog"
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
