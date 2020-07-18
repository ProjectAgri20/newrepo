using System;
using System.IO;
using Microsoft.Exchange.WebServices.Data;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.Email
{
    /// <summary>
    /// Represents a file attached to an email retrieved from a Microsoft Exchange server. 
    /// </summary>
    public sealed class ExchangeEmailAttachment : EmailAttachment
    {
        private readonly FileAttachment _attachment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExchangeEmailAttachment" /> class.
        /// </summary>
        /// <param name="attachment">The attachment retrieved from the Exchange server.</param>
        /// <exception cref="ArgumentNullException"><paramref name="attachment" /> is null.</exception>
        internal ExchangeEmailAttachment(FileAttachment attachment)
        {
            _attachment = attachment ?? throw new ArgumentNullException(nameof(attachment));
            FileName = attachment.Name;
        }

        /// <summary>
        /// Saves the attachment to the specified directory.
        /// </summary>
        /// <param name="directory">The directory to save the file into.</param>
        /// <param name="fileName">The file name to give the downloaded file</param>
        /// <returns>A <see cref="FileInfo" /> object representing the downloaded file.</returns>
        public override FileInfo Save(string directory, string fileName)
        {
            string filePath = Path.Combine(directory, fileName);
            LogDebug($"Downloading attachment '{FileName}' to {filePath}");
            _attachment.Load(filePath);
            return new FileInfo(filePath);
        }
    }
}
