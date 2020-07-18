namespace HP.ScalableTest.Core.EnterpriseTest
{
    /// <summary>
    /// Configuration data for a Solution Tester virtual resource.
    /// </summary>
    public class SolutionTester : OfficeWorker
    {
        /// <summary>
        /// Gets or sets a value indicating whether to launch the worker process under a specific user.
        /// </summary>
        public bool UseCredential { get; set; }

        /// <summary>
        /// Gets or sets the type of credential to use during automation.
        /// </summary>
        public string CredentialType { get; set; }

        /// <summary>
        /// Gets or sets the credential user name.
        /// </summary>
        public string CredentialName { get; set; }

        /// <summary>
        /// Gets or sets the credential domain.
        /// </summary>
        public string CredentialDomain { get; set; }

        /// <summary>
        /// Gets or sets the credential password.
        /// </summary>
        public string CredentialPassword { get; set; }
    }
}
