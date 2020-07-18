using System;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Core.EnterpriseTest.Configuration
{
    /// <summary>
    /// Manages changes to the configuration objects in the Enterprise Test database,
    /// while maintaining an in-memory map of the objects to minimize database operations.
    /// </summary>
    public sealed class EnterpriseTestConfigController : IDisposable
    {
        private readonly EnterpriseTestConnectionString _connectionString;
        private readonly EnterpriseTestConfigMap _configMap = new EnterpriseTestConfigMap();
        private EnterpriseTestContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestConfigController" /> class.
        /// </summary>
        /// <param name="connectionString">The <see cref="EnterpriseTestConnectionString" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="connectionString" /> is null.</exception>
        public EnterpriseTestConfigController(EnterpriseTestConnectionString connectionString)
        {
            _connectionString = connectionString ?? throw new ArgumentNullException(nameof(connectionString));
        }

        /// <summary>
        /// Loads all configuration objects from the database.
        /// </summary>
        public ConfigurationObjectChangeSet LoadConfigurationObjects()
        {
            return LoadConfigurationMap(n => n.EnterpriseScenarios);
        }

        /// <summary>
        /// Loads configuration objects accessible by the specified user from the database.
        /// </summary>
        /// <param name="userName">The user whose accessible configuration objects should be loaded.</param>
        public ConfigurationObjectChangeSet LoadConfigurationObjects(string userName)
        {
            // Only allow scenarios that are owned by the user OR accessible to a group the user is in
            return LoadConfigurationMap(n => from scenario in n.EnterpriseScenarios
                                             let groupUsers = scenario.UserGroups.SelectMany(m => m.Users).Select(m => m.UserName)
                                             where scenario.Owner == userName || groupUsers.Contains(userName)
                                             select scenario);
        }

        private ConfigurationObjectChangeSet LoadConfigurationMap(Func<EnterpriseTestContext, IQueryable<EnterpriseScenario>> scenarioSelector)
        {
            _configMap.Clear();

            using (EnterpriseTestContext context = new EnterpriseTestContext(_connectionString))
            {
                // Set up queries for scenarios, resources, and metadata
                IQueryable<EnterpriseScenario> scenarios = scenarioSelector(context);
                IQueryable<VirtualResource> resources = scenarios.SelectMany(n => n.VirtualResources);
                IQueryable<VirtualResourceMetadata> metadata = resources.SelectMany(n => n.VirtualResourceMetadataSet);

                // Create configuration objects from the above queries
                List<ConfigurationObjectTag> scenarioTags = scenarios.ToConfigurationObjectTags().ToList();
                List<ConfigurationObjectTag> resourceTags = resources.ToConfigurationObjectTags().ToList();
                List<ConfigurationObjectTag> metadataTags = metadata.ToConfigurationObjectTags().ToList();

                // The configuration tags need to be added in order, such that every tag comes *after* its parent and folder.
                // We can take advantage of the strict object hierarchy to minimize the amount of sorting required.
                // Scenarios, resources, and metadata require no particular order, since they are independent of each other.
                // Folders must be sorted to ensure that subfolders are added after their parent folder.
                LoadSortedConfigFolders(context, ConfigurationObjectType.ScenarioFolder, Enumerable.Empty<ConfigurationObjectTag>());
                _configMap.AddRange(scenarioTags);
                LoadSortedConfigFolders(context, ConfigurationObjectType.ResourceFolder, scenarioTags);
                _configMap.AddRange(resourceTags);
                LoadSortedConfigFolders(context, ConfigurationObjectType.MetadataFolder, resourceTags);
                _configMap.AddRange(metadataTags);
            }

            ConfigurationObjectChangeSet changeSet = new ConfigurationObjectChangeSet();
            changeSet.AddedObjects.Add(_configMap.AllObjects);
            return changeSet;
        }

        private void LoadSortedConfigFolders(EnterpriseTestContext context, ConfigurationObjectType folderType, IEnumerable<ConfigurationObjectTag> parentTags)
        {
            List<ConfigurationTreeFolder> folders = context.ConfigurationTreeFolders.Where(n => n.FolderType == folderType.ToString()).ToList();
            Dictionary<Guid, VirtualResourceType> parentResourceTypes = parentTags.ToDictionary(n => n.Id, n => n.ResourceType);

            // Loop over folders collection until we reach a point where no folders are added.
            // The remainder of the folders don't have parents in the tree and will be ignored.
            bool foundFoldersToAdd;
            do
            {
                foundFoldersToAdd = false;

                // While loop used here so that we can modify the collection as we iterate
                int i = 0;
                while (i < folders.Count)
                {
                    ConfigurationTreeFolder folder = folders[i];

                    // Determine whether or not we can create a tag for this folder right now
                    ConfigurationObjectTag folderTag = null;
                    if (folder.ParentId == null)
                    {
                        // Folder has no parent - add it now
                        folderTag = new ConfigurationObjectTag(folder);
                    }
                    else
                    {
                        // Check to see if parent has already been added
                        if (parentResourceTypes.TryGetValue(folder.ParentId.Value, out VirtualResourceType resourceType))
                        {
                            // Parent has been added, so this folder can be added now
                            folderTag = new ConfigurationObjectTag(folder, resourceType);
                        }
                    }

                    // If we were able to create a tag, add it to the new list and remove it from the old
                    if (folderTag != null)
                    {
                        foundFoldersToAdd = true;
                        _configMap.Add(folderTag);
                        parentResourceTypes.Add(folderTag.Id, folderTag.ResourceType);
                        folders.RemoveAt(i);
                    }
                    else
                    {
                        // Cannot add this folder - move to the next one
                        i++;
                    }
                }
            } while (foundFoldersToAdd);
        }

        /// <summary>
        /// Gets an <see cref="EnterpriseTestContext" /> managed by this controller that can be used for editing configuration data.
        /// </summary>
        /// <returns>An <see cref="EnterpriseTestContext" /> managed by this controller.</returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Design", "CA1024:UsePropertiesWhereAppropriate", Justification = "Method creates a new object each time it is called.")]
        public EnterpriseTestContext GetEditContext()
        {
            // Dispose of the old context and create a new one.
            // This ensures that any changes or cached data are forgotten.
            _context?.Dispose();

            _context = new EnterpriseTestContext(_connectionString);
            return _context;
        }

        /// <summary>
        /// Commits all changes to this controller's managed <see cref="EnterpriseTestContext" />.
        /// </summary>
        public void CommitChanges()
        {
            _context?.SaveChanges();
        }

        /// <summary>
        /// Discards all pending changes to this controller's managed <see cref="EnterpriseTestContext" />.
        /// </summary>
        public void DiscardChanges()
        {
            // Dispose of the old context - a new one will be created when needed
            if (_context != null)
            {
                _context.Dispose();
                _context = null;
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
