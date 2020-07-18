using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Manifest
{
    /// <summary>
    /// Contains details and availability information for a Jedi Simulator.
    /// </summary>
    [DataContract]
    [AssetHost("JediSimulatorHost")]
    public class JediSimulatorDetail : DeviceAssetDetail
    {
        /// <summary>
        /// Gets or sets the VM hostname (used to boot the simulator virtual machine).
        /// </summary>
        [DataMember]
        public string MachineName { get; set; }

        /// <summary>
        /// Gets or sets the device admin password.
        /// </summary>
        [DataMember]
        public string AdminPassword { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediSimulatorDetail"/> class.
        /// </summary>
        public JediSimulatorDetail()
            : base()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="JediSimulatorDetail"/> class.
        /// </summary>
        /// <param name="deviceId">The device id.</param>
        /// <param name="availabilityEndTime">The availability end time.</param>
        /// <param name="description">The description.</param>
        public JediSimulatorDetail(string deviceId, DateTime? availabilityStartTime, DateTime? availabilityEndTime)
            : base(deviceId, availabilityStartTime, availabilityEndTime)
        {
            MachineName = string.Empty;
        }
    }
}