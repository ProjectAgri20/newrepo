using System;
using System.Configuration;
using System.Linq;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.EnterpriseTest;

namespace HP.ScalableTest.Framework.Settings
{
    /// <summary>
    /// Settings that are stored globally for use across the STF libraries.
    /// </summary>
    public static class GlobalSettings
    {
        private static readonly FrameworkSettings _settings = new FrameworkSettings();
        private static readonly WcfHostSettings _wcfHosts = new WcfHostSettings();

        private static string _database;
        private static bool _autoRefresh = false;
        private static readonly TimeSpan _refreshInterval = TimeSpan.FromMinutes(10);

        private static bool _isDistributed = true;

        /// <summary>
        /// Gets the name of the database being used.
        /// </summary>
        public static string Database
        {
            get { return _database; }
        }

        /// <summary>
        /// Gets the settings.
        /// </summary>
        public static FrameworkSettings Items
        {
            get
            {
                CheckRefresh(_settings, "FrameworkSetting");
                return _settings;
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether the framework is running with distributed services or standalone (STFLite).
        /// </summary>
        /// <value><c>true</c> if this instance is remote; otherwise, <c>false</c>.</value>
        public static bool IsDistributedSystem
        {
            get { return _isDistributed; }
            set { _isDistributed = value; }
        }

        /// <summary>
        /// Gets the WCF hosts.
        /// </summary>
        public static WcfHostSettings WcfHosts
        {
            get
            {
                CheckRefresh(_wcfHosts, "WcfHostSetting");
                return _wcfHosts;
            }
        }

        /// <summary>
        /// Gets the environment this system is running on.
        /// If settings have not been loaded, returns "Unassigned".
        /// </summary>
        /// <value>The data environment, beta, development, production or unassigned.</value>
        public static DataEnvironment Environment
        {
            get
            {
                CheckRefresh(_settings, "FrameworkSetting");
                try
                {
                    return (DataEnvironment)Enum.Parse(typeof(DataEnvironment), _settings[Setting.Environment]);
                }
                catch (SettingNotFoundException)
                {
                    return DataEnvironment.Unassigned;
                }
            }
        }

        /// <summary>
        /// Refresh the settings
        /// </summary>
        public static void Refresh()
        {
            ReloadSettings(_settings, "FrameworkSetting");
            ReloadSettings(_wcfHosts, "WcfHostSetting");
        }

        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1031:DoNotCatchGeneralExceptionTypes",
            Justification = "This refresh should be passive and fail silently, rather than bringing down the hosting application.")]
        private static void CheckRefresh(SettingsCollectionBase settings, string settingSubType)
        {
            if (_autoRefresh && settings.IsExpired(_refreshInterval))
            {
                ReloadSettings(settings, settingSubType);
            }
        }

        private static void ReloadSettings(SettingsCollectionBase settings, string settingSubType)
        {
            try
            {
                LoadSettings(settings, settingSubType);
            }
            catch (Exception ex)
            {
                TraceFactory.Logger.Debug("Settings refresh attempt failed: " + ex.Message);
            }
        }


        /// <summary>
        /// Sets the IP address and service host for the dispatcher.
        /// </summary>
        /// <param name="address">The dispatcher address.</param>
        public static void SetDispatcher(string address)
        {
            // Add the dispatcher service settings
            _wcfHosts.Add(WcfService.SessionServer, address);
            _wcfHosts.Add(WcfService.SessionBackend, address);
            _wcfHosts.Add(WcfService.DataGateway, address);
        }

        /// <summary>
        /// Clears all settings.
        /// </summary>
        public static void Clear()
        {
            _settings.Clear();
            _wcfHosts.Clear();
        }

        /// <summary>
        /// Loads the settings from the specified database.
        /// </summary>
        /// <param name="database">The database.</param>
        /// <param name="autoRefresh">if set to <c>true</c> automatically refresh from the database.</param>
        public static void Load(string database, bool autoRefresh = false)
        {
            if (string.IsNullOrEmpty(database))
            {
                throw new ArgumentException("Database cannot be null or empty.", nameof(database));
            }

            TraceFactory.Logger.Debug("Loading system settings from: " + database);

            _database = database;
            _autoRefresh = autoRefresh;

            EntityFrameworkConfiguration.Initialize();
            SettingsLoader.LoadSystemConfiguration(_database);

            LoadSettings(_settings, "FrameworkSetting");
            LoadSettings(_wcfHosts, "WcfHostSetting");

            TraceFactory.Logger.Debug("System settings loaded.");
            TraceFactory.Logger.Debug("AutoRefresh: " + _autoRefresh);
        }

        private static void LoadSettings(SettingsCollectionBase settings, string settingSubType)
        {
            using (EnterpriseTestContext context = DbConnect.EnterpriseTestContext())
            {
                foreach (SystemSetting setting in context.SystemSettings.Where(n => n.Type == "SystemSetting" && n.SubType == settingSubType))
                {
                    settings.AddValue(setting.Name, setting.Value);
                }
            }
            settings.LoadTime = DateTime.Now;
        }

        /// <summary>
        /// Loads settings from App.Config.
        /// </summary>
        public static void Load()
        {
            foreach (string key in ConfigurationManager.AppSettings.AllKeys)
            {
                _settings.Add(key, ConfigurationManager.AppSettings[key]);
            }
        }
    }
}
