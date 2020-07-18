using System;
using System.Globalization;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// This class represents an Office Worker credential.  It includes a Port value and Working Directory.
    /// </summary>
    [DataContract]
    public class OfficeWorkerCredential
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
        /// Gets or sets the password for the user.
        /// </summary>
        /// <value>
        /// The password.
        /// </value>
        [DataMember]
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the domain for the user.
        /// </summary>
        /// <value>
        /// The Windows domain name.
        /// </value>
        [DataMember]
        public string Domain { get; set; }

        /// <summary>
        /// Gets or sets the port.
        /// </summary>
        /// <value>
        /// The port.
        /// </value>
        [DataMember]
        public int Port { get; set; }

        /// <summary>
        /// Gets or sets the working directory.
        /// </summary>
        /// <value>
        /// The working directory.
        /// </value>
        [DataMember]
        public string WorkingDirectory { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>The unique identifier.</value>
        [DataMember]
        public string ResourceInstanceId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use windows authentication.
        /// </summary>
        /// <value><c>true</c> if using windows authentication; otherwise, <c>false</c>.</value>
        [DataMember]
        public bool UseWindowsAuthentication { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorkerCredential"/> class.
        /// </summary>
        public OfficeWorkerCredential()
        {
            UseWindowsAuthentication = false;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="OfficeWorkerCredential"/> class.
        /// </summary>
        /// <param name="userName">The username.</param>
        /// <param name="password">The password.</param>
        /// <param name="domain">The domain.</param>
        /// <param name="port">The port.</param>
        public OfficeWorkerCredential(string userName, string password, string domain, int port, bool useWindowsAuthentication = false)
        {
            UserName = userName;
            Password = password;
            Domain = domain;
            Port = port;
            UseWindowsAuthentication = useWindowsAuthentication;
            ResourceInstanceId = userName;
        }

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return string.Format(CultureInfo.InvariantCulture, "{0}, InstanceId: {1}, Port: {2}, Directory: {3}", base.ToString(), ResourceInstanceId, Port, WorkingDirectory);
        }
    }
}
