using System;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Core.EnterpriseTest;

namespace HP.ScalableTest.Core.Configuration
{
    /// <summary>
    /// Contains connection strings and static methods for connecting to STF databases.
    /// </summary>
    public static class DbConnect
    {
        /// <summary>
        /// Gets or sets the <see cref="AssetInventory.AssetInventoryConnectionString" />.
        /// </summary>
        public static AssetInventoryConnectionString AssetInventoryConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DataLog.DataLogConnectionString" />.
        /// </summary>
        public static DataLogConnectionString DataLogConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="DocumentLibrary.DocumentLibraryConnectionString" />.
        /// </summary>
        public static DocumentLibraryConnectionString DocumentLibraryConnectionString { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="EnterpriseTest.EnterpriseTestConnectionString" />.
        /// </summary>
        public static EnterpriseTestConnectionString EnterpriseTestConnectionString { get; set; }

        /// <summary>
        /// Creates a new <see cref="AssetInventory.AssetInventoryContext" /> connection.
        /// </summary>
        /// <returns>A new <see cref="AssetInventory.AssetInventoryContext" />.</returns>
        /// <exception cref="InvalidOperationException">The asset inventory connection string has not been initialized.</exception>
        public static AssetInventoryContext AssetInventoryContext()
        {
            if (AssetInventoryConnectionString == null)
            {
                throw new InvalidOperationException("Asset inventory connection string has not been initialized.");
            }

            return new AssetInventoryContext(AssetInventoryConnectionString);
        }

        /// <summary>
        /// Creates a new <see cref="DataLog.DataLogContext" /> connection.
        /// </summary>
        /// <returns>A new <see cref="DataLog.DataLogContext" />.</returns>
        /// <exception cref="InvalidOperationException">The data log connection string has not been initialized.</exception>
        public static DataLogContext DataLogContext()
        {
            if (DataLogConnectionString == null)
            {
                throw new InvalidOperationException("Data log connection string has not been initialized.");
            }

            return new DataLogContext(DataLogConnectionString);
        }

        /// <summary>
        /// Creates a new <see cref="DocumentLibrary.DocumentLibraryContext" /> connection.
        /// </summary>
        /// <returns>A new <see cref="DocumentLibrary.DocumentLibraryContext" />.</returns>
        /// <exception cref="InvalidOperationException">The document library connection string has not been initialized.</exception>
        public static DocumentLibraryContext DocumentLibraryContext()
        {
            if (DocumentLibraryConnectionString == null)
            {
                throw new InvalidOperationException("Document library connection string has not been initialized.");
            }

            return new DocumentLibraryContext(DocumentLibraryConnectionString);
        }

        /// <summary>
        /// Creates a new <see cref="EnterpriseTest.EnterpriseTestContext" /> connection.
        /// </summary>
        /// <returns>A new <see cref="EnterpriseTest.EnterpriseTestContext" />.</returns>
        /// <exception cref="InvalidOperationException">The enterprise test connection string has not been initialized.</exception>
        public static EnterpriseTestContext EnterpriseTestContext()
        {
            if (EnterpriseTestConnectionString == null)
            {
                throw new InvalidOperationException("Enterprise test connection string has not been initialized.");
            }

            return new EnterpriseTestContext(EnterpriseTestConnectionString);
        }
    }
}
