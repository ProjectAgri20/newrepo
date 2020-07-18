using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Data.Objects.DataClasses;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core;
using HP.ScalableTest.Core.Configuration;

namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// Handles interfacing between an <see cref="EnterpriseTestMap"/> instance and UI controls
    /// </summary>
    public sealed class EnterpriseTestUIController : IDisposable
    {
        private readonly EnterpriseTestMap _databaseMap;
        private EnterpriseTestContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestUIController"/> class.
        /// </summary>
        public EnterpriseTestUIController()
        {
            _databaseMap = new EnterpriseTestMap();
        }

        /// <summary>
        /// Gets the context.
        /// </summary>
        /// <value>The context.</value>
        public EnterpriseTestContext Context
        {
            get { return _context; }
        }

        #region Events

        /// <summary>
        /// Occurs when a node is added.
        /// </summary>
        public event EventHandler<EnterpriseTestUIEventArgs> NodeAdded;

        /// <summary>
        /// Occurs when a node is modified.
        /// </summary>
        public event EventHandler<EnterpriseTestUIEventArgs> NodeModified;

        /// <summary>
        /// Occurs when a node is removed.
        /// </summary>
        public event EventHandler<EnterpriseTestUIEventArgs> NodeRemoved;

        /// <summary>
        /// Occurs when a node is about to be removed.  Called recursively for every node that is removed.
        /// </summary>
        public event EventHandler<EnterpriseTestUIEventArgs> NodeRemoving;

        /// <summary>
        /// Called when a node is added.
        /// </summary>
        /// <param name="node">The node.</param>
        private void OnNodeAdded(EnterpriseTestMapNode node)
        {
            if (NodeAdded != null && DisplayNode(node))
            {
                NodeAdded(this, new EnterpriseTestUIEventArgs(node));
            }
        }

        /// <summary>
        /// Called when a node is modified.
        /// </summary>
        /// <param name="node">The node.</param>
        private void OnNodeModified(EnterpriseTestMapNode node)
        {
            if (NodeModified != null && DisplayNode(node))
            {
                NodeModified(this, new EnterpriseTestUIEventArgs(node));
            }
        }

        /// <summary>
        /// Called when a node is removed.
        /// </summary>
        /// <param name="node">The node.</param>
        private void OnNodeRemoved(EnterpriseTestMapNode node)
        {
            if (NodeRemoved != null)
            {
                NodeRemoved(this, new EnterpriseTestUIEventArgs(node));
            }
        }

        /// <summary>
        /// Called when a node is about to be removed.
        /// </summary>
        /// <param name="node">The node.</param>
        private void OnNodeRemoving(EnterpriseTestMapNode node)
        {
            if (NodeRemoving != null)
            {
                NodeRemoving(this, new EnterpriseTestUIEventArgs(node));
            }
        }

        #endregion

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Collection<EnterpriseTestUIEventArgs> GetNodes()
        {
            // Load the map from the database
            _databaseMap.Clear();
            _databaseMap.Load(UserManager.CurrentUser);

            Collection<EnterpriseTestUIEventArgs> nodes = new Collection<EnterpriseTestUIEventArgs>();
            foreach (var node in _databaseMap.GetSortedNodes())
            {
                nodes.Add(new EnterpriseTestUIEventArgs(node));
            }

            return nodes;
        }

        /// <summary>
        /// Loads this instance.
        /// </summary>
        public void Load()
        {
            // Load the map from the database
            _databaseMap.Clear();
            _databaseMap.Load(UserManager.CurrentUser);

            foreach (EnterpriseTestMapNode node in _databaseMap.GetSortedNodes())
            {
                OnNodeAdded(node);
            }
        }

        /// <summary>
        /// Refreshes this instance with existing data without re-loading from the database.
        /// </summary>
        public void Refresh()
        {
            foreach (EnterpriseTestMapNode node in _databaseMap.GetSortedNodes())
            {
                OnNodeAdded(node);
            }
        }

        /// <summary>
        /// Determines whether the object with the specified id can be enabled or disabled.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public Tuple<bool, string> CanEnableDisable(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];
            if (node.NodeType.CanEnableDisable())
            {
                return new Tuple<bool, string>(true, node.Enabled ? "Disable" : "Enable");
            }
            else
            {
                return new Tuple<bool, string>(false, null);
            }
        }

        /// <summary>
        /// Determines whether the object with the specified id can contain a folder.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if the object with the specified id can contain a folder; otherwise, <c>false</c>.
        /// </returns>
        public bool CanContainFolder(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];
            return node.NodeType != ConfigurationObjectType.ResourceMetadata;
        }

        /// <summary>
        /// Determines whether the object with the specified id can contain a scenario.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>
        ///   <c>true</c> if the object with the specified id can contain a scenario; otherwise, <c>false</c>.
        /// </returns>
        public bool CanContainScenario(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];
            return node.NodeType == ConfigurationObjectType.ScenarioFolder;
        }

        /// <summary>
        /// Determines whether the node with the specified ID can be dropped onto the target node with the specified ID.
        /// </summary>
        /// <param name="nodeId">The node id.</param>
        /// <param name="targetId">The target id.</param>
        /// <returns>
        ///   <c>true</c> if the drop is valid; otherwise, <c>false</c>.
        /// </returns>
        public bool CanDrop(Guid nodeId, Guid targetId)
        {
            EnterpriseTestMapNode source = _databaseMap[nodeId];
            EnterpriseTestMapNode target = _databaseMap[targetId];

            // Do a schema check first
            if (!target.NodeType.CanContain(source.NodeType))
            {
                return false;
            }

            // Make sure we don't copy metadata to a different resource type
            if (target.NodeType == ConfigurationObjectType.VirtualResource)
            {
                return target.ResourceType == source.ResourceType;
            }
            else
            {
                // Otherwise, we're good.
                return true;
            }
        }


        /// <summary>
        /// Creates a new folder under the specified parent id.
        /// </summary>
        /// <param name="parentId">The parent id.</param>
        /// <returns>The ID of the created folder.</returns>
        public Guid CreateFolder(Guid? parentId)
        {
            // Create a default folder, which is valid if the parent is null
            var folder = new ConfigurationTreeFolder(SequentialGuid.NewGuid(), "New Folder", "ScenarioFolder", parentId);

            // Determine the real folder type and parent
            if (parentId != null)
            {
                EnterpriseTestMapNode node = _databaseMap[(Guid)parentId];
                switch (node.NodeType)
                {
                    case ConfigurationObjectType.EnterpriseScenario:
                    case ConfigurationObjectType.ResourceFolder:
                        folder.FolderType = "ResourceFolder";
                        break;

                    case ConfigurationObjectType.VirtualResource:
                    case ConfigurationObjectType.MetadataFolder:
                        folder.FolderType = "MetadataFolder";
                        break;
                }
            }

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                context.ConfigurationTreeFolders.AddObject(folder);
                HandleObjectChange(context, folder);
                context.SaveChanges();
            }

            return folder.ConfigurationTreeFolderId;
        }

        /// <summary>
        /// Creates a new scenario under the specified parent id.
        /// </summary>
        /// <param name="folderId">The folder id.</param>
        /// <returns>The ID of the created scenario.</returns>
        public Guid CreateScenario(Guid? folderId)
        {
            // Create a new scenario
            EnterpriseScenario scenario = new EnterpriseScenario();
            scenario.FolderId = folderId;
            scenario.Owner = UserManager.CurrentUserName;
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                scenario.AddGroups(context, scenario.Owner);
                context.EnterpriseScenarios.AddObject(scenario);
                HandleObjectChange(context, scenario);
                context.SaveChanges();
            }

            return scenario.EnterpriseScenarioId;
        }

        /// <summary>
        /// Renames the object with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        public void Rename(Guid id, string name)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                EnterpriseTestMapNode node = _databaseMap[id];
                switch (node.NodeType)
                {
                    case ConfigurationObjectType.EnterpriseScenario:
                        var scenario = EnterpriseScenario.Select(context, id);
                        scenario.Name = name;
                        HandleObjectChange(context, scenario);
                        break;

                    case ConfigurationObjectType.VirtualResource:
                        var resource = VirtualResource.Select(context, id);
                        resource.Name = name;
                        HandleObjectChange(context, resource);
                        break;

                    case ConfigurationObjectType.ResourceMetadata:
                        var metadata = VirtualResourceMetadata.Select(context, id);
                        metadata.Name = name;
                        HandleObjectChange(context, metadata);
                        break;

                    case ConfigurationObjectType.ScenarioFolder:
                    case ConfigurationObjectType.ResourceFolder:
                    case ConfigurationObjectType.MetadataFolder:
                        var folder = ConfigurationTreeFolder.Select(context, id);
                        folder.Name = name;
                        HandleObjectChange(context, folder);
                        break;
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Enables/disable the specified object.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="enabled">Whether the object should be enabled.</param>
        public void EnableDisable(Guid id, bool enabled)
        {
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                EnterpriseTestMapNode node = _databaseMap[id];
                switch (node.NodeType)
                {
                    case ConfigurationObjectType.VirtualResource:
                        var resource = VirtualResource.Select(context, id);
                        resource.Enabled = enabled;
                        HandleObjectChange(context, resource);
                        break;

                    case ConfigurationObjectType.ResourceMetadata:
                        var metadata = VirtualResourceMetadata.Select(context, id);
                        metadata.Enabled = enabled;
                        HandleObjectChange(context, metadata);
                        break;
                }
                context.SaveChanges();
            }
        }

        /// <summary>
        /// Moves the object with the specified id to the specified parent.
        /// </summary>
        /// <param name="nodeId">The id.</param>
        /// <param name="targetId">The target parent id.</param>
        public void Move(Guid nodeId, Guid? targetId)
        {
            // Get the map node that this id corresponds to and update it
            EnterpriseTestMapNode node = _databaseMap[nodeId];
            var affected = _databaseMap.GetMoveAffectedNodes(node);
            _databaseMap.Move(node, targetId);

            // Update all database objects that were affected
            // Use the local context if it is available - this fixes an issue where the parent id changes
            // but the cached copy in the local context does not get updated, which causes a crash when the object is committed
            if (_context != null)
            {
                MoveHandler(_context, affected);
            }
            else
            {
                using (EnterpriseTestContext context = new EnterpriseTestContext())
                {
                    MoveHandler(context, affected);
                }
            }
        }

        private void MoveHandler(EnterpriseTestContext context, IEnumerable<Guid> affected)
        {
            foreach (Guid affectedId in affected)
            {
                EnterpriseTestMapNode moved = _databaseMap[affectedId];
                switch (moved.NodeType)
                {
                    case ConfigurationObjectType.EnterpriseScenario:
                        var scenario = EnterpriseScenario.Select(context, affectedId);
                        scenario.FolderId = moved.FolderId;
                        HandleObjectChange(context, scenario);
                        break;

                    case ConfigurationObjectType.VirtualResource:
                        var resource = VirtualResource.Select(context, affectedId);
                        resource.EnterpriseScenarioId = (Guid)moved.ParentId;
                        resource.FolderId = moved.FolderId;
                        HandleObjectChange(context, resource);
                        break;

                    case ConfigurationObjectType.ResourceMetadata:
                        var metadata = VirtualResourceMetadata.Select(context, affectedId);
                        metadata.VirtualResourceId = (Guid)moved.ParentId;
                        metadata.FolderId = moved.FolderId;
                        HandleObjectChange(context, metadata);
                        break;

                    case ConfigurationObjectType.ScenarioFolder:
                    case ConfigurationObjectType.ResourceFolder:
                    case ConfigurationObjectType.MetadataFolder:
                        var folder = ConfigurationTreeFolder.Select(context, affectedId);
                        folder.ParentId = moved.ParentId;
                        HandleObjectChange(context, folder);
                        break;
                }
            }
            context.SaveChanges();
        }

        /// <summary>
        /// Removes the object with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        public void Delete(Guid id)
        {
            // Get the map node that this id corresponds to and the subtree below it
            EnterpriseTestMapNode node = _databaseMap[id];
            IEnumerable<EnterpriseTestMapNode> subtree = _databaseMap.GetSubtree(node);
            foreach (EnterpriseTestMapNode removingNode in subtree)
            {
                OnNodeRemoving(removingNode);
            }

            // Update all the database objects that were affected
            DatabaseDelete(subtree);

            // Handle object changes
            foreach (EnterpriseTestMapNode deleted in subtree)
            {
                _databaseMap.Remove(deleted);
            }
            OnNodeRemoved(subtree.FirstOrDefault());
        }

        /// <summary>
        /// Gets a descriptive tag for the object with the specified id.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public ConfigurationObjectTag GetObjectTag(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];
            if (node == null)
            {
                return null;
            }
            else
            {
                return new ConfigurationObjectTag(node);
            }
        }

        /// <summary>
        /// Gets the entity object.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public EntityObject GetEntityObject(Guid id)
        {
            // Discard our old entity context and create a new one.  This seems wasteful,
            // but if we maintain the same context it will cache all the data that is ever
            // loaded from it.  This can cause huge memory growth over time, as well as
            // concurrency issues.  Creating a new context is cheap and avoids these issues.
            if (_context != null)
            {
                _context.Dispose();
            }
            _context = new EnterpriseTestContext();

            EnterpriseTestMapNode node = _databaseMap[id];
            switch (node.NodeType)
            {
                case ConfigurationObjectType.EnterpriseScenario:
                    return EnterpriseScenario.Select(_context, id);

                case ConfigurationObjectType.VirtualResource:
                    return VirtualResource.Select(_context, id);

                case ConfigurationObjectType.ResourceMetadata:
                    return VirtualResourceMetadata.Select(_context, id);

                case ConfigurationObjectType.ScenarioFolder:
                case ConfigurationObjectType.ResourceFolder:
                case ConfigurationObjectType.MetadataFolder:
                    return ConfigurationTreeFolder.Select(_context, id);

                default:
                    return null;
            }
        }

        private static bool DisplayNode(EnterpriseTestMapNode node)
        {
            if (node.NodeType == ConfigurationObjectType.ResourceMetadata)
            {
                VirtualResourceType resourceType = EnumUtil.Parse<VirtualResourceType>(node.ResourceType);
                return resourceType.UsesPlugins();
            }
            else
            {
                return true;
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has unsaved changes.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has unsaved changes; otherwise, <c>false</c>.
        /// </value>
        public bool HasUnsavedChanges
        {
            get
            {
                if (_context == null)
                {
                    return false;
                }

                _context.FinalizeChanges();
                return _context.HasObjectsInState(EntityState.Added | EntityState.Modified | EntityState.Deleted);
            }
        }

        /// <summary>
        /// Gets a value indicating whether this instance has added or removed objects.
        /// </summary>
        /// <value>
        /// 	<c>true</c> if this instance has added or removed objects; otherwise, <c>false</c>.
        /// </value>
        public bool HasAddedOrRemovedObjects
        {
            get
            {
                if (_context == null)
                {
                    return false;
                }

                _context.FinalizeChanges();
                foreach (EntityObject entity in _context.GetObjectsInState(EntityState.Added | EntityState.Deleted))
                {
                    if (entity is EnterpriseScenario || entity is VirtualResource || entity is VirtualResourceMetadata)
                    {
                        return true;
                    }
                }
                return false;
            }
        }

        /// <summary>
        /// Commits changes to the database.
        /// </summary>
        /// <returns>True if the commit completed normally; false if there were any issues.</returns>
        public bool Commit()
        {
            // If the context is null, there's nothing to do here
            if (_context == null)
            {
                return true;
            }

            _context.FinalizeChanges();

            // Find all the objects that have changed so we can handle them appropriately
            foreach (EntityObject entity in _context.GetObjectsInState(EntityState.Added | EntityState.Modified | EntityState.Deleted))
            {
                HandleObjectChange(_context, entity);
            }

            // Save the changes to the database
            try
            {
                _context.SaveChanges();
                return true;
            }
            catch (OptimisticConcurrencyException ex)
            {
                // Log information about the exception we received, then tell the caller it failed
                TraceFactory.Logger.Error("Save changes failed.", ex);
                return false;
            }
        }

        private void HandleObjectChange(EnterpriseTestEntities entities, EntityObject entity)
        {
            // Get a node that is representative of this item
            EnterpriseTestMapNode node = EnterpriseTestMapNode.Create(entity);
            if (node != null)
            {
                switch (entity.EntityState)
                {
                    case EntityState.Added:
                        _databaseMap.Add(node);
                        OnNodeAdded(node);
                        break;

                    case EntityState.Modified:
                        _databaseMap.Update(node);
                        OnNodeModified(node);
                        break;

                    case EntityState.Deleted:
                        _databaseMap.Remove(node);
                        OnNodeRemoved(node);
                        break;
                }
            }
        }

        /// <summary>
        /// Discards all changes.
        /// </summary>
        public void DiscardChanges()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }

        private static void DatabaseDelete(IEnumerable<EnterpriseTestMapNode> nodes)
        {
           //--------------------------------------------------------------------------------------------------
           // ADD: CR #1894.  Cascading deletes were throwing an unhandled exception because of a DB Foreign
           // Key Constraint on the folderId field.  What that means is that the ORDER in which things are
           // deleted, now becomes very important.  If we attempt to remove a 'container' before we remove
           // the contents, the exception will be thrown when the contents are removed.  So - in order to make
           // this work, we group the objects to be deleted - and then we have to order those groups (since the
           // order of the GroupBy could be non-deterministic) and make sure that we delete things in the
           // correct sequence.  There is probably a more elegant way of doing this - but there was no time.
            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                int[] ordered = new int[6] {-1, -1, -1, -1, -1, -1};

                var colls = nodes.GroupBy(n => n.NodeType, n => n.Id);
                int ndx = 0;
                foreach (var grp in colls)
                {
                    switch (grp.Key)
                    {
                        case ConfigurationObjectType.ResourceMetadata:
                            ordered[0] = ndx++;
                            break;
                        case ConfigurationObjectType.MetadataFolder:
                            ordered[1] = ndx++;
                            break;
                        case ConfigurationObjectType.VirtualResource:
                            ordered[2] = ndx++;
                            break;
                        case ConfigurationObjectType.ResourceFolder:
                            ordered[3] = ndx++;
                            break;
                        case ConfigurationObjectType.EnterpriseScenario:
                            ordered[4] = ndx++;
                            break;
                        case ConfigurationObjectType.ScenarioFolder:
                            ordered[5] = ndx++;
                            break;
                    }
                }

                for (ndx = 0; ndx < 6; ndx++)
                {
                    if (ordered[ndx] >= 0)
                    {
                        switch (colls.ElementAt(ordered[ndx]).Key)
                        {
                            case ConfigurationObjectType.ResourceMetadata:
                                foreach (var vrmd in VirtualResourceMetadata.Select(context, colls.ElementAt(ordered[ndx])))
                                {
                                    context.DeleteObject(vrmd);
                                }
                                break;
                            case ConfigurationObjectType.MetadataFolder:
                                foreach (var mdf in ConfigurationTreeFolder.Select(context, colls.ElementAt(ordered[ndx])))
                                {
                                    context.DeleteObject(mdf);
                                }
                                break;
                            case ConfigurationObjectType.VirtualResource:
                                foreach (var vr in VirtualResource.Select(context, colls.ElementAt(ordered[ndx])))
                                {
                                    context.DeleteObject(vr);
                                }
                                break;
                            case ConfigurationObjectType.ResourceFolder:
                                foreach (var rf in ConfigurationTreeFolder.Select(context, colls.ElementAt(ordered[ndx])))
                                {
                                    context.DeleteObject(rf);
                                }
                                break;
                            case ConfigurationObjectType.EnterpriseScenario:
                                foreach (var scenario in EnterpriseScenario.Select(context, colls.ElementAt(ordered[ndx])))
                                {
                                    context.DeleteObject(scenario);
                                }
                                break;
                            case ConfigurationObjectType.ScenarioFolder:
                                foreach (var sf in ConfigurationTreeFolder.Select(context, colls.ElementAt(ordered[ndx])))
                                {
                                    var deletingScenarioIds = nodes.Where(n => n.NodeType == ConfigurationObjectType.EnterpriseScenario).Select(n => n.Id);
                                    foreach (var folder in ConfigurationTreeFolder.Select(context, colls.ElementAt(ordered[ndx])))
                                    {
                                        var containedScenarios = ConfigurationTreeFolder.ContainedScenarioIds(context, folder.ConfigurationTreeFolderId);
                                        if (containedScenarios.All(n => deletingScenarioIds.Contains(n)))
                                        {
                                            // All of the sceanrios under this folder are in the list of scenarios to delete,
                                            // so we can delete this folder as well.
                                            context.DeleteObject(folder);
                                        }
                                    }
                                }
                                break;
                        }
                        context.SaveChanges();
                    }
                }
            }
        }

        public ConfigurationObjectType GetType(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];
            return node.NodeType;
        }

        /// <summary>
        /// Checks if the id is the guid for any type of a folder
        /// </summary>
        /// <param name="id">The id to check</param>
        /// <returns>True if this node is any type of a folder</returns>
        public bool IsFolder(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];

            return (node.NodeType == ConfigurationObjectType.ScenarioFolder || 
                node.NodeType == ConfigurationObjectType.ResourceFolder ||
                node.NodeType == ConfigurationObjectType.MetadataFolder);
        }


        /// <summary>
        /// Checks if the id is the guid for a scenario folder
        /// </summary>
        /// <param name="id">The id to check</param>
        /// <returns>True if this node is a scenario folder</returns>
        public bool IsScenarioFolder(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];
            return node.NodeType == ConfigurationObjectType.ScenarioFolder;
        }

        /// <summary>
        /// Checks if the id is the guid for a Resource folder
        /// </summary>
        /// <param name="id">The id to check</param>
        /// <returns>True if this node is a Resource folder</returns>
        public bool IsResourceFolder(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];
            return node.NodeType == ConfigurationObjectType.ResourceFolder;
        }

        /// <summary>
        /// Checks if the id is the guid for a Metadata (activity) folder
        /// </summary>
        /// <param name="id">The id to check</param>
        /// <returns>True if this node is a Metadata (activity) folder</returns>
        public bool IsMetadataFolder(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];
            return node.NodeType == ConfigurationObjectType.MetadataFolder;
        }

        /// <summary>
        /// Checks if the id is a guid for a scenario
        /// </summary>
        /// <param name="id">The id to check</param>
        /// <returns>True if the Node 'is' a scenario</returns>
        public bool IsScenario(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];
            return node.NodeType == ConfigurationObjectType.EnterpriseScenario;
        }


        /// <summary>
        /// Checks if the id is a Guid for a Virtual Resource
        /// </summary>
        /// <param name="id">The node id (Guid)</param>
        /// <returns>True if the node 'is' a Virtual Resource</returns>
        public bool IsResource(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];
            return node.NodeType == ConfigurationObjectType.VirtualResource;
        }

        /// <summary>
        /// Checks if the id is for a node that support export to another format (scenario configuration file or plugin configuration file).
        /// </summary>
        /// <param name="id">The node id (Guid)</param>
        /// <returns>True if the node type can be exported.</returns>
        public bool CanExport(Guid id)
        {
            return IsScenario(id) || IsResourceMetaData(id);
        }

        /// <summary>
        /// Checks if the id is the guid for resource metadata (Activity)
        /// </summary>
        /// <param name="id">Guid</param>
        /// <returns>True if the node 'is' an activity (Resource Metadata)</returns>
        public bool IsResourceMetaData(Guid id)
        {
            EnterpriseTestMapNode node = _databaseMap[id];
            return node.NodeType == ConfigurationObjectType.ResourceMetadata;
        }

        /// <summary>
        /// Checks two RadTreeNodes to see if they are of different 'types'.
        /// </summary>
        /// <param name="id1">First node</param>
        /// <param name="id2">Second node</param>
        /// <returns>true of the nodes are the same type, else false</returns>
        public bool NodeCCPGroupsMatch(Guid id1, Guid id2)
        {
            if ((id1 == Guid.Empty) || (id2 == Guid.Empty))
            {
                return false;
            }
            EnterpriseTestMapNode node1 = _databaseMap[id1];
            EnterpriseTestMapNode node2 = _databaseMap[id2];

            return node1.NodeType == node2.NodeType;
        }


        /// <summary>
        /// Creates a Copy of a Folder node.  Can make an exact copy or one with a date/time stamp appended.
        /// </summary>
        /// <param name="source">Fodler to copy</param>
        /// <param name="parent">Parent node for the copy</param>
        /// <returns>Guid of the folder node that was created</returns>
        public Guid CopyFolder(Guid source, Guid? parent)
        {
            using (EnterpriseTestContext ctx = new EnterpriseTestContext())
            {
                ConfigurationTreeFolder srcFolder = ConfigurationTreeFolder.Select(ctx, source);
                Guid dst = CreateFolder(parent);
                ConfigurationTreeFolder dstFolder = ConfigurationTreeFolder.Select(ctx, dst);

                Rename(dst, srcFolder.Name);
                HandleObjectChange(ctx, dstFolder);
                if (parent != null)
                {
                    HandleObjectChange(ctx, GetEntityObject((Guid)parent));
                }
                ctx.SaveChanges();
                return dst;
            }
        }


        /// <summary>
        /// Creates a copy of a scenario. Can make an exact copy, or one that has the name appendeed with a time/date stamp
        /// </summary>
        /// <param name="source">The Source scenario Id</param>
        /// <param name="parent">Parent if available</param>
        /// <returns>Guid of the Scenario node that was created</returns>
        public Guid CopyScenario(Guid source, Guid? parent)
        {
            using (EnterpriseTestContext ctx = new EnterpriseTestContext())
            {
                var srcScenario = EnterpriseScenario.Select(ctx, source);
                Guid dst = CreateScenario(parent);
                EnterpriseScenario dstScenario = EnterpriseScenario.Select(ctx, dst);

                dstScenario.Name = srcScenario.Name;
                dstScenario.Description = srcScenario.Description;
                dstScenario.Vertical = srcScenario.Vertical;
                dstScenario.Company = srcScenario.Company;

                HandleObjectChange(ctx, dstScenario);
                if (parent != null)
                {
                    HandleObjectChange(ctx, GetEntityObject((Guid)parent));
                }
                ctx.SaveChanges();
                return dst;
            }
        }

        /// <summary>
        /// Creates a copy of a Resource.
        /// </summary>
        /// <param name="source">The Source Resource to copy from</param>
        /// <param name="srcParent"></param>
        /// <param name="newParent"></param>
        /// <param name="Mappings"></param>
        /// <returns>Guid of the Resource node that was created</returns>
        public Guid CopyResource(Guid source, Guid srcParent, Guid newParent, Dictionary<Guid?, Guid?> Mappings)
        {
            using (EnterpriseTestContext ctx = new EnterpriseTestContext())
            {
                VirtualResource vrSource = VirtualResource.Select(ctx, source);
                VirtualResource vrCopy = vrSource.ShallowCopy();
                HandleObjectChange(ctx, vrCopy);
                ctx.SaveChanges();
                EnterpriseScenario dstScenario;
                if (vrSource.EnterpriseScenarioId == srcParent)
                {
                    dstScenario = EnterpriseScenario.Select(ctx, newParent);
                }
                else
                {
                    try
                    {
                        vrCopy.FolderId = (Guid)Mappings[srcParent];
                        dstScenario = EnterpriseScenario.Select(ctx, (Guid)Mappings[vrSource.EnterpriseScenarioId]);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Cannot find New Target Scenario /Resource Folder for " + source.ToString());
                    }
                }
                dstScenario.VirtualResources.Add(vrCopy);
                HandleObjectChange(ctx, vrCopy);
                HandleObjectChange(ctx, dstScenario);
                ctx.SaveChanges();
                return vrCopy.VirtualResourceId;
            }
        }

        /// <summary>
        /// Creates a copy of an activity (Virtual Resource metadata)
        /// </summary>
        /// <param name="source">The source activity to copy from</param>
        /// <param name="srcParent"></param>
        /// <param name="newParent"></param>
        /// <param name="Mappings"></param>
        /// <param name="isPartOfResourceCopy"></param>
        /// <returns>Guid of the Activity (metadata) node that was created</returns>
        public Guid CopyActivity(Guid source, Guid srcParent, Guid newParent, Dictionary<Guid?, Guid?> Mappings, bool isPartOfResourceCopy)
        {
            using (EnterpriseTestContext ctx = new EnterpriseTestContext())
            {
                VirtualResourceMetadata vrmdSource = VirtualResourceMetadata.Select(ctx, source);
                VirtualResourceMetadata vrmdCopy = vrmdSource.Copy(isPartOfResourceCopy);

                vrmdCopy.Name = vrmdSource.Name;
                VirtualResource dstVR;
                HandleObjectChange(ctx, vrmdCopy);
                ctx.SaveChanges();
                if (vrmdSource.VirtualResourceId == srcParent)
                {
                    dstVR = VirtualResource.Select(ctx, newParent);
                }
                else
                {
                    try
                    {
                        vrmdCopy.FolderId = (Guid)Mappings[srcParent];
                        dstVR = VirtualResource.Select(ctx, (Guid)Mappings[vrmdSource.VirtualResourceId]);
                    }
                    catch (Exception)
                    {
                        throw new Exception("Cannot find New Target VR / MetaData Folder for " + source.ToString());
                    }
                }
                dstVR.VirtualResourceMetadataSet.Add(vrmdCopy);
                HandleObjectChange(ctx, vrmdCopy);
                HandleObjectChange(ctx, dstVR);
                ctx.SaveChanges();
                return vrmdCopy.VirtualResourceMetadataId;
            }
        }


        /// <summary>
        /// Manages the copying of any of the nodes used. Tries to link the UI structure to the database
        /// structure of the trees.
        /// </summary>
        /// <param name="SourceNodes">List representation of the tree.  In couplet format - 
        ///   [0] is parent, [1] is the 'node'. Using List because any node can be parent to multiple
        ///   other nodes and reversing it to use a dictionary negates the performance benefit of dictioanries</param>
        /// <param name="Ndx">Index into the tree representation list.</param>
        /// <param name="Mappings">Dictionary. Key is the original node ID, and the Value is the new/copy
        ///   node Id</param>
        /// <returns>Guid / Node ID of the node that was created during the copy.</returns>
        public Guid CopyNode(List<Guid?> SourceNodes, int Ndx, Dictionary<Guid?, Guid?> Mappings, ConfigurationObjectType rootMovedType)
        {
            if (SourceNodes.Count <= (Ndx + 1))
            {
                throw new Exception("Bad Source List / Index in CopyNode ::" + Ndx.ToString());
            }
            if (Mappings.ContainsKey(SourceNodes[Ndx + 1]))
            {
                throw new Exception("Doubly Linked Node in UI Tree: " + SourceNodes[Ndx + 1].ToString());
            }
            EnterpriseTestMapNode src = _databaseMap[(Guid)SourceNodes[Ndx + 1]];
            switch (src.NodeType)
            {
                case ConfigurationObjectType.ScenarioFolder:
                case ConfigurationObjectType.ResourceFolder:
                case ConfigurationObjectType.MetadataFolder:
                    return CopyFolder((Guid)SourceNodes[Ndx + 1], Mappings[(Guid?)SourceNodes[Ndx]]);

                case ConfigurationObjectType.EnterpriseScenario:
                    return CopyScenario((Guid)SourceNodes[Ndx + 1], Mappings[(Guid?)SourceNodes[Ndx]]);

                case ConfigurationObjectType.VirtualResource:
                    return CopyResource((Guid)SourceNodes[Ndx + 1], (Guid)SourceNodes[Ndx], (Guid)Mappings[(Guid?)SourceNodes[Ndx]], Mappings);

                case ConfigurationObjectType.ResourceMetadata:
                    bool partOfResource = (rootMovedType != ConfigurationObjectType.ResourceMetadata && rootMovedType != ConfigurationObjectType.MetadataFolder);
                    return CopyActivity((Guid)SourceNodes[Ndx + 1], (Guid)SourceNodes[Ndx], (Guid)Mappings[(Guid?)SourceNodes[Ndx]], Mappings, partOfResource);

                default:
                    throw new Exception("Unknown Node type in CopyNode?");
            }
        }
   
        #region IDisposable Members

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
            }
        }

        #endregion
    }
}
