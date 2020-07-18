using System.Runtime.CompilerServices;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.DeviceAutomation.Phoenix;
using HP.DeviceAutomation.Sirius;

namespace HP.ScalableTest.DeviceAutomation.Wireless
{
    /// <summary>
    /// Factory for creating <see cref="IWireless" /> objects.
    /// </summary>
    public sealed class WirelessFactory : DeviceFactoryCore<IWireless>
    {
        private static WirelessFactory _instance;

        /// <summary>
        /// Creates an <see cref="IWireless" /> for the specified <see cref="IDevice" />.
        /// </summary>
        /// <param name="device">The device.</param>
        /// <returns>An <see cref="IWireless" /> for the specified device.</returns>
        public static IWireless Create(IDevice device)
        {
            _instance = new WirelessFactory(device);
            return _instance.FactoryCreate(device);
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="WirelessFactory" /> class.
        /// </summary>
        public WirelessFactory(IDevice device)
        {
            if (GetDeviceInterfaceType(device) == DeviceInterfaceType.MultipleInterface)
            {
                Add<JediOmniDevice, JediOmniMultipleInterfaceWireless>();
            }
            else if(GetDeviceInterfaceType(device) == DeviceInterfaceType.SingleInterface)
            {
                Add<JediOmniDevice, JediOmniSingleInterfaceWireless>();
            }

            Add<PhoenixMagicFrameDevice, PhoenixMagicWireless>();
            Add<PhoenixNovaDevice, PhoenixNovaWireless>();
            Add<SiriusUIv3Device, SiriusUIv3Wireless>();
        }

        private static DeviceInterfaceType GetDeviceInterfaceType(IDevice device)
        {
            var snmp = new Snmp(device.Address);
            var description = snmp.Get("1.3.6.1.2.1.1.1.0");

            return description.Contains("JDI") ? DeviceInterfaceType.MultipleInterface : DeviceInterfaceType.SingleInterface;
        }
    }
}
