using System;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Settings
{
    /// <summary>
    /// A collection of settings in key/value format.
    /// </summary>
    [DataContract]
    public abstract class SettingsCollectionBase
    {
        [DataMember]
        private Dictionary<string, string> _settings = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

        /// <summary>
        /// Gets the number of items stored in this instance.
        /// </summary>
        public int Count
        {
            get { return _settings.Count; }
        }

        /// <summary>
        /// Removes all items from this instance.
        /// </summary>
        public void Clear()
        {
            _settings.Clear();
        }

        /// <summary>
        /// Determines whether the specified key exists within the collection.
        /// </summary>
        /// <param name="setting">The setting name (key)</param>
        /// <returns></returns>
        public bool ContainsKey(Setting setting)
        {
            return _settings.ContainsKey(setting.ToString());
        }

        /// <summary>
        /// Determines whether the specified key name exists within the collection.
        /// </summary>
        /// <param name="settingName">The setting name (key)</param>
        /// <returns></returns>
        public bool ContainsKey(string settingName)
        {
            return _settings.ContainsKey(settingName);
        }

        /// <summary>
        /// Gets or sets the time when the settings were loaded.
        /// </summary>
        internal DateTime LoadTime { get; set; }

        /// <summary>
        /// Adds the specified setting.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <param name="value">The value.</param>
        protected internal void AddValue(string key, string value)
        {
            if (_settings.ContainsKey(key))
            {
                if (_settings[key] != value)
                {
                    TraceFactory.Logger.Debug("Modified setting {0} from '{1}' to '{2}'.".FormatWith(key, _settings[key], value));
                }
                _settings[key] = value;
            }
            else
            {
                _settings.Add(key, value);
            }
        }

        /// <summary>
        /// Gets the value of the setting with the specified key.
        /// </summary>
        /// <param name="key">The key.</param>
        /// <returns></returns>
        protected string GetSetting(string key)
        {
            if (_settings.ContainsKey(key))
            {
                return _settings[key];
            }
            else
            {
                throw new SettingNotFoundException("Could not find setting {0}. Settings searched: {1}.".FormatWith(key, this.Count));
            }
        }

        /// <summary>
        /// Gets the setting keys.
        /// </summary>
        internal IEnumerable<string> Keys
        {
            get { return _settings.Keys; }
        }

        /// <summary>
        /// Determines whether the settings are older than the specified timeout.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        /// <returns></returns>
        internal bool IsExpired(TimeSpan timeout)
        {
            return DateTime.Now > LoadTime + timeout;
        }
    }
}
