using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Runtime.InteropServices;
using HP.ScalableTest.Core.AssetInventory;
using HP.ScalableTest.Core.Configuration;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;

namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// UI Controller class supporting the <see cref="FrameworkServer"/> entity.
    /// </summary>
    public class FrameworkServerController : IDisposable
    {
        private AssetInventoryContext _context = null;
        private SortableBindingList<FrameworkServer> _servers = new SortableBindingList<FrameworkServer>();
        private Collection<FrameworkServer> _deletedItems = new Collection<FrameworkServer>();

        /// <summary>
        /// Initializes a new instance of the <see cref="FrameworkServerController"/> class.
        /// </summary>
        public FrameworkServerController(bool loadEntries = true)
        {
            _context = DbConnect.AssetInventoryContext();

            if (loadEntries)
            {
                foreach (var item in _context.FrameworkServers.OrderBy(e => e.HostName))
                {
                    _servers.Add(item);
                }
            }
        }

        /// <summary>
        /// Gets the database context for this controller.
        /// </summary>
        /// <value>
        /// The context.
        /// </value>
        public AssetInventoryContext Context
        {
            get { return _context; }
        }

        /// <summary>
        /// Gets the servers available in the inventory.
        /// </summary>
        public SortableBindingList<FrameworkServer> ServersList
        {
            get { return _servers; }
        }

        /// <summary>
        /// Determines if the hostname already exists in the inventory
        /// </summary>
        /// <param name="hostName">The hostname.</param>
        /// <returns></returns>
        public bool HostNameExists(string hostNameOrIp)
        {
            if (string.IsNullOrEmpty(hostNameOrIp))
            {
                throw new ArgumentNullException("hostNameOrIp");
            }

            if (IsIpAddress(hostNameOrIp))
            {
                return _servers.Any(x => x.IPAddress == hostNameOrIp);
            }

            string simpleName = hostNameOrIp.Split('.').First();

            return
                (
                    from x in _servers
                    where x.HostName.Split('.').First().Equals(simpleName, StringComparison.OrdinalIgnoreCase)
                    select x
                ).Any();
        }

        /// <summary>
        /// Saves the changes.
        /// </summary>
        public void SaveChanges()
        {
            _context.SaveChanges();
        }

        /// <summary>
        /// Selects the <see cref="FrameworkServer"/> based on the specified host name.
        /// </summary>
        /// <param name="hostName">Name of the host.</param>
        /// <returns></returns>
        public FrameworkServer Select(string hostNameOrIp)
        {
            if (string.IsNullOrEmpty(hostNameOrIp))
            {
                throw new ArgumentNullException("hostNameOrIp");
            }

            if (IsIpAddress(hostNameOrIp))
            {
                return _servers.Where(x => x.IPAddress == hostNameOrIp).FirstOrDefault();
            }

            string simpleName = hostNameOrIp.Split('.').First();

            return
                (
                    from x in _servers
                    where x.HostName.Split('.').First().Equals(simpleName, StringComparison.OrdinalIgnoreCase)
                    select x
                ).FirstOrDefault();
        }

        /// <summary>
        /// Commits changes to the inventory
        /// </summary>
        public void Commit()
        {
            foreach (var entity in _deletedItems)
            {
                _context.FrameworkServers.Remove(entity);
            }

            foreach (var server in _servers)
            {
                switch (_context.Entry(server).State)
                {
                    case EntityState.Added:
                    case EntityState.Detached:
                        {
                            _context.FrameworkServers.Add(server);
                            break;
                        }
                }
            }

            _context.SaveChanges();
            _deletedItems.Clear();
        }

        /// <summary>
        /// Gets the available operating systems.
        /// </summary>
        /// <returns></returns>
        public IQueryable<string> OperatingSystems
        {
            get
            {
                return _context.FrameworkServers.Select(n => n.OperatingSystem).Distinct().OrderBy(n => n);
            }
        }

        /// <summary>
        /// Gets the available inventory types.
        /// </summary>
        /// <returns></returns>
        public IQueryable<FrameworkServerType> ServerTypes
        {
            get
            {
                return _context.FrameworkServerTypes.OrderBy(n => n.Name);
            }
        }

        /// <summary>
        /// Gets the type of the server.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public FrameworkServerType GetServerType(ServerType serverType)
        {
            string type = serverType.ToString();
            return
                (
                    from x in _context.FrameworkServerTypes
                    where x.Name.Equals(type.ToString(), StringComparison.OrdinalIgnoreCase)
                    select x
                ).FirstOrDefault();
        }

        /// <summary>
        /// Adds a new <see cref="FrameworkServerType"/>
        /// </summary>
        /// <param name="serverType">Type of the server.</param>
        public void AddServerType(FrameworkServerType serverType)
        {
            _context.FrameworkServerTypes.Add(serverType);
        }

        /// <summary>
        /// Gets the servers based on the server type.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public IQueryable<FrameworkServer> GetServersByType(ServerType type)
        {
            string serverType = type.ToString();

            return
                from x in _context.FrameworkServers
                where x.ServerTypes.Any(e => e.Name.Equals(serverType, StringComparison.OrdinalIgnoreCase))
                    && x.Active
                orderby x.HostName ascending
                select x;
        }

        /// <summary>
        /// Removes a server entry at the specified index.
        /// </summary>
        public void Remove(FrameworkServer server)
        {
            _deletedItems.Add(server);
            _servers.Remove(server);
        }

        /// <summary>
        /// Adds the new server.
        /// </summary>
        /// <param name="server">The server.</param>
        public void AddNewServer(FrameworkServer server)
        {
            _context.FrameworkServers.Add(server);
            ServersList.Add(server);
        }

        /// <summary>
        /// Queries the specified server for system properties.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="status">The status.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">server</exception>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1021:AvoidOutParameters", MessageId = "1#")]
        public bool QueryServer(FrameworkServerProxy server, out string status)
        {
            status = string.Empty;

            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            bool serverLoaded = false;

            // Try to load information about this server, if it fails then ask the user if they
            // would like to load information manually.
            try
            {
                var info = WindowsManagementInstrumentation.GetSystemInformation(server.HostName);
                server.Architecture = info.Architecture;
                server.Cores = info.Cores;
                server.DiskSpace = info.DiskSpace;
                server.HostName = info.HostName;
                server.Memory = info.Memory;
                server.OperatingSystem = info.OperatingSystem;
                server.Processors = info.Processors;

                // We may (or likely will) have multiple IP Addresses for the environment so go with the first found
                server.IpAddress = info.IpAddresses.FirstOrDefault();

                serverLoaded = true;
            }
            catch (COMException ex)
            {
                status = ex.Message;
            }
            catch (UnauthorizedAccessException ex)
            {
                status = ex.Message;
            }

            return serverLoaded;
        }

        /// <summary>
        /// Compares all queues for a server with those assigned to activities to determine those not being used.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <param name="queuesInUse">The queues in use.</param>
        /// <returns></returns>
        public SortableBindingList<RemotePrintQueue> GetQueuesNotInUse(FrameworkServer server, SortableBindingList<PrintQueueInUse> queuesInUse)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            SortableBindingList<RemotePrintQueue> queuesToDelete = new SortableBindingList<RemotePrintQueue>();

            foreach (var queue in _context.RemotePrintQueues.Where(n => n.PrintServerId == server.FrameworkServerId))
            {
                if (!queuesInUse.Any(x => x.QueueName == queue.Name))
                {
                    queuesToDelete.Add(queue);
                }
            }

            return queuesToDelete;
        }

        /// <summary>
        /// Selects all the print queues currently being used in activities for the given server.
        /// </summary>
        /// <param name="server">The server.</param>
        /// <returns></returns>
        public SortableBindingList<PrintQueueInUse> SelectQueuesInUse(FrameworkServer server)
        {
            if (server == null)
            {
                throw new ArgumentNullException("server");
            }

            SortableBindingList<PrintQueueInUse> queuesInUse = new SortableBindingList<PrintQueueInUse>();

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                foreach (RemotePrintQueue printQueue in _context.RemotePrintQueues.Where(n => n.PrintServerId == server.FrameworkServerId))
                {
                    foreach (PrintQueueInUse printQueueInUse in GetQueueUsages(context, printQueue.RemotePrintQueueId.ToString()))
                    {
                        printQueueInUse.ServerName = server.HostName;
                        printQueueInUse.QueueName = printQueue.Name;
                        queuesInUse.Add(printQueueInUse);
                    }
                }
            }

            return queuesInUse;
        }

        private static IEnumerable<PrintQueueInUse> GetQueueUsages(EnterpriseTestEntities entities, string queueId)
        {

            return from mpqu in entities.VirtualResourceMetadataPrintQueueUsages.AsEnumerable()
                   where mpqu.PrintQueueSelectionData.Contains(queueId)
                   join vrm in entities.VirtualResourceMetadataSet
                       on mpqu.VirtualResourceMetadataId equals vrm.VirtualResourceMetadataId into Metadata
                   from m in Metadata
                   join vr in entities.VirtualResources
                       on m.VirtualResourceId equals vr.VirtualResourceId into Resources
                   from r in Resources
                   join es in entities.EnterpriseScenarios
                       on r.EnterpriseScenarioId equals es.EnterpriseScenarioId into Scenarios
                   from s in Scenarios

                   select new PrintQueueInUse
                   {
                       ScenarioName = s.Name,
                       VirtualResource = r.Name,
                       ResourceType = r.ResourceType,
                       MetadataDescription = m.Name,
                       MetadataType = m.MetadataType
                   };
        }

        /// <summary>
        /// Determines if the specified string is a valid IP Address.
        /// </summary>
        /// <param name="addressString"></param>
        /// <returns></returns>
        private static bool IsIpAddress(string addressString)
        {
            IPAddress ipAddress;
            return IPAddress.TryParse(addressString, out ipAddress);
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
                if (_context != null)
                {
                    _context.Dispose();
                }
            }
        }

        #endregion
    }
}
