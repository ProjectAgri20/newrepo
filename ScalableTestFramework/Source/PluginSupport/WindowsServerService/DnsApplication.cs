using HP.ScalableTest.Framework;
using HP.ScalableTest.PluginSupport.Connectivity.DnsApp;

namespace HP.ScalableTest.PluginSupport.WindowsServerService
{
    internal class DnsApplication : IDnsApplication
    {
        public DnsApplication()
        {
            Logger.LogInfo("DNS application started...");
        }

        public bool AddDomain(string domainName)
        {
            return PluginSupport.Connectivity.DnsApp.DnsApplication.AddDomain(domainName);
        }

        public bool AddRecord(string domainName, string hostName, string recordType, string printerIP)
        {
            return PluginSupport.Connectivity.DnsApp.DnsApplication.AddRecord(domainName, hostName, recordType, printerIP);
        }

        public bool DeleteRecord(string domainName, string hostName, string recordType, string printerIP)
        {
            return PluginSupport.Connectivity.DnsApp.DnsApplication.DeleteRecord(domainName, hostName, recordType, printerIP);
        }

        public bool DeleteRecords(string zoneName, string recordType, string fqdn)
        {
            return PluginSupport.Connectivity.DnsApp.DnsApplication.DeleteRecords(zoneName, recordType, fqdn);
        }

        public bool DeleteAllRecords(string zoneName, string fqdn)
        {
            return PluginSupport.Connectivity.DnsApp.DnsApplication.DeleteAllRecords(zoneName, fqdn);
        }

        public bool StopDnsService()
        {
            return PluginSupport.Connectivity.DnsApp.DnsApplication.StopDnsService();
        }

        public bool StartDnsService()
        {
            return PluginSupport.Connectivity.DnsApp.DnsApplication.StartDnsService();
        }

        public bool IsDnsServiceRunning()
        {
            return PluginSupport.Connectivity.DnsApp.DnsApplication.IsDnsServiceRunning();
        }

        public bool ValidateRecord(string serverIP, string domainName, DnsRecordType recordType, string hostName, string ipAddress)
        {
            return PluginSupport.Connectivity.DnsApp.DnsApplication.ValidateRecord(serverIP, domainName, recordType, hostName, ipAddress);
        }
    }
}
