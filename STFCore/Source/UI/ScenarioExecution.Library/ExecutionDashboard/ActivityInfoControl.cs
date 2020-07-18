using System.Collections.Generic;
using System.Linq;
using HP.ScalableTest.Core.DataLog;
using HP.ScalableTest.Framework.Dispatcher;
using HP.ScalableTest.Utility;

namespace HP.ScalableTest.UI.SessionExecution
{
    [ObjectFactory(ElementType.Activity)]
    public partial class ActivityInfoControl : WorkerActivityLogInfoControl
    {
        private string _sessionId = null;
        private string _activityName = null;

        public override void Initialize(SessionMapElement element, SessionInfo sessionInfo)
        {
            _sessionId = sessionInfo.SessionId;
            _activityName = element.Name;
            base.Initialize(element, sessionInfo);
        }

        protected override List<SessionActivityData> GetActivityExecutionData(DataLogContext context)
        {
            return context.SessionData(_sessionId).Activities.Where(n => n.ActivityName == _activityName).ToList();
        }
    }
}
