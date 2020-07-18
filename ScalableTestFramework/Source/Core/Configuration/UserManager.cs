using HP.ScalableTest.Core.Security;

namespace HP.ScalableTest.Core.Configuration
{
    /// <summary>
    /// Tracks the current user of the STF environment.
    /// </summary>
    public static class UserManager
    {
        /// <summary>
        /// Gets or sets the <see cref="UserCredential" /> for the logged-in STF user.
        /// </summary>
        public static UserCredential CurrentUser { get; set; }

        /// <summary>
        /// Gets the user name of the logged-in STF user.
        /// </summary>
        public static string CurrentUserName => CurrentUser?.UserName;

        /// <summary>
        /// Gets a value indicating whether a user is logged in to STF.
        /// </summary>
        public static bool UserLoggedIn => CurrentUser != null;
    }
}
