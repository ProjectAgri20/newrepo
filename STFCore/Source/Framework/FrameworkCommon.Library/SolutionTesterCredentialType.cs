namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Defines the Solution Tester's Credential Source
    /// </summary>
    public enum SolutionTesterCredentialType
    {
        /// <summary>
        /// Taken from User Account Pool
        /// </summary>
        AccountPool,
        /// <summary>
        /// Uses Default Logged in Credentials
        /// </summary>
        DefaultDesktop,
        /// <summary>
        /// Uses User Entered Credentials
        /// </summary>
        ManuallyEntered,
    }
}
