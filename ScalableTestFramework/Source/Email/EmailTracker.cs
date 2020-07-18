using System;
using System.Net.Mail;

namespace HP.ScalableTest.Email
{
    /// <summary>
    /// Tags email messages with a custom header that can be used to identify them when retrieved from the server.
    /// </summary>
    public static class EmailTracker
    {
        /// <summary>
        /// The header used to tag messages.
        /// </summary>
        public static readonly string HeaderTag = "x-stf_email_tracker";

        /// <summary>
        /// Tags the specified <see cref="MailMessage" /> with a tracking header.
        /// </summary>
        /// <param name="message">The <see cref="MailMessage" /> to tag.</param>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> is null.</exception>
        public static void Tag(MailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            message.Headers.Add(HeaderTag, "Delete");
        }

        /// <summary>
        /// Determines whether the specified message is tagged.
        /// </summary>
        /// <param name="message">The <see cref="EmailMessage" /> to check.</param>
        /// <returns><c>true</c> if the specified message is tagged; otherwise, <c>false</c>.</returns>
        /// <exception cref="ArgumentNullException"><paramref name="message" /> is null.</exception>
        public static bool IsTagged(EmailMessage message)
        {
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            return message.Headers.ContainsKey(HeaderTag);
        }
    }
}
