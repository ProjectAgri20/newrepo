using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Management;
using System.Net;

namespace HP.ScalableTest.Framework
{
    /// <summary>
    /// Class that integrates with WMI to collect system information
    /// </summary>
    public static class WindowsManagementInstrumentation
    {
        /// <summary>
        /// Gets the <see cref="WindowsSystemInfo"/> for the defined host.
        /// </summary>
        /// <param name="hostName">The hostname to query.</param>
        /// <param name="credential">The credentials to use when connecting to the host.</param>
        /// <returns>A <see cref="WindowsSystemInfo"/> object containing system information.</returns>
        public static WindowsSystemInfo GetSystemInformation(string hostName, NetworkCredential credential = null)
        {
            ConnectionOptions connectionOptions = null;
            ManagementScope scope = null;
            WindowsSystemInfo systemInfo = new WindowsSystemInfo();

            if (credential != null)
            {
                connectionOptions = new ConnectionOptions();
                connectionOptions.Username = credential.UserName;
                connectionOptions.Password = credential.Password;
                connectionOptions.Authority = "ntdlmdomain:{0}".FormatWith(credential.Domain);
                scope = new ManagementScope(@"\\{0}\root\cimv2".FormatWith(hostName), connectionOptions);
            }
            else
            {
                scope = new ManagementScope(@"\\{0}\root\cimv2".FormatWith(hostName));
            }

            scope.Connect();

            ObjectQuery query = new ObjectQuery("SELECT * FROM Win32_OperatingSystem");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject info in queryCollection)
                {
                    systemInfo.HostName = info["CSName"].ToString();
                    systemInfo.OperatingSystem = info["Caption"].ToString();
                    systemInfo.Revision = int.Parse(info["ServicePackMinorVersion"].ToString(), CultureInfo.CurrentCulture);
                    systemInfo.ServicePack = int.Parse(info["ServicePackMajorVersion"].ToString(), CultureInfo.CurrentCulture);
                }
            }

            query = new ObjectQuery("SELECT * FROM Win32_ComputerSystem");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject info in queryCollection)
                {
                    systemInfo.Processors = int.Parse(info["NumberOfProcessors"].ToString(), CultureInfo.CurrentCulture);

                    long memory = long.Parse(info["TotalPhysicalMemory"].ToString(), CultureInfo.CurrentCulture);
                    memory /= 1000000;
                    systemInfo.Memory = (int)memory;
                }
            }

            query = new ObjectQuery("SELECT * FROM Win32_Processor");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject info in queryCollection)
                {
                    systemInfo.Architecture = info["DataWidth"].ToString() == "32" ? WindowsSystemInfo.ArchitectureX86 : WindowsSystemInfo.ArchitectureX64;

                    try
                    {
                        if (info["NumberOfCores"] != null)
                        {
                            systemInfo.Cores = int.Parse(info["NumberOfCores"].ToString(), CultureInfo.CurrentCulture);
                        }
                        else
                        {
                            systemInfo.Cores = 1;
                        }
                    }
                    catch (ManagementException)
                    {
                        systemInfo.Cores = 1;
                    }

                    break;
                }
            }

            Collection<string> disks = new Collection<string>();
            query = new ObjectQuery("SELECT * FROM Win32_LogicalDisk");
            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
            {
                ManagementObjectCollection queryCollection = searcher.Get();
                foreach (ManagementObject m in queryCollection)
                {
                    string device = m["DeviceId"].ToString();
                    if (m["Size"] != null)
                    {
                        long size = long.Parse(m["Size"].ToString(), CultureInfo.CurrentCulture);
                        size /= 1000000000;
                        disks.Add("{0}{1}GB".FormatWith(device, size));
                    }
                }
            }
            systemInfo.DiskSpace = string.Join(", ", disks);

            // Get the IP Addresses
            systemInfo.IpAddresses = GetIpAddresses(scope);

            return systemInfo;
        }

        /// <summary>
        /// Gets the ip addresses.
        /// </summary>
        /// <param name="scope">The scope.</param>
        /// <returns>Collection{System.String}.</returns>
        private static Collection<string> GetIpAddresses(ManagementScope scope)
        {
            Collection<string> ipAddresses = new Collection<string>();
            try
            {
                var query = new ObjectQuery("SELECT * FROM Win32_NetworkAdapterConfiguration");
                using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(scope, query))
                {
                    ManagementObjectCollection queryCollection = searcher.Get();
                    foreach (ManagementObject m in queryCollection)
                    {
                        if (m["IPAddress"] != null)
                        {
                            if (m["IPAddress"] is Array)
                            {
                                var addresses = (string[])m["IPAddress"];                            
                                foreach (var address in addresses)
                                {
                                    ipAddresses.Add(address);
                                }
                            }
                            else
                            {
                                ipAddresses.Add(m["IPAddress"].ToString());
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to query ip addresses", ex);
            }

            return ipAddresses;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="hostName"></param>
        /// <returns></returns>
        /// <exception cref="Exception"></exception>
        public static Dictionary<string, string> GetFileShares(string hostName)
        {
            Dictionary<string, string> shares = new Dictionary<string, string>();
            try
            {
                var scope = new ManagementScope(@"\\{0}\root\cimv2".FormatWith(hostName));

                var query = new ObjectQuery("Select * From Win32_Share where Type = 0");
                using (var searcher = new ManagementObjectSearcher(scope, query))
                {
                    var queryCollection = searcher.Get();
                    foreach (var m in queryCollection)
                    {
                        var drive = m["Name"].ToString();
                        var path = m["Path"].ToString();
                        if (drive != null)
                        {
                            shares.Add(drive, path);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception("Unable to query ip addresses", ex);
            }

            return shares;
        }
    }
}
