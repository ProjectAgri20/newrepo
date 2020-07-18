using System;
using System.Runtime.Serialization;

namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// <see cref="PrintDeviceInfo" /> class with internal extensions.
    /// </summary>
    [DataContract]
    public class PrintDeviceInfoInternal : PrintDeviceInfo
    {
        /// <summary>
        /// Gets or sets the model name.
        /// </summary>
        [DataMember]
        public string ModelName { get; set; }

        /// <summary>
        /// Gets or sets the model number.
        /// </summary>
        [DataMember]
        public string ModelNumber { get; set; }

        /// <summary>
        /// Gets or sets the serial number.
        /// </summary>
        [DataMember]
        public string SerialNumber { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="PrintDeviceInfo" /> class.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <param name="attributes">The asset attributes.</param>
        /// <param name="assetType">The asset type.</param>
        /// <param name="address">The device address.</param>
        /// <param name="address2">Secondary address used by the device.</param>
        /// <param name="adminPassword">The device admin password.</param>
        /// <param name="productName">The product name.</param>
        /// <param name="printerPort">The port used for printing to the device.</param>
        /// <param name="snmpEnabled">if set to <c>true</c> SNMP is enabled for the device.</param>
        /// <exception cref="ArgumentNullException">
        /// <paramref name="assetId" /> is null.
        /// <para>or</para>
        /// <paramref name="address" /> is null.
        /// </exception>
        public PrintDeviceInfoInternal(string assetId, AssetAttributes attributes, string assetType, string address, string address2, string adminPassword, string productName, int printerPort, bool snmpEnabled)
            : base(assetId, attributes, assetType, address, address2, adminPassword, productName, printerPort, snmpEnabled)
        {
        }
    }
}
