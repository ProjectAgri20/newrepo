using System;
using System.Net.Mail;
using HP.ScalableTest.Framework.Plugin;

namespace HP.ScalableTest.Email
{
    /// <summary>
    /// Constructs email addresses.
    /// </summary>
    public sealed class EmailBuilder
    {
        /// <summary>
        /// Gets or sets the user name for the email address.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the domain for the email address.
        /// </summary>
        public string Domain { get; set; }

        /// <summary>
        /// Gets the <see cref="MailAddress" /> constructed from this <see cref="EmailBuilder" />.
        /// </summary>
        /// <exception cref="FormatException">The parameters used to construct the email address are invalid.</exception>
        public MailAddress Address
        {
            get { return new MailAddress($"{UserName}@{Domain}".ToLowerInvariant()); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailBuilder" /> class.
        /// </summary>
        public EmailBuilder()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailBuilder" /> class.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="domain">The domain.</param>
        public EmailBuilder(string userName, string domain)
        {
            UserName = userName;
            Domain = domain;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailBuilder" /> class.
        /// </summary>
        /// <param name="userName">The user name.</param>
        /// <param name="executionData">The <see cref="PluginExecutionData" /> containing information about the execution environment.</param>
        public EmailBuilder(string userName, PluginExecutionData executionData)
            : this(userName, executionData?.Environment.UserDnsDomain)
        {
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Address.ToString();
        }
    }
}
