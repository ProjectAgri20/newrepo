using System;
using System.Diagnostics;
using System.Net;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Core.Security
{
    /// <summary>
    /// A network credential that can be serialized.  (<see cref="NetworkCredential" /> should be used if serialization is not required.)
    /// </summary>
    [DataContract, DebuggerDisplay("{UserName,nq}")]
    public class SerializableCredential
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _userName;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _password;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _domain;

        /// <summary>
        /// Gets the user name.
        /// </summary>
        public string UserName => _userName;

        /// <summary>
        /// Gets the password.
        /// </summary>
        protected string Password => _password;

        /// <summary>
        /// Gets the domain.
        /// </summary>
        protected string Domain => _domain;

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableCredential" /> class.
        /// </summary>
        /// <param name="credential">The <see cref="NetworkCredential" />.</param>
        /// <exception cref="ArgumentNullException"><paramref name="credential" /> is null.</exception>
        public SerializableCredential(NetworkCredential credential)
        {
            if (credential == null)
            {
                throw new ArgumentNullException(nameof(credential));
            }

            _userName = credential.UserName;
            _password = credential.Password;
            _domain = credential.Domain;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SerializableCredential"/> class.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="domain">The domain.</param>
        public SerializableCredential(string userName, string password, string domain)
        {
            _userName = userName;
            _password = password;
            _domain = domain;
        }

        /// <summary>
        /// Creates a <see cref="NetworkCredential" /> using the data stored in this instance.
        /// </summary>
        /// <returns>A <see cref="NetworkCredential" />.</returns>
        public virtual NetworkCredential ToNetworkCredential()
        {
            return new NetworkCredential(_userName, _password, _domain);
        }
    }
}
