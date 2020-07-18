using System.Data.SqlClient;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.UI.Reporting
{
    /// <summary>
    /// Provides static access to the connection strings for the Enterprise Test database.
    /// </summary>
    public static class ReportingSqlConnection
    {
        /// <summary>
        /// Gets the SQL connection string.
        /// </summary>
        public static string ConnectionString
        {
            get
            {
                return BuildConnectionString(GlobalSettings.Items[Setting.ReportingDatabaseServer], GlobalSettings.Items[Setting.ReportingDatabase]);
            }
        }

        /// <summary>
        /// Builds the connection string.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <returns></returns>
        public static string BuildConnectionString(string dataSource, string database)
        {
            SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder()
            {
                DataSource = dataSource,
                InitialCatalog = database,
                PersistSecurityInfo = true,
                UserID = "report_viewer",
                Password = "report_viewer",
                MultipleActiveResultSets = true
            };

            return sqlBuilder.ToString();
        }
    }
}
