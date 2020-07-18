using System.ServiceProcess;

namespace HP.ScalableTest.PluginSupport.HpcrSimulationProxyService
{
    internal class Program
    {
        static void Main(string[] args)
        {
            ServiceBase.Run(new HpcrSimulationProxyService());
        }
    }
}
