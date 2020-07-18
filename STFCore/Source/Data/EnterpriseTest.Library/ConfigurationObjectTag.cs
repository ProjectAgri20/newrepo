using System;
using HP.ScalableTest.Core;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// Configuration object information, suitable for instantiating or loading a scenario configuration object.
    /// </summary>
    public class ConfigurationObjectTag
    {
        /// <summary>
        /// Gets the id.
        /// </summary>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the type of the object.
        /// </summary>
        /// <value>
        /// The type of the object.
        /// </value>
        public ConfigurationObjectType ObjectType { get; private set; }

        /// <summary>
        /// Gets or sets the type of the resource.
        /// </summary>
        /// <value>
        /// The type of the resource.
        /// </value>
        public VirtualResourceType ResourceType { get; private set; }

        /// <summary>
        /// Gets the type of the metadata.
        /// </summary>
        /// <value>
        /// The type of the metadata.
        /// </value>
        public string MetadataType { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectTag"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        internal ConfigurationObjectTag(EnterpriseTestMapNode node)
        {
            Id = node.Id;
            ObjectType = node.NodeType;
            if (!string.IsNullOrEmpty(node.ResourceType))
            {
                ResourceType = EnumUtil.Parse<VirtualResourceType>(node.ResourceType);
                MetadataType = node.MetadataType;
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectTag"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        internal ConfigurationObjectTag(ConfigurationObjectType type)
        {
            Id = Guid.Empty;
            ObjectType = type;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectTag"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="resourceType">Type of the resource.</param>
        internal ConfigurationObjectTag(ConfigurationObjectType type, VirtualResourceType resourceType)
            : this(type)
        {
            ResourceType = resourceType;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ConfigurationObjectTag"/> class.
        /// </summary>
        /// <param name="type">The type.</param>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="metadataType">Type of the metadata.</param>
        internal ConfigurationObjectTag(ConfigurationObjectType type, VirtualResourceType resourceType, string metadataType)
            : this(type, resourceType)
        {
            MetadataType = metadataType;
        }

        /// <summary>
        /// Creates a <see cref="ConfigurationObjectTag"/> for an enterprise scenario.
        /// </summary>
        /// <returns></returns>
        public static ConfigurationObjectTag CreateScenario()
        {
            return new ConfigurationObjectTag(ConfigurationObjectType.EnterpriseScenario);
        }

        /// <summary>
        /// Creates a <see cref="ConfigurationObjectTag"/> for the specified resource type.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <returns></returns>
        public static ConfigurationObjectTag Create(VirtualResourceType resourceType)
        {
            return new ConfigurationObjectTag(ConfigurationObjectType.VirtualResource, resourceType);
        }

        /// <summary>
        /// Creates a <see cref="ConfigurationObjectTag"/> for the specified resource metadata type.
        /// </summary>
        /// <param name="resourceType">Type of the resource.</param>
        /// <param name="metadataType">Type of the metadata.</param>
        /// <returns></returns>
        public static ConfigurationObjectTag Create(VirtualResourceType resourceType, string metadataType)
        {
            return new ConfigurationObjectTag(ConfigurationObjectType.ResourceMetadata, resourceType, metadataType);
        }
    }
}
