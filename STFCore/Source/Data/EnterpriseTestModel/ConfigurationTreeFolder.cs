using System;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    public partial class ConfigurationTreeFolder
    {
        /// <summary>
        /// Selects all <see cref="ConfigurationTreeFolder"/>s.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IQueryable<ConfigurationTreeFolder> Select(EnterpriseTestEntities entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return from n in entities.ConfigurationTreeFolders
                   select n;
        }

        /// <summary>
        /// Selects a <see cref="ConfigurationTreeFolder"/> by its id.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static ConfigurationTreeFolder Select(EnterpriseTestEntities entities, Guid id)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from n in entities.ConfigurationTreeFolders
                    where n.ConfigurationTreeFolderId == id
                    select n).FirstOrDefault();
        }

        /// <summary>
        /// Selects all <see cref="ConfigurationTreeFolder"/> objects with ids in the specified list.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public static IQueryable<ConfigurationTreeFolder> Select(EnterpriseTestEntities entities, IEnumerable<Guid> ids)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return from n in entities.ConfigurationTreeFolders
                   where ids.Contains(n.ConfigurationTreeFolderId)
                   select n;
        }

        /// <summary>
        /// Selects all <see cref="ConfigurationTreeFolder"/> objects of the specified type, sorted hierarchically.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public static IQueryable<ConfigurationTreeFolder> Select(EnterpriseTestEntities entities, string type)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return from n in entities.ConfigurationTreeFolders
                          where n.FolderType == type
                          select n;
        }

        // We don't use this constructor, but entity framework does
        internal ConfigurationTreeFolder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationTreeFolder"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        /// <param name="folderType">Type of the folder.</param>
        /// <param name="parentId">The parent id.</param>
        public ConfigurationTreeFolder(Guid id, string name, string folderType, Guid? parentId)
        {
            this.ConfigurationTreeFolderId = id;
            this.Name = name;
            this.FolderType = folderType;
            this.ParentId = parentId;
        }

        /// <summary>
        /// Selects all the ScenarioIds that are children of the specified folder Id.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="rootFolderId"></param>
        /// <returns>An enumerable collection of ScenarioIds.</returns>
        public static IEnumerable<Guid> ContainedScenarioIds(EnterpriseTestEntities entities, Guid rootFolderId)
        {
            List<Guid> scenarioIds = new List<Guid>();

            // Find all the scenarios that are direct children of this one
            scenarioIds.AddRange(entities.EnterpriseScenarios.Where(n => n.FolderId == rootFolderId).Select(n => n.EnterpriseScenarioId));

            // Find all the folders that are direct children of this one
            var folderIds = entities.ConfigurationTreeFolders.Where(n => n.ParentId == rootFolderId).Select(n => n.ConfigurationTreeFolderId);
            foreach (Guid folderId in folderIds)
            {
                scenarioIds.AddRange(ContainedScenarioIds(entities, folderId));
            }

            return scenarioIds;
        }

        /// <summary>
        /// Selects the specified <see cref="ConfigurationTreeFolder"/>s sorted in hierarchical order.
        /// </summary>
        /// <param name="folders"></param>
        /// <returns></returns>
        public static IEnumerable<ConfigurationTreeFolder> SortHierarchical(IEnumerable<ConfigurationTreeFolder> folders)
        {
            // We need to make sure that any subfolders come after their parent folder.
            // Since this will be a relatively small set, we'll use a simple (though inefficient) approach.
            List<ConfigurationTreeFolder> remaining = new List<ConfigurationTreeFolder>(folders);
            List<ConfigurationTreeFolder> sorted = new List<ConfigurationTreeFolder>();
            while (remaining.Any())
            {
                // Create a list of the folder IDs that have not been processed yet
                IEnumerable<Guid> remainingIds = remaining.Select(n => n.ConfigurationTreeFolderId);

                // Find all nodes whose parents are not in the "unprocessed" list.
                // Their parents must either be processed already,
                // so these nodes can be moved to the sorted list.
                var ready = remaining.Where(n => n.ParentId == null || !remainingIds.Contains((Guid)n.ParentId));
                foreach (ConfigurationTreeFolder folder in ready.ToList())
                {
                    remaining.Remove(folder);
                    sorted.Add(folder);
                }
            }

            return sorted;
        }
    }
}
