using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// This class represents a credential for an external resource like HpId, Google, Facebook, etc.
    /// </summary>
    [DataContract]
    public class ExternalCredentialDetail
    {
        /// <summary>
        /// Gets or sets the username.
        /// </summary>
        /// <value>
        /// The username.
        /// </value>
        [DataMember]
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the provider of the credential.
        /// </summary>
        [DataMember]
        public ExternalCredentialType Provider { get; set; }

        /// <summary>
        /// Gets or sets the Office Worker user name.
        /// </summary>
        [DataMember]
        public string DomainUserName { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalCredentialDetail"/> class.
        /// </summary>
        public ExternalCredentialDetail()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExternalCredentialDetail"/> class.
        /// </summary>
        /// <param name="userName">The credential Username.</param>
        /// <param name="password">The credential password.</param>
        /// <param name="provider">The provider of the external credential account.</param>
        /// <param name="domainUserName">The username of the domain user that owns the external credential account.</param>
        public ExternalCredentialDetail(string userName, string password, string externalCredentialType, string domainUserName)
        {
            UserName = userName;
            Password = password;
            Provider = (ExternalCredentialType)Enum.Parse(typeof(ExternalCredentialType), externalCredentialType);
            DomainUserName = domainUserName;
        }

        /// <summary>
        /// Builds an <see cref="ExternalCredentialInfo" /> from this instance.
        /// </summary>
        /// <returns>An <see cref="ExternalCredentialInfo" /> with the data from this instance, or null.</returns>
        public ExternalCredentialInfo ToExternalCredentialInfo()
        {
            return new ExternalCredentialInfo()
            {
                UserName = this.UserName,
                Password = this.Password,
                Provider = Provider
            };
        }
    }
}
