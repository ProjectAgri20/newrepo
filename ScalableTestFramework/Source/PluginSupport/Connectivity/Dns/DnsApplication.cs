using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.DnsApp
{
    /// <summary>
    /// Contains DNS operations like Adding Domain, Adding / Deleting Record
    /// </summary>
    public static class DnsApplication
    {
        /// <summary>
        /// Add Domain/Forward Lookup zone to dns server
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="domainName">Domain Name</param>
        /// <example>
        /// Usage: AddDomain(serverIP, domainName)
        /// <code>
        /// AddDomain("win2k8r2sp1dhcp.hssl.com", "hssl.com")
        /// </code>
        /// </example>
        public static bool AddDomain(string domainName)
        {
            Logger.LogInfo("Adding Domain/Forward Lookup zone to DNS server");

            return NetworkUtil.ExecuteCommandAndValidate("dnscmd . /zoneadd {0} /primary \n".FormatWith(domainName));
        }

        /// <summary>
        /// Add Record/hostname to the domain created
        /// </summary>
        /// <param name="domainName">Domain Name</param>
        /// <param name="hostName">Host Name</param>
        /// <param name="recordType">Record Type</param>
        /// <param name="printerIP">Printer IP Address</param>
        /// <returns>true if successful, false otherwise</returns>
        /// <example>
        /// Usage: AddRecord(serverIP, domainName, hostName, recordType, printerIP)
        /// AddRecord("win2k8r2sp1dhcp.hssl.com", "hssl.com", "Default", "A", "192.168.100.16")
        /// </example>
        public static bool AddRecord(string domainName, string hostName, string recordType, string printerIP)
        {
            Logger.LogInfo("Adding Record to the domain created");

            return NetworkUtil.ExecuteCommandAndValidate("dnscmd . /recordadd {0} {1} {2} {3} \n".FormatWith(domainName, hostName, recordType, printerIP));
        }

        /// <summary>
        /// Deletes Record/hostname 
        /// </summary>
        /// <param name="domainName">Domain Name</param>
        /// <param name="hostName">Host Name</param>
        /// <param name="recordType">Record Type</param>
        /// <param name="printerIP">Printer IP Address</param>
        /// <returns>true if successful, false otherwise</returns>
        /// Usage: DeleteRecord(serverIP, domainName, hostName, recordType, printerIP)
        /// <code>
        /// DeleteRecord("win2k8r2sp1dhcp.hssl.com", "hssl.com", "Default", "A", "192.168.100.16")
        /// </code>
        public static bool DeleteRecord(string domainName, string hostName, string recordType, string printerIP)
        {
            Logger.LogInfo("Deleting Record of the domain created");

            return NetworkUtil.ExecuteCommandAndValidate("dnscmd . /recorddelete {0} {1} {2} {3} /f\n".FormatWith(domainName, hostName, recordType, printerIP));
        }

        /// <summary>
        /// Deletes all the records for the given zone and record type from the foward lookup zone
        /// </summary>
        /// <param name="zoneName">Name of the Zone</param>
        /// <param name="recordType">Record Type A, AAAA</param>
        /// <param name="fqdn">domain name of the client hssl.com</param>
        /// <returns>Returns true if all the recrods are deletes else returns false</returns>
        public static bool DeleteRecords(string zoneName, string recordType, string fqdn)
        {
            bool result = true;
            if (string.IsNullOrEmpty(fqdn))
            {
                Logger.LogInfo("Deleting Records of type {0} from zone {1}".FormatWith(recordType, zoneName));

                // First get all the records.
                string output = ProcessUtil.Execute("cmd.exe", "/C dnscmd . /EnumRecords {0} @ /Type {1}".FormatWith(zoneName, recordType)).StandardOutput;
                string[] entries = output.Split('\n');

                // walk thru the records and delete each record
                for (int i = 1; i < entries.Length; i++)
                {
                    // if the line is not empty and doesn't start with end of command line then only delete the record.
                    if (!string.IsNullOrEmpty(entries[i].Trim()) && !entries[i].StartsWith("Command completed successfully.", StringComparison.CurrentCultureIgnoreCase) && !entries[i].StartsWith("Returned records", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string[] data = entries[i].Split(new char[] { ' ', '\t' });
                        result &= DeleteRecord(zoneName, data[0], recordType, data[data.Length - 1]);
                    }
                }
            }
            else
            {
                Logger.LogInfo("Deleting Records of type {0} from the folder {1}".FormatWith(recordType, fqdn));

                // First get all the records.
                string output = ProcessUtil.Execute("cmd.exe", "/C dnscmd . /EnumRecords {0} {1} /Type {2}".FormatWith(zoneName, fqdn, recordType)).StandardOutput;
                string[] entries = output.Split('\n');

                // walk thru the records and delete each record
                for (int i = 1; i < entries.Length; i++)
                {
                    // if the line is not empty and doesn't start with end of command line then only delete the record.
                    if (!string.IsNullOrEmpty(entries[i].Trim()) && !entries[i].StartsWith("Command completed successfully.", StringComparison.CurrentCultureIgnoreCase) && !entries[i].StartsWith("Returned records", StringComparison.CurrentCultureIgnoreCase))
                    {
                        string[] data = entries[i].Split(new char[] { ' ', '\t' });
                        result &= DeleteRecord(zoneName, data[0], recordType, data[data.Length - 1]);
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Deletes all the A and AAAA records for the given zone from the foward lookup zone
        /// </summary>
        /// <param name="zoneName">Name of the Zone</param>
        /// <param name="fqdn">Domain Name of the client</param>
        /// <returns>Returns true if all the recrods are deletes else returns false</returns>
        public static bool DeleteAllRecords(string zoneName, string fqdn = null)
        {
            return DeleteRecords(zoneName, "A", fqdn) && DeleteRecords(zoneName, "AAAA", fqdn);
        }

        /// <summary>
        /// Starts the DNS service.
        /// </summary>
        /// <returns>True if the DNS service is started, else false.</returns>
        public static bool StartDnsService()
        {
            if (IsDnsServiceRunning())
            {
                return true;
            }
            else
            {
                string result = ProcessUtil.Execute("cmd.exe", "/C net start dns").StandardOutput;

                if (result.Contains("started successfully", StringComparison.CurrentCultureIgnoreCase) || result.Contains("already been started", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Stops the DNS service
        /// </summary>
        /// <returns>True if the DNS service is stopped, else false.</returns>
        public static bool StopDnsService()
        {
            if (!IsDnsServiceRunning())
            {
                return true;
            }
            else
            {
                string result = ProcessUtil.Execute("cmd.exe", "/C net stop dns").StandardOutput;

                if (result.Contains("stopped successfully", StringComparison.CurrentCultureIgnoreCase) || result.Contains("not started", StringComparison.CurrentCultureIgnoreCase))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }

        /// <summary>
        /// Check whether DNS Service is running on DHCP server
        /// </summary>
        /// <returns>true if DNS Service is running, false otherwise</returns>
        public static bool IsDnsServiceRunning()
        {
            return ProcessUtil.Execute("cmd.exe", "/C sc query dns").StandardOutput.Contains("running", StringComparison.CurrentCultureIgnoreCase) ? true : false;
        }

        /// <summary>
        /// Validates a record under the specified domain based on the specified <see cref="DnsRecordType"/>.
        /// </summary>
        /// <param name="serverIP">IP Address of the DNS server.</param>
        /// <param name="domainName">The domain name where the record is added.</param>
        /// <param name="recordType"><see cref="DnsRecordType"/></param>
        /// <param name="hostName">Host name of the device.</param>
        /// <param name="ipAddress">IP address of the device.</param>
        /// <returns>True if the record is present, else false.</returns>
        public static bool ValidateRecord(string serverIP, string domainName, DnsRecordType recordType, string hostName, string ipAddress)
        {
            string result = string.Empty;

            if (recordType == DnsRecordType.A)
            {
                result = ProcessUtil.Execute("cmd.exe", "/C dnscmd . /zoneprint {0}".FormatWith(domainName)).StandardOutput;
                return result.Contains(hostName, StringComparison.CurrentCultureIgnoreCase) && result.Contains(ipAddress, StringComparison.CurrentCultureIgnoreCase);
            }
            else
            {
                // For reverse lookup zone, if the server IP address is 192.168.200.254, the domain name is 200.168.192.in-addr.arpa.
                //string [] values = serverIP.Split('.');
                string[] values = ipAddress.Split('.');
                domainName = "{0}.{1}.{2}.in-addr.arpa".FormatWith(values[2], values[1], values[0]);
                result = ProcessUtil.Execute("cmd.exe", "/C dnscmd . /zoneprint {0}".FormatWith(domainName)).StandardOutput;

                return result.Contains(hostName, StringComparison.CurrentCultureIgnoreCase);
            }
        }
    }
}
