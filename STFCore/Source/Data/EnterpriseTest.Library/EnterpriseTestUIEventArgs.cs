using System;

namespace HP.ScalableTest.Data.EnterpriseTest
{
    /// <summary>
    /// Event args for a events to and from an <see cref="EnterpriseTestUIController"/> instance.
    /// </summary>
    public class EnterpriseTestUIEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid Id { get; private set; }

        /// <summary>
        /// Gets or sets the parent id.
        /// </summary>
        /// <value>
        /// The parent id.
        /// </value>
        public Guid? ParentId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the image key.
        /// </summary>
        /// <value>
        /// The image key.
        /// </value>
        public string ImageKey { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestUIEventArgs"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        public EnterpriseTestUIEventArgs(Guid id)
        {
            Id = id;
            ParentId = null;
            Name = string.Empty;
            ImageKey = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestUIEventArgs"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="parentId">The parent id.</param>
        public EnterpriseTestUIEventArgs(Guid id, Guid parentId)
            : this(id)
        {
            ParentId = parentId;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestUIEventArgs"/> class.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <param name="name">The name.</param>
        public EnterpriseTestUIEventArgs(Guid id, string name)
            : this(id)
        {
            Name = name;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EnterpriseTestUIEventArgs"/> class.
        /// </summary>
        /// <param name="node">The node.</param>
        internal EnterpriseTestUIEventArgs(EnterpriseTestMapNode node)
        {
            Id = node.Id;
            ParentId = node.FolderId ?? node.ParentId;
            Name = node.Name;

            // Determine the correct image key
            switch (node.NodeType)
            {
                case ConfigurationObjectType.ScenarioFolder:
                case ConfigurationObjectType.ResourceFolder:
                case ConfigurationObjectType.MetadataFolder:
                    ImageKey = "Folder";
                    break;

                case ConfigurationObjectType.EnterpriseScenario:
                    ImageKey = "Scenario";
                    break;

                case ConfigurationObjectType.VirtualResource:
                    ImageKey = node.ResourceType;
                    break;

                case ConfigurationObjectType.ResourceMetadata:
                    ImageKey = node.MetadataType;
                    break;
            }

            // If the node is disabled, modify the key
            if (!node.Enabled)
            {
                ImageKey += "Disabled";
            }
        }
    }
}
