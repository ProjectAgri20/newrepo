using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Core.DataLog.Model;
using HP.ScalableTest.Data.EnterpriseTest;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework.Manifest;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Framework.Dispatcher
{
    /// <summary>
    /// Class used to build a <see cref="SystemManifest"/> based on the defined <see cref="SessionTicket"/>.
    /// </summary>
    public class SystemManifestAgent : IDisposable
    {
        private static int _globalCounter = 0;
        private readonly List<IManifestComponentAgent> _agents;

        internal DataModelCache DataCache { get; private set; }

        /// <summary>
        /// Gets the quantities for items used by resources (user accounts, IP addresses, etc).
        /// </summary>
        public SessionResourceQuantity Quantities { get; private set; }

        /// <summary>
        /// Gets the assets used in this session.
        /// </summary>
        public List<AssetDetail> Assets { get; private set; }

        /// <summary>
        /// Gets the ticket for this session.
        /// </summary>
        public SessionTicket Ticket { get; private set; }

        /// <summary>
        /// Gets the ScenarioId used to build this manifest.
        /// </summary>
        public Guid ScenarioId { get; private set; }

        /// <summary>
        /// Gets all reserved user accounts for this session.
        /// </summary>
        public DomainAccountReservationSet UserAccounts { get; private set; }

        /// <summary>
        /// Gets all the virtual resources being used in this session.
        /// </summary>
        public IEnumerable<VirtualResource> Resources { get; private set; }

        /// <summary>
        /// Gets all the Server Names being used in this session.
        /// </summary>
        public IEnumerable<string> ServerHostNames
        {
            get
            {
                ManifestServerAgent serverAgent = (ManifestServerAgent)_agents.Where(a => a.GetType() == typeof(ManifestServerAgent)).FirstOrDefault();
                return serverAgent.ServerNames ?? Enumerable.Empty<string>();
            }
        }

        /// <summary>
        /// Gets the <see cref="SystemManifestCollection"/> that represents all manifests build for this scenario.
        /// </summary>
        public SystemManifestCollection ManifestSet { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SystemManifestAgent" /> class.
        /// </summary>
        /// <param name="ticket">The session configuration.</param>
        /// <param name="scenarioId">The scenarioId from which to build the manifest(s).</param>
        public SystemManifestAgent(SessionTicket ticket, Guid scenarioId)
        {
            Ticket = ticket;
            ScenarioId = scenarioId;
            Assets = new List<AssetDetail>();
            DataCache = new DataModelCache();
            Quantities = new SessionResourceQuantity();
            ManifestSet = new SystemManifestCollection();
            _agents = new List<IManifestComponentAgent>()
            {
                new ManifestAssetAgent(ScenarioId),
                new ManifestDocumentAgent(ScenarioId),
                new ManifestServerAgent(ScenarioId),
                new ManifestPrintQueueAgent(ScenarioId),
                new ManifestRetrySettingsAgent(ScenarioId),
                new ManifestSettingsAgent(),
                new ManifestPluginAssembliesAgent()
            };
            Initialize();
        }

        /// <summary>
        /// The requested AssetIds.
        /// </summary>
        public IEnumerable<string> RequestedAssets
        {
            get { return _agents.SelectMany(n => n.RequestedAssets).Distinct(); }
        }

        /// <summary>
        /// Log the individual components for all manifest agents.
        /// </summary>
        public void LogSessionComponents()
        {
            _agents.ForEach(n => n.LogComponents(Ticket.SessionId));
        }

        /// <summary>
        /// Log the products associated with this scenario.
        /// </summary>
        public void LogAssociatedProducts()
        {
            using (var enterpriseTestContext = new EnterpriseTestContext())
            {
                using (var dataLogContext = DbConnect.DataLogContext())
                {
                    TraceFactory.Logger.Debug($"Inserting {Ticket.AssociatedProductList.Count} products used in scenario.");
                    foreach (var ap in Ticket.AssociatedProductList)
                    {
                        if (ap.Active)
                        {
                            if (string.IsNullOrWhiteSpace(ap.Version))
                            {
                                TraceFactory.Logger.Debug($"Skipping insert of version information for {ap.Vendor} {ap.Name}.  No version was provided.");
                            }
                            else
                            {
                                string msg = $"associated product version '{ap.Vendor} {ap.Name} {ap.Version}' for session {Ticket.SessionId}...(AssociatedProductId: {ap.AssociatedProductId})";

                                if (dataLogContext.DbSessionProducts.Any(rec => rec.EnterpriseTestAssociatedProductId == (Guid)ap.AssociatedProductId && rec.SessionId == Ticket.SessionId))
                                {
                                    TraceFactory.Logger.Debug($"Skipping...User entered information twice for {msg}");
                                }
                                else
                                {
                                    try
                                    {
                                        TraceFactory.Logger.Debug($"Adding {msg}");
                                        TraceFactory.Logger.Debug($"Session:{Ticket.SessionId}, {ap.AssociatedProductId}, {ap.Version}, {ap.Name}, {ap.Vendor}");
                                        SessionProduct sessionProduct = new SessionProduct
                                        {
                                            SessionId = Ticket.SessionId,
                                            EnterpriseTestAssociatedProductId = ap.AssociatedProductId,
                                            Version = ap.Version,
                                            Name = ap.Name,
                                            Vendor = ap.Vendor
                                        };
                                        dataLogContext.DbSessionProducts.Add(sessionProduct);
                                        int rowCount = dataLogContext.SaveChanges();
                                        TraceFactory.Logger.Debug($"Database reports {rowCount} records inserted.");
                                    }
                                    catch (Exception x)
                                    {
                                        string msg2 = $"Could not add {msg}";
                                        TraceFactory.Logger.Debug(msg2);
                                        throw new DataException(msg2, x);
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        /// <summary>
        /// Builds all <see cref="SystemManifest"/>s for the defined <see cref="SessionTicket"/>.
        /// </summary>
        /// <param name="mockReserve">if set to <c>true</c> build dummy manifests, without any real reservations.  Otherwise, build the real manifests.</param>
        public void BuildManifests(bool mockReserve = false)
        {
            Dictionary<Guid, DetailBuilderBase> detailBuilders = new Dictionary<Guid, DetailBuilderBase>();
            VirtualResourcePacker packer = new VirtualResourcePacker(Resources);

            foreach (var packedResourceSet in packer.PackedSets)
            {
                // Create the custom resource builder
                var builder = VirtualResourceDetailBuilder.CreateDetailBuilder(packedResourceSet.ResourceType, this, packer);
                detailBuilders.Add(packedResourceSet.Id, builder);
            }

            string machineName = Environment.MachineName;

            // Get the user accounts needed for this session.  If reserveItems is false
            // then there won't be actual accounts reserved in the database.
            UserAccounts = GetReservedAccounts(mockReserve);

            // Create a manifest for each packed set
            foreach (var packedResourceSet in packer.PackedSets)
            {
                SystemManifest manifest = new SystemManifest(machineName);

                manifest.ResourceType = packedResourceSet.ResourceType;
                TraceFactory.Logger.Debug("Manifest.ResourceType: {0}".FormatWith(manifest.ResourceType));

                manifest.Platform = packedResourceSet.Platform;
                TraceFactory.Logger.Debug("Manifest.Platform: {0}".FormatWith(manifest.Platform));

                manifest.SessionId = Ticket.SessionId;
                TraceFactory.Logger.Debug("Manifest.SessionId: {0}".FormatWith(manifest.SessionId));

                manifest.SessionOwner = Ticket.SessionOwner;
                TraceFactory.Logger.Debug("Manifest.SessionOwner: {0}".FormatWith((manifest.SessionOwner != null ? manifest.SessionOwner.UserName : "null")));

                manifest.CollectEventLogs = Ticket.CollectEventLogs;
                TraceFactory.Logger.Debug("Manifest.CollectEventLogs: {0}".FormatWith(manifest.CollectEventLogs));

                TraceFactory.Logger.Debug("Processing Packed Set, Platform: {0}, Type: {1}".FormatWith(packedResourceSet.Platform, packedResourceSet.ResourceType));

                //DEBUG
                TraceFactory.Logger.Debug($"User Pool Count: {UserAccounts.UserPools.Count}");

                // Add all assets being used in this resource to the manifest
                foreach (AssetDetail asset in Assets)
                {
                    manifest.Assets.Add(asset);
                }

                // Have all agents populate their respective portions of the manifest
                foreach (IManifestComponentAgent agent in _agents)
                {
                    agent.AssignManifestInfo(manifest);
                }

                // Add software installers being used by this resource into the manifest
                var metadataTypes = packedResourceSet.SelectMany(x => x.VirtualResourceMetadataSet).Where(x => x.Enabled).Select(x => x.MetadataType).Distinct();
                foreach (SoftwareInstallerDetail installer in DataCache.GetSoftwareInstallerDetails(manifest.ResourceType, metadataTypes))
                {
                    TraceFactory.Logger.Debug("Adding {0}".FormatWith(installer.Description));
                    manifest.SoftwareInstallers.Add(installer);
                }

                // Now call the resource specific detail builder to add any additional information specific to this resource type
                detailBuilders[packedResourceSet.Id].AddToManifest(packedResourceSet, manifest);

                ManifestSet.Add(manifest);
            }
           
        }

        /// <summary>
        /// Loads all data from the database required to build the manifests for this Scenario so that
        /// properties like Resources and Quantities can be evaluated before the actual building of the manifests.
        /// </summary>
        private void Initialize()
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                TraceFactory.Logger.Debug($"Loading all resources for scenarioId: {ScenarioId}");

                // Get the enabled VirtualResources but filter out any disabled items from the VirtualResourceMetadataSet
                var resources = EnterpriseScenario.SelectWithAllChildren(context, ScenarioId)?.VirtualResources.Where(e => e.Enabled);
                if (resources == null || !resources.Any())
		        {
                    //No reason to continue if there are no resources
		            return;
		        }
		
                foreach (var resource in resources)
                {
                    var temp = resource.VirtualResourceMetadataSet.Where(x => x.Enabled).ToList();
                    resource.VirtualResourceMetadataSet.Clear();
                    temp.ForEach(resource.VirtualResourceMetadataSet.Add);
                }
                Resources = resources;

                ManifestSet.ScenarioId = ScenarioId;
                ManifestSet.ScenarioName = EnterpriseScenario.Select(context, ScenarioId).Name;

                Quantities = new SessionResourceQuantity(Resources);
            }
        }

        private DomainAccountReservationSet GetReservedAccounts(bool mockReserve = false)
        {
            DomainAccountReservationSet result = new DomainAccountReservationSet(Ticket.SessionId);

            using (AssetInventoryContext context = DbConnect.AssetInventoryContext())
            {
                if (mockReserve)
                {
                    //Build a mock reservation
                    foreach (var poolType in Quantities.DomainAccountQuantity.Keys)
                    {
                        DomainAccountPool pool = DomainAccountService.SelectPool(context, poolType);
                        result.Add(poolType, pool, 0, Quantities.DomainAccountQuantity[poolType]);
                    }
                }
                else
                {
                    foreach (DomainAccountReservation reservation in DomainAccountService.SelectReservationsBySession(context, Ticket.SessionId))
                    {
                        DomainAccountPool pool = DomainAccountService.SelectPool(context, reservation.DomainAccountKey);
                        result.Add(reservation.DomainAccountKey, pool, reservation.StartIndex, reservation.Count);
                    }
                }
            }

            return result;
        }

        /// <summary>
        /// Return a unique Id for the passed-in id string.
        /// </summary>
        /// <param name="id">The Id.</param>
        /// <returns>A unique Id.</returns>
        public static string CreateUniqueId(string id)
        {
            return "{0}-{1}".FormatWith(id, Interlocked.Increment(ref _globalCounter));
        }

        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Releases unmanaged and - optionally - managed resources
        /// </summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                if (DataCache != null)
                {
                    DataCache.Dispose();
                    DataCache = null;
                }
            }
        }

        #endregion IDisposable Members
    }

    /// <summary>
    /// Helper class used to manage multiple <see cref="SystemManifestAgent"/>s.
    /// </summary>
    public class SystemManifestAgentDictionary : Dictionary<Guid, SystemManifestAgent>
    {
        /// <summary>
        /// Gets the domain account quantities for all members of the dictionary.
        /// </summary>
        public DomainAccountQuantityDictionary DomainAccountQuantities
        {
            get
            {
                DomainAccountQuantityDictionary result = new DomainAccountQuantityDictionary();

                foreach (Guid key in Keys)
                {
                    SystemManifestAgent agent = this[key];
                    result.Add(agent.Quantities.DomainAccountQuantity);
                }
                return result;
            }
        }
    }
}
