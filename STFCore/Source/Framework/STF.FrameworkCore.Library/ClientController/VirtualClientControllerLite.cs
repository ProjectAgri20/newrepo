
namespace HP.ScalableTest.Framework.Automation
{
    public class VirtualClientControllerLite : VirtualClientController
    {
        public VirtualClientControllerLite()
            : base("localhost", 0)
        {
        }

        protected override void InstallPrintingCertficates()
        {
            TraceFactory.Logger.Debug("No certs installed for STB");
        }

        //protected override void InstallClientSoftware()
        //{
        //    TraceFactory.Logger.Debug("No client SW installed for STB");
        //}
    }
}
