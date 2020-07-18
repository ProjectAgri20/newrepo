using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DocumentLibrary;
using HP.ScalableTest.Core.EnterpriseTest;
using HP.ScalableTest.Core.Plugin;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.Configuration
{
    /// <summary>
    /// Loads STF environment settings from the Enterprise Test database.
    /// </summary>
    public static class SettingsLoader
    {
        /// <summary>
        /// Retrieves the specified system setting.
        /// </summary>
        /// <param name="setting">The name of the setting to retrieve.</param>
        /// <returns>The setting value, or null if no such setting is found.</returns>
        public static string RetrieveSetting(string setting)
        {
            using (EnterpriseTestContext context = DbConnect.EnterpriseTestContext())
            {
                return context.SystemSettings.FirstOrDefault(n => n.Type == "SystemSetting" && n.Name == setting)?.Value;
            }
        }

        /// <summary>
        /// Retrieves the specified system setting as an object of type <typeparamref name="T" />.
        /// If the setting is not found, returns the default value for type <typeparamref name="T" />.
        /// </summary>
        /// <typeparam name="T">The type of data to return</typeparam>
        /// <param name="setting">The name of the setting to retrieve.</param>
        /// <returns>The setting value, or the default value for type <typeparamref name="T" /> if no such setting is found.</returns>
        /// <exception cref="FormatException">The data could not be converted to the specified type <typeparamref name="T" />.</exception>
        /// <exception cref="InvalidCastException">Conversion to the specified type <typeparamref name="T" /> is not supported.</exception>
        public static T RetrieveSetting<T>(string setting) where T : struct
        {
            return RetrieveSetting(setting, default(T));
        }

        /// <summary>
        /// Retrieves the specified system setting as an object of type <typeparamref name="T" />.
        /// If the setting is not found, returns the specified default value.
        /// </summary>
        /// <typeparam name="T">The type of data to return</typeparam>
        /// <param name="setting">The name of the setting to retrieve.</param>
        /// <param name="defaultValue">The value to return if the specified setting is not found.</param>
        /// <returns>The setting value, or <paramref name="defaultValue" /> if no such setting is found.</returns>
        /// <exception cref="FormatException">The data could not be converted to the specified type <typeparamref name="T" />.</exception>
        /// <exception cref="InvalidCastException">Conversion to the specified type <typeparamref name="T" /> is not supported.</exception>
        public static T RetrieveSetting<T>(string setting, T defaultValue) where T : struct
        {
            string value = RetrieveSetting(setting);
            if (value != null)
            {
                return (T)Convert.ChangeType(value, typeof(T), CultureInfo.InvariantCulture);
            }
            else
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Loads the system settings from the database and populates the appropriate configuration objects with their values.
        /// </summary>
        /// <param name="databaseServer">The address of the server hosting the EnterpriseTest database.</param>
        public static void LoadSystemConfiguration(string databaseServer)
        {
            LogInfo($"Loading system configuration from {databaseServer}.");

            Dictionary<string, string> settings;
            EnterpriseTestConnectionString connectionString = new EnterpriseTestConnectionString(databaseServer);
            using (EnterpriseTestContext context = new EnterpriseTestContext(connectionString))
            {
                settings = context.SystemSettings.Where(n => n.Type == "SystemSetting").ToDictionary(n => n.Name, n => n.Value, StringComparer.OrdinalIgnoreCase);
            }

            InitializeDbConnect(settings);
            InitializePluginFactory(settings);
        }

        private static void InitializeDbConnect(Dictionary<string, string> settings)
        {
            DbConnect.AssetInventoryConnectionString = new AssetInventoryConnectionString(settings["AssetInventoryDatabase"]);
            DbConnect.DataLogConnectionString = new DataLogConnectionString(settings["DataLogDatabase"]);
            DbConnect.DocumentLibraryConnectionString = new DocumentLibraryConnectionString(settings["DocumentLibraryDatabase"]);
            DbConnect.EnterpriseTestConnectionString = new EnterpriseTestConnectionString(settings["EnterpriseTestDatabase"]);
        }

        private static void InitializePluginFactory(Dictionary<string, string> settings)
        {
            PluginFactory.Register(LoadPluginDefinitions());
            PluginFactory.PluginRelativePath = settings["PluginRelativeLocation"];
        }

        /// <summary>
        /// Loads the plugin definitions from the database.
        /// </summary>
        /// <returns>A <see cref="PluginDefinition" /> collection.</returns>
        public static IEnumerable<PluginDefinition> LoadPluginDefinitions()
        {
            using (EnterpriseTestContext context = DbConnect.EnterpriseTestContext())
            {
                var pluginSettings = context.SystemSettings.Where(n => n.Type == "PluginSetting");
                IEnumerable<PluginDefinition> definitions = context.MetadataTypes.Select(n => new
                {
                    n.Name,
                    n.AssemblyName,
                    n.Title,
                    Settings = pluginSettings.Where(m => m.SubType == n.Name).Select(m => new { m.Name, m.Value })
                })
                .AsEnumerable()
                .Select(n => new PluginDefinition
                (
                    n.Name,
                    n.AssemblyName,
                    n.Title,
                    n.Settings.ToDictionary(m => m.Name, m => m.Value)
                ));

                return definitions.ToList();
            }
        }
    }
}
