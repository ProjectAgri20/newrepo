using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Metadata that defines work performed by a virtual resource.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class VirtualResourceMetadata
    {
        /// <summary>
        /// Gets or sets the unique identifier for the virtual resource metadata.
        /// </summary>
        public Guid VirtualResourceMetadataId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="VirtualResource" /> this metadata belongs to.
        /// </summary>
        public Guid VirtualResourceId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="ConfigurationTreeFolder" /> that contains this metadata.
        /// </summary>
        public Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the name of this virtual resource metadata.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the virtual resource type for this metadata.
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the metadata type.
        /// </summary>
        public string MetadataType { get; set; }

        /// <summary>
        /// Gets or sets the metadata version.
        /// </summary>
        public string MetadataVersion { get; set; }

        /// <summary>
        /// Gets or sets the XML metadata.
        /// </summary>
        public string Metadata { get; set; }

        /// <summary>
        /// Gets or sets the XML execution plan.
        /// </summary>
        public string ExecutionPlan { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this metadata is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="VirtualResourceMetadataAssetUsage" /> for this metadata.
        /// </summary>
        public virtual VirtualResourceMetadataAssetUsage AssetUsage { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="VirtualResourceMetadataDocumentUsage" /> for this metadata.
        /// </summary>
        public virtual VirtualResourceMetadataDocumentUsage DocumentUsage { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="VirtualResourceMetadataPrintQueueUsage" /> for this metadata.
        /// </summary>
        public virtual VirtualResourceMetadataPrintQueueUsage PrintQueueUsage { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="VirtualResourceMetadataServerUsage" /> for this metadata.
        /// </summary>
        public virtual VirtualResourceMetadataServerUsage ServerUsage { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="VirtualResourceMetadataRetrySetting" /> collection for this metadata.
        /// </summary>
        public virtual ICollection<VirtualResourceMetadataRetrySetting> RetrySettings { get; set; } = new HashSet<VirtualResourceMetadataRetrySetting>();
    }
}
