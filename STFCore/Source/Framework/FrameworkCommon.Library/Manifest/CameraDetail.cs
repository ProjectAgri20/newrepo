using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{

    /// <summary>
    /// Contains details and availability information for a camera used in an enterprise test.
    /// </summary>
    [DataContract]
    [AssetHost("CameraHost")]
    public class CameraDetail : AssetDetail
    {
        /// <summary>
        /// Gets or sets the address at which the camera can be contacted.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the ID of the camera.
        /// </summary>
        /// <value>
        /// The id.
        /// </value>
        
        [DataMember]
        public string CameraId { get; set; }

        /// <summary>
        /// Gets and sets the Printer the camera belongs to.
        /// </summary>
        [DataMember]
        public string PrinterId { get; set; }

        /// <summary>
        /// Gets and sets the Camera Server of the Camera.
        /// </summary>
        [DataMember]
        public string CameraServer { get; set; }

        /// <summary>
        /// Gets and set the Username for logging into the Camera server.
        /// </summary>
        [DataMember]
        public string ServerUser { get; set; }

        /// <summary>
        /// Gets and sets the password for logging into the Camera server.
        /// </summary>
        [DataMember]
        public string ServerPassword { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether AutoStart mode needs to be turned on.
        /// </summary>
        [DataMember]
        public bool UseAutoStart { get; set; }

        public CameraDetail()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CameraDetail"/> class.
        /// </summary>
        /// <param name="assetId">The device id.</param>
        /// <param name="availabilityEndTime">The availability end time.</param>
        /// <param name="description">The description.</param>
        public CameraDetail(string assetId, DateTime? availabilityStartTime, DateTime? availabilityEndTime)
            : base(assetId, availabilityStartTime, availabilityEndTime)
        {
            Address = string.Empty;
            CameraId = string.Empty;
            CameraServer = string.Empty;
            UseAutoStart = true;
        }
    }
}
