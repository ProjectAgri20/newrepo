using System.Diagnostics;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Information about the environment in which a plugin is configured and executed.
    /// </summary>
    public sealed class PluginEnvironment
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly SettingsDictionary _pluginSettings;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _userDomain;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _userDnsDomain;

        /// <summary>
        /// Plugin-specific settings configured for the current environment.
        /// </summary>
        public SettingsDictionary PluginSettings => _pluginSettings;

        /// <summary>
        /// The user domain for the execution environment.
        /// </summary>
        public string UserDomain => _userDomain;

        /// <summary>
        /// The user DNS domain for the execution environment.
        /// </summary>
        public string UserDnsDomain => _userDnsDomain;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginEnvironment" /> class.
        /// </summary>
        /// <param name="pluginSettings">The plugin settings.</param>
        /// <param name="userDomain">The user domain.</param>
        /// <param name="userDnsDomain">The user DNS domain.</param>
        public PluginEnvironment(SettingsDictionary pluginSettings, string userDomain, string userDnsDomain)
        {
            _pluginSettings = pluginSettings;
            _userDomain = userDomain;
            _userDnsDomain = userDnsDomain;
        }
    }
}
