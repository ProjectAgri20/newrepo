using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Net.Mail;

namespace HP.ScalableTest.Email
{
    /// <summary>
    /// Represents an email message retrieved from an email server.
    /// </summary>
    public abstract class EmailMessage
    {
        /// <summary>
        /// Gets the email address this email was sent from.
        /// </summary>
        public MailAddress FromAddress { get; protected set; }

        /// <summary>
        /// Gets the email addresses this email was sent to.
        /// </summary>
        public MailAddressCollection ToRecipients { get; } = new MailAddressCollection();

        /// <summary>
        /// Gets the collection of email addresses that were CC'd.
        /// </summary>
        public MailAddressCollection CcRecipients { get; } = new MailAddressCollection();

        /// <summary>
        /// Gets the date and time this email was sent.
        /// </summary>
        public DateTimeOffset DateTimeSent { get; protected set; }

        /// <summary>
        /// Gets the date and time this email was received.
        /// </summary>
        public DateTimeOffset DateTimeReceived { get; protected set; }

        /// <summary>
        /// Gets the subject of the email.
        /// </summary>
        public string Subject { get; protected set; }

        /// <summary>
        /// Gets the body of the email.
        /// </summary>
        public string Body { get; protected set; }

        /// <summary>
        /// Gets the files that were attached to this email.
        /// </summary>
        public Collection<EmailAttachment> Attachments { get; } = new Collection<EmailAttachment>();

        /// <summary>
        /// Gets the headers associated with this email.
        /// </summary>
        public Dictionary<string, string> Headers { get; } = new Dictionary<string, string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailMessage" /> class.
        /// </summary>
        protected EmailMessage()
        {
            // Constructor explicitly declared for XML doc.
        }
    }
}
