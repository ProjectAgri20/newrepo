using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a device asset, such as a printer, scanner, or MFP.
    /// </summary>
    [DataContract]
    public class DeviceInfo : AssetInfo, IDeviceInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _address;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _address2;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _adminPassword;

        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _productName;

        /// <summary>
        /// Gets the network address (IP or hostname) of the device.
        /// </summary>
        public string Address => _address;

        /// <summary>
        /// Gets a secondary address used by the device.
        /// </summary>
        public string Address2 => _address2;

        /// <summary>
        /// Gets the admin password of the device.
        /// </summary>
        public string AdminPassword => _adminPassword;

        /// <summary>
        /// Gets the product name of the device.
        /// </summary>
        public string ProductName => _productName;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInfo" /> class.
        /// </summary>
        /// <param name="assetId">The unique identifier for the device.</param>
        /// <param name="attributes">The <see cref="AssetAttributes" /> for the device.</param>
        /// <param name="assetType">The device type.</param>
        /// <param name="address">The device network address.</param>
        /// <param name="address2">Secondary address used by the device.</param>
        /// <param name="adminPassword">The device admin password.</param>
        /// <param name="productName">The device product name.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetId" /> is null.
        /// <para>or</para>
        /// <paramref name="address" /> is null.
        /// </exception>
        public DeviceInfo(string assetId, AssetAttributes attributes, string assetType, string address, string address2, string adminPassword, string productName)
            : base(assetId, attributes, assetType)
        {
            _address = address ?? throw new ArgumentNullException(nameof(address));
            _address2 = address2;
            _adminPassword = adminPassword;
            _productName = productName;
        }
    }
}
