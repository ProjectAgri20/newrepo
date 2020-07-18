using System;
using System.Collections.Generic;
using System.Linq;
using System.Xml.Linq;
using HP.DeviceAutomation;
using HP.DeviceAutomation.Jedi;
using HP.ScalableTest.DeviceAutomation;
using HP.ScalableTest.Framework.Manifest;

namespace HP.ScalableTest.Framework.Dispatcher.DeviceSetup
{
    /// <summary>
    /// Uses DAT services to retrieve specific information on Jedi devices.
    /// </summary>
    internal class SessionDeviceSetupManager
    {
        private const string _networkCardModelOid = "1.3.6.1.4.1.11.2.4.3.1.10.0";
        private const string _networkdInterfaceVersionOid = "1.3.6.1.2.1.1.1.0";

        /// <summary>
        /// Gets device information for logging purposes.
        /// </summary>
        /// <param name="sessionDeviceLogger">The Session device logger to be filled in.</param>
        /// <param name="address">The Device IP Address</param>
        /// <param name="address2">The secondary Device IP Address</param>
        /// <param name="adminPassword">The Device Administrator password</param>
        public static void GetSessionDeviceInfo(SessionDeviceLogger sessionDeviceLogger, IDevice device)
        {
            if (sessionDeviceLogger == null)
            {
                throw new ArgumentNullException(nameof(sessionDeviceLogger));
            }

            if (device == null)
            {
                throw new ArgumentNullException(nameof(device));
            }

            SetDeviceLoggerInfo(sessionDeviceLogger, device);
        }

        /// <summary>
        /// Gets Jedi Simulator information for logging purposes.
        /// </summary>
        /// <param name="sessionDeviceLogger">The Session device logger to be filled in.</param>
        /// <param name="address">The Device IP Address</param>
        /// <param name="adminPassword">The Device Administrator password</param>
        public static void GetSessionDeviceInfo(SessionDeviceLogger sessionDeviceLogger, string address, string adminPassword)
        {
            if (sessionDeviceLogger == null)
            {
                throw new ArgumentNullException(nameof(sessionDeviceLogger));
            }

            if (string.IsNullOrEmpty(address))
            {
                throw new ArgumentNullException(nameof(address));
            }

            if (string.IsNullOrEmpty(adminPassword))
            {
                throw new ArgumentNullException(nameof(adminPassword));
            }

            using (JediDevice device = new JediDevice(address, adminPassword))
            {
                SetDeviceLoggerInfo(sessionDeviceLogger, device);
            }
        }

        private static string GetFimBundle(IDevice device)
        {
            string fimBundleVersion = "Unknown";
            if (device.FirmwareType.Equals("Jedi"))
            {

                XElement xeDeviceData = ((HP.DeviceAutomation.Jedi.JediDevice)device)?.WebServices.GetDeviceTicket("fim", "urn:hp:imaging:con:service:fim:FIMService:Assets");


                IEnumerable<XElement> xmlData = from el in xeDeviceData.Descendants()
                                                where el.Name.LocalName == "AssetDescription"
                                                select el;

                if (xmlData.Nodes().Count() > 0)
                {
                    fimBundleVersion = xmlData.First().Value;
                }
            }
            return fimBundleVersion;
        }

        /// <summary>
        /// Sets session device logger values retrieved from the device.
        /// </summary>
        /// <param name="sessionDeviceLogger"></param>
        /// <param name="device"></param>
        private static void SetDeviceLoggerInfo(SessionDeviceLogger sessionDeviceLogger, IDevice device)
        {
            IDeviceInfo deviceInfo = device.GetDeviceInfo();
            sessionDeviceLogger.DeviceName = deviceInfo.ModelName;
            sessionDeviceLogger.FirmwareRevision = deviceInfo.FirmwareRevision;
            sessionDeviceLogger.FirmwareDatecode = deviceInfo.FirmwareDateCode;
            sessionDeviceLogger.FirmwareBundleVersion = GetFimBundle(device);
            sessionDeviceLogger.FirmwareType = device.FirmwareType;
            sessionDeviceLogger.ModelNumber = deviceInfo.ModelNumber;

            Snmp snmp = new Snmp(device.Address);
            sessionDeviceLogger.NetworkCardModel = snmp.Get(_networkCardModelOid);
            sessionDeviceLogger.NetworkInterfaceVersion = snmp.Get(_networkdInterfaceVersionOid);
        }
    }
}
