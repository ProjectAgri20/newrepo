using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Base class for a virtual resource used in a test.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public abstract class VirtualResource
    {
        /// <summary>
        /// Gets or sets the unique identifier for the virtual resource.
        /// </summary>
        public Guid VirtualResourceId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="EnterpriseScenario" /> this resource belongs to.
        /// </summary>
        public Guid EnterpriseScenarioId { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier for the <see cref="ConfigurationTreeFolder" /> that contains this resource.
        /// </summary>
        public Guid? FolderId { get; set; }

        /// <summary>
        /// Gets or sets the name of this virtual resource.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the description of this virtual resource.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the virtual resource type.
        /// </summary>
        public string ResourceType { get; set; }

        /// <summary>
        /// Gets or sets the resource instance count.
        /// </summary>
        public int InstanceCount { get; set; }

        /// <summary>
        /// Gets or sets the resource execution platform.
        /// </summary>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the number of resources deployable on a single VM.
        /// </summary>
        public int? ResourcesPerVM { get; set; }

        /// <summary>
        /// Gets or sets the test case ID.
        /// </summary>
        public int TestCaseId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this resource is enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the <see cref="VirtualResourceMetadata" /> objects associated with this virtual resource.
        /// </summary>
        public virtual ICollection<VirtualResourceMetadata> VirtualResourceMetadataSet { get; set; } = new HashSet<VirtualResourceMetadata>();
    }
}
