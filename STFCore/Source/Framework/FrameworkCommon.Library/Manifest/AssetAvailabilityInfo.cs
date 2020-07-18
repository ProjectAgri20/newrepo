using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    [DataContract]
    public sealed class AssetAvailabilityInfo
    {
        public string AssetId { get; set; }

        public bool Enabled { get; set; }

        public AssetAvailability Availability { get; set; }

        public DateTime AvailabilityEndTime { get; set; }

        public AssetAvailabilityInfo(string assetId, AssetAvailability availability, DateTime availabilityEndTime)
        {
            AssetId = assetId;
            Availability = availability;
            AvailabilityEndTime = availabilityEndTime;
        }

        public AssetAvailabilityInfo(AssetDetail detail)
        {
            AssetId = detail.AssetId;
            Availability = detail.Availability;
            AvailabilityEndTime = detail.AvailabilityEndTime;
        }
    }
}