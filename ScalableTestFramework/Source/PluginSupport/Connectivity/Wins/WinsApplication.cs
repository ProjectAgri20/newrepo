using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.PluginSupport.Connectivity.Wins
{
    /// <summary>
    /// Contains Wins Application operations.
    /// </summary>
    public static class WinsApplication
    {
        /// <summary>
        /// Validate the device Registration through Winserver Log
        /// </summary>
        /// <returns>Returns true if the command result contains printer IP, Host Name, Record Type, else return false</returns>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="hostName">Host Name</param>
        /// <param name="recordType">Record Type</param>
        /// <param name="printerIP">Printer IP Address</param>
        /// <example>
        /// Usage: ValidateWinserverLog(serverIP, printerIP, hostName, recordType)
        /// <code>
        /// AlterDHCPLeaseTime("192.168.100.254" "DefaultHostname", "00", "192.168.100.16")
        /// </code>
        /// </example>
        public static bool ValidateWinServerLog(string serverIP, string hostName, string recordType, string printerIP)
        {
            //TODO: What is record type need to provide as an enum which applies to other application also.
            Logger.LogInfo("Validating Windows Server Log");

            string result = NetworkUtil.ExecuteCommand("netsh wins server {0} show name Name= {1} EndChar= \n".FormatWith(serverIP, hostName));

            Logger.LogInfo("Looking for the Printer IP : {0}, Host Name : {1} and Record Type : {2} in the command output".FormatWith(printerIP, hostName, recordType));

            Logger.LogInfo("Command Result: {0}".FormatWith(result));

            //the result should have printerIP, hostName and recordType"copy here the command line result"
            return result.Contains(printerIP, StringComparison.CurrentCultureIgnoreCase) && result.Contains(hostName, StringComparison.CurrentCultureIgnoreCase) && result.Contains(recordType, StringComparison.CurrentCultureIgnoreCase);
        }

        /// <summary>
        /// Starts the WINS service.
        /// </summary>
        /// <returns>True if the WINS service is started, else false.</returns>
        public static bool StartWinsService()
        {
            if (IsWinsServiceRunning())
            {
                return true;
            }
            else
            {
                string result = ProcessUtil.Execute("cmd.exe", "/C net start WINS").StandardOutput;

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
        /// Stops the WINS service
        /// </summary>
        /// <returns>True if the WINS service is stopped, else false.</returns>
        public static bool StopWinsService()
        {
            if (!IsWinsServiceRunning())
            {
                return true;
            }
            else
            {
                string result = ProcessUtil.Execute("cmd.exe", "/C net stop WINS").StandardOutput;

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
        /// Check whether WINS Service is running on DHCP server
        /// </summary>
        /// <returns>true if WINS Service is running, false otherwise</returns>
        public static bool IsWinsServiceRunning()
        {
            return ProcessUtil.Execute("cmd.exe", "/C sc query WINS").StandardOutput.Contains("running", StringComparison.CurrentCultureIgnoreCase) ? true : false;
        }

        /// <summary>
        /// Delete all the WINS records
        /// </summary>
        /// <param name="serverIP">IP address of the WINS server.</param>
        /// <returns>True if all the records are deleted, else false.</returns>
        public static bool DeleteAllRecords(string serverIP)
        {
            Logger.LogInfo("Deleting all the records from WINS server.");

            string result = NetworkUtil.ExecuteCommand("netsh wins server {0} delete records minver= {0,0} maxver={0,0} op=1\n".FormatWith(serverIP));

            if (result.Contains("command executed successfully", StringComparison.CurrentCultureIgnoreCase))
            {
                Logger.LogInfo("Successfully deleted all the records from WINS server.");
                return true;
            }
            else
            {
                Logger.LogInfo("Failed to delete all the records from WINS server.");
                return false;
            }
        }
    }
}
