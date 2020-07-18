namespace HP.ScalableTest.Plugin.HpacSimulation
{
    /// <summary>
    /// Represents the authentication modes available for HPAC Simulation.
    /// </summary>
    public enum HpacAuthenticationMode
    {
        /// <summary>
        /// Authenticate using the logged-in user's domain credentials
        /// </summary>
        DomainCredentials,
        /// <summary>
        /// Authenticate using the user's Pic - personal identification code (U00001)
        /// </summary>
        Pic,
        /// <summary>
        /// Authenticate using the user's smartcard (card ID 00001)
        /// </summary>
        SmartCard
    }
}
