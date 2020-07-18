using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// A read-only dictionary of <see cref="PluginRetrySetting" /> definitions.
    /// </summary>
    [DataContract]
    public sealed class PluginRetrySettingDictionary : ReadOnlyDictionary<PluginResult, PluginRetrySetting>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="PluginRetrySettingDictionary" /> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <exception cref="ArgumentNullException"><paramref name="settings" /> is null.</exception>
        public PluginRetrySettingDictionary(IEnumerable<PluginRetrySetting> settings)
            : base(BuildDictionary(settings))
        {
        }

        private static IDictionary<PluginResult, PluginRetrySetting> BuildDictionary(IEnumerable<PluginRetrySetting> settings)
        {
            // Pre-populate the dictionary with default values for all plugin results.
            var dictionary = new Dictionary<PluginResult, PluginRetrySetting>();
            foreach (PluginResult result in Enum.GetValues(typeof(PluginResult)))
            {
                dictionary[result] = new PluginRetrySetting(result, PluginRetryAction.Continue, 1, TimeSpan.Zero, PluginRetryAction.Continue);
            }

            // Set the values based on the settings provided.
            foreach (PluginRetrySetting setting in settings)
            {
                dictionary[setting.State] = setting;
            }

            return dictionary;
        }
    }
}
