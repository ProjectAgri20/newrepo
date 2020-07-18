using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Core.Plugin
{
    /// <summary>
    /// Contains configuration-driven metadata for a plugin registered in STF.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    [DataContract]
    public sealed class PluginDefinition
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _name;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _assemblyName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _title;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SettingsDictionary _pluginSettings;

        /// <summary>
        /// Gets the plugin name.  (This is also the metadata type for the plugin.)
        /// </summary>
        public string Name => _name;

        /// <summary>
        /// Gets the name of the plugin assembly.
        /// </summary>
        public string AssemblyName => _assemblyName;

        /// <summary>
        /// Gets the title to use when displaying plugin activities/metadata.
        /// </summary>
        public string Title => _title;

        /// <summary>
        /// Gets the plugin-specific settings.
        /// </summary>
        public SettingsDictionary PluginSettings => _pluginSettings;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginDefinition" /> class.
        /// </summary>
        /// <param name="name">The plugin name.</param>
        /// <param name="assemblyName">The name of the plugin assembly.</param>
        /// <param name="title">The plugin title.</param>
        /// <param name="pluginSettings">The plugin settings.</param>
        public PluginDefinition(string name, string assemblyName, string title, IDictionary<string, string> pluginSettings)
        {
            _name = name;
            _assemblyName = assemblyName;
            _title = title;
            _pluginSettings = new SettingsDictionary(pluginSettings);
        }
    }
}
