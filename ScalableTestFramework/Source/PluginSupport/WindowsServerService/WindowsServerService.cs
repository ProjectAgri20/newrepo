using System.ServiceModel;
using System.ServiceProcess;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Wcf;
using HP.ScalableTest.PluginSupport.Connectivity.Dhcp;
using HP.ScalableTest.PluginSupport.Connectivity.DnsApp;
using HP.ScalableTest.PluginSupport.Connectivity.KiwiSyslog;
using HP.ScalableTest.PluginSupport.Connectivity.RadiusServer;
using HP.ScalableTest.PluginSupport.Connectivity.SystemConfiguration;
using HP.ScalableTest.PluginSupport.Connectivity.Wins;

namespace HP.ScalableTest.PluginSupport.WindowsServerService
{
    internal class WindowsServerService : ServiceBase
    {
        public ServiceHost _dhcpServiceHost = null;
        public ServiceHost _dnsServiceHost = null;
        public ServiceHost _winsServiceHost = null;
        public ServiceHost _kiwiSyslogServiceHost = null;
        public ServiceHost _radiusServiceHost = null;
        public ServiceHost _systemConfigurationServiceHost = null;

        /// <summary>
        /// Initializes a new instance of the <see cref="PacketCaptureService"/> class.
        /// </summary>
        public WindowsServerService()
        {
            ServiceName = "WindowsServerService";
        }

        protected override void OnStart(string[] args)
        {
            base.OnStart(args);

            Logger.LogDebug("Starting Windows Server Service");

            // Create service host for Service Contract
            _dhcpServiceHost = new WcfHost<IDhcpApplication>(typeof(DhcpApplication), MessageTransferType.Http, DhcpApplicationServiceClient.BuildUri("localhost"));
            _dhcpServiceHost.Open();

            _dnsServiceHost = new WcfHost<IDnsApplication>(typeof(DnsApplication), MessageTransferType.Http, DnsApplicationServiceClient.BuildUri("localhost"));
            _dnsServiceHost.Open();

            _winsServiceHost = new WcfHost<IWinsApplication>(typeof(WinsApplication), MessageTransferType.Http, WinsApplicationServiceClient.BuildUri("localhost"));
            _winsServiceHost.Open();

            _kiwiSyslogServiceHost = new WcfHost<IKiwiSyslogApplication>(typeof(KiwiSyslogApplication), MessageTransferType.Http, KiwiSyslogApplicationServiceClient.BuildUri("localhost"));
            _kiwiSyslogServiceHost.Open();

            _radiusServiceHost = new WcfHost<IRadiusApplication>(typeof(RadiusApplication), MessageTransferType.Http, RadiusApplicationServiceClient.BuildUri("localhost"));
            _radiusServiceHost.Open();

            _systemConfigurationServiceHost = new WcfHost<ISystemConfiguration>(typeof(SystemConfiguration), MessageTransferType.Http, SystemConfigurationClient.BuildUri("localhost"));
            _systemConfigurationServiceHost.Open();
        }

        protected override void OnStop()
        {
            base.OnStop();

            Logger.LogDebug("Stopping Windows Server Service");

            if (_dhcpServiceHost.State != CommunicationState.Closed)
            {
                _dhcpServiceHost.Close();
            }

            if (_dnsServiceHost.State != CommunicationState.Closed)
            {
                _dnsServiceHost.Close();
            }

            if (_winsServiceHost.State != CommunicationState.Closed)
            {
                _winsServiceHost.Close();
            }

            if (_kiwiSyslogServiceHost.State != CommunicationState.Closed)
            {
                _kiwiSyslogServiceHost.Close();
            }
        }
    }
}
