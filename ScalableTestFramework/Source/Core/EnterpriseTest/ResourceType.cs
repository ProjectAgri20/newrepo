using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Information about a type of <see cref="VirtualResource" /> that can be created by the framework.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class ResourceType
    {
        /// <summary>
        /// Gets or sets the virtual resource type name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of resources of this type that can be hosted on a single client.
        /// </summary>
        public int MaxResourcesPerHost { get; set; }

        /// <summary>
        /// Gets or sets the metadata types that are associated with this resource type.
        /// </summary>
        public virtual ICollection<MetadataType> MetadataTypes { get; set; } = new HashSet<MetadataType>();

        /// <summary>
        /// Gets or sets the framework client platforms that are associated with this resource type.
        /// </summary>
        public virtual ICollection<ResourceTypeFrameworkClientPlatformAssociation> FrameworkClientPlatforms { get; set; } = new HashSet<ResourceTypeFrameworkClientPlatformAssociation>();
    }
}
