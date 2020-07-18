using System;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.Plugin.EwsHeadless
{
    /// <summary>
    /// Factory class for creating <see cref="IEwsCommunicator"/> instances.
    /// </summary>
    public class EwsCommunicatorFactory
    {
        /// <summary>
        /// Creates an <see cref="IEwsCommunicator"/> for the specified <see cref="IDevice"/>.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <param name="password"></param>
        /// <returns>An <see cref="IEwsCommunicator"/> for the specified device.</returns>
        public static IEwsCommunicator Create(IDevice device, string password)
        {
            if (device is PhoenixDevice)
            {
                return (new PhoenixCommunicator(device, password));
            }
            if (device is SiriusDevice)
            {
                return (new SiriusCommunicator(device, password));
            }
            if (device is JediWindjammerDevice)
            {
                return (new JediCommunicator(device, password));
            }
            if (device is JediOmniDevice)
            {
                return (new OmniCommunicator(device, password));
            }
            throw new ArgumentException("Could not find the appropriate communicator to communicate with the device");
        }

        /// <summary>
        /// Creates the Communicator interface
        /// </summary>
        /// <param name="device">device</param>
        /// <returns></returns>
        public static IEwsCommunicator Create(IDevice device)
        {
            if (device is PhoenixDevice)
            {
                return !string.IsNullOrEmpty(device.AdminPassword) ? new PhoenixCommunicator(device, device.AdminPassword) : new PhoenixCommunicator(device, string.Empty);
            }
            if (device is SiriusDevice)
            {
                return !string.IsNullOrEmpty(device.AdminPassword) ? new SiriusCommunicator(device, device.AdminPassword) : new SiriusCommunicator(device, string.Empty);
            }
            if (device is JediWindjammerDevice)
            {
                return !string.IsNullOrEmpty(device.AdminPassword) ? new JediCommunicator(device, device.AdminPassword) : new JediCommunicator(device, string.Empty);
            }
            if (device is JediOmniDevice)
            {
                return !string.IsNullOrEmpty(device.AdminPassword) ? new OmniCommunicator(device, device.AdminPassword) : new OmniCommunicator(device, string.Empty);
            }
            throw new ArgumentException("Could not find the appropriate communicator to communicate with the device");
        }
    }
}