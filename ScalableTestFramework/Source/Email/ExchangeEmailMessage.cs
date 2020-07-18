using System;
using System.Net.Mail;
using Microsoft.Exchange.WebServices.Data;
using ExchangeMessage = Microsoft.Exchange.WebServices.Data.EmailMessage;

namespace HP.ScalableTest.Email
{
    /// <summary>
    /// Represents an email message retrieved from a Microsoft Exchange server.
    /// </summary>
    public sealed class ExchangeEmailMessage : EmailMessage
    {
        /// <summary>
        /// Gets the Exchange item identifier for this message.
        /// </summary>
        internal ItemId ExchangeId { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeEmailMessage" /> class.
        /// </summary>
        /// <param name="email">The <see cref="ExchangeMessage" /> retrieved from the Exchange server.</param>
        /// <exception cref="ArgumentNullException"><paramref name="email" /> is null.</exception>
        internal ExchangeEmailMessage(ExchangeMessage email)
        {
            if (email == null)
            {
                throw new ArgumentNullException(nameof(email));
            }

            ExchangeId = email.Id;
            DateTimeSent = email.DateTimeSent;
            DateTimeReceived = email.DateTimeReceived;
            Subject = email.Subject;
            Body = email.Body;

            if (!string.IsNullOrEmpty(email.From?.Address))
            {
                try
                {
                    FromAddress = new MailAddress(email.From?.Address, email.From?.Name);
                }
                catch (FormatException)
                {
                    // Invalid From address.
                }
            }

            foreach (EmailAddress toRecipient in email.ToRecipients)
            {
                ToRecipients.Add(new MailAddress(toRecipient.Address, toRecipient.Name));
            }

            foreach (EmailAddress ccRecipient in email.CcRecipients)
            {
                CcRecipients.Add(new MailAddress(ccRecipient.Address, ccRecipient.Name));
            }

            if (email.HasAttachments)
            {
                foreach (FileAttachment file in email.Attachments)
                {
                    Attachments.Add(new ExchangeEmailAttachment(file));
                }
            }

            foreach (var header in email.InternetMessageHeaders)
            {
                Headers[header.Name] = header.Value;
            }
        }

        /// <summary>
        /// Returns a <see cref="string" /> that represents this instance.
        /// </summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"{FromAddress} - {Subject}";
        }
    }
}
