using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace HP.ScalableTest.PluginSupport.ConnectivityUtilityService
{
    [RunInstaller(true)]
    public class ConnectivityUtilityServiceInstaller : Installer
    {
        public ConnectivityUtilityServiceInstaller()
        {
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            Installers.Add(processInstaller);

            ServiceInstaller serviceInstaller = new ServiceInstaller
            {
                ServiceName = "ConnectivityUtilityService",
                DisplayName = "Connectivity Utility Service",
                Description = "Performs Connectivity specific operations.",
                StartType = ServiceStartMode.Automatic,
                DelayedAutoStart = true
            };
            Installers.Add(serviceInstaller);
        }
    }
}
