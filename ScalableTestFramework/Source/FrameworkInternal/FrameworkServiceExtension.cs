using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Provides extension methods for framework service classes.
    /// </summary>
    public static class FrameworkServiceExtension
    {
        /// <summary>
        /// Casts the specified <see cref="IAssetInventory" /> to an <see cref="IAssetInventoryInternal" /> object.
        /// </summary>
        /// <param name="assetInventory">The <see cref="IAssetInventory" /> instance to cast.</param>
        /// <returns>The specified <see cref="IAssetInventory" /> object cast to <see cref="IAssetInventoryInternal" />.</returns>
        /// <exception cref="FrameworkServiceUnavailableException"><paramref name="assetInventory" /> does not implement <see cref="IAssetInventoryInternal" />.</exception>
        public static IAssetInventoryInternal AsInternal(this IAssetInventory assetInventory)
        {
            return assetInventory as IAssetInventoryInternal ?? throw new FrameworkServiceUnavailableException("Asset Inventory service does not implement internal members.");
        }

        /// <summary>
        /// Casts the specified <see cref="IEnvironmentConfiguration" /> to an <see cref="IEnvironmentConfigurationInternal" /> object.
        /// </summary>
        /// <param name="environmentConfiguration">The <see cref="IEnvironmentConfiguration" /> instance to cast.</param>
        /// <returns>The specified <see cref="IEnvironmentConfiguration" /> object cast to <see cref="IEnvironmentConfigurationInternal" />.</returns>
        /// <exception cref="FrameworkServiceUnavailableException"><paramref name="environmentConfiguration" /> does not implement <see cref="IEnvironmentConfigurationInternal" />.</exception>
        public static IEnvironmentConfigurationInternal AsInternal(this IEnvironmentConfiguration environmentConfiguration)
        {
            return environmentConfiguration as IEnvironmentConfigurationInternal ?? throw new FrameworkServiceUnavailableException("Environment Configuration service does not implement internal members.");
        }

        /// <summary>
        /// Casts the specified <see cref="IDataLogger" /> to an <see cref="IDataLoggerInternal" /> object.
        /// </summary>
        /// <param name="dataLogger">The <see cref="IDataLogger" /> instance to cast.</param>
        /// <returns>The specified <see cref="IDataLogger" /> object cast to <see cref="IDataLoggerInternal" />.</returns>
        /// <exception cref="FrameworkServiceUnavailableException"><paramref name="dataLogger" /> does not implement <see cref="IDataLoggerInternal" />.</exception>
        public static IDataLoggerInternal AsInternal(this IDataLogger dataLogger)
        {
            return dataLogger as IDataLoggerInternal ?? throw new FrameworkServiceUnavailableException("Data Logger service does not implement internal members.");
        }

        /// <summary>
        /// Casts the specified <see cref="IFileRepository" /> to an <see cref="IFileRepositoryInternal" /> object.
        /// </summary>
        /// <param name="fileRepository">The <see cref="IFileRepository" /> instance to cast.</param>
        /// <returns>The specified <see cref="IFileRepository" /> object cast to <see cref="IFileRepositoryInternal" />.</returns>
        /// <exception cref="FrameworkServiceUnavailableException"><paramref name="fileRepository" /> does not implement <see cref="IFileRepositoryInternal" />.</exception>
        public static IFileRepositoryInternal AsInternal(this IFileRepository fileRepository)
        {
            return fileRepository as IFileRepositoryInternal ?? throw new FrameworkServiceUnavailableException("File Repository service does not implement internal members.");
        }

        /// <summary>
        /// Casts the specified <see cref="ISessionRuntime" /> to an <see cref="ISessionRuntimeInternal" /> object.
        /// </summary>
        /// <param name="sessionRuntime">The <see cref="ISessionRuntime" /> instance to cast.</param>
        /// <returns>The specified <see cref="ISessionRuntime" /> object cast to <see cref="ISessionRuntimeInternal" />.</returns>
        /// <exception cref="FrameworkServiceUnavailableException"><paramref name="sessionRuntime" /> does not implement <see cref="ISessionRuntimeInternal" />.</exception>
        public static ISessionRuntimeInternal AsInternal(this ISessionRuntime sessionRuntime)
        {
            return sessionRuntime as ISessionRuntimeInternal ?? throw new FrameworkServiceUnavailableException("Session Runtime service does not implement internal members.");
        }
    }
}
