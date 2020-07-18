using System;
using System.Diagnostics;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.EnterpriseTest.Configuration
{
    /// <summary>
    /// A lightweight class containing key properties of a configuration object, such as a scenario or virtual resource.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public sealed class ConfigurationObjectTag
    {
        /// <summary>
        /// Gets the unique ID of the configuration object.
        /// </summary>
        public Guid Id { get; }

        /// <summary>
        /// Gets the <see cref="ConfigurationObjectType" />.
        /// </summary>
        public ConfigurationObjectType ObjectType { get; } = ConfigurationObjectType.Unknown;

        /// <summary>
        /// Gets the <see cref="VirtualResourceType" />.  Only applicable for virtual resources,
        /// virtual resource metadata, and metadata folders.
        /// </summary>
        public VirtualResourceType ResourceType { get; } = VirtualResourceType.None;

        /// <summary>
        /// Gets the virtual resource metadata type.  Only applicable for virtual resource metadata objects.
        /// </summary>
        public string MetadataType { get; }

        /// <summary>
        /// Gets the name of the configuration object.
        /// </summary>
        public string Name { get; internal set; }

        /// <summary>
        /// Gets the unique ID of the configuration object that owns this configuration object,
        /// or null if this represents a top-level object.
        /// </summary>
        public Guid? ParentId { get; internal set; }

        /// <summary>
        /// Gets the unique ID of the folder that should be displayed containing this configuration object,
        /// or null if this object should be displayed directly beneath its parent.
        /// </summary>
        public Guid? FolderId { get; internal set; }

        /// <summary>
        /// Gets the <see cref="EnabledState" /> of this configuration object.
        /// </summary>
        public EnabledState EnabledState { get; internal set; } = EnabledState.NotApplicable;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectTag" /> class.
        /// </summary>
        /// <param name="id">The unique ID of the configuration object.</param>
        /// <param name="objectType">The <see cref="ConfigurationObjectType" />.</param>
        /// <param name="resourceType">The <see cref="VirtualResourceType" />.</param>
        /// <param name="metadataType">The virtual resource metadata type.</param>
        internal ConfigurationObjectTag(Guid id, ConfigurationObjectType objectType, VirtualResourceType resourceType, string metadataType)
        {
            Id = id;
            ObjectType = objectType;
            ResourceType = resourceType;
            MetadataType = metadataType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectTag" /> class
        /// representing an <see cref="EnterpriseScenario" /> object.
        /// </summary>
        /// <param name="scenario">The <see cref="EnterpriseScenario" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="scenario" /> is null.</exception>
        public ConfigurationObjectTag(EnterpriseScenario scenario)
        {
            if (scenario == null)
            {
                throw new ArgumentNullException(nameof(scenario));
            }

            Id = scenario.EnterpriseScenarioId;
            ObjectType = ConfigurationObjectType.EnterpriseScenario;
            Name = scenario.Name;
            FolderId = scenario.FolderId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectTag" /> class
        /// representing a <see cref="VirtualResource" /> object.
        /// </summary>
        /// <param name="resource">The <see cref="VirtualResource" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="resource" /> is null.</exception>
        public ConfigurationObjectTag(VirtualResource resource)
        {
            if (resource == null)
            {
                throw new ArgumentNullException(nameof(resource));
            }

            Id = resource.VirtualResourceId;
            ObjectType = ConfigurationObjectType.VirtualResource;
            ResourceType = EnumUtil.Parse<VirtualResourceType>(resource.ResourceType);
            Name = resource.Name;
            ParentId = resource.EnterpriseScenarioId;
            FolderId = resource.FolderId;
            EnabledState = resource.Enabled ? EnabledState.Enabled : EnabledState.Disabled;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectTag" /> class
        /// representing a <see cref="VirtualResourceMetadata" /> object.
        /// </summary>
        /// <param name="metadata">The <see cref="VirtualResourceMetadata" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="metadata" /> is null.</exception>
        public ConfigurationObjectTag(VirtualResourceMetadata metadata)
        {
            if (metadata == null)
            {
                throw new ArgumentNullException(nameof(metadata));
            }

            Id = metadata.VirtualResourceMetadataId;
            ObjectType = ConfigurationObjectType.VirtualResourceMetadata;
            ResourceType = EnumUtil.Parse<VirtualResourceType>(metadata.ResourceType);
            MetadataType = metadata.MetadataType;
            Name = metadata.Name;
            ParentId = metadata.VirtualResourceId;
            FolderId = metadata.FolderId;
            EnabledState = metadata.Enabled ? EnabledState.Enabled : EnabledState.Disabled;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectTag" /> class
        /// representing a <see cref="ConfigurationTreeFolder" /> object.
        /// </summary>
        /// <param name="folder">The <see cref="ConfigurationTreeFolder" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="folder" /> is null.</exception>
        public ConfigurationObjectTag(ConfigurationTreeFolder folder)
        {
            if (folder == null)
            {
                throw new ArgumentNullException(nameof(folder));
            }

            Id = folder.ConfigurationTreeFolderId;
            ObjectType = EnumUtil.Parse<ConfigurationObjectType>(folder.FolderType);
            Name = folder.Name;
            ParentId = folder.ParentId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectTag" /> class
        /// representing a <see cref="ConfigurationTreeFolder" /> object.
        /// </summary>
        /// <param name="folder">The <see cref="ConfigurationTreeFolder" />.</param>
        /// <param name="resourceType">The <see cref="VirtualResourceType" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="folder" /> is null.</exception>
        public ConfigurationObjectTag(ConfigurationTreeFolder folder, VirtualResourceType resourceType)
            : this(folder)
        {
            ResourceType = resourceType;
        }
    }
}
