using System.Diagnostics;
using System.IO;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a print driver configuration.
    /// </summary>
    [DataContract]
    public class PrintDriverConfiguration
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _configurationFile;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _defaultShortcut;

        /// <summary>
        /// Gets the configuration file path.
        /// </summary>
        public string ConfigurationFile => _configurationFile;

        /// <summary>
        /// Gets the default shortcut.
        /// </summary>
        public string DefaultShortcut => _defaultShortcut;

        /// <summary>
        /// Gets the file name of the configuration file.
        /// </summary>
        public string FileName => Path.GetFileNameWithoutExtension(_configurationFile);

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDriverConfiguration" /> class.
        /// </summary>
        public PrintDriverConfiguration()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDriverConfiguration"/> class.
        /// </summary>
        /// <param name="configurationFile">The configuration file.</param>
        /// <param name="defaultShortcut">The default shortcut.</param>
        public PrintDriverConfiguration(string configurationFile, string defaultShortcut)
        {
            _configurationFile = configurationFile;
            _defaultShortcut = defaultShortcut;
        }
    }
}
