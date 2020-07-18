using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// An association between a <see cref="ResourceType" /> and a framework client platform.
    /// </summary>
    [DebuggerDisplay("{FrameworkClientPlatformId,nq}")]
    public sealed class ResourceTypeFrameworkClientPlatformAssociation
    {
        /// <summary>
        /// Gets or sets the resource type name.
        /// </summary>
        public string ResourceTypeName { get; set; }

        /// <summary>
        /// Gets or sets the framework client platform ID.
        /// </summary>
        public string FrameworkClientPlatformId { get; set; }
    }
}
