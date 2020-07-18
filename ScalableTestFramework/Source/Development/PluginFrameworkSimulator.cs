using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// A simulated version of the framework for use during plugin development.
    /// </summary>
    public sealed class PluginFrameworkSimulator : IPluginFrameworkSimulator
    {
        private readonly AssetInventoryMock _assetInventory = new AssetInventoryMock();
        private readonly CriticalSectionMock _criticalSection = new CriticalSectionMock();
        private readonly DataLoggerMock _dataLogger = new DataLoggerMock();
        private readonly DocumentLibraryMock _documentLibrary = new DocumentLibraryMock();
        private readonly EnvironmentConfigurationMock _environmentConfiguration = new EnvironmentConfigurationMock();
        private readonly FileRepositoryMock _fileRepository = new FileRepositoryMock();
        private readonly SessionRuntimeMock _sessionRuntime = new SessionRuntimeMock();
        private readonly SystemTraceMock _systemTrace = new SystemTraceMock();
        private readonly Dictionary<string, string> _pluginSettings = new Dictionary<string, string>();
        private readonly Collection<PluginRetrySetting> _retrySettings = new Collection<PluginRetrySetting>();

        /// <summary>
        /// The <see cref="AssetInventoryMock" /> for the simulator.
        /// </summary>
        public AssetInventoryMock AssetInventory => _assetInventory;

        /// <summary>
        /// The <see cref="CriticalSectionMock" /> for the simulator.
        /// </summary>
        public CriticalSectionMock CriticalSection => _criticalSection;

        /// <summary>
        /// The <see cref="DataLoggerMock" /> for the simulator.
        /// </summary>
        public DataLoggerMock DataLogger => _dataLogger;

        /// <summary>
        /// The <see cref="DocumentLibraryMock" /> for the simulator.
        /// </summary>
        public DocumentLibraryMock DocumentLibrary => _documentLibrary;

        /// <summary>
        /// The <see cref="EnvironmentConfigurationMock" /> for the simulator.
        /// </summary>
        public EnvironmentConfigurationMock EnvironmentConfiguration => _environmentConfiguration;

        /// <summary>
        /// The <see cref="FileRepositoryMock" /> for the simulator.
        /// </summary>
        public FileRepositoryMock FileRepository => _fileRepository;

        /// <summary>
        /// The <see cref="SessionRuntimeMock" /> for the simulator.
        /// </summary>
        public SessionRuntimeMock SessionRuntime => _sessionRuntime;

        /// <summary>
        /// The <see cref="SystemTraceMock" /> for the simulator.
        /// </summary>
        public SystemTraceMock SystemTrace => _systemTrace;

        /// <summary>
        /// The plugin settings to provide in <see cref="PluginEnvironment" />.
        /// </summary>
        public Dictionary<string, string> PluginSettings => _pluginSettings;

        /// <summary>
        /// The plugin retry settings to provide in <see cref="PluginExecutionContext" />.
        /// </summary>
        public Collection<PluginRetrySetting> RetrySettings => _retrySettings;

        /// <summary>
        /// Gets the Name of the loaded Plugin Assembly.
        /// </summary>
        public string PluginAssemblyName { get; set; }

        /// <summary>
        /// Gets or sets the type of <see cref="IPluginConfigurationControl" /> to use.
        /// </summary>
        public Type ConfigurationControlType { get; set; }

        /// <summary>
        /// Gets or sets the type of <see cref="IPluginExecutionEngine" /> to use.
        /// </summary>
        public Type ExecutionEngineType { get; set; }

        /// <summary>
        /// Gets or sets the session ID to provide in <see cref="PluginExecutionContext" />.
        /// </summary>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the user name to provide in <see cref="PluginExecutionContext" />.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user password to provide in <see cref="PluginExecutionContext" />.
        /// </summary>
        public string UserPassword { get; set; }

        /// <summary>
        /// Gets or sets the user domain to provide in <see cref="PluginEnvironment" />.
        /// </summary>
        public string UserDomain { get; set; }

        /// <summary>
        /// Gets or sets the user DNS domain to provide in <see cref="PluginEnvironment" />.
        /// </summary>
        public string UserDnsDomain { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginFrameworkSimulator" /> class.
        /// </summary>
        public PluginFrameworkSimulator()
        {
            // Constructor explicitly declared for XML doc.
        }

        #region Explicit IPluginFrameworkSimulator Members

        IAssetInventory IPluginFrameworkSimulator.AssetInventory => _assetInventory;
        ICriticalSection IPluginFrameworkSimulator.CriticalSection => _criticalSection;
        IDataLogger IPluginFrameworkSimulator.DataLogger => _dataLogger;
        IDocumentLibrary IPluginFrameworkSimulator.DocumentLibrary => _documentLibrary;
        IEnvironmentConfiguration IPluginFrameworkSimulator.EnvironmentConfiguration => _environmentConfiguration;
        IFileRepository IPluginFrameworkSimulator.FileRepository => _fileRepository;
        ISessionRuntime IPluginFrameworkSimulator.SessionRuntime => _sessionRuntime;
        ISystemTrace IPluginFrameworkSimulator.SystemTrace => _systemTrace;

        PluginEnvironment IPluginFrameworkSimulator.Environment
        {
            get
            {
                return new PluginEnvironment
                (
                    new SettingsDictionary(_pluginSettings),
                    UserDomain,
                    UserDnsDomain
                );
            }
        }

        PluginExecutionData IPluginFrameworkSimulator.CreateExecutionData(PluginConfigurationData configurationData)
        {
            if (configurationData == null)
            {
                throw new ArgumentNullException(nameof(configurationData));
            }

            PluginExecutionContext executionContext = new PluginExecutionContext
            {
                ActivityExecutionId = SequentialGuid.NewGuid(),
                SessionId = SessionId,
                UserName = UserName,
                UserPassword = UserPassword
            };

            // Retrieve all selected assets, then add any badge boxes associated with those assets
            var selectedAssets = PluginConfigurationTransformer.GetExecutionAssets(configurationData, _assetInventory);
            var badgeBoxes = _assetInventory.GetBadgeBoxes(selectedAssets);
            AssetInfoCollection executionAssets = new AssetInfoCollection(selectedAssets.Union(badgeBoxes).ToList());

            return new PluginExecutionData
            (
                configurationData.GetMetadata(),
                configurationData.MetadataVersion,
                executionAssets,
                PluginConfigurationTransformer.GetExecutionDocuments(configurationData, _documentLibrary),
                PluginConfigurationTransformer.GetExecutionServers(configurationData, _assetInventory),
                PluginConfigurationTransformer.GetExecutionPrintQueues(configurationData, _assetInventory),
                (this as IPluginFrameworkSimulator).Environment,
                executionContext,
                new PluginRetrySettingDictionary(RetrySettings)
            );
        }

        #endregion
    }
}
