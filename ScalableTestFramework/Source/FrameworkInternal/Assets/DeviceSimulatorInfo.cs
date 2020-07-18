using System;
using System.Diagnostics;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a software component that simulates a device.
    /// </summary>
    [DataContract]
    public class DeviceSimulatorInfo : DeviceInfo
    {
        [DataMember, DebuggerBrowsable(DebuggerBrowsableState.Never)]
        private readonly string _simulatorType;

        /// <summary>
        /// Gets the type of the simulator.
        /// </summary>
        public string SimulatorType => _simulatorType;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceInfo" /> class.
        /// </summary>
        /// <param name="assetId">The unique identifier for the device.</param>
        /// <param name="attributes">The <see cref="AssetAttributes" /> for the device.</param>
        /// <param name="assetType">The device type.</param>
        /// <param name="address">The device network address.</param>
        /// <param name="adminPassword">The device admin password.</param>
        /// <param name="productName">The device product name.</param>
        /// <param name="simulatorType">The simulator type.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetId" /> is null.
        /// <para>or</para>
        /// <paramref name="address" /> is null.
        /// </exception>
        public DeviceSimulatorInfo(string assetId, AssetAttributes attributes, string assetType, string address, string adminPassword, string productName, string simulatorType)
            : base(assetId, attributes, assetType, address, string.Empty, adminPassword, productName)
        {
            _simulatorType = simulatorType;
        }
    }
}
