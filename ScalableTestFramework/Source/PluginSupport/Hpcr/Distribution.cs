using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.PluginSupport.Hpcr
{
    /// <summary>
    /// Details about an HPCR distribution.
    /// </summary>
    [DataContract]
    public sealed class Distribution
    {
        /// <summary>
        /// Gets or sets the title.
        /// </summary>
        /// <value>
        /// The title.
        /// </value>
        [DataMember]
        public string Title { get; set; }

        /// <summary>
        /// Gets or sets the name of the application.
        /// </summary>
        /// <value>
        /// The name of the application.
        /// </value>
        [DataMember]
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the application tag.
        /// </summary>
        /// <value>
        /// The application tag.
        /// </value>
        [DataMember]
        public string ApplicationTag { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [use only once].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [use only once]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool UseOnlyOnce { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether this <see cref="Distribution"/> is public.
        /// </summary>
        /// <value>
        ///   <c>true</c> if public; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool Public { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether [show device at top].
        /// </summary>
        /// <value>
        ///   <c>true</c> if [show device at top]; otherwise, <c>false</c>.
        /// </value>
        [DataMember]
        public bool ShowDeviceAtTop { get; set; }

        /// <summary>
        /// Gets or sets the scan settings.
        /// </summary>
        /// <value>
        /// The scan settings.
        /// </value>
        [DataMember]
        public string ScanSettings { get; set; }

        /// <summary>
        /// Gets or sets the subject.
        /// </summary>
        /// <value>
        /// The subject.
        /// </value>
        [DataMember]
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the comment.
        /// </summary>
        /// <value>
        /// The comment.
        /// </value>
        [DataMember]
        public string Comment { get; set; }

        /// <summary>
        /// Gets or sets the type.
        /// </summary>
        /// <value>
        /// The type.
        /// </value>
        [DataMember]
        public string Type { get; set; }

        /// <summary>
        /// Gets or sets the date created.
        /// </summary>
        /// <value>
        /// The date created.
        /// </value>
        [DataMember]
        public DateTime DateCreated { get; set; }

        /// <summary>
        /// Gets or sets the recipients.
        /// </summary>
        /// <value>
        /// The recipients.
        /// </value>
        [DataMember]
        public string Recipients { get; set; }
    }
}
