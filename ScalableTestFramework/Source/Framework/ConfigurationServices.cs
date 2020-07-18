using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Provides access to framework services available to an <see cref="IPluginConfigurationControl" />.
    /// </summary>
    public static class ConfigurationServices
    {
        private static IAssetInventory _assetInventory;
        private static IDocumentLibrary _documentLibrary;
        private static IEnvironmentConfiguration _environmentConfiguration;
        private static ISystemTrace _systemTrace;

        /// <summary>
        /// Provides access to information stored in Asset Inventory.
        /// </summary>
        /// <exception cref="FrameworkServiceUnavailableException">The configuration service is not available in the current context.</exception>
        public static IAssetInventory AssetInventory
        {
            get => _assetInventory ?? throw new FrameworkServiceUnavailableException("Configuration service Asset Inventory is not available in the current context.");
            internal set => _assetInventory = value;
        }

        /// <summary>
        /// Provides access to document information stored in the Document Library.
        /// </summary>
        /// <exception cref="FrameworkServiceUnavailableException">The configuration service is not available in the current context.</exception>
        public static IDocumentLibrary DocumentLibrary
        {
            get => _documentLibrary ?? throw new FrameworkServiceUnavailableException("Configuration service Document Library is not available in the current context.");
            internal set => _documentLibrary = value;
        }

        /// <summary>
        /// Provides access to test environment configuration information.
        /// </summary>
        /// <exception cref="FrameworkServiceUnavailableException">The configuration service is not available in the current context.</exception>
        public static IEnvironmentConfiguration EnvironmentConfiguration
        {
            get => _environmentConfiguration ?? throw new FrameworkServiceUnavailableException("Configuration service Environment Configuration is not available in the current context.");
            internal set => _environmentConfiguration = value;
        }

        /// <summary>
        /// Provides trace logging capability for debugging purposes.
        /// </summary>
        /// <exception cref="FrameworkServiceUnavailableException">The configuration service is not available in the current context.</exception>
        public static ISystemTrace SystemTrace
        {
            get => _systemTrace ?? throw new FrameworkServiceUnavailableException("Configuration service System Trace is not available in the current context.");
            internal set => _systemTrace = value;
        }
    }
}
