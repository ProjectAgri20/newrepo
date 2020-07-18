using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.SessionExecution
{
    [ObjectFactory(ElementType.Machine)]
    public partial class MachineInfoControl : WorkerActivityLogInfoControl
    {
        string _sessionId = null;
        string _hostName = null;

        public override void Initialize(SessionMapElement element, SessionInfo sessionInfo)
        {
            _sessionId = sessionInfo.SessionId;
            _hostName = element.Name;

            base.Initialize(element, sessionInfo);
        }

        protected override List<SessionActivityData> GetActivityExecutionData(DataLogContext context)
        {
            return context.SessionData(_sessionId).Activities.Where(n => n.HostName == _hostName).ToList();
        }
    }
}
