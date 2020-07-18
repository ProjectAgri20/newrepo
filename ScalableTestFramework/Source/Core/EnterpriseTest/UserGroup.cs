using System.Collections.Generic;
using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// A user group that defines permissions for its users.
    /// </summary>
    [DebuggerDisplay("{Name,nq}")]
    public class UserGroup
    {
        /// <summary>
        /// Gets or sets the group name.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the creator of the group.
        /// </summary>
        public string Creator { get; set; }

        /// <summary>
        /// Gets or sets the group description.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the users that are part of this group.
        /// </summary>
        public virtual ICollection<User> Users { get; set; } = new HashSet<User>();

        /// <summary>
        /// Gets or sets the framework clients associated with this user group.
        /// </summary>
        public virtual ICollection<UserGroupFrameworkClientAssociation> FrameworkClients { get; set; } = new HashSet<UserGroupFrameworkClientAssociation>();
    }
}
