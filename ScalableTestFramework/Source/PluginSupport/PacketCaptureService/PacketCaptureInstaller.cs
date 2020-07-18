using System.ComponentModel;
using System.Configuration.Install;
using System.ServiceProcess;

namespace HP.ScalableTest.PluginSupport.PacketCaptureService
{
    [RunInstaller(true)]
    public class PacketCaptureInstaller : Installer
    {
        public PacketCaptureInstaller()
        {
            ServiceProcessInstaller processInstaller = new ServiceProcessInstaller
            {
                Account = ServiceAccount.LocalSystem
            };
            Installers.Add(processInstaller);

            ServiceInstaller serviceInstaller = new ServiceInstaller
            {
                ServiceName = "PacketCaptureService",
                DisplayName = "Packet Capture Service",
                Description = "Captures network packets.",
                StartType = ServiceStartMode.Automatic,
                DelayedAutoStart = true
            };
            Installers.Add(serviceInstaller);
        }
    }
}