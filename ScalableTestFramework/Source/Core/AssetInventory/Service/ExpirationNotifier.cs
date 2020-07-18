using System;
using System.Net.Mail;
using System.Text;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Core.AssetInventory.Service
{
    /// <summary>
    /// Sends notifications for expiring reservations and licenses.
    /// </summary>
    public sealed class ExpirationNotifier
    {
        private readonly string _smtpHost;
        private readonly MailAddress _fromAddress = new MailAddress("DoNotReply@hp.com");

        /// <summary>
        /// Initializes a new instance of the <see cref="ExpirationNotifier" /> class.
        /// </summary>
        /// <param name="smtpHost">The SMTP host.</param>
        /// <exception cref="ArgumentNullException"><paramref name="smtpHost" /> is null.</exception>
        public ExpirationNotifier(string smtpHost)
        {
            _smtpHost = smtpHost ?? throw new ArgumentNullException(nameof(smtpHost));
        }

        /// <summary>
        /// Sends an email notification using the specified parameters.
        /// </summary>
        /// <param name="toAddress">The address of the recipient.</param>
        /// <param name="subject">The email subject.</param>
        /// <param name="body">The email body.</param>
        public void SendNotification(MailAddress toAddress, string subject, string body)
        {
            try
            {
                using (SmtpClient smtp = new SmtpClient(_smtpHost))
                {
                    using (MailMessage message = new MailMessage(_fromAddress, toAddress))
                    {
                        message.Subject = subject;
                        message.Body = body;
                        message.BodyEncoding = Encoding.ASCII;

                        smtp.Send(message);
                    }
                }
            }
            catch (SmtpFailedRecipientException ex)
            {
                LogError($"Failed to send notification to {toAddress}.", ex);
            }
        }
    }
}
