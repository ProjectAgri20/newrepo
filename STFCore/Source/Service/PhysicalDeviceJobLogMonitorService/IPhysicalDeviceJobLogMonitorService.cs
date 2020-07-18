using System;
using System.ServiceModel;
namespace HP.ScalableTest.Service.PhysicalDeviceJobLogMonitor
{
    [ServiceContract]
    public interface IPhysicalDeviceJobLogMonitorService
    {
        [OperationContract]
        void MonitorDeviceForDocumentIds(
            string sessionId, Guid activityId, Guid transactionId, 
            string deviceId, string deviceIpAddress, 
            System.Collections.Generic.List<string> documentIdentifiers, 
            string role, string password, int minutesToMonitorBeforeTimeout = 30);

        [OperationContract]
        string Ping();
    }
}
