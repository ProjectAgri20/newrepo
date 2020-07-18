using System;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Runtime.InteropServices;
using System.ServiceProcess;
using System.Threading;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.SystemConfiguration
{
    /// <summary>
    /// Contains various methods related to the system configuration.
    /// </summary>
    public static class SystemConfiguration
    {
        [DllImport("kernel32.dll", EntryPoint = "GetSystemTime")]
        private extern static void GetSystemTime(ref SYSTEMTIME lpSystemTime);

        [DllImport("kernel32.dll", EntryPoint = "SetSystemTime")]
        private extern static uint SetSystemTime(ref SYSTEMTIME lpSystemTime);

        [DllImport("kernel32.dll", EntryPoint = "GetLocalTime")]
        private extern static uint GetLocalTime(ref SYSTEMTIME lpSystemTime);

        private struct SYSTEMTIME
        {
            public short Year;
            public short Month;
            public short DayOfWeek;
            public short Day;
            public short Hour;
            public short Minute;
            public short Second;
            public short Milliseconds;
        }

        /// <summary>
        /// Gets the current date time value.
        /// </summary>
        /// <returns>The current system time.</returns>
        public static DateTime GetSystemTime()
        {
            SYSTEMTIME systemTime = new SYSTEMTIME();
            GetSystemTime(ref systemTime);
            return new DateTime(systemTime.Year, systemTime.Month, systemTime.Day, systemTime.Hour, systemTime.Minute, systemTime.Second, systemTime.Milliseconds).ToLocalTime();
        }

        /// <summary>
        /// Sets the system time to the specified date time value.
        /// </summary>
        /// <param name="dateTime">Time to be set.</param>
        /// <param name="resetTime">The amount of time to reset the changed time.</param>
        /// <returns>true if the date time is set, else false.</returns>
        public static bool SetSystemTime(DateTime dateTime, System.TimeSpan? resetTime = null)
        {
            dateTime = dateTime.ToUniversalTime();

            DateTime currentTime = DateTime.UtcNow;

            SYSTEMTIME systemTime = new SYSTEMTIME
            {
                Year = (short)dateTime.Year,
                Month = (short)dateTime.Month,
                DayOfWeek = (short)dateTime.DayOfWeek,
                Day = (short)dateTime.Day,
                Hour = (short)dateTime.Hour,
                Minute = (short)dateTime.Minute,
                Second = (short)dateTime.Second,
                Milliseconds = (short)dateTime.Millisecond
            };

            uint result = SetSystemTime(ref systemTime);

            int errorNumber = Marshal.GetLastWin32Error();

            if (result == 0)
            {
                Logger.LogInfo("Failed to set system time to {0}".FormatWith(dateTime.ToLocalTime()));
                return false;
            }
            else
            {
                Logger.LogInfo("Successfully set the system time to: {0}".FormatWith(dateTime.ToLocalTime()));
            }

            if (null != resetTime)
            {
                Thread resetThread = new Thread(() => ResetTime(currentTime, resetTime));
                resetThread.Start();
            }

            return true;
        }

        /// <summary>
        /// Resets the time after the specified time interval.
        /// </summary>
        /// <param name="currentTime">The date time to be set.</param>
        /// <param name="resetTime">The reset time interval.</param>
        private static void ResetTime(DateTime currentTime, System.TimeSpan? resetTime)
        {
            if (null == resetTime)
            {
                return;
            }

            Thread.Sleep(resetTime.Value);

            currentTime = currentTime.Add(resetTime.Value);

            SYSTEMTIME systemTime = new SYSTEMTIME
            {
                Year = (short)currentTime.Year,
                Month = (short)currentTime.Month,
                DayOfWeek = (short)currentTime.DayOfWeek,
                Day = (short)currentTime.Day,
                Hour = (short)currentTime.Hour,
                Minute = (short)currentTime.Minute,
                Second = (short)currentTime.Second,
                Milliseconds = (short)currentTime.Millisecond
            };

            SetSystemTime(ref systemTime);
        }

        /// <summary>
        /// Starts the service on the specified server. If server is not specified, the local machine is considered.
        /// </summary>
        /// <param name="serviceName">The name of the service.</param>
        /// <param name="serverName">The server where the service is to be started.</param>
        /// <returns>True if the service is started, else false.</returns>
        public static bool StartService(string serviceName, string serverName = "localhost")
        {
            try
            {
                ServiceController sc = new ServiceController(serviceName, serverName);
                sc.Start();
                sc.WaitForStatus(ServiceControllerStatus.Running, new TimeSpan(0, 0, 30));
                sc.Refresh();

                if (sc.Status == ServiceControllerStatus.Running)
                {
                    Logger.LogInfo("Successfully started '{0}' service on : {1}".FormatWith(serviceName, serverName));
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to start '{0}' service on : {1}".FormatWith(serviceName, serverName));
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (GetServiceStatus(serviceName, serverName) == ServiceControllerStatus.Running)
                {
                    Logger.LogInfo("The '{0}' service is already running on: {1}.".FormatWith(serviceName, serverName));
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to start '{0}' service on : {1}.".FormatWith(serviceName, serverName));
                    Logger.LogDebug("Exception Details: {0}".FormatWith(ex.JoinAllErrorMessages()));
                    return false;
                }
            }
        }

        /// <summary>
        /// Stops the service on the specified server. If server is not specified, the local machine is considered.
        /// </summary>
        /// <param name="serviceName">The name of the service.</param>
        /// <param name="serverName">The server where the service is to be stopped.</param>
        /// <returns>True if the service is started, else false.</returns>
        public static bool StopService(string serviceName, string serverName = "localhost")
        {
            try
            {
                ServiceController sc = new ServiceController(serviceName, serverName);
                sc.Stop();
                sc.WaitForStatus(ServiceControllerStatus.Stopped, new TimeSpan(0, 0, 30));
                sc.Refresh();

                if (sc.Status == ServiceControllerStatus.Stopped)
                {
                    Logger.LogInfo("Successfully stopped '{0}' service on : {1}".FormatWith(serviceName, serverName));
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to stop '{0}' service on : {1}".FormatWith(serviceName, serverName));
                    return false;
                }
            }
            catch (Exception ex)
            {
                if (GetServiceStatus(serviceName, serverName) == ServiceControllerStatus.Stopped)
                {
                    Logger.LogInfo("The '{0}' service is already stopped on: {1}.".FormatWith(serviceName, serverName));
                    return true;
                }
                else
                {
                    Logger.LogInfo("Failed to stop '{0}' service on : {1}.".FormatWith(serviceName, serverName));
                    Logger.LogDebug("Exception Details: {0}".FormatWith(ex.JoinAllErrorMessages()));
                    return false;
                }
            }
        }

        /// <summary>
        /// Gets the service status on the specified server. If server is not specified, the local machine is considered.
        /// </summary>
        /// <param name="serviceName">The name of the service.</param>
        /// <param name="serverName">The server where the service status to be checked.</param>
        /// <returns>True if the service is started, else false.</returns>
        public static ServiceControllerStatus GetServiceStatus(string serviceName, string serverName = "localhost")
        {
            ServiceController sc = new ServiceController(serviceName, serverName);
            return sc.Status;
        }

        /// <summary>
        /// Checks whether the specified service is running on the server specified.
        /// </summary>
        /// <param name="serviceName">The name of the service.</param>
        /// <param name="serverName">The server where the service is to be started.</param>
        /// <returns>True if the service is running, else false.</returns>
        public static bool IsServiceRunning(string serviceName, string serverName = "localhost")
        {
            try
            {
                if (GetServiceStatus(serviceName, serverName) == ServiceControllerStatus.Running)
                {
                    Logger.LogInfo("The '{0}' service is running on : {1}".FormatWith(serviceName, serverName));
                    return true;
                }
                else
                {
                    Logger.LogInfo("The '{0}' service is not running on : {1}".FormatWith(serviceName, serverName));
                    return false;
                }
            }
            catch (Exception ex)
            {
                Logger.LogInfo("Failed to get '{0}' service status from {1}.".FormatWith(serviceName, serverName));
                Logger.LogDebug("Exception Details: {0}".FormatWith(ex.JoinAllErrorMessages()));
                return false;
            }
        }

        /// <summary>
        /// Set static IP address for the client machine for the specified network.
        /// </summary>
        /// <param name="networkName">The name of the network to be modified.</param>
        /// <param name="ipAddress">The new IP address to be set.</param>
        /// <param name="subnetMask">The subnet mask to be set.</param>
        /// <param name="gateWay">The gateway to be set.</param>
        /// <returns>True if the operation is successful, else false.</returns>
        public static bool SetStaticIPAddress(string networkName, IPAddress ipAddress, IPAddress subnetMask, IPAddress gateWay = null)
        {
            string command = "interface ip set address name=\"{0}\" static {1} {2} {3}".FormatWith(networkName, ipAddress, subnetMask, gateWay != null ? gateWay.ToString() : string.Empty);

            if (string.IsNullOrEmpty(ProcessUtil.Execute("netsh.exe", command).StandardOutput))
            {
                Logger.LogInfo("Successfully set static IP address: {0} with subnet mask: {1} and Gateway: {2} on the network: {3}.".FormatWith(ipAddress, subnetMask, gateWay, networkName));
                return true;
            }
            else
            {
                Logger.LogInfo("Failed to set static IP address: {0} with subnet mask: {1} and Gateway: {2} on the network: {3}.".FormatWith(ipAddress, subnetMask, gateWay, networkName));
                return false;
            }
        }

        /// <summary>
        /// Set DHCP address for the client machine for the specified network.
        /// </summary>
        /// <param name="networkName">The name of the network to be modified.</param>
        /// <returns>True if the operation is successful, else false.</returns>
        public static bool SetDhcpIPAddress(string networkName)
        {
            string command = "interface ip set address name=\"{0}\" source=dhcp".FormatWith(networkName);
            string result = string.Empty;
            result = ProcessUtil.Execute("netsh.exe", command).StandardOutput;

            if (string.IsNullOrEmpty(result) | result.EqualsIgnoreCase("DHCP is already enabled on this interface."))
            {
                Logger.LogInfo("Successfully set DHCP IP address on the network: {0}.".FormatWith(networkName));
                return true;
            }
            else
            {
                Logger.LogInfo("Failed to set DHCP IP address on the network: {0}.".FormatWith(networkName));
                return false;
            }
        }

        /// <summary>
        /// Gets the fully qualified name of the machine
        /// </summary>
        /// <returns>The fully qualified name of the machine.</returns>
        public static string GetFullyQualifiedname()
        {
            string domainName = ".{0}".FormatWith(IPGlobalProperties.GetIPGlobalProperties().DomainName);
            string hostName = Dns.GetHostName();
            string fqdn = string.Empty;

            if (!hostName.EndsWith(domainName))
            {
                fqdn = "{0}{1}".FormatWith(hostName, domainName);
            }
            else
            {
                fqdn = hostName;
            }

            Logger.LogInfo("Fully Qualified Name of the system: {0}".FormatWith(fqdn));
            return fqdn;
        }

        /// <summary>
        /// Gets hostname of the machine
        /// </summary>
        /// <returns>The hostname of the machine.</returns>
        public static string GetHostName()
        {
            string hostName = Dns.GetHostName();
            Logger.LogInfo("Hostname of the system: {0}".FormatWith(hostName));
            return hostName;
        }

        public static bool IsServiceExist(string serviceName, string serverName = "localhost")
        {
            ServiceController ctl = ServiceController.GetServices().FirstOrDefault(s => s.ServiceName == serverName);
            if (ctl == null)
            {
                Logger.LogDebug("Service {0} does not exist".FormatWith(serviceName));
                return false;
            }
            else
            {
                Logger.LogDebug("Service {0} exists".FormatWith(serviceName));
                return true;
            }



        }
    }
}
