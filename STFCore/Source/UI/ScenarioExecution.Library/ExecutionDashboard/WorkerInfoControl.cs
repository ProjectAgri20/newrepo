using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.SessionExecution
{
    [ObjectFactory(ElementType.Worker)]
    public partial class WorkerInfoControl : WorkerActivityLogInfoControl
    {
        string _sessionId = null;
        string _instanceId = null;

        public override void Initialize(SessionMapElement element, SessionInfo sessionInfo)
        {
            _sessionId = sessionInfo.SessionId;
            _instanceId = element.Name;
            base.Initialize(element, sessionInfo);
        }

        protected override List<SessionActivityData> GetActivityExecutionData(DataLogContext context)
        {
            return context.SessionData(_sessionId).Activities.Where(n => n.ResourceInstanceId == _instanceId).ToList();
        }
    }
}
