using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Net;
using System.Xml.Linq;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;

namespace HP.ScalableTest.Framework.Plugin
{
    /// <summary>
    /// Execution data provided to an <see cref="IPluginExecutionEngine" />.
    /// </summary>
    public sealed class PluginExecutionData : PluginData
    {
        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly AssetInfoCollection _assets;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly DocumentCollection _documents;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ServerInfoCollection _servers;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PrintQueueInfoCollection _printQueues;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PluginEnvironment _environment;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PluginExecutionContext _context;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly Lazy<NetworkCredential> _credential;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly ExternalCredentialInfoCollection _externalCredentials;

        [DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly PluginRetrySettingDictionary _retrySettings;

        /// <summary>
        /// Gets an <see cref="AssetInfoCollection" /> with all assets available for this plugin execution.
        /// </summary>
        public AssetInfoCollection Assets => _assets;

        /// <summary>
        /// Gets a <see cref="DocumentCollection" /> with all documents available for this plugin execution.
        /// </summary>
        public DocumentCollection Documents => _documents;

        /// <summary>
        /// Gets a <see cref="ServerInfoCollection" /> with all servers available for this plugin execution.
        /// </summary>
        public ServerInfoCollection Servers => _servers;

        /// <summary>
        /// Gets a <see cref="PrintQueueInfoCollection" /> with all print queues available for this plugin execution.
        /// </summary>
        public PrintQueueInfoCollection PrintQueues => _printQueues;

        /// <summary>
        /// Gets a <see cref="PluginEnvironment" /> object with information about the execution environment.
        /// </summary>
        public PluginEnvironment Environment => _environment;

        /// <summary>
        /// Gets a <see cref="ExternalCredentialInfoCollection" /> with all external credentials for this plugin execution.
        /// </summary>
        public ExternalCredentialInfoCollection ExternalCredentials => _externalCredentials;

        /// <summary>
        /// Gets a <see cref="PluginRetrySettingDictionary" /> with retry configuration for this plugin execution.
        /// </summary>
        public PluginRetrySettingDictionary RetrySettings => _retrySettings;

        /// <summary>
        /// Provides user credentials for use within the associated automation plugin.
        /// </summary>
        /// <remarks>
        /// These credentials are assigned to this configuration by the core framework, 
        /// and should be referenced whenever the plugin needs to communicate
        /// and authenticate with an outside component or service.
        /// </remarks>
        public NetworkCredential Credential => _credential.Value;

        /// <summary>
        /// Gets the unique identifier for the current activity execution.
        /// </summary>
        public Guid ActivityExecutionId => _context.ActivityExecutionId;

        /// <summary>
        /// Gets the session ID associated with execution of this plugin.
        /// </summary>
        public string SessionId => _context.SessionId;

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginExecutionData" /> class.
        /// </summary>
        /// <param name="metadata">An <see cref="XElement" /> containing plugin-specific XML metadata.</param>
        /// <param name="metadataVersion">The plugin-defined schema version of the XML metadata.</param>
        /// <param name="assets">The assets available for this plugin execution.</param>
        /// <param name="documents">The documents available for this plugin execution.</param>
        /// <param name="servers">The servers available for this plugin execution.</param>
        /// <param name="printQueues">The print queues available for this plugin execution.</param>
        /// <param name="environment">Information about the plugin execution environment.</param>
        /// <param name="context">Contextual/environmental information about this plugin execution.</param>
        /// <param name="retrySettings">Retry configuration for this plugin execution.</param>
        /// <exception cref="ArgumentNullException"><paramref name="metadata" /> is null.</exception>
        public PluginExecutionData(XElement metadata, string metadataVersion, AssetInfoCollection assets, DocumentCollection documents, ServerInfoCollection servers, PrintQueueInfoCollection printQueues,
                                   PluginEnvironment environment, PluginExecutionContext context, PluginRetrySettingDictionary retrySettings)
            : this(metadata, metadataVersion, assets, documents, servers, printQueues, environment, context, retrySettings, new ExternalCredentialInfoCollection(new List<ExternalCredentialInfo>()))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="PluginExecutionData" /> class.
        /// </summary>
        /// <param name="metadata">An <see cref="XElement" /> containing plugin-specific XML metadata.</param>
        /// <param name="metadataVersion">The plugin-defined schema version of the XML metadata.</param>
        /// <param name="assets">The assets available for this plugin execution.</param>
        /// <param name="documents">The documents available for this plugin execution.</param>
        /// <param name="servers">The servers available for this plugin execution.</param>
        /// <param name="printQueues">The print queues available for this plugin execution.</param>
        /// <param name="environment">Information about the plugin execution environment.</param>
        /// <param name="context">Contextual/environmental information about this plugin execution.</param>
        /// <param name="retrySettings">Retry configuration for this plugin execution.</param>
        /// <param name="externalCredentials">External credentials used by this plugin execution.</param>
        /// <exception cref="ArgumentNullException"><paramref name="metadata" /> is null.</exception>
        public PluginExecutionData(XElement metadata, string metadataVersion, AssetInfoCollection assets, DocumentCollection documents, ServerInfoCollection servers, PrintQueueInfoCollection printQueues,
                           PluginEnvironment environment, PluginExecutionContext context, PluginRetrySettingDictionary retrySettings, ExternalCredentialInfoCollection externalCredentials)
            : base(metadata, metadataVersion)
        {
            _assets = assets;
            _documents = documents;
            _servers = servers;
            _printQueues = printQueues;
            _environment = environment;
            _context = context;
            _credential = new Lazy<NetworkCredential>(CreateCredential);
            _retrySettings = retrySettings;
            _externalCredentials = externalCredentials;
        }

        private NetworkCredential CreateCredential()
        {
            return new NetworkCredential(_context.UserName, _context.UserPassword, _context.UserDomain ?? _environment.UserDomain);
        }
    }
}
