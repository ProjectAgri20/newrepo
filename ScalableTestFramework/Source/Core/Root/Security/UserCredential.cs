using System.Diagnostics;
using System.Net;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Core.Security
{
    /// <summary>
    /// A credential for an STF user with a <see cref="UserRole" /> to specify privileges.
    /// Also encrypts the password to obscure it during debugging or data transmission.
    /// </summary>
    [DataContract, DebuggerDisplay("{UserName,nq} ({Role,nq})")]
    public class UserCredential : SerializableCredential
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly UserRole _role;

        /// <summary>
        /// Gets the <see cref="UserRole" />.
        /// </summary>
        protected UserRole Role => _role;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCredential" /> class.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="domain">The domain.</param>
        public UserCredential(string userName, string password, string domain)
            : this(userName, password, domain, UserRole.Guest)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="UserCredential" /> class.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="password">The password.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="role">The <see cref="UserRole" />.</param>
        public UserCredential(string userName, string password, string domain, UserRole role)
            : base(userName, EncryptPassword(password), domain)
        {
            _role = role;
        }

        /// <summary>
        /// Determines whether this user has at least the specified privilege level.
        /// </summary>
        /// <param name="role">The <see cref="UserRole" /> representing the target privilege level.</param>
        /// <returns><c>true</c> if the user has at least the specified privilege level; otherwise, <c>false</c>.</returns>
        public bool HasPrivilege(UserRole role)
        {
            return (int)_role >= (int)role;
        }

        /// <summary>
        /// Creates a <see cref="NetworkCredential" /> using the data stored in this instance.
        /// </summary>
        /// <returns>A <see cref="NetworkCredential" />.</returns>
        public override NetworkCredential ToNetworkCredential()
        {
            return new NetworkCredential(UserName, DecryptPassword(Password), Domain);
        }

        private static string EncryptPassword(string password)
        {
            return SimpleEncryption.Encrypt(password, Resource.CredentialKey);
        }

        private static string DecryptPassword(string password)
        {
            return SimpleEncryption.Decrypt(password, Resource.CredentialKey);
        }
    }
}
