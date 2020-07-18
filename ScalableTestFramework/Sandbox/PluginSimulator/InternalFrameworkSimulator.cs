using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HP.ScalableTest.Core.Plugin;
using HP.ScalableTest.Development;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DeviceSettings;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.Synchronization;

namespace PluginSimulator
{
    internal sealed class InternalFrameworkSimulator : IPluginFrameworkSimulator
    {
        private readonly AssetInventoryMockInternal _assetInventory = new AssetInventoryMockInternal();
        private readonly CriticalSectionMockInternal _criticalSection = new CriticalSectionMockInternal();
        private readonly DataLoggerMockInternal _dataLogger = new DataLoggerMockInternal();
        private readonly DocumentLibraryMockInternal _documentLibrary = new DocumentLibraryMockInternal();
        private readonly EnvironmentConfigurationMockInternal _environmentConfiguration = new EnvironmentConfigurationMockInternal();
        private readonly FileRepositoryMockInternal _fileRepository = new FileRepositoryMockInternal();
        private readonly SessionRuntimeMockInternal _sessionRuntime = new SessionRuntimeMockInternal();
        private readonly SystemTraceLogger _systemTrace = new SystemTraceLogger();
        private readonly Dictionary<string, string> _pluginSettings = new Dictionary<string, string>();
        private readonly Collection<PluginRetrySetting> _retrySettings = new Collection<PluginRetrySetting>();

        /// <summary>
        /// The <see cref="AssetInventoryMockInternal" /> for the simulator.
        /// </summary>
        public AssetInventoryMockInternal AssetInventory => _assetInventory;

        /// <summary>
        /// The <see cref="CriticalSectionMockInternal" /> for the simulator.
        /// </summary>
        /// <value>The critical section.</value>
        public CriticalSectionMockInternal CriticalSection => _criticalSection;

        /// <summary>
        /// The <see cref="DataLoggerMockInternal" /> for the simulator.
        /// </summary>
        public DataLoggerMockInternal DataLogger => _dataLogger;

        /// <summary>
        /// The <see cref="DocumentLibraryMockInternal" /> for the simulator.
        /// </summary>
        public DocumentLibraryMockInternal DocumentLibrary => _documentLibrary;

        /// <summary>
        /// The <see cref="EnvironmentConfigurationMockInternal" /> for the simulator.
        /// </summary>
        public EnvironmentConfigurationMockInternal EnvironmentConfiguration => _environmentConfiguration;

        /// <summary>
        /// The <see cref="FileRepositoryMockInternal" /> for the simulator.
        /// </summary>
        public FileRepositoryMockInternal FileRepository => _fileRepository;

        /// <summary>
        /// The <see cref="SessionRuntimeMockInternal" /> for the simulator.
        /// </summary>
        public SessionRuntimeMockInternal SessionRuntime => _sessionRuntime;

        /// <summary>
        /// The <see cref="SystemTraceLogger" /> for the simulator.
        /// </summary>
        public SystemTraceLogger SystemTrace => _systemTrace;

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
        public string PluginAssemblyName { get; private set; }

        /// <summary>
        /// Gets or sets the type of <see cref="IPluginConfigurationControl" /> to use.
        /// </summary>
        /// <value>The type of <see cref="IPluginConfigurationControl" /> to use.</value>
        public Type ConfigurationControlType { get; }

        /// <summary>
        /// Gets or sets the type of <see cref="IPluginExecutionEngine" /> to use.
        /// </summary>
        /// <value>The type of <see cref="IPluginExecutionEngine" /> to use.</value>
        public Type ExecutionEngineType { get; }

        /// <summary>
        /// Gets or sets the session ID to provide in <see cref="PluginExecutionContext" />.
        /// </summary>
        /// <value>The session ID.</value>
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the user name to provide in <see cref="PluginExecutionContext" />.
        /// </summary>
        /// <value>The user name.</value>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user password to provide in <see cref="PluginExecutionContext" />.
        /// </summary>
        /// <value>The user password.</value>
        public string UserPassword { get; set; }

        /// <summary>
        /// Gets or sets the user domain to provide in <see cref="PluginEnvironment" />.
        /// </summary>
        /// <value>The user domain.</value>
        public string UserDomain { get; set; }

        /// <summary>
        /// Gets or sets the user DNS domain to provide in <see cref="PluginEnvironment" />.
        /// </summary>
        /// <value>The user DNS domain.</value>
        public string UserDnsDomain { get; set; }

        /// <summary>
        /// Gets or sets the desired job media mode for devices during execution.
        /// </summary>
        public JobMediaMode PaperlessMode { get; set; } = JobMediaMode.Paperless;

        /// <summary>
        /// Initializes a new instance of the <see cref="InternalFrameworkSimulator" /> class.
        /// </summary>
        /// <param name="globalDatabase">The host name of the machine hosting the global databases.</param>
        public InternalFrameworkSimulator(string globalDatabase, PluginAssembly pluginAssembly)
        {
            _assetInventory.Database = globalDatabase;
            _documentLibrary.Database = globalDatabase;

            Logger.Initialize(new SystemTraceLogger(typeof(Logger)));

            ConfigurationControlType = pluginAssembly.Create<IPluginConfigurationControl>().GetType();
            ExecutionEngineType = pluginAssembly.Create<IPluginExecutionEngine>().GetType();
            PluginAssemblyName = pluginAssembly.Name;
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

            // Set job media mode
            if (PaperlessMode != JobMediaMode.Unknown)
            {
                foreach (DeviceInfo deviceInfo in selectedAssets.Where(n => n.Attributes.HasFlag(AssetAttributes.Printer)))
                {
                    using (var device = DeviceConstructor.Create(deviceInfo))
                    {
                        try
                        {
                            DeviceSettingsManagerFactory.Create(device).SetJobMediaMode(PaperlessMode);
                        }
                        catch
                        {
                            //Did not set paperless mode.  Ignore error.
                            System.Diagnostics.Debug.WriteLine($"Error setting paperless mode.  {executionAssets.ToString()}");
                        }
                    }
                }
            }

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
                new PluginRetrySettingDictionary(RetrySettings),
                new ExternalCredentialInfoCollection(_assetInventory.GetExternalCredentials(executionContext.UserName))
            );
        }

        #endregion
    }
}
