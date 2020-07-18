using System.Data.SqlClient;

namespace HP.ScalableTest.Core.AssetInventory
{
    /// <summary>
    /// Provides the connection string used to communicate with the AssetInventory database.
    /// </summary>
    public sealed class AssetInventoryConnectionString
    {
        private readonly SqlConnectionStringBuilder _sqlBuilder;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInventoryConnectionString" /> class.
        /// </summary>
        /// <param name="databaseServer">The address of the server hosting the AssetInventory database.</param>
        public AssetInventoryConnectionString(string databaseServer)
        {
            _sqlBuilder = new SqlConnectionStringBuilder
            {
                DataSource = databaseServer,
                InitialCatalog = "AssetInventory",
                UserID = "asset_admin",
                Password = "asset_admin",
                PersistSecurityInfo = true,
                MultipleActiveResultSets = true,
                ApplicationName = "STF.Core.AssetInventory"
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
