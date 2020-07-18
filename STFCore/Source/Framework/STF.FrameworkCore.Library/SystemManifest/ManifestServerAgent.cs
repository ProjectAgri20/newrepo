using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Assets;
using HP.ScalableTest.Framework.ClientController.ActivityExecution;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Framework
{
    public class ManifestServerAgent : IManifestComponentAgent
    {
        private readonly List<ServerInfo> _servers = new List<ServerInfo>();
        private readonly Dictionary<Guid, ServerIdCollection> _activityServers = new Dictionary<Guid, ServerIdCollection>();
        private readonly Dictionary<string, Dictionary<string, string>> _serverSettings = new Dictionary<string, Dictionary<string, string>>();

        /// <summary>
        /// Gets the Requested Assets.
        /// </summary>
        public IEnumerable<string> RequestedAssets
        {
            get { yield break; }
        }

        /// <summary>
        /// Gets the Server Names used in this scenario.
        /// </summary>
        public IEnumerable<string> ServerNames
        {
            get { return _servers.Select(s => s.HostName); }
        }

        /// <summary>
        /// Creates a new instance of the <see cref="ManifestServerAgent"/> class.
        /// </summary>
        /// <param name="scenarioId"></param>
        public ManifestServerAgent(Guid scenarioId)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                // Retrieve server usage data for all enabled activities in the specified session
                var activities = (from serverusage in context.VirtualResourceMetadataServerUsages
                                  let data = serverusage.ServerSelectionData
                                  let metadata = serverusage.VirtualResourceMetadata
                                  let resource = metadata.VirtualResource
                                  where resource.EnterpriseScenarioId == scenarioId
                                     && resource.Enabled == true
                                     && metadata.Enabled == true
                                     && data != null
                                  select new { Id = metadata.VirtualResourceMetadataId, Servers = data }).ToList();

                foreach (var activity in activities)
                {
                    ServerSelectionData serverSelectionData = GetSelectionData(activity.Servers);
                    _activityServers.Add(activity.Id, GetServerIds(serverSelectionData));
                }
            }

            var serverIds = _activityServers.Values.SelectMany(n => n).Distinct().ToList();
            using (AssetInventoryContext assetContext = DbConnect.AssetInventoryContext())
            {
                _servers.AddRange(assetContext.FrameworkServers.Where(n => serverIds.Contains(n.FrameworkServerId)).ToServerInfoCollection());
            }
        }

        private static ServerSelectionData GetSelectionData(string data)
        {
            return Serializer.Deserialize<ServerSelectionData>(XElement.Parse(data));
        }

        private static ServerIdCollection GetServerIds(ServerSelectionData selectionData)
        {
            return new ServerIdCollection(selectionData.SelectedServers);
        }

        private Dictionary<string, string> GetServerSettings(string serverName)
        {
            return _serverSettings.ContainsKey(serverName) ? _serverSettings[serverName] : null;
        }

        public void AssignManifestInfo(SystemManifest manifest)
        {
            manifest.Servers.Clear();
            manifest.ActivityServers.Clear();

            foreach (ServerInfo server in _servers)
            {
                manifest.Servers.Add(server);
            }

            foreach (var activityServer in _activityServers)
            {
                manifest.ActivityServers.Add(activityServer.Key, activityServer.Value);
            }
        }

        public void LogComponents(string sessionId)
        {
            foreach (ServerInfo server in _servers)
            {
                SessionServerLogger logger = new SessionServerLogger
                {
                    SessionServerId = SequentialGuid.NewGuid(),
                    SessionId = sessionId,
                    ServerId = server.ServerId,
                    HostName = server.HostName,
                    Address = server.Address,
                    OperatingSystem = server.OperatingSystem,
                    Architecture = server.Architecture,
                    Processors = (short)server.Processors,
                    Cores = (short)server.Cores,
                    Memory = server.Memory
                };
                ExecutionServices.DataLogger.AsInternal().SubmitAsync(logger);
            }
        }
    }
}
