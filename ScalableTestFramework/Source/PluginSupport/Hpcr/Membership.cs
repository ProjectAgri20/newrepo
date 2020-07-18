using System.Runtime.Serialization;

namespace HP.ScalableTest.PluginSupport.Hpcr
{
    /// <summary>
    /// Details about an HPCR membership.
    /// </summary>
    [DataContract]
    public class Membership
    {
        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        /// <value>
        /// The unique identifier.
        /// </value>
        [DataMember]
        public string Guid { get; set; }

        /// <summary>
        /// Gets or sets the distinguished name.
        /// </summary>
        /// <value>
        /// The distinguished name.
        /// </value>
        [DataMember]
        public string DistinguishedName { get; set; }
    }
}
