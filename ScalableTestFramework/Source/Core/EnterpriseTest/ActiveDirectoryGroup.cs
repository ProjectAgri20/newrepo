using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// An Active Directory group that can be configured for use with a virtual resource.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public sealed class ActiveDirectoryGroup
    {
        /// <summary>
        /// Gets or sets the Active Directory group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the Active Directory group description.
        /// </summary>
        public string Description { get; set; }
    }
}
