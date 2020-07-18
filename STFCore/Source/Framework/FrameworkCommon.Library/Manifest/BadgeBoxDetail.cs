using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Contains details and availability information for a badge box used in an enterprise test.
    /// </summary>
    [DataContract]
    [AssetHost("BadgeBoxHost")]
    public class BadgeBoxDetail : AssetDetail
    {
        /// <summary>
        /// Gets or sets the address at which the badgebox can be contacted.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [DataMember]
        public string IPAddress { get; set; }

        /// <summary>
        /// Gets or sets the ID of the BadgeBox.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>        
        [DataMember]
        public string BadgeBoxId { get; set; }

        /// <summary>
        /// Gets or sets the badges belonging to the badgebox.
        /// </summary>
        /// <value>
        /// The BadgeDetails.
        /// </value>
        [DataMember]
        public System.Collections.ObjectModel.Collection<BadgeDetail> BadgeDetails { get; set; }
        /// <summary>
        /// Gets and sets the Printer the badge box belongs to.
        /// </summary>
        [DataMember]
        public string PrinterId { get; set; }

        public BadgeBoxDetail()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BadgeBoxDetail"/> class.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="availabilityEndTime">The availability end time.</param>
        /// <param name="description">The description.</param>
        public BadgeBoxDetail(string deviceId, DateTime availabilityEndTime)
            : base(deviceId, DateTime.MinValue, availabilityEndTime)
        {
            IPAddress = string.Empty;
            BadgeBoxId = null;
            BadgeDetails = null;
        }
    }
}
