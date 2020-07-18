using System;
using System.Collections.Generic;
using System.Data.Objects.DataClasses;
using System.Linq;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    public partial class ResourceWindowsCategory
    {
        public const string InstanceDoesNotApply = "N/A";

        /// <summary>
        /// Selects ResourceWindowsCategory for the given category type.
        /// </summary>
        /// <param name="entities">The data context.</param>
        /// <param name="categoryType">The category type.</param>
        /// <returns></returns>
        public static IQueryable<ResourceWindowsCategory> Select(EnterpriseTestEntities entities, string categoryType)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from c in entities.ResourceWindowsCategories.Include("Children")
                    where c.CategoryType == categoryType orderby(c.CategoryType)
                    select c);
        }

        public static ResourceWindowsCategory SelectByName(EnterpriseTestEntities entities, string name, string categoryType)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from c in entities.ResourceWindowsCategories
                    where c.Name == name && c.CategoryType == categoryType
                    select c).First();
        }

        public static ResourceWindowsCategory SelectById(EnterpriseTestEntities entities, int id)
        {

            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from c in entities.ResourceWindowsCategories
                    where c.CategoryId == id
                    select c).FirstOrDefault();
        }

        public static List<ResourceWindowsCategory> SelectByParent(EnterpriseTestEntities entities, int parentId)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            ResourceWindowsCategory parent = entities.ResourceWindowsCategories.Where(p => p.CategoryId == parentId).FirstOrDefault();

            if (parent != null)
            {
                return parent.Children.OrderBy(x => x.CategoryType).ToList();
            }

            return null;
        }

        public static IQueryable<ResourceWindowsCategory> SelectParent(EnterpriseTestEntities entities, string categoryType)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from c in entities.ResourceWindowsCategories
                    where c.CategoryType == categoryType && c.Parents.Count == 0
                    select c);
        }


        public static void RemoveChild(EnterpriseTestEntities entities, int parentId, int childId)
        {
            if(entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            ResourceWindowsCategory componentToRemove = ResourceWindowsCategory.SelectById(entities, childId);
            EntityCollection<ResourceWindowsCategory> parentList = componentToRemove.Parents;
            int count = 0;
            int deleteIndex = 0;
           
            foreach(ResourceWindowsCategory parent in parentList)
            {
                if(parent.CategoryId == parentId)
                {
                    deleteIndex = count;
                }
                count++;
            }

            componentToRemove.Parents.Remove(componentToRemove.Parents.ElementAt(deleteIndex));
            if(componentToRemove.Parents.Count < 1)
            {
                entities.DeleteObject(componentToRemove);
                entities.SaveChanges();
            }
        }

        /// <summary>
        /// Selects ResourceWindowsCategory for the given category type and category name.
        /// </summary>
        /// <param name="entities">The data context.</param>
        /// <param name="categoryType">The category name.</param>
        /// <param name="categoryName">The instance name.</param>
        /// <returns></returns>
        public static IQueryable<ResourceWindowsCategory> Select(EnterpriseTestEntities entities, string categoryType, string categoryName)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from c in entities.ResourceWindowsCategories.Include("Children")
                    where c.CategoryType.Equals(categoryType, StringComparison.InvariantCultureIgnoreCase) && c.Name.Equals(categoryName, StringComparison.InvariantCultureIgnoreCase)
                    select c);
        }


        /// <summary>
        /// Selects a disctinct list of Category types
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IQueryable<String> SelectDistinctCategoryTypes(EnterpriseTestEntities entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from c in entities.ResourceWindowsCategories
                    select c.CategoryType).Distinct();
        }


        /// <summary>
        /// Adds a new set of PerfMon counters.
        /// </summary>
        /// <param name="entities">The data context.</param>
        /// <param name="categoryName">The category name.</param>
        /// <param name="instanceName">The instance name.</param>
        /// <param name="counterName">The counter name.</param>
        public static void AddPerfMon(EnterpriseTestEntities entities, string categoryName, string instanceName, string counterName, ResourceWindowsCategoryType categoryType)
        {
            string categoryTypeName = categoryType.ToString();
            ResourceWindowsCategory category = null;
            ResourceWindowsCategory instance = null;
            ResourceWindowsCategory counter = Select(entities, categoryTypeName, counterName).FirstOrDefault();

            if (counter == null)
            {
                counter = ResourceWindowsCategory.CreateResourceWindowsCategory(NextId(entities), counterName, categoryTypeName);
            }

            //If instance is blank, use the constant string.  This is because in the ResourceWindowsCategory table the blank category is the root, 
            //so we can't reuse it further down in the hierarchy.
            instance = GetParent(entities, instanceName == string.Empty ? InstanceDoesNotApply : instanceName, counter, categoryTypeName);
            category = GetParent(entities, categoryName, instance, categoryTypeName);

            //Wire up the parent relationships so that the EF can handle it.
            if (counter.EntityState == System.Data.EntityState.Detached)
            {
                counter.Parents.Add(instance);
            }
            if (instance.EntityState == System.Data.EntityState.Detached)
            {
                instance.Parents.Add(category);
            }
            if (category.EntityState == System.Data.EntityState.Detached)
            {
                //We're adding a new category, so hook it up the the root parent
                ResourceWindowsCategory root = Select(entities, categoryTypeName, string.Empty).FirstOrDefault();
                if (root != null)
                {
                    category.Parents.Add(root);
                }
            }

        }

        public static int AddResource(EnterpriseTestEntities entities, string name, string categoryType)
        {
            int doesNotExist = -1;
            int id = ResourceWindowsCategory.Exists(entities, name, categoryType);
            if(id == doesNotExist)
            {
                ResourceWindowsCategory rwc = new ResourceWindowsCategory();
                rwc.Name = name;
                rwc.CategoryType = categoryType;
                entities.AddToResourceWindowsCategories(rwc);
                entities.SaveChanges();
                ResourceWindowsCategory newRWC = SelectByName(entities, rwc.Name, categoryType);
                return newRWC.CategoryId;
            }
            else
            {
                return id;
            }
        }


        private static int Exists(EnterpriseTestEntities entities, string name, string categoryType)
        {
            int doesNotExist = -1;   
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            try
            {
                ResourceWindowsCategory component = ResourceWindowsCategory.SelectByName(entities, name, categoryType);
                return component.CategoryId;
            }
            catch
            {
                return doesNotExist;
            }
        }

        /// <summary>
        /// Gets the next Id for ResourceWindowsCategory.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        private static int NextId(EnterpriseTestEntities entities)
        {
            int maxId = -1;

            maxId = (from c in entities.ResourceWindowsCategories
                     select c.CategoryId).Max();

            return maxId + 1;
        }

        /// <summary>
        /// Returns the parent ResourceWindowsCategory for the passed-in ResourceWindowsCategory item.
        /// </summary>
        /// <param name="entities"></param>
        /// <param name="name"></param>
        /// <param name="item"></param>
        /// <returns></returns>
        private static ResourceWindowsCategory GetParent(EnterpriseTestEntities entities, string name, ResourceWindowsCategory item, string categoryType)
        {
            //Try to find the parent in the item's Parent collection
            ResourceWindowsCategory result = item.Parents.Where(p => p.Name.Equals(name, StringComparison.OrdinalIgnoreCase)).FirstOrDefault();

            if (result == null)
            {
                //Not in the item's parent collection.  Try to find the parent in the table
                result = Select(entities, categoryType, name).FirstOrDefault();
            }

            if (result == null)
            {
                //Not in the item's parent collection or the database.  Create a new item
                result = ResourceWindowsCategory.CreateResourceWindowsCategory(NextId(entities), name, categoryType);
            }

            return result;
        }
    }

    /// <summary>
    /// Enumeration for category types
    /// </summary>
    public enum ResourceWindowsCategoryType
    {
        /// <summary>
        /// PerfMon
        /// </summary>
        PerfMon,

        /// <summary>
        /// EventLog
        /// </summary>
        EventLog,

        /// <summary>
        /// Session Name
        /// </summary>
        SessionName,

        /// <summary>
        /// Session Cycle
        /// </summary>
        SessionCycle,

        /// <summary>
        /// Session Type
        /// </summary>
        SessionType
    }
}
