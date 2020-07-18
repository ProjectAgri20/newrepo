using System.ServiceModel;

namespace HP.ScalableTest.PluginSupport.Connectivity.DnsApp
{
    /// <summary>
    /// Represents the DNS record Type
    /// </summary>
    public enum DnsRecordType
    {
        /// <summary>
        /// The A record
        /// </summary>
        A = 1,
        /// <summary>
        /// The PTR record
        /// </summary>
        PTR = 2,
        /// <summary>
        /// The AAAA record
        /// </summary>
        AAAA = 4
    }

    /// <summary>
    /// Contains DNS operations like Adding Domain, Adding / Deleting Record
    /// </summary>
    [ServiceContract]
    public interface IDnsApplication
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
        [OperationContract]
        bool AddDomain(string domainName);

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
        [OperationContract]
        bool AddRecord(string domainName, string hostName, string recordType, string printerIP);

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
        [OperationContract]
        bool DeleteRecord(string domainName, string hostName, string recordType, string printerIP);

        /// <summary>
        /// Deletes all the records for the given zone and record type from the foward lookup zone
        /// </summary>
        /// <param name="zoneName">Name of the Zone</param>
        /// <param name="recordType">Record Type A, AAAA</param>
        /// <param name="fqdn">Domain Name of the Client</param>
        /// <returns>Returns true if all the recrods are deletes else returns false</returns>
        [OperationContract]
        bool DeleteRecords(string zoneName, string recordType, string fqdn);

        /// <summary>
        /// Deletes all the A and AAAA records for the given zone from the foward lookup zone
        /// </summary>
        /// <param name="zoneName">Name of the Zone</param>
        /// <param name="fqdn">Domain Name of the Client</param>
        /// <returns>Returns true if all the recrods are deletes else returns false</returns>
        [OperationContract]
        bool DeleteAllRecords(string zoneName, string fqdn);

        /// <summary>
        /// Starts the DNS service.
        /// </summary>
        /// <returns>True if the DNS service is started, else false.</returns>
        [OperationContract]
        bool StartDnsService();

        /// <summary>
        /// Stops the DNS service
        /// </summary>
        /// <returns>True if the DNS service is stopped, else false.</returns>
        [OperationContract]
        bool StopDnsService();

        /// <summary>
        /// Check whether DNS Service is running on DHCP server
        /// </summary>
        /// <returns>true if DNS Service is running, false otherwise</returns>
        [OperationContract]
        bool IsDnsServiceRunning();

        /// <summary>
        /// Validates a record under the specified domain based on the specified <see cref="DnsRecordType"/>.
        /// </summary>
        /// <param name="serverIP">IP Address of the DNS server.</param>
        /// <param name="domainName">The domain name where the record is added.</param>
        /// <param name="recordType"><see cref="DnsRecordType"/></param>
        /// <param name="hostName">Host name of the device.</param>
        /// <param name="ipAddress">IP address of the device.</param>
        /// <returns>True if the record is present, else false.</returns>
        [OperationContract]
        bool ValidateRecord(string serverIP, string domainName, DnsRecordType recordType, string hostName, string ipAddress);
    }
}
