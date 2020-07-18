using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Framework.Assets;

namespace HP.ScalableTest.Development
{
    /// <summary>
    /// Builder class for creating mock <see cref="AssetInfo" /> objects.
    /// </summary>
    public sealed class AssetInfoBuilder
    {
        /// <summary>
        /// Gets or sets the <see cref="AssetAttributes" /> to be applied to constructed <see cref="AssetInfo" /> objects.
        /// </summary>
        public AssetAttributes Attributes { get; set; } = AssetAttributes.None;

        /// <summary>
        /// Gets or sets the asset type to be applied to constructed <see cref="AssetInfo" /> objects.
        /// </summary>
        public string AssetType { get; set; }

        /// <summary>
        /// Gets or sets the description to be applied to constructed <see cref="AssetInfo" /> objects.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the reservation key to be applied to constructed <see cref="AssetInfo" /> objects.
        /// </summary>
        public string ReservationKey { get; set; }

        /// <summary>
        /// Gets or sets the device address to be applied to constructed <see cref="DeviceInfo" /> objects.
        /// </summary>
        public string DeviceAddress { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the secondary device address to be applied to constructed <see cref="DeviceInfo" /> objects.
        /// </summary>
        public string DeviceAddress2 { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the admin password to be applied to constructed <see cref="DeviceInfo" /> objects.
        /// </summary>
        public string DeviceAdminPassword { get; set; }

        /// <summary>
        /// Gets or sets the product name to be applied to constructed <see cref="DeviceInfo" /> objects.
        /// </summary>
        public string DeviceProductName { get; set; }

        /// <summary>
        /// Gets or sets the printer port to be applied to constructed <see cref="PrintDeviceInfo" /> objects.
        /// </summary>
        public int PrinterPort { get; set; } = 9100;

        /// <summary>
        /// Gets or sets the SNMP-enabled value to be applied to constructed <see cref="PrintDeviceInfo" /> objects.
        /// </summary>
        public bool SnmpEnabled { get; set; } = false;

        /// <summary>
        /// Initializes a new instance of the <see cref="AssetInfoBuilder" /> class.
        /// </summary>
        public AssetInfoBuilder()
        {
            // Constructor explicitly declared for XML doc.
        }

        /// <summary>
        /// Builds a generic <see cref="AssetInfo" /> object with the specified asset ID
        /// and the properties specified in this <see cref="AssetInfoBuilder" />.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <returns>A generic <see cref="AssetInfo" /> object with the specified asset ID.</returns>
        public AssetInfo BuildGenericAsset(string assetId)
        {
            return new GenericAssetInfo(assetId, Attributes, AssetType ?? "Generic Asset")
            {
                Description = Description ?? $"Asset {assetId}"
            };
        }

        /// <summary>
        /// Builds a list of generic <see cref="AssetInfo" /> objects with the specified asset IDs
        /// and the properties specified in this <see cref="AssetInfoBuilder" />.
        /// </summary>
        /// <param name="assetIds">The asset identifiers.</param>
        /// <returns>A list of generic <see cref="AssetInfo"/> objects with the specified asset IDs.</returns>
        public IEnumerable<AssetInfo> BuildGenericAssets(params string[] assetIds)
        {
            return assetIds.Select(BuildGenericAsset);
        }

        /// <summary>
        /// Builds a list of generic <see cref="AssetInfo" /> objects with generated asset IDs
        /// and the properties specified in this <see cref="AssetInfoBuilder" />.
        /// </summary>
        /// <param name="assetIdBase">The base string to use for generating asset identifiers.</param>
        /// <param name="count">The number of assets to generate.</param>
        /// <returns>A list of generic <see cref="AssetInfo"/> objects with generated asset IDs.</returns>
        public IEnumerable<AssetInfo> BuildGenericAssets(string assetIdBase, int count)
        {
            return BuildGenericAssets(BuildNames(assetIdBase, count));
        }

        /// <summary>
        /// Builds a generic <see cref="DeviceInfo" /> object with the specified asset ID
        /// and the properties specified in this <see cref="AssetInfoBuilder" />.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <returns>A generic <see cref="DeviceInfo" /> object with the specified asset ID.</returns>
        public DeviceInfo BuildDevice(string assetId)
        {
            return new DeviceInfo(assetId, Attributes, AssetType ?? "Device", DeviceAddress, "DeviceAddress2", DeviceAdminPassword, DeviceProductName ?? "Device")
            {
                Description = Description ?? $"Device {assetId}"
            };
        }

        /// <summary>
        /// Builds a list of generic <see cref="DeviceInfo" /> objects with the specified asset IDs
        /// and the properties specified in this <see cref="AssetInfoBuilder" />.
        /// </summary>
        /// <param name="assetIds">The asset identifiers.</param>
        /// <returns>A list of generic <see cref="DeviceInfo"/> objects with the specified asset IDs.</returns>
        public IEnumerable<DeviceInfo> BuildDevices(params string[] assetIds)
        {
            return assetIds.Select(BuildDevice);
        }

        /// <summary>
        /// Builds a list of generic <see cref="DeviceInfo" /> objects with generated asset IDs
        /// and the properties specified in this <see cref="AssetInfoBuilder" />.
        /// </summary>
        /// <param name="assetIdBase">The base string to use for generating asset identifiers.</param>
        /// <param name="count">The number of assets to generate.</param>
        /// <returns>A list of generic <see cref="DeviceInfo"/> objects with generated asset IDs.</returns>
        public IEnumerable<DeviceInfo> BuildDevices(string assetIdBase, int count)
        {
            return BuildDevices(BuildNames(assetIdBase, count));
        }

        /// <summary>
        /// Builds a generic <see cref="PrintDeviceInfo" /> object with the specified asset ID
        /// and the properties specified in this <see cref="AssetInfoBuilder" />.
        /// </summary>
        /// <param name="assetId">The asset identifier.</param>
        /// <returns>A generic <see cref="DeviceInfo" /> object with the specified asset ID.</returns>
        public PrintDeviceInfo BuildPrintDevice(string assetId)
        {
            return new PrintDeviceInfo(assetId, Attributes, AssetType ?? "PrintDevice", DeviceAddress, DeviceAddress2, DeviceAdminPassword, DeviceProductName ?? "PrintDevice", PrinterPort, SnmpEnabled)
            {
                Description = Description ?? $"PrintDevice {assetId}"
            };
        }

        /// <summary>
        /// Builds a list of generic <see cref="PrintDeviceInfo" /> objects with the specified asset IDs
        /// and the properties specified in this <see cref="AssetInfoBuilder" />.
        /// </summary>
        /// <param name="assetIds">The asset identifiers.</param>
        /// <returns>A list of generic <see cref="PrintDeviceInfo"/> objects with the specified asset IDs.</returns>
        public IEnumerable<PrintDeviceInfo> BuildPrintDevices(params string[] assetIds)
        {
            return assetIds.Select(BuildPrintDevice);
        }

        /// <summary>
        /// Builds a list of generic <see cref="PrintDeviceInfo" /> objects with generated asset IDs
        /// and the properties specified in this <see cref="AssetInfoBuilder" />.
        /// </summary>
        /// <param name="assetIdBase">The base string to use for generating asset identifiers.</param>
        /// <param name="count">The number of assets to generate.</param>
        /// <returns>A list of generic <see cref="PrintDeviceInfo"/> objects with generated asset IDs.</returns>
        public IEnumerable<PrintDeviceInfo> BuildPrintDevices(string assetIdBase, int count)
        {
            return BuildPrintDevices(BuildNames(assetIdBase, count));
        }

        private static string[] BuildNames(string assetIdBase, int count)
        {
            return Enumerable.Range(1, count).Select(n => assetIdBase + n).ToArray();
        }

        private sealed class GenericAssetInfo : AssetInfo
        {
            public GenericAssetInfo(string assetId, AssetAttributes attributes, string assetType)
                : base(assetId, attributes, assetType)
            {
            }
        }
    }
}
