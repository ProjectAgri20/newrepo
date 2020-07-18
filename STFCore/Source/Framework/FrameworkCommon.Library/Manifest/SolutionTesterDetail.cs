using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Solution Tester
    /// </summary>
    [DataContract]
    public class SolutionTesterDetail : OfficeWorkerDetail
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>The username.</value>
        [DataMember]
        public string Username { get; set; }

        /// <summary>
        /// Gets or sets the domain.
        /// </summary>
        /// <value>The domain.</value>
        [DataMember]
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>The password.</value>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the type of the credential.
        /// </summary>
        /// <value>The type of the credential.</value>
        [DataMember]
        public string CredentialType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the provided credential.
        /// </summary>
        /// <value><c>true</c> if you will use the provided credential; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UseCredential { get; set; }
    }
}