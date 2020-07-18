using System.Runtime.Serialization;
using HP.ScalableTest.Data.EnterpriseTest.Model;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Core.ImportExport
{
    /// <summary>
    /// Data contract for SolutionTester (used for import/export).
    /// </summary>
    [DataContract(Name = "SolutionTester", Namespace = "")]
    [ObjectFactory(VirtualResourceType.SolutionTester)]
    public class SolutionTesterContract : OfficeWorkerContract
    {
        /// <summary>
        /// Loads the SolutionTesterContract using the specified VirtualResource object.
        /// </summary>
        public override void Load(VirtualResource resource)
        {
            base.Load(resource);

            var tester = resource as SolutionTester;

            Username = tester.UserName;
            Domain = tester.Domain;
            Password = tester.Password;
            AccountType = tester.AccountType;
            UseCredential = tester.UseCredential;
        }

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
        /// Gets or sets the type of the credential to be used when executing plugins.
        /// </summary>
        /// <value>The type of the credential.</value>
        [DataMember]
        public SolutionTesterCredentialType AccountType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the solution tester 
        /// should run under the provided credential or run as the desktop user.
        /// </summary>
        /// <value><c>true</c> if using the provided credential; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UseCredential { get; set; }

    }
}
