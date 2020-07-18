using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.SessionExecution
{
    [ObjectFactory(ElementType.Session)]
    public partial class SessionInfoControl : WorkerActivityLogInfoControl
    {
        SessionInfo _sessionInfo = null;

        public override void Initialize(SessionMapElement element, SessionInfo sessionInfo)
        {
            _sessionInfo = sessionInfo;
            base.Initialize(element, sessionInfo);
        }

        protected override List<SessionActivityData> GetActivityExecutionData(DataLogContext context)
        {
            return context.SessionData(_sessionInfo.SessionId).Activities.ToList();
        }

        public override string GetTitle()
        {
            return ("Session{0}".FormatWith((_sessionInfo == null ? string.Empty : " " + _sessionInfo.SessionId)));
        }
    }
}
