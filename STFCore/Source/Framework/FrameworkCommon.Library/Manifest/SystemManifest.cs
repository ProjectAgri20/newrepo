using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Xml;
using System.Xml.Linq;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.Plugin;
using HP.ScalableTest.Core.Security;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.Documents;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Xml;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Defines the overall system manifest used to describe all configuration data
    /// used by automation components in a test scenario.
    /// </summary>
    [DataContract]
    public class SystemManifest
    {
        /// <summary>
        /// Gets or sets the dispatcher.
        /// </summary>
        [DataMember]
        public string Dispatcher { get; set; }

        /// <summary>
        /// Gets the settings defined for this manifest.
        /// </summary>
        [DataMember]
        public FrameworkSettings Settings { get; private set; }

        /// <summary>
        /// Gets the WCF hosts for this manifest.
        /// </summary>
        [DataMember]
        public WcfHostSettings WcfHosts { get; private set; }

        /// <summary>
        /// Gets the plugin definitions.
        /// </summary>
        [DataMember]
        public Collection<PluginDefinition> PluginDefinitions { get; private set; }

        /// <summary>
        /// Gets the resources used in this test scenario
        /// </summary>
        [DataMember]
        public ResourceDetailCollection Resources { get; private set; }

        /// <summary>
        /// Assets used by the scenario
        /// </summary>
        [DataMember]
        public Collection<AssetInfo> AllAssets { get; private set; }

        /// <summary>
        /// Assets used by the activities
        /// </summary>
        [DataMember]
        public Dictionary<Guid, AssetIdCollection> ActivityAssets { get; private set; }

        /// <summary>
        /// List of documents used by the scenario
        /// </summary>
        [DataMember]
        public Collection<Document> Documents { get; private set; }

        /// <summary>
        /// List of documents used by each activity
        /// </summary>
        [DataMember]
        public Dictionary<Guid, DocumentIdCollection> ActivityDocuments { get; private set; }

        /// <summary>
        /// Retry Settings for each activity
        /// </summary>
        [DataMember]
        public Dictionary<Guid, Plugin.PluginRetrySettingDictionary> ActivityRetrySettings { get; private set; }

        /// <summary>
        /// List of framework servers
        /// </summary>
        [DataMember]
        public Collection<ServerInfo> Servers { get; private set; }

        /// <summary>
        /// list of framework servers used by activities
        /// </summary>
        [DataMember]
        public Dictionary<Guid, ServerIdCollection> ActivityServers { get; private set; }

        /// <summary>
        /// List of print queues used by activity
        /// </summary>
        [DataMember]
        public Dictionary<Guid, PrintQueueInfoCollection> ActivityPrintQueues { get; private set; }

        /// <summary>
        /// Gets the physical assets used in this test scenario.
        /// </summary>
        [DataMember]
        public AvailableDeviceSet<AssetDetail> Assets { get; private set; }

        /// <summary>
        /// Gets the software installers needed to be run on the clients before activity execution.
        /// </summary>
        [DataMember]
        public Collection<SoftwareInstallerDetail> SoftwareInstallers { get; private set; }

        /// <summary>
        /// Gets or sets the system platform this manifest will use.
        /// </summary>
        [DataMember]
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the host machine this manifest will use.
        /// </summary>
        [DataMember]
        public string HostMachine { get; set; }

        /// <summary>
        /// Gets or sets the session id used by this manifest during testing.
        /// </summary>
        [DataMember]
        public string SessionId { get; set; }

        /// <summary>
        /// Gets or sets the type of the resource the manifest represents.
        /// </summary>
        [DataMember]
        public VirtualResourceType ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the session owner (username).
        /// </summary>
        [DataMember]
        public UserCredential SessionOwner { get; set; }

        /// <summary>
        /// Tells VM to Collect Event Logs.
        /// </summary>
        [DataMember]
        public bool CollectEventLogs { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemManifest"/> class.
        /// </summary>
        public SystemManifest()
        {
            Settings = new FrameworkSettings();
            WcfHosts = new WcfHostSettings();
            PluginDefinitions = new Collection<PluginDefinition>();
            Resources = new ResourceDetailCollection();
            AllAssets = new Collection<AssetInfo>();
            ActivityAssets = new Dictionary<Guid, AssetIdCollection>();
            Documents = new Collection<Document>();
            ActivityDocuments = new Dictionary<Guid, DocumentIdCollection>();
            Servers = new Collection<ServerInfo>();
            ActivityServers = new Dictionary<Guid, ServerIdCollection>();
            ActivityPrintQueues = new Dictionary<Guid, PrintQueueInfoCollection>();
            Assets = new AvailableDeviceSet<AssetDetail>();
            SoftwareInstallers = new Collection<SoftwareInstallerDetail>();
            ActivityRetrySettings = new Dictionary<Guid, Plugin.PluginRetrySettingDictionary>();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemManifest"/> class.
        /// </summary>
        /// <param name="dispatcherHostName">Name of the dispatcher host.</param>
        public SystemManifest(string dispatcherHostName)
            : this()
        {
            Dispatcher = dispatcherHostName;
        }

        /// <summary>
        /// Serializes this instance.
        /// </summary>
        /// <returns>A serialized version of this manifest.</returns>
        public string Serialize()
        {
            // Convert to a base 64 string to prevent WCF transmission problems
            string xml = Serializer.Serialize(this).ToString();
            byte[] bytes = Encoding.UTF8.GetBytes(xml);
            return Convert.ToBase64String(bytes);
        }

        /// <summary>
        /// Deserializes the specified manifest data.
        /// </summary>
        /// <param name="manifest">The serialized manifest data.</param>
        /// <returns>A deserialized manifest.</returns>
        public static SystemManifest Deserialize(string manifest)
        {
            byte[] bytes = Convert.FromBase64String(manifest);
            string xml = Encoding.UTF8.GetString(bytes);
            return Serializer.Deserialize<SystemManifest>(XElement.Parse(xml));
        }

        /// <summary>
        /// Pulls the manifest from Global Data Store
        /// </summary>
        public void PullFromGlobalSettings()
        {
            foreach (string item in GlobalSettings.Items.Keys)
            {
                this.Settings.Add(item, GlobalSettings.Items[item]);
            }

            foreach (string service in GlobalSettings.WcfHosts.Keys)
            {
                this.WcfHosts.Add(service, GlobalSettings.WcfHosts[service]);
            }
        }

        /// <summary>
        /// Pushes this manifest to the <see cref="GlobalDataStore"/>.
        /// </summary>
        public void PushToGlobalDataStore()
        {
            GlobalDataStore.Load(this);
        }

        /// <summary>
        /// Pushes this manifest to the <see cref="GlobalDataStore"/>.
        /// </summary>
        /// <param name="resourceInstanceId"></param>
        public void PushToGlobalDataStore(string resourceInstanceId)
        {
            GlobalDataStore.Load(this, resourceInstanceId);
        }

        /// <summary>
        /// Pushes the settings portion of this manifest to the <see cref="GlobalSettings"/>.
        /// </summary>
        public void PushToGlobalSettings()
        {
            GlobalSettings.Clear();

            foreach (string item in Settings.Keys)
            {
                GlobalSettings.Items.Add(item, Settings[item]);
            }

            foreach (string service in WcfHosts.Keys)
            {
                GlobalSettings.WcfHosts.Add(service, WcfHosts[service]);
            }

            PluginFactory.PluginRelativePath = GlobalSettings.Items[Setting.PluginRelativeLocation];
            PluginFactory.Register(PluginDefinitions);
        }

        /// <summary>
        /// Gets the external credentials for the specified domain user
        /// </summary>
        public List<ExternalCredentialInfo> GetExternalCredentials(OfficeWorkerCredential domainCredential)
        {
            List<ExternalCredentialInfo> result = new List<ExternalCredentialInfo>();
            foreach (ExternalCredentialDetail detail in GlobalDataStore.Manifest.Resources.ExternalCredentials.Where(x => x.DomainUserName.Equals(domainCredential.UserName, StringComparison.InvariantCultureIgnoreCase)))
            {
                result.Add(detail.ToExternalCredentialInfo());
            }
            return result;
        }

        /// <summary>
        /// Returns a <see cref="System.String" /> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String" /> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            // Load the document and replace the Activities, which are escaped
            // XML with actual XML.  This will make it display much better.
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(Serializer.Serialize(this).ToString());

            XmlNamespaceManager manager = new XmlNamespaceManager(doc.NameTable);
            manager.AddNamespace("ns", "http://schemas.datacontract.org/2004/07/HP.ScalableTest.Framework.Manifest");

            foreach (XmlNode node in doc.DocumentElement.SelectNodes("//ns:ResourceMetadataDetail/ns:Data", manager))
            {
                var newNode = XmlUtil.CreateNode(doc, node.InnerText);
                node.RemoveAll();
                node.AppendChild((XmlNode)newNode);
            }

            return XmlUtil.CreateXDocument(doc).ToString();
        }

        /// <summary>
        /// Returns the <see cref="ResourceDetailBase"/> class type for the give resource type.
        /// </summary>
        /// <param name="type">The virtual resource type.</param>
        /// <returns>The <see cref="System.Type"/> value of the corresponding detail class.</returns>
        public static Type DetailType(VirtualResourceType type)
        {
            var detailTypeName = "{0}.{1}Detail".FormatWith(typeof(SystemManifest).Namespace, type);
            return Type.GetType(detailTypeName);
        }
    }
}