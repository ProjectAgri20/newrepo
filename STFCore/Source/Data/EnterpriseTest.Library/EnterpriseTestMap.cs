using System;
using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using System.Net;
using System.Data.Common;
using System.Collections.ObjectModel;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;
using HP.ScalableTest.Core.Security;

namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// Lightweight map of the enterprise test configuration objects.
    /// </summary>
    internal class EnterpriseTestMap
    {
        private List<EnterpriseTestMapNode> _nodes = new List<EnterpriseTestMapNode>();

        /// <summary>
        /// Gets the <see cref="EnterpriseTestMapNode"/> with the specified id.
        /// </summary>
        public EnterpriseTestMapNode this[Guid id]
        {
            get { return _nodes.FirstOrDefault(n => n.Id == id); }
        }

        private IEnumerable<EnterpriseTestMapNode> FindChildren(EnterpriseTestMapNode node)
        {
            return _nodes.Where(n => n.ParentId == node.Id || n.FolderId == node.Id);
        }

        /// <summary>
        /// Loads this instance from the database.
        /// </summary>
        public void Load()
        {
            _nodes.AddRange(LoadNodesFromDatabase(null));
        }

        /// <summary>
        /// Loads this instance from the database.
        /// Nodes inaccessible by the specified user are ignored.
        /// </summary>
        /// <param name="user">The user.</param>
        public void Load(UserCredential user)
        {
            _nodes.AddRange(LoadNodesFromDatabase(user));
        }

        /// <summary>
        /// Clears this instance.
        /// </summary>
        public void Clear()
        {
            _nodes.Clear();
        }

        /// <summary>
        /// Adds the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Add(EnterpriseTestMapNode node)
        {
            _nodes.Add(node);
        }

        /// <summary>
        /// Updates the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Update(EnterpriseTestMapNode node)
        {
            Remove(node);
            Add(node);
        }

        /// <summary>
        /// Removes the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        public void Remove(EnterpriseTestMapNode node)
        {
            _nodes.RemoveAll(n => n.Id == node.Id);
        }

        /// <summary>
        /// Gets al nodes in the subtree starting at the specified node.
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public IEnumerable<EnterpriseTestMapNode> GetSubtree(EnterpriseTestMapNode node)
        {
            var nodes = new List<EnterpriseTestMapNode>() { node };
            foreach (EnterpriseTestMapNode child in FindChildren(node))
            {
                nodes.AddRange(GetSubtree(child));
            }
            return nodes;
        }

        /// <summary>
        /// Gets all nodes that would be affected if the specified node was moved.
        /// (This will usually not include the entire subtree.)
        /// </summary>
        /// <param name="node">The node.</param>
        /// <returns></returns>
        public IEnumerable<Guid> GetMoveAffectedNodes(EnterpriseTestMapNode node)
        {
            var affectedNodes = new List<Guid>() { node.Id };

            // If the node being moved is a folder, then children of that node may be affected,
            // since the parent ID could change
            if (node.NodeType.IsFolder())
            {
                foreach (EnterpriseTestMapNode child in FindChildren(node))
                {
                    affectedNodes.AddRange(GetMoveAffectedNodes(child));
                }
            }

            return affectedNodes;
        }

        /// <summary>
        /// Moves the specified node under the specified parent.
        /// </summary>
        /// <param name="node">The node that is moving</param>
        /// <param name="parentId">The Id of the target parent</param>
        public void Move(EnterpriseTestMapNode node, Guid? parentId)
        {
            // Get the parent node - if it is null, we can return now
            if (parentId == null)
            {
                node.ParentId = node.FolderId = null;
                return;
            }
            EnterpriseTestMapNode parent = this[(Guid)parentId];
            if (parent == null)
            {
                node.ParentId = node.FolderId = null;
                return;
            }

            // This can get a little confusing, but here are the rules we have to follow:
            // 1. The parent ID of a configuration object is always another configuration object (or null).
            // 2. The folder ID of a configuration object is always a folder object (or null).
            // 3. The parent ID of a folder object can be either a configuration or folder object (or null).
            // 4. The folder ID of a folder object is always null.

            // There are four cases, depending on the types of the node and the parent
            bool nodeIsFolder = node.NodeType.IsFolder();
            bool parentIsFolder = parent.NodeType.IsFolder();

            if (!nodeIsFolder && !parentIsFolder)
            {
                // Both are configuration objects
                node.ParentId = parentId;
                node.FolderId = null;
            }
            else if (!nodeIsFolder && parentIsFolder)
            {
                // Configuration object being added to a folder
                node.FolderId = parentId;

                // Find the "real" parent of the object by following the tree
                node.ParentId = FindNonFolderParentId(parent);
            }
            else if (nodeIsFolder && !parentIsFolder)
            {
                // Folder being added to a configuration object
                node.ParentId = parentId;

                // All the folder's children may have a new parent
                foreach (EnterpriseTestMapNode child in FindNonFolderChildren(node))
                {
                   //------------------------------------------------------------------------------------------
                   // ADD: Changed to allow for cuttng a subtree to an object which has been relocated to the
                   // root (in which case parent.ParentId is null and causes an exception - but the actual
                   // value needed is parentid.
                    child.ParentId = parent.ParentId == null ? parentId : parent.ParentId;
                }
            }
            else if (nodeIsFolder && parentIsFolder)
            {
                // Folder being added to a folder
                // Combination of the previous two cases
                node.ParentId = parentId;
                Guid? nonFolderParentId = FindNonFolderParentId(parent);
                foreach (EnterpriseTestMapNode child in FindNonFolderChildren(node))
                {
                    child.ParentId = nonFolderParentId;
                }
            }
        }

        private Guid? FindNonFolderParentId(EnterpriseTestMapNode node)
        {
            if (!node.NodeType.IsFolder())
            {
                return node.Id;
            }
            else if (node.ParentId == null)
            {
                return null;
            }
            else
            {
                return FindNonFolderParentId(this[(Guid)node.ParentId]);
            }
        }

        private IEnumerable<EnterpriseTestMapNode> FindNonFolderChildren(EnterpriseTestMapNode node)
        {
            List<EnterpriseTestMapNode> childNodes = new List<EnterpriseTestMapNode>();
            foreach (EnterpriseTestMapNode child in _nodes.Where(n => n.ParentId == node.Id || n.FolderId == node.Id))
            {
                if (!child.NodeType.IsFolder())
                {
                    childNodes.Add(child);
                }
                else
                {
                    childNodes.AddRange(FindNonFolderChildren(child));
                }
            }
            return childNodes;
        }

        private class ScenarioData
        {
            public Guid ScenarioId { get; set; }
            public string ScenarioName { get; set; }
            public Guid? ScenarioFolderId { get; set; }
            public Guid ResourceId { get; set; }
            public string ResourceName { get; set; }
            public Guid? ResourceFolderId { get; set; }
            public string ResourceType { get; set; }
            public bool ResourceEnabled { get; set; }
            public Guid MetadataId { get; set; }
            public string MetadataName { get; set; }
            public Guid? MetadataFolderId { get; set; }
            public string MetadataType { get; set; }
            public bool MetadataEnabled { get; set; }
        }

        private static List<EnterpriseTestMapNode> LoadNodesFromDatabase(UserCredential user)
        {
            List<EnterpriseTestMapNode> nodes = new List<EnterpriseTestMapNode>();
            bool needFilter = (user != null && !user.HasPrivilege(UserRole.Manager));
            string sqlText = needFilter ? Resource.SelectTreeViewData.FormatWith(user.UserName) : Resource.SelectTreeViewDataNoFilter;

            using (EnterpriseTestContext context = new EnterpriseTestContext())
            {
                Collection<ScenarioData> scenarioData = new Collection<ScenarioData>();
                using (SqlAdapter adapter = new SqlAdapter(EnterpriseTestSqlConnection.ConnectionString))
                {
                    DbDataReader reader = adapter.ExecuteReader(sqlText);
                    if (reader != null)
                    {
                        while (reader.Read())
                        {
                            scenarioData.Add(new ScenarioData()
                            {
                                MetadataEnabled = !string.IsNullOrEmpty(reader["MDE"].ToString()) && (bool)reader["MDE"],
                                MetadataFolderId = !string.IsNullOrEmpty(reader["MDFID"].ToString()) ? (Guid?)reader["MDFID"] : (Guid?)null,
                                MetadataId = !string.IsNullOrEmpty(reader["MDID"].ToString()) ? (Guid)reader["MDID"] : Guid.Empty,
                                MetadataName = !string.IsNullOrEmpty(reader["MDN"].ToString()) ? (string)reader["MDN"]: string.Empty,
                                MetadataType = !string.IsNullOrEmpty(reader["MDT"].ToString()) ? (string)reader["MDT"] : string.Empty,
                                ResourceEnabled = !string.IsNullOrEmpty(reader["VRE"].ToString()) && (bool)reader["VRE"],
                                ResourceFolderId = !string.IsNullOrEmpty(reader["VRFID"].ToString()) ? (Guid?)reader["VRFID"] : (Guid?)null,
                                ResourceId = !string.IsNullOrEmpty(reader["VRID"].ToString()) ? (Guid)reader["VRID"] : Guid.Empty,
                                ResourceName = !string.IsNullOrEmpty(reader["VRN"].ToString()) ? (string)reader["VRN"] : string.Empty,
                                ResourceType = !string.IsNullOrEmpty(reader["VRT"].ToString()) ? (string)reader["VRT"] : string.Empty,
                                ScenarioFolderId = !string.IsNullOrEmpty(reader["ESFID"].ToString()) ? (Guid?)reader["ESFID"] : (Guid?)null,
                                ScenarioId = (Guid)reader["ESID"],
                                ScenarioName = (string)reader["ESN"],
                            });
                        }
                    }
                }

                nodes.AddRange
                    (scenarioData
                        .Where(x => x.ScenarioId != (Guid?)null && x.ScenarioId != Guid.Empty)
                        .GroupBy(x => x.ScenarioId)
                        .Select(x => 
                            new EnterpriseTestMapNode(x.First().ScenarioId)
                            {
                                NodeType = ConfigurationObjectType.EnterpriseScenario,
                                Name = x.First().ScenarioName,
                                FolderId = x.First().ScenarioFolderId
                            })
                    );

                nodes.AddRange
                    (scenarioData
                        .Where(x => x.ResourceId != (Guid?)null && x.ResourceId != Guid.Empty)
                        .GroupBy(x => x.ResourceId)
                        .Select(x =>
                            new EnterpriseTestMapNode(x.First().ResourceId)
                            {
                                NodeType = ConfigurationObjectType.VirtualResource,
                                Name = x.First().ResourceName,
                                FolderId = x.First().ResourceFolderId,
                                ParentId = x.First().ScenarioId,
                                Enabled = x.First().ResourceEnabled,
                                ResourceType = x.First().ResourceType
                            })
                    );

                nodes.AddRange
                    (scenarioData
                        .Where(x => x.MetadataId != (Guid?)null && x.MetadataId != Guid.Empty)
                        .GroupBy(x => x.MetadataId)
                        .Select(x =>
                            new EnterpriseTestMapNode(x.First().MetadataId)
                            {
                                NodeType = ConfigurationObjectType.ResourceMetadata,
                                Name = x.First().MetadataName,
                                FolderId = x.First().MetadataFolderId,
                                ParentId = x.First().ResourceId,
                                Enabled = x.First().MetadataEnabled,
                                ResourceType = x.First().ResourceType,
                                MetadataType = x.First().MetadataType
                            })
                    );

                // Load the folders based on the test objects that have already been loaded
                if (needFilter)
                {
                    LoadFolders(context, nodes);
                }
                else
                {
                    foreach (ConfigurationTreeFolder folder in ConfigurationTreeFolder.Select(context))
                    {
                        nodes.Add(new EnterpriseTestMapNode(folder));
                    }
                }
            }

            return nodes;
        }

        private static void LoadFolders(EnterpriseTestContext context, List<EnterpriseTestMapNode> nodes)
        {
            List<ConfigurationTreeFolder> remainingFolders = ConfigurationTreeFolder.Select(context).ToList();
            List<ConfigurationTreeFolder> foldersToAdd = new List<ConfigurationTreeFolder>();
            int lastCount = -1;

            // This process must be iterative, since folders can contain other folders.
            // Keep looping over the folders until none of them got processed - then we're done
            while (remainingFolders.Count != lastCount)
            {
                lastCount = remainingFolders.Count;

                // For all of the remaining folders, add any that we know we can add now
                foreach (ConfigurationTreeFolder folder in remainingFolders)
                {
                    // If something needs to go in this folder, add it
                    if (nodes.Any(n => n.ParentId == folder.ConfigurationTreeFolderId ||
                                       n.FolderId == folder.ConfigurationTreeFolderId))
                    {
                        foldersToAdd.Add(folder);
                    }
                    // If this is not a scenario folder, but its parent exists, add it
                    else if (folder.FolderType != "ScenarioFolder" &&
                             nodes.Any(n => n.Id == folder.ParentId))
                    {
                        foldersToAdd.Add(folder);
                    }
                }

                // Add nodes for all of the folders to add, then remove them from the remaining folder list
                foreach (ConfigurationTreeFolder folder in foldersToAdd)
                {
                    nodes.Add(new EnterpriseTestMapNode(folder));
                    remainingFolders.Remove(folder);
                }
                foldersToAdd.Clear();
            }
        }

        /// <summary>
        /// Gets a list of nodes that have been topologically sorted;
        /// i.e. each node's Parent and Folder come before it in the list.
        /// </summary>
        public IEnumerable<EnterpriseTestMapNode> GetSortedNodes()
        {
            List<EnterpriseTestMapNode> sorted = new List<EnterpriseTestMapNode>();

            // Because of the strict hierarchy, we can segregate the nodes into groups
            // by type, sort the groups, and then add the groups in order.
            List<ConfigurationObjectType> orderedTypes = new List<ConfigurationObjectType>()
            {
                ConfigurationObjectType.ScenarioFolder,
                ConfigurationObjectType.EnterpriseScenario,
                ConfigurationObjectType.ResourceFolder,
                ConfigurationObjectType.VirtualResource,
                ConfigurationObjectType.MetadataFolder,
                ConfigurationObjectType.ResourceMetadata
            };

            foreach (ConfigurationObjectType mapType in orderedTypes)
            {
                // Get all the nodes of that type
                var nodesOfType = _nodes.Where(n => n.NodeType == mapType).ToList();
                sorted.AddRange(Sort(nodesOfType));
            }

            return sorted;
        }

        private static IEnumerable<EnterpriseTestMapNode> Sort(IEnumerable<EnterpriseTestMapNode> nodes)
        {
            // The scenarios, resources, and metadata require no special sorting.
            EnterpriseTestMapNode first = nodes.FirstOrDefault();
            if (first == null || !first.NodeType.IsFolder())
            {
                return nodes;
            }

            // Otherwise, we have a folder set
            List<EnterpriseTestMapNode> remaining = new List<EnterpriseTestMapNode>(nodes);
            List<EnterpriseTestMapNode> sorted = new List<EnterpriseTestMapNode>();

            // We need to make sure that any subfolders come after their parent folder.
            // Since this will be a relatively small set, we'll use a simple (though inefficient) approach.
            while (remaining.Count > 0)
            {
                // Create a list of the node IDs that have not been processed yet.
                IEnumerable<Guid> remainingIds = remaining.Select(n => n.Id);

                // Find all nodes whose parents are not in the "unprocessed" list.
                // Their parents must either be processed already, or some other type (scenario, etc.)
                // so these nodes can be moved to the sorted list.
                var readyNodes = remaining.Where(n => n.ParentId == null || !remainingIds.Contains((Guid)n.ParentId));
                foreach (EnterpriseTestMapNode ready in readyNodes.ToList())
                {
                    remaining.Remove(ready);
                    sorted.Add(ready);
                }
            }

            return sorted;
        }
    }
}
