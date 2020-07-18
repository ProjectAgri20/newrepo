using System.Data.SqlClient;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// Provides static access to the connection strings for the Enterprise Test database.
    /// </summary>
    public static class EnterpriseTestSqlConnection
    {
        /// <summary>
        /// Gets the SQL connection string.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return BuildConnectionString(GlobalSettings.Items[Setting.EnterpriseTestDatabase]);
            }
        }

        /// <summary>
        /// Builds the connection string.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <returns></returns>
        public static string BuildConnectionString(string database)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = database,
                InitialCatalog = "EnterpriseTest",
                PersistSecurityInfo = true,
                UserID = "enterprise_admin",
                Password = "enterprise_admin",
                MultipleActiveResultSets = true
            };

            return sqlBuilder.ToString();
        }
    }
}
