using System.ServiceModel;
using System.ServiceModel.Channels;
using System.ServiceProcess;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.PluginSupport.Hpcr;

namespace HP.ScalableTest.PluginSupport.HpcrSimulationProxyService
{
    internal class HpcrSimulationProxyService : ServiceBase
    {
        private ServiceHost _hpcrProxy;

        public HpcrSimulationProxyService()
        {
            ServiceName = "HpcrSimulationProxyService";
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            Logger.LogDebug("Starting HPCR Simulation Proxy Service");

            Binding binding = BindingFactory.CreateBinding(MessageTransferType.CompressedHttp);

            _hpcrProxy = new ServiceHost(typeof(HpcrSimulationProxy), HpcrProxyUri.LocalHost);
            _hpcrProxy.AddServiceEndpoint(typeof(IHpcrConfigurationProxyService).FullName, binding, string.Empty);
            _hpcrProxy.AddServiceEndpoint(typeof(IHpcrExecutionProxyService).FullName, binding, string.Empty);
            _hpcrProxy.Open();
        }

        protected override void OnStop()
        {
            base.OnStop();

            if (_hpcrProxy.State != CommunicationState.Closed)
            {
                _hpcrProxy.Close();
            }
        }
    }
}
