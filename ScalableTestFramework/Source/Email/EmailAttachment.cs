using System.IO;

namespace HP.ScalableTest.Email
{
    /// <summary>
    /// Represents a file attached to an email retrieved from an email server. 
    /// </summary>
    public abstract class EmailAttachment
    {
        /// <summary>
        /// Gets the file name of the attachment.
        /// </summary>
        public string FileName { get; protected set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailAttachment"/> class.
        /// </summary>
        protected EmailAttachment()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Saves the attachment to the specified directory.
        /// </summary>
        /// <param name="directory">The directory to save the file into.</param>
        /// <returns>A <see cref="FileInfo" /> object representing the downloaded file.</returns>
        public FileInfo Save(string directory)
        {
            return Save(directory, FileName);
        }

        /// <summary>
        /// Saves the attachment to the specified directory.
        /// </summary>
        /// <param name="directory">The directory to save the file into.</param>
        /// <param name="fileName">The file name to give the downloaded file</param>
        /// <returns>A <see cref="FileInfo" /> object representing the downloaded file.</returns>
        public abstract FileInfo Save(string directory, string fileName);
    }
}
