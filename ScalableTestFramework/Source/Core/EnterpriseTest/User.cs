using System.Diagnostics;

namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// A user of the STF system.
    /// </summary>
    [DebuggerDisplay("{UserName,nq}")]
    public sealed class User
    {
        /// <summary>
        /// Gets or sets the user name.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the user domain.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the role of the user, e.g. Administrator.
        /// </summary>
        public string RoleName { get; set; }
    }
}
