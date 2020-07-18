using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Contains information for an EPR profile.
    /// </summary>
    [DataContract]
    public class DeviceProfileDetail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceProfileDetail" /> class.
        /// </summary>
        public DeviceProfileDetail()
        {
        }

        /// <summary>
        /// Gets or sets the id.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        public Guid ProfileId { get; set; }

        /// <summary>
        /// Gets or sets the location.
        /// </summary>
        /// <value>
        /// The location.
        /// </value>
        public string Location { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; }
    }
}