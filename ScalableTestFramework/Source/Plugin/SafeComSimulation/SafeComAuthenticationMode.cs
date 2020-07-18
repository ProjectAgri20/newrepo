
namespace HP.ScalableTest.Plugin.SafeComSimulation
{
    /// <summary>
    /// Represents the authentication modes available for SafeCom Simulation.
    /// </summary>
    public enum SafeComAuthenticationMode
    {
        /// <summary>
        /// Authenticate using a proximity card and user PIN
        /// </summary>
        CardAndPin,
        /// <summary>
        /// Authenticate using a username and password
        /// </summary>
        NameAndPassword,
        /// <summary>
        /// Authenticate using a username and PIN
        /// </summary>
        NameAndPin,
        /// <summary>
        /// Authenticate using the user's Windows credentials
        /// </summary>
        WindowsCredentials
    }
}
