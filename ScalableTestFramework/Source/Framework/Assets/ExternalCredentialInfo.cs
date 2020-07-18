using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a domain user's External credentials.
    /// </summary>
    [DataContract]
    [DebuggerDisplay("{UserName,nq}")]
    public class ExternalCredentialInfo
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
    }
}
