using System.Collections.ObjectModel;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;

namespace HP.ScalableTest.PluginSupport.WindowsServerService
{
    internal class RadiusApplication : IRadiusApplication
    {
        public bool AddClient(string clientName, string address, string sharedSecret)
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.AddClient(clientName, address, sharedSecret);
        }

        public bool DeleteClient(string clientName)
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.DeleteClient(clientName);
        }

        public Collection<RadiusClient> GetAllClients()
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.GetAllClients();
        }

        public bool DeleteAllClients()
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.DeleteAllClients();
        }

        public bool ClearNetworkPolicy()
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.ClearNetworkPolicy();
        }

        public bool AddNetworkPolicy()
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.AddNetworkPolicy();
        }

        public bool SetAuthenticationMode(string networkPolicy, AuthenticationMode authenticationModes, AuthenticationMode priorityMode = AuthenticationMode.None)
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.SetAuthenticationMode(networkPolicy, authenticationModes, priorityMode);
        }
        public bool OpenBrowser()
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.OpenBrowser();
        }

        public bool MapIdCertificate(string userName, string certificatePath, string certificatePassword = "")
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.MapIdCertificate(userName, certificatePath, certificatePassword);
        }

        public bool DeleteIdCertificate(string userName, string certificatePath, string certificatePassword = "")
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.DeleteIdCertificate(userName, certificatePath, certificatePassword);
        }

        public bool AddCACertificate(string certificatePath)
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.AddCACertificate(certificatePath);
        }

        public bool DeleteCACertificate(string certificatePath)
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.DeleteCACertificate(certificatePath);
        }

        public void RestartRadiusServices()
        {
            PluginSupport.Connectivity.RadiusServer.RadiusApplication.RestartRadiusServices();
        }

        public string GetADUserSamAccountName(string userName)
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.GetADUserSamAccountName(userName);
        }

        public bool RenameADUser(string userName, string newName)
        {
            return PluginSupport.Connectivity.RadiusServer.RadiusApplication.RenameADUser(userName, newName);
        }
    }
}
