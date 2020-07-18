using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a mobile device, such as a phone or tablet.
    /// </summary>
    [DataContract]
    public class MobileDeviceInfo : AssetInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _mobileEquipmentId;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly MobileDeviceType _mobileDeviceType;

        /// <summary>
        /// Gets the mobile equipment ID (IMEI) of the device.
        /// </summary>
        public string MobileEquipmentId => _mobileEquipmentId;

        /// <summary>
        /// Gets the mobile device type.
        /// </summary>
        public MobileDeviceType MobileDeviceType => _mobileDeviceType;

        /// <summary>
        /// Initializes a new instance of the <see cref="MobileDeviceInfo" /> class.
        /// </summary>
        /// <param name="assetId">The unique identifier for the device.</param>
        /// <param name="attributes">The <see cref="AssetAttributes" /> for the device.</param>
        /// <param name="assetType">The device type.</param>
        /// <param name="mobileEquipmentId">The mobile equipment identifier.</param>
        /// <param name="mobileDeviceType">The mobile device type.</param>
        /// <exception cref="ArgumentNullException"><paramref name="mobileEquipmentId" /> is null.</exception>
        public MobileDeviceInfo(string assetId, AssetAttributes attributes, string assetType, string mobileEquipmentId, MobileDeviceType mobileDeviceType)
            : base(assetId, attributes, assetType)
        {
            _mobileEquipmentId = mobileEquipmentId ?? throw new ArgumentNullException(nameof(mobileEquipmentId));
            _mobileDeviceType = mobileDeviceType;
        }
    }
}
