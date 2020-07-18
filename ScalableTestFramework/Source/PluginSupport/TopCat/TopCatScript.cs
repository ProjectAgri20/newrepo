using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.PluginSupport.TopCat
{
    /// <summary>
    /// Represents a TopCat script that can be run using <see cref="TopCatExecutionController" />.
    /// </summary>
    [DataContract]
    public class TopCatScript
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _fileName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _scriptName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Collection<string> _selectedTests = new Collection<string>();

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Dictionary<string, string> _properties = new Dictionary<string, string>();

        /// <summary>
        /// Gets the file name.
        /// </summary>
        public string FileName => _fileName;

        /// <summary>
        /// Gets the script name.
        /// </summary>
        public string ScriptName => _scriptName;

        /// <summary>
        /// Gets the selected tests.
        /// </summary>
        public Collection<string> SelectedTests => _selectedTests;

        /// <summary>
        /// Gets the script properties.
        /// </summary>
        public Dictionary<string, string> Properties => _properties;

        /// <summary>
        /// Initializes a new instance of the <see cref="TopCatScript" /> class.
        /// </summary>
        /// <param name="fileName">The file name.</param>
        /// <param name="scriptName">The script name.</param>
        public TopCatScript(string fileName, string scriptName)
        {
            _scriptName = scriptName;
            _fileName = fileName;
        }
    }
}
