using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Dispatcher
{
    

    /// <summary>
    /// 
    /// </summary>
    [DataContract]
    public class AssociatedProductSerializable
    {
        /// <summary>
        /// Associated Product Id
        /// </summary>
        [DataMember]
        public Guid AssociatedProductId { get; set; }

        /// <summary>
        /// Name of the product
        /// </summary>
        [DataMember]
        public string Name { get; set; }

        /// <summary>
        /// Vendor
        /// </summary>
        [DataMember]
        public string Vendor { get; set; }

        /// <summary>
        /// Version
        /// </summary>
        [DataMember]
        public string Version { get; set; }

        /// <summary>
        /// Match Criteria
        /// </summary>
        [DataMember]
        public bool Active { get; set; }
    }
}
