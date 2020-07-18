using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.Email
{
    /// <summary>
    /// Represents the data needed to execute an Email send activity.
    /// </summary>
    [DataContract]
    public class EmailActivityData
    {
        /// <summary>
        /// Gets or sets the number of "To" recipients.
        /// </summary>
        [DataMember]
        public int ToRandomCount { get; set; }

        /// <summary>
        /// Gets or sets the number of "CC" recipients.
        /// </summary>
        [DataMember]
        public int CCRandomCount { get; set; }

        /// <summary>
        /// Gets or sets the subject line of the e-mail activity.
        /// </summary>
        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body of the e-mail activity.
        /// </summary>
        [DataMember]
        public string Body { get; set; }

        /// <summary>
        /// Gets or sets whether all documents found from <see cref="AttachmentsSelection"/> are used.
        /// </summary>
        [DataMember]
        public bool SelectAllDocuments { get; set; }

        /// <summary>
        /// Gets or sets the number of documents to include in the attachment.
        /// </summary>
        [DataMember]
        public int NumberOfDocuments { get; set; }

        /// <summary>
        /// Initializes a new instance of <see cref="EmailActivityData"/>
        /// </summary>
        public EmailActivityData()
        {
            ToRandomCount = 1;
            CCRandomCount = 0;
            Subject = string.Empty;
            Body = string.Empty;
            SelectAllDocuments = true;
            NumberOfDocuments = 1;
        }
    }
}
