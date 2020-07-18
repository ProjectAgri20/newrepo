using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using HP.DeviceAutomation;
using HP.ScalableTest.Data.AssetInventory;
using HP.ScalableTest.Data.AssetInventory.Model;
using HP.ScalableTest.Development;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.DeviceAutomation.DeviceSettings;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Data;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Plugin;
using HP.ScalableTest.Framework.PluginService;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Synchronization;

namespace SDKPluginDevelopment
{
    public sealed class InternalFrameworkSimulator : IPluginFrameworkSimulator
    {
        private readonly IAssetInventory _assetInventory = new AssetInventoryAdapter();
        private readonly IDocumentLibrary _documentLibrary = new DocumentLibraryAdapter();
        private readonly IEnvironmentConfiguration _environmentConfiguration = new EnvironmentConfigurationAdapter();

        private readonly CriticalSectionMockInternal _criticalSection = new CriticalSectionMockInternal();
        private readonly DataLoggerMockInternal _dataLogger = new DataLoggerMockInternal();
        private readonly FileRepositoryMockInternal _fileRepository = new FileRepositoryMockInternal();
        private readonly SessionRuntimeMockInternal _sessionRuntime = new SessionRuntimeMockInternal();
        private readonly SystemTraceMock _systemTrace = new SystemTraceMock();
        private readonly Dictionary<string, string> _pluginSettings = new Dictionary<string, string>();
        private readonly Collection<PluginRetrySetting> _retrySettings = new Collection<PluginRetrySetting>();

        public InternalFrameworkSimulator(string database)
        {
            GlobalSettings.Load(database);
            PaperlessModeOn = true;
        }

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
        /// The <see cref="FileRepositoryMock" /> for the simulator.
        /// </summary>
        public FileRepositoryMockInternal FileRepository => _fileRepository;

        /// <summary>
        /// The <see cref="SessionRuntimeMock" /> for the simulator.
        /// </summary>
        public SessionRuntimeMockInternal SessionRuntime => _sessionRuntime;

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
        /// Gets or sets the type of <see cref="IPluginConfigurationControl" /> to use.
        /// </summary>
        /// <value>The type of <see cref="IPluginConfigurationControl" /> to use.</value>
        public Type ConfigurationControlType { get; set; }

        /// <summary>
        /// Gets or sets the type of <see cref="IPluginExecutionEngine" /> to use.
        /// </summary>
        /// <value>The type of <see cref="IPluginExecutionEngine" /> to use.</value>
        public Type ExecutionEngineType { get; set; }

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
        /// Gets or sets the paperless mode on.
        /// </summary>
        /// <value>The paperless mode on.</value>
        public bool PaperlessModeOn { get; set; }

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
            var badgeBoxes = GetBadgeBoxes(selectedAssets);
            AssetInfoCollection executionAssets = new AssetInfoCollection(selectedAssets.Union(badgeBoxes).ToList());

            foreach (DeviceInfo ai in selectedAssets)
            {
                string ip = ai.Address;
                AssetAttributes aa =  ai.Attributes;
                
                if (aa.HasFlag(AssetAttributes.Printer))
                {
                    SetPaperlessPrintMode(ai.Address, ai.AdminPassword);
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
                new PluginRetrySettingDictionary(RetrySettings)
            );
        }

        /// <summary>
        /// Sets Paperless Print Mode to ON or OFF on the device
        /// </summary>
        /// <param name="paperlessModeOn"></param>
        /// <returns></returns>
        private bool SetPaperlessPrintMode(string ipAddress, string adminPassword)
        {
            JobMediaMode mode = PaperlessModeOn ? JobMediaMode.Paperless : JobMediaMode.Paper;
            bool success = false;

            try
            {
                using (var device = DeviceFactory.Create(ipAddress, adminPassword))
                {
                    try
                    {
                        IDeviceSettingsManager manager = DeviceSettingsManagerFactory.Create(device);
                        success = manager.SetJobMediaMode(mode);
                    }
                    catch (DeviceFactoryCoreException)
                    {
                        return false;
                    }
                }
            }
            catch (Exception ex)
            {
                return false;
            }
            return success;
        }

        private List<BadgeBoxInfo> GetBadgeBoxes(IEnumerable<AssetInfo> assets)
        {
            using (AssetInventoryContext context = new AssetInventoryContext())
            {
                var badgeBoxes = BadgeBox.SelectByAsset(context, assets.Select(n => n.AssetId)).ToList();
                return badgeBoxes.Select(n => BadgeBox.GetInfo(n)).ToList();
            }
        }

        #endregion
    }
}
