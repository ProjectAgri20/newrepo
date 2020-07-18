namespace HP.ScalableTest.Framework.Assets
{
    /// <summary>
    /// Information about a device, including capabilities, network address, and admin password.
    /// </summary>
    public interface IDeviceInfo : IAssetInfo
    {
        /// <summary>
        /// Gets the network address (IP or hostname) of the device.
        /// </summary>
        string Address { get; }

        /// <summary>
        /// Gets a secondary address used by the device.
        /// </summary>
        string Address2 { get; }

        /// <summary>
        /// Gets the admin password of the device.
        /// </summary>
        string AdminPassword { get; }

        /// <summary>
        /// Gets the product name of the device.
        /// </summary>
        string ProductName { get; }
    }
}
