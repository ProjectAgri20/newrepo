using System;
using HP.ScalableTest.Data.EnterpriseTest.Model;

namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// Lightweight representation of a test configuration object, such as a scenario or virtual resource.
    /// </summary>
    internal class EnterpriseTestMapNode
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        /// <value>
        /// The parent id.
        /// </value>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the folder id.
        /// </summary>
        /// <value>
        /// The folder id.
        /// </value>
        public Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the type of the node.
        /// </summary>
        /// <value>
        /// The type of the node.
        /// </value>
        public ConfigurationObjectType NodeType { get; set; }

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the type of the metadata.
        /// </summary>
        /// <value>
        /// The type of the metadata.
        /// </value>
        public string MetadataType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the test configuration object is enabled.
        /// </summary>
        /// <value>
        ///   <c>true</c> if enabled; otherwise, <c>false</c>.
        /// </value>
        public bool Enabled { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestMapNode"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public EnterpriseTestMapNode(Guid id)
        {
            this.Id = id;
            Name = string.Empty;
            NodeType = ConfigurationObjectType.Unknown;
            Enabled = true;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestMapNode"/> class.
        /// </summary>
        /// <param name="scenario">The scenario.</param>
        public EnterpriseTestMapNode(EnterpriseScenario scenario)
            : this(scenario.EnterpriseScenarioId)
        {
            FolderId = scenario.FolderId;
            Name = scenario.Name;
            NodeType = ConfigurationObjectType.EnterpriseScenario;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestMapNode"/> class.
        /// </summary>
        /// <param name="resource">The virtual resource.</param>
        public EnterpriseTestMapNode(VirtualResource resource)
            : this(resource.VirtualResourceId)
        {
            ParentId = resource.EnterpriseScenarioId;
            FolderId = resource.FolderId;
            Name = resource.Name;
            NodeType = ConfigurationObjectType.VirtualResource;
            ResourceType = resource.ResourceType;
            Enabled = resource.Enabled;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestMapNode"/> class.
        /// </summary>
        /// <param name="metadata">The virtual resource metadata.</param>
        public EnterpriseTestMapNode(VirtualResourceMetadata metadata)
            : this(metadata.VirtualResourceMetadataId)
        {
            ParentId = metadata.VirtualResourceId;
            FolderId = metadata.FolderId;
            Name = metadata.Name;
            NodeType = ConfigurationObjectType.ResourceMetadata;
            ResourceType = metadata.ResourceType;
            MetadataType = metadata.MetadataType;
            Enabled = metadata.Enabled;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestMapNode"/> class.
        /// </summary>
        /// <param name="folder">The folder.</param>
        public EnterpriseTestMapNode(ConfigurationTreeFolder folder)
            : this(folder.ConfigurationTreeFolderId)
        {
            ParentId = folder.ParentId;
            Name = folder.Name;
            NodeType = (ConfigurationObjectType)Enum.Parse(typeof(ConfigurationObjectType), folder.FolderType);
        }

        /// <summary>
        /// Creates a node that is representative of the specified object (if applicable).
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns></returns>
        public static EnterpriseTestMapNode Create(object entity)
        {
            EnterpriseScenario scenario = entity as EnterpriseScenario;
            if (scenario != null)
            {
                return new EnterpriseTestMapNode(scenario);
            }

            VirtualResource resource = entity as VirtualResource;
            if (resource != null)
            {
                return new EnterpriseTestMapNode(resource);
            }

            VirtualResourceMetadata metadata = entity as VirtualResourceMetadata;
            if (metadata != null)
            {
                return new EnterpriseTestMapNode(metadata);
            }

            ConfigurationTreeFolder folder = entity as ConfigurationTreeFolder;
            if (folder != null)
            {
                return new EnterpriseTestMapNode(folder);
            }

            return null;
        }
    }
}
