using System;
using System.Collections.Generic;
using System.Linq;

namespace HP.ScalableTest.Core.EnterpriseTest.Configuration
{
    /// <summary>
    /// Creates representative <see cref="ConfigurationObjectTag" /> objects from configuration object queries.
    /// </summary>
    public static class ConfigurationObjectTagBuilder
    {
        /// <summary>
        /// Builds <see cref="ConfigurationObjectTag" /> objects from an <see cref="EnterpriseScenario" /> query.
        /// </summary>
        /// <param name="scenarioQuery">The <see cref="EnterpriseScenario" /> query.</param>
        /// <returns>A collection of <see cref="ConfigurationObjectTag" /> objects.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="scenarioQuery" /> is null.</exception>
        public static IEnumerable<ConfigurationObjectTag> ToConfigurationObjectTags(this IQueryable<EnterpriseScenario> scenarioQuery)
        {
            if (scenarioQuery == null)
            {
                throw new ArgumentNullException(nameof(scenarioQuery));
            }

            return scenarioQuery.Select(n => new ConfigurationObjectQueryResult
            {
                Id = n.EnterpriseScenarioId,
                ObjectType = ConfigurationObjectType.EnterpriseScenario,
                Name = n.Name,
                FolderId = n.FolderId
            })
            .AsEnumerable()
            .Select(n => n.BuildConfigurationObjectTag());
        }

        /// <summary>
        /// Builds <see cref="ConfigurationObjectTag" /> objects from an <see cref="VirtualResource" /> query.
        /// </summary>
        /// <param name="resourceQuery">The <see cref="VirtualResource" /> query.</param>
        /// <returns>A collection of <see cref="ConfigurationObjectTag" /> objects.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="resourceQuery" /> is null.</exception>
        public static IEnumerable<ConfigurationObjectTag> ToConfigurationObjectTags(this IQueryable<VirtualResource> resourceQuery)
        {
            if (resourceQuery == null)
            {
                throw new ArgumentNullException(nameof(resourceQuery));
            }

            return resourceQuery.Select(n => new ConfigurationObjectQueryResult
            {
                Id = n.VirtualResourceId,
                ObjectType = ConfigurationObjectType.VirtualResource,
                ResourceType = n.ResourceType,
                Name = n.Name,
                ParentId = n.EnterpriseScenarioId,
                FolderId = n.FolderId,
                Enabled = n.Enabled
            })
            .AsEnumerable()
            .Select(n => n.BuildConfigurationObjectTag());
        }

        /// <summary>
        /// Builds <see cref="ConfigurationObjectTag" /> objects from an <see cref="VirtualResourceMetadata" /> query.
        /// </summary>
        /// <param name="metadataQuery">The <see cref="VirtualResourceMetadata" /> query.</param>
        /// <returns>A collection of <see cref="ConfigurationObjectTag" /> objects.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="metadataQuery" /> is null.</exception>
        public static IEnumerable<ConfigurationObjectTag> ToConfigurationObjectTags(this IQueryable<VirtualResourceMetadata> metadataQuery)
        {
            if (metadataQuery == null)
            {
                throw new ArgumentNullException(nameof(metadataQuery));
            }

            return metadataQuery.Select(n => new ConfigurationObjectQueryResult
            {
                Id = n.VirtualResourceMetadataId,
                ObjectType = ConfigurationObjectType.VirtualResourceMetadata,
                ResourceType = n.ResourceType,
                MetadataType = n.MetadataType,
                Name = n.Name,
                ParentId = n.VirtualResourceId,
                FolderId = n.FolderId,
                Enabled = n.Enabled
            })
            .AsEnumerable()
            .Select(n => n.BuildConfigurationObjectTag());
        }

        /// <summary>
        /// Helper class for storing EF query results and then building a <see cref="ConfigurationObjectTag" />.
        /// </summary>
        private sealed class ConfigurationObjectQueryResult
        {
            public Guid Id { get; set; }
            public ConfigurationObjectType ObjectType { get; set; }
            public string ResourceType { get; set; }
            public string MetadataType { get; set; }
            public string Name { get; set; }
            public Guid? ParentId { get; set; }
            public Guid? FolderId { get; set; }
            public bool? Enabled { get; set; }

            public ConfigurationObjectTag BuildConfigurationObjectTag()
            {
                return new ConfigurationObjectTag(Id, ObjectType, ParseResourceType(), MetadataType)
                {
                    Name = Name,
                    ParentId = ParentId,
                    FolderId = FolderId,
                    EnabledState = GetEnabledState()
                };
            }

            private VirtualResourceType ParseResourceType()
            {
                VirtualResourceType parsedResourceType = VirtualResourceType.None;
                if (!string.IsNullOrEmpty(ResourceType))
                {
                    Enum.TryParse(ResourceType, out parsedResourceType);
                }
                return parsedResourceType;
            }

            private EnabledState GetEnabledState()
            {
                if (Enabled == null)
                {
                    return EnabledState.NotApplicable;
                }
                else
                {
                    return Enabled == true ? EnabledState.Enabled : EnabledState.Disabled;
                }
            }
        }
    }
}
