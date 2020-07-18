using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Contains details and availability information for an addressed device used in an enterprise test.
    /// </summary>
    [DataContract]
    [KnownType(typeof(PrintDeviceDetail))]
    [KnownType(typeof(JediSimulatorDetail))]
    [KnownType(typeof(SiriusSimulatorDetail))]
    public class DeviceAssetDetail : AssetDetail
    {
        /// <summary>
        /// Gets or sets the host name or IP address of the device.
        /// </summary>
        /// <value>
        /// The address.
        /// </value>
        [DataMember]
        public string Address { get; set; }

        /// <summary>
        /// Gets or sets the product name of the device, e.g. Coral.
        /// </summary>
        /// <value>
        /// The product.
        /// </value>
        [DataMember]
        public string Product { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceAssetDetail"/> class.
        /// </summary>
        public DeviceAssetDetail()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceAssetDetail"/> class.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="availabilityEndTime">The availability end time.</param>
        /// <param name="description">The description.</param>
        public DeviceAssetDetail(string assetId, DateTime? availabilityStartTime, DateTime? availabilityEndTime)
            : base(assetId, availabilityStartTime, availabilityEndTime)
        {
            Product = string.Empty;
            Address = string.Empty;
        }
    }
}