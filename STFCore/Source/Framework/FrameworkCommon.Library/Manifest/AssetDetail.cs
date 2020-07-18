using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Core.AssetInventory.Reservation;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Contains details and availability information for any asset used in an enterprise test.
    /// </summary>
    /// <remarks>
    /// This class and any inheriting classes must use an <see cref="KnownTypeAttribute"/>
    /// to ensure that these classes serialize correctly.
    /// </remarks>
    [DataContract]
    [KnownType(typeof(DeviceAssetDetail))]
    [KnownType(typeof(CardReaderDetail))]
    [KnownType(typeof(BadgeBoxDetail))]
    [KnownType(typeof(CameraDetail))]
    [KnownType(typeof(MobileDeviceDetail))]
    public class AssetDetail : IAvailable
    {
        /// <summary>
        /// Gets or sets the asset id.
        /// </summary>
        [DataMember]
        public string AssetId { get; set; }

        /// <summary>
        /// Gets or sets the availability start time.
        /// </summary>
        [DataMember]
        public DateTime? AvailabilityStartTime { get; set; }

        /// <summary>
        /// Gets or sets the availability end time.
        /// </summary>
        [DataMember]
        public DateTime? AvailabilityEndTime { get; set; }

        /// <summary>
        /// Gets or sets the availability of the Asset.
        /// </summary>
        [DataMember]
        public AssetAvailability Availability { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        [DataMember]
        public string Description { get; set; }

        /// <summary>
        /// Gets the Id of this IAvailable object.
        /// </summary>
        public string Id { get { return AssetId; } }

        /// <summary>
        /// Gets the InventoryId of this IAvailable object.
        /// </summary>        
        public string InventoryId { get { return AssetId; } }

        /// <summary>
        /// Gets or sets the Available status of the Asset.
        /// </summary>
        [DataMember]
        public bool Available { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetDetail"/> class.
        /// </summary>
        public AssetDetail()
            : this(string.Empty, DateTime.MinValue, DateTime.MinValue)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetDetail"/> class.
        /// </summary>
        /// <param name="assetId">The asset id.</param>
        /// <param name="availabilityEndTime">The availability end time.</param>
        /// <param name="description">The description.</param>
        public AssetDetail(string assetId, DateTime? availabilityStartTime, DateTime? availabilityEndTime)
        {
            AssetId = assetId;
            AvailabilityStartTime = availabilityStartTime;
            AvailabilityEndTime = availabilityEndTime;
            Availability = AssetAvailability.Unknown;
            Available = DateTime.Now > availabilityStartTime && DateTime.Now < availabilityEndTime;
        }
    }
}