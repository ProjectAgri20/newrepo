using System;
using System.Linq;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    public partial class MetadataType
    {
        private string _resourceTypes = string.Empty;

        /// <summary>
        /// Returns a distinct list of all groups associated to all MetadataTypes.
        /// </summary>
        /// <param name="entities"></param>
        /// <returns></returns>
        public static IQueryable<string> SelectGroups(EnterpriseTestEntities entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return 
                (
                    from n in entities.MetadataTypes
                    where !string.IsNullOrEmpty(n.Group)
                    orderby n.Group
                    select n.Group
                ).Distinct();
        }

        /// <summary>
        /// Selects the name of the given assembly.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="name">The name of the assembly</param>
        /// <returns>AssemblyName string if found, empty string otherwise.</returns>
        /// <exception cref="System.ArgumentNullException">entities</exception>
        /// <exception cref="System.InvalidOperationException">If more than one assembly matches name.</exception>
        public static string SelectAssemblyName(EnterpriseTestEntities entities, string name)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return
                (from n in entities.MetadataTypes
                 where n.Name == name
                 select n.AssemblyName).SingleOrDefault() ?? string.Empty;
        }

        

        /// <summary>
        /// Returns "Group/Name" string.
        /// </summary>
        public string PromptTitle
        {
            get { return !string.IsNullOrEmpty(Group) ? "{0}/{1}".FormatWith(Group, Name) : Name; }
        }

        public void JoinTypes()
        {
            _resourceTypes = string.Join(", ", ResourceTypes.Select(e => e.Name));
        }

        /// <summary>
        /// Returns the ResourceType as a string.
        /// </summary>
        public string ResourceTypesString
        {
            get
            {
                if (string.IsNullOrEmpty(_resourceTypes))
                {
                    JoinTypes();
                }
                return _resourceTypes;
            }
        }
    }
}
