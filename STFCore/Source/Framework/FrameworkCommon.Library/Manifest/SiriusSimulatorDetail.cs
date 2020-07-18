using System;
using System.Runtime.Serialization;
using HP.ScalableTest.Framework.Settings;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Contains details and availability information for a Sirius Simulator.
    /// </summary>
    [DataContract]
    [AssetHost("SiriusSimulatorHost")]
    public class SiriusSimulatorDetail : DeviceAssetDetail
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusSimulatorDetail"/> class.
        /// </summary>
        public SiriusSimulatorDetail()
            : base()
        {
            AdminPassword = GlobalSettings.Items[Setting.DeviceAdminPassword];
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SiriusSimulatorDetail"/> class.
        /// </summary>
        /// <param name="deviceId">The device id (AssetId)</param>
        /// <param name="availabilityEndTime">The simulator availability end time.</param>
        /// <param name="description">The simulator description.</param>
        public SiriusSimulatorDetail(string deviceId, DateTime? availabilityStartTime, DateTime? availabilityEndTime)
            : base(deviceId, availabilityStartTime, availabilityEndTime)
        {
        }

        /// <summary>
        /// Gets or sets the device admin password.
        /// </summary>
        [DataMember]
        public string AdminPassword { get; set; }

    }
}
