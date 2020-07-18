using System;
using HP.DeviceAutomation;

namespace HP.ScalableTest.DeviceAutomation.InfoCollection.DeviceUsage
{
    /// <summary>
    /// Retrieves device usage counts from devices.
    /// </summary>
    public sealed class DeviceUsageCollector
    {
        // NPI product Bugatti does not use the OID for total print count

        private const string _totalMonoPageCountOid = "1.3.6.1.4.1.11.2.3.9.4.2.1.4.1.2.6.0";
        private const string _totalColorPageCountOid = "1.3.6.1.4.1.11.2.3.9.4.2.1.4.1.2.7.0";
        private const string _totalPrintCountOid = "1.3.6.1.4.1.11.2.3.9.4.2.1.1.16.1.9.0";
        private const string _totalFlatbedCountOid = "1.3.6.1.4.1.11.2.3.9.4.2.1.2.2.1.21.0";
        private const string _totalAdfCountOid = "1.3.6.1.4.1.11.2.3.9.4.2.1.2.2.1.20.0";
        private const string _totalFaxCountOid = "1.3.6.1.4.1.11.2.3.9.4.2.1.3.7.1.32.0";
        private readonly Snmp _snmp;

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceUsageCollector" /> class.
        /// </summary>
        /// <param name="address">The device address.</param>
        public DeviceUsageCollector(string address)
            : this(new Snmp(address))
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="DeviceUsageCollector" /> class.
        /// </summary>
        /// <param name="snmp">The SNMP.</param>
        public DeviceUsageCollector(Snmp snmp)
        {
            _snmp = snmp;
        }

        /// <summary>
        /// Collects the device usage counts from the device.
        /// </summary>
        /// <returns>A <see cref="DeviceUsageCounts" /> object.</returns>
        public DeviceUsageCounts CollectUsageCounts()
        {
            DeviceUsageCounts duc = new DeviceUsageCounts();

            duc.TotalMonoPageCount = SnmpPageCount(_totalMonoPageCountOid);
            duc.TotalColorPageCount = SnmpPageCount(_totalColorPageCountOid);

            int count = SnmpPageCount(_totalPrintCountOid);

            if (count != 0)
            {
                duc.PrintTotalImages = count;
            }
            else
            {
                duc.PrintTotalImages = duc.TotalColorPageCount + duc.TotalMonoPageCount;
            }

            duc.FlatbedTotalImages = SnmpPageCount(_totalFlatbedCountOid);
            duc.AdfTotalImages = SnmpPageCount(_totalAdfCountOid);
            duc.TotalFaxCount = SnmpPageCount(_totalFaxCountOid);

            return duc;
        }
        /// <summary>
        /// Collects Device Counts from a device, for STF use
        /// </summary>
        /// <returns>A <see cref="DeviceUsageCounts" /> object.</returns>
        public DeviceUsageCounts CollectSFPUsageCounts()
        {
            return new DeviceUsageCounts
            {
                PrintTotalImages = SnmpPageCount(_totalPrintCountOid)
            };
        }
        private int SnmpPageCount(string oid)
        {
            int count = 0;

            try
            {
                count = _snmp.Get<int>(oid);
            }
            catch (SnmpException ex) when (ex.SnmpNameNotFound)
            {
                count = 0;
            }
            catch (System.FormatException)
            {
                count = 0;
            }

            return count;
        }
    }
}
