using System.ServiceModel;

namespace HP.ScalableTest.PluginSupport.Connectivity.Wins
{
    /// <summary>
    /// Contains Wins Application operations
    /// </summary>
    [ServiceContract]
    public interface IWinsApplication
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
        [OperationContract]
        bool ValidateWinServerLog(string serverIP, string hostName, string recordType, string printerIP);

        /// <summary>
        /// Starts the WINS service.
        /// </summary>
        /// <returns>True if the WINS service is started, else false.</returns>
        [OperationContract]
        bool StartWinsService();

        /// <summary>
        /// Stops the WINS service
        /// </summary>
        /// <returns>True if the WINS service is stopped, else false.</returns>
        [OperationContract]
        bool StopWinsService();

        /// <summary>
        /// Check whether WINS Service is running on DHCP server
        /// </summary>
        /// <returns>true if WINS Service is running, false otherwise</returns>
        [OperationContract]
        bool IsWinsServiceRunning();

        /// <summary>
        /// Delete all the WINS records
        /// </summary>
        /// <param name="serverIP">IP address of the WINS server.</param>
        /// <returns>True if all the records are deleted, else false.</returns>
        [OperationContract]
        bool DeleteAllRecords(string serverIP);
    }
}