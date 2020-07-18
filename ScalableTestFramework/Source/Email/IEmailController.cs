using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Net.Mail;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.Email
{
    /// <summary>
    /// Interfae for communication with an email server.  Includes methods to send, retrieve, and delete email messages.
    /// </summary>
    public interface IEmailController
    {
        /// <summary>
        /// Occurs when there is an update in a long-running operation.
        /// </summary>
        event EventHandler<StatusChangedEventArgs> StatusUpdate;

        /// <summary>
        /// Sends the specified <see cref="MailMessage" /> through the email server.
        /// </summary>
        /// <param name="message">The <see cref="MailMessage" /> to send.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> is null.</exception>
        void Send(MailMessage message);

        /// <summary>
        /// Sends the specified <see cref="MailMessage" /> through the email server.
        /// </summary>
        /// <param name="message">The <see cref="MailMessage" /> to send.</param>
        /// <param name="attachment">The attachment to include with the message.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is null.
        /// <para>or</para>
        /// <paramref name="attachment" /> is null.
        /// </exception>
        void Send(MailMessage message, FileInfo attachment);

        /// <summary>
        /// Sends the specified <see cref="MailMessage" /> through the email server.
        /// </summary>
        /// <param name="message">The <see cref="MailMessage" /> to send.</param>
        /// <param name="attachments">The attachments to include with the message.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="message" /> is null.
        /// <para>or</para>
        /// <paramref name="attachments" /> is null.
        /// </exception>
        void Send(MailMessage message, IEnumerable<FileInfo> attachments);


        /// <summary>
        /// Retrieves all email messages from the specfied <see cref="EmailFolder" />.
        /// </summary>
        /// <param name="folder">The <see cref="EmailFolder" /> to retrieve from.</param>
        /// <returns>A collection of <see cref="EmailMessage" /> objects.</returns>
        Collection<EmailMessage> RetrieveMessages(EmailFolder folder);

        /// <summary>
        /// Retrieves up to the specified number email messages from the specfied <see cref="EmailFolder" />.
        /// </summary>
        /// <param name="folder">The <see cref="EmailFolder" /> to retrieve from.</param>
        /// <param name="itemLimit">The maximum number of messages to retrieve.</param>
        /// <returns>A collection of <see cref="EmailMessage" /> objects.</returns>
        Collection<EmailMessage> RetrieveMessages(EmailFolder folder, int itemLimit);

        /// <summary>
        /// Deletes the specified message from the server.
        /// </summary>
        /// <param name="message">The message to delete.</param>
        void Delete(EmailMessage message);

        /// <summary>
        /// Deletes the specified messages from the server.
        /// </summary>
        /// <param name="messages">The messages to delete.</param>
        /// <exception cref="ArgumentNullException"><paramref name="messages" /> is null.</exception>
        void Delete(IEnumerable<EmailMessage> messages);

        /// <summary>
        /// Deletes all items from the specified folder.
        /// </summary>
        /// <param name="folder">The <see cref="EmailFolder" /> to clear.</param>
        void Clear(EmailFolder folder);
    }
}
