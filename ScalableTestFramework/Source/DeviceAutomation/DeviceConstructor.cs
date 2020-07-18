using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using static HP.ScalableTest.Framework.Logger;

namespace HP.ScalableTest.DeviceAutomation
{
    /// <summary>
    /// Constructor class for creating <see cref="IDevice" /> objects for STF usage,
    /// especially plugin usage.  Leverages properties in <see cref="Framework.Assets.IDeviceInfo" />
    /// to correctly connect to devices using a debug LAN card.
    /// </summary>
    public static class DeviceConstructor
    {
        /// <summary>
        /// Creates an <see cref="IDevice"/> object for the specified <see cref="IDeviceInfo"/> object.
        /// </summary>
        /// <param name="deviceInfo">The device information from Asset Inventory.</param>
        /// <returns></returns>
        public static IDevice Create(Framework.Assets.IDeviceInfo deviceInfo)
        {
            if (deviceInfo == null)
            {
                throw new ArgumentNullException(nameof(deviceInfo));
            }

            DeviceConstructionParameterCollection parameters = new DeviceConstructionParameterCollection()
            {
                new DeviceAddressParameter(deviceInfo.Address),
                new DeviceAdminPasswordParameter(deviceInfo.AdminPassword)
            };

            if (string.IsNullOrEmpty(deviceInfo.Address2))
            {
                LogDebug($"Creating DAT controller for device at {deviceInfo.Address}.  Admin password: '{deviceInfo.AdminPassword}'");
            }
            else
            {
                parameters.Add(new JediDebugAddressParameter(deviceInfo.Address2));
                LogDebug($"Creating DAT controller for device at {deviceInfo.Address} with debug LAN {deviceInfo.Address2}.  Admin password: '{deviceInfo.AdminPassword}'");
            }

            return DeviceFactory.Create(parameters);
        }
    }
}
