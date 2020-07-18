using System.ServiceModel;

namespace HP.ScalableTest.PluginSupport.Connectivity.Dhcp
{
    /// <summary>
    /// Contains DHCP Application operations
    /// </summary>
    [ServiceContract]
    public interface IDhcpApplication
    {
        /// <summary>
        /// Change lease time for a scope		
        /// </summary>
        /// <returns>true if lease time is set successful, false otherwise</returns>
        /// <param name="leaseTime">Lease time to be set</param>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        [OperationContract]
        bool SetDhcpLeaseTime(string serverIP, string scope, long leaseTime);

        /// <summary>
        /// Create a Reservation for device
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="macAddress">Mac address of device</param>
        /// <param name="reservationIP">IP Address to reserve</param>
        /// <param name="reservationType"><see cref=" ReservationType"/></param>
        [OperationContract]
        bool CreateReservation(string serverIP, string scope, string reservationIP, string macAddress, ReservationType reservationType);

        /// <summary>
        /// Create a Reservation for device
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="macAddress">Mac address of device</param>		
        /// <param name="reservationIP">IP Address to reserve</param>		
        [OperationContract]
        bool DeleteReservation(string serverIP, string scope, string reservationIP, string macAddress);

        /// <summary>
        /// Set hostname for a scope
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="hostName">Host name to set</param>
        [OperationContract]
        bool SetHostName(string serverIP, string scope, string hostName);

        /// <summary>
        /// Delete hostname of specified scope
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        [OperationContract]
        bool DeleteHostName(string serverIP, string scope);

        /// <summary>
        /// Set domain name for a scope
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="domainName">Domain name to set</param>		
        [OperationContract]
        bool SetDomainName(string serverIP, string scope, string domainName);

        /// <summary>
        /// Set DNS server for a scope
        /// Assuming DHCP server IP and DNS server IP are same.
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>		
        [OperationContract]
        bool SetWPADServer(string serverIP, string scope, string wpadServer);

        /// <summary>
        /// Set WPAD server for a scope
        /// </summary>
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="wpadServer">WPAD Server Address</param>
        [OperationContract]
        bool DeleteWPADServer(string serverIP, string scope);

        /// <summary>
		/// Delete WPAD Server from scope
		/// </summary>
		/// <returns>true if successful, false otherwise</returns>
		/// <param name="scope">Scope IP Address</param>
		/// <param name="serverIP">Server IP Address</param>
		[OperationContract]

        bool SetDnsServer(string serverIP, string scope, params string[] dnsServers);

        /// <summary>
        /// Remove dns server for a scope
        /// Assuming DHCP server IP and DNS server IP are same.
        /// </summary>		
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        [OperationContract]
        bool DeleteDnsServer(string serverIP, string scope);

        /// <summary>
        /// Get DHCP ScopeIP
        /// </summary>
        /// <param name="serverIP">DHCP Server IP</param>
        /// <returns>DHCP Scope IP address</returns>
        /// <remarks>
        /// output of Netsh Command to retrieve scopeip
        /// C:\Users\Administrator>netsh dhcp server 192.168.70.254 show scope
        ///==============================================================================
        /// Scope Address  - Subnet Mask    - State        - Scope Name          -  Commen
        ///==============================================================================
        /// 192.168.70.0   - 255.255.255.0  -Active        -TPS_v4_70            -
        /// 192.168.71.0   - 255.255.255.0  -Active        -TPS_v4_71            -
        /// Total No. of Scopes = 2
        ///Command completed successfully.
        ///</remarks>
        [OperationContract]
        string GetDhcpScopeIP(string serverIP);

        /// <summary>
        /// Get DHCP Server DNS Suffix of the Printer
        /// </summary>
        /// <returns>DNS Suffix</returns>
        /// <param name="serverIP">DHCP Server IP</param>
        /// <param name="scope">scopeIP of the DHCP Server</param>
        /// <remarks>
        /// output of Netsh Command to retrieve dns suffix[119-option ID for DNS Suffix]
        /// By default for foo.com the byte array[102,111,111,46,99,111,109]
        ///C:\Users\Administrator>netsh dhcp server 192.168.70.254 scope 192.168.70.0 show optionvalue
        ///Changed the current scope context to 192.168.70.0 scope.
        ///Options for Scope 192.168.70.0:
        ///        DHCP Standard Options :
        ///        General Option Values:
        ///        OptionId : 15
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = STRING
        ///                Option Element Value = hssl.com
        ///        OptionId : 6
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = IPADDRESS
        ///                Option Element Value = 192.168.70.254
        ///        OptionId : 51
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = DWORD
        ///                Option Element Value = 691200
        ///        OptionId : 119
        ///        Option Value:
        ///                Number of Option Elements = 7
        ///                Option Element Type = BYTE
        ///                Option Element Value = 102
        ///                Option Element Value = 111
        ///                Option Element Value = 111
        ///                Option Element Value = 46
        ///                Option Element Value = 99
        ///                Option Element Value = 111
        ///                Option Element Value = 109
        ///        OptionId : 3
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = IPADDRESS
        ///                Option Element Value = 192.168.70.1
        ///        OptionId : 12
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = STRING
        ///                Option Element Value = Default_HostName
        ///        For user class [Default BOOTP Class]:
        ///        OptionId : 51
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = DWORD
        ///                Option Element Value = 2592000
        ///Command completed successfully.
        ///</remarks>
        [OperationContract]
        string GetDnsSuffix(string serverIP, string scope);

        /// <summary>
        /// Set DNS Suffix for the specified scope
        /// </summary>
        /// <param name="serverIP">IP Address of the DHCP server.</param>
        /// <param name="scope">scopeIP of the DHCP Server.</param>
        /// <param name="dnsSuffices">DNS suffices to be set.</param>
        /// <returns>True if the DNS Suffix is set, else false.</returns>
        [OperationContract]
        bool SetDnsSuffix(string serverIP, string scope, params string[] dnsSuffices);

        /// <summary>
        /// Get Server configured parameters like hostname,domain name, primary DNS server, router ip
        /// specific to scope
        /// </summary>
        /// <param name="serverIP">DHCP Server IP</param>
        /// <param name="scope">scopeIP of the DHCP Server</param>
        /// <param name="optionId">Option Id to get value</param>
        /// <returns>based on option id returns the specific value
        /// ex: Option id: 12[Returns hostname],6[primary DNS IP],3[Router IP address]</returns>
        /// <example>
        /// Usage: GetConfiguredParameterValue(serverIP,scopeIP,optionID)
        /// <code>
        /// GetConfiguredParameterValue("192.168.100.254", "192.168.100.0", 6)
        /// </code>
        /// </example>
        /// <remarks>
        /// output of Netsh Command to retrieve server configured parameters
        ///C:\Users\Administrator>netsh dhcp server 192.168.70.254 scope 192.168.70.0 show optionvalue
        ///Changed the current scope context to 192.168.70.0 scope.
        ///Options for Scope 192.168.70.0:
        ///        DHCP Standard Options :
        ///        General Option Values:
        ///        OptionId : 15
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = STRING
        ///                Option Element Value = hssl.com
        ///       OptionId : 6
        ///        Option Value:
        ///               Number of Option Elements = 1
        ///                Option Element Type = IPADDRESS
        ///                Option Element Value = 192.168.70.254
        ///        OptionId : 51
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = DWORD
        ///                Option Element Value = 691200
        ///        OptionId : 119
        ///        Option Value:
        ///                Number of Option Elements = 10
        ///                Option Element Type = BYTE
        ///                Option Element Value = 9
        ///                Option Element Value = 3
        ///                Option Element Value = 102
        ///                Option Element Value = 111
        ///                Option Element Value = 111
        ///                Option Element Value = 3
        ///                Option Element Value = 99
        ///                Option Element Value = 111
        ///                Option Element Value = 109
        ///                Option Element Value = 0
        ///        OptionId : 3
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = IPADDRESS
        ///                Option Element Value = 192.168.70.1
        ///        OptionId : 12
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///                Option Element Type = STRING
        ///                Option Element Value = Default_HostName
        ///        For user class [Default BOOTP Class]:
        ///        OptionId : 51
        ///        Option Value:
        ///                Number of Option Elements = 1
        ///               Option Element Type = DWORD
        ///                Option Element Value = 2592000
        ///Command completed successfully.
        /// Note : Returns values separated by '|' if multiple values are configured on the server for the given options.
        ///        Eg : If primary and secondary DNS Servers are present in the Server, then the values will be returned as "192.168.*.*|192.168.*.*"
        ///</remarks>
        [OperationContract]
        string GetConfiguredParameterValue(string serverIP, string scope, int optionId);

        /// <summary>
        /// Gets the Domain Name
        /// </summary>
        /// <param name="serverIP">Server IP</param>
        /// <param name="scope">Scope</param>
        /// <returns>Returns Domain Name</returns>
        [OperationContract]
        string GetDomainName(string serverIP, string scope);

        /// <summary>
        /// Gets the Host Name
        /// </summary>
        /// <param name="serverIP">Server IP</param>
        /// <param name="scope">Scope</param>
        /// <returns>Returns Domain Name</returns>
        [OperationContract]
        string GetHostName(string serverIP, string scope);

        /// <summary>
        /// Starts DHCP Server
        /// </summary>
        /// <returns>true if DHCP server is started, false otherwise</returns>
        [OperationContract]
        bool StartDhcpServer();

        /// <summary>
        /// Stop DHCP Server
        /// </summary>
        /// <returns>true if DHCP server is stopped, false otherwise</returns>
        [OperationContract]
        bool StopDhcpServer();

        /// <summary>
        /// Get v6 Scope IP if available
        /// </summary>
        /// <returns>All v6 Scopes available if found, string.Empty if not</returns>
        /// <param name="serverIP">Server IP Address</param>
        /// <remarks>
        /// Output Format:
        /// ========================================================================================================
        /// Scope Address                        - State         - Scope Name          -  Comment       - Preference
        ///========================================================================================================
        /// 2000:200:200:200::                    -Active        -200v6                -              -64
        /// 2000:200:200:201::                    -Active        -200v6                -              -64
        /// 2000:200:200:202::                    -Active        -200v6                -              -64
        /// Total No. of Scopes = 3 
        ///Command completed successfully.
        ///</remarks>
        [OperationContract]
        string GetIPv6Scope(string serverIP);

        /// <summary>
        /// Get IPv6 address of the Server if available else returns emptry string.
        /// </summary>
        /// <returns>IPv6 Address of the server</returns>
        [OperationContract]
        string GetIPv6Address();

        /// <summary>
        /// Get Preferred Life Time for v6 Scope
        /// </summary>		
        /// <returns>time in seconds</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <remarks>
        /// Output format:
        /// Changed the current scope context to 2000:200:200:20 scope.
        /// Preferred Lifetime setting : 691200
        /// Command completed successfully.
        /// </remarks>
        [OperationContract]
        int GetPreferredLifetime(string serverIP, string scope);

        /// <summary>
        /// Set Preferred Life Time for v6 Scope
        /// </summary>
        /// <returns>true if command executed successful, false otherwise</returns>
        /// <param name="leaseTime">Lease time to set in seconds</param>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        [OperationContract]
        bool SetPreferredLifetime(string serverIP, string scope, int leaseTime);

        /// <summary>
        /// Get Valid Life Time for v6 Scope
        /// </summary>		
        /// <returns>time in seconds</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <remarks>
        /// Output format:
        /// Changed the current scope context to 2000:200:200:20 scope.
        /// Valid Lifetime setting : 777600
        /// Command completed successfully.
        /// </remarks>
        [OperationContract]
        int GetValidLifetime(string serverIP, string scope);

        /// <summary>
        /// Set Valid Life Time for v6 Scope
        /// </summary>		
        /// <returns>true if command executed successful, false otherwise</returns>
        /// <param name="leaseTime">Lease time to set in seconds</param>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        [OperationContract]
        bool SetValidLifetime(string serverIP, string scope, int leaseTime);

        /// <summary>
        /// Gets the server name.
        /// </summary>
        /// <returns>Name of the Server</returns>
        [OperationContract]
        string GetServerName();

        /// <summary>
        /// Returns primary DNS Server IP Address for the specified scope.
        /// </summary>
        /// <param name="serverIP">The DHCP Server IP Address.</param>
        /// <param name="scope">The scope IP Address.</param>
        /// <returns>The primary DNS Server IP Address.</returns>
        [OperationContract]
        string GetPrimaryDnsServer(string serverIP, string scope);

        /// <summary>
        /// Returns secondary DNS Server IP Address for the specified scope.
        /// </summary>
        /// <param name="serverIP">The DHCP Server IP Address.</param>
        /// <param name="scope">The scope IP Address.</param>
        /// <returns>The secondary DNS Server IP Address.</returns>
        [OperationContract]
        string GetSecondaryDnsServer(string serverIP, string scope);

        /// <summary>
        /// Set BootP host name
        /// </summary>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="hostName">Host name to be set</param>
        /// <returns>true if set successfully, false otherwise</returns>
        [OperationContract]
        bool SetBootPHostName(string serverIP, string scope, string hostName);

        /// <summary>
        /// Gets the Lease time
        /// </summary>
        /// <param name="serverIP">Server IP</param>
        /// <param name="scope">Scope</param>
        /// <returns>Returns Lease time in seconds</returns>
        [OperationContract]
        long GetLeasetime(string serverIP, string scope);

        /// <summary>
        /// Check whether DHCP Service is running on DHCP server
        /// </summary>
        /// <returns>true if DHCP Service is running, false otherwise</returns>
        [OperationContract]
        bool IsDhcpServiceRunning();

        /// <summary>
        /// Gets the Router Address
        /// </summary>
        /// <param name="serverIP">Server IP</param>
        /// <param name="scope">Scope</param>
        /// <returns>Returns Router Address</returns>
        [OperationContract]
        string GetRouterAddress(string serverIP, string scope);

        /// <summary>
        /// Returns primary WINS Server IP Address for the specified scope.
        /// </summary>
        /// <param name="serverIP">The DHCP Server IP Address.</param>
        /// <param name="scope">The scope IP Address.</param>
        /// <returns>The primary WINS Server IP Address.</returns>
        [OperationContract]
        string GetPrimaryWinsServer(string serverIP, string scope);

        /// <summary>
        /// Returns secondary WINS Server IP Address for the specified scope.
        /// Note: If secondary WINS Server IP Address is not available, primary will be returned as default
        /// </summary>
        /// <param name="serverIP">The DHCP Server IP Address.</param>
        /// <param name="scope">The scope IP Address.</param>
        /// <returns>The secondary WINS Server IP Address.</returns>
        [OperationContract]
        string GetSecondaryWinsServer(string serverIP, string scope);

        /// <summary>
        /// Set Primary Wins Server IP Address
        /// </summary>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="winsAddress">Host name to be set</param>
        /// <returns>true if set successfully, false otherwise</returns>
        [OperationContract]
        bool SetWinsServer(string serverIP, string scope, params string[] winsAddress);

        /// <summary>
        /// Delete Domain name option on specified scope 
        /// </summary>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="scope">Scope IP Address</param>
        [OperationContract]
        bool DeleteDomainName(string serverIP, string scope);

        /// <summary>
        /// Get Primary DNS v6 Address
        /// </summary>
        /// <param name="serverIP">Server Address</param>
        /// <param name="scope">v6 Scope Address</param>
        /// <returns>Primary Dns v6 address</returns>
        [OperationContract]
        string GetPrimaryDnsv6Server(string serverIP, string scope);

        /// <summary>
        /// Get Secondary DNS v6 Address
        /// </summary>
        /// <param name="serverIP">Server Address</param>
        /// <param name="scope">v6 Scope Address</param>
        /// <returns>Secondary Dns v6 address</returns>
        [OperationContract]
        string GetSecondaryDnsv6Server(string serverIP, string scope);

        /// <summary>
        /// Get Domain Search List
        /// </summary>
        /// <param name="serverIP">Server Address</param>
        /// <param name="scope">v6 Scope Address</param>
        /// <returns>Domain Search List</returns>
        [OperationContract]
        string GetDomainSearchList(string serverIP, string scope);

        /// <summary>
        /// Delete Wins Server option on specified scope 
        /// </summary>
        /// <param name="serverIP">Server IP Address</param>
        /// <param name="scope">Scope IP Address</param>
        [OperationContract]
        bool DeleteWinsServer(string serverIP, string scope);

        /// <summary>
        /// Create a new scope
        /// </summary>
        /// <param name="serverIP">Server Address</param>
        /// <param name="scope">Scope Address</param>
        /// <returns>true if scope deleted, false otherwise</returns>
        [OperationContract]
        bool DeleteScope(string serverIP, string scope);

        /// <summary>
        /// Create a new scope
        /// </summary>
        /// <param name="serverIP">Server Address</param>
        /// <param name="scope">Scope Address</param>
        /// <param name="scopeName">Scope name</param>
        /// <param name="startIpRange">Start range of IP Address for the scope</param>
        /// <param name="endIpRange">End range for the scope</param>
        /// <returns>true if the scope is created, else false</returns>
        [OperationContract]
        bool CreateScope(string serverIP, string scope, string scopeName, string startIpRange, string endIpRange);

        /// <summary>
        /// Change the server type to DHCP, BOOTP or BOTH.
        /// Note : Deletes all the reservations and change the server type.
        ///	If we want to retain the reservation mention the exceptionAddress and exceptionMacAddress parameters.
        /// </summary>
        /// <param name="serverIP">The IP Address of the DHCP Server</param>
        /// <param name="scope">The scope IP Address</param>
        /// <param name="serverType"><see cref="ReservationType"/></param>
        /// <param name="exceptionAddress"></param>
        /// <param name="exceptionMacAddress"></param>
        /// <returns>true if the server type is successfully changed, else false</returns>
        [OperationContract]
        bool ChangeServerType(string serverIP, string scope, ReservationType serverType, string exceptionAddress = null, string exceptionMacAddress = null);

        /// <summary>
        /// Delete BootP Hostname
        /// </summary>
        /// <param name="serverIP">The IP Address of the DHCP Server</param>
        /// <param name="scope">The scope IP Address</param>
        /// <returns>true if deleted successfully, false otherwise</returns>
        [OperationContract]
        bool DeleteBootPHostname(string serverIP, string scope);

        /// <summary>
        /// Set Domain Search List
        /// </summary>
        /// <param name="serverIP">The IP Address of the DHCP Server</param>
        /// <param name="scope">The v6 scope IP Address</param>		
        /// <param name="domainName">Domain name to be set</param>
        /// <returns>true if set, false otherwise</returns>
        [OperationContract]
        bool SetDomainSearchList(string serverIP, string scope, string domainName);

        /// <summary>
        /// Delete Domain Search List option in v6 scope
        /// </summary>
        /// <param name="serverIP">The IP Address of the DHCP Server</param>
        /// <param name="scope">The v6 scope IP Address</param>		
        /// <returns>true if deleted successfully, false otherwise</returns>
        [OperationContract]
        bool DeleteDomainSearchList(string serverIP, string scope);

        /// <summary>
        /// Set Primary DnsServer ip address
        /// </summary>
        /// <param name="serverIP">The IP Address of the DHCP Server</param>
        /// <param name="scope">The v6 scope IP Address</param>		
        /// <param name="dnsServer">DNS Server IP to be set</param>
        /// <returns>true if set, false otherwise</returns>
        [OperationContract]
        bool SetDnsv6ServerIP(string serverIP, string scope, string dnsServer);

        /// <summary>
        /// Remove dnsv6 server for a scope
        /// Assuming DHCP server IP and DNS server IP are same.
        /// </summary>		
        /// <returns>true if successful, false otherwise</returns>
        /// <param name="scope">Scope IP Address</param>
        /// <param name="serverIP">Server IP Address</param>
        [OperationContract]
        bool DeleteDnsv6Server(string serverIP, string scope);

    }
}
