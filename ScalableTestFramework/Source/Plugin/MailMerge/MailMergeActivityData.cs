using System.Collections.ObjectModel;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Plugin.MailMerge
{
    /// <summary>
    /// Mail Merge Activity Data
    /// </summary>
    [DataContract]
    public class MailMergeActivityData
    {
        /// <summary>
        /// Mail Merge Format - Either Letter or Envelope
        /// </summary>
        [DataMember]
        public MailMergeFormat Format { get; set; }

        /// <summary>
        /// Contains information about the source or originator
        /// </summary>
        [DataMember]
        public Originator Originator { get; set; }

        /// <summary>
        /// Collection of recipients who will receive the message
        /// </summary>
        [DataMember]
        public Collection<Recipients> RecipientCollection { get; set; }

        /// <summary>
        /// The message body contains the message from the recipients
        /// </summary>
        [DataMember]
        public string MessageBody { get; set; }

        /// <summary>
        /// whether to print or not print a job separator page
        /// </summary>
        [DataMember]
        public bool PrintJobSeparator { get; set; }

        /// <summary>
        /// default constructor
        /// </summary>
        public MailMergeActivityData()
        {
            RecipientCollection = new Collection<Recipients>();
        }
    }

    /// <summary>
    /// Recipient class
    /// </summary>
    [DataContract]
    public class Recipients
    {
        /// <summary>
        /// Name of the recipient
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Address of the recipient
        /// </summary>
        [DataMember]
        public string Address { get; set; }
    }

    /// <summary>
    /// Originator class
    /// </summary>
    [DataContract]
    public class Originator
    {
        /// <summary>
        /// Name of the originator
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// The designation of the originator, eg: VP, CEO
        /// </summary>
        [DataMember]
        public string Designation { get; set; }

        /// <summary>
        /// The department to which the originator belonds, for example: PPS, ES, etc.
        /// </summary>
        [DataMember]
        public string Department { get; set; }

        /// <summary>
        /// The company of the originator
        /// </summary>
        [DataMember]
        public string Company { get; set; }
    }

    /// <summary>
    /// Mail Merge format
    /// </summary>
    public enum MailMergeFormat
    {
        /// <summary>
        /// Letter or envelope
        /// </summary>
        Letter,

        /// <summary>
        /// Envelope
        /// </summary>
        Envelope
    }
}