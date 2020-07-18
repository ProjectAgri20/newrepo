using System.ComponentModel;

namespace HP.ScalableTest.Core.Security
{
    /// <summary>
    /// Defines privilege levels for STF users.
    /// </summary>
    /// <remarks>
    /// The order of the enum members defines the privilege order.
    /// Higher number = more permissions.
    /// </remarks>
    public enum UserRole
    {
        /// <summary>
        /// A guest user.  Minimum privileges, mostly read-only.
        /// </summary>
        [Description("Guest")]
        Guest = 0,

        /// <summary>
        /// A general user.  Basic privileges to configure and execute tests.
        /// </summary>
        [Description("User")]
        User = 16,

        /// <summary>
        /// A manager role.  Expanded privileges for managing the STF environment.
        /// </summary>
        [Description("Manager")]
        Manager = 128,

        /// <summary>
        /// An administrative role.  Full permissions for administrating STF.
        /// </summary>
        [Description("Administrator")]
        Administrator = 256,

        /// <summary>
        /// STF developer.  Allows access to debugging tools as well as all administrative tasks.
        /// </summary>
        /// <remarks>
        /// Do not decorate with a Description attribute - prevents this from showing in UI dropdowns.
        /// Developer access is designed to only be configurable via direct database access.
        /// </remarks>
        Developer = 9001
    }
}
