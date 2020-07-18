using System;
using System.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Core.Lock;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Framework.PluginService
{
    public static class FrameworkServicesInitializer
    {
        public static void InitializeConfiguration()
        {
            ConfigurationServices.AssetInventory = new AssetInventoryController(DbConnect.AssetInventoryConnectionString, BuildAssetInventoryConfig());
            ConfigurationServices.DocumentLibrary = new DocumentLibraryController(DbConnect.DocumentLibraryConnectionString);
            ConfigurationServices.EnvironmentConfiguration = new EnvironmentConfigurationAdapter();
            ConfigurationServices.SystemTrace = new SystemTraceLogger();
            Logger.Initialize(new SystemTraceLogger(typeof(Logger)));
        }

        public static void InitializeExecution()
        {
            ExecutionServices.CriticalSection = new CriticalSection(new DistributedLockManager(GlobalSettings.WcfHosts["Lock"]));
            ExecutionServices.DataLogger = new DataLogger(GlobalSettings.WcfHosts[WcfService.DataLog]);
            ExecutionServices.FileRepository = new DocumentFileRepository(GlobalSettings.Items[Setting.DocumentLibraryServer]);
            ExecutionServices.SessionRuntime = new SessionRuntimeAdapter();
            ExecutionServices.SystemTrace = new SystemTraceLogger();
            Logger.Initialize(new SystemTraceLogger(typeof(Logger)));
        }

        private static AssetInventoryConfiguration BuildAssetInventoryConfig()
        {
            AssetInventoryConfiguration configuration = new AssetInventoryConfiguration();

            if (GlobalSettings.Items != null && GlobalSettings.Items.ContainsKey(Setting.AssetInventoryPools))
            {
                foreach (string pool in GlobalSettings.Items[Setting.AssetInventoryPools].Split(",".ToArray(), StringSplitOptions.RemoveEmptyEntries))
                {
                    configuration.AssetPools.Add(pool);
                }
            }

            return configuration;
        }
    }
}
