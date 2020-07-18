using System;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    /// <summary>
    /// Represents a product version that can be associated with a scenario. 
    /// </summary>
    public partial class AssociatedProductVersion
    {
        /// <summary>
        /// Selects <see cref="AssociatedProductVersion"/>s a set of versions for a given set of products, if none exist, they are added and a list is containing them is returned. 
        /// </summary>
        /// <param name="entities">The entities.</param>
        public static List<AssociatedProductVersion> SelectVersions(EnterpriseTestEntities context, IEnumerable<Guid> products,   Guid scenarioGuid)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            List<AssociatedProductVersion> associatedProductVersions = new List<AssociatedProductVersion>();
            var temp = context.AssociatedProductVersions.Where(y => products.Contains(y.AssociatedProductId) && y.EnterpriseScenarioId == scenarioGuid).Select(y => y.AssociatedProductId);

            var preExisting = products.Intersect(temp);
            var newVersionGuids = products.Except(temp);

            if (newVersionGuids.Count() > 0)
            {
                foreach (var ver in newVersionGuids)
                {
                    AssociatedProductVersion version = new AssociatedProductVersion();
                    version.Active = true;
                    version.AssociatedProductId = ver;
                    version.EnterpriseScenarioId = scenarioGuid;
                    version.Version = string.Empty;
                    context.AssociatedProductVersions.AddObject(version);
                    context.SaveChanges();
                    associatedProductVersions.Add(version);
                }
            }
            

            if (preExisting.Count() > 0)
            {
                var items = context.AssociatedProductVersions.Where(y => preExisting.Contains(y.AssociatedProductId) && y.EnterpriseScenarioId == scenarioGuid);
                associatedProductVersions.AddRange(items);
            }

            return associatedProductVersions; 
        }
    }
}
