using System;
using HP.ScalableTest.Framework;
using HP.ScalableTest.Framework.Settings;
using HP.ScalableTest.Framework.Wcf;

namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    /// <summary>
    /// Client class for talking to PhysicalDeviceJobLogMonitorService. This class cannot be inherited.
    /// </summary>
    public sealed class PhysicalDeviceJobLogMonitorServiceClient : WcfClient<IPhysicalDeviceJobLogMonitorService>
    {
        private PhysicalDeviceJobLogMonitorServiceClient(Uri endpoint)
            : base(MessageTransferType.CompressedHttp, endpoint)
        {
        }

        public static PhysicalDeviceJobLogMonitorServiceClient Create(Uri endpoint)
        {
            HP.ScalableTest.TraceFactory.Logger.Debug("Creating PhysicalDeviceJobLogMonitorServiceClient to talk to " + endpoint.ToString());
            return new PhysicalDeviceJobLogMonitorServiceClient(endpoint);
        }

        public static PhysicalDeviceJobLogMonitorServiceClient Create()
        {
            if (IsDefined())
            {
                string serviceHostAddress = GlobalSettings.WcfHosts[WcfService.PhysicalDeviceJobLogMonitorService];
                //string env = GlobalSettings.Items[Setting.Environment];
                var endpoint = WcfService.PhysicalDeviceJobLogMonitorService.GetHttpUri(serviceHostAddress);
                return Create(endpoint);
            }
            else
            {
                return null;
            }
        }

        public static bool IsDefined()
        {
            return 
                GlobalSettings.WcfHosts.ContainsKey(WcfService.PhysicalDeviceJobLogMonitorService.ToString())
                && GlobalSettings.Items.ContainsKey(Setting.Environment);
        }
    }
}
