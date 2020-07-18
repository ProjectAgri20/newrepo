using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Security;
using System.Text;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Data.EnterpriseTest.Model
{
    public partial class EnterpriseScenario
    {
        /// <summary>
        /// Selects an <see cref="EnterpriseScenario"/> by its id.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="scenarioId">The scenario Id.</param>
        /// <returns></returns>
        public static EnterpriseScenario Select(EnterpriseTestEntities entities, Guid scenarioId)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from n in entities.EnterpriseScenarios
                    where n.EnterpriseScenarioId == scenarioId
                    select n).FirstOrDefault();
        }

        /// <summary>
        /// Selects an <see cref="EnterpriseScenario"/> by Scenario name.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="scenarioName">Name of the scenario.</param>
        /// <returns></returns>
        /// <exception cref="System.ArgumentNullException">entities</exception>
        public static EnterpriseScenario Select(EnterpriseTestEntities entities, string scenarioName)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return
                (
                    from n in entities.EnterpriseScenarios.Include("VirtualResources.VirtualResourceMetadataSet")
                    where n.Name.Equals(scenarioName, StringComparison.OrdinalIgnoreCase)
                    select n
                ).FirstOrDefault();
        }

        /// <summary>
        /// Selects all <see cref="EnterpriseScenario"/> objects with ids in the specified list.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="ids">The ids.</param>
        /// <returns></returns>
        public static IQueryable<EnterpriseScenario> Select(EnterpriseTestEntities entities, IEnumerable<Guid> ids)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return from n in entities.EnterpriseScenarios
                   where ids.Contains(n.EnterpriseScenarioId)
                   orderby n.Name ascending
                   select n;
        }

        /// <summary>
        /// Selects an <see cref="EnterpriseScenario"/> by its id, including all children.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="id">The id.</param>
        /// <returns></returns>
        public static EnterpriseScenario SelectWithAllChildren(EnterpriseTestEntities entities, Guid id)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from n in entities.EnterpriseScenarios
                    .Include("VirtualResources.VirtualResourceMetadataSet")
                    .Include("UserGroups")
                    where n.EnterpriseScenarioId == id
                    select n).FirstOrDefault();
        }

        /// <summary>
        /// Selects the distinct companies from all <see cref="EnterpriseScenario"/>s.
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <returns></returns>
        public static IQueryable<string> SelectDistinctCompany(EnterpriseTestEntities entities)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            return (from n in entities.EnterpriseScenarios
                    where n.Company != null
                    select n.Company).Distinct();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseScenario"/> class.
        /// </summary>
        public EnterpriseScenario()
        {
            EnterpriseScenarioId = SequentialGuid.NewGuid();
            Name = "New Scenario " + DateTime.Now.ToString("yyyyMMddHHmm", CultureInfo.CurrentCulture);
        }

        /// <summary>
        /// Adds groups to this scenario based on the given user name
        /// </summary>
        /// <param name="entities">The entities.</param>
        /// <param name="userName">Name of the user.</param>
        /// <exception cref="System.ArgumentNullException">entities</exception>
        public void AddGroups(EnterpriseTestEntities entities, string userName)
        {
            if (entities == null)
            {
                throw new ArgumentNullException("entities");
            }

            var query =
                (
                    from g in entities.UserGroups
                    from u in g.Users
                    where u.UserName.Equals(userName, StringComparison.OrdinalIgnoreCase)
                    select g
                );

            UserGroups.Clear();
            foreach (var item in query)
            {
                UserGroups.Add(item);
            }
        }

        /// <summary>
        /// Returns a <see cref="string"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="string"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Name;
        }

        /// <summary>
        /// Gets the estimated run time of this scenario.
        /// </summary>
        public int EstimatedRuntime
        {
            get
            {
                int longestTime = 0;

                foreach (VirtualResource resource in VirtualResources.Where(n => n.Enabled == true))
                {
                    int runtimeInHours = (int)Math.Ceiling(resource.Runtime.TotalHours);

                    longestTime = Math.Max(longestTime, runtimeInHours);
                }

                if (longestTime == 0)
                {
                    // No time was calculated for test duration, which means it must be an iterative test.  
                    // This means that devices would be reserved for 1 hour since testers usually don't pay attention 
                    // to this value in the startup wizard, which ends up biting them.
                    // Defaulting to a higher value is better than defaulting to zero.
                    longestTime = 72;
                }

                // Add an hour for setup and so forth
                return longestTime + 1;
            }
        }

        /// <summary>
        /// Checks the integrity of the data for this scenario.
        /// </summary>
        /// <returns>A collection of strings, each of which provides information about a data integrity issue.</returns>
        public IEnumerable<string> ValidateData()
        {
            // Check to see if there are any resources
            if (!this.VirtualResources.Any())
            {
                yield return "Scenario contains no resources.";
            }
            else
            {
                // Check to see if there are any enabled resources
                IEnumerable<VirtualResource> enabledResources = this.VirtualResources.Where(n => n.Enabled);

                if (!enabledResources.Any())
                {
                    yield return "Scenario contains no enabled resources.";
                }
                else
                {
                    // Validate all enabled resources
                    foreach (string issue in enabledResources.SelectMany(n => n.ValidateData()))
                    {
                        yield return issue;
                    }
                }
            }
        }
    }
}
