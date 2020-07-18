using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace HP.ScalableTest.PluginSupport.WindowsServerService
{
    [RunInstaller(true)]
    public class WindowsServerServiceInstaller : Installer
    {
        public WindowsServerServiceInstaller()
        {
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            Installers.Add(processInstaller);

            ServiceInstaller serviceInstaller = new ServiceInstaller
            {
                ServiceName = "WindowsServerService",
                DisplayName = "Windows Server Service",
                Description = "Performs Windows DHCP, DNS and Wins server operations.",
                StartType = ServiceStartMode.Automatic,
                DelayedAutoStart = true
            };
            Installers.Add(serviceInstaller);
        }
    }
}
