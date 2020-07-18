using System.Collections.ObjectModel;
using System.ServiceModel;

namespace HP.ScalableTest.Service.EPrintJobMonitor
{
    [ServiceContract]
    public interface IEPrintJobMonitorService
    {
        [OperationContract]
        void NotifySessionFinished(string sessionId);
        
        [OperationContract]
        void ClearSession(string sessionId);

        [OperationContract]
        void ClearAll();
    }
}
