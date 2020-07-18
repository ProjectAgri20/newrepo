using System;
using System.Net;

using HP.DeviceAutomation;

namespace HP.ScalableTest.PluginSupport.Connectivity.PowerSwitch
{
    /// <summary>
    /// Implementation of <see cref="IPowerSwitch"/> for a <see cref="AvioSysPowerSwitch"/>
    /// </summary>
    class AvioSysPowerSwitch : IPowerSwitch
    {
        readonly IPAddress _address;
        string powerOidFormat = ".1.3.6.1.4.1.92.58.2.{0}.0";

        /// <summary>
        /// Creates an instance of <see cref="AvioSysPowerSwitch"/>
        /// </summary>
        /// <param name="address">IP address of the power switch</param>
        public AvioSysPowerSwitch(IPAddress address)
        {
            _address = address;
        }

        /// <summary>
        /// Power off the specified port
        /// </summary>
        /// <param name="portNumber">The port number</param>
        /// <returns>True if the operation is successful.</returns>
        public bool PowerOff(int portNumber)
        {
            return TogglePower(portNumber, false);
        }

        /// <summary>
        /// Power on the specified port
        /// </summary>
        /// <param name="portNumber">The port number</param>
        /// <returns>True if the operation is successful.</returns>
        public bool PowerOn(int portNumber)
        {
            return TogglePower(portNumber, true);
        }

        /// <summary>
        /// Toggle the power on the specified port
        /// </summary>
        /// <param name="portNumber">The port number</param>
        /// <param name="status">True to turn on the port, false to turn off</param>
        /// <returns>True if the operation is successful.</returns>
        private bool TogglePower(int portNumber, bool status)
        {
            var snmp = new Snmp(_address);
            var oid = string.Format(powerOidFormat, portNumber);

            snmp.Set(oid, Convert.ToInt32(status));
            return Convert.ToInt32(snmp.Get(oid)) == Convert.ToInt32(status);
        }
    }
}
