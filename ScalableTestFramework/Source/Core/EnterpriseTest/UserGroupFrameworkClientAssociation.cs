using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// An association between a <see cref="UserGroup" /> and a framework client machine.
    /// </summary>
    [DebuggerDisplay("{FrameworkClientHostName,nq}")]
    public sealed class UserGroupFrameworkClientAssociation
    {
        /// <summary>
        /// Gets or sets the user group name.
        /// </summary>
        public string UserGroupName { get; set; }

        /// <summary>
        /// Gets or sets the framework client machine name.
        /// </summary>
        public string FrameworkClientHostName { get; set; }
    }
}
