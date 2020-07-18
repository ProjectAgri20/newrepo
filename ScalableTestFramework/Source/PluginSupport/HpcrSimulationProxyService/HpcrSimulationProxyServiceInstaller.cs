using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace HP.ScalableTest.PluginSupport.HpcrSimulationProxyService
{
    [RunInstaller(true)]
    public class HpcrSimulationProxyServiceInstaller : Installer
    {
        public HpcrSimulationProxyServiceInstaller()
        {
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            Installers.Add(processInstaller);

            ServiceInstaller serviceInstaller = new ServiceInstaller
            {
                ServiceName = "HpcrSimulationProxyService",
                DisplayName = "HPCR Simulation Proxy Service",
                Description = "STF service for communicating with HP Capture and Route server and configured devices.",
                StartType = ServiceStartMode.Automatic,
                DelayedAutoStart = true
            };
            Installers.Add(serviceInstaller);
        }
    }
}
