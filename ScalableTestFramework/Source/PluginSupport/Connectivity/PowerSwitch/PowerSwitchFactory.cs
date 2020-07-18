using System;
using System.Net;

using HP.DeviceAutomation;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.PowerSwitch
{
    /// <summary>
    /// Factory for creating <see cref="IPowerSwitch" /> objects.
    /// </summary>
    public class PowerSwitchFactory
    {
        /// <summary>
        /// Creates an instance of <see cref="IPowerSwitch"/> for the powe switch.
        /// </summary>
        /// <param name="address">The ip address of the switch</param>
        /// <returns></returns>
        public static IPowerSwitch Create(IPAddress address)
        {
            var snmp = new Snmp(address);

            // Get the System Description to identify the vendor and model
            string desciption = snmp.Get("1.3.6.1.2.1.1.1.0");

            if (desciption.EqualsIgnoreCase("IP9258"))
            {
                return new AvioSysPowerSwitch(address);
            }
            else
            {
                // Implementation is avilable only for AvioSys-IP9258
                throw new NotImplementedException($"{desciption} not implemented");
            }
        }
    }
}
