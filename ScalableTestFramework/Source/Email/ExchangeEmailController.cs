using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.DirectoryServices.AccountManagement;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Mail;
using HP.ScalableTest.Utility;
using Microsoft.Exchange.WebServices.Data;
using static HP.ScalableTest.Framework.Logger;
using ExchangeMessage = Microsoft.Exchange.WebServices.Data.EmailMessage;

namespace HP.ScalableTest.Email
{
    /// <summary>
    /// An implementation of <see cref="IEmailController" /> for Microsoft Exchange Server.
    /// </summary>
    public sealed class ExchangeEmailController : IEmailController
    {
        // The ExchangeVersion used will define the minimum required version of Exchange, as well as the
        // level of functionality available in the API.  This should not need to be updated unless we want
        // to use features that are only available in a newer version of the API.
        private readonly ExchangeService _service = new ExchangeService(ExchangeVersion.Exchange2010_SP2);

        private static readonly Dictionary<EmailFolder, WellKnownFolderName> _exchangeFolders = new Dictionary<EmailFolder, WellKnownFolderName>
        {
            [EmailFolder.Inbox] = WellKnownFolderName.Inbox,
            [EmailFolder.SentItems] = WellKnownFolderName.SentItems,
            [EmailFolder.DeletedItems] = WellKnownFolderName.DeletedItems,
            [EmailFolder.Drafts] = WellKnownFolderName.Drafts,
            [EmailFolder.JunkEmail] = WellKnownFolderName.JunkEmail
        };

        /// <summary>
        /// Gets the user name whose mailbox will be accessed by this <see cref="ExchangeEmailController" />.
        /// </summary>
        public string UserName { get; }

        /// <summary>
        /// Gets or sets the timeout to use for EWS operations.
        /// </summary>
        public TimeSpan Timeout
        {
            get { return TimeSpan.FromMilliseconds(_service.Timeout); }
            set { _service.Timeout = (int)value.TotalMilliseconds; }
        }

        /// <summary>
        /// Occurs when there is an update in a long-running operation.
        /// </summary>
        public event EventHandler<StatusChangedEventArgs> StatusUpdate;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeEmailController" /> class.
        /// </summary>
        /// <param name="user">The <see cref="NetworkCredential" /> for the user whose email this controller will access.</param>
        private ExchangeEmailController(NetworkCredential user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            UserName = user.UserName;

            // Check to see if we are running as the user whose inbox will be accessed.
            if (user.UserName.Equals(Environment.UserName))
            {
                // Same user - use the default credentials to allow pass-through from Windows.
                LogDebug("Connecting to Exchange server using default credentials.");
                _service.UseDefaultCredentials = true;
            }
            else
            {
                // Different user - explicitly set the credentials to use.
                LogDebug($"Connecting to Exchange server using specified credentials for {UserName}.");
                _service.Credentials = user;
            }

            // Bypass the HTTPS certificate validation check
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeEmailController" /> class.
        /// </summary>
        /// <param name="user">The <see cref="NetworkCredential" /> for the user whose email this controller will access.</param>
        /// <param name="autodiscoverEmail">The email address to use for autodiscovering the Exchange Web Services URL.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="user" /> is null.
        /// <para>or</para>
        /// <paramref name="autodiscoverEmail" /> is null.
        /// </exception>
        public ExchangeEmailController(NetworkCredential user, MailAddress autodiscoverEmail)
            : this(user, new ExchangeConnectionSettings(autodiscoverEmail))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeEmailController" /> class.
        /// </summary>
        /// <param name="user">The <see cref="NetworkCredential" /> for the user whose email this controller will access.</param>
        /// <param name="ewsUrl">The Exchange Web Services URL.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="user" /> is null.
        /// <para>or</para>
        /// <paramref name="ewsUrl" /> is null.
        /// </exception>
        public ExchangeEmailController(NetworkCredential user, Uri ewsUrl)
            : this(user, new ExchangeConnectionSettings(ewsUrl))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeEmailController" /> class.
        /// </summary>
        /// <param name="user">The <see cref="NetworkCredential" /> for the user whose email this controller will access.</param>
        /// <param name="settings">The settings for connecting to Exchange Web Services.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="user" /> is null.
        /// <para>or</para>
        /// <paramref name="settings" /> is null.
        /// </exception>
        public ExchangeEmailController(NetworkCredential user, ExchangeConnectionSettings settings)
            : this(user)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (settings.AutodiscoverEnabled)
            {
                LogDebug("$Autodiscovering Exchange server URL for {settings.AutodiscoverEmail}");
                MailAddress address = settings.AutodiscoverEmail ?? GetLdapEmailAddress(user);
                _service.AutodiscoverUrl(address.Address);
                LogDebug("Autodiscover complete.");
            }
            else
            {
                LogDebug($"Using specified Exchange URL: {settings.EwsUrl}");
                _service.Url = settings.EwsUrl;
            }
        }

        /// <summary>
        /// Autodiscovers the Exchange server URL for the specified email address.
        /// </summary>
        /// <param name="address">The email address to use for autodiscovering the Exchange server URL.</param>
        /// <returns>The Exchange server URL.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="address" /> is null.</exception>
        public static Uri AutodiscoverExchangeUrl(MailAddress address)
        {
            if (address == null)
            {
                throw new ArgumentNullException(nameof(address));
            }

            // Bypass the HTTPS certificate validation check
            ServicePointManager.ServerCertificateValidationCallback = delegate { return true; };

            LogDebug($"Autodiscovering Exchange server URL for {address}");
            ExchangeService exchange = new ExchangeService();
            exchange.AutodiscoverUrl(address.Address);
            LogDebug("Autodiscover complete.");
            return exchange.Url;
        }

        /// <summary>
        /// Gets the LDAP email address for the specified user.
        /// </summary>
        /// <param name="user">The <see cref="NetworkCredential" /> representing the user whose email should be retrieved.</param>
        /// <returns>The LDAP email address for the specified user.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="user" /> is null.</exception>
        /// <exception cref="KeyNotFoundException"><paramref name="user" /> not found.</exception>
        public static MailAddress GetLdapEmailAddress(NetworkCredential user)
        {
            if (user == null)
            {
                throw new ArgumentNullException(nameof(user));
            }

            LogDebug($"Retrieving LDAP email address for {user.UserName}");
            PrincipalContext context;
            if (string.IsNullOrEmpty(user.Domain))
            {
                context = new PrincipalContext(ContextType.Domain);
            }
            else
            {
                context = new PrincipalContext(ContextType.Domain, user.Domain);
            }

            UserPrincipal userPrincipal = UserPrincipal.FindByIdentity(context, user.UserName);
            if (userPrincipal == null)
            {
                throw new KeyNotFoundException($"User '{user.UserName}' not found on domain: {user.Domain}");
            }

            return new MailAddress(userPrincipal.EmailAddress);
        }

        /// <summary>
        /// Sends the specified <see cref="MailMessage" /> through the email server.
        /// </summary>
        /// <param name="message">The <see cref="MailMessage" /> to send.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> is null.</exception>
        public void Send(MailMessage message)
        {
            Send(message, Enumerable.Empty<FileInfo>());
        }

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
        public void Send(MailMessage message, FileInfo attachment)
        {
            if (attachment == null)
            {
                throw new ArgumentNullException(nameof(attachment));
            }

            Send(message, new[] { attachment });
        }

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
        public void Send(MailMessage message, IEnumerable<FileInfo> attachments)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            if (attachments == null)
            {
                throw new ArgumentNullException(nameof(attachments));
            }

            ExchangeMessage email = new ExchangeMessage(_service);
            email.Subject = message.Subject;
            email.Body = message.Body;

            foreach (MailAddress toRecipient in message.To)
            {
                email.ToRecipients.Add(toRecipient.Address);
            }

            foreach (MailAddress ccRecipient in message.CC)
            {
                email.CcRecipients.Add(ccRecipient.Address);
            }

            foreach (FileInfo file in attachments)
            {
                email.Attachments.AddFileAttachment(file.FullName);
            }

            foreach (string key in message.Headers.AllKeys)
            {
                ExtendedPropertyDefinition headerDefinition = new ExtendedPropertyDefinition(DefaultExtendedPropertySet.InternetHeaders, key, MapiPropertyType.String);
                email.SetExtendedProperty(headerDefinition, message.Headers[key]);
            }

            email.Send();
        }

        /// <summary>
        /// Retrieves all email messages from the specfied <see cref="EmailFolder" />.
        /// </summary>
        /// <param name="folder">The <see cref="EmailFolder" /> to retrieve from.</param>
        /// <returns>A collection of <see cref="EmailMessage" /> objects.</returns>
        public Collection<EmailMessage> RetrieveMessages(EmailFolder folder)
        {
            return RetrieveMessages(folder, int.MaxValue);
        }

        /// <summary>
        /// Retrieves up to the specified number email messages from the specfied <see cref="EmailFolder" />.
        /// </summary>
        /// <param name="folder">The <see cref="EmailFolder" /> to retrieve from.</param>
        /// <param name="itemLimit">The maximum number of messages to retrieve.</param>
        /// <returns>A collection of <see cref="EmailMessage" /> objects.</returns>
        public Collection<EmailMessage> RetrieveMessages(EmailFolder folder, int itemLimit)
        {
            Collection<EmailMessage> messages = new Collection<EmailMessage>();
            UpdateStatus($"Retrieving {folder} messages for {UserName}.", true);

            List<Item> items = RetrieveHeaders(folder, itemLimit).ToList();
            if (items.Any())
            {
                UpdateStatus($"Total email messages found: {items.Count}", true);

                // If the inbox has thousands of emails, then we will timeout waiting to download them all.
                // Work on this in batches.
                const int batchSize = 10;
                for (int i = 0; i < items.Count; i += batchSize)
                {
                    // Read in the next batch.
                    List<Item> batch = new List<Item>();
                    for (int j = 0; j < batchSize && (i + j) < items.Count; j++)
                    {
                        batch.Add(items[i + j]);
                    }

                    UpdateStatus($"Retrieving email messages from {i} to {i + batch.Count} (total {items.Count})");
                    PropertySet messageProperties = new PropertySet(BasePropertySet.FirstClassProperties, ItemSchema.Attachments);
                    var serviceResponses = _service.LoadPropertiesForItems(batch, messageProperties);
                    UpdateStatus("Messages retrieved.");

                    foreach (GetItemResponse response in serviceResponses)
                    {
                        ExchangeMessage message = response.Item as ExchangeMessage;
                        try
                        {
                            messages.Add(new ExchangeEmailMessage(message));
                        }
                        catch (ServiceObjectPropertyException)
                        {
                            // This error can be thrown when the EmailMessage is an automated email sent from the Exchange server,
                            // such as a notification that the inbox is full. It may not contain all the properties of a normal email.
                            LogWarn($"Found invalid email with subject: {message.Subject}");
                        }
                    }
                }
            }
            return messages;
        }

        /// <summary>
        /// Deletes the specified message from the server.
        /// </summary>
        /// <param name="message">The message to delete.</param>
        public void Delete(EmailMessage message)
        {
            Delete(new[] { message });
        }

        /// <summary>
        /// Deletes the specified messages from the server.
        /// </summary>
        /// <param name="messages">The messages to delete.</param>
        /// <exception cref="ArgumentNullException"><paramref name="messages" /> is null.</exception>
        public void Delete(IEnumerable<EmailMessage> messages)
        {
            if (messages == null)
            {
                throw new ArgumentNullException(nameof(messages));
            }

            var ids = messages.OfType<ExchangeEmailMessage>().Select(n => n.ExchangeId);
            UpdateStatus($"Deleting {messages.Count()} messages for {UserName}.", true);
            DeleteHeaders(ids);
        }

        /// <summary>
        /// Deletes all items from the specified folder.
        /// </summary>
        /// <param name="folder">The <see cref="EmailFolder" /> to clear.</param>
        public void Clear(EmailFolder folder)
        {
            UpdateStatus($"Clearing {folder} messages for {UserName}.", true);
            var ids = RetrieveHeaders(folder, int.MaxValue).Select(n => n.Id);
            DeleteHeaders(ids);
        }

        /// <summary>
        /// Creates an <see cref="ExchangeServiceSubscription" /> for the specified <see cref="EmailFolder" />.
        /// </summary>
        /// <param name="folder">The <see cref="EmailFolder" /> to monitor.</param>
        /// <returns>An <see cref="ExchangeServiceSubscription" />.</returns>
        public ExchangeServiceSubscription CreateSubscription(EmailFolder folder)
        {
            return new ExchangeServiceSubscription(_service, _exchangeFolders[folder]);
        }

        private IEnumerable<Item> RetrieveHeaders(EmailFolder emailFolder, int itemLimit)
        {
            try
            {
                UpdateStatus($"Binding to {emailFolder} folder...");
                WellKnownFolderName folderName = _exchangeFolders[emailFolder];
                Folder folder = Folder.Bind(_service, folderName);
                UpdateStatus("Folder bind successful.");

                UpdateStatus($"Retrieving {emailFolder} headers...");
                IEnumerable<Item> items = folder.FindItems(new ItemView(itemLimit));
                UpdateStatus($"{emailFolder} headers retrieved.");

                return items;
            }
            catch (ServiceRequestException ex)
            {
                throw new EmailServerException("Unable to contact the remote email server.", ex);
            }
        }

        private void DeleteHeaders(IEnumerable<ItemId> itemIds)
        {
            if (itemIds.Any())
            {
                _service.DeleteItems(itemIds, DeleteMode.HardDelete, null, null);
            }
        }

        private void UpdateStatus(string message, bool logDebug = false)
        {
            if (logDebug)
            {
                LogDebug(message);
            }
            else
            {
                LogTrace(message);
            }
            StatusUpdate?.Invoke(this, new StatusChangedEventArgs(message));
        }
    }
}
